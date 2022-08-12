using FluentAssertions;
using Moq;
using Taxually.TechnicalTest.Controllers;
using Taxually.TechnicalTest.VatRegistration;
using Taxually.TechnicalTest.VatRegistration.VatRegistrators;

namespace Taxually.TechnicalTest.Test.VatRegistration
{
    public class VatRegistrationCommandHandlerTests
    {
        [Fact]
        public void Given_EmptyVatRegistrators_When_Invoked_Then_ThrowsError()
        {
            // ARRANGE
            IEnumerable<IVatRegistrator> vatRegistrators = new List<IVatRegistrator>();
            var commandHandler = new VatRegistrationCommandHandler(vatRegistrators);

            var request = new VatRegistrationCommand(new VatRegistrationRequest
            {
                CompanyId = "testCompanyId",
                CompanyName = "testCompanyName",
                Country = "FR"
            });
            CancellationToken cancellationToken = CancellationToken.None;

            // ACT
            Action act = () => commandHandler.Handle(request, cancellationToken);

            // ASSERT
            act.Should().Throw<Exception>().WithMessage(VatRegistrationCommandHandler.ErrorMessage); //TODO - shouldn't throw this type of exception... app specific one
        }

        [Fact]
        public void Given_ProperVatRegistrators_When_Invoked_Then_CreateCsv()
        {
            // ARRANGE
            var taxuallyQueueClientMock = new Mock<ITaxuallyQueueClient>();
            IEnumerable<IVatRegistrator> vatRegistrators = new List<IVatRegistrator> { new FrVatRegistrator(taxuallyQueueClientMock.Object) };
            var commandHandler = new VatRegistrationCommandHandler(vatRegistrators);

            var request = new VatRegistrationCommand(new VatRegistrationRequest
            {
                CompanyId = "testCompanyId",
                CompanyName = "testCompanyName",
                Country = "FR"
            });
            CancellationToken cancellationToken = CancellationToken.None;

            // ACT
            commandHandler.Handle(request, cancellationToken);

            // ASSERT
            taxuallyQueueClientMock.Verify(mock =>
                mock.EnqueueAsync(
                    It.IsAny<string>(),
                    It.IsAny<byte[]>()),
                    Times.Once());
        }
    }
}