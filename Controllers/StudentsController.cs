using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SmartSchool.BLL;
using SmartSchool.DTOs;
using System.ComponentModel.DataAnnotations;
using static SmartSchool.BLL.StudentBll;
namespace SmartSchool.Controllers
{
    [Route("api/Students")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly string _connectionString;
        public StudentsController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }




        [HttpGet("Students")]
        public ActionResult<IEnumerable<StudentDTO>> GetAllStudents()
        {
            var result = StudentBll.GetAllStudents(_connectionString);

            if (!result.Success)
            {
                if (result.Message.Contains("No students"))
                    return NotFound(result.Message);
                else
                    return StatusCode(500, result.Message);
            }

            return Ok(result.Data);
        }




        [HttpGet("{id}")]
        public ActionResult GetStudentById([FromBody] int id)
        {
            var result = StudentBll.GetStudentById(id, _connectionString);

            if (!result.Success)
            {
                if (result.Message.Contains("not found"))
                    return NotFound(result.Message);
                else
                    return StatusCode(500, result.Message);
            }
            
            return Ok(result.Data);
        }





        [HttpPost]
        public ActionResult CreateStudentWithEnrollment([FromBody] CreateStudentDTO student)
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

            if (student.SectionId <= 0)
                return BadRequest("SectionId is invalid.");

            var result = StudentBll.CreateStudentWithEnrollment(student, _connectionString);
            return result.Code switch
            {
                > 0 => Ok(new { StudentId = result.Code, Message = result.Message }),
                -1 => StatusCode(500, result.Message),
                -2 => Conflict(result.Message),
                -3 => NotFound(result.Message),
                _ => StatusCode(500, result.Message)
            };
        }



        [HttpDelete("{id}")]
        public ActionResult DeleteStudent([FromBody] int id)
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
