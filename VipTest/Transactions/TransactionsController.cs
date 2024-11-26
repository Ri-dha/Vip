using System.Transactions;
using Microsoft.AspNetCore.Mvc;
using VipTest.Localization;
using VipTest.Transactions.dto;
using VipTest.Transactions.Payloads;
using VipTest.Transactions.utli;
using VipTest.Utlity;
using VipTest.Utlity.Basic;

namespace VipTest.Transactions;

[Route("transactions")]
public class TransactionsController:BaseController
{
    private readonly ITransactionServices _transactionServices;
    private readonly IEnumTranslationService _enumTranslationService;


    public TransactionsController(ITransactionServices transactionServices, IEnumTranslationService enumTranslationService)
    {
        _transactionServices = transactionServices;
        _enumTranslationService = enumTranslationService;
    }
    
    [HttpPut("update-transaction-status/{transactionId}")]
    public async Task<IActionResult> UpdateTransactionStatus(Guid transactionId, [FromBody] TransactionUpdateForm transactionUpdateForm)
    {
        var (transactionDto, error) = await _transactionServices.UpdateTransactionStatus(transactionId, transactionUpdateForm.Status);
        if (error != null) return BadRequest(new { error });
        return Ok(transactionDto);
    }

    [HttpGet("get-transaction/{transactionId}")]
    public async Task<IActionResult> GetTransactionById(Guid transactionId)
    {
        var (transactionDto, error) = await _transactionServices.GetTransaction(transactionId);
        if (error != null) return BadRequest(new { error });
        return Ok(transactionDto);
    }
    
    [HttpGet("get-all-transactions")]
    public async Task<IActionResult> GetAllTransactions([FromQuery] TransactionFilterForm filterForm)
    {
        var (transactions, totalCount, error) = await _transactionServices.GetTransactions(filterForm);
        if (error != null) return BadRequest(new { error });
    
        var result = new Page<TransactionDto>()
        {
            Data = transactions,
            PagesCount = (int)Math.Ceiling((double)(totalCount ?? 0) / filterForm.PageSize),
            CurrentPage = filterForm.PageNumber,
            TotalCount = totalCount
        };
        return Ok(result);
    }
    
    
    [HttpGet("get-transactions-service-type")]
    public IActionResult GetTransactionsStatus()
    {
        var status = _enumTranslationService.GetAllEnumTranslations<TransactionsServicesType>();
        return Ok(status);
    }
    
    [HttpGet("get-transactions-payment-status")]
    public IActionResult GetTransactionsPaymentStatus()
    {
        var status = _enumTranslationService.GetAllEnumTranslations<TransactionPaymentStatus>();
        return Ok(status);
    }
    
    
    

}