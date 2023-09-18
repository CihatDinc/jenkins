using FluentAssertions;
using Moq;
using Nebim.Era.Plt.Comm.Customer.Application.Contracts;
using Nebim.Era.Plt.Comm.Customer.Domain;
using Nebim.Era.Plt.Core.Types;
using TestUtils;

namespace Application.Tests;

public class CreateCustomerHandlerTests
{
    [Fact]
    public async Task Calls_Create()
    {
        var mockRepository = new Mock<ICustomerRepository>();
        mockRepository.Setup(repo => repo.Create(It.IsAny<Nebim.Era.Plt.Comm.Customer.Domain.Customer>(), It.IsAny<CancellationToken>())).ReturnsAsync((Nebim.Era.Plt.Comm.Customer.Domain.Customer customer, CancellationToken _) => customer);

        var handler = new CreateCustomerHandler(mockRepository.Object, CustomerMapperFactory.Create());
        var createCustomerRequest = new CreateCustomerRequest
        {
            Code = "XHU26KYD5IW",
            Name = "Doctor",
            Surname = "Alban",
            CommunicationPreferences = new List<CommunicationPreference>()
        };

        var customerResponse = await handler.Handle(createCustomerRequest, default);
        mockRepository.Verify(c => c.Create(It.IsAny<Nebim.Era.Plt.Comm.Customer.Domain.Customer>(), It.IsAny<CancellationToken>()), Times.Once());
        customerResponse.Should().BeEquivalentTo(createCustomerRequest, options => options.ExcludingMissingMembers());
    }
}
