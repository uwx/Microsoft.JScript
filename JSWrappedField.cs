// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.JSWrappedField
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Globalization;
using System.Reflection;

#nullable disable
namespace Microsoft.JScript;

internal sealed class JSWrappedField : JSField, IWrappedMember
{
  internal FieldInfo wrappedField;
  internal object wrappedObject;

  internal JSWrappedField(FieldInfo field, object obj)
  {
    if (field is JSFieldInfo)
      field = ((JSFieldInfo) field).field;
    this.wrappedField = field;
    this.wrappedObject = obj;
    if (!(obj is JSObject) || Typeob.JSObject.IsAssignableFrom(field.DeclaringType))
      return;
    switch (obj)
    {
      case BooleanObject _:
        this.wrappedObject = (object) ((BooleanObject) obj).value;
        break;
      case NumberObject _:
        this.wrappedObject = ((NumberObject) obj).value;
        break;
      case StringObject _:
        this.wrappedObject = (object) ((StringObject) obj).value;
        break;
    }
  }

  public override string Name => this.wrappedField.Name;

  public override FieldAttributes Attributes => this.wrappedField.Attributes;

  public override Type DeclaringType => this.wrappedField.DeclaringType;

  public override Type FieldType => this.wrappedField.FieldType;

  internal override string GetClassFullName()
  {
    return this.wrappedField is JSField ? ((JSField) this.wrappedField).GetClassFullName() : this.wrappedField.DeclaringType.FullName;
  }

  internal override object GetMetaData()
  {
    return this.wrappedField is JSField ? ((JSField) this.wrappedField).GetMetaData() : (object) this.wrappedField;
  }

  internal override PackageScope GetPackage()
  {
    return this.wrappedField is JSField ? ((JSField) this.wrappedField).GetPackage() : (PackageScope) null;
  }

  public override object GetValue(object obj) => this.wrappedField.GetValue(this.wrappedObject);

  public object GetWrappedObject() => this.wrappedObject;

  public override void SetValue(
    object obj,
    object value,
    BindingFlags invokeAttr,
    Binder binder,
    CultureInfo locale)
  {
    this.wrappedField.SetValue(this.wrappedObject, value, invokeAttr, binder, locale);
  }
}
