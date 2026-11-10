// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.COMSetterMethod
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System.Globalization;
using System.Reflection;

#nullable disable
namespace Microsoft.JScript;

internal class COMSetterMethod : COMMethodInfo
{
  public override object Invoke(
    object obj,
    BindingFlags invokeAttr,
    Binder binder,
    object[] parameters,
    CultureInfo culture)
  {
    int n = parameters.Length - 1;
    object parameter = parameters[n];
    object[] objArray;
    if (n > 0)
    {
      objArray = new object[n];
      ArrayObject.Copy(parameters, 0, objArray, 0, n);
    }
    else
      objArray = new object[0];
    this._comObject.SetValue(parameter, invokeAttr, binder, objArray, culture);
    return (object) null;
  }
}
