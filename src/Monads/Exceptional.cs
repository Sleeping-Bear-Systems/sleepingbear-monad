namespace SleepingBear.Monad.Monads;

/// <summary>
///     Exceptional monad.
/// </summary>
/// <typeparam name="TValue">The type of the lifted value.</typeparam>
public readonly record struct Exceptional<TValue> where TValue : notnull
{
    private readonly Exception? _exception;
    private readonly TValue? _value;

    /// <summary>
    ///     Default constructor.
    /// </summary>
    public Exceptional()
    {
        this.IsSuccess = false;
        this._value = default;
        this._exception = new InvalidOperationException("Exceptional<TValue> created using default constructor.");
    }

    internal Exceptional(TValue value)
    {
        this.IsSuccess = true;
        this._value = value;
        this._exception = null;
    }

    internal Exceptional(Exception exception)
    {
        this.IsSuccess = false;
        this._value = default;
        this._exception = exception;
    }

    /// <summary>
    ///     Flag indicating the monad contains a value.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    ///     Flag indicating the monad contains an exception.
    /// </summary>
    public bool IsFailure => !this.IsSuccess;

    /// <summary>
    ///     Deconstructs the monad.
    /// </summary>
    /// <param name="isSuccess">The state.</param>
    /// <param name="value">The value.</param>
    /// <param name="exception">The exception.</param>
    public void Deconstruct(out bool isSuccess, out TValue? value, out Exception? exception)
    {
        (isSuccess, value, exception) = (this.IsSuccess, this._value, this._exception);
    }

    /// <summary>
    ///     Implicit operator.
    /// </summary>
    /// <param name="value">The value being lifted.</param>
    /// <returns>A <see cref="Exceptional{TValue}" /> containing the lifted value.</returns>
    public static implicit operator Exceptional<TValue>(TValue value)
    {
        return new Exceptional<TValue>(value);
    }

    /// <summary>
    ///     Implicit operator.
    /// </summary>
    /// <param name="exception">The exception being lifted.</param>
    /// <returns>A <see cref="Exceptional{TValue}" /> containing the lifted exception.</returns>
    public static implicit operator Exceptional<TValue>(Exception exception)
    {
        return new Exceptional<TValue>(exception);
    }

    /// <summary>
    ///     Alternate for operator implicit.
    /// </summary>
    /// <returns>A <see cref="Exceptional{TValue}" />.</returns>
    /// <remarks>
    ///     Reference: https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca2225
    /// </remarks>
    // ReSharper disable once UnusedMember.Global
    public Exceptional<TValue> ToExceptional()
    {
        return this;
    }
}