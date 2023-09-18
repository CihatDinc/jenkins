using Nebim.Era.Plt.Comm.Customer.Domain.Contracts;
using Nebim.Era.Plt.Core;
using Nebim.Era.Plt.Core.Events;
using Nebim.Era.Plt.Core.Types;
using Nebim.Era.Plt.Data.Audit;

namespace Nebim.Era.Plt.Comm.Customer.Domain;

public class Customer : AggregateRoot,
    IHasTenantId,
    IHasSoftDelete,
    IDeleteAuditable,
    IUpdateAuditable,
    ICreateAuditable,
    IHasChangeDataEvents
{
    public Customer(Guid id)
    {
        Id = id;
        CommunicationPreferences = new List<CommunicationPreference>();
        _addresses = new List<Address>();
    }

    public Customer(Guid id,
        string? code,
        string? name,
        string? surname,
        DateTimeOffset? birthDate,
        Gender? genderCode,
        string? phoneNumber,
        string? email,
        IEnumerable<CommunicationPreference>? communicationPreferences,
        IEnumerable<Address>? customerAddresses) : this(id)
    {
        Code = code;
        Name = name;
        Surname = surname;
        BirthDate = birthDate;
        GenderCode = genderCode;
        PhoneNumber = phoneNumber;
        Email = email;
        CommunicationPreferences = communicationPreferences?.ToList() ?? new List<CommunicationPreference>();
        var addresses = customerAddresses?.ToList() ?? new List<Address>();
        addresses.ForEach(AddAddress);
    }

    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public DateTimeOffset? BirthDate { get; set; }
    public Gender? GenderCode { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    private readonly List<Address> _addresses;

    public IReadOnlyCollection<Address> Addresses => _addresses;

    public List<CommunicationPreference> CommunicationPreferences { get; set; }

    public void AddAddress(Address customerAddress)
    {
        if (customerAddress.Id == Guid.Empty)
            customerAddress.Id = Uuid.Next();

        _addresses.Add(customerAddress);

        if (customerAddress.IsDefault || _addresses.Count == 1)
            AssignNewDefaultAddress(customerAddress.Id);
    }

    public void RemoveAddress(Guid customerAddressId)
    {
        var customerAddress = _addresses.FirstOrDefault(o => o.Id == customerAddressId);
        if (customerAddress is null)
            throw new EraNotFoundException(ExceptionMessages.CouldNotFindCustomerAddressById(customerAddressId));

        var others = _addresses.Where(address => address.Id != customerAddressId)
            .ToList();

        // kaldırılan address default sa ilk adressi default yapmak gereklimi ?
        if (customerAddress.IsDefault && others.Any())
        {
            var first = others.First();
            first.IsDefault = true;
        }

        _addresses.Remove(customerAddress);
    }

    public void UpdateAddress(Guid addressId, Address customerAddress)
    {
        var address = _addresses.FirstOrDefault(s => s.Id == addressId);
        if (address is null)
            throw new EraNotFoundException(ExceptionMessages.CouldNotFindCustomerAddressById(customerAddress.Id));

        if (customerAddress.IsDefault)
            AssignNewDefaultAddress(customerAddress.Id);

        MapAddress(address, customerAddress);
    }

    private void AssignNewDefaultAddress(Guid addressId)
    {
        var existingAddress = _addresses.FirstOrDefault(s => s.Id == addressId);
        if (existingAddress is null)
            throw new EraNotFoundException(ExceptionMessages.CouldNotFindCustomerAddressById(addressId));

        foreach (var address in _addresses)
            address.IsDefault = false;
        existingAddress.IsDefault = true;
    }

    private static void MapAddress(Address fromAddress, Address toAddress)
    {
        fromAddress.IsDefault = toAddress.IsDefault;
        fromAddress.Email = toAddress.Email;
        fromAddress.AddressLine = toAddress.AddressLine;
        fromAddress.AddressName = toAddress.AddressName;
        fromAddress.CityId = toAddress.CityId;
        fromAddress.CityName = toAddress.CityName;
        fromAddress.CompanyName = toAddress.CompanyName;
        fromAddress.CountryId = toAddress.CountryId;
        fromAddress.CountryName = toAddress.CountryName;
        fromAddress.DistrictId = toAddress.DistrictId;
        fromAddress.DistrictName = toAddress.DistrictName;
        fromAddress.FirstName = toAddress.FirstName;
        fromAddress.IdentityNumber = toAddress.IdentityNumber;
        fromAddress.InvoiceType = toAddress.InvoiceType;
        fromAddress.LastName = toAddress.LastName;
        fromAddress.PhoneNumber = toAddress.PhoneNumber;
        fromAddress.PostalCode = toAddress.PostalCode;
        fromAddress.TaxNumber = toAddress.TaxNumber;
        fromAddress.TaxOffice = toAddress.TaxOffice;
    }

    #region Change Data Events

    public CustomerEto GetCustomerEto()
    {
        return new CustomerEto()
        {
            Code = Code,
            Email = Email,
            Id = Id,
            Name = Name,
            Surname = Surname,
            BirthDate = BirthDate,
            CommunicationPreferences = CommunicationPreferences,
            PhoneNumber = PhoneNumber,
            GenderCode = GenderCode
        };
    }

    public void RaiseCreatedEvent()
    {
        var createdDataEvent = EventFactory<CustomerEto>.CreatedDataEvent(GetCustomerEto(), Id);

        AddIntegrationEvent(createdDataEvent);
    }

    public void RaiseUpdatedEvent()
    {
        var updatedDataEvent = EventFactory<CustomerEto>.UpdatedDataEvent(GetCustomerEto(), Id);

        AddIntegrationEvent(updatedDataEvent);
    }

    public void RaiseDeletedEvent()
    {
        var deletedDataEvent = EventFactory<CustomerEto>.DeletedDataEvent(Id, Id);

        AddIntegrationEvent(deletedDataEvent);
    }

    #endregion Change Data Events

    #region Platform Props

    public bool IsDeleted { get; set; }
    public Guid? DeletedBy { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public Guid TenantId { get; set; }

    #endregion Platform Props
}
