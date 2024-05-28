using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BootShop.Data;
using BootShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BootShop.Pages
{
    [Authorize]
    public class CartModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;

        public CartModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<ShoppingCart> CartItems { get; set; }
        public decimal CartTotal { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userIdString = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdString == null)
            {
                return RedirectToPage("/Account/Login");
            }

            var userId = Convert.ToInt32(userIdString);
            CartItems = await _dbContext.ShoppingCart
                .Include(sc => sc.Shoes)
                .Where(sc => sc.UserId == userId)
                .ToListAsync();

            CartTotal = CartItems.Sum(item => item.Quantity * item.Shoes.price);

            return Page();
        }

        public async Task<IActionResult> OnPostRemoveAsync(int id)
        {
            var cartItem = await _dbContext.ShoppingCart.FindAsync(id);
            if (cartItem != null)
            {
                _dbContext.ShoppingCart.Remove(cartItem);
                await _dbContext.SaveChangesAsync();
            }

            return RedirectToPage();
        }
    }
}