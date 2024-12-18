using FluentAssertions;
using Itmo.Csharp.Microservices.Lab1.Reactive;
using Itmo.Csharp.Microservices.Lab1.Reactive.Implementations;
using NSubstitute;
using Xunit;

namespace Itmo.Csharp.Microservices.Lab1.Tests;

public class RequestClientTests
{
    private readonly ILibraryOperationService _libraryService;
    private readonly RequestClient _requestClient;

    public RequestClientTests()
    {
        _libraryService = Substitute.For<ILibraryOperationService>();
        _requestClient = new RequestClient(_libraryService);
    }

    [Fact]
    public async Task SendAsync_ShouldReturnResult_WhenOperationCompletesSuccessfully()
    {
        using var tokenSource = new CancellationTokenSource();

        _libraryService.When(x => x.BeginOperation(
                Arg.Any<Guid>(),
                Arg.Any<RequestModel>(),
                Arg.Any<CancellationToken>()))
            .Do(info =>
            {
                Guid id = info.Arg<Guid>();
                byte[] data = info.Arg<RequestModel>().Data;
                Task.Delay(200).ContinueWith(_ => _requestClient.HandleOperationResult(id, data));
            })
            ;
        Task<ResponseModel> task =
            _requestClient.SendAsync(new RequestModel(string.Empty, new byte[] { 1, 2, 3 }), tokenSource.Token);

        ResponseModel res = await task.ConfigureAwait(true);

        res.Should().NotBeNull();
        res.Data.Should().BeEquivalentTo(new byte[] { 1, 2, 3 });
    }

    [Fact]
    public async Task SendAsync_ShouldThrowException_WhenOperationFails()
    {
        using var tokenSource = new CancellationTokenSource();

        _libraryService.When(x => x.BeginOperation(
                Arg.Any<Guid>(),
                Arg.Any<RequestModel>(),
                Arg.Any<CancellationToken>()))
            .Do(info =>
            {
                Guid id = info.Arg<Guid>();
                byte[] data = info.Arg<RequestModel>().Data;
                Task.Delay(200).ContinueWith(_ => _requestClient.HandleOperationError(id, new ArgumentException()));
            })
            ;
        Task<ResponseModel> task =
            _requestClient.SendAsync(new RequestModel(string.Empty, new byte[] { 1, 2, 3 }), tokenSource.Token);

        Func<Task> func = async () => await task.ConfigureAwait(false);
        await Assert.ThrowsAsync<ArgumentException>(func);
    }

    [Fact]
    public async Task SendAsync_ShouldThrowTaskCanceledException_WhenTokenAlreadyCanceled()
    {
        using var tokenSource = new CancellationTokenSource();

        _libraryService.When(x => x.BeginOperation(
                Arg.Any<Guid>(),
                Arg.Any<RequestModel>(),
                Arg.Any<CancellationToken>()))
            .Do(info =>
            {
                Guid id = info.Arg<Guid>();
                byte[] data = info.Arg<RequestModel>().Data;
                Task.Delay(200).ContinueWith(_ => _requestClient.HandleOperationResult(id, data));
            })
            ;
        await tokenSource.CancelAsync();

        Task<ResponseModel> task =
            _requestClient.SendAsync(new RequestModel(string.Empty, new byte[] { 1, 2, 3 }), tokenSource.Token);

        Func<Task> func = async () => await task.ConfigureAwait(false);
        await Assert.ThrowsAsync<TaskCanceledException>(func);
    }

    [Fact]
    public async Task SendAsync_ShouldThrowTaskCanceledException_WhenTokenCanceled()
    {
        using var tokenSource = new CancellationTokenSource();

        _libraryService.When(x => x.BeginOperation(
                Arg.Any<Guid>(),
                Arg.Any<RequestModel>(),
                Arg.Any<CancellationToken>()))
            .Do(info =>
            {
            })
            ;

        Task<ResponseModel> task =
            _requestClient.SendAsync(new RequestModel(string.Empty, new byte[] { 1, 2, 3 }), tokenSource.Token);

        await Task.Delay(200);
        await tokenSource.CancelAsync();

        Func<Task> func = async () => await task.ConfigureAwait(false);
        await Assert.ThrowsAsync<TaskCanceledException>(func);
    }

    [Fact]
    public async Task SendAsync_ShouldReturnResult_WhenOperationCompletesImmediately()
    {
        using var tokenSource = new CancellationTokenSource();

        _libraryService.When(x => x.BeginOperation(
                Arg.Any<Guid>(),
                Arg.Any<RequestModel>(),
                Arg.Any<CancellationToken>()))
            .Do(info =>
            {
                Guid id = info.Arg<Guid>();
                byte[] data = info.Arg<RequestModel>().Data;
                _requestClient.HandleOperationResult(id, data);
            })
            ;
        Task<ResponseModel> task =
            _requestClient.SendAsync(new RequestModel(string.Empty, new byte[] { 1, 2, 3 }), tokenSource.Token);

        ResponseModel res = await task.ConfigureAwait(true);

        res.Should().NotBeNull();
        res.Data.Should().BeEquivalentTo(new byte[] { 1, 2, 3 });
    }

    [Fact]
    public async Task SendAsync_ShouldThrowException_WhenOperationFailsImmediately()
    {
        using var tokenSource = new CancellationTokenSource();

        _libraryService.When(x => x.BeginOperation(
                Arg.Any<Guid>(),
                Arg.Any<RequestModel>(),
                Arg.Any<CancellationToken>()))
            .Do(info =>
            {
                Guid id = info.Arg<Guid>();
                byte[] data = info.Arg<RequestModel>().Data;
                _requestClient.HandleOperationError(id, new ArgumentException());
            })
            ;
        Task<ResponseModel> task =
            _requestClient.SendAsync(new RequestModel(string.Empty, new byte[] { 1, 2, 3 }), tokenSource.Token);

        Func<Task> func = async () => await task.ConfigureAwait(false);
        await Assert.ThrowsAsync<ArgumentException>(func);
    }

    [Fact]
    public async Task SendAsync_ShouldThrowTaskCanceledException_WhenOperationCanceledImmediately()
    {
        using var tokenSource = new CancellationTokenSource();

        _libraryService.When(x => x.BeginOperation(
                Arg.Any<Guid>(),
                Arg.Any<RequestModel>(),
                Arg.Any<CancellationToken>()))
            .Do(info =>
            {
                Guid id = info.Arg<Guid>();
                byte[] data = info.Arg<RequestModel>().Data;
                tokenSource.CancelAsync().ConfigureAwait(false);
            })
            ;
        Task<ResponseModel> task =
            _requestClient.SendAsync(new RequestModel(string.Empty, new byte[] { 1, 2, 3 }), tokenSource.Token);

        Func<Task> func = async () => await task.ConfigureAwait(false);
        await Assert.ThrowsAsync<TaskCanceledException>(func);
    }
}