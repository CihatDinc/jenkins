namespace Nebim.Era.Plt.Comm.Customer.Domain;

using Core;
using Nebim.Era.Plt.Data.Audit;

public class Passport : AggregateRoot,
    IHasTenantId,
    IHasSoftDelete,
    IDeleteAuditable,
    IUpdateAuditable,
    ICreateAuditable
{
    public Passport(Guid id, Guid customerId, string number, int issuingStateCode, int nationality, DateTimeOffset issueDate)
    {
        Id = id;
        CustomerId = customerId;
        Number = number;
        IssuingStateCode = issuingStateCode;
        Nationality = nationality;
        IssueDate = issueDate;
    }

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

    #region Platform Specific Props.

    public Guid TenantId { get; set; }
    public bool IsDeleted { get; set; }
    public Guid? DeletedBy { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    #endregion Platform Specific Props.
}
