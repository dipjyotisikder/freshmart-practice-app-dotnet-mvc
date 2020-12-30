using System;
using System.Linq;
using System.Threading.Tasks;
using FreshMart.Core.Utilities;
using FreshMart.Database;
using FreshMart.Helper;
using FreshMart.Models;
using FreshMart.Models.Queries;
using FreshMart.Services;
using FreshMart.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FreshMart.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AppDbContext _context;
        //private int cartCount;
        private IHostingEnvironment environment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ICartService _cartService;
        private IProductService _productService;


        private readonly IMediator _mediator;

        public ProductsController(AppDbContext con,
            IHostingEnvironment appEnvironment,
            IHttpContextAccessor httpContextAccessor,
            ICartService cartService,
            IProductService productService,
            IMediator mediator)
        {
            _context = con;
            environment = appEnvironment;
            _httpContextAccessor = httpContextAccessor;
            _cartService = cartService;
            _productService = productService;
            _mediator = mediator;
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }


        [Route("Products/Index/{SearchVM?}")]
        public async Task<ViewResult> Index(GetAllProductQuery query)
        {
            var result = await _mediator.Send(query);
            return View(result);
        }



        [HttpGet]
        [Route("products/index/{id}")]
        public async Task<IActionResult> Index(long id)
        {
            var productView = await _productService.GetProductViewModelWithCartCountAsync(id);
            return View(productView);
        }



        [HttpGet]
        [Route("products/category/{id}")]
        public async Task<IActionResult> Category(long id)
        {
            var cartCount = _cartService.GetCartCount();

            var parent = await _productService.GetParentCategoryAsync(id);

            var products = _context.Products
                .Include(c => c.Category)
                .Include(c => c.District)
                .Where(c => c.Category.ParentId == parent.Id).AsNoTracking()
                .ToList();

            var pro = _productService.GetAllProducts();
            var categories = await _productService.GetAllCategoriesAsync();
            var districts = await _productService.GetAllDistrictsAsync();
            var parentCategories = _productService.GetParentCategoryNames();

            var productView = new ProductViewModel
            {
                Products = products,
                Category = categories,
                District = districts,
                DistinctCat = parentCategories,
                BaseProduct = pro, //it will always remain same as it is inherited
                CartCount = cartCount,
                TotalPrice = _cartService.GetCartTotalPrice(),
                Sellers = _context.Sellers.ToList()
            };
            return View("Index", productView);
        }



        [Authorize]
        [HttpGet]
        [Route("Products/addproduct")]
        public IActionResult AddProduct()
        {

            var viewmodel = _productService.GetProductViewModelAsync();
            return View(viewmodel);
        }



        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddProduct(IFormFile file, ProductViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var Seller = _context.Sellers.Where(s => s.Email.Contains(User.Identity.Name));
                if (Seller.SingleOrDefault() == null)
                {
                    return RedirectToAction("RequestForSell", "Products");
                }


                ImgUploader img = new ImgUploader(environment);
                var imgPath = img.ImageUrl(file);   //function working here

                var products = new Product
                {
                    Id = NumberUtilities.GetUniqueNumber(),
                    Title = vm.Product.Title,

                    Description = vm.Product.Description,
                    Price = vm.Product.Price,
                    SellerId = 1,  //manually value. Next time it will be from Session User Value
                    CategoryId = vm.Product.CategoryId,
                    DistrictId = vm.Product.DistrictId,
                    IsPublished = vm.Product.IsPublished,  //manually valuess
                    Unit = vm.Product.Unit,
                    ItemInStock = vm.Product.ItemInStock,
                    UpdatedAt = DateTime.Now,//manually valuess
                    OfferExpireDate = DateTime.Now,//manually valuess
                    ImagePath = imgPath,
                    OfferPrice = vm.Product.OfferPrice,

                };

                await _context.AddAsync(products);
                await _context.SaveChangesAsync();

                long id = products.Id;


            }
            else
            {
                var viewmodel = _productService.GetProductViewModelAsync();

                ViewBag.error = "You Cannot ignore required fields";
                return View("AddProduct", viewmodel);
            }


            return RedirectToAction("AddProduct", "Products");

        }

        [Authorize]
        [HttpGet]
        [Route("Products/RequestForSell/{msg?}")]
        public ActionResult RequestForSell(bool? msg)
        {

            if (msg == true)
            {
                ViewBag.addedreq = "You have successfully requested to be a seller";
            }

            var status = _context.SellerRequests.Where(s => s.Email.Contains(User.Identity.Name));
            if (status.SingleOrDefault() != null)
            {
                return RedirectToAction("Index", "Home", new { msg = true });
            }

            var categories = _context.Categories.ToList();
            var districts = _context.Districts.ToList();
            var domains = _context.Categories.Where(x => x.ParentId != null)
                .Include(x => x.Parent)
                .Select(c => c.Parent.Name).AsEnumerable();

            CartService cs = new CartService(_httpContextAccessor, _context);
            var totalPrice = cs.GetCartTotalPrice();
            var cartCount = cs.GetCartCount();
            var viewmodel = new ProductViewModel
            {
                District = districts,
                Category = categories,
                DistinctCat = domains,
                CartCount = cartCount,
                TotalPrice = totalPrice,
                SellerRequest = new SellerRequest { Email = User.Identity.Name }

            };
            return View(viewmodel);
        }



        public ActionResult LayoutPartial()
        {

            var categories = _context.Categories.ToList();

            return PartialView(categories);
        }




    }
}