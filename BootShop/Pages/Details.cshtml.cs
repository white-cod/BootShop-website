using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BootShop.Data;
using BootShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BootShop.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;

        public DetailsModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Shoes Shoe { get; set; }
        public List<string> ImageUrls { get; set; } = new List<string>();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Shoe = await _dbContext.Shoes.FirstOrDefaultAsync(m => m.Id == id);

            if (Shoe == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(Shoe.image_url1)) ImageUrls.Add(Shoe.image_url1);
            if (!string.IsNullOrEmpty(Shoe.image_url2)) ImageUrls.Add(Shoe.image_url2);
            if (!string.IsNullOrEmpty(Shoe.image_url3)) ImageUrls.Add(Shoe.image_url3);
            if (!string.IsNullOrEmpty(Shoe.image_url4)) ImageUrls.Add(Shoe.image_url4);
            if (!string.IsNullOrEmpty(Shoe.image_url5)) ImageUrls.Add(Shoe.image_url5);
            if (!string.IsNullOrEmpty(Shoe.image_url6)) ImageUrls.Add(Shoe.image_url6);
            if (!string.IsNullOrEmpty(Shoe.image_url7)) ImageUrls.Add(Shoe.image_url7);

            return Page();
        }

        public async Task<IActionResult> OnPostAddToCartAsync(int id)
        {
            var userIdString = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdString == null)
            {
                return RedirectToPage("/Account/Login");
            }

            var userId = Convert.ToInt32(userIdString);

            var shoeExists = await _dbContext.Shoes.AnyAsync(s => s.Id == id);
            if (!shoeExists)
            {
                return NotFound("The product you are trying to add does not exist.");
            }

            var cartItem = await _dbContext.ShoppingCart
                .FirstOrDefaultAsync(sc => sc.UserId == userId && sc.ProductId == id);

            if (cartItem == null)
            {
                cartItem = new ShoppingCart
                {
                    UserId = userId,
                    ProductId = id,
                    Quantity = 1
                };
                _dbContext.ShoppingCart.Add(cartItem);
            }
            else
            {
                cartItem.Quantity++;
            }

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding item to cart: {ex.Message}");
                return RedirectToPage("/Error");
            }

            return RedirectToPage("/Cart");
        }
    }
}