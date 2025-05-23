using FinalExam.DataAccessLayer;
using FinalExam.ViewModels.HomeViewModels;
using FinalExam.ViewModels.ProductVM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalExam.Controllers
{
    public class HomeController(ExamDbContext _context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.Select(x => new ProductGetVM()
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                ImageUrl = x.ImageUrl,
                Reviews = x.Reviews,
                Price = x.Price
            }).ToListAsync();
            HomeVM homevm = new()
            {
                Products = products,
            };
            return View(homevm);
        }
    }
}
