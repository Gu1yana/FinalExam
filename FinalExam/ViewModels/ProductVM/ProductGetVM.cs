using FinalExam.ViewModels.CommonVM;

namespace FinalExam.ViewModels.ProductVM
{
    public class ProductGetVM:BaseVM
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public int Reviews { get; set; }
    }
}
