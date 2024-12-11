using System.Diagnostics.CodeAnalysis;

namespace SleepingBear.Monad.Monads;

/// <summary>
///     Extension methods for <see cref="Maybe{TSome}" />.
/// </summary>
public static class MaybeExtensions
{
    /// <summary>
    ///     Extension method for lifting value to a <see cref="Maybe{TSome}" />.
    /// </summary>
    /// <param name="some">The value being lifted.</param>
    /// <typeparam name="TSome">The 'Some' type.</typeparam>
    /// <returns>A <see cref="Maybe{TSome}" />.</returns>
    public static Maybe<TSome> ToMaybe<TSome>(this TSome? some) where TSome : notnull
    {
        return new Maybe<TSome>(some);
    }

    /// <summary>
    ///     Extension method for filtering a <see cref="Maybe{TSome}" />.
    /// </summary>
    /// <param name="maybe">The maybe instance.</param>
    /// <param name="predicate">The filter predicate.</param>
    /// <typeparam name="TSome">The 'Some' type.</typeparam>
    /// <returns>The maybe instance.</returns>
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public static Maybe<TSome> Where<TSome>(this Maybe<TSome> maybe, Func<TSome, bool> predicate) where TSome : notnull
    {
        ArgumentNullException.ThrowIfNull(predicate);

        return maybe.Try(out var some) && predicate(some) ? maybe : Maybe<TSome>.None;
    }

    /// <summary>
    ///     Matches the 'Maybe'.
    /// </summary>
    /// <param name="maybe">The maybe being matched.</param>
    /// <param name="noneValue">The none value.</param>
    /// <returns>The matched value.</returns>
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public static TSome Match<TSome>(this Maybe<TSome> maybe, TSome noneValue) where TSome : notnull
    {
        var (isSome, some) = maybe;
        return isSome ? some! : noneValue;
    }

    /// <summary>
    ///     Matches the 'Maybe'.
    /// </summary>
    /// <param name="maybe">The maybe being matched.</param>
    /// <param name="someFunc">The 'Some' function.</param>
    /// <param name="noneFunc">The 'None' function.</param>
    /// <typeparam name="TOut">The output type.</typeparam>
    /// <typeparam name="TSome">The type of the lifted value.</typeparam>
    /// <returns>The matched value.</returns>
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public static TOut Match<TSome, TOut>(
        this Maybe<TSome> maybe,
        Func<TSome, TOut> someFunc, Func<TOut> noneFunc) where TSome : notnull
    {
        ArgumentNullException.ThrowIfNull(someFunc);
        ArgumentNullException.ThrowIfNull(noneFunc);

        var (isSome, some) = maybe;
        return isSome
            ? someFunc(some!)
            : noneFunc();
    }

    /// <summary>
    ///     Maps the 'Maybe'.
    /// </summary>
    /// <param name="maybe">The maybe being mapped.</param>
    /// <param name="mapFunc">The mapping function.</param>
    /// <typeparam name="TOut">The output type.</typeparam>
    /// <typeparam name="TSome">The type of the lifted value.</typeparam>
    /// <returns>The mapped 'Maybe'.</returns>
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public static Maybe<TOut> Map<TSome, TOut>(
        this Maybe<TSome> maybe,
        Func<TSome, TOut> mapFunc) where TSome : notnull where TOut : notnull
    {
        ArgumentNullException.ThrowIfNull(mapFunc);

        var (isSome, some) = maybe;
        return isSome
            ? new Maybe<TOut>(mapFunc(some!))
            : Maybe<TOut>.None;
    }

    /// <summary>
    ///     Binds the 'Maybe'.
    /// </summary>
    /// <param name="maybe">The maybe being bound.</param>
    /// <param name="bindFunc">The binding function.</param>
    /// <typeparam name="TOut">The output 'Some' type.</typeparam>
    /// <typeparam name="TSome">The type of the lifted value.</typeparam>
    /// <returns>The bound 'Maybe'.</returns>
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public static Maybe<TOut> Bind<TSome, TOut>(
        this Maybe<TSome> maybe,
        Func<TSome, Maybe<TOut>> bindFunc) where TSome : notnull where TOut : notnull
    {
        ArgumentNullException.ThrowIfNull(bindFunc);

        var (isSome, some) = maybe;
        return isSome
            ? bindFunc(some!)
            : Maybe<TOut>.None;
    }

    /// <summary>
    ///     Taps the 'Maybe'.
    /// </summary>
    /// <param name="maybe">The maybe being tapped.</param>
    /// <param name="someAction">The 'Some' action.</param>
    /// <param name="noneAction">The 'None' action.</param>
    /// <returns>The maybe instance</returns>
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public static Maybe<TSome> Tap<TSome>(
        this Maybe<TSome> maybe,
        Action<TSome> someAction,
        Action noneAction) where TSome : notnull
    {
        ArgumentNullException.ThrowIfNull(someAction);
        ArgumentNullException.ThrowIfNull(noneAction);

        var (isSome, some) = maybe;
        if (isSome)
            someAction(some!);
        else
            noneAction();

        return maybe;
    }

    /// <summary>
    ///     Tries to get the 'Some' value.
    /// </summary>
    /// <param name="maybe">The maybe being checked.</param>
    /// <param name="some">The 'Some' value.</param>
    /// <returns>True if is some.</returns>
    public static bool Try<TSome>(
        this Maybe<TSome> maybe,
        [NotNullWhen(true)] out TSome? some) where TSome : notnull
    {
        var (isSome, some2) = maybe;
        some = some2;
        return isSome;
    }
}
