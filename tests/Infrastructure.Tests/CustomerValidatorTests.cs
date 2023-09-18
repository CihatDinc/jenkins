using Application;
using FluentValidation.TestHelper;
using Nebim.Era.Plt.Comm.Customer.Application.Contracts;

namespace Infrastructure.Tests;

public class CustomerValidatorTests
{
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Name_ShoulHaveError_WhenEmpty(string name)
    {
        var validator = new CustomerValidator();
        var result = validator.TestValidate(new CustomerRequest()
        {
            Name = name
        });

        result.ShouldHaveValidationErrorFor(t => t.Name);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Surname_ShoulHaveError_WhenEmpty(string surname)
    {
        var validator = new CustomerValidator();
        var result = validator.TestValidate(new CustomerRequest()
        {
            Surname = surname
        });

        result.ShouldHaveValidationErrorFor(t => t.Name);
    }

    [Fact]
    public void BirthDate_ShouldHaveError_WhenFutureDateIsPassed()
    {
        var validator = new CustomerValidator();
        var result = validator.TestValidate(new CustomerRequest()
        {
            BirthDate = DateTimeOffset.Now.AddYears(1)
        });

        result.ShouldHaveValidationErrorFor(t => t.BirthDate);
    }

    [Fact]
    public void BirthDate_ShouldNotHaveError_WhenBirthDateIsNull()
    {
        var validator = new CustomerValidator();
        var result = validator.TestValidate(new CustomerRequest()
        {
            BirthDate = null!
        });

        result.ShouldNotHaveValidationErrorFor(t => t.BirthDate);
    }

    [Fact]
    public void BirthDate_ShouldNotHaveError_WhenBirthDateIsValid()
    {
        var validator = new CustomerValidator();
        var result = validator.TestValidate(new CustomerRequest()
        {
            BirthDate = DateTimeOffset.Now.AddDays(-1)
        });

        result.ShouldNotHaveValidationErrorFor(t => t.BirthDate);
    }
}