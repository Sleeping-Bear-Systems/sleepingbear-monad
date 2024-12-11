using System.Diagnostics.CodeAnalysis;

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

    /// <summary>
    ///     Maps a <see cref="Exceptional{TValue}" />.
    /// </summary>
    /// <param name="mapFunc">The mapping function.</param>
    /// <typeparam name="TValueOut">The output value type.</typeparam>
    /// <returns>A <see cref="Exceptional{TValue}" />.</returns>
    /// <remarks>
    ///     The <paramref name="mapFunc" /> should NOT throw any exceptions.
    /// </remarks>
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public Exceptional<TValueOut> Map<TValueOut>(Func<TValue, TValueOut> mapFunc) where TValueOut : notnull
    {
        ArgumentNullException.ThrowIfNull(mapFunc);

        return this.IsValue
            ? new Exceptional<TValueOut>(mapFunc(this._value!))
            : new Exceptional<TValueOut>(this._exception!);
    }

    /// <summary>
    ///     Taps a <see cref="Exceptional{TValue}" />.
    /// </summary>
    /// <param name="valueAction">The 'Value' action.</param>
    /// <param name="exceptionAction">The 'Exception' action.</param>
    /// <returns>The <see cref="Exceptional{TValue}" /> instance.</returns>
    /// <remarks>
    ///     The <paramref name="valueAction" /> and <paramref name="exceptionAction" /> should NOT
    ///     throw any exceptions.
    /// </remarks>
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public Exceptional<TValue> Tap(
        Action<TValue> valueAction,
        Action<Exception> exceptionAction)
    {
        ArgumentNullException.ThrowIfNull(valueAction);
        ArgumentNullException.ThrowIfNull(exceptionAction);

        if (this.IsValue)
            valueAction(this._value!);
        else
            exceptionAction(this._exception!);

        return this;
    }

    /// <summary>
    ///     Binds a <see cref="Exceptional{TValue}" />.
    /// </summary>
    /// <param name="bindFunc">The binding function.</param>
    /// <typeparam name="TValueOut">The output value type.</typeparam>
    /// <returns>A <see cref="Exceptional{TValue}" />.</returns>
    /// <remarks>
    ///     The <paramref name="bindFunc" /> should NOT throw any exceptions.
    /// </remarks>
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public Exceptional<TValueOut> Bind<TValueOut>(Func<TValue, Exceptional<TValueOut>> bindFunc)
        where TValueOut : notnull
    {
        ArgumentNullException.ThrowIfNull(bindFunc);

        return this.IsValue
            ? bindFunc(this._value!)
            : new Exceptional<TValueOut>(this._exception!);
    }

    /// <summary>
    ///     Try to get the value from <see cref="Exceptional{TValue}" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>True if state is 'Value', false otherwise.</returns>
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public bool Try([NotNullWhen(true)] out TValue? value)
    {
        value = this.IsValue
            ? this._value!
            : default;
        return this.IsValue;
    }


    /// <summary>
    ///     Matches a <see cref="Exceptional{TValue}" />.
    /// </summary>
    /// <param name="valueFunc">The 'Value' function.</param>
    /// <param name="exceptionFunc">The 'Exception' function.</param>
    /// <typeparam name="TValueOut">The output value type.</typeparam>
    /// <returns>The matched value.</returns>
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public TValueOut Match<TValueOut>(Func<TValue, TValueOut> valueFunc, Func<Exception, TValueOut> exceptionFunc)
    {
        ArgumentNullException.ThrowIfNull(valueFunc);
        ArgumentNullException.ThrowIfNull(exceptionFunc);

        return this.IsValue
            ? valueFunc(this._value!)
            : exceptionFunc(this._exception!);
    }
}
