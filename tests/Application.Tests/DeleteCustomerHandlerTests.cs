using FluentAssertions;
using Moq;
using Nebim.Era.Plt.Comm.Customer.Application.Contracts;
using Nebim.Era.Plt.Comm.Customer.Domain;
using Nebim.Era.Plt.Core;

namespace Application.Tests;

public class DeleteCustomerHandlerTests
{
    [Fact]
    public async Task Call_Delete()
    {
        var customerId = Uuid.Next();
        var mockRepository = new Mock<ICustomerRepository>();
        mockRepository.Setup(repo => repo.Delete(It.IsAny<Guid>(),default)).ReturnsAsync(() => 1);
        
        var handler = new DeleteCustomerHandler(mockRepository.Object);
        var deleteCustomerRequest = new DeleteCustomerRequest { Id = customerId };
        await handler.Handle(deleteCustomerRequest, default);
        
        mockRepository.Verify(c => c.Delete(It.Is<Guid>(id => id.Equals(customerId)),default), Times.Once());
    }

    [Fact]
    public async Task Throw_EraValidationException_WhenEffectedRecordZero()
    {
        var mockRepository = new Mock<ICustomerRepository>();
        mockRepository.Setup(repo => repo.Delete(It.IsAny<Guid>(),default)).ReturnsAsync(() => 0);
        
        var handler = new DeleteCustomerHandler(mockRepository.Object);
        var deleteCustomerRequest = new DeleteCustomerRequest { Id = Uuid.Next() };
        var handleAction = () => handler.Handle(deleteCustomerRequest, default);
        await handleAction.Should().ThrowAsync<EraNotFoundException>();
    }
}
