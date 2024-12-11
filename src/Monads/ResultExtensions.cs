using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
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

    /// <summary>
    ///     Conditionally binds a <see cref="Result{TOk}" />.
    /// </summary>
    /// <param name="result">The result being bound.</param>
    /// <param name="predicate">The predicate.</param>
    /// <param name="bindFunc">The binding function.</param>
    /// <typeparam name="TOk">The input OK type.</typeparam>
    /// <returns>A <see cref="Result{TOk}" /> containing the bound value..</returns>
    /// <exception cref="InvalidOperationException">Thrown if the result's state is invalid.</exception>
    /// <exception cref="UnreachableException">Thrown if the result's state is unknown.</exception>
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public static Result<TOk> BindIf<TOk>(
        this Result<TOk> result,
        Func<TOk, bool> predicate,
        Func<TOk, Result<TOk>> bindFunc) where TOk : notnull
    {
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNull(bindFunc);

        return result.Try(out var ok)
            ? predicate(ok)
                ? bindFunc(ok)
                : result
            : result;
    }

    /// <summary>
    ///     Conditionally maps a <see cref="Result{TOk}" />.
    /// </summary>
    /// <param name="result">The result being mapped.</param>
    /// <param name="predicate">The predicate.</param>
    /// <param name="mapFunc">The mapping function.</param>
    /// <typeparam name="TOk">The lifted value type.</typeparam>
    /// <returns>A <see cref="Result{TOk}" /> containing the mapped value.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the result's state is invalid.</exception>
    /// <exception cref="UnreachableException">Thrown if the result's state is unknown.</exception>
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public static Result<TOk> MapIf<TOk>(
        this Result<TOk> result,
        Func<TOk, bool> predicate,
        Func<TOk, TOk> mapFunc) where TOk : notnull
    {
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNull(mapFunc);

        return result.Try(out var ok)
            ? predicate(ok)
                ? mapFunc(ok).ToResult()
                : result
            : result;
    }
}