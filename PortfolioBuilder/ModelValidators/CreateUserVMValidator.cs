using FluentValidation;
using PortfolioBuilder.ViewModels;

namespace PortfolioBuilder.ModelValidators
{
    public class CreateUserVMValidator : AbstractValidator<CreateUserViewModel>
    {
        public CreateUserVMValidator()
        {
            RuleFor(usr => usr.Password).NotNull();

            RuleFor(usr => usr.ConfirmPassword).NotNull()
                .Equal(x => x.Password)
                .WithMessage("Passwords do not match");
        }
    }
}
