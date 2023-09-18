namespace Application.Tests;

public class UpdateCustomerAddressOfCustomerHandlerTests
{
   // [Fact]
   //  public async Task Calls_Get_WithCustomerId()
   //  {
   //      // ARRANGE
   //      var request = CustomerFakeFactory.NewUpdateCustomerAddressOfCustomerRequest();
   //      var fakeCustomer = CustomerFakeFactory.Customers.Single(o => o.Id == request.CustomerId);
   //
   //      var mockRepository = new Mock<ICustomerRepository>();
   //      mockRepository.Setup(repo => repo.Get(It.IsAny<Guid>(),default)).ReturnsAsync(fakeCustomer);
   //
   //      var mapper = CustomerMapperFactory.Create();
   //      // ACT
   //      var handler = new UpdateCustomerAddressOfCustomerHandler(mapper,mockRepository.Object);
   //      var result = await handler.Handle(request, default);
   //
   //      // ASSERT
   //      result.Id.Should().Be(request.CustomerId);
   //      mockRepository.Verify(t => t.Get(request.CustomerId, default), Times.Once);
   //
   //
   //  }
   //
   //  [Fact]
   //  public async Task Throws_EraNotFoundException_WhenCustomerWithGivenIdIsNotFound()
   //  {
   //      var customerAddress = CustomerFakeFactory.CustomerAddress[new Random().Next(0, 4)];
   //      var fakeCustomer = CustomerFakeFactory.Customers.Single(o => o.Id == customerAddress.CustomerId);
   //      // ARRANGE
   //      var request = CustomerFakeFactory.NewUpdateCustomerAddressOfCustomerRequest();
   //
   //      var mapper = CustomerMapperFactory.Create();
   //      var mockRepository = new Mock<ICustomerRepository>();
   //      mockRepository.Setup(repo => repo.Get(It.IsAny<Guid>(),default)).ReturnsAsync(fakeCustomer);
   //      mockRepository.Setup(repo => repo.Update(It.IsAny<Customer>(), default)).ReturnsAsync(() => 1);
   //
   //
   //      // ACT
   //      var handler = new UpdateCustomerAddressOfCustomerHandler(mapper,mockRepository.Object);
   //      var result = () => handler.Handle(request, default);
   //
   //      // ASSERT
   //      await result.Should()
   //          .ThrowAsync<EraNotFoundException>()
   //          .WithMessage(ExceptionMessages.CouldNotFindCustomerById(request.CustomerId));
   //  }
   //
   //  [Fact]
   //  public async Task Calls_Update_WithUpdatedCustomer()
   //  {
   //      var customerAddress = CustomerFakeFactory.CustomerAddress[new Random().Next(0, 4)];
   //      var fakeCustomer = CustomerFakeFactory.Customers.Single(o => o.Id == customerAddress.CustomerId);
   //
   //      // ARRANGE
   //      var request = CustomerFakeFactory.NewUpdateCustomerAddressOfCustomerRequest();
   //
   //      var mapper = CustomerMapperFactory.Create();
   //      var mockRepository = new Mock<ICustomerRepository>();
   //      mockRepository.Setup(repo => repo.Get(It.IsAny<Guid>(),default)).ReturnsAsync(fakeCustomer);
   //      mockRepository.Setup(repo => repo.Update(It.IsAny<Customer>(), default)).ReturnsAsync(() => 1);
   //
   //      // ACT
   //      var handler = new UpdateCustomerAddressOfCustomerHandler(mapper,mockRepository.Object);
   //      var result = await handler.Handle(request, default);
   //
   //      // ASSERT
   //      result.Should().Be(1);        
   //      mockRepository.Verify(t => t.Update(It.Is<Customer>(sc => sc.Id == request.CustomerId), default), Times.Once);
   //  }
}