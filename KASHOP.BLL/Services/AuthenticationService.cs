using KASHOP.BLL.Common;
using KASHOP.DAL.DTO;
using KASHOP.DAL.Models;
using Mapster;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public AuthenticationService(UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        public async Task<bool> ConfirnmEmail(ConfirmEmailRequest request)
        {
            var user= await _userManager.FindByIdAsync(request.UserId);
            if(user == null) return false;
            var result = await _userManager.ConfirmEmailAsync(user, request.Token);
            if (!result.Succeeded) return false;
            return true;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var user= await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                return new LoginResponse()
                {
                    Message= "Invalid Email"
                }; 
            }
            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                return new LoginResponse()
                {
                    Message = "Email not confirmed"
                };
            }
            var result = await _userManager.CheckPasswordAsync(user, request.Password);
            if(!result)
            {
                return new LoginResponse()
                {
                    Message = "Invalid Password"
                };
            }
            return new LoginResponse()
            {
                Message = "Login Successful"
            };

        }

        public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
        {
            var user= request.Adapt<ApplicationUser>();
            var result = await _userManager.CreateAsync(user, request.Password);
            foreach (var item in result.Errors)
            {
                Console.WriteLine(item.Description);
            }
            if (!result.Succeeded)
            {
                return new RegisterResponse 
                { 
                    Message = "Error",
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };
            }
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            token= Uri.EscapeDataString(token);
            var emailURL= $"https://localhost:7259/api/account/ConfirmEmail?token={token}&userId={user.Id}";
            await _emailSender.SendEmailAsync(user.Email, "Confirm Email", $"<h1>Thank you for registering with KASHOP!</h1>"+
                "<p>Please click the link below to confirm your email:</p>"+
                $"<a href='{emailURL}'>Confirm Email</a>");

            return new RegisterResponse { Message = "Success" };
        }
    }
}
