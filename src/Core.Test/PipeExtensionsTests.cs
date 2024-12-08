namespace SleepingBear.Monad.Core.Test;

/// <summary>
///     Tests for <see cref="PipeExtensions" />.
/// </summary>
internal static class PipeExtensionsTests
{
    [Test]
    public static void Pipe_ReturnsValue()
    {
        var value = 1234.Pipe(x => x * 2);
        Assert.That(value, Is.EqualTo(2468));
    }

    [Test]
    public static async Task PipeAsync_SyncPipeFunction_ReturnsValue()
    {
        var value = await Task
            .FromResult(1234)
            .PipeAsync(x => x * 2)
            .ConfigureAwait(true);
        Assert.That(value, Is.EqualTo(2468));
    }

    [Test]
    public static async Task PipeAsync_ASyncPipeFunction_ReturnsValue()
    {
        var value = await Task
            .FromResult(1234)
            .PipeAsync(async x =>
            {
                await Task.Delay(0).ConfigureAwait(false);
                return x * 2;
            })
            .ConfigureAwait(true);
        Assert.That(value, Is.EqualTo(2468));
    }
}