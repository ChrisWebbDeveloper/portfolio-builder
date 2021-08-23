using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PortfolioBuilder.Models
{
    public class Portfolio
    {
        public Portfolio()
        {
            Private = false;
            Published = false;
            Featured = false;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Private { get; set; }
        public bool Published { get; set; }
        public bool Featured { get; set; }
        public int? Position { get; set; }
        public int? Width { get; set; }
        public string PasswordHash { get; set; }

        [Display(Name = "Password")]
        public string UnencryptedPassword { get; set; }

        [Display(Name = "Custom HTML")]
        public string CustomHtml { get; set; }

        [Display(Name = "Featured Photo")]
        public int? FeaturedPhotoId { get; set; }

        public Photo FeaturedPhoto { get; set; }
        public FeaturedPortfolio FeaturedPortfolio { get; set; }
        public virtual ICollection<PortfolioPhoto> Photos { get; set; }
        public virtual ICollection<PortfolioCategory> Categories { get; set; }

    }
}