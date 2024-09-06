using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyProject.Api.Core;
using MyProject.Application.Actor;
using MyProject.DataAccess;
using MyProject.Domain.Entities;
using System;
using System.Linq;

namespace MyProject.Api.Handlers.ProductOperations
{
    public class DeleteProductHandler
    {
        private readonly MyDBContext _dbCon;
        private readonly ILogger<DeleteProductHandler> _logger;
        private readonly AuditLogger _auditLogger;
        private readonly IApplicationActor _applicationActor;

        public DeleteProductHandler(MyDBContext context, 
                                    ILogger<DeleteProductHandler> logger, 
                                    IApplicationActor applicationActor, 
                                    AuditLogger auditLogger)
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
                var product = _dbCon.Products.FirstOrDefault(p => p.Id == id);

                if (product == null)
                {
                    return new NotFoundObjectResult("Product not found.");
                }

                var productRestaurants = _dbCon.ProductRestaurants.Where(pr => pr.ProductId == id).ToList();
                if (productRestaurants.Any())
                {
                    _dbCon.ProductRestaurants.RemoveRange(productRestaurants);
                }

                var cartItems = _dbCon.Carts.Where(c => c.ProductId == id).ToList();
                if (cartItems.Any())
                {
                    _dbCon.Carts.RemoveRange(cartItems);
                }

                product.IsDeleted = true;
                product.IsActive = false;
                product.DeletedAt = DateTime.Now;

                _dbCon.SaveChanges();
                _auditLogger.LogAudit(_applicationActor.Username, "Delete_Product", "Success");
                return new NoContentResult();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: \n{ex}\n");
                var error = new ServerError { Error500 = ex.Message };

                _auditLogger.LogAudit(_applicationActor.Username, "Delete_Product", "Failed");
                return new StatusCodeResult(500);
            }
        }
    }
}
