namespace Application;
using FluentValidation;
using Infrastructure.Mapper;
using Nebim.Era.Plt.Comm.Customer.Application.Contracts;
using Nebim.Era.Plt.Comm.Customer.Domain;
using Nebim.Era.Plt.Core;

public class GetPassportByCustomerHandler : IRequestHandler<GetPassportByCustomerRequest, PassportResponse>
{
    private readonly IPassportRepository _passportRepository;
    private readonly IPassportMapper _passportMapper;

    public GetPassportByCustomerHandler(IPassportRepository passportRepository, IPassportMapper passportMapper)
    {
        _passportRepository = passportRepository;
        _passportMapper = passportMapper;
    }

    public async Task<PassportResponse> Handle(GetPassportByCustomerRequest request, CancellationToken cancellationToken)
    {
        var passport = await _passportRepository.GetByCustomerId(request.CustomerId, cancellationToken);
        if (passport == null)
            throw new EraNotFoundException(ExceptionMessages.CouldNotFindPassportByCustomerId(request.CustomerId));

        var response = _passportMapper.ToPassportResponse(passport);
        return response;
    }
}

public class GetPassportByCustomerRequestValidator : AbstractValidator<GetPassportByCustomerRequest>
{
    public GetPassportByCustomerRequestValidator()
    {
        RuleFor(t => t.CustomerId).NotEmpty();
    }
}
