using FinalExam.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinalExam.DataAccessLayer
{
    public class ExamDbContext:IdentityDbContext<AppUser>
    {
        public ExamDbContext(DbContextOptions option):base(option)
        {
            
        }
        public DbSet<Product> Products { get; set; }
    }
}