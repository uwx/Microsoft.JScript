// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.JSNativeMethod
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using Microsoft.JScript.Vsa;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;

#nullable disable
namespace Microsoft.JScript;

internal sealed class JSNativeMethod : JSMethod
{
  private MethodInfo method;
  private ParameterInfo[] formalParams;
  private bool hasThis;
  private bool hasVarargs;
  private bool hasEngine;
  private VsaEngine engine;

  internal JSNativeMethod(MethodInfo method, object obj, VsaEngine engine)
    : base(obj)
  {
    this.method = method;
    this.formalParams = method.GetParameters();
    object[] customAttributes = CustomAttribute.GetCustomAttributes((MemberInfo) method, typeof (JSFunctionAttribute), false);
    JSFunctionAttributeEnum attributeValue = (customAttributes.Length != 0 ? (JSFunctionAttribute) customAttributes[0] : new JSFunctionAttribute(JSFunctionAttributeEnum.None)).attributeValue;
    if ((attributeValue & JSFunctionAttributeEnum.HasThisObject) != JSFunctionAttributeEnum.None)
      this.hasThis = true;
    if ((attributeValue & JSFunctionAttributeEnum.HasEngine) != JSFunctionAttributeEnum.None)
      this.hasEngine = true;
    if ((attributeValue & JSFunctionAttributeEnum.HasVarArgs) != JSFunctionAttributeEnum.None)
      this.hasVarargs = true;
    this.engine = engine;
  }

  internal override object Construct(object[] args)
  {
    throw new JScriptException(JSError.NoConstructor);
  }

  public override MethodAttributes Attributes => this.method.Attributes;

  public override Type DeclaringType => this.method.DeclaringType;

  public override ParameterInfo[] GetParameters() => this.formalParams;

  internal override MethodInfo GetMethodInfo(CompilerGlobals compilerGlobals) => this.method;

  [DebuggerStepThrough]
  [DebuggerHidden]
  internal override object Invoke(
    object obj,
    object thisob,
    BindingFlags options,
    Binder binder,
    object[] parameters,
    CultureInfo culture)
  {
    int length1 = this.formalParams.Length;
    int length2 = parameters != null ? parameters.Length : 0;
    if (!this.hasThis && !this.hasVarargs && length1 == length2)
      return binder != null ? TypeReferences.ToExecutionContext(this.method).Invoke(this.obj, BindingFlags.SuppressChangeType, (Binder) null, this.ConvertParams(0, parameters, binder, culture), (CultureInfo) null) : TypeReferences.ToExecutionContext(this.method).Invoke(this.obj, options, binder, parameters, culture);
    int index1 = (this.hasThis ? 1 : 0) + (this.hasEngine ? 1 : 0);
    object[] objArray = new object[length1];
    if (this.hasThis)
    {
      objArray[0] = thisob;
      if (this.hasEngine)
        objArray[1] = (object) this.engine;
    }
    else if (this.hasEngine)
      objArray[0] = (object) this.engine;
    if (this.hasVarargs)
    {
      if (length1 == index1 + 1)
      {
        objArray[index1] = (object) parameters;
      }
      else
      {
        int num = length1 - 1 - index1;
        if (length2 > num)
        {
          ArrayObject.Copy(parameters, 0, objArray, index1, num);
          int n = length2 - num;
          object[] target = new object[n];
          ArrayObject.Copy(parameters, num, target, 0, n);
          objArray[length1 - 1] = (object) target;
        }
        else
        {
          ArrayObject.Copy(parameters, 0, objArray, index1, length2);
          for (int index2 = length2; index2 < num; ++index2)
            objArray[index2 + index1] = (object) Missing.Value;
          objArray[length1 - 1] = (object) new object[0];
        }
      }
    }
    else
    {
      if (parameters != null)
      {
        if (length1 - index1 < length2)
          ArrayObject.Copy(parameters, 0, objArray, index1, length1 - index1);
        else
          ArrayObject.Copy(parameters, 0, objArray, index1, length2);
      }
      if (length1 - index1 > length2)
      {
        for (int index3 = length2 + index1; index3 < length1; ++index3)
          objArray[index3] = index3 != length1 - 1 || !this.formalParams[index3].ParameterType.IsArray || !CustomAttribute.IsDefined(this.formalParams[index3], typeof (ParamArrayAttribute), true) ? (object) Missing.Value : (object) Array.CreateInstance(this.formalParams[index3].ParameterType.GetElementType(), 0);
      }
    }
    return binder != null ? TypeReferences.ToExecutionContext(this.method).Invoke(this.obj, BindingFlags.SuppressChangeType, (Binder) null, this.ConvertParams(index1, objArray, binder, culture), (CultureInfo) null) : TypeReferences.ToExecutionContext(this.method).Invoke(this.obj, options, binder, objArray, culture);
  }

  private object[] ConvertParams(
    int offset,
    object[] parameters,
    Binder binder,
    CultureInfo culture)
  {
    int length = this.formalParams.Length;
    if (this.hasVarargs)
      --length;
    for (int index = offset; index < length; ++index)
    {
      Type parameterType = this.formalParams[index].ParameterType;
      if (parameterType != Typeob.Object)
        parameters[index] = binder.ChangeType(parameters[index], parameterType, culture);
    }
    return parameters;
  }

  public override string Name => this.method.Name;

  public override Type ReturnType => this.method.ReturnType;
}
