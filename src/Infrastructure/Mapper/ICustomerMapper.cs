using AutoMapper;
using Nebim.Era.Plt.Comm.Customer.Application.Contracts;
using Nebim.Era.Plt.Comm.Customer.Domain;
using Nebim.Era.Plt.Comm.Customer.Domain.Contracts;
using Nebim.Era.Plt.Core;

namespace Infrastructure.Mapper;

public class CustomerMapperProfile : Profile
{
    public CustomerMapperProfile()
    {
        CreateMap<Customer, CustomerResponse>();
        CreateMap<Customer, CustomerEto>();
        CreateMap<CreateCustomerRequest, Customer>().ConstructUsing(request => new Customer(Uuid.Next()));
        CreateMap<UpdateCustomerRequest, Customer>().ConstructUsing(request => new Customer(request.Id));
    }
}

public interface ICustomerMapper
{
    CustomerResponse ToCustomerResponse(Customer customer);
    CustomerEto ToCustomerEto(Customer customer);
    Customer ToCustomer(CreateCustomerRequest request);
    Customer ToCustomer(Customer customer, UpdateCustomerRequest request);
    List<Customer> ToCustomers(List<Customer> requestList);
    List<CustomerResponse> ToCustomerResponse(List<Customer> requestList);
}

public class CustomerMapper : ICustomerMapper
{
    private readonly IMapper _mapper;

    public CustomerMapper(IMapper mapper)
    {
        _mapper = mapper;
    }

    public CustomerResponse ToCustomerResponse(Customer customer)
    {
        return _mapper.Map<CustomerResponse>(customer);
    }

    public CustomerEto ToCustomerEto(Customer customer)
    {
        return _mapper.Map<CustomerEto>(customer);
    }

    public Customer ToCustomer(Customer customer, UpdateCustomerRequest request)
    {
        return new Customer(
            request.Id,
            request.Code ?? customer.Code,
            request.Name ?? customer.Name,
            request.Surname ?? customer.Surname,
            request.BirthDate ?? customer.BirthDate,
            request.GenderCode ?? customer.GenderCode,
            request.PhoneNumber ?? customer.PhoneNumber,
            request.Email ?? customer.Email,
            request.CommunicationPreferences!.Any() ? request.CommunicationPreferences : customer.CommunicationPreferences,
            request.Addresses!.Any() ? request.Addresses : customer.Addresses
        )
        {
            CreatedAt = customer.CreatedAt,
            CreatedBy = customer.CreatedBy,
            DeletedAt = customer.DeletedAt,
            DeletedBy = customer.DeletedBy,
            TenantId = customer.TenantId
        };
    }

    public Customer ToCustomer(CreateCustomerRequest request)
    {
        var customerId = Uuid.Next();
        return new Customer(
            customerId,
            request.Code,
            request.Name,
            request.Surname,
            request.BirthDate,
            request.GenderCode,
            request.PhoneNumber,
            request.Email,
            request.CommunicationPreferences!,
            request.Addresses
        );
    }

    public List<Customer> ToCustomers(List<Customer> requestList)
    {
        var list = new List<Customer>();
        foreach (var request in requestList)
        {
            list.Add(new Customer(
                Uuid.Next(),
                request.Code,
                request.Name,
                request.Surname,
                request.BirthDate,
                request.GenderCode,
                request.PhoneNumber,
                request.Email,
                request.CommunicationPreferences,
                request.Addresses.ToList())
            {
                DeletedAt = request.DeletedAt,
                DeletedBy = request.DeletedBy,
                IsDeleted = request.IsDeleted,
                CreatedBy = request.CreatedBy,
                CreatedAt = request.CreatedAt,
                UpdatedBy = request.UpdatedBy,
                UpdatedAt = request.UpdatedAt,
                TenantId = request.TenantId
            });
        }

        return list;
    }

    public List<CustomerResponse> ToCustomerResponse(List<Customer> requestList)
    {
        var list = new List<CustomerResponse>();
        foreach (var request in requestList)
        {
            list.Add(new CustomerResponse()
            {
                Id = Uuid.Next(),
                Code = request.Code,
                Name = request.Name,
                Surname = request.Surname,
                BirthDate = request.BirthDate,
                GenderCode = request.GenderCode,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                CommunicationPreferences = request.CommunicationPreferences,
                Addresses = request.Addresses.ToList()
            });
        }

        return list;
    }
}
