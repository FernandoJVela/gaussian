using BenchmarkDotNet.Attributes;
using Gaussian.Signals;

namespace Gaussian.Signals.Benchmarks;

[MemoryDiagnoser]
public class TransformBenchmarks
{
    [Params(64, 128, 256, 512, 1024)]
    public int N { get; set; }

    private double[] _samples = null!;

    [GlobalSetup]
    public void Setup()
    {
        var random = new Random(42);

        _samples = Enumerable
            .Range(0, N)
            .Select(_ => random.NextDouble() * 2.0 - 1.0)
            .ToArray();
    }

    [Benchmark(Baseline = true)]
    public Complex<double>[] Dft()
    {
        return Gaussian.Signals.Dft.Transform(_samples);
    }

    [Benchmark]
    public Complex<double>[] Fft()
    {
        return Gaussian.Signals.Fft.Transform(_samples);
    }
}
