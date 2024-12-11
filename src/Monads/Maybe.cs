namespace SleepingBear.Monad.Monads;

/// <summary>
///     Maybe monad.
/// </summary>
/// <typeparam name="TSome">The type of the value being lifted.</typeparam>
public readonly record struct Maybe<TSome> where TSome : notnull
{
    /// <summary>
    ///     None instance.
    /// </summary>
    public static readonly Maybe<TSome> None = new();

    private readonly TSome? _value;

    /// <summary>
    ///     Default constructor.
    /// </summary>
    public Maybe()
    {
        this.IsSome = false;
        this._value = default;
    }

    internal Maybe(TSome? value)
    {
        this.IsSome = value is not null;
        this._value = value;
    }

    /// <summary>
    ///     Flag indicating a value has been lifted.
    /// </summary>
    public bool IsSome { get; }

    /// <summary>
    ///     Flag indicating no value has been lifted.
    /// </summary>
    public bool IsNone => !this.IsSome;

    /// <summary>
    ///     Deconstructs the monad.
    /// </summary>
    /// <param name="isSome">The isSome flag.</param>
    /// <param name="value">The lifted value.</param>
    public void Deconstruct(out bool isSome, out TSome? value)
    {
        (isSome, value) = (this.IsSome, this._value);
    }
}
