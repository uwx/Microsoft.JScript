// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.DebugConvert
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using Microsoft.JScript.Vsa;
using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Converts data from one type to another. Includes methods that convert values to <see cref="T:System.String" /> objects so that they can be displayed by the debugger. </summary>
[Guid("432D76CE-8C9E-4eed-ADDD-91737F27A8CB")]
[ComVisible(true)]
public class DebugConvert : IDebugConvert, IDebugConvert2
{
  /// <summary>Converts the specified value to the specified <see cref="T:System.TypeCode" />, optionally allowing data loss.</summary>
  /// <returns>
  /// <paramref name="value" /> converted to the <paramref name="typeCode" /> type.</returns>
  /// <param name="value">The value to convert to a new type.</param>
  /// <param name="typeCode">The type to convert <paramref name="value" /> to.</param>
  /// <param name="truncationPermitted">true to allow data loss; otherwise, false.</param>
  public object ToPrimitive(object value, TypeCode typeCode, bool truncationPermitted)
  {
    return Convert.Coerce2(value, typeCode, truncationPermitted);
  }

  /// <summary>Converts the specified <see cref="T:System.Byte" /> value to a <see cref="T:System.String" />.</summary>
  /// <returns>The string representation of <paramref name="value" /> in base <paramref name="radix" />.</returns>
  /// <param name="value">The value to convert.</param>
  /// <param name="radix">The base of the return value. The base must be 2, 8, 10, or 16.</param>
  public string ByteToString(byte value, int radix) => System.Convert.ToString(value, radix);

  /// <summary>Converts the specified <see cref="T:System.SByte" /> value to a <see cref="T:System.String" />.</summary>
  /// <returns>The string representation of <paramref name="value" /> in base <paramref name="radix" />.</returns>
  /// <param name="value">The value to convert.</param>
  /// <param name="radix">The base of the return value. The base must be 2, 8, 10, or 16.</param>
  public string SByteToString(sbyte value, int radix)
  {
    return radix == 10 ? value.ToString((IFormatProvider) CultureInfo.InvariantCulture) : System.Convert.ToString((byte) value, radix);
  }

  /// <summary>Converts the specified <see cref="T:System.Int16" /> value to a <see cref="T:System.String" />.</summary>
  /// <returns>The string representation of <paramref name="value" /> in base <paramref name="radix" />.</returns>
  /// <param name="value">The value to convert.</param>
  /// <param name="radix">The base of the return value. The base must be 2, 8, 10, or 16.</param>
  public string Int16ToString(short value, int radix)
  {
    return System.Convert.ToString((short) Convert.ToInteger((double) value), radix);
  }

  /// <summary>Converts the specified <see cref="T:System.UInt16" /> value to a <see cref="T:System.String" />.</summary>
  /// <returns>The string representation of <paramref name="value" /> in base <paramref name="radix" />.</returns>
  /// <param name="value">The value to convert.</param>
  /// <param name="radix">The base of the return value. The base must be 2, 8, 10, or 16.</param>
  public string UInt16ToString(ushort value, int radix)
  {
    return radix == 10 ? value.ToString((IFormatProvider) CultureInfo.InvariantCulture) : System.Convert.ToString((short) Convert.ToInteger((double) value), radix);
  }

  /// <summary>Converts the specified <see cref="T:System.Int32" /> value to a <see cref="T:System.String" />.</summary>
  /// <returns>The string representation of <paramref name="value" /> in base <paramref name="radix" />.</returns>
  /// <param name="value">The value to convert.</param>
  /// <param name="radix">The base of the return value. The base must be 2, 8, 10, or 16.</param>
  public string Int32ToString(int value, int radix)
  {
    return System.Convert.ToString((int) Convert.ToInteger((double) value), radix);
  }

  /// <summary>Converts the specified <see cref="T:System.UInt32" /> value to a <see cref="T:System.String" />.</summary>
  /// <returns>The string representation of <paramref name="value" /> in base <paramref name="radix" />.</returns>
  /// <param name="value">The value to convert.</param>
  /// <param name="radix">The base of the return value. The base must be 2, 8, 10, or 16.</param>
  public string UInt32ToString(uint value, int radix)
  {
    return radix == 10 ? value.ToString((IFormatProvider) CultureInfo.InvariantCulture) : System.Convert.ToString((int) Convert.ToInteger((double) value), radix);
  }

