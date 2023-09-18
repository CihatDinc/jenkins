using FluentValidation;
using Nebim.Era.Plt.Comm.Customer.Application.Contracts;
using Nebim.Era.Plt.Core;

namespace Application;

public class CustomerValidator : AbstractValidator<CustomerRequest>
{
    public CustomerValidator()
    {
        RuleFor(customer => customer.Code).NotEmpty().MaximumLength(50);
        RuleFor(customer => customer.Name).NotEmpty().MaximumLength(50);
        RuleFor(customer => customer.Surname).NotEmpty().MaximumLength(50);
        RuleFor(customer => customer.PhoneNumber).MaximumLength(20);
        RuleFor(customer => customer.Addresses).Must(collection => collection!.Any()).WithMessage("At least 1 Customer Address is required.");
        RuleFor(customer => customer.Addresses).Must(collection => collection!.Count(o => o.IsDefault) < 2).WithMessage("You cannot add default address more than 1.");
        RuleFor(customer => customer.BirthDate).LessThan(SystemClock.UtcNow).When(t => t.BirthDate.HasValue);
    }
}