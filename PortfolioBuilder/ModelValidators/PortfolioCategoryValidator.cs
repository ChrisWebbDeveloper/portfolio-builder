using FluentValidation;
using PortfolioBuilder.Data;
using PortfolioBuilder.Models;
using System.Linq;

namespace PortfolioBuilder.ModelValidators
{
    public class PortfolioCategoryValidator : AbstractValidator<PortfolioCategory>
    {
        private readonly ApplicationDbContext _context;

        public PortfolioCategoryValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(pc => pc.PortfolioId).NotNull();
            RuleFor(pc => new { pc.PortfolioId, pc.CategoryId }).Must((portfolioCategory, portfolioCategoryItem) => UniquePortfolioCategory(portfolioCategory.Id, portfolioCategoryItem.PortfolioId, portfolioCategoryItem.CategoryId)).WithMessage("The Portfolio is already present in the Category");
            RuleFor(pc => pc.CategoryId).NotNull();
            RuleFor(pc => pc.Width).InclusiveBetween(1, 12);
        }

        protected bool UniquePortfolioCategory(int id, int portfolioId, int categoryId)
        {
            var portfolioCategoryAlreadyExists = _context.PortfolioCategories
                                              .Where(pc => pc.PortfolioId == portfolioId)
                                              .Where(pc => pc.CategoryId == categoryId)
                                              .Where(pc => pc.Id != id)
                                              .FirstOrDefault();

            if (portfolioCategoryAlreadyExists != null)
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
