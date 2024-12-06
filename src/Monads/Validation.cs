using SleepingBear.Monad.Errors;

namespace SleepingBear.Monad.Monads;

/// <summary>
/// Validation monad.
/// </summary>
public readonly struct Validation<TValue> : IEquatable<Validation<TValue>> where TValue : notnull
{
    private readonly TValue? _value;

    private readonly IEnumerable<Error>? _errors;

    internal Validation(TValue value)
    {
        this._value = value;
        this._errors = default;
        this.IsValid = true;
    }

    internal Validation(IEnumerable<Error>? errors)
    {
        this._value = default;
        this._errors = errors ?? [];
        this.IsValid = false;
    }

    /// <summary>
    /// Default constructor.
    /// </summary>
    public Validation()
    {
        this._value = default;
        this._errors = [];
        this.IsValid = false;
    }

    /// <summary>
    /// Is valid?
    /// </summary>
    public bool IsValid { get; }

    /// <summary>
    /// Is invalid?
    /// </summary>
    public bool IsInvalid => !this.IsValid;
    
    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is Validation<TValue> other && this.Equals(other);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return HashCode.Combine(this._value, this._errors, this.IsValid);
    }

    /// <summary>
    /// Equality operator.
    /// </summary>
    public static bool operator ==(Validation<TValue> left, Validation<TValue> right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Inequality operator.
    /// </summary>
    public static bool operator !=(Validation<TValue> left, Validation<TValue> right)
    {
        return !(left == right);
    }

    /// <inheritdoc cref="IEquatable{T}"/>.
    public bool Equals(Validation<TValue> other)
    {
        return EqualityComparer<TValue?>.Default.Equals(this._value, other._value) && Equals(this._errors, other._errors) && this.IsValid == other.IsValid;
    }
    
    /// <summary>
    /// Deconstructs the validation monad.
    /// </summary>
    /// <param name="isValid">The is valid flag.</param>
    /// <param name="value">The value.</param>
    /// <param name="errors">The collection of errors.</param>
    public void Deconstruct(out bool isValid, out TValue? value, out IEnumerable<Error>? errors)
    {
        value = this._value;
        errors = this._errors;
        isValid = this.IsValid;
    }
}