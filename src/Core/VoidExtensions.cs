namespace SleepingBear.Monad.Core;

/// <summary>
///     Extensions methods of the <see cref="Void" /> class.
/// </summary>
public static class VoidExtensions
{
    /// <summary>
    ///     Converts an <see cref="Action" /> to a <inheritdoc cref="Func{TResult}" />.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <returns>The function.</returns>
    public static Func<Void> ToFunc(this Action action)
    {
        ArgumentNullException.ThrowIfNull(action);

        return () =>
        {
            action();
            return default;
        };
    }

    /// <summary>
    ///     Converts an <see cref="Action{T}" /> to a <inheritdoc cref="Func{TResult}" />.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <returns>The function.</returns>
    public static Func<T1, Void> ToFunc<T1>(this Action<T1> action)
    {
        ArgumentNullException.ThrowIfNull(action);

        return t1 =>
        {
            action(t1);
            return default;
        };
    }

    /// <summary>
    ///     Converts an <see cref="Action{T}" /> to a <inheritdoc cref="Func{TResult}" />.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <returns>The function.</returns>
    public static Func<T1, T2, Void> ToFunc<T1, T2>(this Action<T1, T2> action)
    {
        ArgumentNullException.ThrowIfNull(action);

        return (t1, t2) =>
        {
            action(t1, t2);
            return default;
        };
    }

    /// <summary>
    ///     Converts an <see cref="Action{T}" /> to a <inheritdoc cref="Func{TResult}" />.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <returns>The function.</returns>
    public static Func<T1, T2, T3, Void> ToFunc<T1, T2, T3>(this Action<T1, T2, T3> action)
    {
        ArgumentNullException.ThrowIfNull(action);

        return (t1, t2, t3) =>
        {
            action(t1, t2, t3);
            return default;
        };
    }

    /// <summary>
    ///     Converts an <see cref="Action{T}" /> to a <inheritdoc cref="Func{TResult}" />.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <returns>The function.</returns>
    public static Func<T1, T2, T3, T4, Void> ToFunc<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> action)
    {
        ArgumentNullException.ThrowIfNull(action);

        return (t1, t2, t3, t4) =>
        {
            action(t1, t2, t3, t4);
            return default;
        };
    }

    /// <summary>
    ///     Converts an <see cref="Action{T}" /> to a <inheritdoc cref="Func{TResult}" />.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <returns>The function.</returns>
    public static Func<T1, T2, T3, T4, T5, Void> ToFunc<T1, T2, T3, T4, T5>(this Action<T1, T2, T3, T4, T5> action)
    {
        ArgumentNullException.ThrowIfNull(action);

        return (t1, t2, t3, t4, t5) =>
        {
            action(t1, t2, t3, t4, t5);
            return default;
        };
    }

    /// <summary>
    ///     Converts an <see cref="Action{T}" /> to a <inheritdoc cref="Func{TResult}" />.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <returns>The function.</returns>
    public static Func<T1, T2, T3, T4, T5, T6, Void> ToFunc<T1, T2, T3, T4, T5, T6>(
        this Action<T1, T2, T3, T4, T5, T6> action)
    {
        ArgumentNullException.ThrowIfNull(action);

        return (t1, t2, t3, t4, t5, t6) =>
        {
            action(t1, t2, t3, t4, t5, t6);
            return default;
        };
    }
}