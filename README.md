# Microsoft.Configuration.Extensions

This package allows you to have in `appsettings.json`:
```json
{
  "RealConfiguration": {
    "Size": 1,
    "Interval": "00:05:25",
    "Token": "API-SECRET"
  }
}
```

And RealConfiguration.cs file:
```csharp
public sealed class RealConfiguration
{
    public int Size { get; set; }
    public TimeSpan Interval { get; set; }
    public string Token { get; set; }
}
```

The code below allows to bind easily settings to configuration model:
```csharp
public static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration configuration)
{
    services.AddConfiguration<RealConfiguration>(configuration);
}
```

Or if you want to override name of section:
```csharp
public static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration configuration)
{
    services.AddConfiguration<RealConfiguration>(configuration, "AnotherRealConfigurationName");
}
```

In that case you should use this `appsettings.json`:
```json
{
  "AnotherRealConfigurationName": {
    "Size": 1,
    "Interval": "00:05:25",
    "Token": "API-SECRET"
  }
}
```
