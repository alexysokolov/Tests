using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using TransactionService.DAL;
using TransactionService.Models;
using TransactionService.Services;

namespace TransactionService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<ApplicationContext>(options =>options.UseNpgsql(builder.Configuration.GetConnectionString("psqlconnection")));
            
            builder.Services.AddSingleton<ICustomLogger, LogService>();
            builder.Services.AddScoped<ITransactionService, TransactionService.Services.TransactionService>();
            builder.Services.AddSingleton<IRfc9457Result, RFCErrorResultProvider>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.MapControllers();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
                db.Database.Migrate();
                DatabaseSeeder.Seed(db);
            }

            app.Run();
        }
    }
}
