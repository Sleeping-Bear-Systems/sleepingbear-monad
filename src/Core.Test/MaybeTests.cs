namespace SleepingBear.Monad.Core.Test;

/// <summary>
///     Tests for <see cref="Maybe{TSome}" />.
/// </summary>
internal static class MaybeTests
{
    [Test]
    public static void Ctor_Null_ExpectIsNone()
    {
        var maybe = new Maybe<string>(null);
        var (isSome, some) = maybe;
        Assert.Multiple(() =>
        {
            Assert.That(maybe.IsSome, Is.False);
            Assert.That(maybe.IsNone, Is.True);
            Assert.That(isSome, Is.False);
            Assert.That(some, Is.Null);
        });
    }

    [Test]
    public static void Ctor_NotNull_ExpectIsSome()
    {
        var maybe = new Maybe<string>("some");
        var (isSome, some) = maybe;
        Assert.Multiple(() =>
        {
            Assert.That(maybe.IsSome, Is.True);
            Assert.That(maybe.IsNone, Is.False);
            Assert.That(isSome, Is.True);
            Assert.That(some, Is.EqualTo("some"));
        });
    }

    [Test]
    public static void None_ExpectIsNone()
    {
        var maybe = Maybe<string>.None;
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
            .Tap(some => { Assert.That(some, Is.EqualTo("SOME")); }, () => { Assert.Fail("Should not be called."); });
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
        {
            Assert.That(some, Is.EqualTo("some"));
        }
        else
        {
            Assert.Fail("Should not be called.");
        }
    }
}