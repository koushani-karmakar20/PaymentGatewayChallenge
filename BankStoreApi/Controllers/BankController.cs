using BankStoreApi.Models;
using BankStoreApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;
using MongoDB.Driver;
using BankStoreApi.Exceptions;
// using System.Net.Http;

namespace BankStoreApi.Controllers;

[ApiController]
[Route("Bank")]
public class BankController : ControllerBase
{
    private readonly BankService _bankService;

    public BankController(BankService bankService) =>
        _bankService = bankService;

   

    [HttpPost("PerformTransaction")]
    public async Task<IActionResult> PerformTransaction(Payload payload )
    {
        
    try {
        await _bankService.Transaction(payload);
    } catch(CustomerNotFoundException err) {
        return NotFound(err.Message);
    } catch(CredentialMismatchException err) {
        return BadRequest(err.Message);
    } catch(InsufficientBalanceException err) {
        return BadRequest(err.Message);
    } catch(Exception err) {
        return Problem(err.Message);
    }
    
     return NoContent();



    }
}