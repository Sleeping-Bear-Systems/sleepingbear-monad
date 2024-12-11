using System.Diagnostics.CodeAnalysis;
using SleepingBear.Monad.Errors;

namespace SleepingBear.Monad.Monads.Test;

/// <summary>
///     Tests for <see cref="ResultExtensions" />.
/// </summary>
internal static class ResultExtensionTests
{
    [Test]
    public static void ToResult_OK_ValidatesBehavior()
    {
        var result = 1234.ToResult();
        var (isOk, ok, error) = result;
        Assert.Multiple(() =>
        {
            Assert.That(isOk, Is.True);
            Assert.That(ok, Is.EqualTo(1234));
            Assert.That(error, Is.Null);
        });
    }

    [Test]
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public static void ToResult_Error_ValidatesBehavior()
    {
        var result = 1234.ToError().ToResult<string>();
        var (isOk, ok, error) = result;
        Assert.Multiple(() =>
        {
            Assert.That(isOk, Is.False);
            Assert.That(ok, Is.Null);
            error!.TestErrorOf<Error<int>>(e => { Assert.That(e.Value, Is.EqualTo(1234)); });
        });
    }

    [Test]
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public static void Where_NullPredicate_ThrowsArgumentNullException()
    {
        _ = Assert.Throws<ArgumentNullException>(() =>
        {
            _ = new Result<int>()
                .Where(null!, some => new Error<int>(some));
        });
    }

    [Test]
    public static void MapIf_PredicateTrue_ReturnsMappedValue()
    {
        _ = 1234
            .ToResult()
            .MapIf(ok => ok > 0, ok => -ok)
            .Tap(ok => Assert.That(ok, Is.EqualTo(-1234)),
                _ => Assert.Fail("Should not be called."));
    }

    [Test]
    public static void MapIf_PredicateFalse_ReturnsOriginalValue()
    {
        _ = 1234
            .ToResult()
            .MapIf(ok => ok < 0, ok => -ok)
            .Tap(ok => Assert.That(ok, Is.EqualTo(1234)),
                _ => Assert.Fail("Should not be called."));
    }

    [Test]
    public static void BindIf_PredicateTrue_ReturnsMappedValue()
    {
        _ = 1234
            .ToResult()
            .BindIf(ok => ok > 0, ok => (-ok).ToResult())
            .Tap(ok => Assert.That(ok, Is.EqualTo(-1234)),
                _ => Assert.Fail("Should not be called."));
    }

    [Test]
    public static void BindIf_PredicateFalse_ReturnsOriginalValue()
    {
        _ = 1234
            .ToResult()
            .BindIf(ok => ok < 0, ok => (-ok).ToResult())
            .Tap(ok => Assert.That(ok, Is.EqualTo(1234)),
                _ => Assert.Fail("Should not be called."));
    }

    [Test]
    public static void Tap_Ok_OkActionCalled()
    {
        var okActionCalled = false;
        _ = 1234
            .ToResult()
            .Tap(ok =>
            {
                Assert.That(ok, Is.EqualTo(1234));
                okActionCalled = true;
            }, _ => { Assert.Fail("Should not be called."); });
        Assert.That(okActionCalled, Is.True);
    }

    [Test]
    public static void Tap_Error_ErrorActionCalled()
    {
        var errorActionCalled = false;
        _ = "error"
            .ToError()
            .ToResult<int>()
            .Tap(
                _ => { Assert.Fail("Should not be called"); },
                error =>
                {
                    error.TestErrorOf<Error<string>>(e => Assert.That(e.Value, Is.EqualTo("error")));
                    errorActionCalled = true;
                });
        Assert.That(errorActionCalled, Is.True);
    }

    [Test]
    public static void Try_Ok_ReturnsOk()
    {
        var isOk = 1234
            .ToResult()
            .Try(out var ok);
        Assert.Multiple(() =>
        {
            Assert.That(isOk, Is.True);
            Assert.That(ok, Is.EqualTo(1234));
        });
    }

    [Test]
    public static void TryError_Error_ReturnsError()
    {
        var isOk = 1234
            .ToError()
            .ToResult<int>()
            .TryError(out var resultError);
        Assert.That(isOk, Is.True);
        resultError!.TestErrorOf<Error<int>>(e => { Assert.That(e.Value, Is.EqualTo(1234)); });
    }

    [Test]
    public static void Try_Error_ReturnsError()
    {
        var error = new Error<string>("error");
        var result = new Result<string>(error);
        if (result.Try(out var ok))
            Assert.Fail("Should not be called.");
        else
            Assert.That(ok, Is.Null);
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
