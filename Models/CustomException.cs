namespace TransactionService.Models
{
    public class CustomException: Exception
    {
        public CustomException(string messge):base(messge) { }
    }
}
