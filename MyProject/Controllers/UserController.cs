using Microsoft.AspNetCore.Mvc;
using MyProject.Api.Handlers.UserHandler;
using MyProject.Application.DTO;
using MyProject.Application.DTO.SearchEntities;
using MyProject.Implementation.UseCases;
using System.ComponentModel.DataAnnotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly EfUserCreateCommand _userCreateCommand;
        private readonly GetAllUsersHandler _usersHandler;

        public UserController(EfUserCreateCommand userCreateCommand, GetAllUsersHandler usersHandler)
        {
            _userCreateCommand = userCreateCommand;
            _usersHandler = usersHandler;
        }

        [HttpGet]
        [AuthorizeRole(2)]
        public IActionResult Get([FromQuery] SearchByUserUsername search)
        {
            return _usersHandler.Handle(search);
        }

        [HttpPost]
        [Route("register")]
        public IActionResult RegisterUser([FromBody] UserDTO userDto)
        {
            try
            {
                _userCreateCommand.Execute(userDto);
                return Ok(new { message = "User registered successfully!" });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                var innerExceptionMessage = ex.InnerException?.Message;
                return StatusCode(500, new { details = ex.Message, innerException = innerExceptionMessage });
            }
        }

    }
}
