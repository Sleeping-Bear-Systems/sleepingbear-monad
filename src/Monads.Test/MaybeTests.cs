namespace SleepingBear.Monad.Monads.Test;

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
    public static void None_ReferenceType_ExpectIsNone()
    {
        var maybe = Maybe<string>.None;
        Assert.That(maybe.IsNone, Is.True);
    }

    [Test]
    public static void None_ValueType_ExpectIsNone()
    {
        var maybe = Maybe<int>.None;
        Assert.That(maybe.IsNone, Is.True);
    }
}
