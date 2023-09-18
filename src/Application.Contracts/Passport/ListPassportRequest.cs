namespace Nebim.Era.Plt.Comm.Customer.Application.Contracts;

using Nebim.Era.Plt.Data;

public class ListPassportRequest : ListRequest, IRequest<IEnumerable<PassportResponse>>
{

}
