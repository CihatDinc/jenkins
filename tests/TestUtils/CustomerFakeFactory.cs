using Bogus;
using Nebim.Era.Plt.Comm.Customer.Application.Contracts;
using Nebim.Era.Plt.Comm.Customer.Domain;
using Nebim.Era.Plt.Core;
using Nebim.Era.Plt.Core.Types;

namespace TestUtils;

using Address = Nebim.Era.Plt.Core.Types.Address;

public static class CustomerFakeFactory
{
    public static List<Customer> Customers { get; set; } = CustomerFaker().Generate(5);
    public static List<Address> CustomerAddress { get; set; } = AddressFaker().Generate(5);
    public static Customer Customer() => CustomerFaker().Generate();
    public static CreateCustomerRequest CreateCustomerRequest() => CreateCustomerRequestFaker().Generate();
    public static UpdateCustomerRequest UpdateCustomerRequest() => UpdateCustomerRequestFaker().Generate();

    public static AddCustomerAddressRequest AddCustomerAddressRequest() =>
        AddCustomerAddressRequestFaker().Generate();

    public static UpdateCustomerAddressRequest UpdateCustomerAddressRequest() =>
        UpdateCustomerAddressRequestFaker().Generate();

    public static DeleteCustomerAddressRequest DeleteCustomerAddressRequest() =>
        DeleteCustomerAddressRequestFaker().Generate();

    private static Faker<Customer> CustomerFaker()
    {
        var faker = new Faker<Customer>()
            .CustomInstantiator(_ => new Customer(Uuid.Next()))
            .RuleFor(c => c.Code, Uuid.Next().ToString())
            .RuleFor(c => c.Name, f => f.Person.FirstName)
            .RuleFor(c => c.Surname, f => f.Person.LastName)
            .RuleFor(c => c.BirthDate, _ => SystemClock.UtcNow.AddYears(new Random().Next(-50, -20)))
            .RuleFor(c => c.GenderCode, f => f.Random.Enum<Gender>())
            .RuleFor(c => c.PhoneNumber, f => f.Person.Phone)
            .RuleFor(c => c.Email, f => f.Person.Email)
            .RuleFor(c => c.Addresses, _ => AddressFaker().Generate(2))
            .RuleFor(c => c.CommunicationPreferences, f => new List<CommunicationPreference>
            {
                f.Random.Enum<CommunicationPreference>()
            });

        return faker;
    }

    private static Faker<CreateCustomerRequest> CreateCustomerRequestFaker()
    {
        return new Faker<CreateCustomerRequest>()
            .RuleFor(c => c.Code, _ => Uuid.Next().ToString())
            .RuleFor(c => c.Name, f => f.Person.FirstName)
            .RuleFor(c => c.Surname, f => f.Person.LastName)
            .RuleFor(c => c.BirthDate, _ => SystemClock.UtcNow.AddYears(new Random().Next(-50, -20)))
            .RuleFor(c => c.GenderCode, f => f.Random.Enum<Gender>())
            .RuleFor(c => c.PhoneNumber, f => f.Person.Phone)
            .RuleFor(c => c.Email, f => f.Person.Email)
            
            .RuleFor(c => c.CommunicationPreferences, f => new List<CommunicationPreference>
            {
                f.Random.Enum<CommunicationPreference>()
            });
    }

    private static Faker<UpdateCustomerRequest> UpdateCustomerRequestFaker()
    {
        return new Faker<UpdateCustomerRequest>()
            .RuleFor(c => c.Id, _ => Uuid.Next())
            .RuleFor(c => c.Code, _ => Uuid.Next().ToString())
            .RuleFor(c => c.Name, f => f.Person.FirstName)
            .RuleFor(c => c.Surname, f => f.Person.LastName)
            .RuleFor(c => c.BirthDate, _ => SystemClock.UtcNow.AddYears(new Random().Next(-50, -20)))
            .RuleFor(c => c.GenderCode, f => f.Random.Enum<Gender>())
            .RuleFor(c => c.PhoneNumber, f => f.Person.Phone)
            .RuleFor(c => c.Email, f => f.Person.Email)
            .RuleFor(c => c.CommunicationPreferences, f => new List<CommunicationPreference>
            {
                f.Random.Enum<CommunicationPreference>()
            });
    }
    
