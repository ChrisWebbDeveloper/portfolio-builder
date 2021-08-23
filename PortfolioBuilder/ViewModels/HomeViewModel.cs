using PortfolioBuilder.Models;
using System.Collections.Generic;

namespace PortfolioBuilder.ViewModels
{
    public class HomeViewModel
    {
        public List<CarouselPhoto> CarouselPhotos { get; set; }
        public List<FeaturedPortfolio> FeaturedPortfolios { get; set; }
    }
}
