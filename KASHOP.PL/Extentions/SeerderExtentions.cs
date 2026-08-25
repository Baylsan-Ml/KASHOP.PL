using KASHOP.PL.Utils;

namespace KASHOP.PL.Extentions
{
    public static class SeerderExtentions
    {
        public static async Task SeedDatabaseAsync(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var seeders = services.GetServices<IISeedData>();
                foreach (var seeder in seeders)
                {
                    seeder.DataSeed().Wait();
                }
            }
        }
    }
}
