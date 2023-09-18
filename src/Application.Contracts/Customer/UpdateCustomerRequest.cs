namespace Nebim.Era.Plt.Comm.Customer.Application.Contracts;

[PublicAPI]
public class UpdateCustomerRequest : CustomerRequest, IRequest
{
    public Guid Id { get; set; }
}