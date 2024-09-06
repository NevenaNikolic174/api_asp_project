using FluentValidation;
using MyProject.Application.DTO;
using System.Linq;

namespace MyProject.Implementation.Validators
{
    public class CreateOrderValidator : AbstractValidator<OrderDTO>
    {
        public CreateOrderValidator()
        {
            RuleFor(x => x.Products)
                .NotNull()
                .WithMessage("Cart IDs must be provided.")
                .Must(cartIds => cartIds != null && cartIds.Any())
                .WithMessage("At least one Cart ID must be provided.");

           
            RuleFor(x => x.UserAddress)
                .NotEmpty()
                .WithMessage("User address is required.");

            RuleFor(x => x.PaymentId)
                .InclusiveBetween(1, 2)
                .WithMessage("PaymentId must be either 1 (Credit Card) or 2 (Cash on Delivery).");

            When(x => x.PaymentId == 1, () =>
            {
                RuleFor(x => x.CreditCardNumber)
                    .NotNull()
                    .WithMessage("Credit card number is required.")
                    .Must(IsValidCreditCardNumber)
                    .WithMessage("Invalid credit card number.");
            });

            When(x => x.PaymentId == 2, () =>
            {
                RuleFor(x => x.CreditCardNumber)
                    .Must(ccn => ccn == null)
                    .WithMessage("Credit card number should not be provided when paying by Cash on Delivery.");
            });
        }

        private bool IsValidCreditCardNumber(long? creditCardNumber)
        {
            if (!creditCardNumber.HasValue) return false;

            var length = creditCardNumber.Value.ToString().Length;
            return length >= 15 && length <= 16;
        }
    }
}
