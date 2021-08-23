using FluentValidation;
using PortfolioBuilder.Data;
using PortfolioBuilder.Models;
using System.Linq;

namespace PortfolioBuilder.ModelValidators
{
    public class PortfolioPhotoValidator : AbstractValidator<PortfolioPhoto>
    {
        private readonly ApplicationDbContext _context;

        public PortfolioPhotoValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(pp => pp.PortfolioId).NotNull();
            RuleFor(pp => new { pp.PortfolioId, pp.PhotoId }).Must((portfolioPhoto, portfolioPhotoItem) => UniquePortfolioPhoto(portfolioPhoto.Id, portfolioPhotoItem.PortfolioId, portfolioPhotoItem.PhotoId)).WithMessage("The Photo is already present in the Portfolio");
            RuleFor(pp => pp.PhotoId).NotNull();
            RuleFor(pp => pp.Width).InclusiveBetween(1, 12);
        }

        protected bool UniquePortfolioPhoto(int id, int portfolioId, int photoId)
        {
            var portfolioPhotoAlreadyExists = _context.PortfolioPhotos
                                              .Where(pp => pp.PortfolioId == portfolioId)
                                              .Where(pp => pp.PhotoId == photoId)
                                              .Where(pp => pp.Id != id)
                                              .FirstOrDefault();

            if (portfolioPhotoAlreadyExists != null)
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
