using FluentValidation;
using Infrastructure.Mapper;
using JetBrains.Annotations;
using Nebim.Era.Plt.Comm.Customer.Application.Contracts;
using Nebim.Era.Plt.Comm.Customer.Domain;

namespace Application;

public class CreateCustomerHandler : IRequestHandler<CreateCustomerRequest, CustomerResponse>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ICustomerMapper _mapper;

    public CreateCustomerHandler(ICustomerRepository customerRepository, ICustomerMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<CustomerResponse> Handle(CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        var customer = _mapper.ToCustomer(request);
        var createdCustomer = await _customerRepository.Create(customer, cancellationToken);
        return _mapper.ToCustomerResponse(createdCustomer);
    }
}

[UsedImplicitly]
public class CreateCustomerValidator : AbstractValidator<CreateCustomerRequest>
{
    public CreateCustomerValidator()
    {
        Include(new CustomerValidator());
    }
}