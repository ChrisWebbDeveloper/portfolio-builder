using System.Diagnostics;
using PortfolioBuilder.Data;
using PortfolioBuilder.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PortfolioBuilder.Controllers
{
    public class ErrorsController : ApplicationController
    {
        public ErrorsController(ApplicationDbContext context) : base(context)
        {

        }

        [Route("Error/{statusCode}")]
        public IActionResult Error(int statusCode)
        {
            ViewBag.StatusCode = statusCode;
            ErrorViewModel errorVM = null;

            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "Sorry, the page you are looking for could not be found.";
                    ViewBag.ErrorDescription = "Something may have been removed, or lost long ago.";
                    break;
                case 500:
                    ViewBag.ErrorMessage = "Something is wrong with the code here";
                    ViewBag.ErrorDescription = "The code appears to be a bit buggy in relation to something you have tried. Let our developer know where to look with the ID below:";
                    errorVM = new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier };
                    break;
                default:
                    break;
            }
            return View("Error", errorVM);
        }
    }
}