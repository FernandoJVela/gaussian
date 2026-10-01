namespace Gaussian.Signals;

public static class Windowing
{
    public static T[] Apply<T>(
        IReadOnlyList<T> samples,
        IWindow<T> window)
        where T : System.Numerics.IFloatingPointIeee754<T>
    {
        ArgumentNullException.ThrowIfNull(samples);
        ArgumentNullException.ThrowIfNull(window);

        var result = new T[samples.Count];

        for (var i = 0; i < samples.Count; i++)
        {
            result[i] =
                samples[i] *
                window.Evaluate(i, samples.Count);
        }

        return result;
    }

    public static T CoherentGain<T>(
        IWindow<T> window,
        int length)
        where T : System.Numerics.IFloatingPointIeee754<T>
    {
        ArgumentNullException.ThrowIfNull(window);

        if (length <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length));
        }

        var sum = T.Zero;

        for (var i = 0; i < length; i++)
        {
            sum += window.Evaluate(i, length);
        }

        return sum / T.CreateChecked(length);
    }
}
