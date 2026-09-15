namespace Gaussian.Signals;

public readonly struct Signal<T>
    where T : System.Numerics.IFloatingPointIeee754<T>
{
    public IReadOnlyList<T> Samples { get; }

    public T SamplingRate { get; }

    public Signal(
        IReadOnlyList<T> samples,
        T samplingRate)
    {
        if (samplingRate <= T.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(samplingRate));
        }

        Samples = samples;
        SamplingRate = samplingRate;
    }

    public int Length => Samples.Count;

    public T Duration =>
        T.CreateChecked(Length) / SamplingRate;
}