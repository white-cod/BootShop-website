using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BootShop.Data;
using BootShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BootShop.Pages.Account
{
    [Authorize]
    public class ProfileModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;

        public ProfileModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [BindProperty]
        public User UserProfile { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userIdString = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdString == null)
            {
                return RedirectToPage("/Account/Login");
            }

            UserProfile = await _dbContext.Users.FindAsync(Convert.ToInt32(userIdString));
            if (UserProfile == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {


            var userIdString = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdString == null)
            {
                return RedirectToPage("/Account/Login");
            }

            var userToUpdate = await _dbContext.Users.FindAsync(Convert.ToInt32(userIdString));
            if (userToUpdate == null)
            {
                return NotFound();
            }

            userToUpdate.FullName = UserProfile.FullName;
            userToUpdate.PhoneNumber = UserProfile.PhoneNumber;


            try
            {
                await _dbContext.SaveChangesAsync();
                Console.WriteLine("User profile updated successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating user profile.");
                throw;
            }

            return RedirectToPage();
        }
    }
}