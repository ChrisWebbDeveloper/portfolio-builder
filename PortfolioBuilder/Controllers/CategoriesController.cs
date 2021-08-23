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
using Microsoft.AspNetCore.Authorization;

namespace PortfolioBuilder.Controllers
{
    [Route("Portfolios/Category")]
    public class CategoriesController : ApplicationController
    {
        public CategoriesController(ApplicationDbContext context) : base(context)
        {

        }

        [Route("All")]
        public async Task<IActionResult> Index()
        {
            bool isAuthenticated = User.Identity.IsAuthenticated;

            if (isAuthenticated)
            {
                var applicationDbContext = _context.Categories
                    .Include(m => m.FeaturedPhoto)
                    .OrderBy(m => m.Position)
                    .AsNoTracking();
                return View(await applicationDbContext.ToListAsync());
            }
            else
            {
                var applicationDbContext = _context.Categories
                    .Include(m => m.FeaturedPhoto)
                    .OrderBy(m => m.Position)
                    .Where(m => m.Published == true)
                    .AsNoTracking();
                return View(await applicationDbContext.ToListAsync());
            }
        }

        [Authorize]
        [Route("All/Edit")]
        public async Task<IActionResult> IndexEdit()
        {
            var applicationDbContext = _context.Categories
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
            var categories = _context.Categories.Where(ct => itemsToRemove.Contains(ct.Id)).ToList();

            foreach (Category category in categories)
            {
                _context.Categories.Remove(category);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [Route("GeneralEdit")]
        public async Task<IActionResult> GeneralEdit(string[] categoryIds, string[] categoryPositions, string[] categoryWidths)
        {
            if (ModelState.IsValid)
            {
                for (int i = 0; i < categoryIds.Length; i++)
                {
                    var id = int.Parse(categoryIds[i]);
                    var position = int.Parse(categoryPositions[i]);
                    var width = int.Parse(categoryWidths[i]);

                    var categoryToUpdate = _context.Categories.FirstOrDefault(ct => ct.Id == id);

                    categoryToUpdate.Position = position;
                    categoryToUpdate.Width = width;

                    _context.Categories.Update(categoryToUpdate);
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

        [Route("{Name}")]
        public async Task<IActionResult> Details(string Name)
        {
            if (Name.Length == 0)
            {
                return NotFound();
            }

            bool isAuthenticated = User.Identity.IsAuthenticated;

            Category category;

            if (isAuthenticated)
            {
                category = await _context.Categories
                .Include(m => m.Portfolios)
                    .ThenInclude(m => m.Portfolio)
                        .ThenInclude(pr => pr.FeaturedPhoto)
                .Include(m => m.FeaturedPhoto)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.Name == Name);

                if (category == null)
                {
                    return NotFound();
                }

                category.Portfolios = category.Portfolios.OrderBy(m => m.Position).ToList();
            }
            else
            {
                category = await _context.Categories
                .Include(m => m.Portfolios)
                    .ThenInclude(m => m.Portfolio)
                        .ThenInclude(pr => pr.FeaturedPhoto)
                .Include(m => m.FeaturedPhoto)
                .Where(m => m.Published == true)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.Name == Name);

                if (category == null)
                {
                    return NotFound();
                }

                category.Portfolios = category.Portfolios.Where(m => m.Portfolio.Published == true).OrderBy(m => m.Position).ToList();
            }

            return View(category);
        }

        [Authorize]
        [Route("Add")]
        public IActionResult Create()
        {
            PopulateFeaturedPhotosDropDownList();
            PopulatePortfolioData();
            ViewData["AllPhotos"] = _context.Photos.OrderByDescending(pr => pr.Id);

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

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [Route("Add")]
        public async Task<IActionResult> Create([Bind("Id,Name,Published,FeaturedPhotoId,Position,Width")] Category category, string[] selectedPortfolios, string[] portfolioIds, string[] selectedPortfolioPositions, string[] selectedPortfolioWidths)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();

                UpdatePortfolios(selectedPortfolios, portfolioIds, selectedPortfolioPositions, selectedPortfolioWidths, category);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            PopulateFeaturedPhotosDropDownList(category.FeaturedPhotoId);
            PopulatePortfolioData(category);
            ViewData["AllPhotos"] = _context.Photos.OrderByDescending(pr => pr.Id);

            return View(category);
        }

        [Authorize]
        [Route("{Name}/Edit")]
        public async Task<IActionResult> Edit(string Name)
        {
            if (Name.Length == 0)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Include(m => m.Portfolios)
                    .ThenInclude(m => m.Portfolio)
                        .ThenInclude(pr => pr.FeaturedPhoto)
                .Include(m => m.FeaturedPhoto)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.Name == Name);

            if (category == null)
            {
                return NotFound();
            }

            ViewBag.ReturnUrl = Request.Headers["Referer"].ToString();

            PopulateFeaturedPhotosDropDownList(category.FeaturedPhotoId);
            PopulatePortfolioData(category);
            ViewData["AllPhotos"] = _context.Photos.OrderByDescending(pr => pr.Id);

            return View(category);
        }

        [Authorize]
        private void PopulatePortfolioData(Category portfoliosContext = null)
        {
            var allPortfolios = _context.Portfolios
                                .Include(m => m.FeaturedPhoto)
                                .OrderByDescending(pr => pr.Id);

            HashSet<int> portfoliosIncluded = new HashSet<int>();

            if (portfoliosContext != null)
            {
                if (portfoliosContext.Portfolios != null)
                {
                    portfoliosIncluded = new HashSet<int>(portfoliosContext.Portfolios.Select(pc => pc.PortfolioId));
                }
            }

            var viewModel = new List<CategoryPortfolioDataViewModel>();

            foreach (var portfolio in allPortfolios)
            {
                int? position = null;
                int? width = null;

                if (portfoliosContext != null)
                {
                    if (portfoliosContext.Portfolios != null)
                    {
                        if (portfoliosContext.Portfolios.Any(pc => pc.PortfolioId == portfolio.Id))
                        {
                            position = portfoliosContext.Portfolios.SingleOrDefault(pc => pc.PortfolioId == portfolio.Id).Position;
                            width = portfoliosContext.Portfolios.SingleOrDefault(pc => pc.PortfolioId == portfolio.Id).Width;
                        }
                    }
                }

                viewModel.Add(new CategoryPortfolioDataViewModel
                {
                    PortfolioId = portfolio.Id,
                    Name = portfolio.Name,
                    FeaturedPhoto = portfolio.FeaturedPhoto,
                    Selected = portfoliosIncluded.Contains(portfolio.Id),
                    Width = width,
                    Position = position
                });
            }

            ViewData["PortfoliosList"] = viewModel;
            ViewData["AllPorfolios"] = allPortfolios;
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [Route("{Name}/Edit")]
        public async Task<IActionResult> Edit(int Id, [Bind("Id,Name,Published,FeaturedPhotoId,Position,Width")] Category category, string returnUrl, string[] selectedPortfolios, string[] portfolioIds, string[] selectedPortfolioPositions, string[] selectedPortfolioWidths)
        {
            ViewBag.ReturnUrl = returnUrl;

            if (Id != category.Id)
            {
                return NotFound();
            }

            if (returnUrl == "")
            {
                returnUrl = "Index";
            }

            if (ModelState.IsValid)
            {
                var categoryToUpdate = await _context.Categories
                    .Include(m => m.Portfolios)
                        .ThenInclude(m => m.Portfolio)
                            .ThenInclude(pr => pr.FeaturedPhoto)
                    .Include(m => m.FeaturedPhoto)
                    .SingleOrDefaultAsync(m => m.Id == Id);

                var lastOfReturnUrl = returnUrl.Split("/").Last();
                var originalName = categoryToUpdate.Name;

                if (await TryUpdateModelAsync<Category>(
                    categoryToUpdate,
                    "",
                    ct => ct.Id,
                    ct => ct.Name,
                    ct => ct.Published,
                    ct => ct.Position,
                    ct => ct.FeaturedPhotoId,
                    ct => ct.Width))
                {
                    UpdatePortfolios(selectedPortfolios, portfolioIds, selectedPortfolioPositions, selectedPortfolioWidths, categoryToUpdate);

                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!CategoryExists(category.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }

                    if (lastOfReturnUrl == originalName && lastOfReturnUrl != category.Name)
                    {
                        return RedirectToAction("Details", new { name = categoryToUpdate.Name });
                    }

                    return Redirect(returnUrl);
                }
                PopulateFeaturedPhotosDropDownList(category.FeaturedPhotoId);
                PopulatePortfolioData(categoryToUpdate);
                UpdatePortfolios(selectedPortfolios, portfolioIds, selectedPortfolioPositions, selectedPortfolioWidths, categoryToUpdate);
                ViewData["AllPhotos"] = _context.Photos.OrderByDescending(pr => pr.Id);

                if (lastOfReturnUrl == originalName && lastOfReturnUrl != category.Name)
                {
                    return RedirectToAction("Details", new { name = categoryToUpdate.Name });
                }

                return Redirect(returnUrl);
            }

            var categoryToLoad = await _context.Categories
                                .Include(m => m.Portfolios)
                                    .ThenInclude(m => m.Category)
                                .Include(m => m.FeaturedPhoto)
                                .AsNoTracking()
                                .SingleOrDefaultAsync(m => m.Id == Id);

            PopulateFeaturedPhotosDropDownList(categoryToLoad.FeaturedPhotoId);
            PopulatePortfolioData(categoryToLoad);
            ViewData["AllPhotos"] = _context.Photos.OrderByDescending(pr => pr.Id);
            return View(category);
        }

        [Authorize]
        private void UpdatePortfolios(string[] selectedPortfolios, string[] portfolioIds, string[] selectedPortfolioPositions, string[] selectedPortfolioWidths, Category modelToUpdate)
        {
            var portfolioPositionsDict = portfolioIds.Zip(selectedPortfolioPositions, (id, position) => new { id, position });
            var portfolioWidthsDict = portfolioIds.Zip(selectedPortfolioWidths, (id, width) => new { id, width });

            modelToUpdate.Portfolios = new List<PortfolioCategory>();

            var selectedPortfoliosHS = new HashSet<string>(selectedPortfolios);
            var portfolios = new HashSet<int>(modelToUpdate.Portfolios.Select(m => m.Portfolio.Id));

            foreach (var portfolio in _context.Portfolios)
            {
                if (selectedPortfoliosHS.Contains(portfolio.Id.ToString()))
                {
                    if (!portfolios.Contains(portfolio.Id))
                    {
                        modelToUpdate.Portfolios.Add(new PortfolioCategory { PortfolioId = portfolio.Id, CategoryId = modelToUpdate.Id });
                    }

                    if (portfolioPositionsDict.Any(ppd => ppd.id == portfolio.Id.ToString()))
                    {
                        var newPosition = portfolioPositionsDict.SingleOrDefault(ppd => ppd.id == portfolio.Id.ToString()).position;

                        if (newPosition != null)
                        {
                            modelToUpdate.Portfolios.SingleOrDefault(pp => pp.PortfolioId == portfolio.Id).Position = int.Parse(newPosition);
                        }
                        else
                        {
                            modelToUpdate.Portfolios.SingleOrDefault(pp => pp.PortfolioId == portfolio.Id).Position = null;
                        }
                    }

                    if (portfolioWidthsDict.Any(pp => pp.id == portfolio.Id.ToString()))
                    {
                        var newWidth = portfolioWidthsDict.SingleOrDefault(ppd => ppd.id == portfolio.Id.ToString()).width;

                        if (newWidth != null)
                        {
                            modelToUpdate.Portfolios.SingleOrDefault(pp => pp.PortfolioId == portfolio.Id).Width = int.Parse(newWidth);
                        }
                        else
                        {
                            modelToUpdate.Portfolios.SingleOrDefault(pp => pp.PortfolioId == portfolio.Id).Width = null;
                        }
                    }
                }
                else
                {
                    if (portfolios.Contains(portfolio.Id))
                    {
                        var portfolioToRemove = modelToUpdate.Portfolios.SingleOrDefault(m => m.Portfolio.Id == portfolio.Id);
                        _context.Remove(portfolioToRemove);
                    }
                }
            }
        }

        [Route("{Name}/Delete")]
        [Authorize]
        public async Task<IActionResult> Delete(string Name)
        {
            if (Name.Length == 0)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Include(m => m.Portfolios)
                    .ThenInclude(m => m.Portfolio)
                        .ThenInclude(pr => pr.FeaturedPhoto)
                .Include(m => m.FeaturedPhoto)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.Name == Name);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        [Route("{Name}/Delete")]
        public async Task<IActionResult> DeleteConfirmed(string Name)
        {
            var category = await _context.Categories.SingleOrDefaultAsync(ct => ct.Name == Name);
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int Id)
        {
            return _context.Categories.Any(ct => ct.Id == Id);
        }
    }
}
