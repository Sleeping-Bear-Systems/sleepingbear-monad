using System.Collections.Immutable;
using SleepingBear.Monad.Errors;

namespace SleepingBear.Monad.Monads;

/// <summary>
///     Extension methods for <see cref="Validation{TValue}" />.
/// </summary>
public static class ValidationExtensions
{
    /// <summary>
    ///     Wraps a value in a <see cref="Validation{TValue}" />.
    /// </summary>
    /// <param name="value">The value being validated.</param>
    /// <typeparam name="TValue">The value type.</typeparam>
    /// <returns>A <see cref="Validation{TValue}" /> instance.</returns>
    public static Validation<TValue> ToValidation<TValue>(this TValue value) where TValue : notnull
    {
        return new Validation<TValue>(value);
    }

    /// <summary>
    ///     Wraps a collection of errors in a <see cref="Validation{TValue}" />.
    /// </summary>
    /// <param name="errors">The <see cref="ImmutableList{T}" /> of errors.</param>
    /// <typeparam name="TValue">The value type.</typeparam>
    /// <returns>A <see cref="Validation{TValue}" /> instance.</returns>
    public static Validation<TValue> ToValidation<TValue>(this ImmutableList<Error>? errors) where TValue : notnull
    {
        return new Validation<TValue>(errors);
    }
}