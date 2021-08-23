using FluentValidation;
using PortfolioBuilder.Data;
using PortfolioBuilder.Models;
using System.Linq;

namespace PortfolioBuilder.ModelValidators
{
    public class AboutPhotoValidator : AbstractValidator<AboutPhoto>
    {
        private readonly ApplicationDbContext _context;

        public AboutPhotoValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(ap => ap.AboutId).NotNull();
            RuleFor(ap => ap.PhotoId).NotNull()
                                     .Must((photo, photoId) => UniqueAboutPhoto(photo.Id, photoId)).WithMessage("'{PropertyName}' must be unique");
        }
        protected bool UniqueAboutPhoto(int id, int photoId)
        {
            var featuredPortfolioAlreadyExists = _context.AboutPhotos
                                                 .Where(ap => ap.PhotoId == photoId)
                                                 .Where(ap => ap.Id != id)
                                                 .FirstOrDefault();

            if (featuredPortfolioAlreadyExists != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
