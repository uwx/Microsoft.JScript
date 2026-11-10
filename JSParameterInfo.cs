// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.JSParameterInfo
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Reflection;

#nullable disable
namespace Microsoft.JScript;

internal sealed class JSParameterInfo : ParameterInfo
{
  private ParameterInfo parameter;
  private Type parameterType;
  private object[] attributes;

  internal JSParameterInfo(ParameterInfo parameter) => this.parameter = parameter;

  public override object DefaultValue => TypeReferences.GetDefaultParameterValue(this.parameter);

  public sealed override object[] GetCustomAttributes(bool inherit)
  {
    return this.attributes ?? (this.attributes = this.parameter.GetCustomAttributes(true));
  }

  public sealed override object[] GetCustomAttributes(Type type, bool inherit)
  {
    return this.attributes ?? (this.attributes = CustomAttribute.GetCustomAttributes(this.parameter, type, true));
  }

  public sealed override bool IsDefined(Type type, bool inherit)
  {
    object[] objArray = this.attributes;
    if (objArray == null)
      this.attributes = objArray = CustomAttribute.GetCustomAttributes(this.parameter, type, true);
    return objArray.Length != 0;
  }

  public override string Name => this.parameter.Name;

  public override Type ParameterType
  {
    get
    {
      Type parameterType = this.parameterType;
      return parameterType != (Type) null ? parameterType : (this.parameterType = this.parameter.ParameterType);
    }
  }
}
