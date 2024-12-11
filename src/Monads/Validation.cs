using System.Collections.Immutable;
using SleepingBear.Monad.Errors;

namespace SleepingBear.Monad.Monads;

/// <summary>
///     Validation monad.
/// </summary>
public readonly record struct Validation<TValue> where TValue : notnull
{
    private readonly ImmutableList<Error>? _errors;
    private readonly TValue? _value;

    internal Validation(TValue value)
    {
        this._value = value;
        this._errors = default;
        this.IsValid = true;
    }

    internal Validation(ImmutableList<Error>? errors)
    {
        this._value = default;
        this._errors = errors ?? ImmutableList<Error>.Empty;
        this.IsValid = false;
    }

    /// <summary>
    ///     Default constructor.
    /// </summary>
    public Validation()
    {
        this._value = default;
        this._errors = ImmutableList<Error>.Empty;
        this.IsValid = false;
    }

    /// <summary>
    ///     Is valid?
    /// </summary>
    public bool IsValid { get; }

    /// <summary>
    ///     Is invalid?
    /// </summary>
    public bool IsInvalid => !this.IsValid;

    /// <summary>
    ///     Deconstructs the validation monad.
    /// </summary>
    /// <param name="isValid">The is valid flag.</param>
    /// <param name="value">The value.</param>
    /// <param name="errors">The collection of errors.</param>
    public void Deconstruct(out bool isValid, out TValue? value, out ImmutableList<Error>? errors)
    {
        (isValid, value, errors) = (this.IsValid, this._value, this._errors);
    }
}