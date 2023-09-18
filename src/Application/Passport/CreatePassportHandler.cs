namespace Application;
using FluentValidation;
using Infrastructure.Mapper;
using Nebim.Era.Plt.Comm.Customer.Application.Contracts;
using Nebim.Era.Plt.Comm.Customer.Domain;
using Nebim.Era.Plt.Core;

public class CreatePassportHandler : IRequestHandler<CreatePassportRequest, PassportResponse>
{
    private readonly IPassportRepository _passportRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IPassportMapper _passportMapper;

    public CreatePassportHandler(IPassportRepository passportRepository, ICustomerRepository customerRepository, IPassportMapper passportMapper)
    {
        _passportRepository = passportRepository;
        _customerRepository = customerRepository;
        _passportMapper = passportMapper;
    }

    public async Task<PassportResponse> Handle(CreatePassportRequest request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.Get(request.CustomerId, cancellationToken);
        if (customer is null)
            throw new EraNotFoundException(ExceptionMessages.CouldNotFindCustomerById(request.CustomerId));

        var existingPassport = await _passportRepository.GetByCustomerId(request.CustomerId, cancellationToken);
        if (existingPassport != null)
            throw new EraNotFoundException(ExceptionMessages.CannotHaveMultiplePassportDefined(request.CustomerId));

        var passport = _passportMapper.ToPassport(request);
        var insertedPassport = await _passportRepository.Create(passport, cancellationToken);

        PassportResponse response = _passportMapper.ToPassportResponse(insertedPassport);
        return response;
    }
}

public class CreatePassportRequestValidator : AbstractValidator<CreatePassportRequest>
{
    public CreatePassportRequestValidator()
    {
        Include(new PassportRequestValidator());
    }
}
