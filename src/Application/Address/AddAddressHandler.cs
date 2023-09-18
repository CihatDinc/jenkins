using FluentValidation;
using Infrastructure.Mapper;
using JetBrains.Annotations;
using Nebim.Era.Plt.Comm.Customer.Application.Contracts;
using Nebim.Era.Plt.Comm.Customer.Domain;
using Nebim.Era.Plt.Core;

namespace Application;

public class AddAddressHandler : IRequestHandler<AddCustomerAddressRequest, CustomerResponse>
{
    private readonly ICustomerMapper _customerMapper;
    private readonly ICustomerRepository _customerRepository;

    public AddAddressHandler(ICustomerMapper customerMapper, ICustomerRepository customerRepository)
    {
        _customerMapper = customerMapper;
        _customerRepository = customerRepository;
    }

    public async Task<CustomerResponse> Handle(AddCustomerAddressRequest request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetAsTracking(request.CustomerId, cancellationToken);
        if (customer is null)
            throw new EraNotFoundException(ExceptionMessages.CouldNotFindCustomerById(request.CustomerId));

        if (request.Id == Guid.Empty)
            request.Id = Uuid.Next();
        
        customer.AddAddress(request);
        await _customerRepository.Update(customer, cancellationToken);

        return _customerMapper.ToCustomerResponse(customer);
    }
}

[UsedImplicitly]
public class AddCustomerAddressRequestValidator : AbstractValidator<AddCustomerAddressRequest>
{
    public AddCustomerAddressRequestValidator()
    {
        Include(new AddressValidator());
    }
}