using System.Diagnostics.CodeAnalysis;
using SleepingBear.Monad.Errors;

namespace SleepingBear.Monad.Monads.Test;

/// <summary>
///     Extension methods for testing.
/// </summary>
internal static class TestExtensions
{
    /// <summary>
    ///     Extension method for casting abstract values to concrete values for testing.
    /// </summary>
    /// <param name="error">The <see cref="Error" /> instance.</param>
    /// <param name="testAction">The test action.</param>
    /// <typeparam name="TError">The concrete type to cast the abstract value to.</typeparam>
    public static void TestErrorOf<TError>(this Error error, Action<TError> testAction)
        where TError : Error
    {
        Assert.That(testAction, Is.Not.Null);
        if (error is not TError concreteValue)
        {
            Assert.Fail($"Error is not of the expected type: {typeof(TError).FullName}");
            return;
        }

        testAction(concreteValue);
    }

    /// <summary>
    ///     Get the <see cref="Error" /> at the specified index and passes ito the supplied test <see cref="Action{T}" />.
    /// </summary>
    /// <param name="errors">The collection of <see cref="Error" />.</param>
    /// <param name="index">The index.</param>
    /// <param name="testAction">The test action.</param>
    /// <typeparam name="TError">The concrete error type.</typeparam>
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
    public static void TestErrorAt<TError>(this IEnumerable<Error>? errors, int index, Action<TError> testAction)
        where TError : Error
    {
        Assert.That(errors, Is.Not.Null);
        var array = errors!.ToArray();
        if (0 <= index && index < array.Length)
        {
            array[index].TestErrorOf(testAction);
            return;
        }

        Assert.Fail("Index out of range.");
    }
}