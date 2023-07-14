using PaymentApi.Models;
using PaymentApi.Services;
using Microsoft.AspNetCore.Mvc;
using PaymentApi.Exceptions;
using AwesomeApi.Filters;

namespace PaymentApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentGatewayController : ControllerBase
{
    private readonly PaymentGatewayService _paymentGatewayService;

    public PaymentGatewayController(PaymentGatewayService paymentGatewayService) =>
        _paymentGatewayService = paymentGatewayService;

    [HttpGet("GetPayments")]
    public async Task<List<Payment>> GetPayments() =>
        await _paymentGatewayService.GetAsyncPayment();

    [HttpGet("GetMerchants")]
    public async Task<List<Merchant>> GetMerchants() =>
        await _paymentGatewayService.GetAsyncMerchant();

    [HttpGet("GetPaymentWithId")]
    public async Task<ActionResult<Payment>> GetPaymentWithId(string id)
    {
        
        var payment = await _paymentGatewayService.GetAsyncPaymentWithId(id);

        if (payment is null)
        {
            return NotFound();
        }

        return payment;
    }

    [HttpGet("GetPaymentsWithMerchantId")]
    public async Task<ActionResult<List<Payment>>> GetPaymentsWithMerchantId(string id)
    {
        
        var payment = await _paymentGatewayService.GetPaymentsWithMerchantId(id);

        if (payment is null)
        {
            return NotFound();
        }

        return payment;
    }


    [HttpGet("GetMerchantWithId")]
    public async Task<ActionResult<Merchant>> GetMerchantWithId(string id)
    {
        var merchant = await _paymentGatewayService.GetAsyncMerchantWithId(id);

        if (merchant is null)
        {
            return NotFound();
        }

        return merchant;
    }

    [HttpPost("CreatePayment")]
    public async Task<IActionResult> Post(Payment newPayment)
    {
        await _paymentGatewayService.CreateAsync(newPayment);

        return CreatedAtAction(nameof(GetPaymentWithId), new { id = newPayment.Id }, newPayment);
    }


     [HttpPost("CreateMerchant")]
    public async Task<IActionResult> Post(Merchant newMerchant)
    {
        await _paymentGatewayService.CreateAsync(newMerchant);

        return CreatedAtAction(nameof(GetMerchantWithId), new { id = newMerchant.Id }, newMerchant);
    }

    [HttpPut("UpdatePayment")]
    public async Task<IActionResult> UpdatePayment(string id, Payment updatedPayment)
    {
        var payment = await _paymentGatewayService.GetAsyncPaymentWithId(id);

        if (payment is null)
        {
            return NotFound();
        }

        updatedPayment.Id = payment.Id;

        await _paymentGatewayService.UpdateAsync(id, updatedPayment);

        return NoContent();
    }


    [HttpPut("UpdateMerchant")]
    public async Task<IActionResult> UpdateMerchant(string id, Merchant updatedMerchant)
    {
        var merchant = await _paymentGatewayService.GetAsyncMerchantWithId(id);

        if (merchant is null)
        {
            return NotFound();
        }

        updatedMerchant.Id = merchant.Id;

        await _paymentGatewayService.UpdateAsync(id, updatedMerchant);

        return NoContent();
    }

    [HttpDelete("DeletePayment")]
    public async Task<IActionResult> DeletePayment(string id)
    {
        var payment = await _paymentGatewayService.GetAsyncPaymentWithId(id);

        if (payment is null)
        {
            return NotFound();
        }

        await _paymentGatewayService.RemoveAsyncPayment(id);

        return NoContent();
    }


    [HttpDelete("DeleteMerchant")]
    public async Task<IActionResult> DeleteMerchant(string id)
    {
        var merchant = await _paymentGatewayService.GetAsyncMerchantWithId(id);

        if (merchant is null)
        {
            return NotFound();
        }

        await _paymentGatewayService.RemoveAsyncMerchant(id);

        return NoContent();
    }

    [ApiKey]
    [HttpPost("SendPaymentRequestToBank")]
    public async Task<ActionResult<PaymentResponse>> SendPaymentRequestToBank(PaymentPayload paymentPayload)
    {
        PaymentResponse paymentResponse;
        try{
           paymentResponse = await _paymentGatewayService.SendPaymentRequest(paymentPayload);
        }
        catch(MerchantNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch(MerchantInactiveException ex)
        {
            return BadRequest(ex.Message);
        }
        
        if(paymentResponse.Status==false)
        return BadRequest("payment unsuccessful");

        return paymentResponse;
    }

    [ApiKey]
    [HttpPost("RetrievePayment")]
    public async Task<ActionResult<RetrieveResponse>> RetrievePastPayment(RetrievePayload retrievePayload)
    {
        var response = await _paymentGatewayService.RetrievePayment(retrievePayload);

        if (response is null)
        {
            return NotFound();
        }

        return response;
    }

}