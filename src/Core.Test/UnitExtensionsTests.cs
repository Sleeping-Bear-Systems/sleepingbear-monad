namespace SleepingBear.Monad.Core.Test;

/// <summary>
///     Tests for <see cref="UnitExtensions" />.
/// </summary>
internal static class UnitExtensionsTests
{
    [Test]
    public static void ToFunc_ZeroParameters_ValidatesBehavior()
    {
        var action = new Action(() => { });
        var func = action.ToFunc();
        func();
    }

    [Test]
    public static void ToFunc_OneParameter_ValidatesBehavior()
    {
        var action = new Action<int>(t1 => { Assert.That(t1, Is.EqualTo(1)); });
        var func = action.ToFunc();
        func(1);
    }

    [Test]
    public static void ToFunc_TwoParameters_ValidatesBehavior()
    {
        var action = new Action<int, int>((t1, t2) => { Assert.That(t1 + t2 * 10, Is.EqualTo(21)); });
        var func = action.ToFunc();
        func(1, 2);
    }

    [Test]
    public static void ToFunc_ThreeParameters_ValidatesBehavior()
    {
        var action = new Action<int, int, int>((t1, t2, t3) =>
        {
            Assert.That(t1 + t2 * 10 + t3 * 100, Is.EqualTo(321));
        });
        var func = action.ToFunc();
        func(1, 2, 3);
    }

    [Test]
    public static void ToFunc_FourParameters_ValidatesBehavior()
    {
        var action = new Action<int, int, int, int>((t1, t2, t3, t4) =>
        {
            Assert.That(t1 + t2 * 10 + t3 * 100 + t4 * 1000, Is.EqualTo(4321));
        });
        var func = action.ToFunc();
        func(1, 2, 3, 4);
    }

    [Test]
    public static void ToFunc_FiveParameters_ValidatesBehavior()
    {
        var action = new Action<int, int, int, int, int>((t1, t2, t3, t4, t5) =>
        {
            Assert.That(t1 + t2 * 10 + t3 * 100 + t4 * 1000 + t5 * 10000, Is.EqualTo(54321));
        });
        var func = action.ToFunc();
        func(1, 2, 3, 4, 5);
    }

    [Test]
    public static void ToFunc_SixParameters_ValidatesBehavior()
    {
        var action = new Action<int, int, int, int, int, int>((t1, t2, t3, t4, t5, t6) =>
        {
            Assert.That(t1 + t2 * 10 + t3 * 100 + t4 * 1000 + t5 * 10000 + t6 * 100000, Is.EqualTo(654321));
        });
        var func = action.ToFunc();
        func(1, 2, 3, 4, 5, 6);
    }
}
