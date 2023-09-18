namespace Nebim.Era.Plt.Comm.Customer.Application.Contracts;

public class PassportRequest
{
    public Guid CustomerId { get; set; }

    /// <summary>
    /// Passport number
    /// </summary>
    public string Number { get; set; }

    /// <summary>
    /// Corresponds to "ISO 3166-1 numeric" compliant three-digit country code
    /// </summary>
    public int IssuingStateCode { get; set; }

    /// <summary>
    /// Corresponds to "ISO 3166-1 numeric" compliant three-digit country code
    /// </summary>
    public int Nationality { get; set; }

    public DateTimeOffset IssueDate { get; set; }
}
