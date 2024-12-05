using System.Globalization;

namespace SleepingBear.Monad.Tasks.Test;

/// <summary>
///     Tests for <see cref="TaskExtensions" />.
/// </summary>
internal static class TaskExtensionsTests
{
    [Test]
    public static async Task MapAsync_ValidatesBehavior()
    {
        var value = await 1234
            .ToTask()
            .Map(value => value.ToString(CultureInfo.InvariantCulture))
            .ConfigureAwait(true);
        Assert.That(value, Is.EqualTo("1234"));
    }

    [Test]
    public static async Task BindAsync_ValidatesBehavior()
    {
        var value = await 1234
            .ToTask()
            .Bind(async value =>
            {
                await Task.Delay(0).ConfigureAwait(false);
                return value.ToString(CultureInfo.InvariantCulture);
            }).ConfigureAwait(true);
        Assert.That(value, Is.EqualTo("1234"));
    }
}