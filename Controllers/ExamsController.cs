using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartSchool.BLL;
using SmartSchool.DTOs;

namespace SmartSchool.Controllers
{
    [Route("api/Exams")]
    [ApiController]
    public class ExamsController : ControllerBase
    {
        private readonly string _connectionString;

        public ExamsController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        

        [HttpGet]
        public ActionResult<IEnumerable<ExamDTO>> GetAllActiveExams()
        {
            var result = ExamBll.GetAllActiveExams(_connectionString);
            if (!result.Success)
                return result.Message!.Contains("No exams") ? NotFound(result.Message) : StatusCode(500, result.Message);
            return Ok(result.Data);
        }





        [HttpPost]
        public ActionResult CreateExam([FromBody] CreateExamDTO exam)
        {
            var result = ExamBll.CreateExam(exam, _connectionString);
            return result.Code switch
            {
                > 0 => Ok(new { ExamId = result.Code, Message = result.Message }),
                -2 => Conflict(result.Message),
                -3 => BadRequest(result.Message),
                _ => StatusCode(500, result.Message)
            };
        }

        
        
        
        [HttpPut]
        public ActionResult UpdateExam([FromBody] UpdateExamDTO exam)
        {
            var result = ExamBll.UpdateExam(exam, _connectionString);
            return result.Code switch
            {
                1 => Ok(result.Message),
                -2 => NotFound(result.Message),
                -3 => Conflict(result.Message),
                -4 => BadRequest(result.Message),
                _ => StatusCode(500, result.Message)
            };
        }

        
        
        
        [HttpDelete("{id}")]
        public ActionResult DeleteExam(int id)
        {
            var result = ExamBll.DeleteExam(id, _connectionString);
            return result.Code switch
            {
                1 => Ok(result.Message),
                -2 => NotFound(result.Message),
                -3 => Conflict(result.Message),
                _ => StatusCode(500, result.Message)
            };
        }
    }
}
