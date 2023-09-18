using AutoMapper;
using Infrastructure.Mapper;

namespace TestUtils;

public static class PassportMapperFactory
{
    public static IPassportMapper Create()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<PassportMapperProfile>());
        return new PassportMapper(config.CreateMapper());
    }
}
