namespace Itmo.Csharp.Microservices.Lab3.Extensions;

public static class ConfigurationManagerExtensions
{
    public static ConfigurationManager AddOuterServiceProvider(this ConfigurationManager manager)
    {
        manager.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("file.json", false, true)
            .Build();

        return manager;
    }

    public static ConfigurationManager AddPageInfoProvider(this ConfigurationManager manager)
    {
        manager.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("pageInfo.json", false, true)
            .Build();

        return manager;
    }
}