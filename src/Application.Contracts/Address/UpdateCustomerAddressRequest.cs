using Nebim.Era.Plt.Core.Types;

namespace Nebim.Era.Plt.Comm.Customer.Application.Contracts;

[PublicAPI]
public sealed class UpdateCustomerAddressRequest : Address, IRequest<CustomerResponse>
{
    public required Guid CustomerId { get; set; }
    public required Guid CustomerAddressId { get; set; }
}