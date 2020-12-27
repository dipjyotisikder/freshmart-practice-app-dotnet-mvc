using FreshMart.Database;
using FreshMart.Models;
using FreshMart.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

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


        public ProductViewModel GetProductViewModelWithCartCount(long id)
        {
            var products = GetProductsByCategoryId(id);
            var all = GetAllProducts();
            var categories = GetAllCategories();
            var districts = GetAllDistricts();
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


        public List<District> GetAllDistricts()
        {
            return _context.Districts.AsNoTracking().ToList();
        }


        //Category
        public List<Category> GetAllCategories()
        {
            return _context.Categories.Include(x => x.Parent).AsNoTracking().ToList();
        }


        public List<string> GetParentCategoryNames()
        {
            var distinctParents = _context.Categories.AsNoTracking().Select(c => c.ParentId).Distinct();

            return (from pcId in distinctParents
                    join c in _context.Categories.AsNoTracking()
                    on pcId equals c.Id
                    select c.Name).ToList();
        }


        public Category GetParentCategory(long categoryId)
        {
            return _context.Categories.Where(c => c.Id == categoryId).Select(c => c.Parent).FirstOrDefault();
        }


        public ProductViewModel GetProductViewModel()
        {
            CartService cs = new CartService(_httpContextAccessor, _context);
            var totalPrice = cs.GetCartTotalPrice();
            var viewmodel = new ProductViewModel
            {
                District = GetAllDistricts(),
                Category = GetAllCategories(),
                DistinctCat = GetParentCategoryNames(),
                CartCount = _cartService.GetCartCount(),
                TotalPrice = totalPrice,
                Sellers = _context.Sellers.AsNoTracking().ToList()
            };
            return viewmodel;

        }



    }
}
