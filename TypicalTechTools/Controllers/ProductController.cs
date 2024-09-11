using Ganss.Xss;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TypicalTechTools.Models;
using TypicalTechTools.Models.Repositories;

namespace TypicalTools.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly HtmlSanitizer _sanitizer;

        public ProductController(IProductRepository productRepository, HtmlSanitizer sanitizer)
        {
            _productRepository = productRepository;
            _sanitizer = sanitizer;
        }

        // Show all products
        public IActionResult Index()
        {
            var products = _productRepository.GetAllProducts();
            return View(products);
        }

        // Show details of a product
        public IActionResult Details(int id)
        {
            var product = _productRepository.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // Show form to create a new product
        [Authorize(Roles = "ADMIN")]
        public IActionResult Create()
        {
            string authenticated = HttpContext.Session.GetString("Authenticated") ?? "False";
            if (!authenticated.Equals("True", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("AdminLogin", "Admin");
            }

            return View();
        }

        // POST: Create a new product
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMIN")]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                string authenticated = HttpContext.Session.GetString("Authenticated") ?? "False";
                if (!authenticated.Equals("True", StringComparison.OrdinalIgnoreCase))
                {
                    return RedirectToAction("AdminLogin", "Admin");
                }

                // Sanitize product fields
                product.ProductName = _sanitizer.Sanitize(product.ProductName);
                product.ProductDescription = _sanitizer.Sanitize(product.ProductDescription);

                // Set the UpdatedDate before saving
                product.UpdatedDate = DateTime.UtcNow;

                _productRepository.CreateProduct(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // Show form to edit an existing product
        [Authorize(Roles = "ADMIN")]
        public IActionResult Edit(int id)
        {
            string authenticated = HttpContext.Session.GetString("Authenticated") ?? "False";
            if (!authenticated.Equals("True", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("AdminLogin", "Admin");
            }

            var product = _productRepository.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Edit an existing product price
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMIN")]
        public IActionResult Edit(int id, decimal productPrice)
        {
            string authenticated = HttpContext.Session.GetString("Authenticated") ?? "False";
            if (!authenticated.Equals("True", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("AdminLogin", "Admin");
            }

            var existingProduct = _productRepository.GetProductById(id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            // Update the price and set the UpdatedDate
            existingProduct.ProductPrice = productPrice;
            existingProduct.UpdatedDate = DateTime.UtcNow;

            _productRepository.UpdateProductPrice(existingProduct);
            return RedirectToAction(nameof(Index));
        }

        // Show confirmation page to delete a product
        [Authorize(Roles = "ADMIN")]
        public IActionResult Delete(int id)
        {
            string authenticated = HttpContext.Session.GetString("Authenticated") ?? "False";
            if (!authenticated.Equals("True", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("AdminLogin", "Admin");
            }

            var product = _productRepository.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Delete a product
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMIN")]
        public IActionResult DeleteConfirmed(int id)
        {
            string authenticated = HttpContext.Session.GetString("Authenticated") ?? "False";
            if (!authenticated.Equals("True", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("AdminLogin", "Admin");
            }

            _productRepository.DeleteProduct(id);
            return RedirectToAction(nameof(Index));
        }

        // Privacy page
        public IActionResult Privacy()
        {
            return View();
        }
    }
}
