using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyProject.Api.Core;
using MyProject.Application.Actor;
using MyProject.Application.DTO;
using MyProject.DataAccess;

namespace MyProject.Api.Handlers.PaymentHandler
{
    public class UpdatePaymentHandler
    {
        private readonly MyDBContext _dbCon;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdatePaymentHandler> _logger;
        private readonly AuditLogger _auditLogger;
        private readonly IApplicationActor _applicationActor;

        public UpdatePaymentHandler(MyDBContext context,
                                    IMapper mapper,
                                    ILogger<UpdatePaymentHandler> logger,
                                    AuditLogger auditLogger,
                                    IApplicationActor applicationActor)
        {
            _dbCon = context;
            _mapper = mapper;
            _logger = logger;
            _auditLogger = auditLogger;
            _applicationActor = applicationActor;
        }

        public IActionResult Handle(int id, PaymentDTO dto)
        {
            try
            {
                var payment = _dbCon.Payments.FirstOrDefault(p => p.Id == id);

                if (payment == null)
                {
                    return new NotFoundObjectResult("Payment not found.");
                }

                _mapper.Map(dto, payment);
                payment.ModifiedAt = DateTime.Now;
                _dbCon.SaveChanges();
                _auditLogger.LogAudit(_applicationActor.Username, "Update_Payment", "Success");

                return new OkObjectResult("Category updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: \n{ex}\n");
                var error = new ServerError { Error500 = ex.Message };
                _auditLogger.LogAudit(_applicationActor.Username, "Update_Payment", "Failed");
                return new StatusCodeResult(500);
            }
        }
    }
}
