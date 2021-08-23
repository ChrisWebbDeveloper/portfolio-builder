using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace PortfolioBuilder.Models
{
    public class Contact
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Email { get; set; }

        [Display(Name = "Phone No.")]
        public string PhoneNo { get; set; }

        public string Facebook { get; set; }
        public string Instagram { get; set; }
        public string LinkedIn { get; set; }
    }
}
