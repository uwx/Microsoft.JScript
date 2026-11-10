// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.JSPropertyField
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Globalization;
using System.Reflection;

#nullable disable
namespace Microsoft.JScript;

internal sealed class JSPropertyField : JSField
{
  internal PropertyInfo wrappedProperty;
  internal object wrappedObject;

  internal JSPropertyField(PropertyInfo field, object obj)
  {
    this.wrappedProperty = field;
    this.wrappedObject = obj;
  }

  public override string Name => this.wrappedProperty.Name;

  public override FieldAttributes Attributes => FieldAttributes.Public;

  public override Type DeclaringType => this.wrappedProperty.DeclaringType;

  public override Type FieldType => this.wrappedProperty.PropertyType;

  public override object GetValue(object obj)
  {
    return this.wrappedProperty.GetValue(this.wrappedObject, new object[0]);
  }

  public override void SetValue(
    object obj,
    object value,
    BindingFlags invokeAttr,
    Binder binder,
    CultureInfo locale)
  {
    this.wrappedProperty.SetValue(this.wrappedObject, value, invokeAttr, binder, new object[0], locale);
  }
}
