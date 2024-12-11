namespace SleepingBear.Monad.Errors;

/// <summary>
///     Extension methods for <see cref="ValidationError" />.
/// </summary>
public static class ValidationErrorExtensions
{
    /// <summary>
    ///     Creates a <see cref="ValidationError" /> from a message.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="tag">The error tag. (optional)</param>
    /// <returns>A <see cref="ValidationError" />.</returns>
    public static ValidationError ToValidationError(this string message, string? tag = null)
    {
        return new ValidationError(message, (tag ?? string.Empty).Trim());
    }
}
