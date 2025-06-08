using AutoMapper;
using CashFlow.Application.AutoMapper;

namespace CommomTestUtiilities.Mapper;

public class MapperBuilder
{
    public static IMapper Build()
    {
        var mapper = new MapperConfiguration(config =>
        {
            config.AddProfile(new AutoMapperConfig());
        });

        return mapper.CreateMapper();
    }
}