using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyProject.Api.DTO;
using MyProject.Application.DTO.SearchEntities;
using MyProject.Application.UseCases.Queries;
using MyProject.DataAccess;

namespace MyProject.Api.Handlers.ProductOperations
{
    public class GetAllProductsHandler
    {
        private readonly MyDBContext _dbCon;
        private readonly IMapper _mapper;

        public GetAllProductsHandler(MyDBContext context, IMapper mapper)
        {
            _dbCon = context;
            _mapper = mapper;
        }

        public IActionResult Handle(SearchProductsDTO search)
        {
            var query = _dbCon.Products.AsQueryable();

            if (!string.IsNullOrEmpty(search?.Name))
            {
                var searchTerm = search.Name.Trim().ToLower();
                query = query.Where(product => product.Name.ToLower().Contains(searchTerm));
            }
            if (search.PriceMin > 0)
            {
                query = query.Where(p => p.Price >= search.PriceMin);
            }
            if (search.PriceMax > 0)
            {
                query = query.Where(p => p.Price <= search.PriceMax);
            }
            if (search.OrderByName.HasValue)
            {
                query = search.OrderByName.Value == OrderByFunc.ASC ?
                    query.OrderBy(p => p.Name) :
                    query.OrderByDescending(p => p.Name);
            }
            if (search.OrderByPrice.HasValue)
            {
                query = search.OrderByPrice.Value == OrderByFunc.ASC ?
                        query.OrderBy(p => p.Price) :
                        query.OrderByDescending(p => p.Price);
            }
            if (search.OrderByYear.HasValue)
            {
                query = search.OrderByYear.Value == OrderByFunc.ASC ?
                        query.OrderBy(p => p.Year) :
                        query.OrderByDescending(p => p.Year);
            }

            var totalCount = query.Count();

            if (totalCount == 0)
            {
                if (!string.IsNullOrEmpty(search?.Name))
                {
                    return new NotFoundObjectResult($"No data found with name '{search.Name}'.");
                }
                return new NotFoundObjectResult("No data in the database.");
            }

            var page = search.CalculatePage(totalCount);
            var maxPage = search.CalculateMaxPage(totalCount);

            if (page > maxPage)
            {
                return new NotFoundObjectResult($"No data for page {page}.");
            }

            var products = query
               .Skip((page - 1) * search.PerPage)
               .Take(search.PerPage)
               .Select(p => _mapper.Map<ProductDTO>(p))
               .ToList();

            return new OkObjectResult(new PageSearchResponse<ProductDTO>
            {
                TotalCount = totalCount,
                CurrentPage = page,
                ItemsPerPage = search.PerPage,
                Items = products
            });
        }
    }
}
