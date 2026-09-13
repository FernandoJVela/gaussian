namespace Gaussian.Signals;

public static class RootsOfUnity
{
    public static IReadOnlyList<Complex<T>> Create<T>(int n)
        where T : System.Numerics.IFloatingPointIeee754<T>
    {
        if (n < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(n));
        }

        var result = new Complex<T>[n];

        for (var k = 0; k < n; k++)
        {
            var angle =
                T.CreateChecked(2) *
                T.Pi *
                T.CreateChecked(k) /
                T.CreateChecked(n);

            result[k] =
                Complex<T>.FromPolar(
                    T.One,
                    angle);
        }

        return result;
    }
}