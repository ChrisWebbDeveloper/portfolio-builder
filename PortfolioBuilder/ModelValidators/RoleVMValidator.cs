using FluentValidation;
using PortfolioBuilder.ViewModels;

namespace PortfolioBuilder.ModelValidators
{
    public class RoleVMValidator : AbstractValidator<RoleViewModel>
    {
        public RoleVMValidator()
        {
            RuleFor(rl => rl.RoleName).NotNull()
                      .MaximumLength(200);
        }
    }
}
