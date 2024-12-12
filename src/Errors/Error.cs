using System.Diagnostics.CodeAnalysis;

namespace SleepingBear.Monad.Errors;

/// <summary>
///     Abstract error base record.
/// </summary>
[SuppressMessage("Naming", "CA1716:Identifiers should not match keywords")]
public abstract record Error;