using KASHOP.BLL.Common;
using KASHOP.BLL.Services;
using KASHOP.DAL.Repository;
using KASHOP.PL.Utils;

namespace KASHOP.PL.Extentions
{
    public static class ServicesExtentions
    {
        public static IServiceCollection AddServicesExtentions(this IServiceCollection Services, IConfiguration Configuration)
        {
            // Add services to the container.
            Services.AddExceptionHandler<GlobalExeptionHandler>();
            //
            Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            Services.AddOpenApi();
            //Database Extention Method to add database services and connection string
            Services.AddDatabaseServices(Configuration);
            //Localization Extention Method to add localization services and configuration
            Services.AddLocalizationServices();
            //Identitiy Extention Method to add identity services and configuration
            Services.AddIdentityServices();
            //JWT Extention Method to add jwt auth services and configuration
            Services.AddJwtAuthenticationServices(Configuration);
            //builder.Services.AddScoped<TokenService>();
            Services.AddApplicationServices();
            Services.AddProblemDetails();
            return Services;
        }
    }
}
