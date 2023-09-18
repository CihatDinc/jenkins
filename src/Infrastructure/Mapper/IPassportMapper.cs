namespace Infrastructure.Mapper;

using AutoMapper;
using Nebim.Era.Plt.Comm.Customer.Application.Contracts;
using Nebim.Era.Plt.Comm.Customer.Domain;
using Nebim.Era.Plt.Core;

public class PassportMapperProfile : Profile
{
    public PassportMapperProfile()
    {
        CreateMap<Passport, PassportResponse>().ReverseMap();
        CreateMap<UpdatePassportRequest, Passport>().ReverseMap();
        CreateMap<CreatePassportRequest, Passport>()
            .ForMember(dest => dest.Id, opts => opts.MapFrom(t => Uuid.Next()));
    }
}

public interface IPassportMapper
{
    Passport ToPassport(CreatePassportRequest request);
    Passport ToPassport(Passport passport, UpdatePassportRequest request);
    PassportResponse ToPassportResponse(Passport passport);
}

public class PassportMapper : IPassportMapper
{
    private readonly IMapper _mapper;

    public PassportMapper(IMapper mapper)
    {
        _mapper = mapper;
    }

    public Passport ToPassport(CreatePassportRequest request)
    {
        var dest = _mapper.Map<Passport>(request);
        return dest;
    }

    public Passport ToPassport(Passport passport, UpdatePassportRequest request)
    {
        var dest = _mapper.Map(request, passport);
        return dest;
    }

    public PassportResponse ToPassportResponse(Passport passport)
    {
        var dest = _mapper.Map<PassportResponse>(passport);
        return dest;
    }
}
