using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartSchool.BLL;
using SmartSchool.DTOs;

namespace SmartSchool.Controllers
{
    [Route("api/Teachers")]
    [ApiController]
    public class TeachersController : ControllerBase
    {
        private readonly string _connectionString;
        public TeachersController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
    ?? throw new ArgumentNullException("Connection string 'DefaultConnection' not found.");
        }





        [HttpGet("Teachers")]
        public ActionResult<IEnumerable<StudentDTO>> GetAllActiveTeacherss()
        {
            var result = TeacherBll.GetAllActiveTeachers(_connectionString);

            if (!result.Success)
            {
                if (result.Message!.Contains("No teachers"))
                    return NotFound(result.Message);
                else
                    return StatusCode(500, result.Message);
            }

            return Ok(result.Data);
        }




        [HttpGet("{id}")]
        public ActionResult<TeacherDTO> GetTeacherById(int id)
        {
            if (id <= 0)
                return BadRequest("TeacherId is invalid.");

            var result = TeacherBll.GetTeacherById(id, _connectionString);

            if (!result.Success)
            {
                if (result.Message!.Contains("not found"))
                    return NotFound(result.Message);
                else
                    return StatusCode(500, result.Message);
            }

            return Ok(result.Data);
        }






        [HttpDelete("{id}")]
        public ActionResult DeleteTeacher(int id)
        {

            if (id <= 0)
                return BadRequest("TeacherId is invalid.");


            var result = TeacherBll.DeleteTeacher(id, _connectionString);

            return result.Code switch
            {
                1 => Ok(new { Message = result.Message }),
                -2 => NotFound(result.Message),
                _ => StatusCode(500, result.Message)
            };
        }




        [HttpPost]
        public ActionResult CreateTeacher([FromBody] CreateTeacherDTO teacher)
        {
            if (teacher == null)
                return BadRequest("Teacher data is required.");

            if (string.IsNullOrWhiteSpace(teacher.Specialization))
                return BadRequest("Specialization is required.");
            
            if (string.IsNullOrWhiteSpace(teacher.FullName))
                return BadRequest("Full name is required.");

            if (string.IsNullOrWhiteSpace(teacher.Email))
                return BadRequest("Email is required.");

            if (string.IsNullOrWhiteSpace(teacher.Password))
                return BadRequest("Password is required.");

       

           

            var result =  TeacherBll.CreateTeacher(teacher, _connectionString);
            return result.Code switch
            {
                > 0 => Ok(new { TeacherId = result.Code, Message = result.Message }),
                -1 => StatusCode(500, result.Message),
                -2 => Conflict(result.Message),
                _ => StatusCode(500, result.Message)
            };
        }





        [HttpPut]
        public ActionResult UpdateTeacher([FromBody] UpdateTeacherDTO teacher)
        {
            if (teacher == null)
                return BadRequest("Teacher data is required.");

            if (string.IsNullOrWhiteSpace(teacher.Specialization))
                return BadRequest("Specialization is required.");

            if (string.IsNullOrWhiteSpace(teacher.FullName))
                return BadRequest("Full name is required.");

            if (string.IsNullOrWhiteSpace(teacher.Email))
                return BadRequest("Email is required.");


            if (teacher.TeacherId <= 0)
                return BadRequest("TeacherId is invalid.");

            var result = TeacherBll.UpdateTeacher(teacher, _connectionString);

            return result.Code switch
            {
                1 => Ok(new { Message = result.Message }),
                -2 => Conflict(new { Message = result.Message }),
                -3 => NotFound(new { Message = result.Message }),
                -1 => StatusCode(500, new { Message = result.Message }),
                _ => StatusCode(500, new { Message = result.Message })
            };
        }

    }
}
