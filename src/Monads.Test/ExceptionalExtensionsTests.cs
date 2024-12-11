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
}
