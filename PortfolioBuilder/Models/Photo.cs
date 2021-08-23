using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PortfolioBuilder.Models
{
    public class Photo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [Display(Name = "File Path")]
        public string FilePath { get; set; }

        public virtual ICollection<PortfolioPhoto> Portfolios { get; set; }

        [Display(Name = "Featured in Portfolios")]
        public ICollection<Portfolio> FeaturedInPortfolios { get; set; }

        [Display(Name = "Featured in Categories")]
        public ICollection<Category> FeaturedInCategories { get; set; }

        [Display(Name = "Featured in Home Carousel")]
        public virtual ICollection<CarouselPhoto> FeaturedInHomeCarousel { get; set; }

        [Display(Name = "Featured in About")]
        public virtual ICollection<AboutPhoto> FeaturedInAbout { get; set; }
    }
}
