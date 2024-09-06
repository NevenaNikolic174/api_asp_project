using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyProject.Api.Core;
using MyProject.Api.DTO;
using MyProject.Application.Actor;
using MyProject.Application.DTO;
using MyProject.DataAccess;

namespace MyProject.Api.Handlers.ProducerHandler
{
    public class UpdateProducerHandler
    {
        private readonly MyDBContext _dbCon;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateProducerHandler> _logger;
        private readonly AuditLogger _auditLogger;
        private readonly IApplicationActor _applicationActor;

        public UpdateProducerHandler(MyDBContext context,
                                    IMapper mapper,
                                    ILogger<UpdateProducerHandler> logger,
                                    AuditLogger auditLogger,
                                    IApplicationActor applicationActor)
        {
            _dbCon = context;
            _mapper = mapper;
            _logger = logger;
            _auditLogger = auditLogger;
            _applicationActor = applicationActor;
        }

        public IActionResult Handle(int id, ProducerDTO dto)
        {
            try
            {
                var producer = _dbCon.Producers.FirstOrDefault(p => p.Id == id);

                if (producer == null)
                {
                    return new NotFoundObjectResult("Producer not found.");
                }

                _mapper.Map(dto, producer);
                producer.ModifiedAt = DateTime.Now;
                _dbCon.SaveChanges();
                _auditLogger.LogAudit(_applicationActor.Username, "Update_Producer", "Success");

                return new OkObjectResult("Producer updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: \n{ex}\n");
                var error = new ServerError { Error500 = ex.Message };
                _auditLogger.LogAudit(_applicationActor.Username, "Update_Producer", "Failed");
                return new StatusCodeResult(500);
            }
        }
    }
}
