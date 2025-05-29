using System.ComponentModel.DataAnnotations;

namespace TransactionService.CustomValidators
{
    public class NotFutureDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is DateTime date)
                return date <= DateTime.Now;

            return false;
        }
    }

}
