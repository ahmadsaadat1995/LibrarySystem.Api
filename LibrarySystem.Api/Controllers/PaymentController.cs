using LibrarySystem.Api.DTOs;
using LibrarySystem.Api.Errors;
using LibrarySystem.Core.Services.Contract;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace LibrarySystem.Api.Controllers
{
    public class PaymentController : ApiControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IConfiguration _configuration;

        public PaymentController(IPaymentService paymentService , IConfiguration configuration)
        {
            _paymentService = paymentService;
            _configuration = configuration;
        }

        [HttpPost("{BasketId}")]
        [ProducesResponseType(typeof(CustomerBasketDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntent(string BasketId)
        {
            var Basket =await _paymentService.CreateOrUpdatePaymentIntentId(BasketId);
            if (Basket is null)
                return BadRequest(new ApiResponse(400, "There is Problem With Your Basket !"));
            return Ok(Basket);
        }

        [HttpPost("Webhook")] //stripe listen --forward-to https://localhost:7194/api/payment/Webhook
        public async Task<ActionResult> Stripewebhook() 
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var stripeSignature = Request.Headers["Stripe-Signature"];
            if (string.IsNullOrEmpty(stripeSignature))
            {
                return BadRequest(new ApiResponse(400, "Missing Stripe signature"));
            }

            var stripeEvent = Stripe.EventUtility.ConstructEvent(json, stripeSignature, _configuration["StripeSettings:WebhookSecret"]);

            if (stripeEvent.Type == "payment_intent.succeeded")
            {
                var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                if (paymentIntent != null)
                {
                    await _paymentService.UpdatePatmentIntentIdSucceedOrFailed(paymentIntent.Id, true);
                }
                else
                {
                    return BadRequest(new ApiResponse(400, "PaymentIntent object is null"));
                }
            }
            else if (stripeEvent.Type == "payment_intent.failed")
            {
                var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                if (paymentIntent != null)
                {   
                    await _paymentService.UpdatePatmentIntentIdSucceedOrFailed(paymentIntent.Id, false);
                }
                else
                {
                    return BadRequest(new ApiResponse(400, "PaymentIntent object is null"));
                }
            }
            return Ok();

        }

    }
}
