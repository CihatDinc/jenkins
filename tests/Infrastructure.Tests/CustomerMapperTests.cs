using FluentAssertions;
using TestUtils;

namespace Infrastructure.Tests;

public class CustomerMapperTests
{
    [Fact]
    public void Map_Customer_To_CustomerResponse()
    {
        var mapper = CustomerMapperFactory.Create();
        var customer = CustomerFakeFactory.Customers.First();
        var customerResponse = mapper.ToCustomerResponse(customer);
        customerResponse.Should().BeEquivalentTo(customer, options => options.ExcludingMissingMembers());
    }

    [Fact]
    public void Map_CreateCustomerRequest_To_Customer()
    {
 
        var mapper = CustomerMapperFactory.Create();
        var createCustomerRequest = CustomerFakeFactory.CreateCustomerRequest();
        var customer = mapper.ToCustomer(createCustomerRequest);
        customer.Id.Should().NotBeEmpty();
        customer.Should().BeEquivalentTo(createCustomerRequest, options => options.ExcludingMissingMembers());
    }

    [Fact]
    public void Map_UpdateCustomerRequest_To_Customer()
    {
        var mapper = CustomerMapperFactory.Create();
        var customer = CustomerFakeFactory.Customer();
        var updateCustomerRequest = CustomerFakeFactory.UpdateCustomerRequest();
        var updatedCustomer = mapper.ToCustomer(customer, updateCustomerRequest);
        updatedCustomer.Should().BeEquivalentTo(updateCustomerRequest, options => options.ExcludingMissingMembers());
    }
}