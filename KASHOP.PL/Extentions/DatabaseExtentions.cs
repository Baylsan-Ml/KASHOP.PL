using Microsoft.EntityFrameworkCore;

namespace KASHOP.PL.Extentions
{
    public static class DatabaseExtentions
    {
        public static IServiceCollection AddDatabaseServices(this IServiceCollection Services, IConfiguration Configuration)
        {
            Services.AddDbContext<KASHOP.DAL.Data.ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            return Services;
        }
    }
}
