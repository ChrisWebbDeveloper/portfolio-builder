using FluentValidation;
using PortfolioBuilder.ViewModels;

namespace PortfolioBuilder.ModelValidators
{
    public class UserVMValidator : AbstractValidator<UserViewModel>
    {
        public UserVMValidator()
        {
            RuleFor(usr => usr.Id).NotNull();

            RuleFor(usr => usr.Email).NotNull()
                      .EmailAddress();
        }
    }
}
