using Taxually.TechnicalTest.Controllers;

namespace Taxually.TechnicalTest.VatRegistration.VatRegistrators
{
    public interface IVatRegistrator
    {
        string LanguageCode { get; }

        void Register(VatRegistrationRequest request);
    }
}