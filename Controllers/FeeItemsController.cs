using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartSchool.BLL;
using SmartSchool.DTOs;

namespace SmartSchool.Controllers
{
    [Route("api/FeeItems")]
    [ApiController]
    public class FeeItemsController : ControllerBase
    {
        private readonly string _connectionString;

        public FeeItemsController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException("Connection string 'DefaultConnection' not found.");
        }

        [HttpGet]
        public ActionResult<IEnumerable<FeeItemDTO>> GetAllActiveFeeItems()
        {
            var result = FeeItemBll.GetAllActiveFeeItems(_connectionString);

            if (!result.Success)
            {
                if (result.Message!.Contains("No fee items"))
                    return NotFound(result.Message);
                else
                    return StatusCode(500, result.Message);
            }

            return Ok(result.Data);
        }




        [HttpGet("{id}")]
        public ActionResult<FeeItemDTO> GetFeeItemById(int id)
        {
            if (id <= 0)
                return BadRequest("FeeItemId is invalid.");

            var result = FeeItemBll.GetFeeItemById(id, _connectionString);

            if (!result.Success)
            {
                if (result.Message!.Contains("not found"))
                    return NotFound(result.Message);
                else
                    return StatusCode(500, result.Message);
            }

            return Ok(result.Data);
        }




        [HttpPost]
        public ActionResult CreateFeeItem([FromBody] CreateFeeItemDTO feeItem)
        {
            if (feeItem == null)
                return BadRequest("Fee item data is required.");

            if (string.IsNullOrWhiteSpace(feeItem.Name))
                return BadRequest("Name is required.");

            if (feeItem.Amount <= 0)
                return BadRequest("Amount must be greater than zero.");

            if (string.IsNullOrWhiteSpace(feeItem.Frequency) ||
                (feeItem.Frequency != "Monthly" && feeItem.Frequency!= "Yearly"&& feeItem.Frequency!= "OneTime") )
                return BadRequest("Invalid frequency. Allowed values: Monthly, Yearly, OneTime.");

            var result = FeeItemBll.CreateFeeItem(feeItem, _connectionString);

            return result.Code switch
            {
                > 0 => Ok(new { FeeItemId = result.Code, Message = result.Message }),
                -2 => Conflict(new { Message = result.Message }),
                -4 => Ok(new { Message = result.Message }), 
                _ => StatusCode(500, new { Message = result.Message })
            };
        }



        [HttpPut]
        public ActionResult UpdateFeeItem([FromBody] UpdateFeeItemDTO feeItem)
        {
            if (feeItem == null)
                return BadRequest("Fee item data is required.");

            if (feeItem.FeeItemId<=0 )
                return BadRequest("Invalid FeeItemId.");

            if (string.IsNullOrWhiteSpace(feeItem.Name))
                return BadRequest("Name is required.");

            if (feeItem.Amount <= 0)
                return BadRequest("Amount must be greater than zero.");

            if (string.IsNullOrWhiteSpace(feeItem.Frequency) ||
                (feeItem.Frequency != "Monthly" && feeItem.Frequency != "Yearly" && feeItem.Frequency != "OneTime"))
                return BadRequest("Invalid frequency. Allowed values: Monthly, Yearly, OneTime.");

            var result = FeeItemBll.UpdateFeeItem(feeItem, _connectionString);

            return result.Code switch
            {
                1 => Ok(new { Message = result.Message }),
                -1 => NotFound(new { Message = result.Message }),
                -5 => Conflict(new { Message = result.Message }),               
                _ => StatusCode(500, new { Message = result.Message })
            };
        }



        [HttpDelete("{id}")]
        public ActionResult DeleteFeeItem(int id)
        {
            if (id <= 0)
                return BadRequest("FeeItemId is invalid.");

            var result = FeeItemBll.DeleteFeeItem(id, _connectionString);

            return result.Code switch
            {
                1 => Ok(new { Message = result.Message }),
                -1 => NotFound(new { Message = result.Message }),
                -2 => Conflict( new { Message = result.Message }),
                _ => StatusCode(500, new { Message = result.Message })
            };
        }
    }
}

