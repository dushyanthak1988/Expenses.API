using Expenses.API.Data.Services;
using Expenses.API.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Expenses.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _service;
        public TransactionController(ITransactionService service)
        {
            _service = service;
        }
        [HttpPost]
        public IActionResult CreateTransaction([FromBody] PostTransactionDto payload)
        {
            var newTransactionDto = _service.Add(payload);

            if (newTransactionDto != null)
            {
                return Ok(new { message = "Transaction created successfully." });
            }
            else
            {
                return BadRequest(new { message = "Failed to create transaction." });
            }
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var allTransactions = _service.GetAll();
            return Ok(allTransactions);
        }

        [HttpGet("details")]
        public IActionResult GetByIdQuery([FromQuery] int id)
        {
            var transactionDb = _service.GetById(id);
            if (transactionDb == null)
            {
                return NotFound(new { message = $"Transaction with ID {id} not found." });
            }

            return Ok(transactionDb);
        }
        [HttpPut("update/{id}")]
        public IActionResult UpdateTransaction(int id, [FromBody] PutTransactionDto payload)
        {
            var transactionDb = _service.Update(id, payload);

            if (transactionDb == null)
            {
                return NotFound(new { message = $"Transaction with ID {id} not found." });
            }
            return Ok(transactionDb);
        }

        [HttpDelete("delete/{id}")]
        public IActionResult DeleteTransaction(int id)
        {
            _service.Delete(id);
            return Ok(new { message = $"Transaction with ID {id} deleted successfully." });
        }

    }
}
