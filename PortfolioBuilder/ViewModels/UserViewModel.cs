using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PortfolioBuilder.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }

        [Display(Name = "Role(s)")]
        public IEnumerable<string> RoleNames { get; set; }
    }
}
