namespace TransactionService.DAL
{
    public static class DatabaseSeeder
    {
        public static void Seed(ApplicationContext context)
        {
            if (context.Transactions.Any())
                return;

            var transactions = new List<Models.Transaction>();
            var random = new Random();

            for (int i = 0; i < 99; i++)
            {
                var transaction = new Models.Transaction
                {
                    Id = Guid.NewGuid(),
                    TransactionDate = DateTime.UtcNow.AddDays(-random.Next(0, 365)),
                    InsertDateTime = DateTime.UtcNow.AddDays(-random.Next(0, 365)),
                    Amount = Math.Round((decimal)(random.NextDouble() * 1000), 2)
                };

                transactions.Add(transaction);
            }

            context.Transactions.AddRange(transactions);
            context.SaveChanges();
        }


    }
}
