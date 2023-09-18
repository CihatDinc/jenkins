using Infrastructure.Mapper;
using Nebim.Era.Plt.Comm.Customer.Application.Contracts;
using Nebim.Era.Plt.Comm.Customer.Domain;
using Nebim.Era.Plt.Core;

namespace Application;

public class DeleteAddressHandler : IRequestHandler<DeleteCustomerAddressRequest, CustomerResponse>
{
    private readonly ICustomerMapper _customerMapper;
    private readonly ICustomerRepository _customerRepository;

    public DeleteAddressHandler(ICustomerMapper customerMapper, ICustomerRepository customerRepository)
    {
        _customerMapper = customerMapper;
        _customerRepository = customerRepository;
    }

    public async Task<CustomerResponse> Handle(DeleteCustomerAddressRequest request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetAsTracking(request.CustomerId, cancellationToken);
        if (customer is null)
            throw new EraNotFoundException(ExceptionMessages.CouldNotFindCustomerById(request.CustomerId));

        customer.RemoveAddress(request.CustomerAddressId);
        await _customerRepository.Update(customer, cancellationToken);

        return _customerMapper.ToCustomerResponse(customer);
    }
}