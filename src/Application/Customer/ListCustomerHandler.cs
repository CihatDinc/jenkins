using FluentValidation;
using Infrastructure.Mapper;
using JetBrains.Annotations;
using Nebim.Era.Plt.Comm.Customer.Application.Contracts;
using Nebim.Era.Plt.Comm.Customer.Domain;
using Nebim.Era.Plt.Data;

namespace Application;

public class ListCustomerHandler : IRequestHandler<ListCustomerRequest, IEnumerable<CustomerResponse>>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ICustomerMapper _mapper;

    public ListCustomerHandler(ICustomerRepository customerRepository, ICustomerMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CustomerResponse>> Handle(ListCustomerRequest request, CancellationToken cancellationToken)
    {
        var searchResult = await _customerRepository.List(request, cancellationToken);
        var customers = searchResult.ToArray();
        var customerResponses = new List<CustomerResponse>(customers.Length);
        foreach (var customer in customers) customerResponses.Add(_mapper.ToCustomerResponse(customer));

        return customerResponses;
    }
}

[UsedImplicitly]
public class ListCustomerValidator : AbstractValidator<ListCustomerRequest>
{
    public ListCustomerValidator()
    {
        Include(new ListValidator());
    }
}