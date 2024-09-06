using Microsoft.AspNetCore.Mvc;
using MyProject.Api.Core;
using MyProject.Application.Actor;
using MyProject.DataAccess;

namespace MyProject.Api.Handlers.PaymentHandler
{
    public class DeletePaymentHandler
    {
        private readonly MyDBContext _dbCon;
        private readonly ILogger<DeletePaymentHandler> _logger;
        private readonly AuditLogger _auditLogger;
        private readonly IApplicationActor _applicationActor;


        public DeletePaymentHandler(MyDBContext context,
                                     ILogger<DeletePaymentHandler> logger,
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
                var payment = _dbCon.Payments.FirstOrDefault(p => p.Id == id);

                if (payment == null)
                {
                    return new NotFoundObjectResult("Payment not found.");
                }

                payment.IsDeleted = true;
                payment.IsActive = false;
                payment.DeletedAt = DateTime.Now;

                _dbCon.SaveChanges();
                _auditLogger.LogAudit(_applicationActor.Username, "Delete_Payment", "Success");
                return new NoContentResult();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: \n{ex}\n");
                var error = new ServerError { Error500 = ex.Message };
                _auditLogger.LogAudit(_applicationActor.Username, "Delete_Payment", "Failed");
                return new StatusCodeResult(500);
            }
        }
    }
}
