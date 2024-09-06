using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyProject.Api.Core;
using MyProject.Api.Extensions;
using MyProject.Application.Actor;
using MyProject.Application.DTO;
using MyProject.DataAccess;
using MyProject.Domain.Entities;
using MyProject.Implementation.Validators;

namespace MyProject.Api.Handlers.CategoryHandler
{
    public class CreateCategoryHandler
    {
        private readonly MyDBContext _dbCon;
        private readonly IMapper _mapper;
        private readonly CreateCategoryValidator _validator;
        private readonly ILogger<CreateCategoryHandler> _logger;
        private readonly AuditLogger _auditLogger;
        private readonly IApplicationActor _applicationActor; 

        public CreateCategoryHandler(MyDBContext context, 
                                     IMapper mapper, 
                                     CreateCategoryValidator validator, 
                                     ILogger<CreateCategoryHandler> logger, 
                                     AuditLogger auditLogger, 
                                     IApplicationActor applicationActor)
        {
            _dbCon = context;
            _mapper = mapper;
            _validator = validator;
            _logger = logger;
            _auditLogger = auditLogger;
            _applicationActor = applicationActor;
        }

        public IActionResult Handle(CategoryDTO dto)
        {
            var result = _validator.Validate(dto);

            if (!result.IsValid)
            {
                return result.AsClientErrors();
            }

            try
            {
                var category = _mapper.Map<Category>(dto);
                category.IsActive = true;

                _dbCon.Categories.Add(category);
                _dbCon.SaveChanges();
                _auditLogger.LogAudit(_applicationActor.Username, "Create_Category", "Success");

                return new OkObjectResult("Category successfully created.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: \n{ex}\n");
                var error = new ServerError { Error500 = ex.Message };
                _auditLogger.LogAudit(_applicationActor.Username, "Create_Category", "Failed");
                return new StatusCodeResult(500);
            }
        }
    }
}
