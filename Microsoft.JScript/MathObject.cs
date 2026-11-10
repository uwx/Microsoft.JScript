// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.MathObject
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Provides constants and static methods for trigonometric functions, logarithmic functions, and other common mathematical functions. This class belongs to the built-in object model category.</summary>
public class MathObject : JSObject
{
    /// <summary>Stores a value that represents Euler's constant, which is the base of the natural logarithm, specified by the constant e.</summary>
    public const double E = 2.7182818284590451;

    /// <summary>Stores a value that represents the logarithm base e of 10, the natural logarithm of 10.</summary>
    public const double LN10 = 2.3025850929940459;

    /// <summary>Stores a value that represents the logarithm base e of 2, the natural logarithm of 2.</summary>
    public const double LN2 = 0.69314718055994529;

    /// <summary>Stores a value that represents the logarithm base 2 of e, the binary logarithm of e.</summary>
    public const double LOG2E = 1.4426950408889634;

    /// <summary>Stores a value that represents the logarithm base 10 of e, the common logarithm of e.</summary>
    public const double LOG10E = 0.43429448190325182;

    /// <summary>Stores a value that represents the ratio of the circumference of a circle to its diameter, specified by the constant π.</summary>
    public const double PI = 3.1415926535897931;

    /// <summary>Stores a value that represents the following equivalent values: (√2)/2, √(½), and 1/(√2).</summary>
    public const double SQRT1_2 = 0.70710678118654757;

    /// <summary>Stores a value that represents √2.</summary>
    public const double SQRT2 = 1.4142135623730951;

    private static readonly Random internalRandom = new Random();
    internal static MathObject ob = (MathObject)null;

    internal MathObject(ScriptObject parent)
        : base(parent)
    {
    }

