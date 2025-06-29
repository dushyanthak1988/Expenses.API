using Expenses.API.Dtos;
using Expenses.API.Models;

namespace Expenses.API.Data.Services
{
    public interface ITransactionService
    {
        List<Transaction> GetAll();
        Transaction? GetById(int id);
        Transaction Add(PostTransactionDto dto);
        Transaction? Update(int id, PutTransactionDto dto);
        void Delete(int id);
    }
    public class TransactionService : ITransactionService
    {
        private readonly AppDbContext _AppDbContext;
        public TransactionService(AppDbContext appDbContext)
        {
            _AppDbContext = appDbContext;
        }
        Transaction ITransactionService.Add(PostTransactionDto dto)
        {
            var newTransaction = new Models.Transaction
            {
                Type = dto.Type,
                Amount = dto.Amount,
                Category = dto.Category,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Add to DB
            _AppDbContext.Transactions.Add(newTransaction);
            _AppDbContext.SaveChanges();

            return newTransaction;

        }

        void ITransactionService.Delete(int id)
        {
            var transactionDb = _AppDbContext.Transactions.FirstOrDefault(t => t.Id == id);

            if (transactionDb != null)
            {
                _AppDbContext.Transactions.Remove(transactionDb);
                _AppDbContext.SaveChanges();
            }
        }

        List<Transaction> ITransactionService.GetAll()
        {
            var allTransactions = _AppDbContext.Transactions.ToList();
            return allTransactions;
        }

        Transaction? ITransactionService.GetById(int id)
        {
            var transactionDb = _AppDbContext.Transactions.FirstOrDefault(t => t.Id == id);

            if (transactionDb != null)
            {
                return transactionDb;
            }
            else
            {
                return null;
            }
        }

        Transaction? ITransactionService.Update(int id, PutTransactionDto dto)
        {
            var transactionDb = _AppDbContext.Transactions.FirstOrDefault(t => t.Id == id);

            if (transactionDb != null)
            {
                // Update fields
                transactionDb.Type = dto.Type;
                transactionDb.Amount = dto.Amount;
                transactionDb.Category = dto.Category;
                transactionDb.UpdatedAt = DateTime.UtcNow;

                // Save changes
                _AppDbContext.Transactions.Update(transactionDb);
                _AppDbContext.SaveChanges();
                return transactionDb;
            }
            else
            {
                return null;
            }


        }
    }
}
