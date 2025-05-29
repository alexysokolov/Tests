using Microsoft.Extensions.Logging;
using TransactionService.Models;

namespace TransactionService.Services
{
    public interface ITransactionService
    {
        public DateTime Save(Transaction transaction);
        public Transaction? Get(Guid id);
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
        public DateTime Save(Transaction trans)
        {
            var existedTransaction = _databaseContext.Transactions.FirstOrDefault(x => x.Id == trans.Id);
            if (existedTransaction != null) return existedTransaction.InsertDateTime;

            

            var maxCountRecordsinDatabase = _configuration["MaxTransactionRecords"];

            int maxCount = 0;
            int.TryParse(maxCountRecordsinDatabase, out maxCount);
            if (_databaseContext.Transactions.Count()== maxCount)
            {
                throw new CustomException("Max count records has reached. MaxCount is " + maxCount);
            }

            var transaction = trans with { InsertDateTime = DateTime.UtcNow };

            _databaseContext.Transactions.Add(transaction);

            _logger.Log("Transaction {0} has saved!", transaction.Id.ToString());

            _databaseContext.SaveChanges();

            return transaction.InsertDateTime;
        }
        public Transaction? Get(Guid id)
        {
            return _databaseContext.Transactions.FirstOrDefault(x => x.Id == id);
        }
    }
}
