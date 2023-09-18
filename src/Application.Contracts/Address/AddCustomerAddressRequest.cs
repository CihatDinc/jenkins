using Nebim.Era.Plt.Core.Types;

namespace Nebim.Era.Plt.Comm.Customer.Application.Contracts;

[PublicAPI]
public sealed class AddCustomerAddressRequest : Address, IRequest<CustomerResponse>
{
    public required Guid CustomerId { get; set; }
}