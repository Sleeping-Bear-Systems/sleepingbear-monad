using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using SleepingBear.Monad.Errors;

namespace SleepingBear.Monad.Monads;

/// <summary>
///     Extension methods for <see cref="Task{TResult}" />.
/// </summary>
public static partial class TaskExtensions
{
    /// <summary>
    ///     Async version of the bind method of <see cref="Result{TOk}" />.
    /// </summary>
    /// <param name="task">The task.</param>
    /// <param name="bindFunc">The binding function.</param>
    /// <typeparam name="TOk">The OK type.</typeparam>
    /// <typeparam name="TOkOut">The output OK type.</typeparam>
    /// <returns>A <see cref="Task{TResult}" />.</returns>
    /// <exception cref="InvalidOperationException">Thrown if state is Invalid.</exception>
    /// <exception cref="UnreachableException">Thrown if the state is unknown.</exception>
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public static async Task<Result<TOkOut>> BindAsync<TOk, TOkOut>(
        this Task<Result<TOk>> task,
        Func<TOk, Task<Result<TOkOut>>> bindFunc) where TOk : notnull where TOkOut : notnull
    {
        ArgumentNullException.ThrowIfNull(task);
        ArgumentNullException.ThrowIfNull(bindFunc);
        
        var result = await task.ConfigureAwait(false);
        var (state, ok, error) = result;
        return state switch
        {
            ResultState.Ok => await bindFunc(ok!).ConfigureAwait(false),
            ResultState.Error => new Result<TOkOut>(error!),
            ResultState.Invalid => throw new InvalidOperationException(),
            _ => throw new UnreachableException()
        };
    }

    /// <summary>
    ///     Async version of the bind method of <see cref="Result{TOk}" />.
    /// </summary>
    /// <param name="task">The task.</param>
    /// <param name="bindFunc">The binding function.</param>
    /// <typeparam name="TOk">The OK type.</typeparam>
    /// <typeparam name="TOkOut">The output OK type.</typeparam>
    /// <returns>A <see cref="Task{TResult}" />.</returns>
    /// <exception cref="InvalidOperationException">Thrown if state is Invalid.</exception>
    /// <exception cref="UnreachableException">Thrown if the state is unknown.</exception>
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public static async Task<Result<TOkOut>> BindAsync<TOk, TOkOut>(
        this Task<Result<TOk>> task,
        Func<TOk, Result<TOkOut>> bindFunc) where TOk : notnull where TOkOut : notnull
    {
        ArgumentNullException.ThrowIfNull(task);
        ArgumentNullException.ThrowIfNull(bindFunc);
        
        var result = await task.ConfigureAwait(false);
        var (state, ok, error) = result;
        return state switch
        {
            ResultState.Ok => bindFunc(ok!),
            ResultState.Error => new Result<TOkOut>(error!),
            ResultState.Invalid => throw new InvalidOperationException(),
            _ => throw new UnreachableException()
        };
    }
    
    /// <summary>
    ///     Async version of the bind error method of <see cref="Result{TOk}" />.
    /// </summary>
    /// <param name="task">The task.</param>
    /// <param name="bindFunc">The binding function.</param>
    /// <typeparam name="TOk">The OK type.</typeparam>
    /// <returns>A <see cref="Task{TResult}" />.</returns>
    /// <exception cref="InvalidOperationException">Thrown if state is Invalid.</exception>
    /// <exception cref="UnreachableException">Thrown if the state is unknown.</exception>
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public static async Task<Result<TOk>> BindErrorAsync<TOk>(
        this Task<Result<TOk>> task,
        Func<Error, Task<Result<TOk>>> bindFunc) where TOk : notnull
    {
        ArgumentNullException.ThrowIfNull(task);
        ArgumentNullException.ThrowIfNull(bindFunc);

        var result = await task.ConfigureAwait(false);
        var (state, _, error) = result;
        return state switch
        {
            ResultState.Ok => result,
            ResultState.Error => await bindFunc(error!).ConfigureAwait(false),
            ResultState.Invalid => throw new InvalidOperationException(),
            _ => throw new UnreachableException()
        };
    }

