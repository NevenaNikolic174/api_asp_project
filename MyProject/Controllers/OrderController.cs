using Microsoft.AspNetCore.Mvc;
using MyProject.Application.DTO;
using MyProject.Application.UseCases.Queries;

namespace MyProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly CreateOrderHandler _createOrderHandler;

        public OrderController(CreateOrderHandler createOrderHandler)
        {
            _createOrderHandler = createOrderHandler;
        }

        [HttpPost]
        [AuthorizeRole(1)]
        public IActionResult Post([FromBody] OrderDTO search)
        {
            return _createOrderHandler.Handle(search);
        }
    }
}
