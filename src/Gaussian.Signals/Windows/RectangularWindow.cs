namespace Gaussian.Signals;

public sealed class RectangularWindow<T> : IWindow<T>
    where T : System.Numerics.IFloatingPointIeee754<T>
{
    public T Evaluate(int index, int length)
    {
        ValidateArguments(index, length);

        return T.One;
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
