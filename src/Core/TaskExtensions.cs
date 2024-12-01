using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace SleepingBear.Monad.Core;

/// <summary>
///     Extension methods for <see cref="Task{TResult}" />.
/// </summary>
public static class TaskExtensions
{
    /// <summary>
    ///     Extension method to convert a value to a <see cref="Task{TResult}" />.
    /// </summary>
    /// <param name="result">The task value.</param>
    /// <typeparam name="TResult">The value type.</typeparam>
    /// <returns>A <see cref="Task{TResult}" />.</returns>
    public static Task<TResult> ToTask<TResult>(this TResult result)
    {
        return Task.FromResult(result);
    }

    /// <summary>
    ///     Async version of the bind method of <see cref="Result{TOk}" />.
    /// </summary>
    /// <param name="task">The task.</param>
    /// <param name="bind">The binding function.</param>
    /// <typeparam name="TOk">The OK type.</typeparam>
    /// <typeparam name="TOkOut">The output OK type.</typeparam>
    /// <returns>A <see cref="Task{TResult}" />.</returns>
    /// <exception cref="InvalidOperationException">Thrown if state is Invalid.</exception>
    /// <exception cref="UnreachableException">Thrown if the state is unknown.</exception>
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public static async Task<Result<TOkOut>> BindAsync<TOk, TOkOut>(
        this Task<Result<TOk>> task,
        Func<TOk, Task<Result<TOkOut>>> bind)
    {
        ArgumentNullException.ThrowIfNull(task);
        ArgumentNullException.ThrowIfNull(bind);

        var result = await task.ConfigureAwait(false);
        var (state, ok, error) = result;
        return state switch
        {
            ResultState.Ok => await bind(ok!).ConfigureAwait(false),
            ResultState.Failure => new Result<TOkOut>(error!),
            ResultState.Invalid => throw new InvalidOperationException(),
            _ => throw new UnreachableException()
        };
    }

    /// <summary>
    ///     Async version of the bind failure method of <see cref="Result{TOk}" />.
    /// </summary>
    /// <param name="task">The task.</param>
    /// <param name="bind">The binding function.</param>
    /// <typeparam name="TOk">The OK type.</typeparam>
    /// <returns>A <see cref="Task{TResult}" />.</returns>
    /// <exception cref="InvalidOperationException">Thrown if state is Invalid.</exception>
    /// <exception cref="UnreachableException">Thrown if the state is unknown.</exception>
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public static async Task<Result<TOk>> BindFailureAsync<TOk>(
        this Task<Result<TOk>> task,
        Func<Error, Task<Result<TOk>>> bind)
    {
        ArgumentNullException.ThrowIfNull(task);
        ArgumentNullException.ThrowIfNull(bind);

        var result = await task.ConfigureAwait(false);
        var (state, _, error) = result;
        return state switch
        {
            ResultState.Ok => result,
            ResultState.Failure => await bind(error!).ConfigureAwait(false),
            ResultState.Invalid => throw new InvalidOperationException(),
            _ => throw new UnreachableException()
        };
    }

    /// <summary>
    ///     Async version of the map method of <see cref="Result{TOk}" />.
    /// </summary>
    /// <param name="task">The task.</param>
    /// <param name="bind">The mapping function.</param>
    /// <typeparam name="TOk">The OK type.</typeparam>
    /// <typeparam name="TOkOut">The output OK type.</typeparam>
    /// <returns>A <see cref="Task{TResult}" />.</returns>
    /// <exception cref="InvalidOperationException">Thrown if state is Invalid.</exception>
    /// <exception cref="UnreachableException">Thrown if the state is unknown.</exception>
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public static async Task<Result<TOkOut>> MapAsync<TOk, TOkOut>(
        this Task<Result<TOk>> task,
        Func<TOk, Task<TOkOut>> bind)
    {
        ArgumentNullException.ThrowIfNull(task);
        ArgumentNullException.ThrowIfNull(bind);

        var result = await task.ConfigureAwait(false);
        var (state, ok, error) = result;
        return state switch
        {
            ResultState.Ok => new Result<TOkOut>(await bind(ok!).ConfigureAwait(false)),
            ResultState.Failure => new Result<TOkOut>(error!),
            ResultState.Invalid => throw new InvalidOperationException(),
            _ => throw new UnreachableException()
        };
    }

    /// <summary>
    ///     Async version of the map failure method of <see cref="Result{TOk}" />.
    /// </summary>
    /// <param name="task">The task.</param>
    /// <param name="bind">The mapping function.</param>
    /// <typeparam name="TOk">The OK type.</typeparam>
    /// <returns>A <see cref="Task{TResult}" />.</returns>
    /// <exception cref="InvalidOperationException">Thrown if state is Invalid.</exception>
    /// <exception cref="UnreachableException">Thrown if the state is unknown.</exception>
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public static async Task<Result<TOk>> MapFailureAsync<TOk>(
        this Task<Result<TOk>> task,
        Func<Error, Task<Error>> bind)
    {
        ArgumentNullException.ThrowIfNull(task);
        ArgumentNullException.ThrowIfNull(bind);

        var result = await task.ConfigureAwait(false);
        var (state, _, error) = result;
        return state switch
        {
            ResultState.Ok => result,
            ResultState.Failure => new Result<TOk>(await bind(error!).ConfigureAwait(false)),
            ResultState.Invalid => throw new InvalidOperationException(),
            _ => throw new UnreachableException()
        };
    }

    /// <summary>
    ///     Async version of the match method of <see cref="Result{TOk}" />.
    /// </summary>
    /// <param name="task">The task.</param>
    /// <param name="okFunc">The OK function.</param>
    /// <param name="errorFunc">The error function.</param>
    /// <typeparam name="TOk">The OK type.</typeparam>
    /// <typeparam name="TOut">The output type.</typeparam>
    /// <returns>A <see cref="Task{TResult}" />.</returns>
    /// <exception cref="InvalidOperationException">Thrown if state is Invalid.</exception>
    /// <exception cref="UnreachableException">Thrown if the state is unknown.</exception>
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public static async Task<TOut> MatchAsync<TOk, TOut>(
        this Task<Result<TOk>> task,
        Func<TOk, Task<TOut>> okFunc,
        Func<Error, Task<TOut>> errorFunc)
    {
        ArgumentNullException.ThrowIfNull(task);
        ArgumentNullException.ThrowIfNull(okFunc);
        ArgumentNullException.ThrowIfNull(errorFunc);

        var result = await task.ConfigureAwait(false);
        var (state, ok, error) = result;
        return state switch
        {
            ResultState.Ok => await okFunc(ok!).ConfigureAwait(false),
            ResultState.Failure => await errorFunc(error!).ConfigureAwait(false),
            ResultState.Invalid => throw new InvalidOperationException(),
            _ => throw new UnreachableException()
        };
    }
}