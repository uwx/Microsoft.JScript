// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.IDebugConvert
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using Microsoft.JScript.Vsa;
using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Defines a set of utility methods that convert data from one type to another. Includes methods that convert values to <see cref="T:System.String" /> objects so that they can be displayed by the debugger.</summary>
[Guid("AA51516D-C0F2-49fe-9D38-61D20456904C")]
[ComVisible(true)]
public interface IDebugConvert
{
    /// <summary>When implemented in a class, converts the specified value to the specified <see cref="T:System.TypeCode" />, optionally allowing data loss.</summary>
    /// <returns>
    /// <paramref name="value" /> converted to the <paramref name="typeCode" /> type.</returns>
    /// <param name="value">The value to convert to a new type.</param>
    /// <param name="typeCode">The type to convert <paramref name="value" /> to.</param>
    /// <param name="truncationPermitted">true to allow data loss; otherwise, false.</param>
    object ToPrimitive(object value, TypeCode typeCode, bool truncationPermitted);

    /// <summary>When implemented in a class, converts the specified <see cref="T:System.Byte" /> value to a <see cref="T:System.String" />.</summary>
    /// <returns>The string representation of <paramref name="value" /> in base <paramref name="radix" />.</returns>
    /// <param name="value">The value to convert.</param>
    /// <param name="radix">The base of the return value. The base must be 2, 8, 10, or 16.</param>
    string ByteToString(byte value, int radix);

    /// <summary>When implemented in a class, converts the specified <see cref="T:System.SByte" /> value to a <see cref="T:System.String" />.</summary>
    /// <returns>The string representation of <paramref name="value" /> in base <paramref name="radix" />.</returns>
    /// <param name="value">The value to convert.</param>
    /// <param name="radix">The base of the return value. The base must be 2, 8, 10, or 16.</param>
    string SByteToString(sbyte value, int radix);

    /// <summary>When implemented in a class, converts the specified <see cref="T:System.Int16" /> value to a <see cref="T:System.String" />.</summary>
    /// <returns>The string representation of <paramref name="value" /> in base <paramref name="radix" />.</returns>
    /// <param name="value">The value to convert.</param>
    /// <param name="radix">The base of the return value. The base must be 2, 8, 10, or 16.</param>
    string Int16ToString(short value, int radix);

    /// <summary>When implemented in a class, converts the specified <see cref="T:System.UInt16" /> value to a <see cref="T:System.String" />.</summary>
    /// <returns>The string representation of <paramref name="value" /> in base <paramref name="radix" />.</returns>
    /// <param name="value">The value to convert.</param>
    /// <param name="radix">The base of the return value. The base must be 2, 8, 10, or 16.</param>
    string UInt16ToString(ushort value, int radix);

    /// <summary>When implemented in a class, converts the specified <see cref="T:System.Int32" /> value to a <see cref="T:System.String" />.</summary>
    /// <returns>The string representation of <paramref name="value" /> in base <paramref name="radix" />.</returns>
    /// <param name="value">The value to convert.</param>
    /// <param name="radix">The base of the return value. The base must be 2, 8, 10, or 16.</param>
    string Int32ToString(int value, int radix);

    /// <summary>When implemented in a class, converts the specified <see cref="T:System.UInt32" /> value to a <see cref="T:System.String" />.</summary>
    /// <returns>The string representation of <paramref name="value" /> in base <paramref name="radix" />.</returns>
    /// <param name="value">The value to convert.</param>
    /// <param name="radix">The base of the return value. The base must be 2, 8, 10, or 16.</param>
    string UInt32ToString(uint value, int radix);

    /// <summary>When implemented in a class, converts the specified <see cref="T:System.Int64" /> value to a <see cref="T:System.String" />.</summary>
    /// <returns>The string representation of <paramref name="value" /> in base <paramref name="radix" />.</returns>
    /// <param name="value">The value to convert.</param>
    /// <param name="radix">The base of the return value. The base must be 2, 8, 10, or 16.</param>
    string Int64ToString(long value, int radix);

    /// <summary>When implemented in a class, converts the specified <see cref="T:System.UInt64" /> value to a <see cref="T:System.String" />.</summary>
    /// <returns>The string representation of <paramref name="value" /> in base <paramref name="radix" />.</returns>
    /// <param name="value">The value to convert.</param>
    /// <param name="radix">The base of the return value. The base must be 2, 8, 10, or 16.</param>
    string UInt64ToString(ulong value, int radix);

