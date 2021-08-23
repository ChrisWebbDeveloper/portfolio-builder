using System.ComponentModel.DataAnnotations;

namespace PortfolioBuilder.Models
{
    public class FeaturedPortfolio
    {
        public int Id { get; set; }

        [Display(Name = "Portfolio")]
        public int PortfolioId { get; set; }
        public int? Position { get; set; }

        public Portfolio Portfolio { get; set; }
    }
}
