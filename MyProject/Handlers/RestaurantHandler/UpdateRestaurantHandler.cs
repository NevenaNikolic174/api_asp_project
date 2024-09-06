using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyProject.Api.Core;
using MyProject.Api.Handlers.CategoryHandler;
using MyProject.Application.Actor;
using MyProject.Application.DTO;
using MyProject.DataAccess;

namespace MyProject.Api.Handlers.RestaurantHandler
{
    public class UpdateRestaurantHandler
    {
        private readonly MyDBContext _dbCon;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateRestaurantHandler> _logger;
        private readonly AuditLogger _auditLogger;
        private readonly IApplicationActor _applicationActor;

        public UpdateRestaurantHandler(MyDBContext context,
                                    IMapper mapper,
                                    ILogger<UpdateRestaurantHandler> logger,
                                    AuditLogger auditLogger,
                                    IApplicationActor applicationActor)
        {
            _dbCon = context;
            _mapper = mapper;
            _logger = logger;
            _auditLogger = auditLogger;
            _applicationActor = applicationActor;
        }

        public IActionResult Handle(int id, RestaurantDTO dto)
        {
            try
            {
                var restaurant = _dbCon.Restaurants.FirstOrDefault(p => p.Id == id);

                if (restaurant == null)
                {
                    return new NotFoundObjectResult("Restaurant not found.");
                }

                _mapper.Map(dto, restaurant);
                restaurant.ModifiedAt = DateTime.Now;
                _dbCon.SaveChanges();
                _auditLogger.LogAudit(_applicationActor.Username, "Update_Restaurant", "Success");

                return new OkObjectResult("Restaurant updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: \n{ex}\n");
                var error = new ServerError { Error500 = ex.Message };
                _auditLogger.LogAudit(_applicationActor.Username, "Update_Restaurant", "Failed");
                return new StatusCodeResult(500);
            }
        }
    }
}
