using System.Globalization;
using SleepingBear.Monad.Errors;

namespace SleepingBear.Monad.Monads.Test;

/// <summary>
///     Tests for <see cref="ExceptionalExtensions" />.
/// </summary>
internal static class ExceptionalExtensionsTests
{
    [Test]
    public static void ToResult_Value_ReturnResultOk()
    {
        _ = 1234
            .ToExceptional()
            .ToResult()
            .Tap(value => { Assert.That(value, Is.EqualTo(1234)); },
                _ => { Assert.Fail("Should not be called."); });
    }

    [Test]
    public static void ToResult_Exception_ReturnResultError()
    {
        var exception = new InvalidOperationException("test");
        _ = exception
            .ToExceptional<int>()
            .ToResult()
            .Tap(_ => { Assert.Fail("Should not be called."); },
                error =>
                {
                    switch (error)
                    {
                        case Error<Exception> exceptionError:
                            Assert.That(exceptionError.Value, Is.SameAs(exception));
                            break;
                        default:
                            Assert.Fail("Should not be called.");
                            break;
                    }
                }
            );
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
            Assert.That(value, Is.EqualTo(1234));
        else
            Assert.Fail("Should not be called.");
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
