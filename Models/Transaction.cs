using System.ComponentModel.DataAnnotations;

namespace TransactionService.Models
{
    public record Transaction
    {
        public required Guid Id { get; init; }
        public required DateTime TransactionDate { get; init; }
        public DateTime InsertDateTime { get; init; }
        public required decimal Amount { get; init; }
    }
    public class TransactionRequest
    {
        [Required(ErrorMessage = "Guid is required.")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "TransactionDate is required.")]
        public DateTime TransactionDate { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage ="Amount has to be more than 0")]
        [Required(ErrorMessage = "Transaction Amount is required.")]
        public decimal Amount { get; set; }

        public Transaction ToTransaction() 
        {
            return new Transaction { Id = this.Id, TransactionDate = this.TransactionDate, Amount = this.Amount };
        }
    }
}
