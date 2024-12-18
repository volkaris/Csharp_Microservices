using Itmo.Csharp.Microservices.Lab2.Task1.Entities;
using Microsoft.Extensions.Configuration;

namespace Itmo.Csharp.Microservices.Lab2.Task2.Providers;

public class CustomConfigurationProvider : ConfigurationProvider
{
    public void UpdateConfigurations(IEnumerable<Configurations> configurations)
    {
        var newData = configurations.ToDictionary(item => item.Key, item => item.Value);

        bool changed = false;

        var keysToRemove = Data.Keys.Except(newData.Keys).ToList();

        foreach (string key in keysToRemove)
        {
            Data.Remove(key);
            changed = true;
        }

        foreach (KeyValuePair<string, string> kvp in newData)
        {
            if (Data.TryGetValue(kvp.Key, out string? existingValue) && existingValue == kvp.Value)
            {
                continue;
            }

            Data[kvp.Key] = kvp.Value;

            changed = true;
        }

        if (changed)
        {
            OnReload();
        }
    }
}