namespace SleepingBear.Monad.Tasks;

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
    ///     Maps a <see cref="Task{TValue}" /> to a <see cref="Task{TValueOut}" />.
    /// </summary>
    /// <param name="task">The task being mapped.</param>
    /// <param name="mapFunc">The mapping function.</param>
    /// <typeparam name="TValue">The type of the value wrapped in the <see cref="Task{TResult}" />.</typeparam>
    /// <typeparam name="TValueOut">The type of the value wrapped in the <see cref="Task{TResult}" /> being returned.</typeparam>
    /// <returns>A <see cref="Task{TResult}" /> containing the mapped value.</returns>
    public static async Task<TValueOut> Map<TValue, TValueOut>(
        this Task<TValue> task,
        Func<TValue, TValueOut> mapFunc)
    {
        ArgumentNullException.ThrowIfNull(task);
        ArgumentNullException.ThrowIfNull(mapFunc);

        var value = await task.ConfigureAwait(false);
        return mapFunc(value);
    }

    /// <summary>
    ///     Binds a <see cref="Task{TResult}" /> to a <see cref="Task{TResult}" />.
    /// </summary>
    /// <param name="task">The task being mapped.</param>
    /// <param name="bindFunc">The binding function.</param>
    /// <typeparam name="TValue">The type of the value wrapped in the <see cref="Task{TResult}" />.</typeparam>
    /// <typeparam name="TValueOut">The type of the value wrapped in the <see cref="Task{TResult}" /> being returned.</typeparam>
    /// <returns>A <see cref="Task{TResult}" /> containing the bound value.</returns>
    public static async Task<TValueOut> Bind<TValue, TValueOut>(
        this Task<TValue> task,
        Func<TValue, Task<TValueOut>> bindFunc)
    {
        ArgumentNullException.ThrowIfNull(task);
        ArgumentNullException.ThrowIfNull(bindFunc);

        var value = await task.ConfigureAwait(false);
        return await bindFunc(value).ConfigureAwait(false);
    }
}
