using FluentAssertions;
using Nebim.Era.Plt.Core;
using TestUtils;

namespace IntegrationTests;

using Infrastructure.Data.Repositories;

[Collection(nameof(DatabaseCollection))]
public class CustomerRepositoryTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _databaseFixture;

    public CustomerRepositoryTests(DatabaseFixture databaseFixture)
    {
        _databaseFixture = databaseFixture;
    }

    [Fact]
    public async Task CanCreate()
    {
        await using var dbContext = _databaseFixture.CreateDbContext();
        var repository = new CustomerRepository(dbContext);
        var customer = CustomerFakeFactory.Customer();
        var savedCustomer = await repository.Create(customer);
        savedCustomer.Should().BeEquivalentTo(customer);
    }

    [Fact]
    public async Task CanUpdate()
    {
        await using var dbContext = _databaseFixture.CreateDbContext();
        var repository = new CustomerRepository(dbContext);
        var customer = CustomerFakeFactory.Customer();
        await repository.Create(customer);
        customer.Name = Uuid.Next().ToString();
        var affected = await repository.Update(customer);
        affected.Should().Be(1);
        var savedCustomer = await repository.Get(customer.Id);
        savedCustomer.Should().BeEquivalentTo(customer);
    }

    [Fact]
    public async Task CanDelete()
    {
        await using var dbContext = _databaseFixture.CreateDbContext();
        var repository = new CustomerRepository(dbContext);
        var customer = CustomerFakeFactory.Customer();
        var savedCustomer = await repository.Create(customer);
        var affected = await repository.Delete(savedCustomer.Id);
        affected.Should().Be(1);
        savedCustomer = await repository.Get(savedCustomer.Id);
        savedCustomer!.IsDeleted.Should().BeTrue();
    }

    [Fact]
    public async Task CanSearch()
    {
        await using var dbContext = _databaseFixture.CreateDbContext();
        var repository = new CustomerRepository(dbContext);
        var customer = CustomerFakeFactory.Customer();
        var savedCustomer = await repository.Create(customer);
        var response = await repository.List();
        var searchResult = response.ToArray();
        searchResult.Should().NotBeNull();
        searchResult.Should().HaveCountGreaterThan(0);
        searchResult.Should().ContainEquivalentOf(savedCustomer, options => options.ExcludingMissingMembers());
    }

    [Fact]
    public async Task Get_ReturnsAnCustomer_WhenCustomerIdExist()
    {
        await using var dbContext = _databaseFixture.CreateDbContext();
        var repository = new CustomerRepository(dbContext);
        var customer = CustomerFakeFactory.Customer();
        await repository.Create(customer);
        var savedCustomer = await repository.Get(customer.Id);
        savedCustomer.Should().BeEquivalentTo(customer);
    }

    [Fact]
    public async Task Get_ReturnsNull_WhenIdNotExist()
    {
        await using var dbContext = _databaseFixture.CreateDbContext();
        var repository = new CustomerRepository(dbContext);
        var result = await repository.Get(Uuid.Next());
        result.Should().BeNull();
    }
}
