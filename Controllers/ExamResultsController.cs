using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartSchool.BLL;
using SmartSchool.DTOs;

namespace SmartSchool.Controllers
{
    [Route("api/ExamResults")]
    [ApiController]
    public class ExamResultsController : ControllerBase
    {
        private readonly string _connectionString;

        public ExamResultsController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException("Connection string 'DefaultConnection' not found.");
        }


        [HttpPost]
        public ActionResult CreateExamResult([FromBody] CreateExamResultDTO result)
        {
            if (result == null)
                return BadRequest("Exam result data is required.");

            if (result.EnrollmentId <= 0)
                return BadRequest("EnrollmentId is invalid.");

            if (result.SubjectId <= 0)
                return BadRequest("SubjectId is invalid.");

            if (result.SubjectId <= 0)
                return BadRequest("SubjectId is invalid.");

            if (result.MarksObtained <= 0)
                return BadRequest("MarksObtained is invalid.");

            if (string.IsNullOrWhiteSpace(result.Remarks))
                return BadRequest("Remarks is required.");

            var res = ExamResultBll.CreateExamResult(result, _connectionString);
            return res.Code switch
            {
                > 0 => Ok(new { ResultId = res.Code, Message = res.Message }),
                -2 => NotFound(res.Message),
                -3 => NotFound(res.Message),
                -4 => NotFound(res.Message),
                -5 => BadRequest(res.Message),
                -6 => Conflict(res.Message),
                _ => StatusCode(500, res.Message)
            };
        }

        [HttpPut]
        public ActionResult UpdateExamResult([FromBody] UpdateExamResultDTO result)
        {
            if (result == null)
                return BadRequest("Exam result data is required.");

            if (result.ResultId <= 0)
                return BadRequest("ResultId is invalid.");

            if (result.MarksObtained <= 0)
                return BadRequest("MarksObtained is invalid.");

            if (string.IsNullOrWhiteSpace(result.Remarks))
                return BadRequest("Remarks is required.");

            var res = ExamResultBll.UpdateExamResult(result, _connectionString);
            return res.Code switch
            {
                1 => Ok(new { Message = res.Message }),
                -2 => NotFound(res.Message),
                -3 => BadRequest(res.Message),
                _ => StatusCode(500, res.Message)
            };
        }

        
        
        [HttpDelete("{id}")]
        public ActionResult DeleteExamResult(int id)
        {
            if (id <= 0) return BadRequest("ResultId is invalid.");

            var res = ExamResultBll.DeleteExamResult(id, _connectionString);
            return res.Code switch
            {
                1 => Ok(new { Message = res.Message }),
                -1 => NotFound(res.Message),
                _ => StatusCode(500, res.Message)
            };
        }




        [HttpGet("ByExam/{examId}")]
        public ActionResult<IEnumerable<ExamResultDTO>> GetByExam(int examId)
        {
            if (examId <= 0) return BadRequest("examId is invalid.");

            var result = ExamResultBll.GetExamResultsByExam(examId, _connectionString);
            if (!result.Success) return NotFound(result.Message);
            return Ok(result.Data);
        }




        [HttpGet("ByStudent/{enrollmentId}")]
        public ActionResult<IEnumerable<ExamResultDTO>> GetByStudent(int studentId)
        {
            if (studentId <= 0) return BadRequest("enrollmentId is invalid.");


            var result = ExamResultBll.GetExamResultsByStudent(studentId, _connectionString);
            if (!result.Success) return NotFound(result.Message);
            return Ok(result.Data);
        }


      
        [HttpGet]
        public ActionResult<IEnumerable<ExamResultDTO>> GetAll()
        {
            var result = ExamResultBll.GetAllExamResults(_connectionString);
            if (!result.Success) return NotFound(result.Message);
            return Ok(result.Data);
        }
    }
}
