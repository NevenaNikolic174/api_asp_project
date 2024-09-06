using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyProject.Api.Core;
using MyProject.Api.DTO;
using MyProject.Application.Actor;
using MyProject.Application.DTO;
using MyProject.DataAccess;

namespace MyProject.Api.Handlers.CategoryHandler
{
    public class UpdateCategoryHandler
    {
        private readonly MyDBContext _dbCon;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateCategoryHandler> _logger;
        private readonly AuditLogger _auditLogger;
        private readonly IApplicationActor _applicationActor;

        public UpdateCategoryHandler(MyDBContext context, 
                                    IMapper mapper, 
                                    ILogger<UpdateCategoryHandler> logger, 
                                    AuditLogger auditLogger, 
                                    IApplicationActor applicationActor)
        {
            _dbCon = context;
            _mapper = mapper;
            _logger = logger;
            _auditLogger = auditLogger;
            _applicationActor = applicationActor;
        }

        public IActionResult Handle(int id, CategoryDTO dto)
        {
            try
            {
                var category = _dbCon.Categories.FirstOrDefault(p => p.Id == id);

                if (category == null)
                {
                    return new NotFoundObjectResult("Category not found.");
                }

                _mapper.Map(dto, category);
                category.ModifiedAt = DateTime.Now;
                _dbCon.SaveChanges();
                _auditLogger.LogAudit(_applicationActor.Username, "Update_Category", "Success");

                return new OkObjectResult("Category updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: \n{ex}\n");
                var error = new ServerError { Error500 = ex.Message };
                _auditLogger.LogAudit(_applicationActor.Username, "Update_Category", "Failed");
                return new StatusCodeResult(500);
            }
        }
    }
}
