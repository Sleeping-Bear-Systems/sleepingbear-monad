namespace SleepingBear.Monad.Core;

/// <summary>
///     Extension methods for piping.
/// </summary>
public static class PipeExtensions
{
    /// <summary>
    ///     Extension method for piping.
    /// </summary>
    /// <param name="input">The input value.</param>
    /// <param name="pipeFunc">The pipe function.</param>
    /// <typeparam name="T1">The input type.</typeparam>
    /// <typeparam name="TOutput">The output type.</typeparam>
    /// <returns>The output value.</returns>
    public static TOutput Pipe<T1, TOutput>(this T1 input, Func<T1, TOutput> pipeFunc)
    {
        ArgumentNullException.ThrowIfNull(pipeFunc);
        return pipeFunc(input);
    }

    /// <summary>
    ///     Asynchronous extension method for piping.
    /// </summary>
    /// <param name="task">The <see cref="Task" /> wrapping the input value.</param>
    /// <param name="pipeFunc">The pipe function.</param>
    /// <typeparam name="T1">The input value type.</typeparam>
    /// <typeparam name="TOutput">The output value type.</typeparam>
    /// <returns>A <see cref="Task{TResult}" /> wrapping the output value.</returns>
    public static async Task<TOutput> PipeAsync<T1, TOutput>(
        this Task<T1> task,
        Func<T1, TOutput> pipeFunc)
    {
        ArgumentNullException.ThrowIfNull(task);
        ArgumentNullException.ThrowIfNull(pipeFunc);

        var input = await task.ConfigureAwait(false);
        return pipeFunc(input);
    }

    /// <summary>
    ///     Asynchronous extension method for piping.
    /// </summary>
    /// <param name="task">The <see cref="Task" /> wrapping the input value.</param>
    /// <param name="pipeFunc">The pipe function.</param>
    /// <typeparam name="T1">The input value type.</typeparam>
    /// <typeparam name="TOutput">The output value type.</typeparam>
    /// <returns>A <see cref="Task{TResult}" /> wrapping the output value.</returns>
    public static async Task<TOutput> PipeAsync<T1, TOutput>(
        this Task<T1> task,
        Func<T1, Task<TOutput>> pipeFunc)
    {
        ArgumentNullException.ThrowIfNull(task);
        ArgumentNullException.ThrowIfNull(pipeFunc);

        var input = await task.ConfigureAwait(false);
        return await pipeFunc(input).ConfigureAwait(false);
    }
}