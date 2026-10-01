using Gaussian.Signals;

namespace Gaussian.Signals.Tests;

public class FftTests
{
    [Fact]
    public void Transform_ShouldRejectEmptyInput()
    {
        Assert.Throws<ArgumentException>(
            () => Fft.Transform(Array.Empty<double>()));
    }

    [Theory]
    [InlineData(3)]
    [InlineData(6)]
    [InlineData(10)]
    [InlineData(12)]
    public void Transform_ShouldRejectNonPowerOfTwoInput(int length)
    {
        var samples = new double[length];

        Assert.Throws<ArgumentException>(
            () => Fft.Transform(samples));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(4)]
    [InlineData(8)]
    [InlineData(16)]
    [InlineData(32)]
    public void Transform_ShouldMatchDft_ForImpulse(int length)
    {
        var samples = new double[length];
        samples[0] = 1.0;

        AssertMatchesDft(samples);
    }

    [Fact]
    public void Transform_ShouldMatchDft_ForConstantSignal()
    {
        var samples =
            Enumerable
                .Repeat(1.0, 32)
                .ToArray();

        AssertMatchesDft(samples);
    }

    [Fact]
    public void Transform_ShouldMatchDft_ForBinAlignedSine()
    {
        const int sampleCount = 64;
        const double samplingRate = 1024.0;
        const double frequency = 128.0;

        var samples = Enumerable
            .Range(0, sampleCount)
            .Select(n =>
                Math.Sin(
                    2.0 *
                    Math.PI *
                    frequency *
                    n /
                    samplingRate))
            .ToArray();

        AssertMatchesDft(samples);
    }

    [Fact]
    public void Transform_ShouldMatchDft_ForDeterministicRandomInput()
    {
        var random = new Random(42);

        var samples = Enumerable
            .Range(0, 128)
            .Select(_ => random.NextDouble() * 2.0 - 1.0)
            .ToArray();

        AssertMatchesDft(samples);
    }

    [Fact]
    public void Transform_ShouldPreserveHermitianSymmetry_ForRealInput()
    {
        var random = new Random(42);

        var samples = Enumerable
            .Range(0, 64)
            .Select(_ => random.NextDouble() * 2.0 - 1.0)
            .ToArray();

        var spectrum = Fft.Transform(samples);

        for (var k = 1; k < spectrum.Length; k++)
        {
            var mirrorIndex = spectrum.Length - k;

            AssertClose(
                spectrum[k].Real,
                spectrum[mirrorIndex].Real);

            AssertClose(
                spectrum[k].Imaginary,
                -spectrum[mirrorIndex].Imaginary);
        }
    }

    private static void AssertMatchesDft(
        IReadOnlyList<double> samples)
    {
        var expected = Dft.Transform(samples);
        var actual = Fft.Transform(samples);

        for (var k = 0; k < samples.Count; k++)
        {
            AssertClose(
                expected[k].Real,
                actual[k].Real);

            AssertClose(
                expected[k].Imaginary,
                actual[k].Imaginary);
        }
    }

    private static void AssertClose(
        double expected,
        double actual,
        double absoluteTolerance = 1e-12,
        double relativeTolerance = 1e-12)
    {
        var difference = Math.Abs(actual - expected);
        var tolerance =
            absoluteTolerance +
            relativeTolerance * Math.Abs(expected);

        Assert.True(
            difference <= tolerance,
            $"Expected {expected:G17}, actual {actual:G17}, " +
            $"difference {difference:E3}, tolerance {tolerance:E3}.");
    }
}
