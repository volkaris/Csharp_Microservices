using Itmo.Csharp.Microservices.Lab2.Task1.Entities;
using Microsoft.Extensions.Options;
using System.Collections.Specialized;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Web;

namespace Itmo.Csharp.Microservices.Lab2.Task1.ManualClients;

public class ManualConfigurationClient : IConfigurationClient
{
    private readonly HttpClient _client;
    private readonly IOptions<OuterServiceOptions> _options;

    public ManualConfigurationClient(IHttpClientFactory clientFactory, IOptions<OuterServiceOptions> options)
    {
        _options = options;
        _client = clientFactory.CreateClient();
    }

    public async IAsyncEnumerable<Configurations> GetConfigurationsAsync(
        int pageSize,
        string? pageToken,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var uriBuilder = new UriBuilder($"http://{_options.Value.Host}:{_options.Value.Port}/configurations");

        NameValueCollection query = HttpUtility.ParseQueryString(uriBuilder.Query);

        query["pageSize"] = pageSize.ToString();

        if (!string.IsNullOrEmpty(pageToken))
        {
            query["pageToken"] = pageToken;
        }

        do
        {
            uriBuilder.Query = query.ToString();

            string uri = uriBuilder.ToString();

            HttpResponseMessage response = await _client.GetAsync(uri, cancellationToken).ConfigureAwait(false);

            Page page = await response.Content.ReadFromJsonAsync<Page>(cancellationToken) ?? throw new ArgumentNullException(nameof(pageToken));

            pageToken = page.PageToken;

            query["pageToken"] = pageToken;

            uriBuilder.Query = query.ToString();

            foreach (Configurations config in page.Configurations)
            {
                yield return config;
            }
        }
        while (pageToken != null);
    }
}