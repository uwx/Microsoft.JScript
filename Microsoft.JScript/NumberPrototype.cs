// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.NumberPrototype
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Globalization;
using System.Text;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Provides static methods and properties for <see cref="T:Microsoft.JScript.NumberObject" /> objects. All methods of this class are called by passing in a reference to the object that is requesting access to the method. This class belongs to the built-in object model category.</summary>
public class NumberPrototype : NumberObject
{
    internal static readonly NumberPrototype ob = new NumberPrototype(ObjectPrototype.ob);
    internal static NumberConstructor _constructor;

    internal NumberPrototype(ObjectPrototype parent)
        : base((ScriptObject)parent, (object)0.0)
    {
        this.noExpando = true;
    }

    /// <summary>Gets a reference to a prototype-based object that is using this prototype.</summary>
    /// <returns>A reference to a prototype-based object.</returns>
    public static NumberConstructor constructor => NumberPrototype._constructor;

    private static double ThisobToDouble(object thisob)
    {
        thisob = NumberPrototype.valueOf(thisob);
        return ((IConvertible)thisob).ToDouble((IFormatProvider)null);
    }

    /// <summary>Creates a string representation of the specified <see cref="T:Microsoft.JScript.NumberObject" /> that uses scientific (E) notation.</summary>
    /// <returns>A string representation of <paramref name="thisob" />.</returns>
    /// <param name="thisob">The object that this method is acting upon.</param>
    /// <param name="fractionDigits">A number between 0 and 20 that represents the number of decimal places to display. The default is 16.</param>
    [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Number_toExponential)]
    public static string toExponential(object thisob, object fractionDigits)
    {
        double num1 = NumberPrototype.ThisobToDouble(thisob);
        double num2;
        switch (fractionDigits)
        {
            case null:
            case Missing _:
                num2 = 16.0;
                break;
            default:
                num2 = Convert.ToInteger(fractionDigits);
                break;
        }

        if (num2 < 0.0 || num2 > 20.0)
            throw new JScriptException(JSError.FractionOutOfRange);
        StringBuilder stringBuilder = new StringBuilder("#.");
        for (int index = 0; (double)index < num2; ++index)
            stringBuilder.Append('0');
        stringBuilder.Append("e+0");
        return num1.ToString(stringBuilder.ToString(), (IFormatProvider)CultureInfo.InvariantCulture);
    }

    /// <summary>Creates a string representation of the specified <see cref="T:Microsoft.JScript.NumberObject" /> that displays the specified number of decimal digits.</summary>
    /// <returns>A string representation of <paramref name="thisob" />.</returns>
    /// <param name="thisob">The object that this method is acting upon.</param>
    /// <param name="fractionDigits">A number between 0 and 20 that represents the number of decimal places to display. The default is 0.</param>
    [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Number_toFixed)]
    public static string toFixed(object thisob, double fractionDigits)
    {
        double num = NumberPrototype.ThisobToDouble(thisob);
        if (double.IsNaN(fractionDigits))
            fractionDigits = 0.0;
        else if (fractionDigits < 0.0 || fractionDigits > 20.0)
            throw new JScriptException(JSError.FractionOutOfRange);
        return num.ToString("f" + ((int)fractionDigits).ToString((IFormatProvider)CultureInfo.InvariantCulture),
            (IFormatProvider)CultureInfo.InvariantCulture);
    }

    /// <summary>Creates a string representation of the specified object that is based on <see cref="P:System.Globalization.NumberFormatInfo.CurrentInfo" /> and <see cref="T:System.Globalization.CultureInfo" />.</summary>
    /// <returns>A string representation of <paramref name="thisob" />.</returns>
    /// <param name="thisob">The object that this method is acting upon.</param>
    [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Number_toLocaleString)]
    public static string toLocaleString(object thisob)
    {
        return Convert.ToString(NumberPrototype.valueOf(thisob), PreferredType.LocaleString, true);
    }

    /// <summary>Creates a string representation of the specified <see cref="T:Microsoft.JScript.NumberObject" /> that displays the specified number of precision digits.</summary>
    /// <returns>A string representation of <paramref name="thisob" />.</returns>
    /// <param name="thisob">The object that this method is acting upon.</param>
    /// <param name="precision">A number between 0 and 21 that represents the number of precision digits to display. <paramref name="Precision digits" /> are the digits of the decimal and the exponent combined.</param>
    [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Number_toPrecision)]
    public static string toPrecision(object thisob, object precision)
    {
        double d = NumberPrototype.ThisobToDouble(thisob);
        switch (precision)
        {
            case null:
            case Missing _:
                return Convert.ToString(d);
            default:
                double integer = Convert.ToInteger(precision);
                int num1 = integer >= 1.0 && integer <= 21.0
                    ? (int)integer
                    : throw new JScriptException(JSError.PrecisionOutOfRange);
                if (double.IsNaN(d))
                    return "NaN";
                if (double.IsInfinity(d))
                    return d <= 0.0 ? "-Infinity" : "Infinity";
                string str1;
                if (d >= 0.0)
                {
                    str1 = "";
                }
                else
                {
                    str1 = "-";
                    d = -d;
                }

                string str2 = d.ToString("e" + (num1 - 1).ToString((IFormatProvider)CultureInfo.InvariantCulture),
                    (IFormatProvider)CultureInfo.InvariantCulture);
                int num2 = int.Parse(str2.Substring(str2.Length - 4), (IFormatProvider)CultureInfo.InvariantCulture);
                string str3 = str2.Substring(0, 1) + str2.Substring(2, num1 - 1);
                if (num2 >= num1 || num2 < -6)
                    return str1 + str3.Substring(0, 1) + (num1 > 1 ? "." + str3.Substring(1) : "") +
                           (num2 >= 0 ? "e+" : "e") + num2.ToString((IFormatProvider)CultureInfo.InvariantCulture);
                if (num2 == num1 - 1)
                    return str1 + str3;
                return num2 >= 0
                    ? $"{str1}{str3.Substring(0, num2 + 1)}.{str3.Substring(num2 + 1)}"
                    : $"{str1}0.{str3.PadLeft(num1 - num2 - 1, '0')}";
        }
    }

    /// <summary>Creates a string representation of the specified object.</summary>
    /// <returns>A string representation of <paramref name="thisob" />.</returns>
    /// <param name="thisob">The object that this method is acting upon.</param>
    /// <param name="radix">A number between 2 and 36 that represents the base of the number system that <paramref name="thisob" /> is expressed in. The default is 10.</param>
    [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Number_toString)]
    public static string toString(object thisob, object radix)
    {
        int radix1 = 10;
        if (radix is IConvertible)
        {
            double num1 = ((IConvertible)radix).ToDouble((IFormatProvider)CultureInfo.InvariantCulture);
            int num2 = (int)num1;
            if (num1 == (double)num2)
                radix1 = num2;
        }

        if (radix1 < 2 || radix1 > 36)
            radix1 = 10;
        return Convert.ToString(NumberPrototype.valueOf(thisob), radix1);
    }

    /// <summary>Returns the value of the specified <see cref="T:Microsoft.JScript.NumberObject" />.</summary>
    /// <returns>The value of <paramref name="thisob" />.</returns>
    /// <param name="thisob">The object that this method is acting upon.</param>
    [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Number_valueOf)]
    public static object valueOf(object thisob)
    {
        if (thisob is NumberObject)
            return ((NumberObject)thisob).value;
        switch (Convert.GetTypeCode(thisob))
        {
            case TypeCode.SByte:
            case TypeCode.Byte:
            case TypeCode.Int16:
            case TypeCode.UInt16:
            case TypeCode.Int32:
            case TypeCode.UInt32:
            case TypeCode.Int64:
            case TypeCode.UInt64:
            case TypeCode.Single:
            case TypeCode.Double:
                return thisob;
            default:
                throw new JScriptException(JSError.NumberExpected);
        }
    }
}