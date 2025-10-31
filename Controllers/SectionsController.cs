using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartSchool.BLL;
using SmartSchool.DAL;
using SmartSchool.DTOs;
using static System.Collections.Specialized.BitVector32;

namespace SmartSchool.Controllers
{
    [Route("api/Sections")]
    [ApiController]
    public class SectionsController : ControllerBase
    {
        private readonly string _connectionString;
        public SectionsController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
    ?? throw new ArgumentNullException("Connection string 'DefaultConnection' not found.");
        }




        [HttpGet("Sections")]
        public ActionResult<IEnumerable<SectionDTO>> GetAllActiveSections()
        {
            var result = SectionBll.GetAllActiveSections(_connectionString);

            if (!result.Success)
            {
                if (result.Message!.Contains("No sections"))
                    return NotFound(result.Message);
                else
                    return StatusCode(500, result.Message);
            }

            return Ok(result.Data);
        }




        [HttpGet("{id}")]
        public ActionResult<SectionDTO> GetSectionById(int id)
        {
            if (id <= 0)
                return BadRequest("SectionId is invalid.");

            var result = StudentBll.GetStudentById(id, _connectionString);

            if (!result.Success)
            {
                if (result.Message!.Contains("No sections"))
                    return NotFound(result.Message);
                else
                    return StatusCode(500, result.Message);
            }

            return Ok(result.Data);
        }





        [HttpPost]
        public ActionResult CreateSection([FromBody] CreateSectionDTO section)
        {
            if (section == null)
                return BadRequest("Student data is required.");

            if (string.IsNullOrWhiteSpace(section.SectionName))
                return BadRequest("SectionName is required.");

            if (section.Capacity <= 0)
                return BadRequest("Capacity is invalid.");

            if (section.HomeroomTeacherId <= 0)
                return BadRequest("HomeroomTeacherId is invalid.");


            if (section.ClassId <= 0)
                return BadRequest("ClassId is invalid.");

            var result = SectionBll.CreateSection(section, _connectionString);
            return result.Code switch
            {
                > 0 => Ok(new { SectionId = result.Code, Message = result.Message }),
                -2 => BadRequest(result.Message),
                -3 => BadRequest(result.Message),
                -4 => Conflict(result.Message),
                _ => StatusCode(500, result.Message)
            };
            
        }



        [HttpDelete("{id}")]
        public ActionResult DeleteSection(int id)
        {

            if (id <= 0)
                return BadRequest("SectionId is invalid.");


            var result = SectionBll.DeleteSection(id, _connectionString);

            return result.Code switch
            {
                1 => Ok(new { Message = result.Message}),
                -2 => NotFound(new { Message = result.Message }),
                -3 => Conflict(new { Message = result.Message }),
                _ => StatusCode(500, result.Message)              
            };
        }




        [HttpPut]
        public ActionResult UpdateSection([FromBody] UpdateSectionDTO section)
        {
            if (section == null)
                return BadRequest("Student data is required.");

            if (string.IsNullOrWhiteSpace(section.SectionName))
                return BadRequest("SectionName is required.");

            if (section.Capacity <= 0)
                return BadRequest("Capacity is invalid.");

            if (section.SectionId <= 0)
                return BadRequest("SectionId is invalid.");

            if (section.HomeroomTeacherId <= 0)
                return BadRequest("HomeroomTeacherId is invalid.");

            if (section.ClassId <= 0)
                return BadRequest("ClassId is invalid.");

            var result = SectionBll.UpdateSection(section, _connectionString);

            return result.Code switch
            {
                1 => Ok(new { Message = result.Message }),
                -2 => NotFound(result.Message),
                -3 => NotFound(result.Message),
                -4 => NotFound(result.Message),
                -5 => Conflict(result.Message),
                _ => StatusCode(500, result.Message),
            };
        }
    }
}
