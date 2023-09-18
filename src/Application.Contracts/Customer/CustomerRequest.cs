using Nebim.Era.Plt.Core.Types;

namespace Nebim.Era.Plt.Comm.Customer.Application.Contracts;

[PublicAPI]
public class CustomerRequest
{
    /// <summary>
    ///  Customer unique code that given by the user.
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// Customer Name
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Customer Surname
    /// </summary>
    public string? Surname { get; set; }

    /// <summary>
    /// Customer BirthDate
    /// </summary>
    public DateTimeOffset? BirthDate { get; set; }

    /// <summary>
    /// Customer Gender
    /// </summary>
    public Gender? GenderCode { get; set; }

    /// <summary>
    /// Customer PhoneNumber
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Customer Email
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Customer CommunicationPreference
    /// </summary>
    public List<CommunicationPreference>? CommunicationPreferences { get; set; } = new();

    /// <summary>
    /// Customer Addresses
    /// </summary>
    public List<Address>? Addresses { get; set; } = new();
}
