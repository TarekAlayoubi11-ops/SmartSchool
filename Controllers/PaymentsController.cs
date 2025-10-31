using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartSchool.BLL;
using SmartSchool.DTOs;

namespace SmartSchool.Controllers
{
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




        [HttpGet("{id}")]
        public ActionResult<PaymentDTO> GetPaymentById(int id)
        {
            if (id <= 0) return BadRequest("Invalid PaymentId.");
            var result = PaymentBll.GetPaymentById(id, _connectionString);

            if (!result.Success) return NotFound(result.Message);
            return Ok(result.Data);
        }

   
       
        
        [HttpGet("Invoice/{invoiceId}")]
        public ActionResult<IEnumerable<PaymentDTO>> GetPaymentsByInvoiceId(int invoiceId)
        {
            if (invoiceId <= 0) return BadRequest("Invalid InvoiceId.");
            var result = PaymentBll.GetPaymentsByInvoiceId(invoiceId, _connectionString);

            if (!result.Success) return NotFound(result.Message);
            return Ok(result.Data);
        }



        [HttpGet("Student/{studentId}")]
        public ActionResult<IEnumerable<PaymentDTO>> GetPaymentsByStudentId(int studentId)
        {
            if (studentId <= 0) return BadRequest("Invalid StudentId.");
            var result = PaymentBll.GetPaymentsByStudentId(studentId, _connectionString);

            if (!result.Success) return NotFound(result.Message);
            return Ok(result.Data);
        }
    }
}
