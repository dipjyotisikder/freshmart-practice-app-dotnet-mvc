using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FreshMart.Database;
using FreshMart.Helper;
using FreshMart.Models;
using FreshMart.Models.Queries;
using FreshMart.Models.ViewModels;
using FreshMart.Services;
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
        private List<Product> product;
        private readonly ApplicationDbContext _context;
        //private int cartCount;
        private IHostingEnvironment environment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private CartService cs;
        private ProductService ps;


        private readonly IMediator _mediator;

        public ProductsController(ApplicationDbContext con,
            IHostingEnvironment appEnvironment,
            IHttpContextAccessor httpContextAccessor,
            IMediator mediator)
        {
            _context = con;
            environment = appEnvironment;
            _httpContextAccessor = httpContextAccessor;
            cs = new CartService(httpContextAccessor, _context);
            ps = new ProductService(httpContextAccessor, _context);
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
        public IActionResult Index(int id)
        {
            var productView = ps.ProductVMWithCartCount(id);
            return View(productView);
        }



        [HttpGet]
        [Route("products/category/{id}")]
        public IActionResult Category(int id)
        {
            var cartCount = cs.GetCartCount();

            var domainName = ps.GetCategoryByDomainID(id);
            var products = _context.Products
                .Include(c => c.Category)
                .Include(c => c.District)
                .Where(c => c.Category.Domain == domainName)
                .ToList();
            var pro = ps.GetAllProducts();
            var categories = ps.GetAllCategories();
            var districts = ps.GetAllDistricts();
            var domains = ps.GetCategoryByDomain();

            var productView = new ProductViewModel
            {
                Products = products,
                Category = categories,
                District = districts,
                DistinctCat = domains,
                BaseProduct = pro, //it will always remain same as it is inherited
                CartCount = cartCount,
                TotalPrice = cs.GetCartTotalPrice(),
                Sellers = _context.Sellers.ToList()
            };
            return View("Index", productView);
        }


        [Authorize]
        [HttpGet]
        [Route("Products/addproduct")]
        public IActionResult AddProduct()
        {

            var viewmodel = ps.GetProductViewModel();
            return View(viewmodel);
        }


        [Authorize]
        [HttpPost]
        public IActionResult AddProduct(IFormFile file, ProductViewModel vm)
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
                    Title = vm.Product.Title,

                    Description = vm.Product.Description,
                    Price = vm.Product.Price,
                    SellerId = 1,  //manually value. Next time it will be from Session User Value
                    CategoryId = vm.Product.CategoryId,
                    DistrictId = vm.Product.DistrictId,
                    IsPublished = vm.Product.IsPublished,  //manually valuess
                    Unit = vm.Product.Unit,
                    ItemInStock = vm.Product.ItemInStock,
                    CreatedAt = DateTime.Now,//manually valuess
                    UpdatedAt = DateTime.Now,//manually valuess
                    OfferExpireDate = DateTime.Now,//manually valuess
                    ImagePath = imgPath,
                    OfferPrice = vm.Product.OfferPrice,

                };

                _context.Add(products);
                _context.SaveChanges();

                int id = products.Id;


            }
            else
            {
                var viewmodel = ps.GetProductViewModel();

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
            var domains = _context.Categories.Select(c => c.Domain).Distinct().ToList();

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