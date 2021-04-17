using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using roles.Data;
using roles.Models;

namespace eshop.Controllers
{
    public class AdminController : Controller
    {
        ApplicationDbContext _db;
        private IWebHostEnvironment _hostingEnvironment;

   

        public AdminController(ApplicationDbContext db, IWebHostEnvironment environment)
        {
            _db = db;
            _hostingEnvironment = environment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Products()
        {
            List<Product> products = _db.Products.Include(c => c.Category).ToList();
            ViewBag.products = products;
            return View();
        }

        public IActionResult Product(long id)
        {
            Product product = _db.Products.Find(id);
            ViewBag.product = product;
            return View();
        }

        public IActionResult AddProduct()
        {
            return View();
        }


        [HttpPost]
        public IActionResult AddNewProduct(string productName, string description)
        {
            Product product = new Product();
            product.Name = productName;
            product.Description = description;
            _db.Products.Add(product);
            _db.SaveChanges();
            return RedirectToAction("Products", "Admin");
        }

        [HttpPost]
        public IActionResult UpdateProduct(long id, string productName, string description)
        {
            // return Content($"{id} {productName}");
            Product product = _db.Products.Find(id);
            product.Name = productName;
            product.Description = description;
            _db.SaveChanges();
            return RedirectToAction("Products", "Admin");
        }

        public async Task<IActionResult> UpdateProductImageAsync(long id, IFormFile image)
        {
            string uploads = Path.Combine(_hostingEnvironment.WebRootPath, "uploads/images");
            string filePath = Path.Combine(uploads, image.FileName);
            using (Stream fileStream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            Product product = _db.Products.Find(id);
            product.Image = image.FileName;
            _db.SaveChanges();

            return RedirectToAction("Product", "Admin", new { id = id });
        }
    }
}
