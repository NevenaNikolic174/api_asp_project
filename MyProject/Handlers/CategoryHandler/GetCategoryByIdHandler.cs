using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyProject.Api.DTO;
using MyProject.Application.DTO;
using MyProject.DataAccess;

namespace MyProject.Api.Handlers.CategoryHandler
{
    public class GetCategoryByIdHandler
    {
        private readonly MyDBContext _dbCon;
        private readonly IMapper _mapper;

        public GetCategoryByIdHandler(MyDBContext context, IMapper mapper)
        {
            _dbCon = context;
            _mapper = mapper;
        }

        public IActionResult Handle(int id)
        {
            var category = _dbCon.Categories
                .Where(p => p.Id == id)
                .Select(c => _mapper.Map<CategoryDTO>(c))
                .FirstOrDefault();

            if (category == null)
            {
                return new NotFoundObjectResult("Category not found.");
            }

            return new OkObjectResult(category);
        }
    }
}
