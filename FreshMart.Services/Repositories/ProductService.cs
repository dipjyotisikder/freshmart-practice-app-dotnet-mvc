using FreshMart.Database;
using FreshMart.Models;
using FreshMart.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreshMart.Services
{
    public class ProductService : IProductService
    {
        private readonly ICartService _cartService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _context;

        public ProductService(IHttpContextAccessor httpContextAccessor, AppDbContext context,
            ICartService cartService)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _cartService = cartService;
        }


        //Products
        public List<Product> GetAllProducts()
        {
            var products = _context.Products
                .Include(c => c.Category)
                .Include(c => c.District)
                .Include(p => p.Seller)
                .OrderByDescending(c => c.CreatedAt).AsNoTracking()
                .ToList();
            return products;
        }


        public List<Product> GetProductsByCategoryId(long id)
        {
            var pro = _context.Products
                .Include(c => c.Category)
                .Include(c => c.District)
                .Where(c => c.CategoryId == id)
                .OrderByDescending(c => c.CreatedAt).AsNoTracking()
                .ToList();

            return pro;
        }


        public async Task<ProductViewModel> GetProductViewModelWithCartCountAsync(long id)
        {
            var products = GetProductsByCategoryId(id);
            var all = GetAllProducts();
            var categories = await GetAllCategoriesAsync();
            var districts = await GetAllDistrictsAsync();
            var domains = categories.Where(x => x.ParentId != null).Select(x => x.Parent.Name);

            var productView = new ProductViewModel
            {
                Products = products,
                Category = categories,
                District = districts,
                DistinctCat = domains,
                BaseProduct = all,
                CartCount = _cartService.GetCartCount()
            };

            return productView;
        }


        public async Task<List<District>> GetAllDistrictsAsync()
        {
            return await _context.Districts.AsNoTracking().ToListAsync();
        }


        //Category
        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories.Include(x => x.Parent).AsNoTracking().ToListAsync();
        }


        public List<string> GetParentCategoryNames()
        {
            var distinctParents = _context.Categories.AsNoTracking().Select(c => c.ParentId).Distinct();

            return (from pcId in distinctParents
                    join c in _context.Categories.AsNoTracking()
                    on pcId equals c.Id
                    select c.Name).ToList();
        }


        public async Task<Category> GetParentCategoryAsync(long categoryId)
        {
            return await _context.Categories.Where(c => c.Id == categoryId).Select(c => c.Parent).FirstOrDefaultAsync();
        }


        public async Task<ProductViewModel> GetProductViewModelAsync()
        {
            CartService cs = new CartService(_httpContextAccessor, _context);
            var totalPrice = cs.GetCartTotalPrice();
            var viewmodel = new ProductViewModel
            {
                District = await GetAllDistrictsAsync(),
                Category = await GetAllCategoriesAsync(),
                DistinctCat = GetParentCategoryNames(),
                CartCount = _cartService.GetCartCount(),
                TotalPrice = totalPrice,
                Sellers = _context.Sellers.AsNoTracking().ToList()
            };
            return viewmodel;

        }



    }
}
