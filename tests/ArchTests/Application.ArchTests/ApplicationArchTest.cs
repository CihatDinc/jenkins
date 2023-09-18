using System.Reflection;
using Application;
using FluentAssertions;
using NetArchTest.Rules;

namespace Application.ArchTests;

public class ApplicationArchTest
{
    [Fact]
    public void ApplicationDll_Should_not_HaveDependencyOn_Service()
    {
        Types.InAssembly(typeof(CreateCustomerHandler).Assembly)
            .That()
            .ResideInNamespace("Application")
            .ShouldNot()
            .HaveDependencyOn("Service")
            .GetResult()
            .IsSuccessful.Should().BeTrue();
    }
    
}