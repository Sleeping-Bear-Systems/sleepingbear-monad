using SleepingBear.Monad.Monads;

namespace SleepingBear.Monad.Partials;

/// <summary>
///     Extension methods for <see cref="IEnumerable{T}" />.
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>
    ///     Extension method to get the first element of a sequence or None if the sequence is empty.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="predicate"></param>
    /// <typeparam name="TValue"></typeparam>
    /// <returns></returns>
    public static Maybe<TValue> FirstOrNone<TValue>(this IEnumerable<TValue>? source, Func<TValue, bool> predicate)
        where TValue : notnull
    {
        ArgumentNullException.ThrowIfNull(predicate);

        if (source is null) return Maybe<TValue>.None;

        if (!typeof(TValue).IsValueType) return source.FirstOrDefault(predicate).ToMaybe();

        foreach (var item in source)
            if (predicate(item))
                return item.ToMaybe();

        return Maybe<TValue>.None;
    }
}