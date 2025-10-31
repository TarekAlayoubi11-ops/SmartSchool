using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartSchool.BLL;
using SmartSchool.DTOs;

namespace SmartSchool.Controllers
{
    [Route("api/Subjects")]
    [ApiController]
    public class SubjectsController : ControllerBase
    {
        private readonly string _connectionString;
        public SubjectsController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException("Connection string 'DefaultConnection' not found.");
        }
  


        [HttpGet]
        public ActionResult<IEnumerable<SubjectDTO>> GetAllActiveSubjects()
        {
            var result = SubjectBll.GetAllActiveSubjects(_connectionString);
            if (!result.Success)
                return result.Message!.Contains("No subjects") ? NotFound(result.Message) : StatusCode(500, result.Message);

            return Ok(result.Data);
        }

  
        
        
        
        [HttpPost]
        public ActionResult CreateSubject([FromBody] CreateSubjectDTO subject)
        {
            if (subject == null)
                return BadRequest("Student data is required.");

            if (string.IsNullOrWhiteSpace(subject.SubjectName))
                return BadRequest("SubjectName is required.");

            if (string.IsNullOrWhiteSpace(subject.SubjectCode))
                return BadRequest("SubjectCode is required.");
          
            if (subject.CreditHours <= 0)
                return BadRequest("CreditHours is invalid.");

            var result = SubjectBll.CreateSubject(subject, _connectionString);
            return result.Code switch
            {
                > 0 => Ok(new { SubjectId = result.Code, Message = result.Message }),
                -2 => Conflict(result.Message),
                _ => StatusCode(500, result.Message)
            };
        }

    
        
        
        
        [HttpPut]
        public ActionResult UpdateSubject([FromBody] UpdateSubjectDTO subject)
        {
            if (subject == null)
                return BadRequest("Student data is required.");

            if (string.IsNullOrWhiteSpace(subject.SubjectName))
                return BadRequest("SubjectName is required.");

            if (string.IsNullOrWhiteSpace(subject.SubjectCode))
                return BadRequest("SubjectCode is required.");


            if (subject.CreditHours <= 0)
                return BadRequest("CreditHours is invalid.");

            if (subject.SubjectId <= 0)
                return BadRequest("SubjectId is invalid.");


            var result = SubjectBll.UpdateSubject(subject, _connectionString);
            return result.Code switch
            {
                1 => Ok(new { Message = result.Message }),
                -2 => Conflict(result.Message),
                -3 => NotFound(result.Message),
                _ => StatusCode(500, result.Message)
            };
        }

     
   




        [HttpDelete("{id}")]
        public ActionResult DeleteSubject(int id)
        {
            if (id <= 0)
                return BadRequest("SubjectId is invalid.");

            var result = SubjectBll.DeleteSubject(id, _connectionString);
            return result.Code switch
            {
                1 => Ok(new { Message = result.Message }),
                -2 => Conflict(result.Message),
                -3 => NotFound( result.Message),
                _ => StatusCode(500, result.Message)
            };
        }



        [HttpGet("{id}")]
        public ActionResult<SubjectDTO> GetStudentById(int id)
        {
            if (id <= 0)
                return BadRequest("SubjectId is invalid.");

            var result = SubjectBll.GetSubjectById(id, _connectionString);

            if (!result.Success)
            {
                if (result.Message!.Contains("not found"))
                    return NotFound(result.Message);
                else
                    return StatusCode(500, result.Message);
            }

            return Ok(result.Data);
        }
    }
}
