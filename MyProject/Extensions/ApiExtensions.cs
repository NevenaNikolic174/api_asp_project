using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MyProject.Api.Core; // Adjust based on actual namespace

namespace MyProject.Api.Extensions
{
    public static class ApiExtensions
    {
        public static UnprocessableEntityObjectResult AsClientErrors(this FluentValidation.Results.ValidationResult result)
        {
            var errorMessages = result.Errors.Select(error => new ClientError
            {
                ErrorMsg = error.ErrorMessage,
                NameOfProperty = error.PropertyName
            }).ToList();

            return new UnprocessableEntityObjectResult(new
            {
                Errors = errorMessages
            });
        }
    }
}
