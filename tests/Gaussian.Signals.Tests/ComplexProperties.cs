using FsCheck;
using FsCheck.Xunit;
using Gaussian.Signals;

namespace Gaussian.Signals.Tests;

public class ComplexProperties
{
    [Property(MaxTest = 100)]
    public void ConjugateOfConjugateIsOriginal(int real, int imaginary)
    {
        var value = new Complex<double>(
            real,
            imaginary);

        var result = value
            .Conjugate()
            .Conjugate();

        NumericAssert.Equal(
            value.Real,
            result.Real);

        NumericAssert.Equal(
            value.Imaginary,
            result.Imaginary);
    }

    [Property(MaxTest = 100)]
    public void MagnitudeOfProductEqualsProductOfMagnitudes(
        int realA,
        int imaginaryA,
        int realB,
        int imaginaryB)
    {
        var a = new Complex<double>(
            realA,
            imaginaryA);

        var b = new Complex<double>(
            realB,
            imaginaryB);

        var actual = (a * b).Magnitude;
        var expected = a.Magnitude * b.Magnitude;

        NumericAssert.Equal(
            expected,
            actual);
    }

    [Property(MaxTest = 100)]
    public void ValueTimesConjugateEqualsMagnitudeSquared(
        int real,
        int imaginary)
    {
        var value = new Complex<double>(
            real,
            imaginary);

        var result = value * value.Conjugate();

        var expected = value.Magnitude * value.Magnitude;

        NumericAssert.Equal(
            expected,
            result.Real);

        NumericAssert.Equal(
            0.0,
            result.Imaginary);
    }

    [Property(MaxTest = 100)]
    public void PolarConversionPreservesMagnitude(
        int magnitude,
        int phaseDegrees)
    {
        var positiveMagnitude = Math.Abs(magnitude);

        var phase =
            phaseDegrees *
            Math.PI /
            180.0;

        var value =
            Complex<double>.FromPolar(
                positiveMagnitude,
                phase);

        NumericAssert.Equal(
            positiveMagnitude,
            value.Magnitude);
    }

    [Property(MaxTest = 100)]
    public void EulerRotationHasUnitMagnitude(int angleDegrees)
    {
        var angle =
            angleDegrees *
            Math.PI /
            180.0;

        var value =
            new Complex<double>(
                0.0,
                angle);

        var result =
            Complex<double>.Exp(value);

        NumericAssert.Equal(
            1.0,
            result.Magnitude);
    }

    [Property(MaxTest = 50)]
    public void SumOfRootsShouldBeZero(
        PositiveInt input)
    {
        var n = input.Item;

        if (n < 2 || n > 64)
        {
            return;
        }

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
            sum.Real,
            1e-10);

        NumericAssert.Equal(
            0.0,
            sum.Imaginary,
            1e-10);
    }
}