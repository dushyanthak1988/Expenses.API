using Expenses.API.Data;
using Expenses.API.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Expenses.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly AppDbContext _AppDbContext;

        public TransactionController(AppDbContext appDbContext)
        {
            _AppDbContext = appDbContext;
            // Constructor logic can be added here if needed
        }        
        [HttpPost]
        public IActionResult CreateTransaction([FromBody] PostTransactionDto payload)
        {
            // Map DTO to Entity
            var newTransaction = new Models.Transaction
            {
                Type = payload.Type,
                Amount = payload.Amount,
                Category = payload.Category,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Add to DB
            _AppDbContext.Transactions.Add(newTransaction);
            _AppDbContext.SaveChanges();

            return Ok(new { message = "Transaction created successfully." });
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var allTransactions = _AppDbContext.Transactions.ToList();
            return Ok(allTransactions);
        }

        [HttpGet("details")]
        public IActionResult GetByIdQuery([FromQuery] int id)
        {
            var transactionDb = _AppDbContext.Transactions.FirstOrDefault(t => t.Id == id);

            if (transactionDb == null)
            {
                return NotFound(new { message = $"Transaction with ID {id} not found." });
            }

            return Ok(transactionDb);
        }
        [HttpPut("update/{id}")]
        public IActionResult UpdateTransaction(int id, [FromBody] PutTransactionDto payload)
        {
            var transactionDb = _AppDbContext.Transactions.FirstOrDefault(t => t.Id == id);

            if (transactionDb == null)
            {
                return NotFound(new { message = $"Transaction with ID {id} not found." });
            }

            // Update fields
            transactionDb.Type = payload.Type;
            transactionDb.Amount = payload.Amount;
            transactionDb.Category = payload.Category;
            transactionDb.UpdatedAt = DateTime.UtcNow;

            // Save changes
            _AppDbContext.Transactions.Update(transactionDb);
            _AppDbContext.SaveChanges();

            return Ok(transactionDb);
        }

        [HttpDelete("delete/{id}")]
        public IActionResult DeleteTransaction(int id)
        {
            var transactionDb = _AppDbContext.Transactions.FirstOrDefault(t => t.Id == id);

            if (transactionDb == null)
            {
                return NotFound(new { message = $"Transaction with ID {id} not found." });
            }

            _AppDbContext.Transactions.Remove(transactionDb);
            _AppDbContext.SaveChanges();

            return Ok(new { message = $"Transaction with ID {id} deleted successfully." });
        }

    }
}
