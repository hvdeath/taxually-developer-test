using MediatR;
using Microsoft.AspNetCore.Mvc;
using Taxually.TechnicalTest.VatRegistration;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

//TODO -> rewrite to minimal api with https://github.com/FastEndpoints/Library
namespace Taxually.TechnicalTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VatRegistrationController : ControllerBase
    {
        private readonly ISender _sender;

        public VatRegistrationController(ISender sender)
        {
            this._sender = sender;
        }

        /// <summary>
        /// Registers a company for a VAT number in a given country
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] VatRegistrationRequest request)
        {
            await _sender.Send(new VatRegistrationCommand(request));

            return Ok();
        }
    }

    public class VatRegistrationRequest
    {
        public string CompanyName { get; set; }
        public string CompanyId { get; set; }
        public string Country { get; set; }
    }
}