// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.COMGetterMethod
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System.Globalization;
using System.Reflection;

#nullable disable
namespace Microsoft.JScript;

internal class COMGetterMethod : COMMethodInfo
{
  public override object Invoke(
    object obj,
    BindingFlags invokeAttr,
    Binder binder,
    object[] parameters,
    CultureInfo culture)
  {
    return this._comObject.GetValue(invokeAttr, binder, parameters != null ? parameters : new object[0], culture);
  }
}
