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
            Assert.That(isOk, Is.EqualTo(true));
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
            Assert.That(isOk, Is.EqualTo(false));
            Assert.That(ok, Is.EqualTo(0));
            error!.TestErrorOf<Error<int>>(e => { Assert.That(e.Value, Is.EqualTo(1234)); });
        });
    }

    [Test]
    public static void Try_Error_ReturnsError()
    {
        var error = new Error<string>("error");
        var result = new Result<string>(error);
        if (result.Try(out var ok))
        {
            Assert.Fail("Should not be called.");
        }
        else
        {
            Assert.That(ok, Is.Null);
        }
    }

    [Test]
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public static void TryError_Error_ReturnsError()
    {
        var error = 1234.ToError();
        var result = new Result<int>(error);
        var isOk = result.TryError(out var resultError);
        Assert.That(isOk, Is.True);
        resultError!.TestErrorOf<Error<int>>(e => { Assert.That(e.Value, Is.EqualTo(1234)); });
    }

    [Test]
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public static void Tap_NullOkAction_ThrowsArgumentNullException()
    {
        _ = Assert.Throws<ArgumentNullException>(() =>
        {
            var result = new Result<int>();
            _ = result.Tap(null!, _ => { });
        });
    }

    [Test]
    public static void Map_Ok_MapsValue()
    {
        _ = 1234
            .ToResult()
            .Map(ok => ok * 2)
            .Tap(ok => { Assert.That(ok, Is.EqualTo(2468)); },
                _ => { Assert.Fail("Should not be called."); });
    }

    [Test]
    public static void MapError_Error_MapsError()
    {
        _ = new Error<string>("error")
            .ToResult<int>()
            .MapError(_ => 1234.ToError())
            .Tap(
                _ => { Assert.Fail("Should not be called."); },
                error =>
                {
                    error.TestErrorOf<Error<int>>(intError => { Assert.That(intError.Value, Is.EqualTo(1234)); });
                });
    }
    
    [Test]
    public static void Bind_Ok_MapsValue()
    {
        _ = 1234
            .ToResult()
            .Bind(ok => (ok * 2).ToResult())
            .Tap(ok => { Assert.That(ok, Is.EqualTo(2468)); },
                _ => { Assert.Fail("Should not be called."); });
    }

    [Test]
    public static void BindError_Error_MapsError()
    {
        _ = new Error<string>("error")
            .ToResult<int>()
            .BindError(_ => 1234.ToError().ToResult<int>())
            .Tap(
                _ => { Assert.Fail("Should not be called."); },
                error =>
                {
                    error.TestErrorOf<Error<int>>(intError => { Assert.That(intError.Value, Is.EqualTo(1234)); });
                });
    }
    
    [Test]
    public static void Match_Ok_MapsValue()
    {
        var result = 1234
            .ToResult()
            .Match(
                ok => ok * 2,
                _ => 0);
        Assert.That(result, Is.EqualTo(2468));
    }

}