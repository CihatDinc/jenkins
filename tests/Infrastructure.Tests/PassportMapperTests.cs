using FluentAssertions;
using TestUtils;

namespace Infrastructure.Tests;

public class PassportMapperTests
{
    [Fact]
    public void MapsBetween_Passport_and_PassportResponse()
    {
        var mapper = PassportMapperFactory.Create();
        var passport = PassportFakerFactory.Passport().Generate();

        var passportResponse = mapper.ToPassportResponse(passport);

        passportResponse.Should().BeEquivalentTo(passport, options => options.ExcludingMissingMembers());
    }

    [Fact]
    public void MapsBetween_CreatePassportRequest_and_Passport()
    {
        var mapper = PassportMapperFactory.Create();
        var createPassportRequest = PassportFakerFactory.CreatePassportRequest().Generate();

        var passport = mapper.ToPassport(createPassportRequest);

        passport.Id.Should().NotBeEmpty();
        passport.Should().BeEquivalentTo(createPassportRequest, options => options.ExcludingMissingMembers());
    }

    [Fact]
    public void MapsBetween_UpdatePassportRequest_and_Passport()
    {
        var mapper = PassportMapperFactory.Create();
        var passport = PassportFakerFactory.Passport().Generate();
        var updatePassportRequest = PassportFakerFactory.UpdatePassportRequest();

        var updatedPassport = mapper.ToPassport(passport, updatePassportRequest);

        updatedPassport.Should().BeEquivalentTo(updatePassportRequest, options => options.ExcludingMissingMembers());
    }
}
