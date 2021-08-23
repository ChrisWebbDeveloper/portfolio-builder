namespace PortfolioBuilder.Models
{
    public class PortfolioPhoto
    {
        public int Id { get; set; }
        public int PortfolioId { get; set; }
        public int PhotoId { get; set; }
        public int? Position { get; set; }
        public int? Width { get; set; }

        public Portfolio Portfolio { get; set; }
        public Photo Photo { get; set; }
    }
}