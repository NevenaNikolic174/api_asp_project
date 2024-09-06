using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyProject.Api.Core;
using MyProject.Application.Actor;
using MyProject.Application.DTO;
using MyProject.DataAccess;
using MyProject.Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace MyProject.Api.Handlers.ProductRestaurantHandler
{
    public class DeletePRHandler
    {
        private readonly MyDBContext _dbCon;
        private readonly ILogger<DeletePRHandler> _logger;
        private readonly AuditLogger _auditLogger;
        private readonly IApplicationActor _applicationActor;

        public DeletePRHandler(MyDBContext context,
                               ILogger<DeletePRHandler> logger,
                               AuditLogger auditLogger,
                               IApplicationActor applicationActor)
        {
            _dbCon = context;
            _logger = logger;
            _auditLogger = auditLogger;
            _applicationActor = applicationActor;
        }

        public IActionResult Handle(int productId, int restaurantId)
        {
            try
            {
                var productRestaurant = _dbCon.ProductRestaurants
                                               .FirstOrDefault(pr => pr.ProductId == productId &&
                                                                     pr.RestaurantId == restaurantId);

                if (productRestaurant == null)
                {
                    return new NotFoundObjectResult("No data found for the given product ID and restaurant ID.");
                }

                _dbCon.ProductRestaurants.Remove(productRestaurant);
                _dbCon.SaveChanges();

                _auditLogger.LogAudit(_applicationActor.Username, "Delete_Product_Restaurant", "Success");
                return new NoContentResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error");
                _auditLogger.LogAudit(_applicationActor.Username, "Delete_Product_Restaurant", "Failed");
                return new StatusCodeResult(500);
            }
        }
    }
}
