namespace FinalExam.ViewModels.ProductVM
{
    public class ProductCreateVM
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public IFormFile? ImageFile { get; set; }
        public int Reviews { get; set; }
    }
}
