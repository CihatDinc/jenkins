namespace Application;
using FluentValidation;
using Infrastructure.Mapper;
using Nebim.Era.Plt.Comm.Customer.Application.Contracts;
using Nebim.Era.Plt.Comm.Customer.Domain;
using Nebim.Era.Plt.Core;

public class UpdatePassportHandler : IRequestHandler<UpdatePassportRequest, PassportResponse>
{
    private readonly IPassportRepository _passportRepository;
    private readonly IPassportMapper _passportMapper;

    public UpdatePassportHandler(IPassportRepository passportRepository, IPassportMapper passportMapper)
    {
        _passportRepository = passportRepository;
        _passportMapper = passportMapper;
    }

    public async Task<PassportResponse> Handle(UpdatePassportRequest request, CancellationToken cancellationToken)
    {
        var passport = await _passportRepository.Get(request.Id, cancellationToken);
        if (passport is null)
            throw new EraNotFoundException(ExceptionMessages.CouldNotFindPassportById(request.Id));

        var updatedPassport = _passportMapper.ToPassport(passport, request);
        var affectedRows = await _passportRepository.Update(updatedPassport, cancellationToken);
        if (affectedRows == 0)
            throw new EraNotFoundException(ExceptionMessages.CouldNotFindCustomerById(request.Id));

        var response = _passportMapper.ToPassportResponse(updatedPassport);
        return response;
    }
}

public class UpdatePassportRequestValidator : AbstractValidator<UpdatePassportRequest>
{
    public UpdatePassportRequestValidator()
    {
        RuleFor(t => t.Id).NotEmpty();

        Include(new PassportRequestValidator());
    }
}
