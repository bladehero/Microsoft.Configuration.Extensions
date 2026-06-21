using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Bladehero.Configuration.Extensions.Tests;

public sealed class ConfigurationExtensionsTests
{
    private const string SectionName = "SomeConfiguration";
    private const string FirstNestedSectionName = "FirstNestedItem";
    private const string SecondNestedSectionName = "SecondNestedItem";

    private static readonly Dictionary<string, string?> InMemorySettings =
        new()
        {
            { $"{SectionName}:{FirstNestedSectionName}:Field1", "SomeString" },
            { $"{SectionName}:{FirstNestedSectionName}:Field2", "2" },
            { $"{SectionName}:{SecondNestedSectionName}:Field", "true" },
        };

    private static readonly IConfiguration Configuration = new ConfigurationBuilder()
        .AddInMemoryCollection(InMemorySettings)
        .Build();

    private static readonly object FirstNestedItem = new { Field1 = "SomeString", Field2 = 2 };

    private static readonly object ConfigurationShape = new
    {
        FirstNestedItem,
        SecondNestedItem = new { Field = true },
    };

    private readonly ServiceCollection _serviceCollection = [];

    [Fact]
    public void AddConfiguration_WhenTypeFromGenericArgument_ShouldProperlyAddConfiguration()
    {
        // Act
        var actual = _serviceCollection
            .AddConfiguration<SomeConfiguration>(Configuration)
            .BuildServiceProvider()
            .GetRequiredService<IOptions<SomeConfiguration>>()
            .Value;

        // Assert
        actual.Should().BeEquivalentTo(ConfigurationShape);
    }

    [Fact]
    public void AddConfiguration_WhenTypeFromGenericArgumentWithCustomSectionName_ShouldProperlyAddConfiguration()
    {
        // Act
        var actual = _serviceCollection
            .AddConfiguration<SomeConfiguration>(Configuration, SectionName)
            .BuildServiceProvider()
            .GetRequiredService<IOptions<SomeConfiguration>>()
            .Value;

        // Assert
        actual.Should().BeEquivalentTo(ConfigurationShape);
    }

    [Fact]
    public void AddConfiguration_WhenTypeIsMethodArgument_ShouldProperlyAddConfiguration()
    {
        // Act
        var actual = _serviceCollection
            .AddConfiguration(typeof(SomeConfiguration), Configuration)
            .BuildServiceProvider()
            .GetRequiredService<IOptions<SomeConfiguration>>()
            .Value;

        // Assert
        actual.Should().BeEquivalentTo(ConfigurationShape);
    }

    [Fact]
    public void AddConfiguration_WhenTypeIsMethodArgumentWithCustomSectionName_ShouldProperlyAddConfiguration()
    {
        // Act
        var actual = _serviceCollection
            .AddConfiguration(typeof(SomeConfiguration), Configuration, SectionName)
            .BuildServiceProvider()
            .GetRequiredService<IOptions<SomeConfiguration>>()
            .Value;

        // Assert
        actual.Should().BeEquivalentTo(ConfigurationShape);
    }

    [Fact]
    public void AddConfiguration_WhenHostBuilderWithTypeFromGenericArgument_ShouldProperlyAddConfiguration()
    {
        // Arrange
        var builder = CreateHostApplicationBuilder();

        // Act
        var actual = builder
            .AddConfiguration<SomeConfiguration>()
            .Services.BuildServiceProvider()
            .GetRequiredService<IOptions<SomeConfiguration>>()
            .Value;

        // Assert
        actual.Should().BeEquivalentTo(ConfigurationShape);
    }

    [Fact]
    public void AddConfiguration_WhenHostBuilderWithTypeFromGenericArgumentWithCustomSectionName_ShouldProperlyAddConfiguration()
    {
        // Arrange
        var builder = CreateHostApplicationBuilder();

        // Act
        var actual = builder
            .AddConfiguration<SomeConfiguration>(SectionName)
            .Services.BuildServiceProvider()
            .GetRequiredService<IOptions<SomeConfiguration>>()
            .Value;

        // Assert
        actual.Should().BeEquivalentTo(ConfigurationShape);
    }