    private static Faker<AddCustomerAddressRequest> AddCustomerAddressRequestFaker()
    {
        return new Faker<AddCustomerAddressRequest>()
            .RuleFor(c => c.CustomerId, _ => Uuid.Next())
            .RuleFor(c => c.AddressName, f => f.Random.String())
            .RuleFor(c => c.FirstName, f => f.Person.FirstName)
            .RuleFor(c => c.LastName, f => f.Person.LastName)
            .RuleFor(c => c.PhoneNumber, f => f.Person.Phone)
            .RuleFor(c => c.Email, f => f.Person.Email)
            .RuleFor(c => c.IdentityNumber, f => f.Random.Int().ToString())
            .RuleFor(c => c.CountryId, f => f.Random.Int())
            .RuleFor(c => c.CountryName, f => f.Address.Country())
            .RuleFor(c => c.CityId, f =>  f.Random.Int())
            .RuleFor(c => c.CityName, f => f.Address.City())
            .RuleFor(c => c.DistrictId,f => f.Random.Int())
            .RuleFor(c => c.DistrictName, f => f.Address.State())
            .RuleFor(c => c.AddressLine, f => f.Address.FullAddress())
            .RuleFor(c => c.PostalCode, f => f.Address.ZipCode())
            .RuleFor(c => c.InvoiceType, _ => InvoiceType.Personal)
            .RuleFor(c => c.CompanyName, f => f.Person.Company.Name)
            .RuleFor(c => c.TaxOffice, f => f.Random.Int().ToString())
            .RuleFor(c => c.TaxNumber, f => f.Random.Int().ToString());
    }
    
    private static Faker<UpdateCustomerAddressRequest> UpdateCustomerAddressRequestFaker()
    {
        return new Faker<UpdateCustomerAddressRequest>()
            .RuleFor(c => c.CustomerId, _ => Uuid.Next())
            .RuleFor(c => c.CustomerAddressId, _ => Uuid.Next())
            .RuleFor(c => c.AddressName, f => f.Random.String())
            .RuleFor(c => c.FirstName, f => f.Person.FirstName)
            .RuleFor(c => c.LastName, f => f.Person.LastName)
            .RuleFor(c => c.PhoneNumber, f => f.Person.Phone)
            .RuleFor(c => c.Email, f => f.Person.Email)
            .RuleFor(c => c.IdentityNumber, f => f.Random.Int().ToString())
            .RuleFor(c => c.CountryId, f => f.Random.Int())
            .RuleFor(c => c.CountryName, f => f.Address.Country())
            .RuleFor(c => c.CityId, f =>  f.Random.Int())
            .RuleFor(c => c.CityName, f => f.Address.City())
            .RuleFor(c => c.DistrictId,f => f.Random.Int())
            .RuleFor(c => c.DistrictName, f => f.Address.State())
            .RuleFor(c => c.AddressLine, f => f.Address.FullAddress())
            .RuleFor(c => c.PostalCode, f => f.Address.ZipCode())
            .RuleFor(c => c.InvoiceType, _ => InvoiceType.Personal)
            .RuleFor(c => c.CompanyName, f => f.Person.Company.Name)
            .RuleFor(c => c.TaxOffice, f => f.Random.Int().ToString())
            .RuleFor(c => c.TaxNumber, f => f.Random.Int().ToString());
    }
    
    private static Faker<DeleteCustomerAddressRequest> DeleteCustomerAddressRequestFaker()
    {
        return new Faker<DeleteCustomerAddressRequest>()
            .RuleFor(c => c.CustomerId, _ => Uuid.Next())
            .RuleFor(c => c.CustomerAddressId, _ => Uuid.Next());
    }

    public static Faker<Address> AddressFaker()
    {
        return new Faker<Address>()
            .CustomInstantiator(f => new Address
            {
                Id = Guid.NewGuid(),
                AddressName = f.Address.StateAbbr(),
                FirstName = f.Person.FirstName,
                LastName = f.Person.LastName,
                PhoneNumber = f.Person.Phone,
                Email = f.Person.Email,
                IdentityNumber = f.Random.Int(0).ToString(),
                CountryId = f.Random.Int(0, 100),
                CountryName = f.Address.Country(),
                CityId = f.Random.Int(0, 100),
                CityName = f.Address.City(),
                DistrictId = f.Random.Int(0, 100),
                DistrictName = f.Address.City(),
                AddressLine = f.Address.FullAddress(),
                PostalCode = f.Address.ZipCode(),
                CompanyName = f.Company.CompanyName(),
                TaxOffice = f.Random.Short(0).ToString(),
                TaxNumber = f.Random.Short(0).ToString(),
                IsDefault = false,
                InvoiceType = InvoiceType.Personal
            });
    }
}
