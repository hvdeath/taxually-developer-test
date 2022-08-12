using System.Xml.Serialization;
using Taxually.TechnicalTest.Controllers;

namespace Taxually.TechnicalTest.VatRegistration.VatRegistrators
{
    public class DeVatRegistrator : IVatRegistrator
    {
        private readonly ITaxuallyQueueClient _xmlQueueClient;

        public string LanguageCode => "DE";

        public DeVatRegistrator(ITaxuallyQueueClient xmlQueueClient)
        {
            _xmlQueueClient = xmlQueueClient;
        }

        public void Register(VatRegistrationRequest request)
        {
            // Germany requires an XML document to be uploaded to register for a VAT number
            using (var stringwriter = new StringWriter())
            {
                var serializer = new XmlSerializer(typeof(VatRegistrationRequest));
                serializer.Serialize(stringwriter, this);
                var xml = stringwriter.ToString();
                // Queue xml doc to be processed
                _xmlQueueClient.EnqueueAsync("vat-registration-xml", xml).Wait();
            }
        }
    }
}