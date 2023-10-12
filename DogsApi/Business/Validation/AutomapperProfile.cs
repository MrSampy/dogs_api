using AutoMapper;

namespace Business.Validation
{
    public class AutomapperProfile: Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Data.Entities.Dog, Models.DogModel>().ReverseMap();
        }
    }
}
