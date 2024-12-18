namespace Itmo.Csharp.Microservices.Lab1.Reactive;

public sealed record RequestModel(string Method, byte[] Data);