using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FreshMart.Core;
using FreshMart.Database;
using FreshMart.Helper;
using FreshMart.Models;
using FreshMart.Models.ViewModels;
using FreshMart.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FreshMart.Controllers
{
    //    [Authorize]
    public class OrderController : Controller
    {

        //ApplicationUser appUser;

        private readonly ApplicationDbContext _context;
        private int cartCount;
        private Customer customer;
        private IHttpContextAccessor _httpContextAccessor;

        public OrderController(ApplicationDbContext con, IHttpContextAccessor hca)
        {
            _context = con;
            _httpContextAccessor = hca;
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            //var b = User.Identity.Name.ToString();

        }


        [HttpGet]
        [Route("Order/Checkout")]
        public ActionResult Checkout()
        {

            if (SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart") == null)
            {
                ViewBag.Message = "Your Cart Is Empty";
                this.cartCount = 0;
                return RedirectToAction("cart", "Cart");
            }
            else
            {
                var cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
                ViewBag.cart = cart;

                this.cartCount = cart.Count;
                ViewBag.total = cart.Sum(c => c.Product.Price * c.Quantity);
            }
            //Cart facts here end.................

            var districts = _context.Districts.ToList();
            var categories = _context.Categories.ToList();
            var domains = _context.Categories.Select(c => c.Domain).Distinct().ToList();
            CartService cs = new CartService(_httpContextAccessor, _context);
            var totalPrice = cs.GetCartTotalPrice();
            var Count = cs.GetCartCount();



            var viewmodel = new OrderViewModel();

            viewmodel.District = districts;

            viewmodel.Category = categories;
            viewmodel.DistinctCat = domains;

            var customer = _context.Customers.Where(c => c.Email == User.Identity.Name);
            if (customer.SingleOrDefault() != null)
            {
                viewmodel.Customer = customer.SingleOrDefault();
            }

            if (User.Identity.Name != null)
            {
                ViewBag.Email = User.Identity.Name;
                viewmodel.Order = new Order
                {
                    Email = User.Identity.Name
                };
            }
            else
            {
                ViewBag.Email = null;
            }

            viewmodel.CartCount = Count;
            viewmodel.TotalPrice = totalPrice;
            viewmodel.Sellers = _context.Sellers.ToList();

            return View(viewmodel);
        }

        [HttpPost]
        public ActionResult Checkout(OrderViewModel ovModel)
        {

            if (SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart") == null)
            {
                return RedirectToAction("Index", "Products");
            }

            var customerChk = _context.Customers.Where(c => c.Email == ovModel.Order.Email);

            if (customerChk.ToList().Count < 1)
            {
                var cust = new Customer();
                cust.Name = ovModel.Order.Name;
                cust.Email = ovModel.Order.Email;
                cust.Phone = ovModel.Customer.Phone;
                cust.DistrictId = ovModel.Order.DistrictId;
                _context.Customers.Add(cust);
                _context.SaveChanges();
                customer = cust;
            }
            else
            {
                customer = _context.Customers.Find(ovModel.Customer.Id);
            }

            var order = new Order
            {
                Name = customer.Name,
                Email = customer.Email,
                DistrictId = customer.DistrictId,
                //fixed portion
                ShippingAddress = ovModel.Order.ShippingAddress,
                PostalCode = ovModel.Order.PostalCode,
                StreetNo = ovModel.Order.StreetNo,
                CustomerId = customer.Id,
                TotalPrice = ovModel.TotalPrice,
                OrderDate = DateTime.Now,
                AccountNo = ovModel.Order.AccountNo,
                TransactionId = ovModel.Order.TransactionId
            };

            _context.Orders.Add(order);
            _context.SaveChanges();


            var cartList = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");

            ViewBag.total = cartList.Sum(c => c.Product.Price * c.Quantity);

            foreach (var item in cartList)
            {
                var order_product = new ProductOrder
                {
                    OrderId = order.Id,
                    ProductId = item.Product.Id,
                    NumberOfProduct = item.Quantity
                };

                _context.ProductOrders.Add(order_product);
                _context.SaveChanges();


                var quantity = _context.Products.Where(c => c.Id == item.Product.Id).SingleOrDefault();
                if (quantity != null)
                {
                    var pro = _context.Products.Find(item.Product.Id);
                    pro.ItemInStock = pro.ItemInStock - order_product.NumberOfProduct;

                    _context.Products.Update(pro);
                    _context.SaveChanges();
                }
            }
            //clear the session now
            HttpContext.Session.Clear();

            var agents = _context.Agents.Where(c => c.DistrictId == order.DistrictId).ToList();
            var random = new Random();
            var index = random.Next(0, agents.Count);
            var agent = agents[index];

            var agentorder = new AgentOrder
            {
                AgentId = agent.Id,
                OrderId = order.Id,
                IsPaid = false,
                IsOnRoute = false,
                IsFullyCompleted = false
            };

            _context.AgentOrders.Add(agentorder);
            _context.SaveChanges();



            TempData["orderSuccess"] = "Your order has been placed successfully!";
            return RedirectToAction("Checkout");
        }

    }
}