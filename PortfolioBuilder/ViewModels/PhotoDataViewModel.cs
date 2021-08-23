namespace PortfolioBuilder.ViewModels
{
    public class PhotoDataViewModel
    {
        public int PhotoId { get; set; }
        public string Name { get; set; }
        public string FilePath { get; set; }
        public int? Position { get; set; }
        public int? Width { get; set; }
        public bool Selected { get; set; }
    }
}
