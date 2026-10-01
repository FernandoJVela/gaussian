namespace Gaussian.Signals;

public static class Fft
{
    /// <summary>
    /// Computes the forward radix-2 Cooley-Tukey FFT.
    ///
    /// The transform uses the convention:
    /// X[k] = sum(x[n] * exp(-i * 2πkn/N))
    ///
    /// The result is not normalized.
    /// </summary>
    public static Complex<T>[] Transform<T>(
        IReadOnlyList<T> samples)
        where T : System.Numerics.IFloatingPointIeee754<T>
    {
        ArgumentNullException.ThrowIfNull(samples);

        if (samples.Count == 0)
        {
            throw new ArgumentException(
                "FFT requires at least one sample.",
                nameof(samples));
        }

        var n = samples.Count;

        if ((n & (n - 1)) != 0)
        {
            throw new ArgumentException(
                "FFT input length must be a power of two.",
                nameof(samples));
        }

        var result = new Complex<T>[n];

        for (var i = 0; i < n; i++)
        {
            result[i] = new Complex<T>(
                samples[i],
                T.Zero);
        }

        // Bit-reversal permutation.
        var j = 0;

        for (var i = 1; i < n; i++)
        {
            var bit = n >> 1;

            while ((j & bit) != 0)
            {
                j ^= bit;
                bit >>= 1;
            }

            j ^= bit;

            if (i < j)
            {
                (result[i], result[j]) =
                    (result[j], result[i]);
            }
        }

        // Iterative radix-2 Cooley-Tukey stages.
        for (var length = 2;
             length <= n;
             length <<= 1)
        {
            var angle =
                -T.CreateChecked(2) *
                T.Pi /
                T.CreateChecked(length);

            var wLength =
                new Complex<T>(
                    T.Cos(angle),
                    T.Sin(angle));

            var halfLength = length / 2;

            for (var start = 0;
                 start < n;
                 start += length)
            {
                var w =
                    new Complex<T>(
                        T.One,
                        T.Zero);

                for (var offset = 0;
                     offset < halfLength;
                     offset++)
                {
                    var evenIndex = start + offset;
                    var oddIndex = evenIndex + halfLength;

                    var even = result[evenIndex];
                    var odd = result[oddIndex];

                    var product = w * odd;

                    result[evenIndex] =
                        even + product;

                    result[oddIndex] =
                        even - product;

                    w *= wLength;
                }
            }
        }

        return result;
    }
}
