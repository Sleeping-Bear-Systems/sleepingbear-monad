namespace SleepingBear.Monad.Monads.Test;

/// <summary>
///     Tests for <see cref="Exceptional{TValue}" />.
/// </summary>
internal static class ExceptionalTests
{
    [Test]
    public static void Ctor_Value_SetValue()
    {
        var exceptional = new Exceptional<int>(1234);
        var (state, value, exception) = exceptional;
        Assert.Multiple(() =>
        {
            Assert.That(exceptional.IsValue, Is.True);
            Assert.That(exceptional.IsException, Is.False);
            Assert.That(state, Is.EqualTo(ExceptionalState.Value));
            Assert.That(value, Is.EqualTo(1234));
            Assert.That(exception, Is.Null);
        });
    }

    [Test]
    public static void Ctor_Default_SetInvalid()
    {
        var exceptional = new Exceptional<int>();
        var (state, value, exception) = exceptional;
        Assert.Multiple(() =>
        {
            Assert.That(exceptional.IsValue, Is.False);
            Assert.That(exceptional.IsException, Is.False);
            Assert.That(state, Is.EqualTo(ExceptionalState.Invalid));
            Assert.That(value, Is.EqualTo(default(int)));
            Assert.That(exception, Is.Null);
        });
    }

    [Test]
    public static void Ctor_Exception_SetException()
    {
        var exception = new InvalidOperationException();
        var exceptional = new Exceptional<int>(exception);
        var (state, value, outException) = exceptional;
        Assert.Multiple(() =>
        {
            Assert.That(exceptional.IsValue, Is.False);
            Assert.That(exceptional.IsException, Is.True);
            Assert.That(state, Is.EqualTo(ExceptionalState.Exception));
            Assert.That(value, Is.EqualTo(default(int)));
            Assert.That(outException, Is.SameAs(exception));
        });
    }

    [Test]
    public static void Map_Value_MapsToValue()
    {
        _ = new Exceptional<string>("value")
            .Map(value => value.ToUpperInvariant())
            .Tap(value => { Assert.That(value, Is.EqualTo("VALUE")); },
                _ => { Assert.Fail("Should not be called."); });
    }

    [Test]
    public static void Bind_Value_MapsToValue()
    {
        _ = new Exceptional<string>("value")
            .Bind(value => new Exceptional<string>(value.ToUpperInvariant()))
            .Tap(value => { Assert.That(value, Is.EqualTo("VALUE")); },
                _ => { Assert.Fail("Should not be called."); });
    }
}