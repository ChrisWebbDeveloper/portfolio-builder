using FluentValidation;
using PortfolioBuilder.Data;
using PortfolioBuilder.Models;
using System.Linq;

namespace PortfolioBuilder.ModelValidators
{
    public class PortfolioValidator : AbstractValidator<Portfolio>
    {
        private readonly ApplicationDbContext _context;

        public PortfolioValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(pr => pr.Name).NotNull()
                                  .MaximumLength(200)
                                  .Must((portfolio, name) => UniqueName(portfolio.Id, name)).WithMessage("'{PropertyName}' must be unique");
            RuleFor(pr => pr.Description).MaximumLength(10000);
            RuleFor(pr => pr.Private).NotNull();
            RuleFor(pr => pr.Published).NotNull();
            RuleFor(pr => pr.Featured).NotNull();
            RuleFor(pr => pr.Width).InclusiveBetween(1, 12);
        }

        protected bool UniqueName(int id, string name)
        {
            var nameAlreadyExists = _context.Portfolios
                                    .Where(pr => pr.Name.ToLower() == name.ToLower())
                                    .Where(pr => pr.Id != id)
                                    .FirstOrDefault();

            if (nameAlreadyExists != null)
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
