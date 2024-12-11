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
            .Tap(
                some => { Assert.That(some, Is.EqualTo(1234)); },
                () => { Assert.Fail("Should not be called."); });
    }

    [Test]
    public static void Where_SomeDoesNotMatchPredicate_ReturnsNone()
    {
        var maybe = 1234
            .ToMaybe()
            .Where(some => some != 1234);
        Assert.That(maybe.IsNone, Is.True);
    }

    [Test]
    public static void Match_None_ReturnsNoneValue()
    {
        var maybe = Maybe<string>.None;
        Assert.That(maybe.Match("none"), Is.EqualTo("none"));
    }

    [Test]
    public static void Match_None_ReturnsNoneFunc()
    {
        var maybe = Maybe<string>.None;
        var matched = maybe.Match(some =>
            {
                Assert.Fail("Should not be called.");
                return some;
            },
            () => "none");
        Assert.That(matched, Is.EqualTo("none"));
    }

    [Test]
    public static void Map_Some_ReturnsMappedValue()
    {
        _ = new Maybe<string>("some")
            .Map(some => some.ToUpperInvariant())
            .Tap(
                some => { Assert.That(some, Is.EqualTo("SOME")); },
                () => { Assert.Fail("Should not be called."); });
    }

    [Test]
    public static void Bind_Some_ReturnMappedValue()
    {
        _ = new Maybe<string>("some")
            .Bind(some => new Maybe<string>(some.ToUpperInvariant()))
            .Tap(some => { Assert.That(some, Is.EqualTo("SOME")); },
                () => { Assert.Fail("Should not be called."); });
    }

    [Test]
    public static void Try_Some_ReturnTrue()
    {
        var maybe = new Maybe<string>("some");
        if (maybe.Try(out var some))
            Assert.That(some, Is.EqualTo("some"));
        else
            Assert.Fail("Should not be called.");
    }
}
