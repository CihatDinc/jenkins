namespace Infrastructure.Data.Repositories;

using System.Data;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Nebim.Era.Plt.Comm.Customer.Application.Contracts;
using Nebim.Era.Plt.Comm.Customer.Domain;
using Nebim.Era.Plt.Core;
using Nebim.Era.Plt.Core.Types;
using Nebim.Era.Plt.Data.EntityFramework;

public class CustomerRepository : CrudRepository<CustomerDbContext, Customer, Guid>, ICustomerRepository
{
    private readonly CustomerDbContext _dbContext;

    public CustomerRepository(CustomerDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Customer?> GetAsTracking(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Customers
            .Where(customer => customer.Id == id)
            .Include(customer => customer.Addresses)
            .AsTracking()
            .FirstOrDefaultAsync(cancellationToken);
    }
    public async Task<Customer?> GetAsNoTracking(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Customers
            .Where(customer => customer.Id == id)
            .Include(customer => customer.Addresses)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Roslynator", "RCS1003:Add braces to if-else (when expression spans over multiple lines).", Justification = "<Pending>")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Roslynator", "RCS1238:Avoid nested ?: operators.", Justification = "<Pending>")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3358:Ternary operators should not be nested", Justification = "<Pending>")]
    public async Task<List<Customer>> GetTemporaryCustomers(ImportCustomerRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query =
                $"select CustomerCode as code,BirthDate as birth_date,Gender as gender_code,CommunicationPreferences as communication_preferences,AddressName as address_name,FirstName as first_name, LastName as last_name,Email as email,PhoneNumber as phone_number, Address as address_line,{1} as country_id,Country as country_name,{1} as city_id,City as city_name,{1} as district_id,District as district_name,PostalCode as postal_code,IdentityNumber as identity_number,InvoiceType as invoice_type,CompanyName as company_name,TaxOffice as tax_office,TaxNumber as tax_number,IsDefault as is_default, NULL as deleted_at,NULL as deleted_by,false as is_deleted from plt_comm_import_export.`{request.TableName}`";
            using var command = _dbContext.Database.GetDbConnection().CreateCommand();
            command.CommandText = query;
            await _dbContext.Database.OpenConnectionAsync(cancellationToken: cancellationToken);
            using var reader = await command.ExecuteReaderAsync(cancellationToken);
            var customers = new List<Customer>();
            while (reader.Read())
            {
                var existentCustomer = customers.FirstOrDefault(o => o.Code == TryToGetString(reader, "code"));
                if (existentCustomer != null)
                    existentCustomer.AddAddress(new Address()
                    {
                        Id = Uuid.Next(),
                        AddressName = TryToGetString(reader, "address_name"),
                        FirstName = TryToGetString(reader, "first_name"),
                        LastName = TryToGetString(reader, "last_name"),
                        PhoneNumber = TryToGetString(reader, "phone_number"),
                        Email = TryToGetString(reader, "email"),
                        IdentityNumber = TryToGetString(reader, "identity_number"),
                        CountryId = 1,
                        CountryName = TryToGetString(reader, "country_name"),
                        CityId = 1,
                        CityName = TryToGetString(reader, "city_name"),
                        DistrictId = 1,
                        DistrictName = TryToGetString(reader, "district_name"),
                        AddressLine = TryToGetString(reader, "address_line"),
                        PostalCode = TryToGetString(reader, "postal_code"),
                        IsDefault = TryToGetString(reader, "is_default").Trim() == "0" ? false : true,
                        InvoiceType = InvoiceType.Corporate,
                        CompanyName = TryToGetString(reader, "company_name"),
                        TaxOffice = TryToGetString(reader, "tax_office"),
                        TaxNumber = TryToGetString(reader, "tax_number")
                    });
                else
                {
                    var customerId = Uuid.Next();
                    var customer = new Customer(
                        customerId,
                        TryToGetString(reader, "code"),
                        TryToGetString(reader, "first_name"),
                        TryToGetString(reader, "last_name"),
                        string.IsNullOrEmpty(TryToGetString(reader, "birth_date")) ? DateTimeOffset.Now : DateTime.Parse(TryToGetString(reader, "birth_date")),
                        TryToGetString(reader, "gender_code").Trim() == "erkek" ? Gender.Male :
                        TryToGetString(reader, "gender_code").Trim() == "kadın" ? Gender.Female : Gender.None,
                        TryToGetString(reader, "phone_number"),
                        TryToGetString(reader, "email"),
                        TryToGetString(reader, "communication_preferences").Trim() == "eposta"
                            ? new List<CommunicationPreference>()
                            {
                                CommunicationPreference.Email
                            }
                            : new List<CommunicationPreference>() { CommunicationPreference.None },
                        new List<Address>())
                    {
                        DeletedAt = null,
                        DeletedBy = null,
                        IsDeleted = false,
                        CreatedBy = request.UserId,
                        CreatedAt = DateTimeOffset.Now,
                        UpdatedBy = null,
                        UpdatedAt = null,
                        TenantId = request.TenantId
                    };
                    customer.AddAddress(new Address()
                    {
                        Id = Uuid.Next(),
                        AddressName = TryToGetString(reader, "address_name"),
                        FirstName = TryToGetString(reader, "first_name"),
                        LastName = TryToGetString(reader, "last_name"),
                        PhoneNumber = TryToGetString(reader, "phone_number"),
                        Email = TryToGetString(reader, "email"),
                        IdentityNumber = TryToGetString(reader, "identity_number"),
                        CountryId = 1,
                        CountryName = TryToGetString(reader, "country_name"),
                        CityId = 1,
                        CityName = TryToGetString(reader, "city_name"),
                        DistrictId = 1,
                        DistrictName = TryToGetString(reader, "district_name"),
                        AddressLine = TryToGetString(reader, "address_line"),
                        PostalCode = TryToGetString(reader, "postal_code"),
                        IsDefault = TryToGetString(reader, "is_default").Trim() == "0" ? false : true,
                        InvoiceType = InvoiceType.Corporate,
                        CompanyName = TryToGetString(reader, "company_name"),
                        TaxOffice = TryToGetString(reader, "tax_office"),
                        TaxNumber = TryToGetString(reader, "tax_number")
                    });
                    customers.Add(customer);
                }
            }
            await _dbContext.Database.CloseConnectionAsync();
            return customers;
        }
        catch (Exception e)
        {
            throw new InvalidDataException($"Invalid data importation: {e.Message}");
        }
    }


