namespace Application;
using FluentValidation;
using Nebim.Era.Plt.Comm.Customer.Application.Contracts;

public class PassportRequestValidator : AbstractValidator<PassportRequest>
{
    public PassportRequestValidator()
    {
        RuleFor(t => t.CustomerId).NotEmpty();
        RuleFor(t => t.IssueDate).NotEmpty();
        RuleFor(t => t.IssuingStateCode).NotEmpty();
        RuleFor(t => t.Nationality).NotEmpty();
        RuleFor(t => t.Number).NotEmpty();
    }
}