  /// <summary>Converts the specified <see cref="T:System.Int64" /> value to a <see cref="T:System.String" />.</summary>
  /// <returns>The string representation of <paramref name="value" /> in base <paramref name="radix" />.</returns>
  /// <param name="value">The value to convert.</param>
  /// <param name="radix">The base of the return value. The base must be 2, 8, 10, or 16.</param>
  public string Int64ToString(long value, int radix)
  {
    return System.Convert.ToString((long) Convert.ToInteger((double) value), radix);
  }

  /// <summary>Converts the specified <see cref="T:System.UInt64" /> value to a <see cref="T:System.String" />.</summary>
  /// <returns>The string representation of <paramref name="value" /> in base <paramref name="radix" />.</returns>
  /// <param name="value">The value to convert.</param>
  /// <param name="radix">The base of the return value. The base must be 2, 8, 10, or 16.</param>
  public string UInt64ToString(ulong value, int radix)
  {
    return radix == 10 ? value.ToString((IFormatProvider) CultureInfo.InvariantCulture) : System.Convert.ToString((long) Convert.ToInteger((double) value), radix);
  }

  /// <summary>Converts the specified <see cref="T:System.Single" /> value to a <see cref="T:System.String" />.</summary>
  /// <returns>The string representation of <paramref name="value" />.</returns>
  /// <param name="value">The value to convert.</param>
  public string SingleToString(float value) => Convert.ToString((double) value);

  /// <summary>Converts the specified <see cref="T:System.Double" /> value to a <see cref="T:System.String" />.</summary>
  /// <returns>The string representation of <paramref name="value" />.</returns>
  /// <param name="value">The value to convert.</param>
  public string DoubleToString(double value) => Convert.ToString(value);

  /// <summary>Converts the specified <see cref="T:System.Boolean" /> value to a <see cref="T:System.String" />.</summary>
  /// <returns>The literal string "true" or the literal string "false".</returns>
  /// <param name="value">The value to convert.</param>
  public string BooleanToString(bool value) => Convert.ToString(value);

  /// <summary>Creates a string representation of the specified <see cref="T:System.Double" />. The string representation contains the day of the week, the month, the date, the time, the time zone, and the year.</summary>
  /// <returns>The string representation of <paramref name="value" />.</returns>
  /// <param name="value">The value to convert.</param>
  public string DoubleToDateString(double value) => DatePrototype.DateToString(value);

  /// <summary>Converts the specified regular expression information to a <see cref="T:System.String" />.</summary>
  /// <returns>The string representation of the regular expression information.</returns>
  /// <param name="source">The regular expression pattern to match.</param>
  /// <param name="ignoreCase">true to use case-insensitive matching; otherwise, false.</param>
  /// <param name="global">true to use a global match; otherwise, false.</param>
  /// <param name="multiline">true to use a multiline match; otherwise, false.</param>
  public string RegexpToString(string source, bool ignoreCase, bool global, bool multiline)
  {
    return RegExpConstructor.ob.Construct(source, ignoreCase, global, multiline).ToString();
  }

  /// <summary>Converts the specified <see cref="T:System.Decimal" /> value to a <see cref="T:System.String" />.</summary>
  /// <returns>The string representation of <paramref name="value" />.</returns>
  /// <param name="value">The value to convert.</param>
  public string DecimalToString(Decimal value)
  {
    return value.ToString((IFormatProvider) CultureInfo.InvariantCulture);
  }

