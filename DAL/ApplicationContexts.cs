using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TransactionService.Models;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
    }
    public DbSet<Transaction> Transactions { get; set; }
}
