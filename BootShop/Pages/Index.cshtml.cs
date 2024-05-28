using BootShop.Data;
using BootShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BootShop.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;
        private const int PageSize = 21;

        public IndexModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            PagedShoes = new List<Shoes>();
        }

        public List<Shoes> PagedShoes { get; set; }
        public int MinPrice { get; set; }
        public int MaxPrice { get; set; }
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public IList<string> Brands { get; set; }
        public IList<string> Seasons { get; set; }
        public IList<string> Colors { get; set; }
        public IList<string> Genders { get; set; }
        public IList<string> Countries { get; set; }

        [BindProperty(SupportsGet = true)]
        public List<string> SelectedBrands { get; set; }
        [BindProperty(SupportsGet = true)]
        public List<string> SelectedSeasons { get; set; }
        [BindProperty(SupportsGet = true)]
        public List<string> SelectedColors { get; set; }
        [BindProperty(SupportsGet = true)]
        public List<string> SelectedGenders { get; set; }
        [BindProperty(SupportsGet = true)]
        public List<string> SelectedCountries { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? SelectedPriceRange { get; set; }

        public bool HasPreviousPage => (PageIndex > 1);
        public bool HasNextPage => (PageIndex < TotalPages);

        public async Task OnGetAsync(int? pageIndex)
        {
            Brands = await _dbContext.Shoes
                .Select(s => s.brand_name)
                .Distinct()
                .ToListAsync();

            Seasons = new List<string> { "Лето", "Весна", "Осень", "Зима" };

            Colors = await _dbContext.Shoes
                .Select(s => s.color)
                .Distinct()
                .ToListAsync();

            Genders = await _dbContext.Shoes
                .Select(s => s.gender)
                .Distinct()
                .ToListAsync();

            Countries = await _dbContext.Shoes
                .Select(s => s.country)
                .Distinct()
                .ToListAsync();

            MinPrice = await _dbContext.Shoes.MinAsync(s => s.price);
            MaxPrice = await _dbContext.Shoes.MaxAsync(s => s.price);

            var shoes = await GetShoes();

            int pageSize = PageSize;
            int pageNumber = pageIndex ?? 1;
            PagedShoes = shoes.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            PageIndex = pageIndex ?? 1;
            TotalPages = (int)System.Math.Ceiling(shoes.Count / (double)PageSize);
        }

        private async Task<List<Shoes>> GetShoes()
        {
            var query = _dbContext.Shoes.AsQueryable();

            if (SelectedBrands != null && SelectedBrands.Count > 0)
            {
                query = query.Where(s => SelectedBrands.Contains(s.brand_name));
            }

            if (SelectedSeasons != null && SelectedSeasons.Count > 0)
            {
                query = query.Where(s => SelectedSeasons.Any(season => s.season.Contains(season)));
            }

            if (SelectedColors != null && SelectedColors.Count > 0)
            {
                query = query.Where(s => SelectedColors.Contains(s.color));
            }

            if (SelectedGenders != null && SelectedGenders.Count > 0)
            {
                query = query.Where(s => SelectedGenders.Contains(s.gender));
            }

            if (SelectedCountries != null && SelectedCountries.Count > 0)
            {
                query = query.Where(s => SelectedCountries.Contains(s.country));
            }

            if (SelectedPriceRange.HasValue)
            {
                query = query.Where(shoe => shoe.price <= SelectedPriceRange.Value);
            }

            return await query.ToListAsync();
        }
    }
}