using FreshMart.Core;
using FreshMart.Database;
using FreshMart.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;

namespace FreshMart.Services
{
    public class CartService : ICartService
    {
        private int cartCount;
        private float total;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _context;

        public CartService(IHttpContextAccessor httpContextAccessor,
            AppDbContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }


        public int GetCartCount()
        {

            if (SessionHelper.GetObjectFromJson<List<CartItem>>(_httpContextAccessor.HttpContext.Session, "cart") == null)
            {
                this.cartCount = 0;
            }
            else
            {
                var cart = SessionHelper.GetObjectFromJson<List<CartItem>>(_httpContextAccessor.HttpContext.Session, "cart");
                this.cartCount = cart.Count;
            }

            return cartCount;
        }


        public float GetCartTotalPrice()
        {
            if (SessionHelper.GetObjectFromJson<List<CartItem>>(_httpContextAccessor.HttpContext.Session, "cart") == null)
            {
                this.total = 0;
            }
            else
            {
                var cart = SessionHelper.GetObjectFromJson<List<CartItem>>(_httpContextAccessor.HttpContext.Session, "cart");
                this.total = cart.Sum(c => c.Product.Price * c.Quantity);
            }

            return this.total;
        }

        //public GetCartItems()
        //{

        //}

    }

}
