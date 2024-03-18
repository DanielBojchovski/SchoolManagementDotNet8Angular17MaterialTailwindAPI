using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Stripe.Models;
using Stripe;
using Stripe.Checkout;

namespace SchoolManagementDotNet8Angular17MaterialTailwindAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StripeController : ControllerBase
    {
        private readonly StripeModel _stripeModel;
        private readonly CustomerService _customerService;
        private readonly ProductService _productService;

        public StripeController(IOptions<StripeModel> stripeModel, CustomerService customerService, ProductService productService)
        {
            _stripeModel = stripeModel.Value;
            _customerService = customerService;
            _productService = productService;
        }

        [HttpPost("Pay")]
        public ActionResult<PayResponse> Pay(PayRequest request)
        {
            StripeConfiguration.ApiKey = _stripeModel.SecretKey;

            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        Price = request.PriceId,
                        Quantity = 1,
                    }
                },
                Mode = "payment",
                SuccessUrl = "http://localhost:4200/success",
                CancelUrl = "http://localhost:4200",
            };

            var service = new SessionService();

            Session session = service.Create(options);

            return new PayResponse { Url = session.Url };
        }

        [HttpPost("CreateCustomer")]
        public async Task<dynamic> CreateCustomer(StripeCustomer customerInfo)
        {
            StripeConfiguration.ApiKey = _stripeModel.SecretKey;

            var customerOptions = new CustomerCreateOptions
            {
                Name = customerInfo.Name,
                Email = customerInfo.Email,
            };

            var customer = await _customerService.CreateAsync(customerOptions);

            return new { customer };
        }

        [HttpGet("GetAllProducts")]
        public IActionResult GetAllProducts()
        {
            StripeConfiguration.ApiKey = _stripeModel.SecretKey;

            var options = new ProductListOptions
            {
                Expand = new List<string> { "data.default_price" },
            };

            var products = _productService.List(options);   

            return Ok(products);
        }
    }
}
