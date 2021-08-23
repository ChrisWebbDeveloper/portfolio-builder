using FluentValidation;
using PortfolioBuilder.Data;
using PortfolioBuilder.Models;
using System;
using System.Linq;

namespace PortfolioBuilder.ModelValidators
{
    public class PhotoValidator : AbstractValidator<Photo>
    {
        private readonly ApplicationDbContext _context;

        public PhotoValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(ph => ph.Name).MaximumLength(200)
                                  .Must((photo, name) => UniqueName(photo.Id, name)).WithMessage("'{PropertyName}' must be unique");
            RuleFor(ph => ph.Description).MaximumLength(10000);
            RuleFor(ph => ph.FilePath).NotNull()
                                      .Must((photo, filePath) => UniqueFilePath(photo.Id, filePath)).WithMessage("'{PropertyName}' must be unique");
        }

        protected bool UniqueName(int id, string name)
        {
            Photo nameAlreadyExists;

            if (!String.IsNullOrEmpty(name))
            {
                nameAlreadyExists = _context.Photos
                                    .Where(ph => ph.Name.ToLower() == name)
                                    .Where(ph => ph.Id != id)
                                    .FirstOrDefault();
            }
            else
            {
                nameAlreadyExists = null;
            }

            if (nameAlreadyExists != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        protected bool UniqueFilePath(int id, string filePath)
        {
            var filePathAlreadyExists = _context.Photos
                                        .Where(ph => ph.FilePath == filePath)
                                        .Where(ph => ph.Id != id)
                                        .FirstOrDefault();

            if (filePathAlreadyExists != null)
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
