using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace TransactionService.Services
{
    public interface IRfc9457Result
    {
        ProblemDetails GetValidationErrorResult(HttpRequest request, string message);
        ProblemDetails GetInternalServerErrorResult(HttpRequest request, string message);
    }
    public class RFCErrorResultProvider : IRfc9457Result
    {
        public ProblemDetails GetValidationErrorResult(HttpRequest request, string message)
        {
            return new ProblemDetails
            {
                Type = "https://example.com/probs/validation-error",
                Title = "Validation error",
                Status = (int)HttpStatusCode.BadRequest,
                Detail = message,
                Instance = request.Path
            };
        }
        public ProblemDetails GetInternalServerErrorResult(HttpRequest request, string message)
        {
            return new ProblemDetails
            {
                Type = "https://example.com/probs/internal-server-error",
                Title = "Internal error",
                Status = (int)HttpStatusCode.InternalServerError,
                Detail = message,
                Instance = request.Path
            };
        }
        public ProblemDetails GetAuthErrorResult(HttpRequest request)
        {
            return new ProblemDetails
            {
                Type = "https://example.com/probs/unauthorized",
                Title = "Unauthorized",
                Status = 401,
                Detail = "Not authorized",
                Instance = request.Path
            };
        }
        public ProblemDetails GetForbiddenResult(HttpRequest request)
        {
            return new ProblemDetails
            {
                Type = "https://example.com/probs/access-denied",
                Title = "Forbidden",
                Status = 403,
                Detail = "Access Denied.",
                Instance = request.Path
            };
        }
    }
}
