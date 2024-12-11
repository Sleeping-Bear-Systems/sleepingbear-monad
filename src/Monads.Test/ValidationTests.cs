using System.Diagnostics.CodeAnalysis;
using SleepingBear.Monad.Errors;

namespace SleepingBear.Monad.Monads.Test;

/// <summary>
///     Test for <see cref="Validation{TValue}" />.
/// </summary>
internal static class ValidationTests
{
    [Test]
    public static void Ctor_Default_ReturnsIsInvalid()
    {
        var validation = new Validation<int>();
        var (isValid, value, errors) = validation;
        Assert.Multiple(() =>
        {
            Assert.That(validation.IsValid, Is.False);
            Assert.That(validation.IsInvalid, Is.True);
            Assert.That(isValid, Is.False);
            Assert.That(value, Is.EqualTo(default(int)));
            Assert.That(errors, Is.Not.Null);
        });
        Assert.That(errors, Is.Empty);
    }

    [Test]
    public static void Ctor_Value_ReturnsIsValid()
    {
        var validation = new Validation<int>(1234);
        var (isValid, value, errors) = validation;
        Assert.Multiple(() =>
        {
            Assert.That(validation.IsValid, Is.True);
            Assert.That(validation.IsInvalid, Is.False);
            Assert.That(isValid, Is.True);
            Assert.That(value, Is.EqualTo(1234));
            Assert.That(errors, Is.Null);
        });
    }

    [Test]
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
    public static void Ctor_Errors_ReturnsIsValid()
    {
        var error = new Error<string>("error");
        var validation = new Validation<int>([error]);
        var (isValid, value, errors) = validation;
        Assert.Multiple(() =>
        {
            Assert.That(validation.IsValid, Is.False);
            Assert.That(validation.IsInvalid, Is.True);
            Assert.That(isValid, Is.False);
            Assert.That(value, Is.EqualTo(default(int)));
            Assert.That(errors, Is.Not.Null);
            var array = errors!.ToArray();
            Assert.That(array, Has.Length.EqualTo(1));
            Assert.That(array[0], Is.EqualTo(error));
        });
    }
}