using InstapotAPI.Dtos.Profile;
using InstapotAPI.Entity;

namespace InstapotAPI.Helpers
{
    public class MappingProfiler : AutoMapper.Profile
    {
        public MappingProfiler()
        {
            CreateMap<Entity.Profile, CreateProfileDto>();
            CreateMap<Entity.Profile, LoginStatusDto>();
        }
    }
}
