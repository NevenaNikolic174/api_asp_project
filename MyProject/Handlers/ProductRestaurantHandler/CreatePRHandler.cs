using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyProject.Api.Core;
using MyProject.Api.Extensions;
using MyProject.Application.Actor;
using MyProject.Application.DTO;
using MyProject.DataAccess;
using MyProject.Domain.Entities;
using MyProject.Implementation.Validators;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace MyProject.Api.Handlers.ProductRestaurantHandler
{
    public class CreatePRHandler
    {
        private readonly MyDBContext _dbCon;
        private readonly IMapper _mapper;
        private readonly CreateProductRestaurantValidator _validator;
        private readonly ILogger<CreatePRHandler> _logger;
        private readonly AuditLogger _auditLogger;
        private readonly IApplicationActor _applicationActor;

        public CreatePRHandler(MyDBContext context,
                               IMapper mapper,
                               CreateProductRestaurantValidator validator,
                               ILogger<CreatePRHandler> logger,
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

        public IActionResult Handle(ProductRestaurantDTO dto)
        {
            var validationResult = _validator.Validate(dto);
            if (!validationResult.IsValid)
            {
                return validationResult.AsClientErrors();
            }
            try
            {
                var productId = _dbCon.Products
                    .Where(p => p.Name.ToLower() == dto.ProductName.ToLower() && !p.IsDeleted)
                    .Select(p => p.Id)
                    .FirstOrDefault();

                var restaurantId = _dbCon.Restaurants
                    .Where(r => r.Name.ToLower() == dto.RestaurantName.ToLower() && !r.IsDeleted)
                    .Select(r => r.Id)
                    .FirstOrDefault();

                if (productId == 0 || restaurantId == 0)
                {
                    return new NotFoundObjectResult("Product or restaurant not found.");
                }

                var productRestaurant = new ProductRestaurant
                {
                    ProductId = productId,
                    RestaurantId = restaurantId
                };

                _dbCon.ProductRestaurants.Add(productRestaurant);
                _dbCon.SaveChanges();

                _auditLogger.LogAudit(_applicationActor.Username, "Create_Product_Restaurant", "Success");
                return new OkObjectResult("Successfully added.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error");
                _auditLogger.LogAudit(_applicationActor.Username, "Create_Product_Restaurant", "Failed");
                return new StatusCodeResult(500);
            }
        }
    }
}
