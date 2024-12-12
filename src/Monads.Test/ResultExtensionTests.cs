using System.Diagnostics.CodeAnalysis;
using System.Globalization;
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
        var result = Result.Ok(1234);
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
        var result = 1234
            .ToGenericError()
            .ToResult<string>();
        var (isOk, ok, error) = result;
        Assert.Multiple(() =>
        {
            Assert.That(isOk, Is.False);
            Assert.That(ok, Is.Null);
            error!.TestErrorOf<GenericError<int>>(e => { Assert.That(e.Value, Is.EqualTo(1234)); });
        });
    }

    [Test]
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public static void Where_NullPredicate_ThrowsArgumentNullException()
    {
        _ = Assert.Throws<ArgumentNullException>(() =>
        {
            _ = new Result<int>()
                .Where(null!, some => new GenericError<int>(some));
        });
    }

    [Test]
    public static void MapIf_PredicateTrue_ReturnsMappedValue()
    {
        _ = Result
            .Ok(1234)
            .MapIf(ok => ok > 0, ok => -ok)
            .Tap(ok => Assert.That(ok, Is.EqualTo(-1234)),
                _ => Assert.Fail("Should not be called."));
    }

    [Test]
    public static void MapIf_PredicateFalse_ReturnsOriginalValue()
    {
        _ = Result
            .Ok(1234)
            .MapIf(ok => ok < 0, ok => -ok)
            .Tap(ok => Assert.That(ok, Is.EqualTo(1234)),
                _ => Assert.Fail("Should not be called."));
    }

    [Test]
    public static void BindIf_PredicateTrue_ReturnsMappedValue()
    {
        _ = Result
            .Ok(1234)
            .BindIf(ok => ok > 0, ok => -ok)
            .Tap(ok => Assert.That(ok, Is.EqualTo(-1234)),
                _ => Assert.Fail("Should not be called."));
    }

    [Test]
    public static void BindIf_PredicateFalse_ReturnsOriginalValue()
    {
        _ = Result
            .Ok(1234)
            .BindIf(ok => ok < 0, ok => -ok)
            .Tap(ok => Assert.That(ok, Is.EqualTo(1234)),
                _ => Assert.Fail("Should not be called."));
    }

    [Test]
    public static void Tap_Ok_OkActionCalled()
    {
        var okActionCalled = false;
        _ = Result
            .Ok(1234)
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
            .ToGenericError()
            .ToResult<int>()
            .Tap(
                _ => { Assert.Fail("Should not be called"); },
                error =>
                {
                    error.TestErrorOf<GenericError<string>>(e => Assert.That(e.Value, Is.EqualTo("error")));
                    errorActionCalled = true;
                });
        Assert.That(errorActionCalled, Is.True);
    }

    [Test]
    public static void Try_Ok_ReturnsOk()
    {
        var isOk = Result
            .Ok(1234)
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
        var isError = 1234
            .ToGenericError()
            .ToResult<int>()
            .TryError(out var error);
        Assert.That(isError, Is.True);
        error.TestErrorOf<GenericError<int>>(e => { Assert.That(e.Value, Is.EqualTo(1234)); });
    }

    [Test]
    public static void Try_Error_ReturnsError()
    {
        var error = new GenericError<string>("error");
        var result = new Result<string>(error);
        if (result.Try(out var ok))
            Assert.Fail("Should not be called.");
        else
            Assert.That(ok, Is.Null);
    }

    [Test]
    public static void Map_Ok_MapsValue()
    {
        _ = Result
            .Ok(1234)
            .Map(ok => ok * 2)
            .Tap(ok => { Assert.That(ok, Is.EqualTo(2468)); },
                _ => { Assert.Fail("Should not be called."); });
    }

    [Test]
    public static void MapError_Error_MapsError()
    {
        _ = new GenericError<string>("error")
            .ToResult<int>()
            .MapError(_ => 1234.ToGenericError())
            .Tap(
                _ => { Assert.Fail("Should not be called."); },
                error =>
                {
                    error.TestErrorOf<GenericError<int>>(intError =>
                    {
                        Assert.That(intError.Value, Is.EqualTo(1234));
                    });
                });
    }

    [Test]
    public static void Bind_Ok_MapsValue()
    {
        _ = Result
            .Ok(1234)
            .Bind(ok => ok > 0
                ? ok.ToString(CultureInfo.InvariantCulture)
                : "error".ToGenericError().ToResult<string>())
            .Tap(ok => { Assert.That(ok, Is.EqualTo("1234")); },
                _ => { Assert.Fail("Should not be called."); });
    }

    [Test]
    public static void BindError_Error_MapsError()
    {
        _ = new GenericError<string>("error")
            .ToResult<int>()
            .BindError(_ => 1234.ToGenericError().ToResult<int>())
            .Tap(
                _ => { Assert.Fail("Should not be called."); },
                error =>
                {
                    error.TestErrorOf<GenericError<int>>(intError =>
                    {
                        Assert.That(intError.Value, Is.EqualTo(1234));
                    });
                });
    }

    [Test]
    public static void Match_Ok_MapsValue()
    {
        var result = Result
            .Ok(1234)
            .Match(
                ok => ok * 2,
                _ => 0);
        Assert.That(result, Is.EqualTo(2468));
    }
}