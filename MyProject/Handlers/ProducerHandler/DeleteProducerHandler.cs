using Microsoft.AspNetCore.Mvc;
using MyProject.Api.Core;
using MyProject.Application.Actor;
using MyProject.DataAccess;

namespace MyProject.Api.Handlers.ProducerHandler
{
    public class DeleteProducerHandler
    {
        private readonly MyDBContext _dbCon;
        private readonly ILogger<DeleteProducerHandler> _logger;
        private readonly AuditLogger _auditLogger;
        private readonly IApplicationActor _applicationActor;


        public DeleteProducerHandler(MyDBContext context,
                                     ILogger<DeleteProducerHandler> logger,
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
                var producer = _dbCon.Producers.FirstOrDefault(p => p.Id == id);

                if (producer == null)
                {
                    return new NotFoundObjectResult("Producer not found.");
                }

                producer.IsDeleted = true;
                producer.IsActive = false;
                producer.DeletedAt = DateTime.Now;

                _dbCon.SaveChanges();
                _auditLogger.LogAudit(_applicationActor.Username, "Delete_Producer", "Success");
                return new NoContentResult();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: \n{ex}\n");
                var error = new ServerError { Error500 = ex.Message };
                _auditLogger.LogAudit(_applicationActor.Username, "Delete_Producer", "Failed");
                return new StatusCodeResult(500);
            }
        }
    }
}
