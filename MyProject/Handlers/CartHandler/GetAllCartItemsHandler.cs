using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyProject.Application.DTO.SearchEntities;
using MyProject.Application.DTO;
using MyProject.Application.UseCases.Queries;
using MyProject.DataAccess;

namespace MyProject.Api.Handlers.CartHandler
{
    public class GetAllCartItemsHandler
    {
        private readonly MyDBContext _dbCon;
        private readonly IMapper _mapper;

        public GetAllCartItemsHandler(MyDBContext context, IMapper mapper)
        {
            _dbCon = context;
            _mapper = mapper;
        }

        public IActionResult Handle(SearchByNameDTO search)
        {
            var query = _dbCon.Carts.AsQueryable();

            var totalCount = query.Count();

            if (totalCount == 0)
            {
                return new NotFoundObjectResult("No data in the database.");
            }

            var page = search.CalculatePage(totalCount);
            var maxPage = search.CalculateMaxPage(totalCount);

            if (page > maxPage)
            {
                return new NotFoundObjectResult($"No data for page {page}.");
            }

            var cartItems = query
               .Skip((page - 1) * search.PerPage)
               .Take(search.PerPage)
               .Select(c => _mapper.Map<CartItemDTO>(c))
               .ToList();

            return new OkObjectResult(new PageSearchResponse<CartItemDTO>
            {
                TotalCount = totalCount,
                CurrentPage = page,
                ItemsPerPage = search.PerPage,
                Items = cartItems
            });
        }
    }
}
