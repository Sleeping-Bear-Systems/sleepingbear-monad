namespace SleepingBear.Monad.Errors;

/// <summary>
///     Concrete <c>GenericError</c> record that wraps a value.
/// </summary>
/// <typeparam name="TValue">The type of the value.</typeparam>
public sealed record GenericError<TValue>(TValue Value) : Error where TValue : notnull;