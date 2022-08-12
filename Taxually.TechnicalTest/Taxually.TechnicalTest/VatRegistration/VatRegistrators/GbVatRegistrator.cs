using Taxually.TechnicalTest.Controllers;

namespace Taxually.TechnicalTest.VatRegistration.VatRegistrators
{
    public class GbVatRegistrator : IVatRegistrator
    {
        private readonly ITaxuallyHttpClient _httpClient;

        public string LanguageCode => "GB";

        public GbVatRegistrator(ITaxuallyHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public void Register(VatRegistrationRequest request)
        {
            _httpClient.PostAsync("https://api.uktax.gov.uk", request).Wait();
        }
    }
}