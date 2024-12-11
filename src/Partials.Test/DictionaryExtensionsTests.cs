using SleepingBear.Monad.Monads;

namespace SleepingBear.Monad.Partials.Test;

/// <summary>
///     Tests for <see cref="DictionaryExtensions" />.
/// </summary>
internal static class DictionaryExtensionsTests
{
    [Test]
    public static void GetValueOrNone_KeyFound_ReturnsValue()
    {
        _ = new Dictionary<string, int> { { "key", 1234 } }
            .GetValueOrNone("key")
            .Tap(
                value => { Assert.That(value, Is.EqualTo(1234)); },
                () => Assert.Fail("Should not be called."));
    }

    [Test]
    public static void GetValueOrNone_KeyNotFound_ReturnsValue()
    {
        var maybe = new Dictionary<string, int> { { "key", 1234 } }
            .GetValueOrNone("not_found");
        Assert.That(maybe.IsNone, Is.True);
    }

    [Test]
    public static void GetValueOrNone_NullDictionary_ReturnsValue()
    {
        var maybe = default(Dictionary<string, int>)
            .GetValueOrNone("not_found");
        Assert.That(maybe.IsNone, Is.True);
    }
}
