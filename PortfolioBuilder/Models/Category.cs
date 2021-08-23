using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PortfolioBuilder.Models
{
    public class Category
    {
        public Category()
        {
            Published = false;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool Published { get; set; }
        public int? Position { get; set; }
        public int? Width { get; set; }

        [Display(Name = "Featured Photo")]
        public int? FeaturedPhotoId { get; set; }

        public Photo FeaturedPhoto { get; set; }
        public virtual ICollection<PortfolioCategory> Portfolios { get; set; }
    }
}