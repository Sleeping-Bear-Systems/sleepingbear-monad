using System.Diagnostics.CodeAnalysis;

namespace SleepingBear.Monad.Errors;

/// <summary>
///     Abstract error base record.
/// </summary>
[SuppressMessage("Naming", "CA1716:Identifiers should not match keywords")]
public abstract record Error;

/// <summary>
///     Concrete <c>Error</c> record that wraps a value.
/// </summary>
/// <typeparam name="TValue">The type of the value.</typeparam>
[SuppressMessage("Naming", "CA1716:Identifiers should not match keywords")]
public sealed record Error<TValue>(TValue Value) : Error where TValue : notnull;
