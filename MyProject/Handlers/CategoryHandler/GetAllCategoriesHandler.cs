﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyProject.Api.DTO;
using MyProject.Application.DTO;
using MyProject.Application.DTO.SearchEntities;
using MyProject.Application.UseCases.Queries;
using MyProject.DataAccess;

namespace MyProject.Api.Handlers.CategoryHandler
{
    public class GetAllCategoriesHandler
    {
        private readonly MyDBContext _dbCon;
        private readonly IMapper _mapper;

        public GetAllCategoriesHandler(MyDBContext context, IMapper mapper)
        {
            _dbCon = context;
            _mapper = mapper;
        }

        public IActionResult Handle(SearchByNameDTO search)
        {
            var query = _dbCon.Categories.AsQueryable();

            if (!string.IsNullOrEmpty(search?.Name))
            {
                var searchTerm = search.Name.Trim().ToLower();
                query = query.Where(c => c.Name.ToLower().Contains(searchTerm));
            }
            if (search.OrderByName.HasValue)
            {
                query = search.OrderByName.Value == SearchByNameDTO.OrderByFunction.ASC ?
                    query.OrderBy(p => p.Name) :
                    query.OrderByDescending(p => p.Name);
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

            var categories = query
               .Skip((page - 1) * search.PerPage)
               .Take(search.PerPage)
               .Select(c => _mapper.Map<CategoryDTO>(c))
               .ToList();

            return new OkObjectResult(new PageSearchResponse<CategoryDTO>
            {
                TotalCount = totalCount,
                CurrentPage = page,
                ItemsPerPage = search.PerPage,
                Items = categories
            });
        }
    }
}
