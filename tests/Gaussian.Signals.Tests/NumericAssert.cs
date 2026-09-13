namespace Gaussian.Signals.Tests;

public static class NumericAssert
{
    public const double DefaultTolerance = 1e-12;

    public static void Equal(
        double expected,
        double actual,
        double tolerance = DefaultTolerance)
    {
        Assert.True(
            Math.Abs(expected - actual) <= tolerance,
            $"Expected {expected}, Actual {actual}, " +
            $"Difference {Math.Abs(expected - actual)} " +
            $"exceeded tolerance {tolerance}.");
    }
}