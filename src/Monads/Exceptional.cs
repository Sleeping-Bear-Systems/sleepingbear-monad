using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using SleepingBear.Monad.Core;

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

    /// <summary>
    ///     Maps a <see cref="Exceptional{TValue}" />.
    /// </summary>
    /// <param name="mapFunc">The mapping function.</param>
    /// <typeparam name="TValueOut">The output value type.</typeparam>
    /// <returns>A <see cref="Exceptional{TValue}" />.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the state is invalid.</exception>
    /// <exception cref="UnreachableException">Thrown if the state is unknown.</exception>
    /// <remarks>
    ///     The <c>FailFastIfCritical()</c> call prevents unrecoverable exceptions from being caught.
    /// </remarks>
    [SuppressMessage("Design", "CA1031:Do not catch general exception types")]
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public Exceptional<TValueOut> Map<TValueOut>(Func<TValue, TValueOut> mapFunc) where TValueOut : notnull
    {
        ArgumentNullException.ThrowIfNull(mapFunc);

        switch (this._state)
        {
            case ExceptionalState.Value:
            {
                try
                {
                    return new Exceptional<TValueOut>(mapFunc(this._value!));
                }
                catch (Exception exception)
                {
                    exception.FailFastIfCritical("SleepingBear.Monad.Exceptional.Map");
                    return new Exceptional<TValueOut>(exception);
                }
            }
            case ExceptionalState.Exception:
            {
                return new Exceptional<TValueOut>(this._exception!);
            }
            case ExceptionalState.Invalid:
                throw new InvalidOperationException();
            default:
                throw new UnreachableException();
        }
    }

    /// <summary>
    ///     Taps a <see cref="Exceptional{TValue}" />.
    /// </summary>
    /// <param name="valueAction">The 'Value' action.</param>
    /// <param name="exceptionAction">The 'Exception' action.</param>
    /// <returns>The <see cref="Exceptional{TValue}" /> instance.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the state is invalid.</exception>
    /// <exception cref="UnreachableException">Thrown if the state is unknown.</exception>
    /// <remarks>
    ///     The <paramref name="valueAction" /> and <paramref name="exceptionAction" /> should NOT
    ///     throw exceptions.
    /// </remarks>
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public Exceptional<TValue> Tap(
        Action<TValue> valueAction,
        Action<Exception> exceptionAction)
    {
        ArgumentNullException.ThrowIfNull(valueAction);
        ArgumentNullException.ThrowIfNull(exceptionAction);

        switch (this._state)
        {
            case ExceptionalState.Value:
            {
                valueAction(this._value!);
                return this;
            }
            case ExceptionalState.Exception:
            {
                exceptionAction(this._exception!);
                return this;
            }
            case ExceptionalState.Invalid:
                throw new InvalidOperationException();
            default:
                throw new UnreachableException();
        }
    }

    /// <summary>
    ///     Binds a <see cref="Exceptional{TValue}" />.
    /// </summary>
    /// <param name="bindFunc">The binding function.</param>
    /// <typeparam name="TValueOut">The output value type.</typeparam>
    /// <returns>A <see cref="Exceptional{TValue}" />.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the state is invalid.</exception>
    /// <exception cref="UnreachableException">Thrown if the state is unknown.</exception>
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public Exceptional<TValueOut> Bind<TValueOut>(Func<TValue, Exceptional<TValueOut>> bindFunc)
        where TValueOut : notnull
    {
        ArgumentNullException.ThrowIfNull(bindFunc);

        return this._state switch
        {
            ExceptionalState.Value => bindFunc(this._value!),
            ExceptionalState.Exception => new Exceptional<TValueOut>(this._exception!),
            ExceptionalState.Invalid => throw new InvalidOperationException(),
            _ => throw new UnreachableException()
        };
    }

    /// <summary>
    ///     Try to get the value from <see cref="Exceptional{TValue}" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>True if state is 'Value', false otherwise.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the state is invalid.</exception>
    /// <exception cref="UnreachableException">Thrown if the state is unknown.</exception>
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public bool Try([NotNullWhen(true)] out TValue? value)
    {
        switch (this._state)
        {
            case ExceptionalState.Invalid:
                value = default;
                return false;
            case ExceptionalState.Value:
                value = this._value!;
                return true;
            case ExceptionalState.Exception:
                throw new InvalidOperationException();
            default:
                throw new UnreachableException();
        }
    }
}