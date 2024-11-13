using CoffeeShopManagement.Models.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CoffeeShopManagement.Business.Services
{
    public class CartCookieService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string CartCookieKey = "UserCart";

        public CartCookieService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public List<CartItem> GetCartItems()
        {
            var cookieValue = _httpContextAccessor.HttpContext.Request.Cookies[CartCookieKey];
            if (string.IsNullOrEmpty(cookieValue)) return new List<CartItem>();

            return JsonConvert.DeserializeObject<List<CartItem>>(cookieValue) ?? new List<CartItem>();
        }

        public void AddToCart(CartItem item)
        {
            var cartItems = GetCartItems();
            var existingItem = cartItems.Find(x => x.ProductId == item.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                cartItems.Add(item);
            }

            SaveCartItems(cartItems);
        }

        public void SaveCartItems(List<CartItem> cartItems)
        {
            var options = new CookieOptions
            {
                HttpOnly = true, 
                Expires = DateTimeOffset.Now.AddDays(7) 
            };

            var cookieValue = JsonConvert.SerializeObject(cartItems);
            _httpContextAccessor.HttpContext.Response.Cookies.Append(CartCookieKey, cookieValue, options);
        }


        public void RemoveFromCart(Guid productId)
        {
            var cartItems = GetCartItems();
            cartItems.RemoveAll(x => x.ProductId == productId);
            SaveCartItems(cartItems);
        }

        public void ClearCart()
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(CartCookieKey);
        }
    }
}
