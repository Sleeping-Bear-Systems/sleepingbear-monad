using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using SleepingBear.Monad.Errors;

namespace SleepingBear.Monad.Monads;

/// <summary>
///     Result monad.
/// </summary>
/// <typeparam name="TOk">The OK type.</typeparam>
public readonly record struct Result<TOk>
    where TOk : notnull
{
    private readonly Error? _error;
    private readonly TOk? _ok;

    /// <summary>
    ///     Default constructor.
    /// </summary>
    public Result()
    {
        this.IsOk = false;
        this._ok = default;
        this._error = new UnknownError();
    }

    internal Result(TOk ok)
    {
        this.IsOk = true;
        this._ok = ok;
        this._error = null;
    }

    internal Result(Error error)
    {
        this.IsOk = false;
        this._ok = default;
        this._error = error;
    }

    /// <summary>
    ///     Indicates the result is in the 'OK' state.
    /// </summary>
    public bool IsOk { get; }

    /// <summary>
    ///     Indicates the result in the 'Error' state.
    /// </summary>
    public bool IsError => !this.IsOk;

    /// <summary>
    ///     Deconstructs the result.
    /// </summary>
    /// <param name="isOk">Flag indicating the OK state.</param>
    /// <param name="ok">The OK value.</param>
    /// <param name="error">The error value.</param>
    public void Deconstruct(out bool isOk, out TOk? ok, out Error? error)
    {
        (isOk, ok, error) = (this.IsOk, this._ok, this._error);
    }

    /// <summary>
    ///     Map a <see cref="Result{TOk}" />.
    /// </summary>
    /// <param name="mapFunc">The mapping function.</param>
    /// <typeparam name="TOkOut">The output OK type.</typeparam>
    /// <returns>A <see cref="Result{TOk}" />.</returns>
    /// <exception cref="InvalidOperationException">Thrown if state is Invalid.</exception>
    /// <exception cref="UnreachableException">Thrown if the state is unknown.</exception>
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public Result<TOkOut> Map<TOkOut>(Func<TOk, TOkOut> mapFunc) where TOkOut : notnull
    {
        ArgumentNullException.ThrowIfNull(mapFunc);

        return this.IsOk
            ? new Result<TOkOut>(mapFunc(this._ok!))
            : new Result<TOkOut>(this._error!);
    }

    /// <summary>
    ///     Map a <see cref="Result{TOk}" />.
    /// </summary>
    /// <param name="mapErrorFunc">The mapping function.</param>
    /// <returns>A <see cref="Result{TOk}" />.</returns>
    /// <exception cref="InvalidOperationException">Thrown if state is Invalid.</exception>
    /// <exception cref="UnreachableException">Thrown if the state is unknown.</exception>
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public Result<TOk> MapError(Func<Error, Error> mapErrorFunc)
    {
        ArgumentNullException.ThrowIfNull(mapErrorFunc);

        return this.IsOk
            ? new Result<TOk>(this._ok!)
            : new Result<TOk>(mapErrorFunc(this._error!));
    }

    /// <summary>
    ///     Binds a <see cref="Result{TOk}" />.
    /// </summary>
    /// <param name="bindFunc">The binding function.</param>
    /// <typeparam name="TOkOut">The output OK type.</typeparam>
    /// <returns>A <see cref="Result{TOk}" />.</returns>
    /// <exception cref="InvalidOperationException">Thrown if state is Invalid.</exception>
    /// <exception cref="UnreachableException">Thrown if the state is unknown.</exception>
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public Result<TOkOut> Bind<TOkOut>(Func<TOk, Result<TOkOut>> bindFunc) where TOkOut : notnull
    {
        ArgumentNullException.ThrowIfNull(bindFunc);

        return this.IsOk
            ? bindFunc(this._ok!)
            : new Result<TOkOut>(this._error!);
    }

    /// <summary>
    ///     Bind a <see cref="Result{TOk}" />.
    /// </summary>
    /// <param name="bindFunc">The binding function.</param>
    /// <returns>A <see cref="Result{TOk}" />.</returns>
    /// <exception cref="InvalidOperationException">Thrown if state is Invalid.</exception>
    /// <exception cref="UnreachableException">Thrown if the state is unknown.</exception>
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public Result<TOk> BindError(Func<Error, Result<TOk>> bindFunc)
    {
        ArgumentNullException.ThrowIfNull(bindFunc);

        return this.IsOk
            ? new Result<TOk>(this._ok!)
            : bindFunc(this._error!);
    }

    /// <summary>
    ///     Matches the <see cref="Result{TOk}" />.
    /// </summary>
    /// <param name="okFunc">The OK function.</param>
    /// <param name="errorFunc">The error function.</param>
    /// <typeparam name="TOut">The output type.</typeparam>
    /// <returns>The matched value.</returns>
    /// <exception cref="InvalidOperationException">Thrown if state is Invalid.</exception>
    /// <exception cref="UnreachableException">Thrown if the state is unknown.</exception>
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public TOut Match<TOut>(Func<TOk, TOut> okFunc, Func<Error, TOut> errorFunc)
    {
        ArgumentNullException.ThrowIfNull(okFunc);
        ArgumentNullException.ThrowIfNull(errorFunc);

        return this.IsOk
            ? okFunc(this._ok!)
            : errorFunc(this._error!);
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
        ok = this.IsOk
            ? this._ok!
            : default;
        return this.IsOk;
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
        error = this.IsOk ? default : this._error!;
        return !this.IsOk;
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

        if (this.IsOk)
        {
            okAction(this._ok!);
        }
        else
        {
            errorAction(this._error!);
        }

        return this;
    }
}