using System.Text.Json.Serialization;

namespace Itmo.Csharp.Microservices.Lab2.Task1.Entities;

public record Configurations(
    [property: JsonPropertyName("key")] string Key,
    [property: JsonPropertyName("value")] string Value);