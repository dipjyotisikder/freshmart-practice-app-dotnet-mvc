using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FreshMart.Data;
using FreshMart.Helper;
using FreshMart.Models;
using FreshMart.Services;
using FreshMart.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FreshMart.Controllers
{

    public class CartController : Controller
    {
        //ApplicationUser appUser;

        private readonly ApplicationDbContext _context;
        private int cartCount;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IProductService _productService;
        public CartController(ApplicationDbContext con, IHttpContextAccessor hca, IProductService productService)
        {
            _context = con;
            _httpContextAccessor = hca;
            _productService = productService;
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            //var b = User.Identity.Name.ToString();

        }
        [Route("/Cart")]
        public ActionResult Cart()
        {

            if (SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart") == null)
            {
                ViewBag.Message = "Your Cart Is Empty";
                this.cartCount = 0;
            }
            else
            {
                var cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
                ViewBag.cart = cart;
                this.cartCount = cart.Count;
                ViewBag.total = cart.Sum(c => c.Product.Price * c.Quantity);
            }
            var viewmodel = _productService.GetProductViewModel();

            ViewBag.CartCount = this.cartCount;
            return View(viewmodel);
        }

        [Route("Cart/addtocart/{id}")]
        public ActionResult AddToCart(int id)
        {



            //ViewBag.route = Url.RouteUrl(RouteData.Values);
            if (SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart") == null)
            {
                var cart = new List<CartItem>();
                var item = new CartItem
                {
                    Product = _context.Products.Find(id),
                    Quantity = 1,

                };
                cart.Add(item);

                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
                var sessiondata = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            }
            else
            {
                var cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
                int index = Exists(cart, id);
                if (index == -1)
                {
                    var item = new CartItem
                    {
                        Product = _context.Products.Find(id),
                        Quantity = 1,

                    };
                    cart.Add(item);
                }
                else
                {
                    cart[index].Quantity++;
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }

            return RedirectToAction("Cart");
        }

        [Route("Cart/RemoveItem/{id}")]
        public ActionResult RemoveItem(int id)
        {

            var cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            int index = Exists(cart, id);
            cart.RemoveAt(index);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);

            return RedirectToAction("Cart");
        }

        //local function start
        private int Exists(List<CartItem> cart, int id)
        {
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].Product.Id == id)
                {
                    return i;
                }
            }
            return -1;
        }

        //local function end




    }
}