    [Fact]
    public void AddConfiguration_WhenHostBuilderWithTypeIsMethodArgument_ShouldProperlyAddConfiguration()
    {
        // Arrange
        var builder = CreateHostApplicationBuilder();

        // Act
        var actual = builder
            .AddConfiguration(typeof(SomeConfiguration))
            .Services.BuildServiceProvider()
            .GetRequiredService<IOptions<SomeConfiguration>>()
            .Value;

        // Assert
        actual.Should().BeEquivalentTo(ConfigurationShape);
    }

    [Fact]
    public void AddConfiguration_WhenHostBuilderWithTypeIsMethodArgumentWithCustomSectionName_ShouldProperlyAddConfiguration()
    {
        // Arrange
        var builder = CreateHostApplicationBuilder();

        // Act
        var actual = builder
            .AddConfiguration(typeof(SomeConfiguration), SectionName)
            .Services.BuildServiceProvider()
            .GetRequiredService<IOptions<SomeConfiguration>>()
            .Value;

        // Assert
        actual.Should().BeEquivalentTo(ConfigurationShape);
    }

    [Fact]
    public void AddConfiguration_WhenHostBuilderWithTypeFromGenericArgument_ShouldReturnSameBuilder()
    {
        // Arrange
        var builder = CreateHostApplicationBuilder();

        // Act
        var actual = builder.AddConfiguration<SomeConfiguration>();

        // Assert
        actual.Should().BeSameAs(builder);
    }

    [Fact]
    public void AddConfiguration_WhenHostBuilderWithTypeIsMethodArgument_ShouldReturnSameBuilder()
    {
        // Arrange
        var builder = CreateHostApplicationBuilder();

        // Act
        var actual = builder.AddConfiguration(typeof(SomeConfiguration));

        // Assert
        actual.Should().BeSameAs(builder);
    }

    [Fact]
    public void GetSectionAs_WhenNoSectionNameProvided_ShouldReturnByTypeName()
    {
        // Act
        var actual = Configuration.GetSectionAs<SomeConfiguration>();

        // Assert
        actual.Should().BeEquivalentTo(ConfigurationShape);
    }

    [Fact]
    public void GetSectionAs_WhenSectionNameIsProvided_ShouldReturnBySectionName()
    {
        // Act
        var actual = Configuration.GetSectionAs<SomeConfiguration.FirstNestedItemConfiguration>(
            $"{SectionName}:{FirstNestedSectionName}"
        );

        // Assert
        actual.Should().BeEquivalentTo(FirstNestedItem);
    }

    [Fact]
    public void GetSectionByType_WhenNoSectionNameProvided_ShouldReturnByTypeName()
    {
        // Act
        var actual = Configuration.GetSectionByType(typeof(SomeConfiguration));

        // Assert
        actual.Should().BeEquivalentTo(ConfigurationShape);
    }

    [Fact]
    public void GetSectionByType_WhenSectionNameIsProvided_ShouldReturnBySectionName()
    {
        // Act
        var actual = Configuration.GetSectionByType(
            typeof(SomeConfiguration.FirstNestedItemConfiguration),
            $"{SectionName}:{FirstNestedSectionName}"
        );

        // Assert
        actual.Should().BeEquivalentTo(FirstNestedItem);
    }

    private static IHostApplicationBuilder CreateHostApplicationBuilder()
    {
        var builder = Host.CreateEmptyApplicationBuilder(settings: null);
        builder.Configuration.AddInMemoryCollection(InMemorySettings);
        return builder;
    }

    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private class SomeConfiguration
    {
        public FirstNestedItemConfiguration? FirstNestedItem { get; set; }

        public SecondNestedItemConfiguration? SecondNestedItem { get; set; }

        public class FirstNestedItemConfiguration
        {
            public string? Field1 { get; set; }
            public int Field2 { get; set; }
        }

        public class SecondNestedItemConfiguration
        {
            public bool Field { get; set; }
        }
    }
}
