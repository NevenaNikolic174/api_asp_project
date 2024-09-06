using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyProject.Api.Core;
using MyProject.Api.Extensions;
using MyProject.Api.Handlers.CategoryHandler;
using MyProject.Application.Actor;
using MyProject.Application.DTO;
using MyProject.DataAccess;
using MyProject.Domain.Entities;
using MyProject.Implementation.Validators;

namespace MyProject.Api.Handlers.RestaurantHandler
{
    public class CreateRestaurantHandler
    {
        private readonly MyDBContext _dbCon;
        private readonly IMapper _mapper;
        private readonly CreateRestaurantValidator _validator;
        private readonly ILogger<CreateRestaurantHandler> _logger;
        private readonly AuditLogger _auditLogger;
        private readonly IApplicationActor _applicationActor;

        public CreateRestaurantHandler(MyDBContext context, IMapper mapper, CreateRestaurantValidator validator, ILogger<CreateRestaurantHandler> logger, AuditLogger auditLogger, IApplicationActor applicationActor)
        {
            _dbCon = context;
            _mapper = mapper;
            _validator = validator;
            _logger = logger;
            _auditLogger = auditLogger;
            _applicationActor = applicationActor;
        }

        public IActionResult Handle(RestaurantDTO dto)
        {
            var result = _validator.Validate(dto);

            if (!result.IsValid)
            {
                return result.AsClientErrors();
            }

            try
            {
                var restaurant = _mapper.Map<Restaurant>(dto);
                restaurant.IsActive = true;

                _dbCon.Restaurants.Add(restaurant);
                _dbCon.SaveChanges();
                _auditLogger.LogAudit(_applicationActor.Username, "Create_Restaurant", "Success");

                return new OkObjectResult("Restaurant successfully created.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: \n{ex}\n");
                var error = new ServerError { Error500 = ex.Message };
                _auditLogger.LogAudit(_applicationActor.Username, "Create_Restaurant", "Failed");
                return new StatusCodeResult(500);
            }
        }
    }
}
