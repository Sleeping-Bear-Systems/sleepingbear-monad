using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using SleepingBear.Monad.Errors;

namespace SleepingBear.Monad.Monads;

/// <summary>
///     Result state enumeration.
/// </summary>
public enum ResultState
{
    /// <summary>
    ///     Invalid state - indicates the Result struct was constructed with the default constructor.
    /// </summary>
    Invalid = 0,

    /// <summary>
    ///     OK state.
    /// </summary>
    Ok,

    /// <summary>
    ///     Error state.
    /// </summary>
    Error
}

/// <summary>
///     Result monad.
/// </summary>
/// <typeparam name="TOk">The OK type.</typeparam>
public readonly struct Result<TOk> : IEquatable<Result<TOk>>
    where TOk : notnull
{
    private readonly ResultState _state;
    private readonly TOk? _ok;
    private readonly Error? _error;

    internal Result(TOk ok)
    {
        this._ok = ok;
        this._error = null;
        this._state = ResultState.Ok;
    }

    internal Result(Error error)
    {
        this._ok = default;
        this._error = error;
        this._state = ResultState.Error;
    }

    /// <summary>
    ///     Indicates the result is in the 'OK' state.
    /// </summary>
    public bool IsOk => this._state == ResultState.Ok;

    /// <summary>
    ///     Indicates the result in the 'Error' state.
    /// </summary>
    public bool IsError => this._state == ResultState.Error;

    /// <summary>
    ///     Deconstructs the result.
    /// </summary>
    /// <param name="state">The state.</param>
    /// <param name="ok">The OK value.</param>
    /// <param name="error">The error value.</param>
    public void Deconstruct(out ResultState state, out TOk? ok, out Error? error)
    {
        state = this._state;
        ok = this._ok;
        error = this._error;
    }

    /// <inheritdoc cref="object" />
    public override bool Equals(object? obj)
    {
        return obj is Result<TOk> other && this.Equals(other);
    }

    /// <inheritdoc cref="object" />
    public override int GetHashCode()
    {
        return HashCode.Combine((int)this._state, this._ok, this._error);
    }

    /// <summary>
    ///     Equality operator.
    /// </summary>
    public static bool operator ==(Result<TOk> left, Result<TOk> right)
    {
        return left.Equals(right);
    }

    /// <summary>
    ///     Inequality operator.
    /// </summary>
    public static bool operator !=(Result<TOk> left, Result<TOk> right)
    {
        return !(left == right);
    }

    /// <inheritdoc cref="IEquatable{T}" />
    public bool Equals(Result<TOk> other)
    {
        return this._state == other._state &&
               EqualityComparer<TOk?>.Default.Equals(this._ok, other._ok) &&
               Equals(this._error, other._error);
    }

    /// <summary>
    ///     Map a <see cref="Result{TOk}" />.
    /// </summary>
    /// <param name="map">The mapping function.</param>
    /// <typeparam name="TOkOut">The output OK type.</typeparam>
    /// <returns>A <see cref="Result{TOk}" />.</returns>
    /// <exception cref="InvalidOperationException">Thrown if state is Invalid.</exception>
    /// <exception cref="UnreachableException">Thrown if the state is unknown.</exception>
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public Result<TOkOut> Map<TOkOut>(Func<TOk, TOkOut> map) where TOkOut : notnull
    {
        ArgumentNullException.ThrowIfNull(map);

        return this._state switch
        {
            ResultState.Ok => new Result<TOkOut>(map(this._ok!)),
            ResultState.Error => new Result<TOkOut>(this._error!),
            ResultState.Invalid => throw new InvalidOperationException(),
            _ => throw new UnreachableException()
        };
    }

    /// <summary>
    ///     Map a <see cref="Result{TOk}" />.
    /// </summary>
    /// <param name="map">The mapping function.</param>
    /// <returns>A <see cref="Result{TOk}" />.</returns>
    /// <exception cref="InvalidOperationException">Thrown if state is Invalid.</exception>
    /// <exception cref="UnreachableException">Thrown if the state is unknown.</exception>
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public Result<TOk> MapError(Func<Error, Error> map)
    {
        ArgumentNullException.ThrowIfNull(map);

        return this._state switch
        {
            ResultState.Ok => this,
            ResultState.Error => new Result<TOk>(map(this._error!)),
            ResultState.Invalid => throw new InvalidOperationException(),
            _ => throw new UnreachableException()
        };
    }

    /// <summary>
    ///     Binds a <see cref="Result{TOk}" />.
    /// </summary>
    /// <param name="bind">The binding function.</param>
    /// <typeparam name="TOkOut">The output OK type.</typeparam>
    /// <returns>A <see cref="Result{TOk}" />.</returns>
    /// <exception cref="InvalidOperationException">Thrown if state is Invalid.</exception>
    /// <exception cref="UnreachableException">Thrown if the state is unknown.</exception>
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public Result<TOkOut> Bind<TOkOut>(Func<TOk, Result<TOkOut>> bind) where TOkOut : notnull
    {
        ArgumentNullException.ThrowIfNull(bind);

        return this._state switch
        {
            ResultState.Ok => bind(this._ok!),
            ResultState.Error => new Result<TOkOut>(this._error!),
            ResultState.Invalid => throw new InvalidOperationException(),
            _ => throw new UnreachableException()
        };
    }

    /// <summary>
    ///     Bind a <see cref="Result{TOk}" />.
    /// </summary>
    /// <param name="bind">The binding function.</param>
    /// <returns>A <see cref="Result{TOk}" />.</returns>
    /// <exception cref="InvalidOperationException">Thrown if state is Invalid.</exception>
    /// <exception cref="UnreachableException">Thrown if the state is unknown.</exception>
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public Result<TOk> BindError(Func<Error, Result<TOk>> bind)
    {
        ArgumentNullException.ThrowIfNull(bind);

        return this._state switch
        {
            ResultState.Ok => this,
            ResultState.Error => bind(this._error!),
            ResultState.Invalid => throw new InvalidOperationException(),
            _ => throw new UnreachableException()
        };
    }

    /// <summary>
    ///     Matches the <see cref="Result{TOk}" />.
    /// </summary>
    /// <param name="ok">The OK function.</param>
    /// <param name="error">The error function.</param>
    /// <typeparam name="TOut">The output type.</typeparam>
    /// <returns>The matched value.</returns>
    /// <exception cref="InvalidOperationException">Thrown if state is Invalid.</exception>
    /// <exception cref="UnreachableException">Thrown if the state is unknown.</exception>
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public TOut Match<TOut>(Func<TOk, TOut> ok, Func<Error, TOut> error)
    {
        ArgumentNullException.ThrowIfNull(ok);
        ArgumentNullException.ThrowIfNull(error);

        return this._state switch
        {
            ResultState.Ok => ok(this._ok!),
            ResultState.Error => error(this._error!),
            ResultState.Invalid => throw new InvalidOperationException(),
            _ => throw new UnreachableException()
        };
    }

    /// <summary>
    ///     Tries to return the 'OK' value.
    /// </summary>
    /// <param name="ok">The 'OK' value.</param>
    /// <returns>True if OK, false otherwise.</returns>
    /// <exception cref="InvalidOperationException">Thrown if state is Invalid.</exception>
    /// <exception cref="UnreachableException">Thrown if the state is unknown.</exception>
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public bool Try([NotNullWhen(true)] out TOk? ok)
    {
        switch (this._state)
        {
            case ResultState.Ok:
                ok = this._ok!;
                return true;
            case ResultState.Error:
                ok = default;
                return false;
            case ResultState.Invalid:
                throw new InvalidOperationException();
            default:
                throw new UnreachableException();
        }
    }

    /// <summary>
    ///     Tries to get the 'Error' value.
    /// </summary>
    /// <param name="error">The 'Error' value.</param>
    /// <returns>True if error, false otherwise.</returns>
    /// <exception cref="InvalidOperationException">Thrown if state is Invalid.</exception>
    /// <exception cref="UnreachableException">Thrown if the state is unknown.</exception>
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public bool TryError([NotNullWhen(true)] out Error? error)
    {
        switch (this._state)
        {
            case ResultState.Ok:
                error = default;
                return false;
            case ResultState.Error:
                error = this._error!;
                return true;
            case ResultState.Invalid:
                throw new InvalidOperationException();
            default:
                throw new UnreachableException();
        }
    }

    /// <summary>
    ///     Taps a <see cref="Result{TOk}" />.
    /// </summary>
    /// <param name="okAction">The OK action.</param>
    /// <param name="errorAction">The error action.</param>
    /// <returns>The result</returns>
    /// <exception cref="InvalidOperationException">Thrown if state is Invalid.</exception>
    /// <exception cref="UnreachableException">Thrown if the state is unknown.</exception>
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public Result<TOk> Tap(Action<TOk> okAction, Action<Error> errorAction)
    {
        ArgumentNullException.ThrowIfNull(okAction);
        ArgumentNullException.ThrowIfNull(errorAction);

        switch (this._state)
        {
            case ResultState.Ok:
                okAction(this._ok!);
                break;
            case ResultState.Error:
                errorAction(this._error!);
                break;
            case ResultState.Invalid:
                throw new InvalidOperationException();
            default:
                throw new UnreachableException();
        }

        return this;
    }
}