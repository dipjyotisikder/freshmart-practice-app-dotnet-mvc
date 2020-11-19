using FreshMart.Database;
using FreshMart.Models;
using FreshMart.Models.Commands;
using FreshMart.Models.Queries;
using FreshMart.Models.ViewModels;
using MediatR;
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
        private readonly ApplicationDbContext _context;

        public ProductQueryHandler(IProductService productService,
            ICartService cartService,
            ApplicationDbContext context)
        {
            _productService = productService;
            _cartService = cartService;
            _context = context;
        }


        public async Task<ProductViewModel> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
        {
            var cartCount = _cartService.GetCartCount();
            Torce torce = new Torce(_context);

            List<Product> products = new List<Product>();

            //            000
            if (request.Text != null && request.PriceRange != null && request.DistrictId != null)
            {
                products = torce.SearchProducts(request.Text, request.PriceRange, request.DistrictId);
            }

            //            001
            if (request.Text != null && request.PriceRange != null && request.DistrictId == null)
            {

                products = torce.SearchProducts(request.Text, request.PriceRange);
            }
            //            010
            if (request.Text != null && request.PriceRange == null && request.DistrictId != null)
            {
                products = torce.SearchProducts(request.Text, request.DistrictId);
            }

            //            011
            if (request.Text != null && request.PriceRange == null && request.DistrictId == null)
            {
                products = torce.SearchProducts(request.Text);
            }

            //            100
            if (request.Text == null && request.PriceRange != null && request.DistrictId != null)
            {

                products = torce.SearchProducts2(request.PriceRange, request.DistrictId);
            }


            //            101
            if (request.Text == null && request.PriceRange != null && request.DistrictId == null)
            {

                products = torce.SearchProducts2(request.PriceRange);
            }
            //            110
            if (request.Text == null && request.PriceRange == null && request.DistrictId != null)
            {

                products = torce.SearchProducts3(request.DistrictId);
            }

            if (request.Text == null && request.PriceRange == null && request.DistrictId == null)
            {
                products = torce.SearchProducts();
            }

            var categories = _productService.GetAllCategories();
            var districts = _productService.GetAllDistricts();
            var domains = _productService.GetCategoryByDomain();
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
                Sellers = _context.Sellers.ToList()
            };

            return await Task.FromResult(productView);
        }
    }
}
