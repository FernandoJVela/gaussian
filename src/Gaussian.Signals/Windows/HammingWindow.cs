namespace Gaussian.Signals;

public sealed class HammingWindow<T> : IWindow<T>
    where T : System.Numerics.IFloatingPointIeee754<T>
{
    public T Evaluate(int index, int length)
    {
        ValidateArguments(index, length);

        if (length == 1)
        {
            return T.One;
        }

        var n = T.CreateChecked(index);
        var denominator = T.CreateChecked(length - 1);

        var angle =
            T.CreateChecked(2) *
            T.Pi *
            n /
            denominator;

        return
            T.CreateChecked(0.54) -
            T.CreateChecked(0.46) *
            T.Cos(angle);
    }

    private static void ValidateArguments(int index, int length)
    {
        if (length <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length));
        }

        if ((uint)index >= (uint)length)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }
    }
}
