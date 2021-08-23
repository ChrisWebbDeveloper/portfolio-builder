using System.Collections.Generic;

namespace PortfolioBuilder.Models
{
    public class About
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ICollection<AboutPhoto> Photos { get; set; }
    }
}