    /// <summary>
    ///     Async version of the bind error method of <see cref="Result{TOk}" />.
    /// </summary>
    /// <param name="task">The task.</param>
    /// <param name="bindFunc">The binding function.</param>
    /// <typeparam name="TOk">The OK type.</typeparam>
    /// <returns>A <see cref="Task{TResult}" />.</returns>
    /// <exception cref="InvalidOperationException">Thrown if state is Invalid.</exception>
    /// <exception cref="UnreachableException">Thrown if the state is unknown.</exception>
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public static async Task<Result<TOk>> BindErrorAsync<TOk>(
        this Task<Result<TOk>> task,
        Func<Error, Result<TOk>> bindFunc) where TOk : notnull
    {
        ArgumentNullException.ThrowIfNull(task);
        ArgumentNullException.ThrowIfNull(bindFunc);

        var result = await task.ConfigureAwait(false);
        var (state, _, error) = result;
        return state switch
        {
            ResultState.Ok => result,
            ResultState.Error => bindFunc(error!),
            ResultState.Invalid => throw new InvalidOperationException(),
            _ => throw new UnreachableException()
        };
    }
    
    /// <summary>
    ///     Async version of the map method of <see cref="Result{TOk}" />.
    /// </summary>
    /// <param name="task">The task.</param>
    /// <param name="mapFunc">The mapping function.</param>
    /// <typeparam name="TOk">The OK type.</typeparam>
    /// <typeparam name="TOkOut">The output OK type.</typeparam>
    /// <returns>A <see cref="Task{TResult}" />.</returns>
    /// <exception cref="InvalidOperationException">Thrown if state is Invalid.</exception>
    /// <exception cref="UnreachableException">Thrown if the state is unknown.</exception>
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public static async Task<Result<TOkOut>> MapAsync<TOk, TOkOut>(
        this Task<Result<TOk>> task,
        Func<TOk, Task<TOkOut>> mapFunc) where TOk : notnull where TOkOut : notnull
    {
        ArgumentNullException.ThrowIfNull(task);
        ArgumentNullException.ThrowIfNull(mapFunc);

        var result = await task.ConfigureAwait(false);
        var (state, ok, error) = result;
        return state switch
        {
            ResultState.Ok => new Result<TOkOut>(await mapFunc(ok!).ConfigureAwait(false)),
            ResultState.Error => new Result<TOkOut>(error!),
            ResultState.Invalid => throw new InvalidOperationException(),
            _ => throw new UnreachableException()
        };
    }

    /// <summary>
    ///     Async version of the map method of <see cref="Result{TOk}" />.
    /// </summary>
    /// <param name="task">The task.</param>
    /// <param name="mapFunc">The mapping function.</param>
    /// <typeparam name="TOk">The OK type.</typeparam>
    /// <typeparam name="TOkOut">The output OK type.</typeparam>
    /// <returns>A <see cref="Task{TResult}" />.</returns>
    /// <exception cref="InvalidOperationException">Thrown if state is Invalid.</exception>
    /// <exception cref="UnreachableException">Thrown if the state is unknown.</exception>
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public static async Task<Result<TOkOut>> MapAsync<TOk, TOkOut>(
        this Task<Result<TOk>> task,
        Func<TOk, TOkOut> mapFunc) where TOk : notnull where TOkOut : notnull
    {
        ArgumentNullException.ThrowIfNull(task);
        ArgumentNullException.ThrowIfNull(mapFunc);

        var result = await task.ConfigureAwait(false);
        var (state, ok, error) = result;
        return state switch
        {
            ResultState.Ok => new Result<TOkOut>(mapFunc(ok!)),
            ResultState.Error => new Result<TOkOut>(error!),
            ResultState.Invalid => throw new InvalidOperationException(),
            _ => throw new UnreachableException()
        };
    }
    