  /// <summary>Converts the specified <see cref="T:System.String" /> to a <see cref="T:System.String" /> that contains escape characters for non-printable characters.</summary>
  /// <returns>The string representation of <paramref name="source" /> that contains escape characters.</returns>
  /// <param name="source">The value to convert.</param>
  public string StringToPrintable(string source)
  {
    int length = source.Length;
    StringBuilder stringBuilder = new StringBuilder(length);
    for (int index1 = 0; index1 < length; ++index1)
    {
      switch (source[index1])
      {
        case char.MinValue:
          stringBuilder.Append("\\0");
          break;
        case '\b':
          stringBuilder.Append("\\b");
          break;
        case '\t':
          stringBuilder.Append("\\t");
          break;
        case '\n':
          stringBuilder.Append("\\n");
          break;
        case '\v':
          stringBuilder.Append("\\v");
          break;
        case '\f':
          stringBuilder.Append("\\f");
          break;
        case '\r':
          stringBuilder.Append("\\r");
          break;
        case '"':
          stringBuilder.Append("\"");
          break;
        case '\\':
          stringBuilder.Append("\\\\");
          break;
        default:
          if (char.GetUnicodeCategory(source[index1]) != UnicodeCategory.Control)
          {
            stringBuilder.Append(source[index1]);
            break;
          }
          stringBuilder.Append("\\u");
          int num1 = (int) source[index1];
          char[] chArray = new char[4];
          for (int index2 = 0; index2 < 4; ++index2)
          {
            int num2 = num1 % 16 /*0x10*/;
            chArray[3 - index2] = num2 > 9 ? (char) (65 + num2 - 10) : (char) (48 /*0x30*/ + num2);
            num1 /= 16 /*0x10*/;
          }
          stringBuilder.Append(chArray);
          break;
      }
    }
    return stringBuilder.ToString();
  }

  /// <summary>Creates a managed object that has the specified primitive value.</summary>
  /// <returns>
  /// <paramref name="value" /> converted to a managed <see cref="T:System.Object" />.</returns>
  /// <param name="value">The value to convert.</param>
  [return: MarshalAs(UnmanagedType.Interface)]
  public object GetManagedObject(object value) => value;

  /// <summary>Creates a managed object that has the specified primitive <see cref="T:System.Int64" /> value.</summary>
  /// <returns>
  /// <paramref name="i" /> converted to an <see cref="T:System.Object" />.</returns>
  /// <param name="i">The value to convert.</param>
  [return: MarshalAs(UnmanagedType.Interface)]
  public object GetManagedInt64Object(long i) => (object) i;

  /// <summary>Creates a managed object that has the specified primitive <see cref="T:System.UInt64" /> value.</summary>
  /// <returns>
  /// <paramref name="i" /> converted to an <see cref="T:System.Object" />.</returns>
  /// <param name="i">The value to convert.</param>
  [return: MarshalAs(UnmanagedType.Interface)]
  public object GetManagedUInt64Object(ulong i) => (object) i;

  /// <summary>Creates a managed object that has the specified primitive <see cref="T:System.Char" /> value.</summary>
  /// <returns>
  /// <paramref name="i" /> converted to a <see cref="T:System.Char" /> and then an <see cref="T:System.Object" />.</returns>
  /// <param name="i">The value to convert.</param>
  [return: MarshalAs(UnmanagedType.Interface)]
  public object GetManagedCharObject(ushort i) => (object) (char) i;

  /// <summary>Gets an error message that is created from the specified <see cref="P:System.Exception.HResult" /> and the <see cref="T:System.Globalization.CultureInfo" /> of the specified <see cref="T:Microsoft.JScript.Vsa.IJSVsaEngine" />.</summary>
  /// <returns>A localized string that contains information about the error.</returns>
  /// <param name="hr">The HRESULT error code.</param>
  /// <param name="engine">A reference to the scripting engine.</param>
  public string GetErrorMessageForHR(int hr, IJSVsaEngine engine)
  {
    CultureInfo culture = (CultureInfo) null;
    if (engine is VsaEngine vsaEngine)
      culture = vsaEngine.ErrorCultureInfo;
    return ((long) hr & 4294901760L) == 2148139008L /*0x800A0000*/ && Enum.IsDefined(typeof (JSError), (object) (hr & (int) ushort.MaxValue)) ? JScriptException.Localize((hr & (int) ushort.MaxValue).ToString((IFormatProvider) CultureInfo.InvariantCulture), culture) : JScriptException.Localize(6011.ToString((IFormatProvider) CultureInfo.InvariantCulture), "0x" + hr.ToString("X", (IFormatProvider) CultureInfo.InvariantCulture), culture);
  }
}
