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
    public class CreateCategoryValidator : AbstractValidator<CategoryDTO>
    {
        private readonly MyDBContext _dbCon;

        public CreateCategoryValidator(MyDBContext context)
        {
            _dbCon = context;

            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Category name is required.")
                .MinimumLength(3).WithMessage("Category name must have a minimum of 3 characters.")
                .MaximumLength(20)
                .Must(name => !_dbCon.Categories.Any(x => x.Name == name)).WithMessage("Name already exists in the database.");
        }
    }
}
