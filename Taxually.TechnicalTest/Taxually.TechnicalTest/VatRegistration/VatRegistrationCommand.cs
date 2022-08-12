using MediatR;
using Taxually.TechnicalTest.Controllers;
using Taxually.TechnicalTest.VatRegistration.VatRegistrators;

namespace Taxually.TechnicalTest.VatRegistration
{
    public class VatRegistrationCommand : IRequest
    {
        public VatRegistrationRequest RegistrationRequest { get; set; }

        public VatRegistrationCommand(VatRegistrationRequest registrationRequest)
        {
            RegistrationRequest = registrationRequest;
        }
    }

    public class VatRegistrationCommandHandler : IRequestHandler<VatRegistrationCommand>
    {
        public const string ErrorMessage = "Country not supported";
        private readonly IEnumerable<IVatRegistrator> _vatRegistrators;

        public VatRegistrationCommandHandler(IEnumerable<IVatRegistrator> vatRegistrators)
        {
            this._vatRegistrators = vatRegistrators;
        }

        public Task<Unit> Handle(VatRegistrationCommand request, CancellationToken cancellationToken)
        {
            var reg = request.RegistrationRequest;

            var specificRegistration = _vatRegistrators.FirstOrDefault(p =>
                p.LanguageCode.Equals(request.RegistrationRequest.Country, StringComparison.InvariantCultureIgnoreCase));

            if (specificRegistration == null)
            {
                throw new Exception(ErrorMessage);
            }

            specificRegistration.Register(reg);

            return Unit.Task;
        }
    }
}