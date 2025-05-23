namespace FinalExam.ViewModels.ProductVM
{
    public class ProductUpdateVM
    {
        public string Title { get; set; }
        public IFormFile? ImageFile { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public int Reviews { get; set; }
    }
}
