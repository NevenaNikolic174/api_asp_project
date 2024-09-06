using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyProject.Api.Core;
using MyProject.Api.Extensions;
using MyProject.Application.Actor;
using MyProject.Application.DTO;
using MyProject.DataAccess;
using MyProject.Domain.Entities;
using MyProject.Implementation.Validators;

public class CreateOrderHandler
{
    private readonly MyDBContext _dbCon;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateOrderHandler> _logger;
    private readonly IApplicationActor _applicationActor;
    private readonly CreateOrderValidator _validator;
    private readonly AuditLogger _auditLogger;

    public CreateOrderHandler(MyDBContext context,
                              IMapper mapper,
                              ILogger<CreateOrderHandler> logger,
                              IApplicationActor actor,
                              CreateOrderValidator validator,
                              AuditLogger auditLogger)
    {
        _dbCon = context;
        _mapper = mapper;
        _logger = logger;
        _applicationActor = actor;
        _validator = validator;
        _auditLogger = auditLogger;
    }

    public IActionResult Handle(OrderDTO dto)
    {
        var result = _validator.Validate(dto);

        if (!result.IsValid)
        {
            return new BadRequestObjectResult(result.AsClientErrors());
        }

        try
        {
            var userId = _applicationActor.Id;

            var productIds = dto.Products.Select(p => p.ProductId).Distinct().ToList();

            var products = _dbCon.Products
                .Where(p => productIds.Contains(p.Id))
                .Select(p => new { p.Id, p.Price })
                .ToList();

            var existingProductIds = products.Select(p => p.Id).ToHashSet();

            var invalidProductIds = productIds.Except(existingProductIds).ToList();
            if (invalidProductIds.Any())
            {
                return new NotFoundObjectResult($"Products with IDs '{invalidProductIds}' do not exist.");
            }

            var cartProductIds = _dbCon.Carts
                .Where(c => c.UserId == userId)
                .Select(c => c.ProductId)
                .ToHashSet();

            //_logger.LogInformation($"in cart: {cartProductIds}");
            //_logger.LogInformation($" in order: {productIds}");

            var missingCartProductIds = productIds.Except(cartProductIds).ToList();
            if (missingCartProductIds.Any())
            {
                return new BadRequestObjectResult($"Products with IDs '{missingCartProductIds}' must be in the cart before placing an order.");
            }

            var totalAmount = _dbCon.Carts.Where(c => c.UserId == userId && 
                                                                  productIds.Contains(c.ProductId) && 
                                                                  !c.IsDeleted && 
                                                                   c.IsActive)
                                           .Sum(c => c.TotalPrice);


            var order = _mapper.Map<Order>(dto);
            order.UserId = userId;
            order.Amount = totalAmount;
            order.IsActive = true;
            order.CreatedAt = DateTime.UtcNow;

            _dbCon.Orders.Add(order);
            _dbCon.SaveChanges();

            var orderProducts = dto.Products.Select(p => new OrderProduct
            {
                OrderId = order.Id,
                ProductId = p.ProductId
            }).ToList();

            _dbCon.OrderProducts.AddRange(orderProducts);
            _dbCon.SaveChanges();

            var cartsToRemove = _dbCon.Carts
           .Where(c => c.UserId == userId && productIds.Contains(c.ProductId))
           .ToList();

            _dbCon.Carts.RemoveRange(cartsToRemove);
            _dbCon.SaveChanges();

            _auditLogger.LogAudit(_applicationActor.Username, "Create_Order", "Success");
            return new OkObjectResult(new { Message = "Order processed successfully!" });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error: \n{ex}\n");
            _auditLogger.LogAudit(_applicationActor.Username, "Create_Order", "Failed");
            return new StatusCodeResult(500);
        }
    }

}
