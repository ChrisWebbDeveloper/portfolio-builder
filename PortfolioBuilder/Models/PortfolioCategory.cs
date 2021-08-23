namespace PortfolioBuilder.Models
{
    public class PortfolioCategory
    {
        public int Id { get; set; }
        public int PortfolioId { get; set; }
        public int CategoryId { get; set; }
        public int? Position { get; set; }
        public int? Width { get; set; }

        public Portfolio Portfolio { get; set; }
        public Category Category { get; set; }
    }
}