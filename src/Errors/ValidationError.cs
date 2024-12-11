namespace SleepingBear.Monad.Errors;

/// <summary>
///     Concrete <see cref="Error" /> record for representing validation errors.
/// </summary>
/// <param name="Message">The error message.</param>
/// <param name="Tag">The error tag.</param>
public record ValidationError(string Message, string Tag) : Error;
