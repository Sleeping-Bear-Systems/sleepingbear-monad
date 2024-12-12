using System.Collections.Immutable;
using SleepingBear.Monad.Errors;

namespace SleepingBear.Monad.Monads.Test;

/// <summary>
///     Tests for <see cref="ValidationExtensions" />.
/// </summary>
internal static class ValidationExtensionsTests
{
    [Test]
    public static void ToValidation_Value_ReturnsIsValid()
    {
        var validation = 1234.ToValidation();
        var (isValid, value, errors) = validation;
        Assert.Multiple(() =>
        {
            Assert.That(isValid, Is.True);
            Assert.That(value, Is.EqualTo(1234));
            Assert.That(errors, Is.Null);
        });
    }

    [Test]
    public static void ToValidation_Errors_ReturnsIsError()
    {
        var validation = ImmutableList<Error>.Empty.Add(new GenericError<string>("error")).ToValidation<int>();
        var (isValid, value, errors) = validation;
        Assert.Multiple(() =>
        {
            Assert.That(isValid, Is.False);
            Assert.That(value, Is.EqualTo(0));
            Assert.That(errors, Has.Count.EqualTo(1));
            errors.TestErrorAt(0, (GenericError<string> e) => { Assert.That(e.Value, Is.EqualTo("error")); });
        });
    }
}