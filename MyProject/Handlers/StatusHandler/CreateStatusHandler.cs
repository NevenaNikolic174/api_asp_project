using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyProject.Api.Core;
using MyProject.Api.DTO;
using MyProject.Api.Extensions;
using MyProject.Api.Handlers.CategoryHandler;
using MyProject.Application.Actor;
using MyProject.Application.DTO;
using MyProject.DataAccess;
using MyProject.Domain.Entities;
using MyProject.Implementation.Validators;

namespace MyProject.Api.Handlers.StatusHandler
{
    public class CreateStatusHandler
    {
        private readonly MyDBContext _dbCon;
        private readonly IMapper _mapper;
        private readonly CreateStatusValidator _validator;
        private readonly ILogger<CreateStatusHandler> _logger;
        private readonly AuditLogger _auditLogger;
        private readonly IApplicationActor _applicationActor;

        public CreateStatusHandler(MyDBContext context, 
                                   IMapper mapper,
                                   CreateStatusValidator validator, 
                                   ILogger<CreateStatusHandler> logger, 
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

        public IActionResult Handle(StatusDTO dto)
        {
            var result = _validator.Validate(dto);

            if (!result.IsValid)
            {
                return result.AsClientErrors();
            }

            try
            {
                var status = _mapper.Map<Status>(dto);
                status.IsActive = true;

                _dbCon.Statuses.Add(status);
                _dbCon.SaveChanges();
                _auditLogger.LogAudit(_applicationActor.Username, "Create_Status", "Success");

                return new OkObjectResult("Status successfully created.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: \n{ex}\n");
                var error = new ServerError { Error500 = ex.Message };
                _auditLogger.LogAudit(_applicationActor.Username, "Create_Status", "Failed");
                return new StatusCodeResult(500);
            }
        }
    }
}
