namespace SleepingBear.Monad.Errors.Test;

/// <summary>
///     Tests for <see cref="ValidationErrorExtensions" />.
/// </summary>
internal static class ValidationErrorExtensionsTests
{
    [Test]
    public static void ToValidationError_NullTag_ReturnValidationError()
    {
        var error = "message".ToValidationError();
        Assert.Multiple(() =>
        {
            Assert.That(error.Message, Is.EqualTo("message"));
            Assert.That(error.Tag, Is.Empty);
        });
    }
}