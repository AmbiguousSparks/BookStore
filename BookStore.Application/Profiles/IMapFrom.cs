using AutoMapper;

namespace BookStore.Application.Profiles;

public interface IMapFrom<T>
{
    void Mapping(Profile profile)
    {
        profile.CreateMap(GetType(), typeof(T));
        profile.CreateMap(typeof(T), GetType());
    }
}