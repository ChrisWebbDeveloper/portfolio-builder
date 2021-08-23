using PortfolioBuilder.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PortfolioBuilder.ViewModels
{
    public class UploadPhotosViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }

        [Display(Name = "File")]
        public IFormFile Photo { get; set; }
    }
}
