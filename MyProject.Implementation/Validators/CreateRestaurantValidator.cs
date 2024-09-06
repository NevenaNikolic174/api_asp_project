using FluentValidation;
using MyProject.Api.DTO;
using MyProject.Application.DTO;
using MyProject.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Implementation.Validators
{
    public class CreateRestaurantValidator : AbstractValidator<RestaurantDTO>
    {
        private readonly MyDBContext _dbCon;

        public CreateRestaurantValidator(MyDBContext dbCon)
        {
            _dbCon = dbCon;

            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Name)
               .NotEmpty().WithMessage("Restaurant name is required.")
               .MinimumLength(3).WithMessage("Restaurant name must be at least 3 characters long.")
               .MaximumLength(50);

            RuleFor(x => x.Image)
              .Must(HaveValidImageExtension).WithMessage("Image must be a valid file with extensions jpg, jpeg, png, or gif.")
              .When(x => !string.IsNullOrEmpty(x.Image));

        }

        private bool HaveValidImageExtension(string image)
        {
            var validExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var extension = System.IO.Path.GetExtension(image).ToLower();
            return validExtensions.Contains(extension);
        }
    }
}
