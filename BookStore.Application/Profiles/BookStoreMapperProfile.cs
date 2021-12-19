using System.Reflection;
using AutoMapper;

namespace BookStore.Application.Profiles;

public class BookStoreMapperProfile : Profile
{
    public BookStoreMapperProfile()
    {
        ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
    }

    private void ApplyMappingsFromAssembly(Assembly assembly)
    {
        var types = assembly.GetExportedTypes()
            .Where(t =>
                t.GetInterfaces()
                    .Any(i => i.IsGenericType &&
                              i.GetGenericTypeDefinition() == typeof(IMapFrom<>)));

        foreach (var type in types)
        {
            var instance = Activator.CreateInstance(type);
            var methodInfo = type.GetMethod("Mapping") ??
                             instance!.GetType()
                                 .GetInterface("IMapFrom`1")?
                                 .GetMethod("Mapping");

            methodInfo!.Invoke(instance, new object[] {this});
        }
    }
}