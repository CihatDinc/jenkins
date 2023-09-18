namespace Nebim.Era.Plt.Comm.Customer.Application.Contracts;

[PublicAPI]
public class GetCustomerByPassportInfoRequest : IRequest<IEnumerable<CustomerResponse>>
{
    public string PassportNumber { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int? CountryId { get; set; }
}
