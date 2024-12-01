namespace SleepingBear.Monad.Core.Test;

/// <summary>
///     Tests for <see cref="TaskExtensions" />.
/// </summary>
internal static class TaskExtensionsTests
{
    [Test]
    public static Task BindAsync_Invalid_ThrowsInvalidOperationException()
    {
        _ = Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            _ = await new Result<int>()
                .ToTask()
                .BindAsync(ok => ok.ToOk().ToTask())
                .ConfigureAwait(false);
        });
        return Task.CompletedTask;
    }

    [Test]
    public static Task BindFailureAsync_Invalid_ThrowsInvalidOperationException()
    {
        _ = Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            _ = await new Result<int>()
                .ToTask()
                .BindFailureAsync(error => error.ToFailure<int>().ToTask())
                .ConfigureAwait(false);
        });
        return Task.CompletedTask;
    }

    [Test]
    public static Task MapAsync_Invalid_ThrowsInvalidOperationException()
    {
        _ = Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            _ = await new Result<int>()
                .ToTask()
                .MapAsync(ok => ok.ToTask())
                .ConfigureAwait(false);
        });
        return Task.CompletedTask;
    }

    [Test]
    public static Task MapFailureAsync_Invalid_ThrowsInvalidOperationException()
    {
        _ = Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            _ = await new Result<int>()
                .ToTask()
                .MapFailureAsync(error => error.ToTask())
                .ConfigureAwait(false);
        });
        return Task.CompletedTask;
    }

    [Test]
    public static Task MatchAsync_Invalid_ThrowsInvalidOperationException()
    {
        _ = Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            _ = await new Result<int>()
                .ToTask()
                .MatchAsync(_ => 0.ToTask(), _ => 1.ToTask())
                .ConfigureAwait(false);
        });
        return Task.CompletedTask;
    }
}