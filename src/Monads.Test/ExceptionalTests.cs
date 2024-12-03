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
            Assert.That(value, Is.EqualTo(default));
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
            Assert.That(value, Is.EqualTo(default));
            Assert.That(outException, Is.SameAs(exception));
        });
    }
}