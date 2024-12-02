using System.Diagnostics.CodeAnalysis;
using SleepingBear.Monad.Errors;

namespace SleepingBear.Monad.Monads.Test;

/// <summary>
///     Tests for <see cref="Result{TOk}" />.
/// </summary>
internal static class ResultTests
{
    [Test]
    public static void Ctor_Default_InvalidState()
    {
        var result = new Result<int>();
        result.Deconstruct(out var state, out var ok, out var error);
        Assert.Multiple(() =>
        {
            Assert.That(result.IsOk, Is.False);
            Assert.That(result.IsError, Is.False);
            Assert.That(state, Is.EqualTo(ResultState.Invalid));
            Assert.That(ok, Is.EqualTo(0));
            Assert.That(error, Is.Null);
        });
    }

    [Test]
    public static void Ctor_Ok_OkState()
    {
        var result = new Result<int>(1234);
        result.Deconstruct(out var state, out var ok, out var error);
        Assert.Multiple(() =>
        {
            Assert.That(result.IsOk, Is.True);
            Assert.That(result.IsError, Is.False);
            Assert.That(state, Is.EqualTo(ResultState.Ok));
            Assert.That(ok, Is.EqualTo(1234));
            Assert.That(error, Is.Null);
        });
    }

    [Test]
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public static void Ctor_Error_ErrorState()
    {
        var error = 1234.ToError();
        var result = new Result<int>(error);
        result.Deconstruct(out var state, out var ok, out var resultError);
        Assert.Multiple(() =>
        {
            Assert.That(state, Is.EqualTo(ResultState.Error));
            Assert.That(ok, Is.EqualTo(0));
            Assert.That((Error<int>)resultError!, Is.EqualTo(error));
        });
    }

    [Test]
    public static void Map_Invalid_ThrowsInvalidOperationException()
    {
        _ = Assert.Throws<InvalidOperationException>(() =>
        {
            var result = new Result<int>();
            _ = result.Map(ok => ok);
        });
    }

    [Test]
    public static void MapError_Invalid_ThrowsInvalidOperationException()
    {
        _ = Assert.Throws<InvalidOperationException>(() =>
        {
            var result = new Result<int>();
            _ = result.MapError(error => error);
        });
    }

    [Test]
    public static void Bind_Invalid_ThrowsInvalidOperationException()
    {
        _ = Assert.Throws<InvalidOperationException>(() =>
        {
            var result = new Result<int>();
            _ = result.Bind(ok => new Result<int>(ok));
        });
    }

    [Test]
    public static void BindError_Invalid_ThrowsInvalidOperationException()
    {
        _ = Assert.Throws<InvalidOperationException>(() =>
        {
            var result = new Result<int>();
            _ = result.BindError(error => new Result<int>(error));
        });
    }

    [Test]
    public static void Match_Invalid_ThrowsInvalidOperationException()
    {
        _ = Assert.Throws<InvalidOperationException>(() =>
        {
            var result = new Result<int>();
            _ = result.Match(_ => 0, _ => 1);
        });
    }

    [Test]
    public static void Try_Invalid_ThrowsInvalidOperationException()
    {
        _ = Assert.Throws<InvalidOperationException>(() =>
        {
            var result = new Result<int>();
            _ = result.Try(out _);
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
    public static void TryError_Invalid_ThrowsInvalidOperationException()
    {
        _ = Assert.Throws<InvalidOperationException>(() =>
        {
            var result = new Result<int>();
            _ = result.TryError(out _);
        });
    }

    [Test]
    public static void TryError_Error_ReturnsError()
    {
        var error = 1234.ToError();
        var result = new Result<int>(error);
        if (result.TryError(out var resultError))
        {
            Assert.That((Error<int>)resultError, Is.EqualTo(error));
        }
        else
        {
            Assert.Fail("Should not be called.");
        }
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
}