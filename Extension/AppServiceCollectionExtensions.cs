using System.Reflection;

namespace BaseAuth.Extension;

public static class AppServiceCollectionExtensions
{
    public static IServiceCollection AddAppService(this IServiceCollection services)
    {
        // Register services annotated with ScopedServiceAttribute
        var serviceTypes = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t.GetCustomAttribute<ScopedServiceAttribute>() != null);

        foreach (var serviceType in serviceTypes)
        {
            var interfaceTypes = serviceType.GetInterfaces().Where(i => i.GetCustomAttribute<ScopedServiceAttribute>() != null ).ToList();
            foreach (var interfaceType in interfaceTypes)
            {
                services.AddScoped(interfaceType, serviceType);
            }
        }

        return services;
    }
}