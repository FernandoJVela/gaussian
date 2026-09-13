namespace Gaussian.Signals;

public readonly struct Complex<T>
    where T : System.Numerics.IFloatingPointIeee754<T>
{
    public T Real { get; }

    public T Imaginary { get; }

    public Complex(T real, T imaginary)
    {
        Real = real;
        Imaginary = imaginary;
    }

    public bool Equals(Complex<T> other)
    {
        return Real == other.Real &&
            Imaginary == other.Imaginary;
    }

    public override bool Equals(object? obj)
    {
        return obj is Complex<T> other &&
            Equals(other);
    }

    public static Complex<T> operator +(Complex<T> left, Complex<T> right)
    {
        return new Complex<T>(
            left.Real + right.Real,
            left.Imaginary + right.Imaginary);
    }

    public static Complex<T> operator -(Complex<T> left, Complex<T> right)
    {
        return new Complex<T>(
            left.Real - right.Real,
            left.Imaginary - right.Imaginary);
    }

    public static Complex<T> operator *(Complex<T> left, Complex<T> right)
    {
        var real =
            left.Real * right.Real -
            left.Imaginary * right.Imaginary;

        var imaginary =
            left.Real * right.Imaginary +
            left.Imaginary * right.Real;

        return new Complex<T>(real, imaginary);
    }

    public Complex<T> Conjugate()
    {
        return new Complex<T>(
            Real,
            -Imaginary);
    }

    public T Magnitude
    {
        get
        {
            return T.Sqrt(
                Real * Real +
                Imaginary * Imaginary);
        }
    }

    public T Phase
    {
        get
        {
            return T.Atan2(Imaginary, Real);
        }
    }

    public static Complex<T> FromPolar(T magnitude, T phase)
    {
        return new Complex<T>(
            magnitude * T.Cos(phase),
            magnitude * T.Sin(phase));
    }

    public static Complex<T> Exp(Complex<T> value)
    {
        var magnitude = T.Exp(value.Real);

        return new Complex<T>(
            magnitude * T.Cos(value.Imaginary),
            magnitude * T.Sin(value.Imaginary));
    }

    public static Complex<T> operator /(Complex<T> left, Complex<T> right)
    {
        var denominator =
            right.Real * right.Real +
            right.Imaginary * right.Imaginary;

        var real =
            (left.Real * right.Real +
            left.Imaginary * right.Imaginary) /
            denominator;

        var imaginary =
            (left.Imaginary * right.Real -
            left.Real * right.Imaginary) /
            denominator;

        return new Complex<T>(real, imaginary);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Real, Imaginary);
    }

    public static bool operator ==(
        Complex<T> left,
        Complex<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(
        Complex<T> left,
        Complex<T> right)
    {
        return !left.Equals(right);
    }
}