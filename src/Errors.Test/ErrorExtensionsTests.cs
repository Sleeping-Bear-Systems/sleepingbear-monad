namespace SleepingBear.Monad.Errors.Test;

/// <summary>
///     Tests for <see cref="ErrorExtensions" />.
/// </summary>
internal static class ErrorExtensionsTests
{
    [Test]
    public static void ToError_WrapValue()
    {
        var error = 1234.ToError();
        Assert.That(error, Is.InstanceOf<Error<int>>());
        Assert.That(error.Value, Is.EqualTo(1234));
    }
}