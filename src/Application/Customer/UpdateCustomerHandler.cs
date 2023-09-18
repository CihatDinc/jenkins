using FluentValidation;
using Infrastructure.Mapper;
using JetBrains.Annotations;
using Nebim.Era.Plt.Comm.Customer.Application.Contracts;
using Nebim.Era.Plt.Comm.Customer.Domain;
using Nebim.Era.Plt.Core;

namespace Application;

public class UpdateCustomerHandler : IRequestHandler<UpdateCustomerRequest>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ICustomerMapper _mapper;

    public UpdateCustomerHandler(ICustomerRepository customerRepository, ICustomerMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task Handle(UpdateCustomerRequest request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetAsNoTracking(request.Id, cancellationToken);
        if (customer is null)
            throw new EraNotFoundException(ExceptionMessages.CouldNotFindCustomerById(request.Id));

        var updatedCustomer = _mapper.ToCustomer(customer, request);
        var effectedRecords = await _customerRepository.Update(updatedCustomer, cancellationToken);
        if (effectedRecords == 0) throw new EraNotFoundException(ExceptionMessages.CouldNotFindCustomerById(request.Id));
    }
}

[UsedImplicitly]
public class UpdateCustomerValidator : AbstractValidator<UpdateCustomerRequest>
{
    public UpdateCustomerValidator()
    {
        Include(new CustomerValidator());
        RuleFor(x => x.Id).NotNull();
    }
}
