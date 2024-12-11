namespace SleepingBear.Monad.Partials.Test;

/// <summary>
///     Tests for <see cref="EnumerableExtensions" />.
/// </summary>
internal static class EnumerableExtensionsTests
{
    [Test]
    public static void FirstOrNone_NullCollection_ReturnsNone()
    {
        var maybe = default(IEnumerable<int>)
            .FirstOrNone(v => v == 1234);
        Assert.That(maybe.IsNone, Is.True);
    }

    [Test]
    public static void FirstOrNone_CollectionWithNoMatch_ReturnsNone()
    {
        int[] array = [1, 2, 3];
        var maybe = array.FirstOrNone(v => v == 1234);
        Assert.That(maybe.IsNone, Is.True);
    }
}
