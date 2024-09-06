using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyProject.Api.Core;
using MyProject.Api.DTO;
using MyProject.Api.Extensions;
using MyProject.Api.Handlers.RestaurantHandler;
using MyProject.Application.DTO;
using MyProject.Application.DTO.SearchEntities;
using MyProject.Application.UseCases.Queries;
using MyProject.DataAccess;
using MyProject.Domain.Entities;
using MyProject.Implementation.Validators;
using System.Linq;

namespace MyProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly GetAllRestaurantsHandler _getRestaurant;
        private readonly GetRestaurantByIdHandler _getRestaurantById;
        private readonly CreateRestaurantHandler _createRestaurant;
        private readonly UpdateRestaurantHandler _updateRestaurant;
        private readonly DeleteRestaurantHandler _deleteRestaurant;

        public RestaurantController(GetAllRestaurantsHandler getRestaurant,
                                    GetRestaurantByIdHandler getRestaurantById,
                                    CreateRestaurantHandler createRestaurant,
                                    UpdateRestaurantHandler updateRestaurant,
                                    DeleteRestaurantHandler deleteRestaurant)
        {
           _getRestaurant = getRestaurant;
           _getRestaurantById = getRestaurantById;
           _createRestaurant = createRestaurant;
           _deleteRestaurant = deleteRestaurant;
           _updateRestaurant = updateRestaurant;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] SearchByNameDTO search)
        {
            return _getRestaurant.Handle(search);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return _getRestaurantById.Handle(id);
        }

        [HttpPost]
        [AuthorizeRole(2)]
        public IActionResult Post([FromBody] RestaurantDTO dto)
        {
            return _createRestaurant.Handle(dto);
        }

        [HttpPut("{id}")]
        [AuthorizeRole(2)]
        public IActionResult Put(int id, [FromBody] RestaurantDTO dto)
        {
            return _updateRestaurant.Handle(id, dto);
        }

        [HttpDelete("{id}")]
        [AuthorizeRole(2)]
        public IActionResult Delete(int id)
        {
            return _deleteRestaurant.Handle(id);
        }
    }
}
