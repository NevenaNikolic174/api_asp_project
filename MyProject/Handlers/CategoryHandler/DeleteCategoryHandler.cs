using Microsoft.AspNetCore.Mvc;
using MyProject.Api.Core;
using MyProject.Application.Actor;
using MyProject.DataAccess;

namespace MyProject.Api.Handlers.CategoryHandler
{
    public class DeleteCategoryHandler
    {
        private readonly MyDBContext _dbCon;
        private readonly ILogger<DeleteCategoryHandler> _logger;
        private readonly AuditLogger _auditLogger;
        private readonly IApplicationActor _applicationActor;


        public DeleteCategoryHandler(MyDBContext context, 
                                     ILogger<DeleteCategoryHandler> logger, 
                                     AuditLogger auditLogger,
                                     IApplicationActor applicationActor)
        {
            _dbCon = context;
            _logger = logger;
            _auditLogger = auditLogger;
            _applicationActor = applicationActor;
        }

        public IActionResult Handle(int id)
        {
            try
            {
                var category = _dbCon.Categories.FirstOrDefault(p => p.Id == id);

                if (category == null)
                {
                    return new NotFoundObjectResult("Category not found.");
                }

                category.IsDeleted = true;
                category.IsActive = false;
                category.DeletedAt = DateTime.Now;

                _dbCon.SaveChanges();
                _auditLogger.LogAudit(_applicationActor.Username, "Delete_Category", "Success");
                return new NoContentResult();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: \n{ex}\n");
                var error = new ServerError { Error500 = ex.Message };
                _auditLogger.LogAudit(_applicationActor.Username, "Delete_Category", "Failed");
                return new StatusCodeResult(500);
            }
        }
    }
}
