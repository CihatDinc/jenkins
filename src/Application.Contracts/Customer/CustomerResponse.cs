using Nebim.Era.Plt.Core.Types;

namespace Nebim.Era.Plt.Comm.Customer.Application.Contracts;

[PublicAPI]
public class CustomerResponse
{
    /// <summary>
    ///  Customer unique id that given by the system.
    /// </summary>
    public Guid Id { get; set; }

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
    /// Customer Address
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Customer CommunicationPreference 0=None, 1=Call, 2=Sms, 3=Email
    /// </summary>
    public List<CommunicationPreference>? CommunicationPreferences { get; set; }

    /// <summary>
    /// Customer Addresses
    /// </summary>
    public List<Address> Addresses { get; set; } = new();
}