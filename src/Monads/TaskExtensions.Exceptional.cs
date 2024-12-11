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
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public static async Task<Exceptional<TValueOut>> MapAsync<TValue, TValueOut>(
        this Task<Exceptional<TValue>> task,
        Func<TValue, Task<TValueOut>> mapFunc) where TValue : notnull where TValueOut : notnull
    {
        ArgumentNullException.ThrowIfNull(task);
        ArgumentNullException.ThrowIfNull(mapFunc);

        var (isValue, value, exception) = await task.ConfigureAwait(false);
        return isValue
            ? new Exceptional<TValueOut>(await mapFunc(value!).ConfigureAwait(false))
            : new Exceptional<TValueOut>(exception!);
    }

    /// <summary>
    ///     Async version of the 'Bind' method of <see cref="Exceptional{TValue}" />.
    /// </summary>
    /// <param name="task">The task.</param>
    /// <param name="bindFunc">The binding function.</param>
    /// <typeparam name="TValue">The value type.</typeparam>
    /// <typeparam name="TValueOut">The output value type.</typeparam>
    /// <returns>A <see cref="Task{TResult}" />.</returns>
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public static async Task<Exceptional<TValueOut>> BindAsync<TValue, TValueOut>(
        this Task<Exceptional<TValue>> task,
        Func<TValue, Task<Exceptional<TValueOut>>> bindFunc) where TValue : notnull where TValueOut : notnull
    {
        ArgumentNullException.ThrowIfNull(task);
        ArgumentNullException.ThrowIfNull(bindFunc);

        var (isValue, value, exception) = await task.ConfigureAwait(false);
        return isValue
            ? await bindFunc(value!).ConfigureAwait(false)
            : new Exceptional<TValueOut>(exception!);
    }

    /// <summary>
    ///     Async version of the 'Tap' method of <see cref="Exceptional{TValue}" />.
    /// </summary>
    /// <param name="task">The task.</param>
    /// <param name="valueAction">The 'Value' action.</param>
    /// <param name="exceptionAction">The 'Exception' action.</param>
    /// <typeparam name="TValue">The value type.</typeparam>
    /// <returns>A <see cref="Task{TResult}" />.</returns>
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public static async Task<Exceptional<TValue>> TapAsync<TValue>(
        this Task<Exceptional<TValue>> task,
        Func<TValue, Task> valueAction,
        Func<Exception, Task> exceptionAction) where TValue : notnull
    {
        ArgumentNullException.ThrowIfNull(task);
        ArgumentNullException.ThrowIfNull(valueAction);
        ArgumentNullException.ThrowIfNull(exceptionAction);

        var (state, value, exception) = await task.ConfigureAwait(false);
        if (state)
            await valueAction(value!).ConfigureAwait(false);
        else
            await exceptionAction(exception!).ConfigureAwait(false);

        return await task.ConfigureAwait(false);
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
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public static async Task<TValueOut> MatchAsync<TValue, TValueOut>(
        this Task<Exceptional<TValue>> task,
        Func<TValue, Task<TValueOut>> valueFunc,
        Func<Exception, Task<TValueOut>> exceptionFunc) where TValue : notnull where TValueOut : notnull
    {
        ArgumentNullException.ThrowIfNull(task);
        ArgumentNullException.ThrowIfNull(valueFunc);
        ArgumentNullException.ThrowIfNull(exceptionFunc);

        var (isValue, value, exception) = await task.ConfigureAwait(false);
        return isValue
            ? await valueFunc(value!).ConfigureAwait(false)
            : await exceptionFunc(exception!).ConfigureAwait(false);
    }
}