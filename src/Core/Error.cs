using System.Diagnostics.CodeAnalysis;

namespace SleepingBear.Monad.Core;

/// <summary>
///     Abstract error base class.
/// </summary>
[SuppressMessage("Naming", "CA1716:Identifiers should not match keywords")]
public abstract record Error;

/// <summary>
///     Concrete error that wraps a single value.
/// </summary>
/// <typeparam name="TValue">The value type.</typeparam>
[SuppressMessage("Naming", "CA1716:Identifiers should not match keywords")]
public sealed record Error<TValue>(TValue Value) : Error where TValue : notnull;