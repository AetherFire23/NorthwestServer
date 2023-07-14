using AutoMapper;

namespace WebAPI.AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<SourceMappa, DestMappa>();
    }
}
