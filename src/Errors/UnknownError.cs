namespace SleepingBear.Monad.Errors;

/// <summary>
///     Unknown error.
/// </summary>
public sealed record UnknownError : Error
{
    private UnknownError() {}

    /// <summary>
    ///     The value of the unknown error.
    /// </summary>
    public static readonly UnknownError Value = new();
}