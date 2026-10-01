using Gaussian.Signals;

namespace Gaussian.Signals.Tests;

public class WindowTests
{
    [Fact]
    public void Rectangular_ShouldReturnOneEverywhere()
    {
        var window = new RectangularWindow<double>();

        for (var i = 0; i < 16; i++)
        {
            Assert.Equal(
                1.0,
                window.Evaluate(i, 16));
        }
    }

    [Fact]
    public void Hann_ShouldBeZeroAtBothEndpoints()
    {
        var window = new HannWindow<double>();

        Assert.Equal(0.0, window.Evaluate(0, 16), 12);
        Assert.Equal(0.0, window.Evaluate(15, 16), 12);
    }

    [Fact]
    public void Hamming_ShouldHaveExpectedEndpointValue()
    {
        var window = new HammingWindow<double>();

        Assert.Equal(0.08, window.Evaluate(0, 16), 12);
        Assert.Equal(0.08, window.Evaluate(15, 16), 12);
    }

    [Fact]
    public void Blackman_ShouldBeApproximatelyZeroAtBothEndpoints()
    {
        var window = new BlackmanWindow<double>();

        Assert.Equal(0.0, window.Evaluate(0, 16), 12);
        Assert.Equal(0.0, window.Evaluate(15, 16), 12);
    }

    [Theory]
    [InlineData(16)]
    [InlineData(17)]
    [InlineData(64)]
    public void Hann_ShouldBeSymmetric(int length)
    {
        var window = new HannWindow<double>();

        for (var i = 0; i < length; i++)
        {
            var mirror = length - 1 - i;

            Assert.Equal(
                window.Evaluate(i, length),
                window.Evaluate(mirror, length),
                12);
        }
    }

    [Theory]
    [InlineData(16)]
    [InlineData(64)]
    public void Hamming_ShouldBeSymmetric(int length)
    {
        var window = new HammingWindow<double>();

        for (var i = 0; i < length; i++)
        {
            var mirror = length - 1 - i;

            Assert.Equal(
                window.Evaluate(i, length),
                window.Evaluate(mirror, length),
                12);
        }
    }

    [Theory]
    [InlineData(16)]
    [InlineData(64)]
    public void Blackman_ShouldBeSymmetric(int length)
    {
        var window = new BlackmanWindow<double>();

        for (var i = 0; i < length; i++)
        {
            var mirror = length - 1 - i;

            Assert.Equal(
                window.Evaluate(i, length),
                window.Evaluate(mirror, length),
                12);
        }
    }

    [Fact]
    public void CoherentGain_ShouldBeOne_ForRectangularWindow()
    {
        var window = new RectangularWindow<double>();

        var gain =
            Windowing.CoherentGain(window, 1024);

        Assert.Equal(1.0, gain, 12);
    }

    [Fact]
    public void CoherentGain_ShouldBeApproximatelyHalf_ForHannWindow()
    {
        var window = new HannWindow<double>();

        var gain =
            Windowing.CoherentGain(window, 1024);

        var expectedGain = 0.5 * (1024 - 1) / 1024;

        Assert.Equal(expectedGain, gain, 12);
    }

    [Fact]
    public void Apply_ShouldMultiplySamplesByWindow()
    {
        var samples =
            new[] { 1.0, 1.0, 1.0, 1.0 };

        var window = new HannWindow<double>();

        var result =
            Windowing.Apply(samples, window);

        Assert.Equal(0.0, result[0], 12);
        Assert.Equal(0.75, result[1], 12);
        Assert.Equal(0.75, result[2], 12);
        Assert.Equal(0.0, result[3], 12);
    }

    [Fact]
    public void Window_ShouldRejectInvalidIndex()
    {
        var window = new HannWindow<double>();

        Assert.Throws<ArgumentOutOfRangeException>(
            () => window.Evaluate(-1, 16));

        Assert.Throws<ArgumentOutOfRangeException>(
            () => window.Evaluate(16, 16));
    }
}
