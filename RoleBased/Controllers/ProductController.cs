using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoleBased.Data;
using RoleBased.Models;
using RoleBased.ViewModels;

namespace RoleProductMvc.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IDataProtector _protector;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProductController(ApplicationDbContext db, IDataProtectionProvider provider, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _protector = provider.CreateProtector("ProductPriceProtector");
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _db.Products.OrderByDescending(p => p.CreatedAt).ToListAsync();
            var vm = products.Select(p => new ProductViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Price = decimal.Parse(_protector.Unprotect(p.EncryptedPrice))
            }).ToList();

            return View(vm);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View(new ProductViewModel());
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var encrypted = _protector.Protect(model.Price.ToString());
            var user = await _userManager.GetUserAsync(User);

            var product = new Product
            {
                Name = model.Name,
                EncryptedPrice = encrypted,
                CreatedAt = DateTime.UtcNow,
                CreatedByUserId = user?.Id
            };

            _db.Products.Add(product);
            await _db.SaveChangesAsync();

            TempData["Success"] = $"Product \"{product.Name}\" has been successfully created!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product == null) return NotFound();

            var vm = new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Price = decimal.Parse(_protector.Unprotect(product.EncryptedPrice))
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var product = await _db.Products.FindAsync(model.Id);
            if (product == null) return NotFound();

            product.Name = model.Name;
            product.EncryptedPrice = _protector.Protect(model.Price.ToString());

            _db.Products.Update(product);
            await _db.SaveChangesAsync();

            TempData["Success"] = $"Product \"{product.Name}\" has been successfully updated!";
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product == null) return NotFound();
            var vm = new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Price = decimal.Parse(_protector.Unprotect(product.EncryptedPrice))
            };
            return View(vm);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product == null) return NotFound();

            _db.Products.Remove(product);
            await _db.SaveChangesAsync();

            TempData["Success"] = $"Product \"{product.Name}\" deleted.";
            return RedirectToAction(nameof(Index));
        }
    }
}
