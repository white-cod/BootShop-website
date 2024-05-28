using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BootShop.Data;
using BootShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BootShop.Pages
{
    [Authorize]
    public class AddProductModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;

        public AddProductModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [BindProperty]
        public Shoes NewShoe { get; set; }

        public string ErrorMessage { get; set; }

        public IActionResult OnGet()
        {
            var userIdString = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdString == null)
            {
                return RedirectToPage("/Account/Login");
            }

            var user = _dbContext.Users.FirstOrDefault(u => u.Id == Convert.ToInt32(userIdString));
            if (user == null || !user.IsAdmin)
            {
                ErrorMessage = "You do not have permission to add products.";
                return Page();
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

            var user = _dbContext.Users.FirstOrDefault(u => u.Id == Convert.ToInt32(userIdString));
            if (user == null || !user.IsAdmin)
            {
                ErrorMessage = "You do not have permission to add products.";
                return Page();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _dbContext.Shoes.Add(NewShoe);
            await _dbContext.SaveChangesAsync();

            return RedirectToPage("/Index");
        }
    }
}