using System.Collections.Generic;
using System.Threading.Tasks;
using BootShop.Data;
using BootShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;


namespace BootShop.Pages
{
    public class ThankYouModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;

        public ThankYouModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IList<User> RegisteredUsers { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            RegisteredUsers = await _dbContext.Users.ToListAsync();
            return Page();
        }
    }
}