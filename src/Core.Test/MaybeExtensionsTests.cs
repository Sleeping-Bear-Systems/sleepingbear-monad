namespace SleepingBear.Monad.Core.Test;

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
}