    /// <summary>Calculates the absolute value of the specified number.</summary>
    /// <returns>The absolute value of <paramref name="d" />.</returns>
    /// <param name="d">The number to calculate the absolute value of.</param>
    [JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Math_abs)]
    public static double abs(double d)
    {
        if (d < 0.0)
            return -d;
        return d > 0.0 || d != d ? d : 0.0;
    }

    /// <summary>Calculates the inverse cosine of the specified number.</summary>
    /// <returns>The inverse cosine of <paramref name="x" />, measured in radians.</returns>
    /// <param name="x">The number to calculate the inverse cosine of.</param>
    [JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Math_acos)]
    public static double acos(double x) => Math.Acos(x);

    /// <summary>Calculates the inverse sine of the specified number.</summary>
    /// <returns>The inverse sine of <paramref name="x" />, measured in radians.</returns>
    /// <param name="x">The number to calculate the inverse sine of.</param>
    [JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Math_asin)]
    public static double asin(double x) => Math.Asin(x);

    /// <summary>Calculates the inverse tangent of the specified number.</summary>
    /// <returns>The inverse tangent of <paramref name="x" />, measured in radians.</returns>
    /// <param name="x">The number to calculate the inverse tangent of.</param>
    [JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Math_atan)]
    public static double atan(double x) => Math.Atan(x);

    /// <summary>Calculates the inverse tangent of the quotient of the specified numbers.</summary>
    /// <returns>The inverse tangent of <paramref name="dy" />/<paramref name="dx" />, measured in radians.</returns>
    /// <param name="dy">The numerator of the quotient to calculate the inverse tangent of.</param>
    /// <param name="dx">The denominator of the quotient to calculate the inverse tangent of.</param>
    [JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Math_atan2)]
    public static double atan2(double dy, double dx) => Math.Atan2(dy, dx);

    /// <summary>Calculates the ceiling of the specified number, which is the smallest integer that is greater than or equal to the specified number.</summary>
    /// <returns>The ceiling of <paramref name="x" />.</returns>
    /// <param name="x">The number to calculate the ceiling of.</param>
    [JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Math_ceil)]
    public static double ceil(double x) => Math.Ceiling(x);

    private static double Compare(double x, double y)
    {
        if (x != 0.0 || y != 0.0)
            return x == y ? 0.0 : x - y;
        double num1 = 1.0 / x;
        double num2 = 1.0 / y;
        if (num1 < 0.0)
            return num2 < 0.0 ? 0.0 : -1.0;
        return num2 < 0.0 ? 1.0 : 0.0;
    }

    /// <summary>Calculates the cosine of the specified angle.</summary>
    /// <returns>The cosine of <paramref name="x" />.</returns>
    /// <param name="x">An angle, measured in radians.</param>
    [JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Math_cos)]
    public static double cos(double x) => Math.Cos(x);

    /// <summary>Calculates the number e to the raised specified power, where e is Euler's constant, the base of the natural logarithm.</summary>
    /// <returns>The number e raised to the <paramref name="x" /> power.</returns>
    /// <param name="x">The power to raise <paramref name="e" /> to.</param>
    [JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Math_exp)]
    public static double exp(double x) => Math.Exp(x);

    /// <summary>Calculates the floor of the specified number, which is the largest integer that is less than or equal to the specified number.</summary>
    /// <returns>The floor of <paramref name="x" />.</returns>
    /// <param name="x">The number to calculate the floor of.</param>
    [JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Math_floor)]
    public static double floor(double x) => Math.Floor(x);

    internal override string GetClassName() => "Math";

    /// <summary>Calculates the logarithm base e of the specified number, which is the natural logarithm of the specified number.</summary>
    /// <returns>The natural logarithm of <paramref name="x" />.</returns>
    /// <param name="x">The number to calculate the natural logarithm of.</param>
    [JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Math_log)]
    public static double log(double x) => Math.Log(x);

    /// <summary>Returns the largest of two or more specified numbers.</summary>
    /// <returns>The largest of <paramref name="x" />, <paramref name="y" />, and the numbers that are contained in <paramref name="args" />.</returns>
    /// <param name="x">The first number to compare.</param>
    /// <param name="y">The second number to compare.</param>
    /// <param name="args">Additional numbers to compare.</param>
    [JSFunction(JSFunctionAttributeEnum.HasVarArgs, JSBuiltin.Math_max)]
    public static double max(object x, object y, params object[] args)
    {
        if (x is Missing)
            return double.NegativeInfinity;
        double number1 = Convert.ToNumber(x);
        if (y is Missing)
            return number1;
        double number2 = Convert.ToNumber(y);
        double num = MathObject.Compare(number1, number2);
        if (num != num)
            return num;
        double lhMax = number1;
        if (num < 0.0)
            lhMax = number2;
        return args.Length == 0 ? lhMax : MathObject.maxv(lhMax, args, 0);
    }

    private static double maxv(double lhMax, object[] args, int start)
    {
        if (args.Length == start)
            return lhMax;
        double num1 = Convert.ToNumber(args[start]);
        double num2 = MathObject.Compare(lhMax, num1);
        if (num2 != num2)
            return num2;
        if (num2 > 0.0)
            num1 = lhMax;
        return MathObject.maxv(num1, args, start + 1);
    }

    /// <summary>Returns the smallest of two or more specified numbers.</summary>
    /// <returns>The smallest of <paramref name="x" />, <paramref name="y" />, and the numbers that are contained in <paramref name="args" />.</returns>
    /// <param name="x">The first number to compare.</param>
    /// <param name="y">The second number to compare.</param>
    /// <param name="args">Additional numbers to compare.</param>
    [JSFunction(JSFunctionAttributeEnum.HasVarArgs, JSBuiltin.Math_min)]
    public static double min(object x, object y, params object[] args)
    {
        if (x is Missing)
            return double.PositiveInfinity;
        double number1 = Convert.ToNumber(x);
        if (y is Missing)
            return number1;
        double number2 = Convert.ToNumber(y);
        double num = MathObject.Compare(number1, number2);
        if (num != num)
            return num;
        double lhMin = number1;
        if (num > 0.0)
            lhMin = number2;
        return args.Length == 0 ? lhMin : MathObject.minv(lhMin, args, 0);
    }

    private static double minv(double lhMin, object[] args, int start)
    {
        if (args.Length == start)
            return lhMin;
        double num1 = Convert.ToNumber(args[start]);
        double num2 = MathObject.Compare(lhMin, num1);
        if (num2 != num2)
            return num2;
        if (num2 < 0.0)
            num1 = lhMin;
        return MathObject.minv(num1, args, start + 1);
    }

    /// <summary>Calculates the specified number raised to the specified power.</summary>
    /// <returns>The number <paramref name="dx" /> raised to the <paramref name="dy" /> power.</returns>
    /// <param name="dx">The number to raise to the <paramref name="dy" /> power.</param>
    /// <param name="dy">The power to raise <paramref name="dx" /> to.</param>
    [JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Math_pow)]
    public static double pow(double dx, double dy)
    {
        if (dy == 0.0)
            return 1.0;
        if ((dx == 1.0 || dx == -1.0) && (dy == double.PositiveInfinity || dy == double.NegativeInfinity) ||
            double.IsNaN(dy))
            return double.NaN;
        if (dx == double.NegativeInfinity && dy < 0.0 && Math.IEEERemainder(-dy + 1.0, 2.0) == 0.0)
            return -0.0;
        try
        {
            return Math.Pow(dx, dy);
        }
        catch
        {
            if (dx != dx || dy != dy || dx != 0.0 || dy >= 0.0)
                return double.NaN;
            return (double)(long)dy == dy && (long)-dy % 2L > 0L && 1.0 / dx < 0.0
                ? double.NegativeInfinity
                : double.PositiveInfinity;
        }
    }

    /// <summary>Generates a random number between 0 and 1. This method does not generate a cryptographically secure random number.</summary>
    /// <returns>A number that is greater than or equal to 0, and less than 1.</returns>
    [JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Math_random)]
    public static double random() => MathObject.internalRandom.NextDouble();

    /// <summary>Rounds the specified number down or up to the nearest integer.</summary>
    /// <returns>If the fractional part of <paramref name="d" /> is less than halfway to the next integer, <paramref name="d" /> rounded down. If the fractional part of <paramref name="d" /> is halfway or more to the next integer, <paramref name="d" /> rounded up.</returns>
    /// <param name="d">The number to round.</param>
    [JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Math_round)]
    public static double round(double d) => d == 0.0 ? d : Math.Floor(d + 0.5);

    /// <summary>Calculates the sine of the specified angle.</summary>
    /// <returns>The sine of <paramref name="x" />.</returns>
    /// <param name="x">An angle, measured in radians.</param>
    [JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Math_sin)]
    public static double sin(double x) => Math.Sin(x);

    /// <summary>Calculates the square root of the specified number.</summary>
    /// <returns>The square root of <paramref name="x" />.</returns>
    /// <param name="x">The number to calculate the square root of.</param>
    [JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Math_sqrt)]
    public static double sqrt(double x) => Math.Sqrt(x);

    /// <summary>Calculates the tangent of the specified angle.</summary>
    /// <returns>The tangent of <paramref name="x" />.</returns>
    /// <param name="x">An angle, measured in radians.</param>
    [JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Math_tan)]
    public static double tan(double x) => Math.Tan(x);
}