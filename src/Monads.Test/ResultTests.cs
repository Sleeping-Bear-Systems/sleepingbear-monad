using System.Diagnostics.CodeAnalysis;
using SleepingBear.Monad.Errors;

namespace SleepingBear.Monad.Monads.Test;

/// <summary>
///     Tests for <see cref="Result{TOk}" />.
/// </summary>
internal static class ResultTests
{
    [Test]
    public static void Ctor_Default_UnknownError()
    {
        var result = new Result<int>();
        var (isOk, ok, error) = result;
        Assert.Multiple(() =>
        {
            Assert.That(result.IsOk, Is.False);
            Assert.That(result.IsError, Is.True);
            Assert.That(isOk, Is.False);
            Assert.That(ok, Is.EqualTo(0));
            Assert.That(error, Is.InstanceOf<UnknownError>());
        });
    }

    [Test]
    public static void Ctor_Ok_OkState()
    {
        var result = new Result<int>(1234);
        var (isOk, ok, error) = result;
        Assert.Multiple(() =>
        {
            Assert.That(result.IsOk, Is.True);
            Assert.That(result.IsError, Is.False);
            Assert.That(isOk, Is.True);
            Assert.That(ok, Is.EqualTo(1234));
            Assert.That(error, Is.Null);
        });
    }

    [Test]
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public static void Ctor_Error_ErrorState()
    {
        var result = new Result<int>(1234.ToError());
        var (isOk, ok, error) = result;
        Assert.Multiple(() =>
        {
            Assert.That(isOk, Is.False);
            Assert.That(ok, Is.EqualTo(0));
            error!.TestErrorOf<Error<int>>(e => { Assert.That(e.Value, Is.EqualTo(1234)); });
        });
    }
}