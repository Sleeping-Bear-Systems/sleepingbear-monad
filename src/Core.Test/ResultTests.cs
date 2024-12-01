namespace SleepingBear.Monad.Core.Test;

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
        Assert.That(state, Is.EqualTo(ResultState.Invalid));
        Assert.That(ok, Is.EqualTo(0));
        Assert.That(error, Is.Null);
    }

    [Test]
    public static void Ctor_Ok_OkState()
    {
        var result = new Result<int>(1234);
        result.Deconstruct(out var state, out var ok, out var error);
        Assert.That(state, Is.EqualTo(ResultState.Ok));
        Assert.That(ok, Is.EqualTo(1234));
        Assert.That(error, Is.Null);
    }

    [Test]
    public static void Ctor_Error_FailureState()
    {
        var error = new Error<int>(1234);
        var result = new Result<int>(error);
        result.Deconstruct(out var state, out var ok, out var resultError);
        Assert.That(state, Is.EqualTo(ResultState.Failure));
        Assert.That(ok, Is.EqualTo(0));
        Assert.That(resultError, Is.EqualTo(error));
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
    public static void MapFailure_Invalid_ThrowsInvalidOperationException()
    {
        _ = Assert.Throws<InvalidOperationException>(() =>
        {
            var result = new Result<int>();
            _ = result.MapFailure(error => error);
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
}