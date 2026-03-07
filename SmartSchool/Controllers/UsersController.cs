
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartSchool.BLL;
using SmartSchool.DTOs;
using System.Security.Claims;

namespace BankingPro.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/Users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly string _connectionString;
        public UsersController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException("Connection string 'DefaultConnection' not found.");
        }

        [HttpPost]
        public IActionResult CreateUser(CreateUserDTO dto)
        {
            var result = UserBll.CreateUser(dto, _connectionString);
            if (!result.Success)
            {
                if (result.Message == "All fields are required.")
                    return BadRequest();

                if (result.Message == "Unable to create user. Please check your information.")
                    return Conflict();


                return StatusCode(500);
            }
            return Ok(result.Data);
        }

        [HttpPut]
        public IActionResult UpdateUser([FromBody] UpdateUserDTO dto)
        {
            var result = UserBll.UpdateUser(dto, _connectionString);
            if (!result.Success)
            {
                if (result.Message == "Invalid input data.")
                    return BadRequest(result);

                if (result.Message == "User not found.")
                    return NotFound(result);

                if (result.Message == "Unable to update user. Please check your information.")
                    return Conflict(result);

                return StatusCode(500, result);
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var result = UserBll.DeleteUser(id, _connectionString);

            if (!result.Success)
            {
                if (result.Message == "Invalid user id.")
                    return BadRequest(result);

                if (result.Message == "User not found.")
                    return NotFound(result);

                return StatusCode(500, result);
            }

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUserByIdAsync(int id, [FromServices] IAuthorizationService authorizationService)
        {
            if (id < 1)
                return BadRequest("Invalid student id.");

            var result = UserBll.GetUserById(id, _connectionString);
            var user = result.Data;

            if (user == null)
                return NotFound("Student not found.");

            var authResult = await authorizationService.AuthorizeAsync(
                User,
                id,
                "UserOwnerOrAdmin");

            if (!authResult.Succeeded)
                return Forbid();

            return Ok(user);
        }

        [HttpGet]
        public ActionResult<List<UserDTO>> GetAllUsers()
        {
            var result = UserBll.GetAllUsers(_connectionString);

            if (!result.Success)
            {
                if (result.Message == "No users found.")
                    return NotFound();

                return StatusCode(500, result);
            }

            return Ok(result);

        }
    }
}
