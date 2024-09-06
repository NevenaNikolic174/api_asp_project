using Microsoft.AspNetCore.Mvc;
using MyProject.Api.Core;
using MyProject.Api.Handlers.CategoryHandler;
using MyProject.Application.Actor;
using MyProject.DataAccess;

namespace MyProject.Api.Handlers.StatusHandler
{
    public class DeleteStatusHandler
    {
        private readonly MyDBContext _dbCon;
        private readonly ILogger<DeleteStatusHandler> _logger;
        private readonly AuditLogger _auditLogger;
        private readonly IApplicationActor _applicationActor;


        public DeleteStatusHandler(MyDBContext context,
                                     ILogger<DeleteStatusHandler> logger,
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
                var status = _dbCon.Statuses.FirstOrDefault(p => p.Id == id);

                if (status == null)
                {
                    return new NotFoundObjectResult("Status not found.");
                }

                status.IsDeleted = true;
                status.IsActive = false;
                status.DeletedAt = DateTime.Now;

                _dbCon.SaveChanges();
                _auditLogger.LogAudit(_applicationActor.Username, "Delete_Status", "Success");
                return new NoContentResult();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: \n{ex}\n");
                var error = new ServerError { Error500 = ex.Message };
                _auditLogger.LogAudit(_applicationActor.Username, "Delete_Status", "Failed");
                return new StatusCodeResult(500);
            }
        }
    }
}
