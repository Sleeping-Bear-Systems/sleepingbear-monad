using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using SleepingBear.Monad.Errors;

namespace SleepingBear.Monad.Monads;

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
        Func<TOk, Task<Result<TOkOut>>> bind) where TOk : notnull where TOkOut : notnull
    {
        ArgumentNullException.ThrowIfNull(task);
        ArgumentNullException.ThrowIfNull(bind);

        var result = await task.ConfigureAwait(false);
        var (state, ok, error) = result;
        return state switch
        {
            ResultState.Ok => await bind(ok!).ConfigureAwait(false),
            ResultState.Error => new Result<TOkOut>(error!),
            ResultState.Invalid => throw new InvalidOperationException(),
            _ => throw new UnreachableException()
        };
    }

    /// <summary>
    ///     Async version of the bind error method of <see cref="Result{TOk}" />.
    /// </summary>
    /// <param name="task">The task.</param>
    /// <param name="bind">The binding function.</param>
    /// <typeparam name="TOk">The OK type.</typeparam>
    /// <returns>A <see cref="Task{TResult}" />.</returns>
    /// <exception cref="InvalidOperationException">Thrown if state is Invalid.</exception>
    /// <exception cref="UnreachableException">Thrown if the state is unknown.</exception>
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public static async Task<Result<TOk>> BindErrorAsync<TOk>(
        this Task<Result<TOk>> task,
        Func<Error, Task<Result<TOk>>> bind) where TOk : notnull
    {
        ArgumentNullException.ThrowIfNull(task);
        ArgumentNullException.ThrowIfNull(bind);

        var result = await task.ConfigureAwait(false);
        var (state, _, error) = result;
        return state switch
        {
            ResultState.Ok => result,
            ResultState.Error => await bind(error!).ConfigureAwait(false),
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
        Func<TOk, Task<TOkOut>> bind) where TOk : notnull where TOkOut : notnull
    {
        ArgumentNullException.ThrowIfNull(task);
        ArgumentNullException.ThrowIfNull(bind);

        var result = await task.ConfigureAwait(false);
        var (state, ok, error) = result;
        return state switch
        {
            ResultState.Ok => new Result<TOkOut>(await bind(ok!).ConfigureAwait(false)),
            ResultState.Error => new Result<TOkOut>(error!),
            ResultState.Invalid => throw new InvalidOperationException(),
            _ => throw new UnreachableException()
        };
    }

    /// <summary>
    ///     Async version of the map error method of <see cref="Result{TOk}" />.
    /// </summary>
    /// <param name="task">The task.</param>
    /// <param name="bind">The mapping function.</param>
    /// <typeparam name="TOk">The OK type.</typeparam>
    /// <returns>A <see cref="Task{TResult}" />.</returns>
    /// <exception cref="InvalidOperationException">Thrown if state is Invalid.</exception>
    /// <exception cref="UnreachableException">Thrown if the state is unknown.</exception>
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public static async Task<Result<TOk>> MapErrorAsync<TOk>(
        this Task<Result<TOk>> task,
        Func<Error, Task<Error>> bind) where TOk : notnull
    {
        ArgumentNullException.ThrowIfNull(task);
        ArgumentNullException.ThrowIfNull(bind);

        var result = await task.ConfigureAwait(false);
        var (state, _, error) = result;
        return state switch
        {
            ResultState.Ok => result,
            ResultState.Error => new Result<TOk>(await bind(error!).ConfigureAwait(false)),
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
        Func<Error, Task<TOut>> errorFunc) where TOk : notnull
    {
        ArgumentNullException.ThrowIfNull(task);
        ArgumentNullException.ThrowIfNull(okFunc);
        ArgumentNullException.ThrowIfNull(errorFunc);

        var result = await task.ConfigureAwait(false);
        var (state, ok, error) = result;
        return state switch
        {
            ResultState.Ok => await okFunc(ok!).ConfigureAwait(false),
            ResultState.Error => await errorFunc(error!).ConfigureAwait(false),
            ResultState.Invalid => throw new InvalidOperationException(),
            _ => throw new UnreachableException()
        };
    }

    /// <summary>
    ///     Async version of the 'Tap' method of <see cref="Result{TOk}" />.
    /// </summary>
    /// <param name="task">The task.</param>
    /// <param name="okFunc">The 'OK' action.</param>
    /// <param name="errorFunc">The 'Error' action.</param>
    /// <typeparam name="TOk">The OK type.</typeparam>
    /// <returns>A <see cref="Task{TResult}" />.</returns>
    /// <exception cref="InvalidOperationException">Thrown if state is Invalid.</exception>
    /// <exception cref="UnreachableException">Thrown if the state is unknown.</exception>
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public static async Task<Result<TOk>> TapAsync<TOk>(
        this Task<Result<TOk>> task,
        Func<TOk, Task> okFunc,
        Func<Error, Task> errorFunc) where TOk : notnull
    {
        ArgumentNullException.ThrowIfNull(task);
        ArgumentNullException.ThrowIfNull(okFunc);
        ArgumentNullException.ThrowIfNull(errorFunc);

        var result = await task.ConfigureAwait(false);
        var (state, ok, error) = result;
        switch (state)
        {
            case ResultState.Ok:
                await okFunc(ok!).ConfigureAwait(false);
                break;
            case ResultState.Error:
                await errorFunc(error!).ConfigureAwait(false);
                break;
            case ResultState.Invalid:
                throw new InvalidOperationException();
            default:
                throw new UnreachableException();
        }

        return result;
    }

    /// <summary>
    ///     Async version of the Map method for <see cref="Exceptional{TValue}" />.
    /// </summary>
    /// <param name="task">The task.</param>
    /// <param name="mapFunc">The mapping function.</param>
    /// <typeparam name="TValue">The value type.</typeparam>
    /// <typeparam name="TValueOut">The output value type.</typeparam>
    /// <returns>A <see cref="Task{TResult}" />.</returns>
    /// <exception cref="InvalidOperationException">Thrown if state is Invalid.</exception>
    /// <exception cref="UnreachableException">Thrown if the state is unknown.</exception>
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public static async Task<Exceptional<TValueOut>> MapAsync<TValue, TValueOut>(
        this Task<Exceptional<TValue>> task,
        Func<TValue, Task<TValueOut>> mapFunc) where TValue : notnull where TValueOut : notnull
    {
        ArgumentNullException.ThrowIfNull(task);
        ArgumentNullException.ThrowIfNull(mapFunc);

        var exceptional = await task.ConfigureAwait(false);
        var (state, value, exception) = exceptional;
        return state switch
        {
            ExceptionalState.Value => new Exceptional<TValueOut>(await mapFunc(value!).ConfigureAwait(false)),
            ExceptionalState.Exception => new Exceptional<TValueOut>(exception!),
            ExceptionalState.Invalid => throw new InvalidOperationException(),
            _ => throw new UnreachableException()
        };
    }

    /// <summary>
    ///     Async version of the 'Bind' method of <see cref="Exceptional{TValue}" />.
    /// </summary>
    /// <param name="task">The task.</param>
    /// <param name="bindFunc">The binding function.</param>
    /// <typeparam name="TValue">The value type.</typeparam>
    /// <typeparam name="TValueOut">The output value type.</typeparam>
    /// <returns>A <see cref="Task{TResult}" />.</returns>
    /// <exception cref="InvalidOperationException">Thrown if state is Invalid.</exception>
    /// <exception cref="UnreachableException">Thrown if the state is unknown.</exception>
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public static async Task<Exceptional<TValueOut>> BindAsync<TValue, TValueOut>(
        this Task<Exceptional<TValue>> task,
        Func<TValue, Task<Exceptional<TValueOut>>> bindFunc) where TValue : notnull where TValueOut : notnull
    {
        ArgumentNullException.ThrowIfNull(task);
        ArgumentNullException.ThrowIfNull(bindFunc);

        var exceptional = await task.ConfigureAwait(false);
        var (state, value, exception) = exceptional;
        return state switch
        {
            ExceptionalState.Value => await bindFunc(value!).ConfigureAwait(false),
            ExceptionalState.Exception => new Exceptional<TValueOut>(exception!),
            ExceptionalState.Invalid => throw new InvalidOperationException(),
            _ => throw new UnreachableException()
        };
    }

    /// <summary>
    ///     Async version of the 'Tap' method of <see cref="Exceptional{TValue}" />.
    /// </summary>
    /// <param name="task">The task.</param>
    /// <param name="valueAction">The 'Value' action.</param>
    /// <param name="exceptionAction">The 'Exception' action.</param>
    /// <typeparam name="TValue">The value type.</typeparam>
    /// <returns>A <see cref="Task{TResult}" />.</returns>
    /// <exception cref="InvalidOperationException">Thrown if state is Invalid.</exception>
    /// <exception cref="UnreachableException">Thrown if the state is unknown.</exception>
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public static async Task<Exceptional<TValue>> TapAsync<TValue>(
        this Task<Exceptional<TValue>> task,
        Func<TValue, Task> valueAction,
        Func<Exception, Task> exceptionAction) where TValue : notnull
    {
        ArgumentNullException.ThrowIfNull(task);
        ArgumentNullException.ThrowIfNull(valueAction);
        ArgumentNullException.ThrowIfNull(exceptionAction);

        var exceptional = await task.ConfigureAwait(false);
        var (state, value, exception) = exceptional;
        switch (state)
        {
            case ExceptionalState.Value:
                await valueAction(value!).ConfigureAwait(false);
                break;
            case ExceptionalState.Invalid:
                await exceptionAction(exception!).ConfigureAwait(false);
                break;
            case ExceptionalState.Exception:
                throw new InvalidOperationException();
            default:
                throw new UnreachableException();
        }

        return exceptional;
    }

    /// <summary>
    ///     Async version of the 'Match' method of <see cref="Exceptional{TValue}" />.
    /// </summary>
    /// <param name="task">The task.</param>
    /// <param name="valueFunc">The 'Value' function.</param>
    /// <param name="exceptionFunc">The 'Exception' function.</param>
    /// <typeparam name="TValue">The 'Value' type.</typeparam>
    /// <typeparam name="TValueOut">The output 'Value' type.</typeparam>
    /// <returns>A <see cref="Task{TResult}" />.</returns>
    /// <exception cref="InvalidOperationException">Thrown if state is Invalid.</exception>
    /// <exception cref="UnreachableException">Thrown if the state is unknown.</exception>
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public static async Task<TValueOut> MatchAsync<TValue, TValueOut>(
        this Task<Exceptional<TValue>> task,
        Func<TValue, Task<TValueOut>> valueFunc,
        Func<Exception, Task<TValueOut>> exceptionFunc) where TValue : notnull where TValueOut : notnull
    {
        ArgumentNullException.ThrowIfNull(task);
        ArgumentNullException.ThrowIfNull(valueFunc);
        ArgumentNullException.ThrowIfNull(exceptionFunc);

        var exceptional = await task.ConfigureAwait(false);
        var (state, value, exception) = exceptional;
        return state switch
        {
            ExceptionalState.Value => await valueFunc(value!).ConfigureAwait(false),
            ExceptionalState.Invalid => await exceptionFunc(exception!).ConfigureAwait(false),
            ExceptionalState.Exception => throw new InvalidOperationException(),
            _ => throw new UnreachableException()
        };
    }
}