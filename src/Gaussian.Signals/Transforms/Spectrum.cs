namespace Gaussian.Signals;
using System.Numerics;

public static class Spectrum
{
    public static T BinFrequency<T>(
        int bin,
        int sampleCount,
        T samplingRate)
        where T : IFloatingPointIeee754<T>
    {
        return
            T.CreateChecked(bin) *
            samplingRate /
            T.CreateChecked(sampleCount);
    }

    public static T[] Magnitudes<T>(
        IReadOnlyList<Complex<T>> spectrum)
        where T : IFloatingPointIeee754<T>
    {
        var result = new T[spectrum.Count];

        for (var k = 0; k < spectrum.Count; k++)
        {
            result[k] = spectrum[k].Magnitude;
        }

        return result;
    }

    public static T[] MagnitudesToDecibels<T>(
        IReadOnlyList<T> magnitudes)
        where T : IFloatingPointIeee754<T>
    {
        var minimumMagnitude = T.CreateChecked(1e-12);

        var result = new T[magnitudes.Count];

        for (var k = 0; k < magnitudes.Count; k++)
        {
            var flooredMagnitude =
                T.Max(magnitudes[k], minimumMagnitude);

            result[k] =
                T.CreateChecked(20) * T.Log10(flooredMagnitude);
        }

        return result;
    }
}
