using FluentValidation;
using PortfolioBuilder.Data;
using PortfolioBuilder.Models;
using System.Linq;

namespace PortfolioBuilder.ModelValidators
{
    public class FeaturedPortfolioValidator : AbstractValidator<FeaturedPortfolio>
    {
        private readonly ApplicationDbContext _context;

        public FeaturedPortfolioValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(fp => fp.PortfolioId).NotNull()
                                         .Must((featuredPortfolio, portfolioId) => UniquePortfolio(featuredPortfolio.Id, portfolioId)).WithMessage("'{PropertyName}' must be unique");
        }

        protected bool UniquePortfolio(int id, int portfolioId)
        {
            var featuredPortfolioAlreadyExists = _context.FeaturedPortfolios
                                                 .Where(fp => fp.PortfolioId == portfolioId)
                                                 .Where(fp => fp.Id != id)
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
