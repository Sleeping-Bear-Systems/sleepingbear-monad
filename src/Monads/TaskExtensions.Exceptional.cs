using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace SleepingBear.Monad.Monads;

/// <summary>
///     Extension methods for <see cref="Task{TResult}" />.
/// </summary>
public static partial class TaskExtensions
{
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