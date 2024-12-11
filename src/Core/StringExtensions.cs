namespace SleepingBear.Monad.Core;

/// <summary>
///     Extension methods for <see cref="string" />.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    ///     Extension method for ensuring a string is not null.
    /// </summary>
    /// <param name="value">The string being checked.</param>
    /// <param name="nullValue">The string to return if <paramref name="value" /> is null. (optional)</param>
    /// <returns>
    ///     The <paramref name="value" /> if not null, the <paramref name="nullValue" /> if not null,
    ///     or the empty string otherwise.
    /// </returns>
    public static string IfNull(this string? value, string? nullValue = null)
    {
        return value ?? nullValue ?? string.Empty;
    }

    /// <summary>
    ///     Extension method for ensuring a string is not null or empty.
    /// </summary>
    /// <param name="value">The string being checked.</param>
    /// <param name="nullValue">The string to return if <paramref name="value" /> is null or empty. (optional)</param>
    /// <returns>
    ///     The <paramref name="value" /> if not null or empty, the <paramref name="nullValue" /> if not null or empty,
    ///     or the empty string otherwise.
    /// </returns>
    public static string IfNullOrEmpty(this string? value, string? nullValue = null)
    {
        return string.IsNullOrEmpty(value)
            ? string.IsNullOrEmpty(nullValue)
                ? string.Empty
                : nullValue
            : value;
    }

    /// <summary>
    ///     Extension method for ensuring a string is not null, empty, or whitespace.
    /// </summary>
    /// <param name="value">The string being checked.</param>
    /// <param name="nullValue">The string to return if <paramref name="value" /> is null, empty, or whitespace. (optional)</param>
    /// <returns>
    ///     The <paramref name="value" /> if not null, empty or whitespace, the <paramref name="nullValue" /> if not null,
    ///     empty, or whitespace, or the empty string otherwise.
    /// </returns>
    public static string IfNullOrWhitespace(this string? value, string? nullValue = null)
    {
        return string.IsNullOrWhiteSpace(value)
            ? string.IsNullOrWhiteSpace(nullValue)
                ? string.Empty
                : nullValue
            : value;
    }
}
