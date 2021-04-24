using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using roles.Data;
using roles.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace eshop.Controllers
{
    public class ShopController : Controller
    {
        ApplicationDbContext _db;

        public ShopController(ApplicationDbContext applicationDbContext)
        {
            _db = applicationDbContext;
        }

        public IActionResult Index()
        {
            List<Product> products = _db.Products.Include(c => c.Category).ToList();
            ViewBag.products = products;
            return View();
        }

        public IActionResult AddToCart(long id)
        {
            var cart = HttpContext.Session.GetString("cart");
            List<long> Cart = new List<long>();
            if (cart != null)
            {
                Cart = JsonConvert.DeserializeObject<List<long>>(cart);
            }
            Cart.Add(id);
            cart = JsonConvert.SerializeObject(Cart);
            HttpContext.Session.SetString("cart", cart);

            ViewBag.cart = Cart;
            return View();
        }

        public IActionResult Cart()
        {
            var cart = HttpContext.Session.GetString("cart");
            List<long> Cart = new List<long>();
            if (cart != null)
            {
                Cart = JsonConvert.DeserializeObject<List<long>>(cart);
            }
            ViewBag.empty = Cart.Count() == 0 ? true : false;
            ViewBag.cart = Cart;
            return View();
        }
    }
}
