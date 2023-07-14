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

    [HttpGet("GetCustomer")]
    public async Task<ActionResult<Customer>> GetCustomer(string id ) {
        var customer = await _bankService.GetAsync(id);

        if (customer is null)
        {
            return NotFound();
        }

        return customer;
    }

    [HttpPost("CreateCustomer")]
    public async Task<IActionResult> CreateCustomer(Customer customer ) {
        await _bankService.CreateAsync(customer);

        return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, customer);
    }
   

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