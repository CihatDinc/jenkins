namespace Nebim.Era.Plt.Comm.Customer.Application.Contracts;

[PublicAPI]
public class GetCustomerRequest : IRequest<CustomerResponse>
{
    public Guid Id { get; set; }
}
