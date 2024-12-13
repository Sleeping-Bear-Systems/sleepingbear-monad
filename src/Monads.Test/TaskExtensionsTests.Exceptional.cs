using System.Globalization;
using SleepingBear.Monad.Tasks;

namespace SleepingBear.Monad.Monads.Test;

/// <summary>
///     Tests for <see cref="TaskExtensions" />.
/// </summary>
internal static partial class TaskExtensionsTests
{
    [Test]
    public static Task MapAsync_ExceptionalValue_MapsToValue()
    {
        _ = new Exceptional<string>("value")
            .ToTask()
            .MapAsync(async value =>
            {
                await Task.Delay(0).ConfigureAwait(false);
                return value.ToUpperInvariant();
            })
            .TapAsync(
                async value =>
                {
                    await Task.Delay(0).ConfigureAwait(false);
                    Assert.That(value, Is.EqualTo("VALUE"));
                },
                async _ =>
                {
                    await Task.Delay(0).ConfigureAwait(false);
                    Assert.Fail("Should not be called.");
                })
            .ConfigureAwait(false);
        return Task.CompletedTask;
    }

    [Test]
    public static async Task BindAsync_ExceptionalValue_BindsToValue()
    {
        _ = await new Exceptional<string>("value")
            .ToTask()
            .BindAsync(async value =>
            {
                await Task.Delay(0).ConfigureAwait(false);
                return Exceptional.Success(value.ToUpperInvariant());
            })
            .TapAsync(
                async value =>
                {
                    await Task.Delay(0).ConfigureAwait(false);
                    Assert.That(value, Is.EqualTo("VALUE"));
                },
                async _ =>
                {
                    await Task.Delay(0).ConfigureAwait(false);
                    Assert.Fail("Should not be called.");
                })
            .ConfigureAwait(false);
    }

    [Test]
    public static async Task Match_Value_ReturnsValue()
    {
        var matched = await new Exceptional<int>(1234)
            .ToTask()
            .MatchAsync(
                async value =>
                {
                    await Task.Delay(0).ConfigureAwait(false);
                    return value.ToString(CultureInfo.InvariantCulture);
                },
                async _ =>
                {
                    await Task.Delay(0).ConfigureAwait(false);
                    return string.Empty;
                })
            .ConfigureAwait(false);
        Assert.That(matched, Is.EqualTo("1234"));
    }
}