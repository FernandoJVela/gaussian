using Gaussian.Signals;

namespace Gaussian.Signals.Tests;

public class ComplexTests
{
    [Fact]
    public void Constructor_ShouldStoreRealAndImaginaryParts()
    {
        var value = new Complex<double>(3.0, 4.0);

        Assert.Equal(3.0, value.Real);
        Assert.Equal(4.0, value.Imaginary);
    }

    [Fact]
    public void Addition_ShouldAddRealAndImaginaryParts()
    {
        var left = new Complex<double>(3.0, 4.0);
        var right = new Complex<double>(2.0, -1.0);

        var result = left + right;

        Assert.Equal(5.0, result.Real);
        Assert.Equal(3.0, result.Imaginary);
    }

    [Fact]
    public void Subtraction_ShouldSubtractRealAndImaginaryParts()
    {
        var left = new Complex<double>(3.0, 4.0);
        var right = new Complex<double>(2.0, 1.0);

        var result = left - right;

        Assert.Equal(1.0, result.Real);
        Assert.Equal(3.0, result.Imaginary);
    }

    [Fact]
    public void Multiplication_ShouldFollowComplexArithmetic()
    {
        var left = new Complex<double>(3.0, 4.0);
        var right = new Complex<double>(2.0, -1.0);

        var result = left * right;

        Assert.Equal(10.0, result.Real);
        Assert.Equal(5.0, result.Imaginary);
    }

    [Fact]
    public void Conjugate_ShouldInvertImaginaryPart()
    {
        var value = new Complex<double>(3.0, 4.0);

        var result = value.Conjugate();

        Assert.Equal(3.0, result.Real);
        Assert.Equal(-4.0, result.Imaginary);
    }

    [Fact]
    public void Magnitude_ShouldCalculateEuclideanLength()
    {
        var value = new Complex<double>(3.0, 4.0);

        Assert.Equal(5.0, value.Magnitude);
    }

    [Fact]
    public void Phase_ShouldReturnAngleInRadians()
    {
        var value = new Complex<double>(0.0, 1.0);

        var phase = value.Phase;

        Assert.Equal(Math.PI / 2.0, phase, 12);
    }

    [Fact]
    public void FromPolar_ShouldCreateCorrectCartesianValue()
    {
        var value = Complex<double>.FromPolar(
            5.0,
            Math.Atan2(4.0, 3.0));

        NumericAssert.Equal(3.0, value.Real);
        NumericAssert.Equal(4.0, value.Imaginary);
    }

    [Fact]
    public void Exp_Of_IPi_ShouldEqualNegativeOne()
    {
        var value = new Complex<double>(
            0.0,
            Math.PI);

        var result = Complex<double>.Exp(value);

        NumericAssert.Equal(-1.0, result.Real);
        NumericAssert.Equal(0.0, result.Imaginary);
    }

    [Fact]
    public void EulerIdentity_ShouldHold()
    {
        var iPi = new Complex<double>(0.0, Math.PI);

        var result =
            Complex<double>.Exp(iPi)
            + new Complex<double>(1.0, 0.0);

        NumericAssert.Equal(0.0, result.Real);
        NumericAssert.Equal(0.0, result.Imaginary);
    }

    [Fact]
    public void Division_ShouldProduceCorrectResult()
    {
        var left = new Complex<double>(3.0, 4.0);
        var right = new Complex<double>(1.0, -2.0);

        var result = left / right;

        NumericAssert.Equal(-1.0, result.Real);
        NumericAssert.Equal(2.0, result.Imaginary);
    }

    [Theory]
    [InlineData(2)]
    [InlineData(4)]
    [InlineData(8)]
    [InlineData(16)]
    public void RootsOfUnity_ShouldReturnNRoots(int n)
    {
        var roots =
            RootsOfUnity.Create<double>(n);

        Assert.Equal(n, roots.Count);
    }

    [Theory]
    [InlineData(2)]
    [InlineData(4)]
    [InlineData(8)]
    [InlineData(16)]
    public void RootsOfUnity_ShouldHaveUnitMagnitude(int n)
    {
        var roots =
            RootsOfUnity.Create<double>(n);

        foreach (var root in roots)
        {
            NumericAssert.Equal(
                1.0,
                root.Magnitude);
        }
    }

    private static Complex<double> Power(
        Complex<double> value,
        int exponent)
    {
        var result =
            new Complex<double>(1.0, 0.0);

        for (var i = 0; i < exponent; i++)
        {
            result *= value;
        }

        return result;
    }

    [Theory]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(8)]
    public void EveryRootRaisedToN_ShouldEqualOne(int n)
    {
        var roots =
            RootsOfUnity.Create<double>(n);

        foreach (var root in roots)
        {
            var result =
                Power(root, n);

            NumericAssert.Equal(
                1.0,
                result.Real);

            NumericAssert.Equal(
                0.0,
                result.Imaginary);
        }
    }

    [Theory]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(8)]
    [InlineData(16)]
    public void SumOfNthRootsOfUnity_ShouldBeZero(int n)
    {
        var roots =
            RootsOfUnity.Create<double>(n);

        var sum =
            new Complex<double>(0.0, 0.0);

        foreach (var root in roots)
        {
            sum += root;
        }

        NumericAssert.Equal(
            0.0,
            sum.Real);

        NumericAssert.Equal(
            0.0,
            sum.Imaginary);
    }
}