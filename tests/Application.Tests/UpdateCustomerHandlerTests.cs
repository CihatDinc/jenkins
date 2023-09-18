using FluentAssertions;
using Moq;
using Nebim.Era.Plt.Comm.Customer.Application.Contracts;
using Nebim.Era.Plt.Comm.Customer.Domain;
using Nebim.Era.Plt.Core;
using TestUtils;

namespace Application.Tests;

public class UpdateCustomerHandlerTests
{
    [Fact]
    public async Task Call_Update()
    {
        var mockRepository = new Mock<ICustomerRepository>();
        mockRepository
            .Setup(repo => repo.GetAsTracking(It.IsAny<Guid>(), default)).
            ReturnsAsync(() => new Nebim.Era.Plt.Comm.Customer.Domain.Customer(Guid.NewGuid()));
        mockRepository
            .Setup(repo => repo.Update(It.IsAny<Nebim.Era.Plt.Comm.Customer.Domain.Customer>(), default)).
            ReturnsAsync(() => 1);

        var handler = new UpdateCustomerHandler(mockRepository.Object, CustomerMapperFactory.Create());
        var updateCustomerRequest = new UpdateCustomerRequest
        {
            Id = Uuid.Next(),
            Code = "XHU26KYD5IW",
        };
            
        await handler.Handle(updateCustomerRequest, default);

        mockRepository.Verify(c => c.Update(It.IsAny<Nebim.Era.Plt.Comm.Customer.Domain.Customer>(), default), Times.Once());
    }

    [Fact]
    public async Task Throw_EraNotFoundException_WhenEffectedRecordZero()
    {
        var mockRepository = new Mock<ICustomerRepository>();
        mockRepository.Setup(repo => repo.Update(It.IsAny<Nebim.Era.Plt.Comm.Customer.Domain.Customer>(), default)).ReturnsAsync(() => 0);

        var handler = new UpdateCustomerHandler(mockRepository.Object, CustomerMapperFactory.Create());
        var updateCustomerRequest = new UpdateCustomerRequest
        {
            Id = Uuid.Next(),
            Code = "XHU26KYD5IW",
        };

        var handleAction = () => handler.Handle(updateCustomerRequest, default);
        await handleAction.Should().ThrowAsync<EraNotFoundException>();
    }
}
