using KASHOP.DAL.Models;
using Microsoft.AspNetCore.Identity;

namespace KASHOP.PL.Utils
{
    public class UserSeedData : IISeedData
    {

        private readonly UserManager<ApplicationUser> _userManager;

        public UserSeedData(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task DataSeed()
        {
            await CreateUserAsync("superadmin@kashop.com", "superadmin","super admin", "Super@1122", "SuperAdmin");
            await CreateUserAsync("admin@kashop.com", "admin", "admin", "Admin@1122", "Admin");
            await CreateUserAsync("user@kashop.com", "user", "regular user", "User@1122", "User");
        }


        private async Task CreateUserAsync(string email, string userName, string fullName, string password, string role)
        {
            if (await _userManager.FindByEmailAsync(email) is not null || await _userManager.FindByNameAsync(userName) is not null) return;

            var user = new ApplicationUser
            {
                Email = email,
                UserName = userName,
                FullName = fullName,
                PhoneNumber = "0598777777",
                City="test",
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, password);
            await _userManager.AddToRoleAsync(user, role);
            
        }
    }
}
