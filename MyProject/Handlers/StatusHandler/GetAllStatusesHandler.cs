using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyProject.Application.DTO.SearchEntities;
using MyProject.Application.DTO;
using MyProject.Application.UseCases.Queries;
using MyProject.DataAccess;
using MyProject.Api.DTO;

namespace MyProject.Api.Handlers.StatusHandler
{
    public class GetAllStatusesHandler
    {
        private readonly MyDBContext _dbCon;
        private readonly IMapper _mapper;

        public GetAllStatusesHandler(MyDBContext context, IMapper mapper)
        {
            _dbCon = context;
            _mapper = mapper;
        }

        public IActionResult Handle(SearchByValueDTO search)
        {
            var query = _dbCon.Statuses.AsQueryable();

            if (!string.IsNullOrEmpty(search?.Value))
            {
                var searchTerm = search.Value.Trim().ToLower();
                query = query.Where(c => c.Value.ToLower().Contains(searchTerm));
            }

            if (search.OrderByValue.HasValue)
            {
                query = search.OrderByValue.Value == OrderByStatus.ASC ?
                    query.OrderBy(p => p.Value) :
                    query.OrderByDescending(p => p.Value);
            }

            var totalCount = query.Count();

            if (totalCount == 0)
            {
                if (!string.IsNullOrEmpty(search?.Value))
                {
                    return new NotFoundObjectResult($"No data found with name '{search.Value}'.");
                }
                return new NotFoundObjectResult("No data in the database.");
            }

            var page = search.CalculatePage(totalCount);
            var maxPage = search.CalculateMaxPage(totalCount);

            if (page > maxPage)
            {
                return new NotFoundObjectResult($"No data for page {page}.");
            }

            var statuses = query
               .Skip((page - 1) * search.PerPage)
               .Take(search.PerPage)
               .Select(c => _mapper.Map<StatusDTO>(c))
               .ToList();

            return new OkObjectResult(new PageSearchResponse<StatusDTO>
            {
                TotalCount = totalCount,
                CurrentPage = page,
                ItemsPerPage = search.PerPage,
                Items = statuses
            });
        }
    }
}
