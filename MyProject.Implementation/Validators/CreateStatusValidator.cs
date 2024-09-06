using FluentValidation;
using MyProject.Api.DTO;
using MyProject.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Implementation.Validators
{
    public class CreateStatusValidator : AbstractValidator<StatusDTO>
    {
        private readonly MyDBContext _dbCon;

        public CreateStatusValidator(MyDBContext dbCon)
        {
            _dbCon = dbCon;

            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Value)
                .NotEmpty().WithMessage("Status value is required.")
                .MinimumLength(3).WithMessage("Status name must have a minimum of 3 characters.")
                .MaximumLength(15)
                .Must(value => !_dbCon.Statuses.Any(x => x.Value == value)).WithMessage("Value already exists in the database.");
        }   
    }
}
