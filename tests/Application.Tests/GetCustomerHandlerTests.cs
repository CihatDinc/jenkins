using FluentAssertions;
using Moq;
using Nebim.Era.Plt.Comm.Customer.Application.Contracts;
using Nebim.Era.Plt.Comm.Customer.Domain;
using Nebim.Era.Plt.Core;
using TestUtils;

namespace Application.Tests;

public class GetCustomerHandlerTests
{
    [Fact]
    public async Task Call_Get()
    {
        var customerId = CustomerFakeFactory.Customers[new Random().Next(0, 4)].Id;
        var mockRepository = new Mock<ICustomerRepository>();
        mockRepository.Setup(repo => repo.Get(It.IsAny<Guid>(),default)).ReturnsAsync((Guid id, CancellationToken _) => CustomerFakeFactory.Customers.FirstOrDefault(s => s.Id.Equals(id)));
        var handler = new GetCustomerHandler(mockRepository.Object,  CustomerMapperFactory.Create());
        var getCustomerRequest = new GetCustomerRequest { Id = customerId };
        var customerResponse = await handler.Handle(getCustomerRequest, default);
        customerResponse.Should().BeEquivalentTo(CustomerFakeFactory.Customers.FirstOrDefault(s => s.Id.Equals(customerId)), options => options.ExcludingMissingMembers());
        mockRepository.Verify(c => c.Get(It.Is<Guid>(id => id.Equals(customerId)),default), Times.Once());
    }

    [Fact]
    public async Task Throw_EraNotFoundException_WhenRepositoryReturnsNull()
    {
        var mockRepo = new Mock<ICustomerRepository>();
        mockRepo.Setup(repo => repo.Get(It.IsAny<Guid>(),default)).ReturnsAsync(() => null);
        var handler = new GetCustomerHandler(mockRepo.Object,  CustomerMapperFactory.Create());
        var getCustomerRequest = new GetCustomerRequest { Id = Guid.NewGuid() };
        var handleAction = () => handler.Handle(getCustomerRequest, default);
        await handleAction.Should().ThrowAsync<EraNotFoundException>();
    }
}