using JetBrains.Annotations;
using Nebim.Era.Plt.Core.Types;

namespace Nebim.Era.Plt.Comm.Customer.Domain.Contracts;

[UsedImplicitly]
public sealed record CustomerEto
{
    /// <summary>
    ///  Customer unique id that given by the system.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    ///  Customer unique code that given by the user.
    /// </summary>
    public string? Code { get; init; }

    /// <summary>
    /// Customer Name
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// Customer Surname
    /// </summary>
    public string? Surname { get; init; }

    /// <summary>
    /// Customer BirthDate
    /// </summary>
    public DateTimeOffset? BirthDate { get; init; }

    /// <summary>
    /// Customer Gender
    /// </summary>
    public Gender? GenderCode { get; init; }

    /// <summary>
    /// Customer PhoneNumber
    /// </summary>
    public string? PhoneNumber { get; init; }

    /// <summary>
    /// Customer Email
    /// </summary>
    public string? Email { get; init; }

    /// <summary>
    /// Customer CommunicationPreference 0=None, 1=Call, 2=Sms, 3=Email
    /// </summary>
    public List<CommunicationPreference>? CommunicationPreferences { get; init; }
}