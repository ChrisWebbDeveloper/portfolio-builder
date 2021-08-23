namespace PortfolioBuilder.Models
{
    public class AboutPhoto
    {
        public AboutPhoto()
        {
            AboutId = 1;
        }

        public int Id { get; set; }
        public int AboutId { get; set; }
        public int PhotoId { get; set; }
        public int? Position { get; set; }

        public Photo Photo { get; set; }
    }
}
