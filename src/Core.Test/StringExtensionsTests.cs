namespace SleepingBear.Monad.Core.Test;

/// <summary>
///     Tests for <see cref="StringExtensions" />.
/// </summary>
internal static class StringExtensionsTests
{
    [TestCase(null, null, ExpectedResult = "", TestName = "if value and nullValue are null, return empty string")]
    [TestCase(null, "nullValue", ExpectedResult = "nullValue",
        TestName = "if value is null but nullValue is not, return nullValue")]
    [TestCase("value", "nullValue", ExpectedResult = "value", TestName = "if value is not null, return value")]
    public static string IfNull_ValidatesBehavior(string? value, string? nullValue)
    {
        return value.IfNull(nullValue);
    }

    [TestCase(null, null, ExpectedResult = "", TestName = "if value and nullValue are null, return empty string")]
    [TestCase(null, "nullValue", ExpectedResult = "nullValue",
        TestName = "if value is null but nullValue is not, return nullValue")]
    [TestCase("", "nullValue", ExpectedResult = "nullValue",
        TestName = "if value is empty but nullValue is not, return nullValue")]
    [TestCase("value", "nullValue", ExpectedResult = "value", TestName = "if value is not null or empty, return value")]
    public static string IsNullOrEmpty_ValidatesBehavior(string? value, string? nullValue)
    {
        return value.IfNullOrEmpty(nullValue);
    }

    [TestCase(null, null, ExpectedResult = "", TestName = "if value and nullValue are null, return empty string")]
    [TestCase(null, "nullValue", ExpectedResult = "nullValue",
        TestName = "if value is null but nullValue is not, return nullValue")]
    [TestCase("", "nullValue", ExpectedResult = "nullValue",
        TestName = "if value is empty but nullValue is not, return nullValue")]
    [TestCase("  ", "nullValue", ExpectedResult = "nullValue",
        TestName = "if value is whitespace but nullValue is not, return nullValue")]
    [TestCase("  ", "   ", ExpectedResult = "", TestName = "if value and nullValue are whitespace, return nullValue")]
    [TestCase("value", "nullValue", ExpectedResult = "value", TestName = "if value is not null or empty, return value")]
    public static string IsNullOrWhitespace_ValidatesBehavior(string? value, string? nullValue)
    {
        return value.IfNullOrWhitespace(nullValue);
    }
}