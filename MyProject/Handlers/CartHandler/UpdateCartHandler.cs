using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProject.Api.Core;
using MyProject.Application.Actor;
using MyProject.Application.DTO;
using MyProject.DataAccess;
using MyProject.Domain.Entities;
using System;
using System.Linq;

namespace MyProject.Api.Handlers.CartHandler
{
    public class UpdateCartHandler
    {
        private readonly MyDBContext _dbCon;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateCartHandler> _logger;
        private readonly IApplicationActor _applicationActor;
        private readonly AuditLogger _auditLogger;

        public UpdateCartHandler(MyDBContext context, 
                                 IMapper mapper, 
                                 ILogger<UpdateCartHandler> logger, 
                                 IApplicationActor actor, 
                                 AuditLogger auditLogger)
        {
            _dbCon = context;
            _mapper = mapper;
            _logger = logger;
            _applicationActor = actor;
            _auditLogger = auditLogger;
        }

        public IActionResult Handle(int id, CartItemDTO dto)
        {
            if (dto.Quantity <= 0)
            {
                return new BadRequestObjectResult("Quantity must be a positive number.");
            }

            try
            {
                var userId = _applicationActor.Id;

                var cartItem = _dbCon.Carts
                    .Include(c => c.Product) 
                    .FirstOrDefault(c => c.Id == id && c.UserId == userId);

                if (cartItem == null)
                {
                    return new NotFoundObjectResult($"Cart item with ID {id} does not exist.");
                }

                cartItem.Quantity = dto.Quantity;
                cartItem.TotalPrice = cartItem.Quantity * cartItem.Product.Price;

                _dbCon.SaveChanges();
                _auditLogger.LogAudit(_applicationActor.Username, "Update_Quantity_Cart", "Success");
                return new OkObjectResult("Cart item updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: \n{ex}\n");
                var error = new ServerError { Error500 = ex.Message };

                _auditLogger.LogAudit(_applicationActor.Username, "Update_Quantity_Cart", "Failed");
                return new StatusCodeResult(500);
            }
        }
    }
}
