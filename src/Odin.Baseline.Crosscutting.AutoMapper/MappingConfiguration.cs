using AutoMapper;
using Odin.Baseline.Crosscutting.AutoMapper.Profiles;

namespace Odin.Baseline.Crosscutting.AutoMapper
{
    public class MappingConfiguration
    {
        public static IMapper CreateMapper()
        {
            var mapperConfig = new MapperConfiguration(x =>
            {
                x.AddProfile<CustomersProfile>();
            });

            return mapperConfig.CreateMapper();
        }
    }
}
