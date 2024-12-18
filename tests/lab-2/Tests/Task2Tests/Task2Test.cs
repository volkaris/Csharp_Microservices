using Itmo.Csharp.Microservices.Lab2.Task1.Entities;
using Itmo.Csharp.Microservices.Lab2.Task2.Providers;
using Microsoft.Extensions.Primitives;
using Xunit;

namespace Itmo.Csharp.Microservices.Lab2.Tests.Task2Tests;

public class Task2Test
{
    [Fact]
    public void UpdateConfiguration_WhenProviderIsEmpty_ShouldAddItemAndReload()
    {
        var provider = new CustomConfigurationProvider();

        var configurations =
            new List<Configurations> { new("TestKey", "TestValue") };

        IChangeToken token = provider.GetReloadToken();

        provider.UpdateConfigurations(configurations);

        provider.TryGet("TestKey", out string? value);

        Assert.False(string.IsNullOrEmpty(value));

        Assert.Equal("TestValue", value);

        Assert.True(token.HasChanged);
    }

    [Fact]
    public void UpdateConfiguration_WhenSameItemProvided_ShouldNotReload()
    {
        var provider = new CustomConfigurationProvider();
        var initialConfigurations = new List<Configurations> { new("TestKey", "TestValue") };

        var sameConfigs = new List<Configurations> { new("TestKey", "TestValue") };

        provider.UpdateConfigurations(initialConfigurations);

        IChangeToken token = provider.GetReloadToken();

        provider.UpdateConfigurations(sameConfigs);

        provider.TryGet("TestKey", out string? value);

        Assert.Equal("TestValue", value);

        Assert.False(token.HasChanged);
    }

    [Fact]
    public void UpdateConfiguration_WhenValueChanged_ShouldUpdateValueAndReload()
    {
        var provider = new CustomConfigurationProvider();

        var initialConfigurations = new List<Configurations> { new("TestKey", "OldValue") };

        var configurations = new List<Configurations> { new("TestKey", "NewValue") };

        provider.UpdateConfigurations(initialConfigurations);

        IChangeToken token = provider.GetReloadToken();

        provider.UpdateConfigurations(configurations);

        provider.TryGet("TestKey", out string? value);

        Assert.Equal("NewValue", value);

        Assert.True(token.HasChanged);
    }

    [Fact]
    public void UpdateConfiguration_WhenEmptyCollectionProvided_ShouldClearDataAndReload()
    {
        var provider = new CustomConfigurationProvider();
        var initialConfigurations = new List<Configurations> { new("TestKey", "TestValue") };
        var configurations = new List<Configurations>();

        provider.UpdateConfigurations(initialConfigurations);

        IChangeToken token = provider.GetReloadToken();

        provider.UpdateConfigurations(configurations);

        Assert.False(provider.TryGet("TestKey", out string? value));

        Assert.True(token.HasChanged);
    }
}