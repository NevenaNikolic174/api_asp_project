using FluentValidation;
using MyProject.Application.DTO;
using MyProject.DataAccess;
using System.Linq;

namespace MyProject.Implementation.Validators
{
    public class CreateProductRestaurantValidator : AbstractValidator<ProductRestaurantDTO>
    {
        private readonly MyDBContext _dbCon;

        public CreateProductRestaurantValidator(MyDBContext dbCon)
        {
            _dbCon = dbCon;
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.ProductName)
                .NotEmpty().WithMessage("Product name is required.")
                .Must(ProductNameExists).WithMessage("Product name must exist in the Products table.");

            RuleFor(x => x.RestaurantName)
                .NotEmpty().WithMessage("Restaurant name is required.")
                .Must(RestaurantNameExists).WithMessage("Restaurant name must exist in the Restaurant table.");

            RuleFor(x => x)
                .Must(BeUniquePair).WithMessage("The combination of Product and Restaurant must be unique.");
        }

        private bool ProductNameExists(string productName)
        {
            var lowerProductName = productName.ToLower();
            return _dbCon.Products
                .Any(p => p.Name.ToLower() == lowerProductName && !p.IsDeleted);
        }

        private bool RestaurantNameExists(string restaurantName)
        {
            var lowerRestaurantName = restaurantName.ToLower();
            return _dbCon.Restaurants
                .Any(r => r.Name.ToLower() == lowerRestaurantName && !r.IsDeleted);
        }

        private bool BeUniquePair(ProductRestaurantDTO dto)
        {
            var productId = _dbCon.Products
                .Where(p => p.Name.ToLower() == dto.ProductName.ToLower() && !p.IsDeleted)
                .Select(p => p.Id)
                .FirstOrDefault();

            var restaurantId = _dbCon.Restaurants
                .Where(r => r.Name.ToLower() == dto.RestaurantName.ToLower() && !r.IsDeleted)
                .Select(r => r.Id)
                .FirstOrDefault();

            if (productId == 0 || restaurantId == 0)
            {
                return false;
            }

            return !_dbCon.ProductRestaurants.Any(pr => pr.ProductId == productId && pr.RestaurantId == restaurantId);
        }
    }
}
