using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyProject.Api.Core;
using MyProject.Api.DTO;
using MyProject.Api.Handlers.CategoryHandler;
using MyProject.Application.Actor;
using MyProject.Application.DTO;
using MyProject.DataAccess;

namespace MyProject.Api.Handlers.StatusHandler
{
    public class UpdateStatusHandler
    {
        private readonly MyDBContext _dbCon;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateStatusHandler> _logger;
        private readonly AuditLogger _auditLogger;
        private readonly IApplicationActor _applicationActor;

        public UpdateStatusHandler(MyDBContext context,
                                    IMapper mapper,
                                    ILogger<UpdateStatusHandler> logger,
                                    AuditLogger auditLogger,
                                    IApplicationActor applicationActor)
        {
            _dbCon = context;
            _mapper = mapper;
            _logger = logger;
            _auditLogger = auditLogger;
            _applicationActor = applicationActor;
        }

        public IActionResult Handle(int id, StatusDTO dto)
        {
            try
            {
                var status = _dbCon.Statuses.FirstOrDefault(p => p.Id == id);

                if (status == null)
                {
                    return new NotFoundObjectResult("Status not found.");
                }

                _mapper.Map(dto, status);
                status.ModifiedAt = DateTime.Now;
                _dbCon.SaveChanges();
                _auditLogger.LogAudit(_applicationActor.Username, "Update_Status", "Success");

                return new OkObjectResult("Status updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: \n{ex}\n");
                var error = new ServerError { Error500 = ex.Message };
                _auditLogger.LogAudit(_applicationActor.Username, "Update_Status", "Failed");
                return new StatusCodeResult(500);
            }
        }
    }
}
