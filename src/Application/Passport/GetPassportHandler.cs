namespace Application;

using FluentValidation;
using Infrastructure.Mapper;
using Nebim.Era.Plt.Comm.Customer.Application.Contracts;
using Nebim.Era.Plt.Comm.Customer.Domain;
using Nebim.Era.Plt.Core;

public class GetPassportHandler : IRequestHandler<GetPassportRequest, PassportResponse>
{
    private readonly IPassportRepository _passportRepository;
    private readonly IPassportMapper _passportMapper;

    public GetPassportHandler(IPassportRepository passportRepository, IPassportMapper passportMapper)
    {
        _passportRepository = passportRepository;
        _passportMapper = passportMapper;
    }

    public async Task<PassportResponse> Handle(GetPassportRequest request, CancellationToken cancellationToken)
    {
        var passport = await _passportRepository.Get(request.Id, cancellationToken);
        if (passport == null)
            throw new EraNotFoundException(ExceptionMessages.CouldNotFindPassportById(request.Id));

        var response = _passportMapper.ToPassportResponse(passport);
        return response;
    }
}

public class GetPassportRequestValidator : AbstractValidator<GetPassportRequest>
{
    public GetPassportRequestValidator()
    {
        RuleFor(t => t.Id).NotEmpty();
    }
}
