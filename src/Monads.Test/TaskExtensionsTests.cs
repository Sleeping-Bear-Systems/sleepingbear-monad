namespace SleepingBear.Monad.Monads.Test;

/// <summary>
///     Tests for <see cref="TaskExtensions" />.
/// </summary>
internal static class TaskExtensionsTests
{
    [Test]
    public static Task BindAsync_ResultInvalid_ThrowsInvalidOperationException()
    {
        _ = Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            _ = await new Result<int>()
                .ToTask()
                .BindAsync(ok => ok.ToResult().ToTask())
                .ConfigureAwait(false);
        });
        return Task.CompletedTask;
    }

    [Test]
    public static Task BindErrorAsync_ResultInvalid_ThrowsInvalidOperationException()
    {
        _ = Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            _ = await new Result<int>()
                .ToTask()
                .BindErrorAsync(error => error.ToResult<int>().ToTask())
                .ConfigureAwait(false);
        });
        return Task.CompletedTask;
    }

    [Test]
    public static Task MapAsync_ResultInvalid_ThrowsInvalidOperationException()
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
    public static Task MapErrorAsync_ResultInvalid_ThrowsInvalidOperationException()
    {
        _ = Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            _ = await new Result<int>()
                .ToTask()
                .MapErrorAsync(error => error.ToTask())
                .ConfigureAwait(false);
        });
        return Task.CompletedTask;
    }

    [Test]
    public static Task MatchAsync_ResultInvalid_ThrowsInvalidOperationException()
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

    [Test]
    public static Task TapAsync_ResultInvalid_ThrowsInvalidOperationException()
    {
        _ = Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            _ = await new Result<int>()
                .ToTask()
                .TapAsync(
                    async _ => { await Task.Delay(0).ConfigureAwait(false); },
                    async _ => { await Task.Delay(0).ConfigureAwait(false); })
                .ConfigureAwait(false);
        });
        return Task.CompletedTask;
    }
}