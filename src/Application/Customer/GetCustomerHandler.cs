using FluentValidation;
using Infrastructure.Mapper;
using JetBrains.Annotations;
using Nebim.Era.Plt.Comm.Customer.Application.Contracts;
using Nebim.Era.Plt.Comm.Customer.Domain;
using Nebim.Era.Plt.Core;

namespace Application;

public class GetCustomerHandler : IRequestHandler<GetCustomerRequest, CustomerResponse>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ICustomerMapper _mapper;

    public GetCustomerHandler(ICustomerRepository customerRepository, ICustomerMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<CustomerResponse> Handle(GetCustomerRequest request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.Get(request.Id, cancellationToken);
        if (customer is null) throw new EraNotFoundException(ExceptionMessages.CouldNotFindCustomerById(request.Id));

        var response = _mapper.ToCustomerResponse(customer);
        return response;
    }
}

[UsedImplicitly]
public class GetCustomerValidator : AbstractValidator<GetCustomerRequest>
{
    public GetCustomerValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}