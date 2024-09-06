using FluentValidation;
using MyProject.Api.DTO;
using MyProject.DataAccess;
using System.Linq;

namespace MyProject.Implementation.Validators
{
    public class CreateProducerValidator : AbstractValidator<ProducerDTO>
    {
        private readonly MyDBContext _dbCon;

        public CreateProducerValidator(MyDBContext context)
        {
            _dbCon = context;

            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Producer name required.")
                .MinimumLength(3).WithMessage("Producer name must have a minimum of 3 characters.")
                .MaximumLength(20)
                .Must(name => !_dbCon.Producers.Any(p => p.Name == name)).WithMessage("Name already exists in the database.");
        }
    }
}