    /// <summary>
    ///     Async version of the map error method of <see cref="Result{TOk}" />.
    /// </summary>
    /// <param name="task">The task.</param>
    /// <param name="mapErrorFunc">The mapping function.</param>
    /// <typeparam name="TOk">The OK type.</typeparam>
    /// <returns>A <see cref="Task{TResult}" />.</returns>
    /// <exception cref="InvalidOperationException">Thrown if state is Invalid.</exception>
    /// <exception cref="UnreachableException">Thrown if the state is unknown.</exception>
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public static async Task<Result<TOk>> MapErrorAsync<TOk>(
        this Task<Result<TOk>> task,
        Func<Error, Task<Error>> mapErrorFunc) where TOk : notnull
    {
        ArgumentNullException.ThrowIfNull(task);
        ArgumentNullException.ThrowIfNull(mapErrorFunc);

        var result = await task.ConfigureAwait(false);
        var (state, _, error) = result;
        return state switch
        {
            ResultState.Ok => result,
            ResultState.Error => new Result<TOk>(await mapErrorFunc(error!).ConfigureAwait(false)),
            ResultState.Invalid => throw new InvalidOperationException(),
            _ => throw new UnreachableException()
        };
    }

    
    /// <summary>
    ///     Async version of the map error method of <see cref="Result{TOk}" />.
    /// </summary>
    /// <param name="task">The task.</param>
    /// <param name="mapErrorFunc">The mapping function.</param>
    /// <typeparam name="TOk">The OK type.</typeparam>
    /// <returns>A <see cref="Task{TResult}" />.</returns>
    /// <exception cref="InvalidOperationException">Thrown if state is Invalid.</exception>
    /// <exception cref="UnreachableException">Thrown if the state is unknown.</exception>
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public static async Task<Result<TOk>> MapErrorAsync<TOk>(
        this Task<Result<TOk>> task,
        Func<Error, Error> mapErrorFunc) where TOk : notnull
    {
        ArgumentNullException.ThrowIfNull(task);
        ArgumentNullException.ThrowIfNull(mapErrorFunc);

        var result = await task.ConfigureAwait(false);
        var (state, _, error) = result;
        return state switch
        {
            ResultState.Ok => result,
            ResultState.Error => new Result<TOk>(mapErrorFunc(error!)),
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
        Func<TOk, TOut> okFunc,
        Func<Error, TOut> errorFunc) where TOk : notnull
    {
        ArgumentNullException.ThrowIfNull(task);
        ArgumentNullException.ThrowIfNull(okFunc);
        ArgumentNullException.ThrowIfNull(errorFunc);

        var result = await task.ConfigureAwait(false);
        var (state, ok, error) = result;
        return state switch
        {
            ResultState.Ok => okFunc(ok!),
            ResultState.Error => errorFunc(error!),
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
    ///     Async version of the 'Tap' method of <see cref="Result{TOk}" />.
    /// </summary>
    /// <param name="task">The task.</param>
    /// <param name="okAction">The 'OK' action.</param>
    /// <param name="errorAction">The 'Error' action.</param>
    /// <typeparam name="TOk">The OK type.</typeparam>
    /// <returns>A <see cref="Task{TResult}" />.</returns>
    /// <exception cref="InvalidOperationException">Thrown if state is Invalid.</exception>
    /// <exception cref="UnreachableException">Thrown if the state is unknown.</exception>
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public static async Task<Result<TOk>> TapAsync<TOk>(
        this Task<Result<TOk>> task,
        Action<TOk> okAction,
        Action<Error> errorAction) where TOk : notnull
    {
        ArgumentNullException.ThrowIfNull(task);
        ArgumentNullException.ThrowIfNull(okAction);
        ArgumentNullException.ThrowIfNull(errorAction);

        var result = await task.ConfigureAwait(false);
        var (state, ok, error) = result;
        switch (state)
        {
            case ResultState.Ok:
                okAction(ok!);
                break;
            case ResultState.Error:
                errorAction(error!);
                break;
            case ResultState.Invalid:
                throw new InvalidOperationException();
            default:
                throw new UnreachableException();
        }

        return result;
    }
}