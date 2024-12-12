using SleepingBear.Monad.Errors;

namespace SleepingBear.Monad.Monads;

/// <summary>
///     Result monad.
/// </summary>
/// <typeparam name="TOk">The type of the lifted value.</typeparam>
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
    /// Implicit operator to convert a value to <see cref="Result{TOk}"/>.
    /// </summary>
    /// <param name="ok">The value being lifted.</param>
    /// <returns>A <see cref="Result{TOk}"/> containing the lifted value.</returns>
    public static implicit operator Result<TOk>(TOk ok)
    {
        return new Result<TOk>(ok);
    }

    /// <summary>
    /// Implicit operator to convert a <see cref="Error"/> to a <see cref="Result{TOk}"/>.
    /// </summary>
    /// <param name="error">The error being lifted.</param>
    /// <returns>A <see cref="Result{TOk}"/> containing the lifted error.</returns>
    public static implicit operator Result<TOk>(Error error)
    {
        return new Result<TOk>(error);
    }

    /// <summary>
    /// Alternate for operator implicit.
    /// </summary>
    /// <returns>A <see cref="Result{TOk}"/>.</returns>
    /// <remarks>
    /// Reference: https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca2225
    /// </remarks>
    // ReSharper disable once UnusedMember.Global
    public Result<TOk> ToResult()
    {
        return this;
    }
}