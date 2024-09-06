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

namespace MyProject.Api.Handlers.ProducerHandler
{
    public class CreateProducerHandler
    {
        private readonly MyDBContext _dbCon;
        private readonly IMapper _mapper;
        private readonly CreateProducerValidator _validator;
        private readonly ILogger<CreateProducerHandler> _logger;
        private readonly AuditLogger _auditLogger;
        private readonly IApplicationActor _applicationActor;


        public CreateProducerHandler(MyDBContext context, 
                                    IMapper mapper,
                                    CreateProducerValidator validator, 
                                    ILogger<CreateProducerHandler> logger, 
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

        public IActionResult Handle(ProducerDTO dto)
        {
            var result = _validator.Validate(dto);

            if (!result.IsValid)
            {
                return result.AsClientErrors();
            }

            try
            {
                var producer = _mapper.Map<Producer>(dto);
                producer.IsActive = true;

                _dbCon.Producers.Add(producer);
                _dbCon.SaveChanges();

                _auditLogger.LogAudit(_applicationActor.Username, "Create_Producer", "Success");
                return new OkObjectResult("Producer successfully created.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: \n{ex}\n");
                var error = new ServerError { Error500 = ex.Message };
                _auditLogger.LogAudit(_applicationActor.Username, "Create_Producer", "Failed");
                return new StatusCodeResult(500);
            }
        }
    }
}
