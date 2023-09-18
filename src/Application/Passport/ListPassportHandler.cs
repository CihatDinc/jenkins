namespace Application;

using Infrastructure.Mapper;
using Nebim.Era.Plt.Comm.Customer.Application.Contracts;
using Nebim.Era.Plt.Comm.Customer.Domain;
using Nebim.Era.Plt.Data;

public class ListPassportHandler : IRequestHandler<ListPassportRequest, IEnumerable<PassportResponse>>
{
    private readonly IPassportRepository _passportRepository;
    private readonly IPassportMapper _passportMapper;

    public ListPassportHandler(IPassportRepository passportRepository, IPassportMapper passportMapper)
    {
        _passportRepository = passportRepository;
        _passportMapper = passportMapper;
    }

    public async Task<IEnumerable<PassportResponse>> Handle(ListPassportRequest request, CancellationToken cancellationToken)
    {
        var passports = (await _passportRepository.List(request, cancellationToken)).ToArray();

        var passportResponses = new List<PassportResponse>(passports.Length);
        foreach (var customer in passports)
            passportResponses.Add(_passportMapper.ToPassportResponse(customer));

        return passportResponses;
    }
}
