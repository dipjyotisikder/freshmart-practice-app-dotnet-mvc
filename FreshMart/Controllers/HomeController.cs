using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FreshMart.Models;
using FreshMart.Data;
using Microsoft.EntityFrameworkCore;
using FreshMart.ViewModels;
using FreshMart.Helper;
using Microsoft.AspNetCore.Http;
using FreshMart.Services;

namespace FreshMart.Controllers
{
    public class HomeController : Controller
    {


        private readonly ApplicationDbContext _context;
        private IHttpContextAccessor _httpContextAccessor;
        private int cartCount;
        private CartService cs;
        public HomeController(ApplicationDbContext con, IHttpContextAccessor hca)
        {
            _context = con;
            _httpContextAccessor = hca;
            cs = new CartService(hca, con);
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        [Route("/{msg?}")]
        public IActionResult Index(string msg)
        {
            string a = "";
            if (msg != null)
            {
                a = "Success";
            }


            if (SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart") == null)
            {
                this.cartCount = 0;
            }
            else
            {
                var cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
                this.cartCount = cart.Count;
            }



            var products = _context.Products
               .Include(c => c.Category)
               .Include(c => c.District)
               .ToList();
            var categories = _context.Categories.ToList();
            var districts = _context.Districts.ToList();
            var domains = _context.Categories.Select(c => c.Domain).Distinct().ToList();


            var productView = new ProductViewModel
            {
                Products = products,
                Category = categories,
                District = districts,
                DistinctCat = domains,
                CartCount = this.cartCount,
                TotalPrice = cs.GetCartTotalPrice(),
                Message = a,
                Sellers = _context.Sellers.ToList(),
                Customers = _context.Customers.ToList(),

            };

            return View(productView);
        }


        [Route("/Home/About")]
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";


            var categories = _context.Categories.ToList();
            var districts = _context.Districts.ToList();
            var domains = _context.Categories.Select(c => c.Domain).Distinct().ToList();

            var cartCount = cs.GetCartCount();
            var productView = new ProductViewModel
            {
                Category = categories,
                District = districts,
                DistinctCat = domains,
                CartCount = cartCount,
                TotalPrice = cs.GetCartTotalPrice(),
                Sellers = _context.Sellers.ToList()
            };




            return View(productView);
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        [Route("Home/Faq")]
        public IActionResult Faq()
        {

            ViewData["Message"] = "Your application Faq page.";


            var categories = _context.Categories.ToList();
            var districts = _context.Districts.ToList();
            var domains = _context.Categories.Select(c => c.Domain).Distinct().ToList();

            var cartCount = cs.GetCartCount();
            var productView = new ProductViewModel
            {
                Category = categories,
                District = districts,
                DistinctCat = domains,
                CartCount = cartCount,
                TotalPrice = cs.GetCartTotalPrice(),
                Sellers = _context.Sellers.ToList()
            };


            return View(productView);
        }


        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
