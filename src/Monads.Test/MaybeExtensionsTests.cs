namespace SleepingBear.Monad.Monads.Test;

/// <summary>
///     Tests for <see cref="MaybeExtensions" />.
/// </summary>
internal static class MaybeExtensionsTests
{
    [Test]
    public static void ToMaybe_Some_ReturnsSome()
    {
        _ = 1234
            .ToMaybe()
            .Tap(some => { Assert.That(some, Is.EqualTo(1234)); }, () => { Assert.Fail("Should not be called."); });
    }

    [Test]
    public static void Where_SomeMatchesPredicate_ReturnsSome()
    {
        _ = 1234
            .ToMaybe()
            .Where(some => some == 1234)
            .Tap(some => { Assert.That(some, Is.EqualTo(1234)); }, () => { Assert.Fail("Should not be called."); });
    }

    [Test]
    public static void Where_SomeDoesNotMatchPredicate_ReturnsNone()
    {
        var maybe = 1234
            .ToMaybe()
            .Where(some => some != 1234);
        Assert.That(maybe.IsNone, Is.True);
    }
}