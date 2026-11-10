// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.COMMemberInfo
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Defines methods that enable JScript to communicate with an external COM member. This interface is used by an external COM implementation to expose its objects to JScript because JScript requires a managed <see cref="T:System.Reflection.MemberInfo" /> to bind to an external object.</summary>
[Guid("84BCEB62-16EB-4e1c-975C-FCB40D331043")]
[ComVisible(true)]
public interface COMMemberInfo
{
  /// <summary>When implemented in a class, enables JScript to call an external COM member. Uses the specified binding information.</summary>
  /// <returns>The value of the call to the external COM member.</returns>
  /// <param name="invokeAttr">A bitwise combination of the enumeration values that control the binding.</param>
  /// <param name="binder">An object that performs type conversion from actual argument type to formal argument type.</param>
  /// <param name="arguments">The argument list to pass to the external COM member.</param>
  /// <param name="culture">Provides information about a specific culture or locale to format numbers, dates, and strings correctly.</param>
  object Call(BindingFlags invokeAttr, Binder binder, object[] arguments, CultureInfo culture);

  /// <summary>When implemented in a class, enables JScript to get the value of an external COM member using the specified binding information.</summary>
  /// <returns>The value of the external COM member.</returns>
  /// <param name="invokeAttr">A bitwise combination of the enumeration values that control the binding.</param>
  /// <param name="binder">An object that performs type conversion from actual argument type to formal argument type.</param>
  /// <param name="index">The argument list to pass to the external COM member.</param>
  /// <param name="culture">Provides information about a specific culture or locale to format numbers, dates, and strings correctly.</param>
  object GetValue(BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture);

  /// <summary>When implemented in a class, enables JScript to set the value of an external COM member to the specified new value, using the specified binding information.</summary>
  /// <param name="value">The new value of the external COM member.</param>
  /// <param name="invokeAttr">A bitwise combination of the enumeration values that control the binding.</param>
  /// <param name="binder">An object that performs type conversion from actual argument type to formal argument type.</param>
  /// <param name="index">The argument list to pass to the external COM member.</param>
  /// <param name="culture">Provides information about a specific culture or locale to format numbers, dates, and strings correctly.</param>
  void SetValue(
    object value,
    BindingFlags invokeAttr,
    Binder binder,
    object[] index,
    CultureInfo culture);
}
