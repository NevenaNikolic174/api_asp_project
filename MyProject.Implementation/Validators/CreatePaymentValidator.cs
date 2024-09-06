using FluentValidation;
using MyProject.Application.DTO;
using MyProject.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Implementation.Validators
{
    public class CreatePaymentValidator : AbstractValidator<PaymentDTO>
    {
        private readonly MyDBContext _dbCon;

        public CreatePaymentValidator(MyDBContext dbCon)
        {
            _dbCon = dbCon;
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Method)
                .NotEmpty().WithMessage("Method is required.")
                .MinimumLength(3)
                .MaximumLength(20)
                .Must(method => !_dbCon.Payments.Any(x => x.Method == method))
                .WithMessage("Method already exists in the database."); ;
             
        }

    }
}
