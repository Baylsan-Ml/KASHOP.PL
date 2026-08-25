using KASHOP.BLL.Common;
using KASHOP.BLL.Services;
using KASHOP.DAL.Repository;
using KASHOP.PL.Utils;

namespace KASHOP.PL.Extentions
{
    public static class ApplicationServicesExtentions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection Services)
        {
            Services.AddScoped<ICategoryRepository, CategoryRepository>();
            Services.AddScoped<ICategoryService, CategoryService>();
            Services.AddScoped<IAuthenticationService, AuthenticationService>();
            Services.AddScoped<IISeedData, RoleSeedData>();
            Services.AddTransient<IEmailSender, EmailSender>();
            Services.AddHttpContextAccessor();
            return Services;
        }
    }
}
