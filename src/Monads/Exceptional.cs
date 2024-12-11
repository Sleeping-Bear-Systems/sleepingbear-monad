namespace SleepingBear.Monad.Monads;

/// <summary>
///     Exceptional monad.
/// </summary>
/// <typeparam name="TValue">The value type.</typeparam>
public readonly record struct Exceptional<TValue> where TValue : notnull
{
    private readonly Exception? _exception;
    private readonly TValue? _value;

    /// <summary>
    ///     Default constructor.
    /// </summary>
    public Exceptional()
    {
        this.IsValue = false;
        this._value = default;
        this._exception = new InvalidOperationException("Exceptional<TValue> created using default constructor.");
    }

    internal Exceptional(TValue value)
    {
        this.IsValue = true;
        this._value = value;
        this._exception = null;
    }

    internal Exceptional(Exception exception)
    {
        this.IsValue = false;
        this._value = default;
        this._exception = exception;
    }

    /// <summary>
    ///     Property indicating the monad contains a value.
    /// </summary>
    public bool IsValue { get; }

    /// <summary>
    ///     Property indicating the monad contains an exception.
    /// </summary>
    public bool IsException => !this.IsValue;

    /// <summary>
    ///     Deconstructs the monad.
    /// </summary>
    /// <param name="isValue">The state.</param>
    /// <param name="value">The value.</param>
    /// <param name="exception">The exception.</param>
    public void Deconstruct(out bool isValue, out TValue? value, out Exception? exception)
    {
        (isValue, value, exception) = (this.IsValue, this._value, this._exception);
    }
}