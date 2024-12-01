namespace SleepingBear.Monad.Core;

/// <summary>
/// Abstract error base class.
/// </summary>
public abstract class Error;

/// <summary>
/// Concrete error that wraps a single value.
/// </summary>
/// <typeparam name="TValue">The value type.</typeparam>
public sealed class Error<TValue> : Error
{
    internal Error(TValue value) => this.Value = value;

    /// <summary>
    /// Gets the error value.
    /// </summary>
    public TValue Value { get; }
}