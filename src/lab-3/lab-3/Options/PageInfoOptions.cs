namespace Itmo.Csharp.Microservices.Lab3.Options;

public class PageInfoOptions
{
    public int PageSize { get; set; }

    public string? PageToken { get; set; } = null;

    public int UpdateTime { get; set; }
}