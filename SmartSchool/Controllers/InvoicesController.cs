using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartSchool.BLL;
using SmartSchool.DAL;
using SmartSchool.DTOs;
using static SmartSchool.Controllers.InvoicesController;

namespace SmartSchool.Controllers
{
    [Authorize]
    [Route("api/Invoices")]
    [ApiController]
    public class InvoicesController : ControllerBase
    {
        private readonly string _connectionString;

        public InvoicesController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException("Connection string 'DefaultConnection' not found.");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("Active")]
        public ActionResult<IEnumerable<InvoiceDTO>> GetAllActiveInvoices()
        {
            var result = InvoiceBll.GetAllActiveInvoices(_connectionString);

            if (!result.Success)
                return result.Message!.Contains("No") ? NotFound(result.Message) : StatusCode(500, result.Message);

            return Ok(result.Data);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<InvoiceDTO>> GetInvoiceByIdAsync(int id, [FromServices] IAuthorizationService authorizationService)
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

            var result = InvoiceBll.GetInvoiceById(id, _connectionString);

            if (!result.Success)
                return result.Message!.Contains("not found") ? NotFound(result.Message) : StatusCode(500, result.Message);

            return Ok(result.Data);
        }

        [Authorize(Roles = "Admin,Student")]
        [HttpGet("ByStudent/{studentId}")]
        public ActionResult<IEnumerable<InvoiceDTO>> GetInvoicesByStudentId(int studentId)
        {
            if (studentId <= 0)
                return BadRequest("Invalid StudentId.");

            var result = InvoiceBll.GetInvoicesByStudentId(studentId, _connectionString);

            if (!result.Success)
                return result.Message!.Contains("No") ? NotFound(result.Message) : StatusCode(500, result.Message);

            return Ok(result.Data);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult CreateInvoice([FromBody] CreateInvoiceDTO invoice)
        {
            if (invoice == null)
                return BadRequest("Invoice data is required.");

            if (invoice.EnrollmentId <= 0)
                return BadRequest("Invalid EnrollmentId.");

            if (invoice.FeeItemId <= 0)
                return BadRequest("Invalid FeeItemId.");

            if (invoice.BillingMonth <= 0 || invoice.BillingMonth > 12)
                return BadRequest("Invalid BillingMonth.");

            if (invoice.BillingYear < 2025)
                return BadRequest("Invalid BillingYear.");

            var result = InvoiceBll.CreateInvoice(invoice, _connectionString);

            return result.Code switch
            {
                > 0 => Ok(new { InvoiceId = result.Code, Message = result.Message }),
                -2 => Conflict(new { Message = result.Message }),
                -3 => NotFound(new { Message = result.Message }),
                -4 => NotFound(new { Message = result.Message }),
                _ => StatusCode(500, new { Message = result.Message })
            };
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public ActionResult UpdateInvoice([FromBody] UpdateInvoiceDTO invoice)
        {
            if (invoice == null)
                return BadRequest("Invoice data is required.");

            if (invoice.InvoiceId <= 0)
                return BadRequest("Invalid InvoiceId.");

            if (invoice.Amount <= 0)
                return BadRequest("Amount must be greater than 0.");

            if (string.IsNullOrWhiteSpace(invoice.Status))
                return BadRequest("Status is required.");

            var result = InvoiceBll.UpdateInvoice(invoice, _connectionString);

            return result.Code switch
            {
                1 => Ok(new { Message = result.Message }),
                -2 => NotFound(new { Message = result.Message }),
                -3 => Conflict(new { Message = result.Message }),
                -4 => Conflict(new { Message = result.Message }),
                _ => StatusCode(500, new { Message = result.Message })
            };
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public ActionResult DeleteInvoice(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid InvoiceId.");

            var result = InvoiceBll.DeleteInvoice(id, _connectionString);

            return result.Code switch
            {
                1 => Ok(new { Message = result.Message }),
                -2 => NotFound(new { Message = result.Message }),
                -3 => Conflict(new { Message = result.Message }),
                _ => StatusCode(500, new { Message = result.Message })
            };
        }
    }
}
