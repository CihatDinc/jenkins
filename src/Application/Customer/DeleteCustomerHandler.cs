using FluentValidation;
using JetBrains.Annotations;
using Nebim.Era.Plt.Comm.Customer.Application.Contracts;
using Nebim.Era.Plt.Comm.Customer.Domain;
using Nebim.Era.Plt.Core;

namespace Application;

public class DeleteCustomerHandler : IRequestHandler<DeleteCustomerRequest>
{
    private readonly ICustomerRepository _customerRepository;

    public DeleteCustomerHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task Handle(DeleteCustomerRequest request, CancellationToken cancellationToken)
    {
        var effectedRecords = await _customerRepository.Delete(request.Id, cancellationToken);
        if (effectedRecords == 0) throw new EraNotFoundException(ExceptionMessages.CouldNotFindCustomerById(request.Id));
    }
}

[UsedImplicitly]
public class DeleteCustomerValidator : AbstractValidator<DeleteCustomerRequest>
{
    public DeleteCustomerValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}