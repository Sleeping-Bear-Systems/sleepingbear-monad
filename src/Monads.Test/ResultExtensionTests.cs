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
            Assert.That(isOk, Is.EqualTo(true));
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
            Assert.That(isOk, Is.EqualTo(false));
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
}