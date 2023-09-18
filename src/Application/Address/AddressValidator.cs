using FluentValidation;
using Nebim.Era.Plt.Core.Types;

namespace Application;

public class AddressValidator : AbstractValidator<Address>
{
    public AddressValidator()
    {
        RuleFor(x => x.InvoiceType).IsInEnum();
        RuleFor(x => x.CompanyName).MaximumLength(50).NotNull().When(request => request.InvoiceType == InvoiceType.Corporate);
        RuleFor(x => x.TaxNumber).MaximumLength(10).NotNull().When(request => request.InvoiceType == InvoiceType.Corporate);
        RuleFor(x => x.TaxOffice).MaximumLength(50).NotNull().When(request => request.InvoiceType == InvoiceType.Corporate);
    }
}