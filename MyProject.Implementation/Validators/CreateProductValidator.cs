using FluentValidation;
using Microsoft.AspNetCore.Http;
using MyProject.Api.DTO;
using MyProject.DataAccess;
using System.Linq;

namespace MyProject.Implementation.Validators
{
    public class CreateProductValidator : AbstractValidator<ProductDTO>
    {
        private readonly MyDBContext _dbCon;

        public CreateProductValidator(MyDBContext dbCon)
        {
            _dbCon = dbCon;
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Product name is required.")
                .MinimumLength(3).WithMessage("Product name must be at least 3 characters long.")
                .Must(UniqueProductName).WithMessage("Product name already exists.");

            RuleFor(x => x.ProductCode)
                .NotEmpty().WithMessage("Product code is required.")
                .Must(UniqueProductCode).WithMessage("Product code already exists.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description must be less than 500 characters.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be a positive number.");

            RuleFor(x => x.Year)
                .InclusiveBetween(1900, DateTime.Now.Year)
                .WithMessage("Year must be between 1900 and the current year.");

            RuleFor(x => x.ImageFile)
                 .Must(HaveValidImageExtension)
                 .WithMessage("Image must be a valid file with extensions jpg, jpeg, png, or gif.")
                 .When(x => x.ImageFile != null && x.ImageFile.Length > 0);


            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("Category ID must be a positive integer.")
                .Must(ValidCategoryId).WithMessage("Invalid Category ID.");

            RuleFor(x => x.ProducerId)
                .GreaterThan(0).WithMessage("Producer ID must be a positive integer.")
                .Must(ValidProducerId).WithMessage("Invalid Producer ID.");

            RuleFor(x => x.StatusId)
                .GreaterThan(0).WithMessage("Status ID must be a positive integer.")
                .Must(ValidStatusId).WithMessage("Invalid Status ID.");
        }

        private bool UniqueProductName(string productName)
        {
            return !_dbCon.Products.Any(x => x.Name == productName && x.IsDeleted == false) && 
                   !_dbCon.Products.Any(x => x.Name == productName && x.IsDeleted == true);
        }
        private bool UniqueProductCode(int productCode)
        {
            return !_dbCon.Products.Any(x => x.ProductCode == productCode && x.IsDeleted == false) && 
                   !_dbCon.Products.Any(x => x.ProductCode == productCode && x.IsDeleted == true);
        }

        private bool ValidCategoryId(int categoryId)
        {
            return _dbCon.Categories.Any(x => x.Id == categoryId);
        }

        private bool ValidProducerId(int producerId)
        {
            return _dbCon.Producers.Any(x => x.Id == producerId);
        }

        private bool ValidStatusId(int statusId)
        {
            return _dbCon.Statuses.Any(x => x.Id == statusId);
        }

        private bool HaveValidImageExtension(IFormFile image)
        {
            if (image == null || image.Length == 0)
            {
                return true; 
            }

            var validExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var extension = Path.GetExtension(image.FileName).ToLower();
            return validExtensions.Contains(extension);
        }

    }
}
