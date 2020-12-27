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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FreshMart.Controllers
{
    //    [Authorize]
    public class OrderController : Controller
    {

        //AppUser appUser;

        private readonly AppDbContext _context;
        private int cartCount;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;

        public OrderController(AppDbContext con,
            IHttpContextAccessor hca,
            UserManager<AppUser> userManager)
        {
            _context = con;
            _httpContextAccessor = hca;
            _userManager = userManager;
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
            var domains = _context.Categories.Where(x => x.ParentId != null).Select(c => c.Parent.Name).Distinct().ToList();
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
        public async Task<ActionResult> Checkout(OrderViewModel ovModel)
        {

            if (SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart") == null)
            {
                return RedirectToAction("Index", "Products");
            }


            Customer customer = new Customer();
            var customerExisted = _context.Customers.Where(c => c.Email == ovModel.Order.Email).AsNoTracking().FirstOrDefault();
            if (customerExisted == null)
            {
                //nw user
                var user = new AppUser
                {
                    Email = ovModel.Order.Email,
                    UserName = ovModel.Order.Email
                };
                var res = await _userManager.CreateAsync(user);
                if (res.Succeeded)
                {
                    var cust = new Customer
                    {
                        Id = NumberUtilities.GetUniqueNumber(),
                        Name = ovModel.Order.Name,
                        Email = ovModel.Order.Email,
                        Phone = ovModel.Customer.Phone,
                        UserId = user.Id
                    };
                    await _context.Customers.AddAsync(cust);
                    await _context.SaveChangesAsync();
                    customer = cust;
                }
            }
            else
            {
                customer = _context.Customers.Find(ovModel.Customer.Id);
            }

            var order = new Order
            {
                Id = NumberUtilities.GetUniqueNumber(),
                Name = customer.Name,
                Email = customer.Email,
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
                    Id = NumberUtilities.GetUniqueNumber(),
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

            var agents = _context.Agents.Where(c => c.User.DistrictId == customer.User.DistrictId).ToList();
            var random = new Random();
            var index = random.Next(0, agents.Count);
            var agent = agents[index];

            var agentorder = new AgentOrder
            {
                Id = NumberUtilities.GetUniqueNumber(),
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