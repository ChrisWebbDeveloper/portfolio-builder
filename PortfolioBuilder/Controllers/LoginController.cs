using Microsoft.AspNetCore.Mvc;
using PortfolioBuilder.Data;

namespace PortfolioBuilder.Controllers
{
    public class LoginController : ApplicationController
    {
        public LoginController(ApplicationDbContext context) : base(context)
        {

        }

        public IActionResult Index()
        {
            return Redirect("Identity/Account/Login");
        }
    }
}
