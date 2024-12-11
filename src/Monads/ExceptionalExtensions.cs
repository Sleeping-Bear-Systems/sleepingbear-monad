using System.Diagnostics.CodeAnalysis;
using SleepingBear.Monad.Errors;

namespace SleepingBear.Monad.Monads;

/// <summary>
///     Extension methods for <see cref="Exceptional{TValue}" />.
/// </summary>
public static class ExceptionalExtensions
{
    /// <summary>
    ///     Converts a value to <see cref="Exceptional{TValue}" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <typeparam name="TValue">The value type.</typeparam>
    /// <returns>A <see cref="Exceptional{TValue}" />.</returns>
    public static Exceptional<TValue> ToExceptional<TValue>(this TValue value) where TValue : notnull
    {
        return new Exceptional<TValue>(value);
    }

    /// <summary>
    ///     Converts a <see cref="Exception" /> to a <see cref="Exceptional{TValue}" />.
    /// </summary>
    /// <param name="exception">The exception.></param>
    /// <typeparam name="TValue">The value type.</typeparam>
    /// <returns>A <see cref="Exceptional{TValue}" />.</returns>
    public static Exceptional<TValue> ToExceptional<TValue>(this Exception exception) where TValue : notnull
    {
        return new Exceptional<TValue>(exception);
    }

    /// <summary>
    ///     Converts a <see cref="Exceptional{TValue}" /> to a <see cref="Result{TOk}" />.
    /// </summary>
    /// <param name="exceptional">The exceptional.</param>
    /// <typeparam name="TValue">The value type.</typeparam>
    /// <returns>A <see cref="Result{TOk}" />.</returns>
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public static Result<TValue> ToResult<TValue>(this Exceptional<TValue> exceptional) where TValue : notnull
    {
        var (isValue, value, exception) = exceptional;
        return isValue
            ? value!.ToResult()
            : exception!.ToError().ToResult<TValue>();
    }
}