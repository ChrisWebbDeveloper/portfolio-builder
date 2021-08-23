using PortfolioBuilder.Models;

namespace PortfolioBuilder.ViewModels
{
    public class CategoryPortfolioDataViewModel
    {
        public int PortfolioId { get; set; }
        public string Name { get; set; }
        public Photo FeaturedPhoto { get; set; }
        public int? Position { get; set; }
        public int? Width { get; set; }
        public bool Selected { get; set; }
    }
}
