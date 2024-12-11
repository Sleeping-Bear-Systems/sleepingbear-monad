using System.Diagnostics.CodeAnalysis;

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

    /// <summary>
    ///     Matches the 'Maybe'.
    /// </summary>
    /// <param name="noneValue">The none value.</param>
    /// <returns>The matched value.</returns>
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public TSome Match(TSome noneValue)
    {
        return this.IsSome ? this._value! : noneValue;
    }

    /// <summary>
    ///     Matches the 'Maybe'.
    /// </summary>
    /// <param name="someFunc">The 'Some' function.</param>
    /// <param name="noneFunc">The 'None' function.</param>
    /// <typeparam name="TOut">The output type.</typeparam>
    /// <returns>The matched value.</returns>
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public TOut Match<TOut>(Func<TSome, TOut> someFunc, Func<TOut> noneFunc)
    {
        ArgumentNullException.ThrowIfNull(someFunc);
        ArgumentNullException.ThrowIfNull(noneFunc);

        return this.IsSome ? someFunc(this._value!) : noneFunc();
    }

    /// <summary>
    ///     Maps the 'Maybe'.
    /// </summary>
    /// <param name="map">The mapping function.</param>
    /// <typeparam name="TOut">The output type.</typeparam>
    /// <returns>The mapped 'Maybe'.</returns>
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public Maybe<TOut> Map<TOut>(Func<TSome, TOut> map) where TOut : notnull
    {
        ArgumentNullException.ThrowIfNull(map);

        return this.IsSome ? new Maybe<TOut>(map(this._value!)) : Maybe<TOut>.None;
    }

    /// <summary>
    ///     Binds the 'Maybe'.
    /// </summary>
    /// <param name="bind">The binding function.</param>
    /// <typeparam name="TOut">The output 'Some' type.</typeparam>
    /// <returns>The bound 'Maybe'.</returns>
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public Maybe<TOut> Bind<TOut>(Func<TSome, Maybe<TOut>> bind) where TOut : notnull
    {
        ArgumentNullException.ThrowIfNull(bind);

        return this.IsSome ? bind(this._value!) : Maybe<TOut>.None;
    }

    /// <summary>
    ///     Taps the 'Maybe'.
    /// </summary>
    /// <param name="someAction">The 'Some' action.</param>
    /// <param name="noneAction">The 'None' action.</param>
    /// <returns>The maybe instance</returns>
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public Maybe<TSome> Tap(Action<TSome> someAction, Action noneAction)
    {
        ArgumentNullException.ThrowIfNull(someAction);
        ArgumentNullException.ThrowIfNull(noneAction);

        if (this.IsSome)
        {
            someAction(this._value!);
        }
        else
        {
            noneAction();
        }

        return this;
    }

    /// <summary>
    ///     Tries to get the 'Some' value.
    /// </summary>
    /// <param name="some">The 'Some' value.</param>
    /// <returns>True if is some.</returns>
    public bool Try([NotNullWhen(true)] out TSome? some)
    {
        some = this._value;
        return this.IsSome;
    }
}