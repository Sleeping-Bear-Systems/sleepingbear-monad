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
}