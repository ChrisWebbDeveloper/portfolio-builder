using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortfolioBuilder.Data;
using PortfolioBuilder.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using PortfolioBuilder.ViewModels;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System;
using ImageMagick;

namespace PortfolioBuilder.Controllers
{
    [Authorize]
    [Route("Photos")]
    public class PhotosController : ApplicationController
    {
        private readonly IWebHostEnvironment hostingEnvironment;

        public PhotosController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment) : base(context)
        {
            this.hostingEnvironment = hostingEnvironment;
        }

        // GET: Photos
        public async Task<IActionResult> Index()
        {
            return View(await _context.Photos.OrderByDescending(ph => ph.Id).ToListAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("UploadPhotos")]
        public async Task<IActionResult> UploadPhotos(List<IFormFile> photosToUpload)
        {
            var notUploaded = new List<string>();

            foreach (IFormFile photo in photosToUpload)
            {
                if (FileIsImage(photo.FileName))
                {
                    string uniqueFileName = UniqueFileName(photo.FileName);
                    UploadFile(photo, uniqueFileName);

                    Photo photoToAdd = new Photo
                    {
                        FilePath = uniqueFileName
                    };

                    _context.Add(photoToAdd);
                }
                else
                {
                    notUploaded.Add(photo.FileName);
                }
            }

            TempData["NotUploaded"] = notUploaded.ToArray();

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private string UniqueFileName(string fileName)
        {
            return Guid.NewGuid().ToString() + "_" + fileName;
        }

        private bool FileIsImage(string fileName)
        {
            string[] permittedExtensions = { ".jpeg", ".jpg", ".png", ".bmp" };

            var ext = Path.GetExtension(fileName).ToLowerInvariant();

            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void UploadFile(IFormFile photo, string fileName)
        {
            string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
            string filePath = Path.Combine(uploadsFolder, fileName);

            var stream = new FileStream(filePath, FileMode.Create);
            photo.CopyTo(stream);
            stream.Close();

            using (MagickImage image = new MagickImage(filePath))
            {
                image.Strip();

                var maxDimension = 1920;
                var imgQuality = 85;
                var imgHeight = image.Height;
                var imgWidth = image.Width;

                image.Quality = imgQuality;

                if (imgHeight >= imgWidth && imgHeight > maxDimension)
                {
                    image.Resize(0, maxDimension);
                }
                else if (imgHeight <= imgWidth && imgWidth > maxDimension)
                {
                    image.Resize(maxDimension, 0);
                }

                image.Write(filePath);
            }
        }

        private void RemoveFile(Photo photo)
        {
            string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
            string fileName = photo.FilePath;
            string filePath = Path.Combine(uploadsFolder, fileName);

            GC.Collect();
            GC.WaitForPendingFinalizers();
            System.IO.File.Delete(filePath);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [Route("GeneralDelete")]
        public async Task<IActionResult> GeneralDelete(int[] itemsToRemove)
        {
            var photos = _context.Photos.Where(ph => itemsToRemove.Contains(ph.Id)).ToList();

            foreach (Photo photo in photos)
            {
                var portfoliosWithFeaturedPhoto = _context.Portfolios.Where(pr => pr.FeaturedPhoto == photo).ToList();

                foreach (Portfolio portfolio in portfoliosWithFeaturedPhoto)
                {
                    portfolio.FeaturedPhoto = null;

                    _context.Update(portfolio);
                }

                RemoveFile(photo);

                _context.Photos.Remove(photo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        // GET: Photos/5
        [Route("{Id}")]
        public async Task<IActionResult> Details(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var photo = await _context.Photos
                .Include(ph => ph.Portfolios)
                    .ThenInclude(pp => pp.Portfolio)
                .Include(ph => ph.FeaturedInPortfolios)
                .Include(ph => ph.FeaturedInCategories)
                .Include(ph => ph.FeaturedInHomeCarousel)
                .Include(ph => ph.FeaturedInAbout)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.Id == Id);
            if (photo == null)
            {
                return NotFound();
            }

            return View(photo);
        }

        // GET: Photos/Create
        [Route("Add")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Photos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Add")]
        public async Task<IActionResult> Create(UploadPhotosViewModel photoVM)
        {
            if (ModelState.IsValid)
            {
                if (FileIsImage(photoVM.Photo.FileName))
                {
                    string uniqueFileName = null;
                    if (photoVM.Photo != null)
                    {
                        uniqueFileName = UniqueFileName(photoVM.Photo.FileName);
                        UploadFile(photoVM.Photo, uniqueFileName);
                    }

                    Photo photo = new Photo
                    {
                        Name = photoVM.Name,
                        Description = photoVM.Description,
                        FilePath = uniqueFileName
                    };

                    _context.Add(photo);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["NotUploaded"] = photoVM.Photo.FileName;
                }
            }

            return View(photoVM);
        }

        // GET: Photos/5/Edit
        [Route("{Id}/Edit")]
        public async Task<IActionResult> Edit(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var photo = await _context.Photos.FindAsync(Id);
            if (photo == null)
            {
                return NotFound();
            }

            ViewBag.ReturnUrl = Request.Headers["Referer"].ToString();

            return View(photo);
        }

        // POST: Photos/5/Edit
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("{Id}/Edit")]
        public async Task<IActionResult> Edit(int Id, [Bind("Id,Name,Description,FilePath")] Photo photo, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            if (Id != photo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(photo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhotoExists(photo.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Redirect(returnUrl);

            }
            return View(photo);
        }

        // GET: Photos/5/Delete
        [Route("{Id}/Delete")]
        public async Task<IActionResult> Delete(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var photo = await _context.Photos
                .Include(ph => ph.Portfolios)
                    .ThenInclude(pp => pp.Portfolio)
                .Include(ph => ph.FeaturedInPortfolios)
                .Include(ph => ph.FeaturedInCategories)
                .Include(ph => ph.FeaturedInHomeCarousel)
                .Include(ph => ph.FeaturedInAbout)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.Id == Id);
            if (photo == null)
            {
                return NotFound();
            }

            return View(photo);
        }

        // POST: Photos/5/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("{Id}/Delete")]
        public async Task<IActionResult> DeleteConfirmed(int Id)
        {
            var photo = await _context.Photos.FindAsync(Id);
            RemoveFile(photo);
            _context.Photos.Remove(photo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PhotoExists(int Id)
        {
            return _context.Photos.Any(e => e.Id == Id);
        }
    }
}
