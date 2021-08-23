using FluentValidation;
using PortfolioBuilder.Data;
using PortfolioBuilder.Models;

namespace PortfolioBuilder.ModelValidators
{
    public class CarouselPhotoValidator : AbstractValidator<CarouselPhoto>
    {
        private readonly ApplicationDbContext _context;

        public CarouselPhotoValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(cp => cp.PhotoId).NotNull();
            RuleFor(cp => cp.ShowName).NotNull();
            RuleFor(cp => cp.ShowDescription).NotNull();
        }
    }
}
