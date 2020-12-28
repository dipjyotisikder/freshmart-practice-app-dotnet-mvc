using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FreshMart.Models;
using Microsoft.EntityFrameworkCore;
using FreshMart.Helper;
using Microsoft.AspNetCore.Http;
using FreshMart.Services;
using FreshMart.Database;
using FreshMart.ViewModels;
using FreshMart.Core;

namespace FreshMart.Controllers
{
    public class HomeController : Controller
    {


        private readonly AppDbContext _context;
        private IHttpContextAccessor _httpContextAccessor;
        private int cartCount;
        private CartService cs;
        public HomeController(AppDbContext con, IHttpContextAccessor hca)
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
        public async Task<IActionResult> Index(string msg)
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



            var products = await _context.Products
               .Include(c => c.Category)
               .Include(c => c.District).AsNoTracking()
               .ToListAsync();
            var categories = await _context.Categories.Where(x => !x.IsParent).AsNoTracking().ToListAsync();
            var districts = await _context.Districts.AsNoTracking().ToListAsync();
            var domains = await _context.Categories.Where(x => x.IsParent).AsNoTracking().Select(c => c.Name).ToListAsync();


            var productView = new ProductViewModel
            {
                Products = products,
                Category = categories,
                District = districts,
                DistinctCat = domains,
                CartCount = this.cartCount,
                TotalPrice = cs.GetCartTotalPrice(),
                Message = a,
                Sellers = await _context.Sellers.AsNoTracking().ToListAsync(),
                Customers = await _context.Customers.AsNoTracking().ToListAsync(),

            };

            return View(productView);
        }


        [Route("/Home/About")]
        public async Task<IActionResult> About()
        {
            ViewData["Message"] = "Your application description page.";


            var categories = await _context.Categories.Where(x => !x.IsParent).ToListAsync();
            var districts = await _context.Districts.ToListAsync();
            var parents = await _context.Categories.Where(x => x.IsParent).Select(c => c.Parent.Name).Distinct().ToListAsync();

            var cartCount = cs.GetCartCount();
            var productView = new ProductViewModel
            {
                Category = categories,
                District = districts,
                DistinctCat = parents,
                CartCount = cartCount,
                TotalPrice = cs.GetCartTotalPrice(),
                Sellers = await _context.Sellers.ToListAsync()
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
            var domains = _context.Categories.Where(x => x.ParentId != null).Select(c => c.Parent.Name).Distinct().ToList();

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
