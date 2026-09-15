using Gaussian.Signals;

namespace Gaussian.Signals.Tests;

public class SignalGeneratorTests
{
    [Fact]
    public void Sine_ShouldGenerateExpectedNumberOfSamples()
    {
        var signal =
            SignalGenerator.Sine<double>(
                frequency: 100,
                samplingRate: 1000,
                duration: 1);

        Assert.Equal(1000, signal.Length);
    }

    [Fact]
    public void Sine_ShouldGenerateValuesBetweenMinusOneAndOne()
    {
        var signal =
            SignalGenerator.Sine<double>(
                frequency: 100,
                samplingRate: 1000,
                duration: 1);

        Assert.All(
            signal.Samples,
            sample =>
            {
                Assert.InRange(sample, -1.0, 1.0);
            });
    }

    [Fact]
    public void SumOfSines_ShouldGenerateExpectedSamples()
    {
        var signal = SignalGenerator.SumOfSines(
            new[] { 10.0 },
            samplingRate: 100.0,
            duration: 0.1);

        Assert.Equal(0.0, signal.Samples[0], precision: 12);

        Assert.Equal(
            Math.Sin(2 * Math.PI * 10 * 0.01),
            signal.Samples[1],
            precision: 12);
    }

    [Fact]
    public void WhiteGaussianNoise_WithSameSeed_ShouldBeReproducible()
    {
        var first = SignalGenerator.WhiteGaussianNoise(
            1000.0,
            1.0,
            0.0,
            1.0,
            seed: 42);

        var second = SignalGenerator.WhiteGaussianNoise(
            1000.0,
            1.0,
            0.0,
            1.0,
            seed: 42);

        Assert.Equal(
            first.Samples,
            second.Samples);
    }

    [Fact]
    public void Chirp_ShouldGenerateExpectedNumberOfSamples()
    {
        var signal =
            SignalGenerator.Chirp<double>(
                samplingRate: 1000,
                duration: 1,
                startFrequency: 100,
                endFrequency: 200);

        Assert.Equal(1000, signal.Length);
    }

    [Fact]
    public void Chirp_WithEqualStartAndEndFrequency_ShouldMatchSine()
    {
        var chirp =
            SignalGenerator.Chirp<double>(
                samplingRate: 1000,
                duration: 1,
                startFrequency: 100,
                endFrequency: 100);

        var sine =
            SignalGenerator.Sine<double>(
                frequency: 100,
                samplingRate: 1000,
                duration: 1);

        Assert.Equal(sine.Length, chirp.Length);

        for (var n = 0; n < chirp.Length; n++)
        {
            NumericAssert.Equal(
                sine.Samples[n],
                chirp.Samples[n]);
        }
    }

    [Fact]
    public void ImpulseTrain_ShouldPlaceImpulsesAtExpectedPeriod()
    {
        var signal = SignalGenerator.ImpulseTrain(
            samplingRate: 1000.0,
            duration: 0.01,
            period: 0.004,
            amplitude: 2.0);

        Assert.Equal(2.0, signal.Samples[0]);
        Assert.Equal(0.0, signal.Samples[1]);
        Assert.Equal(0.0, signal.Samples[2]);
        Assert.Equal(0.0, signal.Samples[3]);
        Assert.Equal(2.0, signal.Samples[4]);
    }
}