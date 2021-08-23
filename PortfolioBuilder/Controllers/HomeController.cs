using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PortfolioBuilder.Models;
using Microsoft.AspNetCore.Authorization;
using PortfolioBuilder.Data;
using Microsoft.EntityFrameworkCore;
using PortfolioBuilder.ViewModels;

namespace PortfolioBuilder.Controllers
{
    public class HomeController : ApplicationController
    {
        public HomeController(ApplicationDbContext context) : base(context)
        {

        }

        [Route("")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = new HomeViewModel();

            applicationDbContext.CarouselPhotos = await _context.CarouselPhotos
                .OrderBy(m => m.Position)
                .Include(m => m.Photo)
                .AsNoTracking()
                .ToListAsync();

            applicationDbContext.FeaturedPortfolios = await _context.FeaturedPortfolios
                .OrderBy(m => m.Position)
                .Include(m => m.Portfolio)
                .AsNoTracking()
                .ToListAsync();

            return View(applicationDbContext);
        }


        // GET: Portfolios
        [Authorize]
        [Route("/Home/Edit")]
        public async Task<IActionResult> Edit()
        {
            var carouselPhotos = await _context.CarouselPhotos
                .OrderBy(m => m.Position)
                .Include(m => m.Photo)
                .AsNoTracking()
                .ToListAsync();

            var portfolios = await _context.Portfolios
                .OrderBy(pr => pr.Id)
                .AsNoTracking()
                .ToListAsync();

            PopulatePhotoData(carouselPhotos);
            PopulatePortfolioData(portfolios);

            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [Route("/Home/Edit")]
        public async Task<IActionResult> Edit(string[] selectedPhotos, string[] photoIds, string[] selectedPhotoPositions, string[] selectedPortfolios, string[] portfolioIds, string[] selectedFeatPortPositions)
        {
            if (ModelState.IsValid)
            {
                //Update carousel photos
                var carouselPhotosToUpdate = await _context.CarouselPhotos
                    .Include(cp => cp.Photo)
                    .ToListAsync();

                UpdatePhotos(selectedPhotos, photoIds, selectedPhotoPositions, carouselPhotosToUpdate);

                foreach (var carouselPhoto in carouselPhotosToUpdate)
                {
                    _context.Update(carouselPhoto);
                };

                //Update featured portfolios
                var featuredPortfoliosToUpdate = await _context.FeaturedPortfolios
                    .Include(fp => fp.Portfolio)
                    .ToListAsync();

                UpdateFeaturedPortfolios(selectedPortfolios, portfolioIds, selectedFeatPortPositions, featuredPortfoliosToUpdate);

                foreach (var featuredPortfolio in featuredPortfoliosToUpdate)
                {
                    _context.Update(featuredPortfolio);
                };

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                return RedirectToAction("Index");

            };

            return View();
        }

        [Authorize]
        private void PopulatePhotoData(List<CarouselPhoto> photosContext = null)
        {
            var allPhotos = _context.Photos.OrderByDescending(ph => ph.Id);
            HashSet<int> photosIncluded = new HashSet<int>();

            if (photosContext != null)
            {
                if (photosContext.Count() > 0)
                {
                    photosIncluded = new HashSet<int>(photosContext.Select(cp => cp.PhotoId));
                }
            }

            var viewModel = new List<PhotoDataViewModel>();

            foreach (var photo in allPhotos)
            {
                int? position = null;

                if (photosContext != null)
                {
                    if (photosContext.Count() > 0)
                    {
                        if (photosContext.Any(cp => cp.PhotoId == photo.Id))
                        {
                            position = photosContext.SingleOrDefault(pp => pp.PhotoId == photo.Id).Position;
                        }
                    }
                }

                viewModel.Add(new PhotoDataViewModel
                {
                    PhotoId = photo.Id,
                    Name = photo.Name,
                    FilePath = photo.FilePath,
                    Selected = photosIncluded.Contains(photo.Id),
                    Position = position
                });
            }

            ViewData["PhotosList"] = viewModel;
            ViewData["AllPhotos"] = allPhotos;
        }

        [Authorize]
        private void UpdatePhotos(string[] selectedPhotos, string[] photoIds, string[] selectedPhotoPositions, List<CarouselPhoto> carouselPhotosToUpdate)
        {
            var photoPositionsDict = photoIds.Zip(selectedPhotoPositions, (id, position) => new { id, position });

            var selectedPhotosHS = new HashSet<string>(selectedPhotos);
            var photos = new HashSet<int>(carouselPhotosToUpdate.Select(m => m.Photo.Id));

            foreach (var photo in _context.Photos)
            {
                if (selectedPhotosHS.Contains(photo.Id.ToString()))
                {

                    if (!photos.Contains(photo.Id))
                    {
                        carouselPhotosToUpdate.Add(new CarouselPhoto { Photo = photo });
                    }

                    if (photoPositionsDict.Any(pp => pp.id == photo.Id.ToString()))
                    {
                        var newPosition = photoPositionsDict.SingleOrDefault(ppd => ppd.id == photo.Id.ToString()).position;

                        if (newPosition != null)
                        {
                            carouselPhotosToUpdate.SingleOrDefault(pp => pp.Photo == photo).Position = int.Parse(newPosition);
                        }
                        else
                        {
                            carouselPhotosToUpdate.SingleOrDefault(pp => pp.Photo == photo).Position = null;
                        }
                    }
                }
                else
                {
                    if (photos.Contains(photo.Id))
                    {
                        var photoToRemove = carouselPhotosToUpdate.SingleOrDefault(m => m.Photo == photo);

                        carouselPhotosToUpdate.Remove(photoToRemove);
                        _context.Remove(photoToRemove);
                    }
                }
            }
        }

        [Authorize]
        private void PopulatePortfolioData(List<Portfolio> portfoliosContext = null)
        {
            var viewModel = new List<PortfolioDataViewModel>();

            foreach (var portfolio in portfoliosContext)
            {
                int? position = null;

                if (portfolio.Featured == true)
                {
                    var featuredPort = _context.FeaturedPortfolios.SingleOrDefault(fp => fp.Portfolio == portfolio);
                    position = featuredPort.Position;
                }

                viewModel.Add(new PortfolioDataViewModel
                {
                    PortfolioId = portfolio.Id,
                    Name = portfolio.Name,
                    Position = position,
                    Featured = portfolio.Featured
                });
            }

            ViewData["PortfoliosList"] = viewModel;
        }

        [Authorize]
        private void UpdateFeaturedPortfolios(string[] selectedPortfolios, string[] portfolioIds, string[] selectedFeatPortPositions, List<FeaturedPortfolio> featuredPortfoliosToUpdate)
        {
            var portfolioPositionsDict = portfolioIds.Zip(selectedFeatPortPositions, (id, position) => new { id, position });

            var selectedPortfoliosHS = new HashSet<string>(selectedPortfolios);
            var portfolios = new HashSet<int>(featuredPortfoliosToUpdate.Select(m => m.Portfolio.Id));

            foreach (var portfolio in _context.Portfolios)
            {
                if (selectedPortfoliosHS.Contains(portfolio.Id.ToString()))
                {

                    if (!portfolios.Contains(portfolio.Id))
                    {
                        featuredPortfoliosToUpdate.Add(new FeaturedPortfolio { Portfolio = portfolio });

                        portfolio.Featured = true;
                        _context.Update(portfolio);
                    }

                    if (portfolioPositionsDict.Any(pp => pp.id == portfolio.Id.ToString()))
                    {
                        var newPosition = portfolioPositionsDict.SingleOrDefault(ppd => ppd.id == portfolio.Id.ToString()).position;

                        if (newPosition != null)
                        {
                            featuredPortfoliosToUpdate.SingleOrDefault(fp => fp.Portfolio == portfolio).Position = int.Parse(newPosition);
                        }
                        else
                        {
                            featuredPortfoliosToUpdate.SingleOrDefault(fp => fp.Portfolio == portfolio).Position = null;
                        }
                    }
                }
                else
                {
                    if (portfolios.Contains(portfolio.Id))
                    {
                        var photoToRemove = featuredPortfoliosToUpdate.SingleOrDefault(m => m.Portfolio == portfolio);

                        featuredPortfoliosToUpdate.Remove(photoToRemove);
                        _context.Remove(photoToRemove);

                        portfolio.Featured = false;
                        _context.Update(portfolio);

                    }
                }
            }
        }
    }
}
