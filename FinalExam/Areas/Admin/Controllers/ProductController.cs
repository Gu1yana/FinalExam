using FinalExam.DataAccessLayer;
using FinalExam.Models;
using FinalExam.ViewModels.ProductVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace FinalExam.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin, Moderator, SuperAdmin")]
    public class ProductController(ExamDbContext _context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var products=await _context.Products.Select(x=>new ProductGetVM()
            {
                Id=x.Id,
                Title=x.Title,
                Description=x.Description,
                ImageUrl=x.ImageUrl,
                Reviews=x.Reviews,
                Price=x.Price
            }).ToListAsync();
            return View(products);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateVM vm)
        {
            if (!ModelState.IsValid) return View(vm);

            if (!vm.ImageFile.ContentType.StartsWith("image"))
            {
                ModelState.AddModelError("ImageFile", "Fayl shekil tipinde olmalidir!");
                return View();
            }
            if (vm.ImageFile.Length > 1024 * 1024 * 2)
            {
                ModelState.AddModelError("ImageFile", "Fayl olchusu 2mb dan az olmalidir!");
                return View();
            }
            string newImageUrl = Guid.NewGuid().ToString() + vm.ImageFile.FileName;
            string path = Path.Combine("wwwroot", "productsImg", newImageUrl);
            using FileStream fs = new(path, FileMode.OpenOrCreate);
            await vm.ImageFile.CopyToAsync(fs);
            Product product = new() 
            { 
                Title=vm.Title,
                Description=vm.Description,
                Reviews=vm.Reviews,
                Price=vm.Price,
                ImageUrl=newImageUrl
            };
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();   
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (!id.HasValue || id < 1) return BadRequest();
            var entity = await _context.Products.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (entity is null) return NotFound();
            _context.Products.Remove(entity);
            if (!System.IO.File.Exists(Path.Combine("wwwroot", "productsImg", entity.ImageUrl)))
                System.IO.File.Delete(Path.Combine("wwwroot", "productsImg", entity.ImageUrl));
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (!id.HasValue || id < 1) return BadRequest();
            var entity = await _context.Products.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (entity is null) return NotFound();
            ProductUpdateVM vm = new()
            {
                Reviews = entity.Reviews,
                Price = entity.Price,
                Description = entity.Description,
                Title = entity.Title,
            };
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id,ProductUpdateVM vm)
        {
            if (!id.HasValue || id < 1) return BadRequest();
            var entity = await _context.Products.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (entity is null) return NotFound();
            if(vm.ImageFile is not null)
            {
                if (!vm.ImageFile.ContentType.StartsWith("image"))
                {
                    ModelState.AddModelError("ImageFile", "Fayl shekil tipinde olmalidir!");
                    return View();
                }
                if (vm.ImageFile.Length > 1024 * 1024 * 2)
                {
                    ModelState.AddModelError("ImageFile", "Fayl olchusu 2mb dan az olmalidir!");
                    return View();
                }
                string newImageUrl = Guid.NewGuid().ToString() + vm.ImageFile.FileName;
                string path = Path.Combine("wwwroot", "productsImg", newImageUrl);
                using FileStream fs = new(path, FileMode.OpenOrCreate);
                if (!System.IO.File.Exists(Path.Combine("wwwroot", "productsImg", entity.ImageUrl)))
                    System.IO.File.Delete(Path.Combine("wwwroot", "productsImg", entity.ImageUrl));
                await vm.ImageFile.CopyToAsync(fs);
                entity.ImageUrl = newImageUrl;
            }
            entity.Description = vm.Description;
            entity.Title = vm.Title;
            entity.Price = vm.Price;
            entity.Reviews = vm.Reviews;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
