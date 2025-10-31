using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartSchool.BLL;
using SmartSchool.DTOs;

namespace SmartSchool.Controllers
{
    [Route("api/Classes")]
    [ApiController]
    public class ClassesController : ControllerBase
    {
        private readonly string _connectionString;
        public ClassesController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
    ?? throw new ArgumentNullException("Connection string 'DefaultConnection' not found.");
        }



        [HttpGet("Students")]
        public ActionResult<IEnumerable<StudentDTO>> GetAllClasses()
        {
            var result = ClassBll.GetAllClasses(_connectionString);

            if (!result.Success)
            {
                if (result.Message!.Contains("No classes"))
                    return NotFound(result.Message);
                else
                    return StatusCode(500, result.Message);
            }

            return Ok(result.Data);
        }
    }
}
