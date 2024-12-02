using System.Diagnostics.CodeAnalysis;

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
        Assert.Multiple(() =>
        {
            Assert.That(state, Is.EqualTo(ResultState.Ok));
            Assert.That(ok, Is.EqualTo(1234));
            Assert.That(error, Is.Null);
        });
    }

    [Test]
    public static void ToError_ValidatesBehavior()
    {
        var error = new Error<int>(1234);
        var result = error.ToError<string>();
        result.Deconstruct(out var state, out var ok, out var resultError);
        Assert.Multiple(() =>
        {
            Assert.That(state, Is.EqualTo(ResultState.Error));
            Assert.That(ok, Is.Null);
            Assert.That(resultError, Is.EqualTo(error));
        });
    }

    [Test]
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public static void Filter_NullPredicate_ThrowsArgumentNullException()
    {
        _ = Assert.Throws<ArgumentNullException>(() =>
        {
            _ = new Result<int>()
                .Filter(null!, some => new Error<int>(some));
        });
    }
}