    public async Task BulkCreate(IEnumerable<Customer> customers, CancellationToken cancellationToken = default)
    {
        try
        {
            var list = customers.ToList();
            foreach (var customer in list)
                _dbContext.Entry(customer).State = EntityState.Detached;
            await _dbContext.Customers.AddRangeAsync(list, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            throw new EraUnprocessableException($"Customer: {e.Message}");
        }
    }

    public async Task<Customer?> GetByPassportNumber(string passportNumber, CancellationToken cancellationToken = default)
    {
        var query =
            from p in _dbContext.Passports
            join c in _dbContext.Customers
                on p.CustomerId equals c.Id
            where p.Number == passportNumber
            select c;

        var customer = await query.FirstOrDefaultAsync(cancellationToken);
        return customer;
    }

    public async Task<List<Customer>> GetBy(GetCustomerByPassportInfoRequest r, CancellationToken cancellationToken = default)
    {
        var query =
            from c in _dbContext.Customers.Include(t => t.Addresses.Where(t => !r.CountryId.HasValue || t.CountryId == r.CountryId))
            join p in _dbContext.Passports.Where(t => t.Number == r.PassportNumber)
                on c.Id equals p.CustomerId
            where p.Number == r.PassportNumber
            //&& (string.IsNullOrWhiteSpace(name) || c.Name != null && c.Name.Contains(name)
            //&& (string.IsNullOrWhiteSpace(surname) || c.Surname != null && c.Surname.Contains(surname))
            select c;

        if (!string.IsNullOrWhiteSpace(r.FirstName))
            query = query.Where(t => t.Name != null && t.Name == r.FirstName);

        if (!string.IsNullOrWhiteSpace(r.LastName))
            query = query.Where(t => t.Surname != null && t.Surname == r.LastName);

        var customers = await query.ToListAsync(cancellationToken);
        return customers;
    }

    private static string TryToGetString(DbDataReader reader, string name)
    {
        ArgumentNullException.ThrowIfNull(reader);
        try
        {
            return reader.GetString(reader.GetOrdinal(name));
        }
        catch (Exception e)
        {
            throw new EraUnprocessableException($"Invalid Data for {name}: {e.Message}");
        }
    }
}
