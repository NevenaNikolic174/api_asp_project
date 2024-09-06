using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using MyProject.Api.Core.Token;
using MyProject.Api.DTO;
using MyProject.Api.Extensions;
using MyProject.Implementation.Exceptions;
using Projekat.Api.Core;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtTokenCreator _tokenCreator;
        public AuthController(JwtTokenCreator tokenCreator)
        {
            _tokenCreator = tokenCreator;
        }

        // POST api/<AuthController>
        [HttpPost]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new { Message = "Email and password is required." });
            }

            try
            {
                var token = _tokenCreator.Create(request.Email, request.Password);
                return Ok(new { Token = token });
            }
            catch (WrongCredentialsException ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
        }


        // DELETE api/<AuthController>
        [HttpDelete]
        [AuthorizeRole(2)]
        public IActionResult Delete([FromServices] ITokenStorage storage)
        {
            storage.Remove(this.Request.GetTokenId().Value);
            return NoContent();
        }
    }
}