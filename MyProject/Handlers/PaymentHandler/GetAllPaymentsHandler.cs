using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyProject.Application.DTO.SearchEntities;
using MyProject.Application.DTO;
using MyProject.Application.UseCases.Queries;
using MyProject.DataAccess;

namespace MyProject.Api.Handlers.PaymentHandler
{
    public class GetAllPaymentsHandler
    {
        private readonly MyDBContext _dbCon;
        private readonly IMapper _mapper;

        public GetAllPaymentsHandler(MyDBContext context, IMapper mapper)
        {
            _dbCon = context;
            _mapper = mapper;
        }

        public IActionResult Handle(SearchByPaymentMethodDTO search)
        {
            var query = _dbCon.Payments.AsQueryable();

            if (!string.IsNullOrEmpty(search?.Method))
            {
                var searchTerm = search.Method.Trim().ToLower();
                query = query.Where(c => c.Method.ToLower().Contains(searchTerm));
            }
           
            if (search.OrderByMethod.HasValue)
            {
                query = search.OrderByMethod.Value == OrderByFunction.ASC ?
                    query.OrderBy(p => p.Method) :
                    query.OrderByDescending(p => p.Method);
            }
            var totalCount = query.Count();

            if (totalCount == 0)
            {
                if (!string.IsNullOrEmpty(search?.Method))
                {
                    return new NotFoundObjectResult($"No data found with name '{search.Method}'.");
                }
                return new NotFoundObjectResult("No data in the database.");
            }

            var page = search.CalculatePage(totalCount);
            var maxPage = search.CalculateMaxPage(totalCount);

            if (page > maxPage)
            {
                return new NotFoundObjectResult($"No data for page {page}.");
            }

            var payments = query
               .Skip((page - 1) * search.PerPage)
               .Take(search.PerPage)
               .Select(c => _mapper.Map<PaymentDTO>(c))
               .ToList();

            return new OkObjectResult(new PageSearchResponse<PaymentDTO>
            {
                TotalCount = totalCount,
                CurrentPage = page,
                ItemsPerPage = search.PerPage,
                Items = payments
            });
        }
    }
}
