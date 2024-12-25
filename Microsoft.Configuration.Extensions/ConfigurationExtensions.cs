using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Configuration.Extensions;

public static class ConfigurationExtensions
{
    private static readonly MethodInfo AddConfigurationGenericMethod = typeof(ConfigurationExtensions).GetMethod(
        "AddConfiguration",
        BindingFlags.Static | BindingFlags.Public,
        null,
        [typeof(IServiceCollection), typeof(IConfiguration), typeof(string)],
        null
    )!;

    public static IServiceCollection AddConfiguration<T>(
        this IServiceCollection services,
        IConfiguration configuration,
        string? name = null
    )
        where T : class => services.Configure<T>(configuration.GetSection(name ?? typeof(T).Name));

    public static IServiceCollection AddConfiguration(
        this IServiceCollection services,
        Type type,
        IConfiguration configuration,
        string? name = null
    )
    {
        var method = AddConfigurationGenericMethod.MakeGenericMethod(type);
        return (IServiceCollection)method.Invoke(null, [services, configuration, name]);
    }

    public static T? GetSectionAs<T>(this IConfiguration configuration, string? sectionName = null) =>
        configuration.GetSection(sectionName ?? typeof(T).Name).Get<T>();

    public static object? GetSectionByType(this IConfiguration configuration, Type type, string? sectionName = null) =>
        configuration.GetSection(sectionName ?? type.Name).Get(type);
}
