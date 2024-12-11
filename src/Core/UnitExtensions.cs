namespace SleepingBear.Monad.Core;

/// <summary>
///     Extensions methods of the <see cref="Unit" /> class.
/// </summary>
public static class UnitExtensions
{
    /// <summary>
    ///     Converts an <see cref="Action" /> to a <inheritdoc cref="Func{TResult}" />.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <returns>The function.</returns>
    public static Func<Unit> ToFunc(this Action action)
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
    public static Func<T1, Unit> ToFunc<T1>(this Action<T1> action)
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
    public static Func<T1, T2, Unit> ToFunc<T1, T2>(this Action<T1, T2> action)
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
    public static Func<T1, T2, T3, Unit> ToFunc<T1, T2, T3>(this Action<T1, T2, T3> action)
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
    public static Func<T1, T2, T3, T4, Unit> ToFunc<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> action)
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
    public static Func<T1, T2, T3, T4, T5, Unit> ToFunc<T1, T2, T3, T4, T5>(this Action<T1, T2, T3, T4, T5> action)
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
    public static Func<T1, T2, T3, T4, T5, T6, Unit> ToFunc<T1, T2, T3, T4, T5, T6>(
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
