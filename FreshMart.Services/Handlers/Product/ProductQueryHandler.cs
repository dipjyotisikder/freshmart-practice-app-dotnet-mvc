using FreshMart.Database;
using FreshMart.Models;
using FreshMart.Models.Commands;
using FreshMart.Models.Queries;
using FreshMart.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FreshMart.Services.QueryHandler
{
    public class ProductQueryHandler : IRequestHandler<GetAllProductQuery, ProductViewModel>
    {
        private readonly IProductService _productService;
        private readonly ICartService _cartService;
        private readonly AppDbContext _context;

        public ProductQueryHandler(IProductService productService,
            ICartService cartService,
            AppDbContext context)
        {
            _productService = productService;
            _cartService = cartService;
            _context = context;
        }


        public async Task<ProductViewModel> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
        {
            var cartCount = _cartService.GetCartCount();
            Torce torce = new Torce(_context);

            var products = _context.Products
               .Include(c => c.Category)
               .Include(c => c.District)
               .Include(x => x.Photo)
               .AsEnumerable().Select(x =>
               {
                   if (x.Photo != null)
                   {
                       x.Photo.Path = "https://localhost:44318" + x.Photo.Path;
                   }

                   return x;
               }).ToList();

            var categories = await _productService.GetAllCategoriesAsync();

            var districts = await _productService.GetAllDistrictsAsync();

            var domains = _productService.GetParentCategoryNames();

            var totalPrice = _cartService.GetCartTotalPrice();

            var productView = new ProductViewModel
            {
                Products = products,
                Category = categories,
                District = districts,
                DistinctCat = domains,
                BaseProduct = products,  //it will always remain same as it is inherited
                CartCount = cartCount,
                TotalPrice = totalPrice,
                Sellers = await _context.Sellers.ToListAsync()
            };

            return await Task.FromResult(productView);
        }
    }
}
