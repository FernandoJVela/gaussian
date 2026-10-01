namespace Gaussian.Signals;
using System.Numerics;

public static class Dft
{
    public static Complex<T>[] Transform<T>(
        IReadOnlyList<T> samples)
        where T : IFloatingPointIeee754<T>
    {
        var n = samples.Count;

        var result = new Complex<T>[n];

        for (var k = 0; k < n; k++)
        {
            var real = T.Zero;
            var imaginary = T.Zero;

            for (var sampleIndex = 0;
                 sampleIndex < n;
                 sampleIndex++)
            {
                var angle =
                    -T.CreateChecked(2) *
                    T.Pi *
                    T.CreateChecked(k) *
                    T.CreateChecked(sampleIndex) /
                    T.CreateChecked(n);

                var sample = samples[sampleIndex];

                real +=
                    sample * T.Cos(angle);

                imaginary +=
                    sample * T.Sin(angle);
            }

            result[k] =
                new Complex<T>(
                    real,
                    imaginary);
        }

        return result;
    }
}