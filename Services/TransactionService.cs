using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TransactionService.Models;

namespace TransactionService.Services
{
    public interface ITransactionService
    {
        public Task<DateTime> SaveAsync(Transaction transaction);
        public Task<Transaction> GetAsync(Guid id);
    }
    public class TransactionService : ITransactionService
    {
        private readonly ICustomLogger _logger;
        private readonly ApplicationContext _databaseContext;
        private readonly IConfiguration _configuration;
        public TransactionService(ApplicationContext databaseContext, IConfiguration configuration,  ICustomLogger logger)
        {
            _databaseContext = databaseContext;
            _configuration = configuration;
            _logger = logger;
        }
        public async Task<DateTime> SaveAsync(Transaction trans)
        {
            var existedTransaction = await _databaseContext.Transactions.AsNoTracking().Select(t => t).Where(tr => tr.Id == trans.Id).ToListAsync();
            if (existedTransaction.Count() > 0) return existedTransaction[0].InsertDateTime;

            var maxCountRecordsinDatabase = _configuration["MaxTransactionRecords"];

            int maxCount = 0;
            int.TryParse(maxCountRecordsinDatabase, out maxCount);
            if (_databaseContext.Transactions.Count()== maxCount)
            {
                throw new CustomException("Max count records has reached. MaxCount is " + maxCount);
            }

            var transaction = trans with { InsertDateTime = DateTime.UtcNow };

            await _databaseContext.Transactions.AddAsync(transaction);

            _logger.Log("Transaction {0} has saved!", transaction.Id.ToString());

            await _databaseContext.SaveChangesAsync();

            return transaction.InsertDateTime;
        }
        public async Task<Transaction> GetAsync(Guid id)
        {
            var res = await _databaseContext.Transactions.AsNoTracking().Select(t => t).Where(tr=>tr.Id==id).ToListAsync();
            if (res.Count() > 0) 
                return res[0];

            throw new CustomException("Not found");
        }
    }
}
