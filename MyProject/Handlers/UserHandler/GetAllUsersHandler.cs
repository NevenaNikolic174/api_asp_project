using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyProject.Api.DTO;
using MyProject.Application.DTO;
using MyProject.Application.DTO.SearchEntities;
using MyProject.Application.UseCases.Queries;
using MyProject.DataAccess;

namespace MyProject.Api.Handlers.UserHandler
{
    public class GetAllUsersHandler
    {
        private readonly MyDBContext _dbCon;
        private readonly IMapper _mapper;

        public GetAllUsersHandler(MyDBContext context, IMapper mapper)
        {
            _dbCon = context;
            _mapper = mapper;
        }

        public IActionResult Handle(SearchByUserUsername search)
        {
            var query = _dbCon.Users.AsQueryable();

            if (!string.IsNullOrEmpty(search?.Username))
            {
                var searchTerm = search.Username.Trim().ToLower();
                query = query.Where(c => c.Username.ToLower().Contains(searchTerm));
            }
            if (search.OrderByUsername.HasValue)
            {
                query = search.OrderByUsername.Value == SearchByUserUsername.OrderByFunction.ASC ?
                    query.OrderBy(p => p.Username) :
                    query.OrderByDescending(p => p.Username);
            }

            var totalCount = query.Count();

            if (totalCount == 0)
            {
                if (!string.IsNullOrEmpty(search?.Username))
                {
                    return new NotFoundObjectResult($"No data found with name '{search.Username}'.");
                }
                return new NotFoundObjectResult("No data in the database.");
            }

            var page = search.CalculatePage(totalCount);
            var maxPage = search.CalculateMaxPage(totalCount);

            if (page > maxPage)
            {
                return new NotFoundObjectResult($"No data for page {page}.");
            }

            var users = query
               .Skip((page - 1) * search.PerPage)
               .Take(search.PerPage)
               .Select(c => _mapper.Map<UserDTO>(c))
               .ToList();

            return new OkObjectResult(new PageSearchResponse<UserDTO>
            {
                TotalCount = totalCount,
                CurrentPage = page,
                ItemsPerPage = search.PerPage,
                Items = users
            });
        }
    }
}
