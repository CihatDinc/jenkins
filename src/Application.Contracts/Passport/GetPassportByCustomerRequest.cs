namespace Nebim.Era.Plt.Comm.Customer.Application.Contracts;
public class GetPassportByCustomerRequest : IRequest<PassportResponse>
{
    public Guid CustomerId { get; set; }
}
