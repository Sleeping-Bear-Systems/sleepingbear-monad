using System.Diagnostics.CodeAnalysis;

namespace SleepingBear.Monad.Core;

/// <summary>
///     Abstract error base class.
/// </summary>
[SuppressMessage("Naming", "CA1716:Identifiers should not match keywords")]
public abstract class Error;

/// <summary>
///     Concrete error that wraps a single value.
/// </summary>
/// <typeparam name="TValue">The value type.</typeparam>
[SuppressMessage("Naming", "CA1716:Identifiers should not match keywords")]
public sealed class Error<TValue> : Error
{
    internal Error(TValue value)
    {
        this.Value = value;
    }

    /// <summary>
    ///     Gets the error value.
    /// </summary>
    public TValue Value { get; }
}