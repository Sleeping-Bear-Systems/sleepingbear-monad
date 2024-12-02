namespace SleepingBear.Monad.Errors;

/// <summary>
///     Extension methods for <see cref="Error" />/
/// </summary>
public static class ErrorExtensions
{
    /// <summary>
    ///     Wraps a value in a <see cref="Error{T}" />.
    /// </summary>
    /// <param name="value">The value being wrapped.</param>
    /// <typeparam name="T">The value type.</typeparam>
    /// <returns>A <see cref="Error{TValue}" />.</returns>
    public static Error<T> ToError<T>(this T value) where T : notnull
    {
        return new Error<T>(value);
    }
}