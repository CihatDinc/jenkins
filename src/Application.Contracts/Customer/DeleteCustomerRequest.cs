namespace Nebim.Era.Plt.Comm.Customer.Application.Contracts;

[PublicAPI]
public sealed class DeleteCustomerRequest : IRequest
{
    public Guid Id { get; set; }
}