using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace KASHOP.PL.Extentions
{
    public static class AuthenticationExtentions
    {
        public static IServiceCollection AddJwtAuthenticationServices(this IServiceCollection Services, IConfiguration Configuration)
        {
            Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
  .AddJwtBearer(options =>
  {
      options.TokenValidationParameters = new TokenValidationParameters
      {
          ValidateIssuer = true,
          ValidateAudience = true,
          ValidateLifetime = true,
          ValidateIssuerSigningKey = true,
          ValidIssuer = Configuration["ApiSettings:issuer"],
          ValidAudience = Configuration["ApiSettings:audience"],
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["ApiSettings:SecretKey"]))
      };
  });

            Services.AddAuthorization();

            return Services;
        }
    }
}
