using MeroBlog.Data;
using MeroBlog.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MeroBlog.Utilities
{
    public class DbInitilize : IDbInitilizer
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public DbInitilize(ApplicationDbContext context, UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task InitilizeAsync()
        {
            if (!_roleManager.RoleExistsAsync(WebSitesRole.WebsiteAdmin).GetAwaiter().GetResult())
            {
                await _roleManager.CreateAsync(new IdentityRole(WebSitesRole.WebsiteAdmin));
                await _roleManager.CreateAsync(new IdentityRole(WebSitesRole.WebsiteAuthor));

          
                var user = new ApplicationUser
                {
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",
                    FirstName = "Super",
                    LastName = "Admin"
                };
                var result = await _userManager.CreateAsync(user, "Admin@0011");
                if (result.Succeeded)
                {
                    var appUser = await _userManager.FindByEmailAsync(user.Email);
                    if (appUser != null)
                    {
                        await _userManager.AddToRoleAsync(appUser, WebSitesRole.WebsiteAdmin);
                        await _userManager.AddToRoleAsync(appUser, WebSitesRole.WebsiteAuthor);
                    }
                }

                var listOfPages = new List<Page> {
                    new Page() { Title = "About", Description = "About Page", Slug = "about" },
                    new Page() { Title = "Contact", Description = "Contact Page", Slug = "contact" },
                    new Page() { Title = "Privacy", Description = "Privacy Page", Slug = "privacy" }
                };
                

                await _context.Pages.AddRangeAsync(listOfPages);
                await _context.SaveChangesAsync();
            }
        }


    }
}
