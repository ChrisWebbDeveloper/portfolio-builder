namespace PortfolioBuilder.Models
{
    public class CarouselPhoto
    {
        public CarouselPhoto()
        {
            ShowName = false;
            ShowDescription = false;
        }

        public int Id { get; set; }
        public int PhotoId { get; set; }
        public int? Position { get; set; }
        public bool? ShowName { get; set; }
        public bool? ShowDescription { get; set; }

        public Photo Photo { get; set; }
    }
}
