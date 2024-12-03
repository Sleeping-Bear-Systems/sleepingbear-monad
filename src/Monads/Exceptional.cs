namespace SleepingBear.Monad.Monads;

/// <summary>
///     Exceptional monad state.
/// </summary>
public enum ExceptionalState
{
    /// <summary>
    ///     Indicates that monad is invalid due to default construction.
    /// </summary>
    Invalid = 0,

    /// <summary>
    ///     Indicates the monad contains a value.
    /// </summary>
    Value,

    /// <summary>
    ///     Indicates the monad contains <see cref="Exception" />.
    /// </summary>
    Exception
}

/// <summary>
///     Exceptional monad.
/// </summary>
/// <typeparam name="TValue">The value type.</typeparam>
public readonly struct Exceptional<TValue> : IEquatable<Exceptional<TValue>> where TValue : notnull
{
    private readonly Exception? _exception;
    private readonly TValue? _value;
    private readonly ExceptionalState _state;

    internal Exceptional(TValue value)
    {
        this._value = value;
        this._exception = null;
        this._state = ExceptionalState.Value;
    }

    internal Exceptional(Exception exception)
    {
        this._value = default;
        this._exception = exception;
        this._state = ExceptionalState.Exception;
    }

    /// <summary>
    ///     Property indicating the monad contains a value.
    /// </summary>
    public bool IsValue => this._state == ExceptionalState.Value;

    /// <summary>
    ///     Property indicating the monad contains an exception.
    /// </summary>
    public bool IsException => this._state == ExceptionalState.Exception;

    /// <summary>
    ///     Deconstructs the monad.
    /// </summary>
    /// <param name="state">The state.</param>
    /// <param name="value">The value.</param>
    /// <param name="exception">The exception.</param>
    public void Deconstruct(out ExceptionalState state, out TValue? value, out Exception? exception)
    {
        state = this._state;
        value = this._value;
        exception = this._exception;
    }

    /// <inheritdoc cref="object" />
    public override bool Equals(object? obj)
    {
        return obj is Exceptional<TValue> other && this.Equals(other);
    }

    /// <inheritdoc cref="object" />
    public override int GetHashCode()
    {
        return HashCode.Combine(this._exception, this._value, (int)this._state);
    }

    /// <inheritdoc cref="object" />
    public static bool operator ==(Exceptional<TValue> left, Exceptional<TValue> right)
    {
        return left.Equals(right);
    }

    /// <inheritdoc cref="object" />
    public static bool operator !=(Exceptional<TValue> left, Exceptional<TValue> right)
    {
        return !(left == right);
    }

    /// <inheritdoc cref="object" />
    public bool Equals(Exceptional<TValue> other)
    {
        return Equals(this._exception, other._exception) &&
               EqualityComparer<TValue?>.Default.Equals(this._value, other._value) && this._state == other._state;
    }
}