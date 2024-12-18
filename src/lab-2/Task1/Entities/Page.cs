using System.Text.Json.Serialization;

namespace Itmo.Csharp.Microservices.Lab2.Task1.Entities;

public record Page(
    [property: JsonPropertyName("items")] IEnumerable<Configurations> Configurations,
    [property: JsonPropertyName("pageToken")]
    string? PageToken);