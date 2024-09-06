using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyProject.Api.DTO;
using MyProject.Api.Extensions;
using MyProject.Api.Handlers.CartHandler;
using MyProject.Api.Handlers.CategoryHandler;
using MyProject.Api.Handlers.ProductOperations;
using MyProject.Application.Actor;
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
    public class CartController : ControllerBase
    {
        private readonly MyDBContext _dbContext;
        private readonly IApplicationActor _applicationActor;
        public CreateCartValidator _validator;

        private readonly CreateCartHandler _createCartHandler;
        private readonly UpdateCartHandler _updateCartHandler;
        private readonly DeleteCartHandler _deleteCartHandler;
        private readonly GetAllCartItemsHandler _getAllCartItemsHandler;

        public CartController(MyDBContext dbContext, 
                              IApplicationActor applicationActor, 
                              CreateCartValidator validator, 
                              CreateCartHandler createCartHandler, 
                              UpdateCartHandler updateCartHandler, 
                              DeleteCartHandler deleteCartHandler,
                              GetAllCartItemsHandler getAllCartItemsHandler)
        {
            _dbContext = dbContext;
            _applicationActor = applicationActor;
            _validator = validator;
            _createCartHandler = createCartHandler;
            _updateCartHandler = updateCartHandler;
            _deleteCartHandler = deleteCartHandler;
            _getAllCartItemsHandler = getAllCartItemsHandler;
        }


        // GET api/cart
        [HttpGet]
        [AuthorizeRole(1)]
        public IActionResult Get([FromQuery] SearchByNameDTO search)
        {
            return _getAllCartItemsHandler.Handle(search);
        }

        // POST api/cart
        [HttpPost]
        [AuthorizeRole(1)]
        public IActionResult Post([FromBody] CartDTO dto)
        {
            return _createCartHandler.Handle(dto);
        }


        // PUT api/cart/{id}
        [HttpPut("{id}")]
        [AuthorizeRole(1)]
        public IActionResult Put(int id, [FromBody] CartItemDTO dto)
        {
            return _updateCartHandler.Handle(id, dto);
        }

        // DELETE api/cart/{id}
        [HttpDelete("{id}")]
        [AuthorizeRole(1)]
        public IActionResult Delete(int id)
        {
            return _deleteCartHandler.Handle(id);
        }
    }
}
