namespace Application;
using FluentValidation;
using Infrastructure.Mapper;
using Nebim.Era.Plt.Comm.Customer.Application.Contracts;
using Nebim.Era.Plt.Comm.Customer.Domain;
using Nebim.Era.Plt.Core;

public class DeletePassportHandler : IRequestHandler<DeletePassportRequest>
{
    private readonly IPassportRepository _passportRepository;
    private readonly IPassportMapper _passportMapper;

    public DeletePassportHandler(IPassportRepository passportRepository, IPassportMapper passportMapper)
    {
        _passportRepository = passportRepository;
        _passportMapper = passportMapper;
    }

    public async Task Handle(DeletePassportRequest request, CancellationToken cancellationToken)
    {
        var affectedRows = await _passportRepository.Delete(request.Id, cancellationToken);
        if (affectedRows == 0)
            throw new EraNotFoundException(ExceptionMessages.CouldNotFindPassportById(request.Id));
    }
}

public class DeletePassportRequestValidator : AbstractValidator<DeletePassportRequest>
{
    public DeletePassportRequestValidator()
    {
        RuleFor(t => t.Id).NotEmpty();
    }
}
