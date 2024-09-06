using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyProject.Application.DTO.SearchEntities;
using MyProject.Application.DTO;
using MyProject.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using MyProject.Application.UseCases.Queries;
using MyProject.Api.Core;
using MyProject.Application.Actor;

namespace MyProject.Api.Handlers.ProductRestaurantHandler
{
    public class GetAllPRHandler
    {
        private readonly MyDBContext _dbCon;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllPRHandler> _logger;
        private readonly AuditLogger _auditLogger;
        private readonly IApplicationActor _applicationActor;

        public GetAllPRHandler(MyDBContext context, IMapper mapper, ILogger<GetAllPRHandler> logger)
        {
            _dbCon = context;
            _mapper = mapper;
            _logger = logger;
        }

        public IActionResult Handle(ProductRestaurantSearchDTO search)
        {
            try
            {
                var query = _dbCon.ProductRestaurants
                    .Include(pr => pr.Product)
                    .Include(pr => pr.Restaurant)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(search?.ProductName) || !string.IsNullOrEmpty(search?.RestaurantName))
                {
                    var productSearchTerm = search.ProductName?.Trim().ToLower();
                    var restaurantSearchTerm = search.RestaurantName?.Trim().ToLower();

                    query = query.Where(pr =>
                        (string.IsNullOrEmpty(productSearchTerm) || pr.Product.Name.ToLower().Contains(productSearchTerm)) &&
                        (string.IsNullOrEmpty(restaurantSearchTerm) || pr.Restaurant.Name.ToLower().Contains(restaurantSearchTerm))
                    );
                }

                var totalCount = query.Count();

                if (totalCount == 0)
                {
                    string message = "No data in database.";
                    if (!string.IsNullOrEmpty(search?.ProductName) || !string.IsNullOrEmpty(search?.RestaurantName))
                    {
                        message = "No data found with the specified criteria.";
                        if (!string.IsNullOrEmpty(search?.ProductName))
                        {
                            message += $" for product '{search.ProductName}'";
                        }
                        if (!string.IsNullOrEmpty(search?.RestaurantName))
                        {
                            message += $" for restaurant '{search.RestaurantName}'";
                        }
                        message += ".";
                    }
                    return new NotFoundObjectResult(message);
                }

                var page = search.Page > 0 ? search.Page : 1;
                var itemsPerPage = search.PerPage > 0 ? search.PerPage : totalCount;

                var maxPage = (totalCount + itemsPerPage - 1) / itemsPerPage;
                if (page > maxPage)
                {
                    return new NotFoundObjectResult($"No data for page: {page}.");
                }

                var productRestaurants = query
                    .Skip((page - 1) * itemsPerPage)
                    .Take(itemsPerPage)
                    .ToList();

                var resultDTOs = productRestaurants.Select(pr => new ProductRestaurantDTO
                {
                    ProductName = pr.Product.Name,
                    RestaurantName = pr.Restaurant.Name,
                }).ToList();

                var response = new PageSearchResponse<ProductRestaurantDTO>
                {
                    TotalCount = totalCount,
                    CurrentPage = page,
                    ItemsPerPage = itemsPerPage,
                    Items = resultDTOs
                };

                return new OkObjectResult(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error.");
                return new StatusCodeResult(500);
            }
        }
    }
}
