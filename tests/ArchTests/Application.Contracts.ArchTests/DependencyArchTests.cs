using FluentAssertions;
using Nebim.Era.Plt.Comm.Customer.Application.Contracts;
using NetArchTest.Rules;

namespace Application.Contracts.ArchTests;

public class DependencyArchTests
{
    [Fact]
    public void ApplicationContracts_Should_not_HaveDependencyOn_Service()
    {
        Types.InAssembly(typeof(CreateCustomerRequest).Assembly)
            .That()
            .ResideInNamespace("Application.Contracts")
            .ShouldNot()
            .HaveDependencyOn("Service")
            .GetResult()
            .IsSuccessful.Should().BeTrue();
    }
}