using Microsoft.AspNetCore.Mvc;
using TransactionService.Services;
using TransactionService.Models;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace TransactionService.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ITransactionService _transactionService;
        private readonly ICustomLogger _logger;
        private readonly IRfc9457Result _rfcResultProvider;
        public TransactionController(ITransactionService transactionService, ICustomLogger logger, IRfc9457Result rfcResultProvider)
        {
            _transactionService = transactionService;
            _logger = logger;
            _rfcResultProvider = rfcResultProvider;
        }
        [HttpPost]
        [Route("/api/v1/transaction")]
        public async Task<JsonResult> Transaction([FromBody] TransactionRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {

                    var result = new JsonResult(_rfcResultProvider.GetValidationErrorResult(this.Request,
                        string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))));
                    result.StatusCode = 400;
                    return result;
                }
                else
                {
                    return Json(new { insertDateTime = await _transactionService.SaveAsync(request.ToTransaction()) });
                }
                
            }
            catch (CustomException ex) {
                var result = new JsonResult(_rfcResultProvider.GetInternalServerErrorResult(this.Request, ex.Message));
                result.StatusCode = 500;
                return result;
            }
            catch (Exception ex)
            {
                _logger.Log("Something went wrong while saving transaction {0}", ex.ToString());
                var result = new JsonResult(_rfcResultProvider.GetInternalServerErrorResult(this.Request, "something went wrong"));
                result.StatusCode = 500;
                return result;
            }
        }
        [HttpGet]
        [Route("/api/v1/transaction")]
        public async Task<JsonResult> Transaction([Required(ErrorMessage = "id is required to get transaction")] Guid id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var result = new JsonResult(_rfcResultProvider.GetValidationErrorResult(this.Request, string.Join("; ",
                    ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage))));
                    result.StatusCode = 400;
                    return result;
                }
                else
                {
                    var transaction = await _transactionService.GetAsync(id);
                    _logger.Log("Trans id {0}", transaction.Id.ToString());
                    return Json(transaction);
                    
                }
            }
            catch (CustomException ex)
            {
                var result = new JsonResult(_rfcResultProvider.GetInternalServerErrorResult(this.Request, ex.Message));
                result.StatusCode = 500;
                return result;
            }
            catch (Exception ex)
            {
                _logger.Log("Get transaction error {0}", ex.ToString());
                var result = new JsonResult(_rfcResultProvider.GetInternalServerErrorResult(this.Request, "something went wrong"));
                result.StatusCode = 500;
                return result;
            }
        }
    }
}
