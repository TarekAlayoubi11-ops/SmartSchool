using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartSchool.BLL;
using SmartSchool.DTOs;

namespace SmartSchool.Controllers
{
    [Authorize]
    [Route("api/Payments")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly string _connectionString;

        public PaymentsController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException("DefaultConnection not found.");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult CreatePayment([FromBody] CreatePaymentDTO payment)
        {
            if (payment == null) return BadRequest("Payment data is required.");
            if (payment.InvoiceId <= 0) return BadRequest("Invalid InvoiceId.");
            if (payment.AmountPaid <= 0) return BadRequest("AmountPaid must be greater than 0.");
            if (string.IsNullOrWhiteSpace(payment.PaymentMethod)) return BadRequest("PaymentMethod is required.");

            var result = PaymentBll.CreatePayment(payment, _connectionString);
            return result.Code switch
            {
                1 => Ok(new { PaymentId = result.Code, Message = result.Message }),
                -2 or -3 or -4 => BadRequest(result.Message),
                _ => StatusCode(500, result.Message)
            };
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public ActionResult UpdatePayment([FromBody] UpdatePaymentDTO payment)
        {
            if (payment == null) return BadRequest("Payment data is required.");
            if (payment.PaymentId <= 0) return BadRequest("Invalid PaymentId.");
            if (payment.AmountPaid <= 0) return BadRequest("AmountPaid must be greater than 0.");
            if (string.IsNullOrWhiteSpace(payment.PaymentMethod)) return BadRequest("PaymentMethod is required.");

            var result = PaymentBll.UpdatePayment(payment, _connectionString);
            return result.Code switch
            {
                1 => Ok(new { Message = result.Message }),
                -2 or -3 => BadRequest(result.Message),
                _ => StatusCode(500, result.Message)
            };
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public ActionResult DeletePayment(int id)
        {
            if (id <= 0) return BadRequest("Invalid PaymentId.");

            var result = PaymentBll.DeletePayment(id, _connectionString);
            return result.Code switch
            {
                1 => Ok(new { Message = result.Message }),
                -2 => NotFound(result.Message),
                _ => StatusCode(500, result.Message)
            };
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public ActionResult<PaymentDTO> GetPaymentById(int id)
        {
            if (id <= 0) return BadRequest("Invalid PaymentId.");
            var result = PaymentBll.GetPaymentById(id, _connectionString);

            if (!result.Success) return NotFound(result.Message);
            return Ok(result.Data);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("Invoice/{invoiceId}")]
        public ActionResult<IEnumerable<PaymentDTO>> GetPaymentsByInvoiceId(int invoiceId)
        {
            if (invoiceId <= 0) return BadRequest("Invalid InvoiceId.");
            var result = PaymentBll.GetPaymentsByInvoiceId(invoiceId, _connectionString);

            if (!result.Success) return NotFound(result.Message);
            return Ok(result.Data);
        }

        [Authorize(Roles = "Admin,Student")]
        [HttpGet("Student/{studentId}")]
        public async Task<ActionResult<IEnumerable<PaymentDTO>>> GetPaymentsByStudentIdAsync(int studentIdint, [FromServices] IAuthorizationService authorizationService)
        {
            if (studentIdint < 1)
                return BadRequest("Invalid Student id.");

            var Student = StudentBll.GetStudentById(studentIdint, _connectionString);

            if (Student == null)
                return NotFound("Student not found.");

            var authResult = await authorizationService.AuthorizeAsync(
                User,
                studentIdint,
                "UserOwnerOrAdmin");

            if (!authResult.Succeeded)
                return Forbid();

            var result = PaymentBll.GetPaymentsByStudentId(studentIdint, _connectionString);

            if (!result.Success) return NotFound(result.Message);
            return Ok(result.Data);
        }
    }
}
