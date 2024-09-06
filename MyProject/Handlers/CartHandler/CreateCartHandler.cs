using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProject.Api.Core;
using MyProject.Api.Extensions;
using MyProject.Application.Actor;
using MyProject.Application.DTO;
using MyProject.DataAccess;
using MyProject.Domain.Entities;
using MyProject.Implementation.Validators;
using System;
using System.Linq;

namespace MyProject.Api.Handlers.CartHandler
{
    public class CreateCartHandler
    {
        private readonly MyDBContext _dbCon;
        private readonly IMapper _mapper;
        private readonly CreateCartValidator _validator;
        private readonly ILogger<CreateCartHandler> _logger;
        private readonly IApplicationActor _applicationActor;
        private readonly AuditLogger _auditLogger;

        public CreateCartHandler(MyDBContext context, 
                                 IMapper mapper,
                                 CreateCartValidator validator,
                                 ILogger<CreateCartHandler> logger,
                                 IApplicationActor actor,
                                 AuditLogger auditLogger)
        {
            _dbCon = context;
            _mapper = mapper;
            _validator = validator;
            _logger = logger;
            _applicationActor = actor;
            _auditLogger = auditLogger;
        }

        public IActionResult Handle(CartDTO dto)
        {
            var result = _validator.Validate(dto);

            if (!result.IsValid)
            {
                return new BadRequestObjectResult(result.AsClientErrors());
            }

            try
            {
                var userId = _applicationActor.Id;
                double overallTotalPrice = 0;

                var productIds = dto.Items.Select(item => item.ProductId).Distinct().ToList();

                var products = _dbCon.Products
                    .Where(p => productIds.Contains(p.Id))
                    .Select(p => new { p.Id, p.Price })
                    .ToList();

                var existingProductIds = products.Select(p => p.Id).ToHashSet();

                foreach (var item in dto.Items)
                {
                    if (item.Quantity <= 0 || item.ProductId <= 0)
                    {
                        return new BadRequestObjectResult("Invalid cart item data.");
                    }

                    var product = products.FirstOrDefault(p => p.Id == item.ProductId);
                    if (product == null)
                    {
                        return new NotFoundObjectResult($"Product with ID {item.ProductId} does not exist.");
                    }

                    item.Price = product.Price; 
                    overallTotalPrice += item.TotalPrice; 

                    var existingCartItem = _dbCon.Carts.FirstOrDefault(c => c.UserId == userId && 
                                                                            c.ProductId == item.ProductId);

                    if (existingCartItem != null)
                    {
                        existingCartItem.Quantity += item.Quantity;
                        existingCartItem.TotalPrice += item.TotalPrice; 
                    }
                    else
                    {
                        var cartItem = _mapper.Map<Cart>(item);
                        cartItem.IsActive = true;
                        cartItem.UserId = userId;
                        _dbCon.Carts.Add(cartItem);
                    }
                }

                _dbCon.SaveChanges();

                dto.TotalPrice = overallTotalPrice;

                _auditLogger.LogAudit(_applicationActor.Username, "Added_Cart", "Success");
                return new OkObjectResult(new { Message = "Cart items processed successfully!"});
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: \n{ex}\n");
                var error = new ServerError { Error500 = ex.Message };

                _auditLogger.LogAudit(_applicationActor.Username, "Added_Cart", "Failed");
                return new StatusCodeResult(500);
            }
        }
    }
}
