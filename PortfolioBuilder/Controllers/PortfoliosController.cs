using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PortfolioBuilder.Data;
using PortfolioBuilder.Models;
using PortfolioBuilder.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using PortfolioBuilder.Security;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace PortfolioBuilder.Controllers
{
    [Route("Portfolios")]
    public class PortfoliosController : ApplicationController
    {
        private readonly IDataProtector _passwordProtector;
        private readonly IDataProtector _cookieProtector;

        public PortfoliosController(ApplicationDbContext context, IDataProtectionProvider dataProtectionProvider) : base(context)
        {
            _passwordProtector = dataProtectionProvider.CreateProtector("PortfolioPasswordHash");
            _cookieProtector = dataProtectionProvider.CreateProtector("PortfolioCookieHash");
        }

        // GET: Portfolios
        public async Task<IActionResult> Index()
        {
            bool isAuthenticated = User.Identity.IsAuthenticated;

            if (isAuthenticated)
            {
                var applicationDbContext = _context.Portfolios
                    .Include(m => m.FeaturedPhoto)
                    .OrderBy(m => m.Position)
                    .AsNoTracking();
                return View(await applicationDbContext.ToListAsync());
            }
            else
            {
                var applicationDbContext = _context.Portfolios
                    .Include(m => m.FeaturedPhoto)
                    .OrderBy(m => m.Position)
                    .Where(m => m.Published == true)
                    .Where(m => m.Private == false)
                    .AsNoTracking();
                return View(await applicationDbContext.ToListAsync());
            }
        }

        // GET: Portfolios
        [Authorize]
        [Route("Edit")]
        public async Task<IActionResult> IndexEdit()
        {
            var applicationDbContext = _context.Portfolios
                .Include(m => m.FeaturedPhoto)
                .OrderBy(m => m.Position)
                .AsNoTracking();
            return View(await applicationDbContext.ToListAsync());
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [Route("GeneralDelete")]
        public async Task<IActionResult> GeneralDelete(int[] itemsToRemove)
        {
            var portfolios = _context.Portfolios.Where(pr => itemsToRemove.Contains(pr.Id)).ToList();

            foreach (Portfolio portfolio in portfolios)
            {
                _context.Portfolios.Remove(portfolio);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [Route("GeneralEdit")]
        public async Task<IActionResult> GeneralEdit(string[] portfolioIds, string[] portfolioPositions, string[] portfolioWidths)
        {
            if (ModelState.IsValid)
            {
                for (int i = 0; i < portfolioIds.Length; i++)
                {
                    var id = int.Parse(portfolioIds[i]);
                    var position = int.Parse(portfolioPositions[i]);
                    var width = int.Parse(portfolioWidths[i]);

                    var portfolioToUpdate = _context.Portfolios.FirstOrDefault(pr => pr.Id == id);

                    portfolioToUpdate.Position = position;
                    portfolioToUpdate.Width = width;

                    _context.Portfolios.Update(portfolioToUpdate);
                }

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(IndexEdit));
        }

        // GET: Portfolios/5
        [Route("{Name}")]
        public async Task<IActionResult> Details(string Name)
        {
            if (Name.Length == 0)
            {
                return NotFound();
            }

            bool isAuthenticated = User.Identity.IsAuthenticated;

            Portfolio portfolio;

            if (isAuthenticated)
            {
                portfolio = await _context.Portfolios
                .Include(m => m.Photos)
                    .ThenInclude(m => m.Photo)
                .Include(m => m.FeaturedPhoto)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.Name == Name);

                if (portfolio == null)
                {
                    return NotFound();
                }
            }
            else
            {
                portfolio = await _context.Portfolios
                .Include(m => m.Photos)
                    .ThenInclude(m => m.Photo)
                .Include(m => m.FeaturedPhoto)
                .Where(m => m.Published == true)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.Name == Name);

                if (portfolio == null)
                {
                    return NotFound();
                }

                if (portfolio.Private == true)
                {
                    if (String.IsNullOrEmpty(portfolio.PasswordHash))
                    {
                        return View(portfolio);
                    };

                    var cookieToCheck = Request.Cookies["Portfolios." + portfolio.Id.ToString() + ".PrivateAccess"];

                    if (cookieToCheck != null)
                    {
                        if (_cookieProtector.Unprotect(cookieToCheck) != portfolio.PasswordHash)
                        {
                            //TODO add validation message for password incorrect

                            return RedirectToAction("PasswordProtected", new { name = Name });
                        }
                    }
                    else
                    {
                        return RedirectToAction("PasswordProtected", new { name = Name });
                    }
                }
            }

            portfolio.Photos = portfolio.Photos.OrderBy(m => m.Position).ToList();

            portfolio.UnencryptedPassword = Encryption.Decrypt(_passwordProtector, portfolio.PasswordHash);

            return View(portfolio);
        }

        [Route("{Name}/Access")]
        public async Task<IActionResult> PasswordProtected(string Name)
        {
            var portfolio = await _context.Portfolios
                           .AsNoTracking()
                           .SingleOrDefaultAsync(pr => pr.Name == Name);

            if (Name.Length == 0)
            {
                return NotFound();
            }

            if (portfolio == null)
            {
                return NotFound();
            }

            Response.Cookies.Delete("Portfolios." + portfolio.Id.ToString() + ".PrivateAccess");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("{Name}/Access")]
        public async Task<IActionResult> PasswordProtected(string Name, string UnencryptedPassword)
        {
            if (ModelState.IsValid)
            {
                var portfolio = await _context.Portfolios
                                .AsNoTracking()
                                .SingleOrDefaultAsync(m => m.Name == Name);

                if (Encryption.Decrypt(_passwordProtector, portfolio.PasswordHash) == UnencryptedPassword)
                {
                    CookieOptions option = new CookieOptions();
                    option.Expires = DateTime.Now.AddDays(1);

                    if (!String.IsNullOrEmpty(portfolio.PasswordHash))
                    {
                        Response.Cookies.Append("Portfolios." + portfolio.Id.ToString() + ".PrivateAccess",
                                            _cookieProtector.Protect(portfolio.PasswordHash),
                                            option);
                    }

                    return RedirectToAction("Details", new { name = Name });
                }
            }
            return View();
        }

        // GET: Portfolios/Create
        [Authorize]
        [Route("Add")]
        public IActionResult Create()
        {
            PopulateFeaturedPhotosDropDownList();
            PopulatePhotoData();

            return View();
        }

        [Authorize]
        private void PopulateFeaturedPhotosDropDownList(object selectedFeaturedPhoto = null)
        {
            var photosQuery = from ph in _context.Photos
                              orderby ph.Id
                              select ph;
            ViewData["FeaturedPhotoId"] = new SelectList(photosQuery.AsNoTracking(), "Id", "FilePath", selectedFeaturedPhoto);
        }

        // POST: Portfolios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [Route("Add")]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Private,UnencryptedPassword,Published,Featured,FeaturedPhotoId,Position,Width,CustomHtml")] Portfolio portfolio, string[] selectedPhotos, string[] photoIds, string[] selectedPhotoPositions, string[] selectedPhotoWidths)
        {
            if (ModelState.IsValid)
            {
                portfolio.PasswordHash = Encryption.Encrypt(_passwordProtector, portfolio.UnencryptedPassword);
                portfolio.CustomHtml = StripScriptTags(portfolio.CustomHtml);

                _context.Add(portfolio);
                await _context.SaveChangesAsync();

                UpdateFeaturedPortfolios(portfolio);
                await _context.SaveChangesAsync();

                UpdatePhotos(selectedPhotos, photoIds, selectedPhotoPositions, selectedPhotoWidths, portfolio);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            PopulateFeaturedPhotosDropDownList(portfolio.FeaturedPhotoId);
            PopulatePhotoData(portfolio);

            return View(portfolio);
        }

        // GET: Portfolios/5/Edit
        [Authorize]
        [Route("{Name}/Edit")]
        public async Task<IActionResult> Edit(string Name)
        {
            if (Name.Length == 0)
            {
                return NotFound();
            }

            var portfolio = await _context.Portfolios
                .Include(m => m.Photos)
                    .ThenInclude(m => m.Photo)
                .Include(m => m.FeaturedPhoto)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.Name == Name);

            if (portfolio == null)
            {
                return NotFound();
            }

            ViewBag.ReturnUrl = Request.Headers["Referer"].ToString();

            PopulateFeaturedPhotosDropDownList(portfolio.FeaturedPhotoId);
            PopulatePhotoData(portfolio);

            portfolio.UnencryptedPassword = Encryption.Decrypt(_passwordProtector, portfolio.PasswordHash);

            return View(portfolio);
        }

        [Authorize]
        private void PopulatePhotoData(Portfolio photosContext = null)
        {
            var allPhotos = _context.Photos.OrderByDescending(ph => ph.Id);
            HashSet<int> photosIncluded = new HashSet<int>();

            if (photosContext != null)
            {
                if (photosContext.Photos != null)
                {
                    photosIncluded = new HashSet<int>(photosContext.Photos.Select(pp => pp.PhotoId));
                }
            }

            var viewModel = new List<PhotoDataViewModel>();

            foreach (var photo in allPhotos)
            {
                int? position = null;
                int? width = null;

                if (photosContext != null)
                {
                    if (photosContext.Photos != null)
                    {
                        if (photosContext.Photos.Any(pp => pp.PhotoId == photo.Id))
                        {
                            position = photosContext.Photos.SingleOrDefault(pp => pp.PhotoId == photo.Id).Position;
                            width = photosContext.Photos.SingleOrDefault(pp => pp.PhotoId == photo.Id).Width;
                        }
                    }
                }

                viewModel.Add(new PhotoDataViewModel
                {
                    PhotoId = photo.Id,
                    Name = photo.Name,
                    FilePath = photo.FilePath,
                    Selected = photosIncluded.Contains(photo.Id),
                    Width = width,
                    Position = position
                });
            }

            ViewData["PhotosList"] = viewModel;
            ViewData["AllPhotos"] = allPhotos;
        }

        // POST: Portfolios/5/Edit
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [Route("{Name}/Edit")]
        public async Task<IActionResult> Edit(int Id, [Bind("Id,Name,Description,Private,UnencryptedPassword,Published,Featured,FeaturedPhotoId,Position,Width,CustomHtml")] Portfolio portfolio, string returnUrl, string[] selectedPhotos, string[] photoIds, string[] selectedPhotoPositions, string[] selectedPhotoWidths)
        {
            ViewBag.ReturnUrl = returnUrl;

            if (Id != portfolio.Id)
            {
                return NotFound();
            }

            if (returnUrl == "")
            {
                returnUrl = "Index";
            }

            if (ModelState.IsValid)
            {
                var portfolioToUpdate = await _context.Portfolios
                    .Include(pr => pr.Photos)
                        .ThenInclude(pp => pp.Photo)
                    .SingleOrDefaultAsync(m => m.Id == Id);

                var lastOfReturnUrl = returnUrl.Split("/").Last();
                var originalName = portfolioToUpdate.Name;

                portfolioToUpdate.PasswordHash = Encryption.Encrypt(_passwordProtector, portfolio.UnencryptedPassword);

                if (await TryUpdateModelAsync<Portfolio>(
                    portfolioToUpdate,
                    "",
                    pr => pr.Id,
                    pr => pr.Name,
                    pr => pr.Description,
                    pr => pr.Private,
                    pr => pr.PasswordHash,
                    pr => pr.Published,
                    pr => pr.Position,
                    pr => pr.Featured,
                    pr => pr.FeaturedPhotoId,
                    pr => pr.Width,
                    pr => pr.CustomHtml))
                {
                    portfolioToUpdate.CustomHtml = StripScriptTags(portfolioToUpdate.CustomHtml);

                    UpdatePhotos(selectedPhotos, photoIds, selectedPhotoPositions, selectedPhotoWidths, portfolioToUpdate);
                    UpdateFeaturedPortfolios(portfolioToUpdate);
                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!PortfolioExists(portfolio.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }

                    if (lastOfReturnUrl == originalName && lastOfReturnUrl != portfolio.Name)
                    {
                        return RedirectToAction("Details", new { name = portfolioToUpdate.Name });
                    }

                    return Redirect(returnUrl);
                }
                PopulateFeaturedPhotosDropDownList(portfolio.FeaturedPhotoId);
                PopulatePhotoData(portfolioToUpdate);
                UpdatePhotos(selectedPhotos, photoIds, selectedPhotoPositions, selectedPhotoWidths, portfolioToUpdate);
                UpdateFeaturedPortfolios(portfolioToUpdate);

                if (lastOfReturnUrl == originalName && lastOfReturnUrl != portfolio.Name)
                {
                    return RedirectToAction("Details", new { name = portfolioToUpdate.Name });
                }

                return Redirect(returnUrl);
            }

            var portfolioToLoad = await _context.Portfolios
                                .Include(m => m.Photos)
                                    .ThenInclude(m => m.Photo)
                                .Include(m => m.FeaturedPhoto)
                                .AsNoTracking()
                                .SingleOrDefaultAsync(m => m.Id == Id);

            PopulateFeaturedPhotosDropDownList(portfolioToLoad.FeaturedPhotoId);
            PopulatePhotoData(portfolioToLoad);
            return View(portfolio);
        }

        [Authorize]
        private void UpdatePhotos(string[] selectedPhotos, string[] photoIds, string[] selectedPhotoPositions, string[] selectedPhotoWidths, Portfolio modelToUpdate)
        {
            var photoPositionsDict = photoIds.Zip(selectedPhotoPositions, (id, position) => new { id, position });
            var photoWidthsDict = photoIds.Zip(selectedPhotoWidths, (id, width) => new { id, width });

            modelToUpdate.Photos = new List<PortfolioPhoto>();

            var selectedPhotosHS = new HashSet<string>(selectedPhotos);
            var photos = new HashSet<int>(modelToUpdate.Photos.Select(m => m.Photo.Id));

            foreach (var photo in _context.Photos)
            {
                if (selectedPhotosHS.Contains(photo.Id.ToString()))
                {
                    if (!photos.Contains(photo.Id))
                    {
                        modelToUpdate.Photos.Add(new PortfolioPhoto { PhotoId = photo.Id, PortfolioId = modelToUpdate.Id });
                    }

                    if (photoPositionsDict.Any(pp => pp.id == photo.Id.ToString()))
                    {
                        var newPosition = photoPositionsDict.SingleOrDefault(ppd => ppd.id == photo.Id.ToString()).position;

                        if (newPosition != null)
                        {
                            modelToUpdate.Photos.SingleOrDefault(pp => pp.PhotoId == photo.Id).Position = int.Parse(newPosition);
                        }
                        else
                        {
                            modelToUpdate.Photos.SingleOrDefault(pp => pp.PhotoId == photo.Id).Position = null;
                        }
                    }

                    if (photoWidthsDict.Any(pp => pp.id == photo.Id.ToString()))
                    {
                        var newWidth = photoWidthsDict.SingleOrDefault(ppd => ppd.id == photo.Id.ToString()).width;

                        if (newWidth != null)
                        {
                            modelToUpdate.Photos.SingleOrDefault(pp => pp.PhotoId == photo.Id).Width = int.Parse(newWidth);
                        }
                        else
                        {
                            modelToUpdate.Photos.SingleOrDefault(pp => pp.PhotoId == photo.Id).Width = null;
                        }
                    }
                }
                else
                {
                    if (photos.Contains(photo.Id))
                    {
                        var photoToRemove = modelToUpdate.Photos.SingleOrDefault(m => m.Photo.Id == photo.Id);
                        _context.Remove(photoToRemove);
                    }
                }
            }
        }

        [Authorize]
        private void UpdateFeaturedPortfolios(Portfolio modelToUpdate)
        {
            var featuredPortfolioToSet = _context.FeaturedPortfolios.SingleOrDefault(fp => fp.Portfolio.Id == modelToUpdate.Id);

            if (modelToUpdate.Featured)
            {
                if (featuredPortfolioToSet == null)
                {
                    _context.FeaturedPortfolios.Add(new FeaturedPortfolio { PortfolioId = modelToUpdate.Id });
                }
            }
            else
            {
                if (featuredPortfolioToSet != null)
                {
                    var featuredPortfolioToRemove = _context.FeaturedPortfolios.SingleOrDefault(fp => fp.Portfolio.Id == modelToUpdate.Id);
                    _context.Remove(featuredPortfolioToRemove);
                }
            }
        }

        // GET: Portfolios/5/Delete
        [Route("{Name}/Delete")]
        [Authorize]
        public async Task<IActionResult> Delete(string Name)
        {
            if (Name.Length == 0)
            {
                return NotFound();
            }


            var portfolio = await _context.Portfolios
                .Include(m => m.Photos)
                    .ThenInclude(m => m.Photo)
                .Include(m => m.FeaturedPhoto)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.Name == Name);
            if (portfolio == null)
            {
                return NotFound();
            }

            portfolio.UnencryptedPassword = Encryption.Decrypt(_passwordProtector, portfolio.PasswordHash);

            return View(portfolio);
        }

        // POST: Portfolios/5/Delete
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        [Route("{Name}/Delete")]
        public async Task<IActionResult> DeleteConfirmed(string Name)
        {
            var portfolio = await _context.Portfolios.SingleOrDefaultAsync(pr => pr.Name == Name);
            _context.Portfolios.Remove(portfolio);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PortfolioExists(int Id)
        {
            return _context.Portfolios.Any(e => e.Id == Id);
        }

        private string StripScriptTags(string str)
        {
            Regex regex = new Regex(@"<script[^>]*>[\s\S]*?</script>");

            if (String.IsNullOrEmpty(str))
            {
                return str;
            }

            return regex.Replace(str, "");


        }
    }
}
