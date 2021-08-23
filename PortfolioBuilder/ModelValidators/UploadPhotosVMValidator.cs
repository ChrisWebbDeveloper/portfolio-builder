using FluentValidation;
using PortfolioBuilder.ViewModels;

namespace PortfolioBuilder.ModelValidators
{
    public class UploadPhotosVMValidator : AbstractValidator<UploadPhotosViewModel>
    {
        public UploadPhotosVMValidator()
        {
            RuleFor(up => up.Photo).NotNull();
        }
    }
}
