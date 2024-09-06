using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyProject.Api.Core;
using MyProject.Application.DTO;
using MyProject.Domain.Entities;
using MyProject.DataAccess;
using MyProject.Application.UseCases.Queries;
using MyProject.Application.DTO.SearchEntities;
using MyProject.Implementation.Validators;
using MyProject.Api.Extensions;
using MyProject.Api.Handlers.CategoryHandler;
using MyProject.Api.Handlers.ProductRestaurantHandler;

namespace MyProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductRestaurantController : ControllerBase
    {
        private readonly CreatePRHandler _createPR;
        private readonly DeletePRHandler _deletePR;
        private readonly GetAllPRHandler _getAllPR;

        public ProductRestaurantController(CreatePRHandler createPR, 
                                           DeletePRHandler deletePR, 
                                           GetAllPRHandler getAllPR)
        {
            _createPR = createPR;
            _deletePR = deletePR;
            _getAllPR = getAllPR;
        }

        // GET: api/ProductRestaurant
        [HttpGet]
        public IActionResult Get([FromQuery] ProductRestaurantSearchDTO search)
        {
            return _getAllPR.Handle(search);
        }

        // POST api/ProductRestaurant
        [HttpPost]
        public IActionResult Post([FromBody] ProductRestaurantDTO dto)
        {
            return _createPR.Handle(dto);
        }

        // DELETE api/productRestaurant?productId=1&restaurantId=2
        [HttpDelete]
        public IActionResult Delete([FromQuery] int productId, [FromQuery] int restaurantId)
        {
            return _deletePR.Handle(productId, restaurantId);
        }

    }
}
