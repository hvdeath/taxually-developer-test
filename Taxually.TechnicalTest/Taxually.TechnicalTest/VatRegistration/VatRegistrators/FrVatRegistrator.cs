using System.Text;
using Taxually.TechnicalTest.Controllers;

namespace Taxually.TechnicalTest.VatRegistration.VatRegistrators
{
    public class FrVatRegistrator : IVatRegistrator
    {
        public const string QueueName = "vat-registration-csv";
        public const string CsvHeader = "CompanyName,CompanyId";
        private readonly ITaxuallyQueueClient _excelQueueClient;

        public string LanguageCode => "FR";

        public FrVatRegistrator(ITaxuallyQueueClient excelQueueClient)
        {
            _excelQueueClient = excelQueueClient;
        }

        public void Register(VatRegistrationRequest request)
        {
            //France requires an excel spreadsheet to be uploaded to register for a VAT number

            var csvBuilder = new StringBuilder();
            csvBuilder.AppendLine(CsvHeader);
            csvBuilder.AppendLine($"{request.CompanyName}{request.CompanyId}");
            var csv = Encoding.UTF8.GetBytes(csvBuilder.ToString());
            // Queue file to be processed
            _excelQueueClient.EnqueueAsync(QueueName, csv).Wait();
        }
    }
}