// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.JSGlobalField
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System.Globalization;
using System.Reflection;

#nullable disable
namespace Microsoft.JScript;

internal sealed class JSGlobalField : JSVariableField
{
  internal FieldInfo ILField;

  internal JSGlobalField(
    ScriptObject obj,
    string name,
    object value,
    FieldAttributes attributeFlags)
    : base(name, obj, attributeFlags)
  {
    this.value = value;
    this.ILField = (FieldInfo) null;
  }

  public override object GetValue(object obj)
  {
    return this.ILField == (FieldInfo) null ? this.value : this.ILField.GetValue((object) null);
  }

  public override void SetValue(
    object obj,
    object value,
    BindingFlags invokeAttr,
    Binder binder,
    CultureInfo culture)
  {
    if (this.ILField != (FieldInfo) null)
      this.ILField.SetValue((object) null, value, invokeAttr, binder, culture);
    else if ((this.IsLiteral || this.IsInitOnly) && !(this.value is Missing))
    {
      if (!(this.value is FunctionObject) || !(value is FunctionObject) || !this.Name.Equals(((ScriptFunction) value).name))
        throw new JScriptException(JSError.AssignmentToReadOnly);
      this.value = value;
    }
    else if (this.type != null)
      this.value = Convert.Coerce(value, (object) this.type);
    else
      this.value = value;
  }
}
