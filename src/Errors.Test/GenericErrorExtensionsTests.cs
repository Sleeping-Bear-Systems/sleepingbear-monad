namespace SleepingBear.Monad.Errors.Test;

/// <summary>
///     Tests for <see cref="GenericError" />.
/// </summary>
internal static class GenericErrorExtensionsTests
{
    [Test]
    public static void ToError_WrapValue()
    {
        var error = 1234.ToGenericError();
        Assert.That(error, Is.InstanceOf<GenericError<int>>());
        Assert.That(error.Value, Is.EqualTo(1234));
    }
}