namespace Gaussian.Signals;
using System.Numerics;

public static class SignalGenerator
{

    private static int CalculateSampleCount<T>(
        T samplingRate,
        T duration)
        where T : IFloatingPointIeee754<T>
    {
        if (samplingRate <= T.Zero)
            throw new ArgumentOutOfRangeException(nameof(samplingRate));

        if (duration <= T.Zero)
            throw new ArgumentOutOfRangeException(nameof(duration));

        return int.CreateChecked(samplingRate * duration);
    }

    private static T NextGaussian<T>(
        Random random)
        where T : IFloatingPointIeee754<T>
    {
        // Avoid log(0).
        var u1 = T.CreateChecked(random.NextDouble());

        var u2 = T.CreateChecked(random.NextDouble());

        var twoPi = T.CreateChecked(2) * T.Pi;

        var radius = T.Sqrt(
            T.CreateChecked(-2) * T.Log(u1));

        var angle = twoPi * u2;

        return radius * T.Cos(angle);
    }

    public static Signal<T> Sine<T>(
        T frequency,
        T samplingRate,
        T duration)
        where T : System.Numerics.IFloatingPointIeee754<T>
    {
        if (frequency < T.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(frequency));
        }

        var sampleCount = CalculateSampleCount(
            samplingRate, 
            duration);

        var samples = new T[sampleCount];

        for (var n = 0; n < sampleCount; n++)
        {
            var time =
                T.CreateChecked(n) / samplingRate;

            samples[n] =
                T.Sin(
                    T.CreateChecked(2) *
                    T.Pi *
                    frequency *
                    time);
        }

        return new Signal<T>(
            samples,
            samplingRate);
    }

    public static Signal<T> SumOfSines<T>(
        IEnumerable<T> frequencies,
        T samplingRate,
        T duration)
        where T : IFloatingPointIeee754<T>
    {
        ArgumentNullException.ThrowIfNull(frequencies);

        var frequencyList = frequencies.ToList();

        if (frequencyList.Count == 0)
            throw new ArgumentException(
                "At least one frequency is required.",
                nameof(frequencies));

        foreach (var frequency in frequencyList)
        {
            if (frequency <= T.Zero)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(frequencies),
                    "Frequencies must be greater than zero.");
            }
        }

        var sampleCount = CalculateSampleCount(
            samplingRate,
            duration);

        var samples = new T[sampleCount];

        var twoPi = T.CreateChecked(2) * T.Pi;

        for (var n = 0; n < sampleCount; n++)
        {
            var t = T.CreateChecked(n) / samplingRate;

            var value = T.Zero;

            foreach (var frequency in frequencyList)
            {
                value += T.Sin(twoPi * frequency * t);
            }

            samples[n] = value;
        }

        return new Signal<T>(
            samples,
            samplingRate);
    }

    public static Signal<T> WhiteGaussianNoise<T>(
        T samplingRate,
        T duration,
        T mean,
        T standardDeviation,
        int seed)
        where T : IFloatingPointIeee754<T>
    {
        if (standardDeviation < T.Zero)
        {
            throw new ArgumentOutOfRangeException(
                nameof(standardDeviation),
                "Standard deviation cannot be negative.");
        }

        var sampleCount = CalculateSampleCount(
            samplingRate,
            duration);

        var random = new Random(seed);

        var samples = new T[sampleCount];

        for (var n = 0; n < sampleCount; n++)
        {
            var standardNormal = NextGaussian<T>(random);

            samples[n] =
                mean + standardDeviation * standardNormal;
        }

        return new Signal<T>(
            samples,
            samplingRate);
    }

    public static Signal<T> Chirp<T>(
        T samplingRate,
        T duration,
        T startFrequency,
        T endFrequency)
        where T : IFloatingPointIeee754<T>
    {
        if (startFrequency < T.Zero)
        {
            throw new ArgumentOutOfRangeException(
                nameof(startFrequency));
        }

        if (endFrequency < T.Zero)
        {
            throw new ArgumentOutOfRangeException(
                nameof(endFrequency));
        }

        var sampleCount = CalculateSampleCount(
            samplingRate,
            duration);

        var samples = new T[sampleCount];

        var k =
            (endFrequency - startFrequency) / duration;

        var twoPi = T.CreateChecked(2) * T.Pi;

        var half = T.CreateChecked(0.5);

        for (var n = 0; n < sampleCount; n++)
        {
            var t = T.CreateChecked(n) / samplingRate;

            var phase =
                twoPi *
                (
                    startFrequency * t
                    + half * k * t * t
                );

            samples[n] = T.Sin(phase);
        }

        return new Signal<T>(
            samples,
            samplingRate);
    }

    public static Signal<T> ImpulseTrain<T>(
        T samplingRate,
        T duration,
        T period,
        T amplitude)
        where T : IFloatingPointIeee754<T>
    {
        if (period <= T.Zero)
        {
            throw new ArgumentOutOfRangeException(
                nameof(period),
                "Period must be greater than zero.");
        }

        var sampleCount = CalculateSampleCount(
            samplingRate,
            duration);

        var samples = new T[sampleCount];

        var samplesPerPeriod =
            int.CreateChecked(
                T.Round(period * samplingRate));

        if (samplesPerPeriod <= 0)
        {
            throw new ArgumentException(
                "The period is too small for the sampling rate.",
                nameof(period));
        }

        for (var n = 0; n < sampleCount; n++)
        {
            samples[n] =
                n % samplesPerPeriod == 0
                    ? amplitude
                    : T.Zero;
        }

        return new Signal<T>(
            samples,
            samplingRate);
    }
}