namespace TestUtils;

using Bogus;
using Nebim.Era.Plt.Comm.Customer.Application.Contracts;
using Nebim.Era.Plt.Comm.Customer.Domain;

public static class PassportFakerFactory
{
    public static Faker<Passport> Passport()
    {
        return new Faker<Passport>()
            .CustomInstantiator(f => new Passport(
                f.Random.Guid(),
                f.Random.Guid(),
                f.Random.AlphaNumeric(10),
                f.Random.Int(0, 999),
                f.Random.Int(0, 999),
                f.Date.Past()));
    }

    public static Faker<CreatePassportRequest> CreatePassportRequest()
    {
        return new Faker<CreatePassportRequest>()
            .CustomInstantiator(f => new CreatePassportRequest
            {
                CustomerId = f.Random.Guid(),
                Number = f.Random.AlphaNumeric(10),
                IssuingStateCode = f.Random.Int(0, 999),
                Nationality = f.Random.Int(0, 999),
                IssueDate = f.Date.Past(),
            });
    }

    public static Faker<UpdatePassportRequest> UpdatePassportRequest()
    {
        return new Faker<UpdatePassportRequest>()
            .CustomInstantiator(f => new UpdatePassportRequest
            {
                Id = f.Random.Guid(),
                CustomerId = f.Random.Guid(),
                Number = f.Random.AlphaNumeric(10),
                IssuingStateCode = f.Random.Int(0, 999),
                Nationality = f.Random.Int(0, 999),
                IssueDate = f.Date.Past()
            });
    }
}
