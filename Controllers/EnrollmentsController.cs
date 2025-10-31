using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartSchool.BLL;
using SmartSchool.DTOs;

namespace SmartSchool.Controllers
{
    [Route("api/Enrollments")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private readonly string _connectionString;

        public EnrollmentsController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException("Connection string not found.");
        }




        [HttpPost]
        public ActionResult CreateEnrollment([FromBody] CreateEnrollmentDTO enrollment)
        {
            if (enrollment == null || enrollment.StudentId <= 0 || enrollment.SectionId <= 0)
                return BadRequest("Invalid enrollment data.");

            var result = EnrollmentBll.CreateEnrollment(enrollment, _connectionString);

            return result.Code switch
            {
                > 0 => Ok(new { EnrollmentId = result.Code, result.Message }),
                -2 => NotFound(result.Message),
                -3 => NotFound(result.Message),
                -4 => Conflict(result.Message),
                -5 => Conflict(result.Message),
                _ => StatusCode(500, result.Message)
            };
        }




        [HttpGet]
        public ActionResult GetAllEnrollments()
        {
            var result = EnrollmentBll.GetAllEnrollments(_connectionString);
            if (!result.Success)
                return NotFound(result.Message);
            return Ok(result.Data);
        }



        
        [HttpGet("student/{studentId}")]
        public ActionResult GetEnrollmentsByStudent(int studentId)
        {
            var result = EnrollmentBll.GetEnrollmentsByStudent(studentId, _connectionString);
            if (!result.Success)
                return NotFound(result.Message);
            return Ok(result.Data);
        }




        [HttpGet("section/{sectionId}")]
        public ActionResult GetStudentsBySection(int sectionId)
        {
            var result = EnrollmentBll.GetStudentsBySection(sectionId, _connectionString);
            if (!result.Success)
                return NotFound(result.Message);
            return Ok(result.Data);
        }




        [HttpDelete("{id}")]
        public ActionResult DeleteEnrollment(int id)
        {
            var result = EnrollmentBll.DeleteEnrollment(id, _connectionString);
            return result.Code switch
            {
                1 => Ok(new { Message = result.Message }),
                -2 => NotFound(result.Message),
                -3 => Conflict(result.Message),
                _ => StatusCode(500, result.Message)
            };
        }




         [HttpPut]
    public ActionResult UpdateEnrollment([FromBody] UpdateEnrollmentDTO enrollment)
    {
        if (enrollment == null)
            return BadRequest("Enrollment data is required.");

       

        if (enrollment.SectionId <= 0)
            return BadRequest("SectionId is invalid.");

        if (enrollment.EnrollmentId<= 0)
            return BadRequest("EnrollmentId is invalid.");

        var result = EnrollmentBll.UpdateEnrollment(enrollment, _connectionString);

        return result.Code switch
        {
            1 => Ok(new { Message = result.Message }),
            -2 or -3 => NotFound(result.Message),
            -4 or -5 => Conflict(result.Message),
            _ => StatusCode(500, result.Message)
        };
    }
}
}
