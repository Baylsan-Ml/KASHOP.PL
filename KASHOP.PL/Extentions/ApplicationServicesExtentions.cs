using KASHOP.BLL.Common;
using KASHOP.BLL.Services.Classes;
using KASHOP.BLL.Services.Interfaces;
using KASHOP.DAL.Repository;
using KASHOP.PL.Utils;

namespace KASHOP.PL.Extentions
{
    public static class ApplicationServicesExtentions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection Services)
        {
            Services.AddScoped<IUnitOfWork, UnitOfWork>();
            Services.AddExceptionHandler<GlobalExeptionHandler>();
            Services.AddScoped<ICategoryService, CategoryService>();
            Services.AddScoped<IProductService, ProductService>();
            Services.AddScoped<IFileService, FileService>();
            Services.AddScoped<IAuthenticationService, AuthenticationService>();
            Services.AddScoped<IISeedData, RoleSeedData>();
            Services.AddTransient<IEmailSender, EmailSender>();
            Services.AddHttpContextAccessor();

            Services.AddProblemDetails();
            return Services;
        }
    }
}
