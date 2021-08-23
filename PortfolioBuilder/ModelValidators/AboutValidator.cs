using FluentValidation;
using PortfolioBuilder.Models;

namespace PortfolioBuilder.ModelValidators
{
    public class AboutValidator : AbstractValidator<About>
    {
        public AboutValidator()
        {
            RuleFor(ab => ab.Title).MaximumLength(800);
            RuleFor(ab => ab.Description).MaximumLength(10000);
        }
    }
}
