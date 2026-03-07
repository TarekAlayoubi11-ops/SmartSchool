using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SmartSchool.BLL;
using SmartSchool.DTOs;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using static SmartSchool.BLL.StudentBll;
namespace SmartSchool.Controllers
{
    [Authorize]
    [Route("api/Students")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly string _connectionString;
        public StudentsController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
    ?? throw new ArgumentNullException("Connection string 'DefaultConnection' not found.");
        }

        [Authorize("Admin")]
        [HttpGet("Students")]
        public ActionResult<IEnumerable<StudentDTO>> GetAllActiveStudents()
        {
            var result = StudentBll.GetAllActiveStudents(_connectionString);

            if (!result.Success)
            {
                if (result.Message!.Contains("No students"))
                    return NotFound(result.Message);
                else
                    return StatusCode(500, result.Message);
            }

            return Ok(result.Data);
        }

        [Authorize("Admin,Student")]
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentDTO>> GetStudentByIdAsync(int id, [FromServices] IAuthorizationService authorizationService)
        {
            if (id < 1)
                return BadRequest("Invalid Student id.");

            var Student = StudentBll.GetStudentById(id, _connectionString);

            if (Student == null)
                return NotFound("Student not found.");

            var authResult = await authorizationService.AuthorizeAsync(
                User,
                id,
                "UserOwnerOrAdmin");

            if (!authResult.Succeeded)
                return Forbid();

            return Ok(Student);
        }

        [Authorize("Admin")]
        [HttpPost]
        public ActionResult CreateStudent([FromBody] CreateStudentDTO student)
        {
            if (student == null)
                return BadRequest("Student data is required.");

            if (string.IsNullOrWhiteSpace(student.GuardianPhone))
                return BadRequest("GuardianPhone is required.");

            if (string.IsNullOrWhiteSpace(student.GuardianName))
                return BadRequest("GuardianName is required.");

            if (string.IsNullOrWhiteSpace(student.FullName))
                return BadRequest("Full name is required.");

            if (string.IsNullOrWhiteSpace(student.Email))
                return BadRequest("Email is required.");

            if (string.IsNullOrWhiteSpace(student.Password))
                return BadRequest("Password is required.");

            if (string.IsNullOrWhiteSpace(student.Gender))
                return BadRequest("Gender is required.");



            var result = StudentBll.CreateStudent(student, _connectionString);
            return result.Code switch
            {
                > 0 => Ok(new { StudentId = result.Code, Message = result.Message }),
                -1 => StatusCode(500, result.Message),
                -2 => Conflict(result.Message),
                _ => StatusCode(500, result.Message)
            };
        }

        [Authorize("Admin")]
        [HttpDelete("{id}")]
        public ActionResult DeleteStudent(int id)
        {

            if (id <= 0)
                return BadRequest("StudentId is invalid.");


            var result = StudentBll.DeleteStudent(id, _connectionString);

            return result.Code switch
            {
                1 => Ok(new { Message = result.Message }),
                -1 => StatusCode(500, result.Message),
                -2 => NotFound(result.Message),
                _ => StatusCode(500, result.Message)
            };
        }

        [Authorize("Admin")]
        [HttpPut]
        public ActionResult UpdateStudent([FromBody] UpdateStudentDTO student)
        {
            if (student == null)
                return BadRequest("Student data is required.");

            if (string.IsNullOrWhiteSpace(student.GuardianPhone))
                return BadRequest("GuardianPhone is required.");

            if (string.IsNullOrWhiteSpace(student.GuardianName))
                return BadRequest("GuardianName is required.");

            if (string.IsNullOrWhiteSpace(student.FullName))
                return BadRequest("Full name is required.");

            if (string.IsNullOrWhiteSpace(student.Email))
                return BadRequest("Email is required.");

            if (string.IsNullOrWhiteSpace(student.Gender))
                return BadRequest("Gender is required.");

            if (student.SectionId <= 0)
                return BadRequest("SectionId is invalid.");

            if (student.StudentId <= 0)
                return BadRequest("StudentId is invalid.");

            var result = StudentBll.UpdateStudent(student, _connectionString);

            return result.Code switch
            {
                1 => Ok(new { Message = result.Message }),
                -2 => Conflict(new { Message = result.Message }),
                -3 => NotFound(new { Message = result.Message }),
                -4 => NotFound(new { Message = result.Message }),
                -1 => StatusCode(500, new { Message = result.Message }),
                _ => StatusCode(500, new { Message = result.Message })
            };
        }
    }
}
