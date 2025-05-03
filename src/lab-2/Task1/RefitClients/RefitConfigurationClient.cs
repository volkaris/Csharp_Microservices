using Itmo.Csharp.Microservices.Lab2.Task1.Entities;
using System.Runtime.CompilerServices;

namespace Itmo.Csharp.Microservices.Lab2.Task1.RefitClients;

public class RefitConfigurationClient : IConfigurationClient
{
    private readonly IRefitClient _refitClient;

    public RefitConfigurationClient(IRefitClient refitClient)
    {
        _refitClient = refitClient;
    }

    public async IAsyncEnumerable<Configurations> GetConfigurationsAsync(
        int pageSize,
        string? pageToken,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        do
        {
            Page page = await _refitClient.GetConfigurationsAsync(pageSize, pageToken);

            pageToken = page.PageToken;

            foreach (Configurations config in page.Configurations)
            {
                yield return config;
            }
        }
        while (pageToken != null);
    }
}