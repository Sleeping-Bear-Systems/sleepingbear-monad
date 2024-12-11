using System.Globalization;

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
            Assert.That(state, Is.True);
            Assert.That(value, Is.EqualTo(1234));
            Assert.That(exception, Is.Null);
        });
    }

    [Test]
    public static void Ctor_Default_InvalidOperationException()
    {
        var exceptional = new Exceptional<int>();
        var (isValue, value, exception) = exceptional;
        Assert.Multiple(() =>
        {
            Assert.That(exceptional.IsValue, Is.False);
            Assert.That(exceptional.IsException, Is.True);
            Assert.That(isValue, Is.False);
            Assert.That(value, Is.EqualTo(default(int)));
            Assert.That(exception, Is.InstanceOf<InvalidOperationException>());
        });
    }

    [Test]
    public static void Ctor_Exception_SetException()
    {
        var exception = new InvalidOperationException();
        var exceptional = new Exceptional<int>(exception);
        var (isValue, value, outException) = exceptional;
        Assert.Multiple(() =>
        {
            Assert.That(exceptional.IsValue, Is.False);
            Assert.That(exceptional.IsException, Is.True);
            Assert.That(isValue, Is.False);
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

    [Test]
    public static void Try_Value_ReturnsValue()
    {
        var exceptional = new Exceptional<int>(1234);
        if (exceptional.Try(out var value))
        {
            Assert.That(value, Is.EqualTo(1234));
        }
        else
        {
            Assert.Fail("Should not be called.");
        }
    }

    [Test]
    public static void Match_Value_ReturnsValue()
    {
        var exceptional = new Exceptional<int>(1234);
        var result = exceptional.Match(
            value => value.ToString(CultureInfo.InvariantCulture),
            _ =>
            {
                Assert.Fail("Should not be called.");
                return string.Empty;
            });
        Assert.That(result, Is.EqualTo("1234"));
    }
}