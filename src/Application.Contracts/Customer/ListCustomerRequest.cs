using Nebim.Era.Plt.Data;

namespace Nebim.Era.Plt.Comm.Customer.Application.Contracts;

[PublicAPI]
public class ListCustomerRequest : ListRequest, IRequest<IEnumerable<CustomerResponse>>
{
}