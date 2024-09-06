using FluentValidation;
using MyProject.Application.DTO;
using MyProject.DataAccess;

namespace MyProject.Implementation.Validators
{
    public class CreateCartValidator : AbstractValidator<CartDTO>
    {
        private readonly MyDBContext _dbContext;

        public CreateCartValidator(MyDBContext dbContext)
        {
            _dbContext = dbContext;

            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleForEach(x => x.Items).SetValidator(new CartItemValidator(_dbContext));
        }
    }

    public class CartItemValidator : AbstractValidator<CartItemDTO>
    {
        private readonly MyDBContext _dbContext;

        public CartItemValidator(MyDBContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(x => x.ProductId).NotEmpty().WithMessage("Need to choose a product ID.");
            RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be a positive number.");
        }
    }
}