    /// <summary>When implemented in a class, converts the specified <see cref="T:System.Single" /> value to a <see cref="T:System.String" />.</summary>
    /// <returns>The string representation of <paramref name="value" />.</returns>
    /// <param name="value">The value to convert.</param>
    string SingleToString(float value);

    /// <summary>When implemented in a class, converts the specified <see cref="T:System.Double" /> value to a <see cref="T:System.String" />.</summary>
    /// <returns>The string representation of <paramref name="value" />.</returns>
    /// <param name="value">The value to convert.</param>
    string DoubleToString(double value);

    /// <summary>When implemented in a class, converts the specified <see cref="T:System.Boolean" /> value to a <see cref="T:System.String" />.</summary>
    /// <returns>The literal string "true" or the literal string "false".</returns>
    /// <param name="value">The value to convert.</param>
    string BooleanToString(bool value);

    /// <summary>When implemented in a class, creates a string representation of the specified <see cref="T:System.Double" />. The string representation contains the day of the week, the month, the date, the time, the time zone, and the year.</summary>
    /// <returns>The string representation of <paramref name="value" />.</returns>
    /// <param name="value">The value to convert.</param>
    string DoubleToDateString(double value);

    /// <summary>When implemented in a class, converts the specified regular expression information to a <see cref="T:System.String" />.</summary>
    /// <returns>The string representation of the regular expression information.</returns>
    /// <param name="source">The regular expression pattern to match.</param>
    /// <param name="ignoreCase">true to use case-insensitive matching; otherwise, false.</param>
    /// <param name="global">true to use a global match; otherwise, false.</param>
    /// <param name="multiline">true to use a multiline match; otherwise, false.</param>
    string RegexpToString(string source, bool ignoreCase, bool global, bool multiline);

    /// <summary>When implemented in a class, converts the specified <see cref="T:System.String" /> to a <see cref="T:System.String" /> that contains escape characters for non-printable characters.</summary>
    /// <returns>The string representation of <paramref name="source" /> that contains escape characters.</returns>
    /// <param name="source">The value to convert.</param>
    string StringToPrintable(string source);

    /// <summary>When implemented in a class, creates a managed object that has the specified primitive value.</summary>
    /// <returns>
    /// <paramref name="value" /> converted to a managed <see cref="T:System.Object" />.</returns>
    /// <param name="value">The value to convert.</param>
    [return: MarshalAs(UnmanagedType.Interface)]
    object GetManagedObject(object value);

    /// <summary>When implemented in a class, creates a managed object that has the specified primitive <see cref="T:System.Int64" /> value.</summary>
    /// <returns>
    /// <paramref name="i" /> converted to an <see cref="T:System.Object" />.</returns>
    /// <param name="i">The value to convert.</param>
    [return: MarshalAs(UnmanagedType.Interface)]
    object GetManagedInt64Object(long i);

    /// <summary>When implemented in a class, creates a managed object that has the specified primitive <see cref="T:System.UInt64" /> value.</summary>
    /// <returns>
    /// <paramref name="i" /> converted to an <see cref="T:System.Object" />.</returns>
    /// <param name="i">The value to convert.</param>
    [return: MarshalAs(UnmanagedType.Interface)]
    object GetManagedUInt64Object(ulong i);

    /// <summary>When implemented in a class, creates a managed object that has the specified primitive <see cref="T:System.Char" /> value.</summary>
    /// <returns>
    /// <paramref name="i" /> converted to a <see cref="T:System.Char" /> and then an <see cref="T:System.Object" />.</returns>
    /// <param name="i">The value to convert.</param>
    [return: MarshalAs(UnmanagedType.Interface)]
    object GetManagedCharObject(ushort i);

    /// <summary>When implemented in a class, gets an error message that is created from the specified <see cref="P:System.Exception.HResult" /> and the <see cref="T:System.Globalization.CultureInfo" /> of the specified <see cref="T:Microsoft.JScript.Vsa.IJSVsaEngine" />.</summary>
    /// <returns>A localized string that contains information about the error.</returns>
    /// <param name="hr">The HRESULT error code.</param>
    /// <param name="engine">A reference to the scripting engine.</param>
    string GetErrorMessageForHR(int hr, IJSVsaEngine engine);
}