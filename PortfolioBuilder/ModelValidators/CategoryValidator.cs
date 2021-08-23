using FluentValidation;
using PortfolioBuilder.Data;
using PortfolioBuilder.Models;
using System.Linq;

namespace PortfolioBuilder.ModelValidators
{
    public class CategoryValidator : AbstractValidator<Category>
    {
        private readonly ApplicationDbContext _context;

        public CategoryValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(ct => ct.Name).NotNull()
                                  .MaximumLength(200)
                                  .Must((category, name) => UniqueName(category.Id, name)).WithMessage("'{PropertyName}' must be unique");
            RuleFor(ct => ct.Published).NotNull();
            RuleFor(ct => ct.Width).InclusiveBetween(1, 12);
        }

        protected bool UniqueName(int id, string name)
        {
            var nameAlreadyExists = _context.Categories
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
