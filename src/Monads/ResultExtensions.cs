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
    /// <param name="ok">The value being lifted.</param>
    /// <typeparam name="TOk">The type of the value being lifted.</typeparam>
    /// <returns>A <see cref="Result{TOk}" />.</returns>
    public static Result<TOk> ToResultOk<TOk>(this TOk ok) where TOk : notnull
    {
        return new Result<TOk>(ok);
    }

    /// <summary>
    ///     Converts a <see cref="Error" /> to a <see cref="Result{TOk}" />.
    /// </summary>
    /// <param name="error">The error value.</param>
    /// <typeparam name="TOk">The OK type.</typeparam>
    /// <returns>A <see cref="Result{TOk}" />.</returns>
    public static Result<TOk> ToResultError<TOk>(this Error error) where TOk : notnull
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
    public static Result<TOk> Where<TOk>(
        this Result<TOk> result,
        Func<TOk, bool> predicate,
        Func<TOk, Error> errorFunc) where TOk : notnull
    {
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNull(errorFunc);

        return result.Try(out var ok)
            ? predicate(ok)
                ? result
                : errorFunc(ok)
            : result;
    }

    /// <summary>
    ///     Conditionally binds a <see cref="Result{TOk}" />.
    /// </summary>
    /// <param name="result">The result being bound.</param>
    /// <param name="predicate">The predicate.</param>
    /// <param name="bindFunc">The binding function.</param>
    /// <typeparam name="TOk">The input OK type.</typeparam>
    /// <returns>A <see cref="Result{TOk}" /> containing the bound value.</returns>
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
                ? mapFunc(ok)
                : result
            : result;
    }


    /// <summary>
    ///     Taps a <see cref="Result{TOk}" />.
    /// </summary>
    /// <param name="result">The result being tapped.</param>
    /// <param name="okAction">The OK action.</param>
    /// <param name="errorAction">The error action.</param>
    /// <returns>The result</returns>
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public static Result<TOk> Tap<TOk>(
        this Result<TOk> result,
        Action<TOk> okAction,
        Action<Error> errorAction) where TOk : notnull
    {
        ArgumentNullException.ThrowIfNull(okAction);
        ArgumentNullException.ThrowIfNull(errorAction);

        var (isOk, ok, error) = result;
        if (isOk)
            okAction(ok!);
        else
            errorAction(error!);

        return result;
    }

    /// <summary>
    ///     Tries to return the 'OK' value.
    /// </summary>
    /// <param name="result">The result.</param>
    /// <param name="ok">The 'OK' value.</param>
    /// <returns>True if OK, false otherwise.</returns>
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public static bool Try<TOk>(this Result<TOk> result, [NotNullWhen(true)] out TOk? ok) where TOk : notnull
    {
        var (isOk, ok2, _) = result;
        ok = isOk
            ? ok2!
            : default;
        return isOk;
    }

    /// <summary>
    ///     Tries to get the 'Error' value.
    /// </summary>
    /// <param name="result">The result.</param>
    /// <param name="error">The 'Error' value.</param>
    /// <returns>True if error, false otherwise.</returns>
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public static bool TryError<TOk>(
        this Result<TOk> result,
        [NotNullWhen(true)] out Error? error) where TOk : notnull
    {
        var (isOk, _, error2) = result;
        error = isOk
            ? null
            : error2!;
        return !isOk;
    }

    /// <summary>
    ///     Map a <see cref="Result{TOk}" />.
    /// </summary>
    /// <param name="result">The result being mapped.</param>
    /// <param name="mapFunc">The mapping function.</param>
    /// <typeparam name="TOk">The input OK type.</typeparam>
    /// <typeparam name="TOkOut">The output OK type.</typeparam>
    /// <returns>A <see cref="Result{TOk}" />.</returns>
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public static Result<TOkOut> Map<TOk, TOkOut>(
        this Result<TOk> result,
        Func<TOk, TOkOut> mapFunc) where TOk : notnull where TOkOut : notnull
    {
        ArgumentNullException.ThrowIfNull(mapFunc);

        var (isOk, ok, error) = result;
        return isOk
            ? mapFunc(ok!)
            : error!;
    }

    /// <summary>
    ///     Map a <see cref="Result{TOk}" />.
    /// </summary>
    /// <param name="result">The result being mapped.</param>
    /// <param name="mapErrorFunc">The mapping function.</param>
    /// <returns>A <see cref="Result{TOk}" />.</returns>
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public static Result<TOk> MapError<TOk>(
        this Result<TOk> result,
        Func<Error, Error> mapErrorFunc) where TOk : notnull
    {
        ArgumentNullException.ThrowIfNull(mapErrorFunc);

        var (isOk, ok, error) = result;
        return isOk
            ? ok!
            : mapErrorFunc(error!);
    }

    /// <summary>
    ///     Binds a <see cref="Result{TOk}" />.
    /// </summary>
    /// <param name="result">The result being bound.</param>
    /// <param name="bindFunc">The binding function.</param>
    /// <typeparam name="TOk">The input OK type.</typeparam>
    /// <typeparam name="TOkOut">The output OK type.</typeparam>
    /// <returns>A <see cref="Result{TOk}" />.</returns>
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public static Result<TOkOut> Bind<TOk, TOkOut>(
        this Result<TOk> result,
        Func<TOk, Result<TOkOut>> bindFunc) where TOk : notnull where TOkOut : notnull
    {
        ArgumentNullException.ThrowIfNull(bindFunc);

        var (isOk, ok, error) = result;
        return isOk
            ? bindFunc(ok!)
            : error!;
    }

    /// <summary>
    ///     Bind a <see cref="Result{TOk}" />.
    /// </summary>
    /// <param name="result">The result being bound.</param>
    /// <param name="bindFunc">The binding function.</param>
    /// <returns>A <see cref="Result{TOk}" />.</returns>
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public static Result<TOk> BindError<TOk>(
        this Result<TOk> result,
        Func<Error, Result<TOk>> bindFunc) where TOk : notnull
    {
        ArgumentNullException.ThrowIfNull(bindFunc);

        var (isOk, ok, error) = result;
        return isOk
            ? ok!
            : bindFunc(error!);
    }

    /// <summary>
    ///     Matches the <see cref="Result{TOk}" />.
    /// </summary>
    /// <param name="result">The result being matched.</param>
    /// <param name="okFunc">The OK function.</param>
    /// <param name="errorFunc">The error function.</param>
    /// <typeparam name="TOk">The value of the lifted type.</typeparam>
    /// <typeparam name="TOut">The output type.</typeparam>
    /// <returns>The matched value.</returns>
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public static TOut Match<TOk, TOut>(
        this Result<TOk> result,
        Func<TOk, TOut> okFunc, Func<Error, TOut> errorFunc) where TOk : notnull
    {
        ArgumentNullException.ThrowIfNull(okFunc);
        ArgumentNullException.ThrowIfNull(errorFunc);

        var (isOk, ok, error) = result;
        return isOk
            ? okFunc(ok!)
            : errorFunc(error!);
    }
}