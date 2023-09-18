namespace Application;
using System.Threading.Tasks;
using FluentValidation;
using Infrastructure.Mapper;
using Nebim.Era.Plt.Comm.Customer.Application.Contracts;
using Nebim.Era.Plt.Comm.Customer.Domain;

public class GetCustomerByPassportInfoHandler : IRequestHandler<GetCustomerByPassportInfoRequest, IEnumerable<CustomerResponse>>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ICustomerMapper _customerMapper;

    public GetCustomerByPassportInfoHandler(ICustomerRepository customerRepository, ICustomerMapper customerMapper)
    {
        _customerRepository = customerRepository;
        _customerMapper = customerMapper;
    }

    public async Task<IEnumerable<CustomerResponse>> Handle(GetCustomerByPassportInfoRequest request, CancellationToken cancellationToken)
    {
        var customers = await _customerRepository.GetBy(request, cancellationToken);
        return customers.ConvertAll(_customerMapper.ToCustomerResponse);
    }
}

public class SearchPassportsByCustomerInfoRequestValidator : AbstractValidator<GetCustomerByPassportInfoRequest>
{
    public SearchPassportsByCustomerInfoRequestValidator()
    {
        RuleFor(t => t.PassportNumber).NotEmpty();
    }
}
