using KASHOP.BLL.Common;
using KASHOP.BLL.Services.Classes;
using KASHOP.BLL.Services.Interfaces;
using KASHOP.DAL.Repository;
using KASHOP.PL.Utils;
using Stripe;
using CheckoutService = KASHOP.BLL.Services.Classes.CheckoutService;
using FileService = KASHOP.BLL.Services.Classes.FileService;
using ProductService = KASHOP.BLL.Services.Classes.ProductService;

namespace KASHOP.PL.Extentions
{
    public static class ApplicationServicesExtentions
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection Services, IConfiguration Configuration)
        {
            Services.AddScoped<IUnitOfWork, UnitOfWork>();
            Services.AddExceptionHandler<GlobalExeptionHandler>();
            Services.AddScoped<ICategoryService, CategoryService>();
            Services.AddScoped<IProductService, ProductService>();
            Services.AddScoped<ICartService, CartService>();
            Services.AddScoped<ICheckoutService, CheckoutService>();
            Services.AddScoped<IFileService, FileService>();
            Services.AddScoped<IAuthenticationService, AuthenticationService>();
            Services.AddScoped<IISeedData, RoleSeedData>();
            Services.AddTransient<IEmailSender, EmailSender>();
            Services.AddHttpContextAccessor();

            Services.Configure<StripeSettings>(Configuration.GetSection("Stripe"));
            StripeConfiguration.ApiKey = Configuration["Stripe:SecretKey"];

            Services.AddProblemDetails();
            return Services;
        }
    }
}
