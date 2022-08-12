using System.Text;
using FluentAssertions;
using Moq;
using Taxually.TechnicalTest.Controllers;
using Taxually.TechnicalTest.VatRegistration.VatRegistrators;

namespace Taxually.TechnicalTest.Test.VatRegistration.VatRegistrators
{
    public class FrVatRegistratorTest
    {
        [Fact]
        public void Given_Request_When_Register_Then_SendsProperCsv()
        {
            // ARRANGE
            var taxuallyQueueClientMock = new Mock<ITaxuallyQueueClient>();
            var request = new VatRegistrationRequest
            {
                CompanyId = "testCompanyId",
                CompanyName = "testCompanyName",
                Country = "FR"
            };

            // ACT
            new FrVatRegistrator(taxuallyQueueClientMock.Object).Register(request);

            // ASSERT
            taxuallyQueueClientMock.Verify(mock =>
                mock.EnqueueAsync(
                    It.Is<string>(p => p == FrVatRegistrator.QueueName),
                    It.Is<byte[]>(p => AssertByteArray(p, request))),
                    Times.Once());
        }

        [Fact]
        public void Given_NullRequest_When_Register_Then_ThrowsError()
        {
            // ARRANGE
            var taxuallyQueueClientMock = new Mock<ITaxuallyQueueClient>();
            VatRegistrationRequest? request = null;

            // ACT
            Action act = () => new FrVatRegistrator(taxuallyQueueClientMock.Object).Register(request!);

            // ASSERT
            act.Should().Throw<NullReferenceException>(); //TODO - shouldn't throw this kind of exception...
        }

        private bool AssertByteArray(byte[] p, VatRegistrationRequest request)
        {
            var csvContent = Encoding.UTF8.GetString(p);
            string[] lines = csvContent.Split(
                    new string[] { "\r\n", "\r", "\n" },
                    StringSplitOptions.None
);
            lines[0].Should().Be(FrVatRegistrator.CsvHeader);
            lines[1].Should().Be($"{request.CompanyName}{request.CompanyId}");
            return true;
        }
    }
}