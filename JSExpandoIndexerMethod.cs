// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.JSExpandoIndexerMethod
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;

#nullable disable
namespace Microsoft.JScript;

internal sealed class JSExpandoIndexerMethod : JSMethod
{
  private ClassScope classScope;
  private bool isGetter;
  private MethodInfo token;
  private ParameterInfo[] GetterParams;
  private ParameterInfo[] SetterParams;

  internal JSExpandoIndexerMethod(ClassScope classScope, bool isGetter)
    : base((object) null)
  {
    this.isGetter = isGetter;
    this.classScope = classScope;
    this.GetterParams = new ParameterInfo[1]
    {
      (ParameterInfo) new ParameterDeclaration(Typeob.String, "field")
    };
    this.SetterParams = new ParameterInfo[2]
    {
      (ParameterInfo) new ParameterDeclaration(Typeob.String, "field"),
      (ParameterInfo) new ParameterDeclaration(Typeob.Object, "value")
    };
  }

  internal override object Construct(object[] args)
  {
    throw new JScriptException(JSError.InvalidCall);
  }

  public override MethodAttributes Attributes => MethodAttributes.Public;

  public override Type DeclaringType => this.classScope.GetTypeBuilderOrEnumBuilder();

  public override ParameterInfo[] GetParameters()
  {
    return this.isGetter ? this.GetterParams : this.SetterParams;
  }

  internal override MethodInfo GetMethodInfo(CompilerGlobals compilerGlobals)
  {
    if (this.isGetter)
    {
      if (this.token == (MethodInfo) null)
        this.token = this.classScope.owner.GetExpandoIndexerGetter();
    }
    else if (this.token == (MethodInfo) null)
      this.token = this.classScope.owner.GetExpandoIndexerSetter();
    return this.token;
  }

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
    throw new JScriptException(JSError.InvalidCall);
  }

  public override string Name => this.isGetter ? "get_Item" : "set_Item";

  public override Type ReturnType => this.isGetter ? Typeob.Object : Typeob.Void;
}
