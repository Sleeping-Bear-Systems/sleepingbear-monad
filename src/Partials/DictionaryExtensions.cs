using SleepingBear.Monad.Monads;

namespace SleepingBear.Monad.Partials;

/// <summary>
///     Extension methods for <see cref="Dictionary{TKey,TValue}" />.
/// </summary>
public static class DictionaryExtensions
{
    /// <summary>
    ///     Tries to get the value associated with the specified key from the <paramref name="dictionary" />.
    /// </summary>
    /// <param name="dictionary">The <see cref="IDictionary{TKey,TValue}" /> instance.</param>
    /// <param name="key">The key.</param>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <typeparam name="TValue">The value type.</typeparam>
    /// <returns>A <see cref="Maybe{TSome}" /> containing the value if found.</returns>
    public static Maybe<TValue> GetValueOrNone<TKey, TValue>(this IDictionary<TKey, TValue>? dictionary, TKey key)
        where TKey : notnull where TValue : notnull
    {
        return dictionary is not null && dictionary.TryGetValue(key, out var value)
            ? value.ToMaybe()
            : Maybe<TValue>.None;
    }
}