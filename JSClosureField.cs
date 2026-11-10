// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.JSClosureField
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Globalization;
using System.Reflection;

#nullable disable
namespace Microsoft.JScript;

internal sealed class JSClosureField : JSVariableField
{
  internal FieldInfo field;

  internal JSClosureField(FieldInfo field)
    : base(field.Name, (ScriptObject) null, field.Attributes | FieldAttributes.Static)
  {
    if (field is JSFieldInfo)
      field = ((JSFieldInfo) field).field;
    this.field = field;
  }

  public override Type DeclaringType => this.field.DeclaringType;

  public override Type FieldType => this.field.FieldType;

  internal override IReflect GetInferredType(JSField inference_target)
  {
    return this.field is JSMemberField ? ((JSVariableField) this.field).GetInferredType(inference_target) : (IReflect) this.field.FieldType;
  }

  internal override object GetMetaData()
  {
    return this.field is JSField ? ((JSField) this.field).GetMetaData() : (object) this.field;
  }

  public override object GetValue(object obj)
  {
    if (obj is StackFrame)
      return this.field.GetValue(((StackFrame) ((ScriptObject) obj).engine.ScriptObjectStackTop()).closureInstance);
    throw new JScriptException(JSError.InternalError);
  }

  public override void SetValue(
    object obj,
    object value,
    BindingFlags invokeAttr,
    Binder binder,
    CultureInfo locale)
  {
    if (!(obj is StackFrame))
      throw new JScriptException(JSError.InternalError);
    this.field.SetValue(((StackFrame) ((ScriptObject) obj).engine.ScriptObjectStackTop()).closureInstance, value, invokeAttr, binder, locale);
  }
}
