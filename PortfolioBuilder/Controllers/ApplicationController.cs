using PortfolioBuilder.Data;
using PortfolioBuilder.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace PortfolioBuilder.Controllers
{
    public abstract class ApplicationController : Controller
    {
        protected readonly ApplicationDbContext _context;

        public ApplicationController(ApplicationDbContext context)
        {
            _context = context;           
        }

        public override void OnActionExecuted(ActionExecutedContext context) {
            var categories = _context.Categories
                .Where(ct => ct.Published == true)
                .OrderBy(ct => ct.Position)
                .ToList();

            var contactDetails = _context.ContactDetails
                .SingleOrDefault(cd => cd.Id == 1);

            ViewData["HeaderCategories"] = categories;
            ViewData["Facebook"] = contactDetails.Facebook;
            ViewData["Instagram"] = contactDetails.Instagram;
            ViewData["LinkedIn"] = contactDetails.LinkedIn;

            ViewData["VersionNo"] = GetType().Assembly.GetName().Version.ToString();
        }
    }
}
