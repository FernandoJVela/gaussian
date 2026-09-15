using Gaussian.Signals;

namespace Gaussian.Signals.Tests;

public class DftTests
{
    [Fact]
    public void Dft_ConstantSignal_ShouldHaveOnlyDcComponent()
    {
        var samples = Enumerable
            .Repeat(1.0, 8)
            .ToArray();

        var spectrum = Dft.Transform(samples);

        Assert.Equal(8.0, spectrum[0].Real, 12);

        for (var k = 1; k < spectrum.Length; k++)
        {
            Assert.Equal(0.0, spectrum[k].Magnitude, 12);
        }
    }

    [Fact]
    public void Dft_PureSine_ShouldPeakAtBinMatchingFrequency()
    {
        const double frequency = 50.0;
        const double samplingRate = 1000.0;
        const double duration = 1.0;

        var signal = SignalGenerator.Sine<double>(
            frequency: frequency,
            samplingRate: samplingRate,
            duration: duration);

        var spectrum = Dft.Transform(signal.Samples);

        // With duration = 1s, each bin spans exactly 1 Hz, so the sine's
        // energy should land squarely on the bin equal to its frequency.
        var expectedBin = (int)(frequency * duration);
        var mirrorBin = spectrum.Length - expectedBin;

        NumericAssert.Equal(
            signal.Length / 2.0,
            spectrum[expectedBin].Magnitude,
            tolerance: 1e-9);

        NumericAssert.Equal(
            signal.Length / 2.0,
            spectrum[mirrorBin].Magnitude,
            tolerance: 1e-9);

        for (var k = 0; k < spectrum.Length; k++)
        {
            if (k == expectedBin || k == mirrorBin)
            {
                continue;
            }

            NumericAssert.Equal(
                0.0,
                spectrum[k].Magnitude,
                tolerance: 1e-9);
        }
    }

    [Fact]
    public void BinFrequency_ShouldComputeFrequencyForBin()
    {
        var frequency = Spectrum.BinFrequency(
            bin: 50,
            sampleCount: 1000,
            samplingRate: 1000.0);

        Assert.Equal(50.0, frequency, 12);
    }

    [Fact]
    public void Dft_MultiToneSignal_ShouldIdentifyTopThreePositiveFrequencyPeaks()
    {
        const double samplingRate = 1000.0;
        const double duration = 1.0;

        var frequencies = new[] { 100.0, 250.0, 400.0 };

        var signal = SignalGenerator.SumOfSines(
            frequencies,
            samplingRate: samplingRate,
            duration: duration);

        var spectrum = Dft.Transform(signal.Samples);

        var magnitudes = Spectrum.Magnitudes(spectrum);

        // A real-valued signal's DFT is mirrored: 100/250/400 Hz appear
        // again as 900/750/600 Hz. Restrict to the positive-frequency
        // half, 0 <= k <= N/2, before ranking peaks.
        var nyquistBin = signal.Length / 2;

        var topFrequencies = Enumerable.Range(0, nyquistBin + 1)
            .OrderByDescending(k => magnitudes[k])
            .Take(frequencies.Length)
            .OrderBy(k => k)
            .Select(k => Spectrum.BinFrequency(k, signal.Length, samplingRate))
            .ToArray();

        Assert.Equal(
            frequencies,
            topFrequencies);
    }
}