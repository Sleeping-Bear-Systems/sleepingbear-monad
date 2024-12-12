namespace SleepingBear.Monad.Errors;

/// <summary>
///     Extension methods for <see cref="GenericError{TValue}" />.
/// </summary>
public static class ErrorExtensions
{
    /// <summary>
    ///     Wraps a value in a <see cref="GenericError{TValue}" />.
    /// </summary>
    /// <param name="value">The value being wrapped.</param>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <returns>A <see cref="GenericError{TValue}" /> containing the value.</returns>
    public static GenericError<TValue> ToGenericError<TValue>(this TValue value) where TValue : notnull
    {
        return new GenericError<TValue>(value);
    }
}