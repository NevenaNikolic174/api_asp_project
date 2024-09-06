using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyProject.Api.Core;
using MyProject.Api.DTO;
using MyProject.Api.Extensions;
using MyProject.Application.Actor;
using MyProject.Application.DTO;
using MyProject.DataAccess;
using MyProject.Domain.Entities;
using MyProject.Implementation.Validators;

namespace MyProject.Api.Handlers.PaymentHandler
{
    public class CreatePaymentHandler
    {
        private readonly MyDBContext _dbCon;
        private readonly IMapper _mapper;
        private readonly CreatePaymentValidator _validator;
        private readonly ILogger<CreatePaymentHandler> _logger;
        private readonly AuditLogger _auditLogger;
        private readonly IApplicationActor _applicationActor;


        public CreatePaymentHandler(MyDBContext context, 
                                    IMapper mapper,
                                    CreatePaymentValidator validator, 
                                    ILogger<CreatePaymentHandler> logger, 
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

        public IActionResult Handle(PaymentDTO dto)
        {
            var result = _validator.Validate(dto);

            if (!result.IsValid)
            {
                return result.AsClientErrors();
            }

            try
            {
                var payment = _mapper.Map<Payment>(dto);
                payment.IsActive = true;

                _dbCon.Payments.Add(payment);
                _dbCon.SaveChanges();
                _auditLogger.LogAudit(_applicationActor.Username, "Create_Payment", "Success");

                return new OkObjectResult("Payment successfully created.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: \n{ex}\n");
                var error = new ServerError { Error500 = ex.Message };
                _auditLogger.LogAudit(_applicationActor.Username, "Create_Payment", "Failed");
                return new StatusCodeResult(500);
            }
        }
    }
}
