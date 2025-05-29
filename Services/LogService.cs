namespace TransactionService.Services
{
    public interface ICustomLogger { 
        public void Log(string message, params string[] args);
    }
    public class LogService: ICustomLogger
    {
        public void Log(string message, params string[] args) {
            Console.WriteLine(String.Format(message, args));
        }
    }
}
