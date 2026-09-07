using KASHOP.BLL.Common;
using KASHOP.BLL.Services.Interfaces;
using KASHOP.DAL.DTO;
using KASHOP.DAL.Models;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Services.Classes
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _config;

        public AuthenticationService(UserManager<ApplicationUser> userManager, IEmailSender emailSender,
            IConfiguration config)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _config = config;
        }

        

        public async Task<Result<bool>> RegisterAsync(RegisterRequest request)
        {
            try
            {
                var user = request.Adapt<ApplicationUser>();
                var result = await _userManager.CreateAsync(user, request.Password);
                foreach (var item in result.Errors)
                {
                    Console.WriteLine(item.Description);
                }
                if (!result.Succeeded)
                {
                    return new Result<bool>
                    {
                        Success = false,
                        Message = "Failed to Register User",
                        Data = false,
                        Errors = result.Errors.Select(e => e.Description).ToList()
                    };
                }
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                token = Uri.EscapeDataString(token);
                var emailURL = $"https://localhost:7259/api/account/ConfirmEmail?token={token}&userId={user.Id}";
                await _emailSender.SendEmailAsync(user.Email, "Confirm Email", $"<h1>Thank you for registering with KASHOP!</h1>" +
                    "<p>Please click the link below to confirm your email:</p>" +
                    $"<a href='{emailURL}'>Confirm Email</a>");

                return new Result<bool>
                {
                    Success = true,
                    Message = "Register Successded",
                    Data = true,
                };
            }
            catch (Exception ex) 
            {
                return new Result<bool>
                {
                    Success = false,
                    Message = ex.InnerException.Message,
                    Data = false,
                };
            }

        }
        public async Task<Result<bool>> ConfirnmEmail(ConfirmEmailRequest request)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(request.UserId);
                if (user == null)
                {
                    return new Result<bool>
                    {
                        Success = false,
                        Message = "Failed to Register User",
                        Data = false,
                    };
                }
                request.Token= Uri.UnescapeDataString(request.Token);
                var result = await _userManager.ConfirmEmailAsync(user, request.Token);

                return new Result<bool>
                {
                    Success = result.Succeeded,
                    Message = result.Succeeded? "Success" : "Failed To Confirm Email",
                    Data = result.Succeeded
                }; 
            }catch (Exception ex) 
            {
                return new Result<bool>
                {
                    Success = false,
                    Message = ex.InnerException.Message,
                    Data = false,
                };
            }
        }

        public async Task<Result<LoginResponse>> LoginAsync(LoginRequest request)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user is null)
                {
                    return new Result<LoginResponse>
                    {
                        Success = false,
                        Message = "Invalid Email",
                    };
                }
                if (!await _userManager.IsEmailConfirmedAsync(user))
                {
                    return new Result<LoginResponse>
                    {
                        Success = false,
                        Message = "Email not confirmed",
                    };
                }
                var result = await _userManager.CheckPasswordAsync(user, request.Password);
                if (!result)
                {
                    return new Result<LoginResponse>
                    {
                        Success = false,
                        Message = "Invalid Password",
                        Data = null
                    };
                }
                return new Result<LoginResponse>
                {
                    Success = true,
                    Message = "Login Successful",
                    Data = new LoginResponse
                    {
                        AccessToken = await GenerateJWT(user)
                    },
                };
            }
            catch (Exception ex)
            {
                return new Result<LoginResponse>
                {
                    Success = false,
                    Message = ex.InnerException.Message,
                    Data = null,
                };
            }
        }

        private async Task<string> GenerateJWT(ApplicationUser user)
        {
            var roles= await _userManager.GetRolesAsync(user);
            var userClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role,string.Join(",", roles))
            };
            // Continue with JWT generation logic
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["ApiSettings:SecretKey"]));

            var creds = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
            issuer: _config["ApiSettings:issuer"],
            audience: _config["ApiSettings:audience"],
            claims: userClaims,
            expires: DateTime.UtcNow.AddDays(20),
            signingCredentials: creds
        );
             
            Console.WriteLine("Token generated successfully.");
            return new JwtSecurityTokenHandler().WriteToken(token);
    }
    }
}
