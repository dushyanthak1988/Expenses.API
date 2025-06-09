using Expenses.API.Data;
using Expenses.API.Dtos;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet]
        public IActionResult getAll()
        {
            var allTransaction = _AppDbContext.Transactions.ToList();
            return Ok(allTransaction);
        }
        [HttpGet("{id}")]
        public IActionResult GetbyID( int id) 
        {
            var transactionDB = _AppDbContext.Transactions.FirstOrDefault( row=> row.Id == id );

            if (transactionDB == null)
                return NotFound();

            return Ok(transactionDB);
        }

        [HttpPost]
        public IActionResult CreateTransactions([FromBody] PostTransactionDto payload)
        {
            var newTransactions =  new Models.Transaction
            {
                Type = payload.Type,
                Amount = payload.Amount,
                Category = payload.Category,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _AppDbContext.Transactions.Add(newTransactions);
            _AppDbContext.SaveChanges();

            return Ok(new { Message = "Saved Sucessfully" });
        }
    }
}
