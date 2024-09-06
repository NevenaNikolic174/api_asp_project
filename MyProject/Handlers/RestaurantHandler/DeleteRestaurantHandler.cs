using Microsoft.AspNetCore.Mvc;
using MyProject.Api.Core;
using MyProject.Api.Handlers.CategoryHandler;
using MyProject.Application.Actor;
using MyProject.DataAccess;

namespace MyProject.Api.Handlers.RestaurantHandler
{
    public class DeleteRestaurantHandler
    {
        private readonly MyDBContext _dbCon;
        private readonly ILogger<DeleteRestaurantHandler> _logger;
        private readonly AuditLogger _auditLogger;
        private readonly IApplicationActor _applicationActor;


        public DeleteRestaurantHandler(MyDBContext context,
                                     ILogger<DeleteRestaurantHandler> logger,
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
                var restaurant = _dbCon.Restaurants.FirstOrDefault(p => p.Id == id);

                if (restaurant == null)
                {
                    return new NotFoundObjectResult("Restaurant not found.");
                }

                restaurant.IsDeleted = true;
                restaurant.IsActive = false;
                restaurant.DeletedAt = DateTime.Now;

                _dbCon.SaveChanges();
                _auditLogger.LogAudit(_applicationActor.Username, "Delete_Restaurant", "Success");
                return new NoContentResult();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: \n{ex}\n");
                var error = new ServerError { Error500 = ex.Message };
                _auditLogger.LogAudit(_applicationActor.Username, "Delete_Restaurant", "Failed");
                return new StatusCodeResult(500);
            }
        }
    }
}
