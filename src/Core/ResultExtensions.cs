namespace SleepingBear.Monad.Core;

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
    public static Result<TOk> ToOk<TOk>(this TOk ok) where TOk : notnull
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
}