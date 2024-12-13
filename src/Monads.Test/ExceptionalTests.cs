using System.Globalization;
using SleepingBear.Monad.Errors;

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
            Assert.That(exceptional.IsSuccess, Is.True);
            Assert.That(exceptional.IsFailure, Is.False);
            Assert.That(state, Is.True);
            Assert.That(value, Is.EqualTo(1234));
            Assert.That(exception, Is.Null);
        });
    }

    [Test]
    public static void Ctor_Default_InvalidOperationException()
    {
        var exceptional = new Exceptional<int>();
        var (isSuccess, value, exception) = exceptional;
        Assert.Multiple(() =>
        {
            Assert.That(exceptional.IsSuccess, Is.False);
            Assert.That(exceptional.IsFailure, Is.True);
            Assert.That(isSuccess, Is.False);
            Assert.That(value, Is.EqualTo(0));
            Assert.That(exception, Is.InstanceOf<InvalidOperationException>());
        });
    }

    [Test]
    public static void Ctor_Exception_SetException()
    {
        var exception = new ArgumentNullException();
        var exceptional = new Exceptional<int>(exception);
        var (isSuccess, value, outException) = exceptional;
        Assert.Multiple(() =>
        {
            Assert.That(exceptional.IsSuccess, Is.False);
            Assert.That(exceptional.IsFailure, Is.True);
            Assert.That(isSuccess, Is.False);
            Assert.That(value, Is.EqualTo(0));
            Assert.That(outException, Is.SameAs(exception));
        });
    }

    [Test]
    public static void ToResult_Value_ReturnResultOk()
    {
        _ = Exceptional
            .Success(1234)
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
                        case GenericError<Exception> exceptionError:
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

    [Test]
    public static void TryCatch_All_ReturnsException()
    {
        var exceptional =
            Exceptional.TryCatch<int>(() => throw new InvalidOperationException("test"));
        var (isSuccess, value, exception) = exceptional;
        Assert.Multiple(() =>
        {
            Assert.That(isSuccess, Is.False);
            Assert.That(value, Is.EqualTo(0));
            Assert.That(exception, Is.InstanceOf<InvalidOperationException>());
        });
    }

    [Test]
    public static void TryCatch_OneException_ReturnsException()
    {
        var exceptional =
            Exceptional.TryCatch<int, InvalidOperationException>(() => throw new InvalidOperationException("test"));
        var (isSuccess, value, exception) = exceptional;
        Assert.Multiple(() =>
        {
            Assert.That(isSuccess, Is.False);
            Assert.That(value, Is.EqualTo(0));
            Assert.That(exception, Is.InstanceOf<InvalidOperationException>());
        });
    }

    [Test]
    public static void TryCatch_TwoException_ReturnsException()
    {
        var exceptional =
            Exceptional.TryCatch<int, InvalidOperationException, ArgumentNullException>(() =>
                throw new InvalidOperationException("test"));
        var (isSuccess, value, exception) = exceptional;
        Assert.Multiple(() =>
        {
            Assert.That(isSuccess, Is.False);
            Assert.That(value, Is.EqualTo(0));
            Assert.That(exception, Is.InstanceOf<InvalidOperationException>());
        });
    }

    [Test]
    public static void TryCatch_ThreeException_ReturnsException()
    {
        var exceptional =
            Exceptional.TryCatch<int, InvalidOperationException, ArgumentNullException, ArgumentOutOfRangeException>(
                () => throw new InvalidOperationException("test"));
        var (isSuccess, value, exception) = exceptional;
        Assert.Multiple(() =>
        {
            Assert.That(isSuccess, Is.False);
            Assert.That(value, Is.EqualTo(0));
            Assert.That(exception, Is.InstanceOf<InvalidOperationException>());
        });
    }
}