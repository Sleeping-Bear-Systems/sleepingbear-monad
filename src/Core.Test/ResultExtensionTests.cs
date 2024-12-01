namespace SleepingBear.Monad.Core.Test;

/// <summary>
///     Tests for <see cref="ResultExtensions" />.
/// </summary>
internal static class ResultExtensionTests
{
    [Test]
    public static void ToOk_ValidatesBehavior()
    {
        var result = 1234.ToOk();
        result.Deconstruct(out var state, out var ok, out var error);
        Assert.That(state, Is.EqualTo(ResultState.Ok));
        Assert.That(ok, Is.EqualTo(1234));
        Assert.That(error, Is.Null);
    }

    [Test]
    public static void ToFailure_ValidatesBehavior()
    {
        var error = new Error<int>(1234);
        var result = error.ToFailure<string>();
        result.Deconstruct(out var state, out var ok, out var resultError);
        Assert.That(state, Is.EqualTo(ResultState.Failure));
        Assert.That(ok, Is.Null);
        Assert.That(resultError, Is.EqualTo(error));
    }
}