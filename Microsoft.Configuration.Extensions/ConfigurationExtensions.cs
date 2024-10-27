using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Configuration.Extensions;

public static class ConfigurationExtensions
{
    public static IServiceCollection AddConfiguration<T>(
        this IServiceCollection services,
        IConfiguration configuration,
        string? name = null
    )
        where T : class => services.Configure<T>(configuration.GetSection(name ?? typeof(T).Name));

    public static T? GetSectionAs<T>(this IConfiguration configuration, string? sectionName = null) =>
        configuration.GetSection(sectionName ?? typeof(T).Name).Get<T>();

    public static object? GetSectionByType(this IConfiguration configuration, Type type, string sectionName) =>
        configuration.GetSection(sectionName).Get(type);
}
