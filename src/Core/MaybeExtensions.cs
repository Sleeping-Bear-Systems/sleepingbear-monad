using System.Diagnostics.CodeAnalysis;

namespace SleepingBear.Monad.Core;

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
    public static Maybe<TSome> Filter<TSome>(this Maybe<TSome> maybe, Func<TSome, bool> predicate) where TSome : notnull
    {
        ArgumentNullException.ThrowIfNull(predicate);

        return maybe.Bind(some => predicate(some) ? new Maybe<TSome>(some) : Maybe<TSome>.None);
    }
}