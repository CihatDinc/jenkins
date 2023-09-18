using FluentValidation;
using Infrastructure.Mapper;
using JetBrains.Annotations;
using Nebim.Era.Plt.Comm.Customer.Application.Contracts;
using Nebim.Era.Plt.Comm.Customer.Domain;
using Nebim.Era.Plt.Core;

namespace Application;

public class UpdateAddressHandler : IRequestHandler<UpdateCustomerAddressRequest, CustomerResponse>
{
    private readonly ICustomerMapper _customerMapper;
    private readonly ICustomerRepository _customerRepository;

    public UpdateAddressHandler(ICustomerMapper customerMapper, ICustomerRepository customerRepository)
    {
        _customerMapper = customerMapper;
        _customerRepository = customerRepository;
    }

    public async Task<CustomerResponse> Handle(UpdateCustomerAddressRequest request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.Get(request.CustomerId, cancellationToken);
        if (customer is null)
            throw new EraNotFoundException(ExceptionMessages.CouldNotFindCustomerById(request.CustomerId));

        customer.UpdateAddress(request.CustomerAddressId, request);
        await _customerRepository.Update(customer, cancellationToken);

        return _customerMapper.ToCustomerResponse(customer);
    }
}

[UsedImplicitly]
public class UpdateCustomerAddressValidator : AbstractValidator<UpdateCustomerAddressRequest>
{
    public UpdateCustomerAddressValidator()
    {
        Include(new AddressValidator());
    }
}