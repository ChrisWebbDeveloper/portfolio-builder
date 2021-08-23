using FluentValidation;
using PortfolioBuilder.Models;

namespace PortfolioBuilder.ModelValidators
{
    public class ContactValidator : AbstractValidator<Contact>
    {
        public ContactValidator()
        {
            RuleFor(cd => cd.Message).MaximumLength(2000);
            RuleFor(cd => cd.Name).MaximumLength(200);
            RuleFor(cd => cd.Location).MaximumLength(200);
            RuleFor(cd => cd.Email).EmailAddress();
            RuleFor(cd => cd.PhoneNo).PhoneNo();
            RuleFor(cd => cd.Facebook).MaximumLength(200);
            RuleFor(cd => cd.Instagram).MaximumLength(200);
            RuleFor(cd => cd.LinkedIn).MaximumLength(200);
        }
    }
}
