using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortfolioBuilder.Data;
using PortfolioBuilder.Models;
using PortfolioBuilder.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace PortfolioBuilder.Controllers
{
    public class AboutController : ApplicationController
    {
        public AboutController(ApplicationDbContext context) : base(context)
        {

        }

        // GET: About
        public async Task<IActionResult> Index()
        {
            var about = await _context.AboutDetails
                .Include(a => a.Photos)
                    .ThenInclude(ap => ap.Photo)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.Id == 1);

            about.Photos = about.Photos.OrderBy(m => m.Position).ToList();

            return View(about);
        }

        // GET: About/Edit
        [Authorize]
        public async Task<IActionResult> Edit()
        {
            var about = await _context.AboutDetails
                .Include(a => a.Photos)
                    .ThenInclude(ap => ap.Photo)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.Id == 1);
            PopulatePhotoData(about);
            return View(about);
        }

        private void PopulatePhotoData(About photosContext)
        {
            var allPhotos = _context.Photos.OrderByDescending(model => model.Id);
            var photosIncluded = new HashSet<int>(photosContext.Photos.Select(ap => ap.PhotoId));
            var viewModel = new List<PhotoDataViewModel>();
            foreach (var photo in allPhotos)
            {
                int? position = null;

                if (photosContext.Photos.Any(ap => ap.PhotoId == photo.Id))
                {
                    position = photosContext.Photos.SingleOrDefault(ap => ap.PhotoId == photo.Id).Position;
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
        }

        // POST: About/Edit
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Title, Description")] About about, string[] selectedPhotos, string[] photoIds, string[] selectedPhotoPositions)
        {
            if (ModelState.IsValid)
            {
                var aboutToUpdate = await _context.AboutDetails
                    .Include(a => a.Photos)
                        .ThenInclude(ap => ap.Photo)
                    .SingleOrDefaultAsync(m => m.Id == 1);

                if (await TryUpdateModelAsync<About>(
                    aboutToUpdate,
                    "",
                    a => a.Title,
                    a => a.Description))
                {
                    UpdatePhotos(selectedPhotos, photoIds, selectedPhotoPositions, aboutToUpdate);
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
                UpdatePhotos(selectedPhotos, photoIds, selectedPhotoPositions, aboutToUpdate);
                PopulatePhotoData(aboutToUpdate);
                return RedirectToAction(nameof(Index));

            }
            return View(about);
        }

        [Authorize]
        private void UpdatePhotos(string[] selectedPhotos, string[] photoIds, string[] selectedPhotoPositions, About modelToUpdate)
        {
            if (ModelState.IsValid)
            {
                var photoPositionsDict = photoIds.Zip(selectedPhotoPositions, (id, position) => new { id, position });

                if (selectedPhotos == null)
                {
                    modelToUpdate.Photos = new List<AboutPhoto>();
                    return;
                }

                var selectedPhotosHS = new HashSet<string>(selectedPhotos);
                var photos = new HashSet<int>
                    (modelToUpdate.Photos.Select(m => m.Photo.Id));
                foreach (var photo in _context.Photos)
                {
                    if (selectedPhotosHS.Contains(photo.Id.ToString()))
                    {
                        if (!photos.Contains(photo.Id))
                        {
                            modelToUpdate.Photos.Add(new AboutPhoto { PhotoId = photo.Id, AboutId = 1 });
                        }

                        if (photoPositionsDict.Any(ap => ap.id == photo.Id.ToString()))
                        {
                            var newPosition = photoPositionsDict.SingleOrDefault(ppd => ppd.id == photo.Id.ToString()).position;

                            if (newPosition != null)
                            {
                                modelToUpdate.Photos.SingleOrDefault(ap => ap.PhotoId == photo.Id).Position = int.Parse(newPosition);
                            }
                            else
                            {
                                modelToUpdate.Photos.SingleOrDefault(ap => ap.PhotoId == photo.Id).Position = null;
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
        }
    }
}
