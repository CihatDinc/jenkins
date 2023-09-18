namespace Application.Tests;

public class AddCustomerAddressToCustomerHandlerTests
{
    // [Fact]
    // public async Task Throws_EraNotFoundException_WhenCustomerWithGivenIdIsNotFound()
    // {
    //     // ARRANGE
    //     var request = CustomerFakeFactory.NewAddCustomerAddressToCustomerRequest();
    //     var fakeCustomer = CustomerFakeFactory.Customers.Single(o => o.Id == request.CustomerId);
    //
    //     var mockRepository = new Mock<ICustomerRepository>();
    //     mockRepository.Setup(repo => repo.Get(It.IsAny<Guid>(),default)).ReturnsAsync(fakeCustomer);
    //
    //     var mapper = CustomerMapperFactory.Create();
    //
    //     // ACT
    //     var handler = new AddCustomerAddressToCustomerHandler(mapper, mockRepository.Object);
    //     var result = () => handler.Handle(request, default);
    //
    //     // ASSERT
    //     await result.Should()
    //         .ThrowAsync<EraNotFoundException>()
    //         .WithMessage(ExceptionMessages.CouldNotFindCustomerById(request.CustomerId));
    // }
    //
    // [Fact]
    // public async Task Calls_Update_WithUpdatedCustomer()
    // {
    //     var request = CustomerFakeFactory.NewAddCustomerAddressToCustomerRequest();
    //
    //     var fakeCustomer = CustomerFakeFactory.Customers.Single(o => o.Id == request.CustomerId);
    //     var mockRepository = new Mock<ICustomerRepository>();
    //     mockRepository.Setup(repo => repo.Get(It.IsAny<Guid>(),default)).ReturnsAsync(fakeCustomer);
    //     mockRepository.Setup(repo => repo.Update(It.IsAny<Customer>(), default)).ReturnsAsync(() => 1);
    //
    //     var mapper = new Mock<ICustomerMapper>();
    //     mapper
    //         .Setup(t => t.ToCustomerResponse(It.IsAny<Customer>()))
    //         .Returns(new CustomerResponse());
    //
    //     // ACT
    //     var handler = new AddCustomerAddressToCustomerHandler(mapper.Object, mockRepository.Object);
    //     await handler.Handle(request, default);
    //
    //     // ASSERT
    //     mapper.Verify(t => t.ToCustomerResponse(It.IsAny<Customer>()));
    // }
}