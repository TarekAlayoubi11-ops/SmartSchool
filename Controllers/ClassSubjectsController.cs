using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartSchool.BLL;
using SmartSchool.DTOs;

namespace SmartSchool.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassSubjectsController : ControllerBase
    {
        private readonly string _connectionString;
        public ClassSubjectsController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException("Connection string 'DefaultConnection' not found.");
        }



        [HttpPost]
        public ActionResult CreateClassSubject([FromBody] CreateClassSubjectDTO dto)
        {
            if (dto.ClassId <= 0 )
                return BadRequest("Invalid ClassId ");

            if (dto.SubjectId <= 0)
                return BadRequest("Invalid SubjectId.");


            var result = ClassSubjectBll.CreateClassSubject(dto, _connectionString);

            return result.Code switch
            {
                > 0 => Ok(new { ClassSubjectId = result.Code, result.Message }),
                -2 => Conflict(result.Message),
                -3 => NotFound(result.Message),
                -4 => NotFound(result.Message),
                _ => StatusCode(500, result.Message)
            };
        }

       
        
        
        [HttpGet("{classId}")]
        public ActionResult<IEnumerable<ClassSubjectDTO>> GetClassSubjectsByClass(int classId)
        {
            var result = ClassSubjectBll.GetClassSubjectsByClass(classId, _connectionString);

            if (!result.Success)
            {
                if (result.Message!.Contains("No subjects"))
                    return NotFound(result.Message);
                else
                    return StatusCode(500, result.Message);
            }

            return Ok(result.Data);
        }

      
        
        
        [HttpDelete("{id}")]
        public ActionResult DeleteClassSubject(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid ClassSubjectId.");

            var result = ClassSubjectBll.DeleteClassSubject(id, _connectionString);

            return result.Code switch
            {
                1 => Ok(new { result.Message }),
                -2 => NotFound(result.Message),
                _ => StatusCode(500, result.Message)
            };
        }
    }
}
