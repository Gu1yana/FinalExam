using FinalExam.Models.Common;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace FinalExam.Models
{
    public class Product:BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public int Reviews { get; set; }
    }
}
