namespace Gaussian.Signals;

public interface IWindow<T>
    where T : System.Numerics.IFloatingPointIeee754<T>
{
    T Evaluate(int index, int length);
}
