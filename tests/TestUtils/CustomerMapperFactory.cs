using AutoMapper;
using Infrastructure.Mapper;

namespace TestUtils;

public static class CustomerMapperFactory
{
    public static ICustomerMapper Create()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<CustomerMapperProfile>());
        return new CustomerMapper(config.CreateMapper());
    }
}