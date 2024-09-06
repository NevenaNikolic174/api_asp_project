using AutoMapper;
using AutoMapper.QueryableExtensions;
using Azure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyProject.Api.Core;
using MyProject.Api.DTO;
using MyProject.Api.Extensions;
using MyProject.Api.Handlers.CategoryHandler;
using MyProject.Application.DTO;
using MyProject.Application.DTO.SearchEntities;
using MyProject.Application.UseCases.Queries;
using MyProject.DataAccess;
using MyProject.Domain.Entities;
using MyProject.Implementation.Validators;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {

        private readonly GetAllCategoriesHandler _getCategoryHandler;
        private readonly GetCategoryByIdHandler _getCategoryByIdHandler;
        private readonly CreateCategoryHandler _createCategoryHandler;
        private readonly UpdateCategoryHandler _updateCategoryHandler;
        private readonly DeleteCategoryHandler _deleteCategoryHandler;

       public CategoryController(GetAllCategoriesHandler getCategoryHandler, 
                                 GetCategoryByIdHandler getCategoryByIdHandler, 
                                 CreateCategoryHandler createCategoryHandler, 
                                 UpdateCategoryHandler updateCategoryHandler, 
                                 DeleteCategoryHandler deleteCategoryHandler)
        {
            _getCategoryHandler = getCategoryHandler;
            _getCategoryByIdHandler = getCategoryByIdHandler;
            _createCategoryHandler = createCategoryHandler;
            _updateCategoryHandler = updateCategoryHandler;
            _deleteCategoryHandler = deleteCategoryHandler;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] SearchByNameDTO search)
        {
            return _getCategoryHandler.Handle(search);
        }

        [HttpGet("{id}")]
        [AuthorizeRole(2)]
        public IActionResult Get(int id)
        {
            return _getCategoryByIdHandler.Handle(id);
        }

        [HttpPost]
        [AuthorizeRole(2)]
        public IActionResult Post([FromBody] CategoryDTO dto)
        {
            return _createCategoryHandler.Handle(dto);
        }

        [HttpPut("{id}")]
        [AuthorizeRole(2)]
        public IActionResult Put(int id, [FromBody] CategoryDTO dto)
        {
            return _updateCategoryHandler.Handle(id, dto);
        }

        [HttpDelete("{id}")]
        [AuthorizeRole(2)]
        public IActionResult Delete(int id)
        {
            return _deleteCategoryHandler.Handle(id);
        }
    }
}
