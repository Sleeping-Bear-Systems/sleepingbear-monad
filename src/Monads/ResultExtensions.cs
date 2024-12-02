using SleepingBear.Monad.Errors;

namespace SleepingBear.Monad.Monads;

/// <summary>
///     Extension methods for <see cref="Result{TOk}" />.
/// </summary>
public static class ResultExtensions
{
    /// <summary>
    ///     Converts a value to a <see cref="Result{TOk}" />.
    /// </summary>
    /// <param name="ok">The OK value.</param>
    /// <typeparam name="TOk">The OK type.</typeparam>
    /// <returns>A <see cref="Result{TOk}" />.</returns>
    public static Result<TOk> ToResult<TOk>(this TOk ok) where TOk : notnull
    {
        return new Result<TOk>(ok);
    }

    /// <summary>
    ///     Converts a <see cref="Error" /> to a <see cref="Result{TOk}" />.
    /// </summary>
    /// <param name="error">The error value.</param>
    /// <typeparam name="TOk">The OK type.</typeparam>
    /// <returns>A <see cref="Result{TOk}" />.</returns>
    public static Result<TOk> ToResult<TOk>(this Error error) where TOk : notnull
    {
        return new Result<TOk>(error);
    }

    /// <summary>
    ///     Filters a <see cref="Result{TOk}" />.
    /// </summary>
    /// <param name="result">The result instance.</param>
    /// <param name="predicate">The filter predicate.</param>
    /// <param name="errorFunc">The error function.</param>
    /// <typeparam name="TOk">The OK type.</typeparam>
    /// <returns>The result instance.</returns>
    public static Result<TOk> Where<TOk>(this Result<TOk> result, Func<TOk, bool> predicate,
        Func<TOk, Error> errorFunc) where TOk : notnull
    {
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNull(errorFunc);

        if (result.Try(out var some))
        {
            return predicate(some) ? result : errorFunc(some).ToResult<TOk>();
        }

        return result;
    }
}