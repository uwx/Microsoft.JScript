// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.JSWrappedMethod
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

internal sealed class JSWrappedMethod : JSMethod, IWrappedMember
{
  internal MethodInfo method;
  private ParameterInfo[] pars;

  internal JSWrappedMethod(MethodInfo method, object obj)
    : base(obj)
  {
    this.obj = obj;
    if (method is JSMethodInfo)
      method = ((JSMethodInfo) method).method;
    this.method = method.GetBaseDefinition();
    this.pars = this.method.GetParameters();
    if (!(obj is JSObject) || Typeob.JSObject.IsAssignableFrom(method.DeclaringType))
      return;
    switch (obj)
    {
      case BooleanObject _:
        this.obj = (object) ((BooleanObject) obj).value;
        break;
      case NumberObject _:
        this.obj = ((NumberObject) obj).value;
        break;
      case StringObject _:
        this.obj = (object) ((StringObject) obj).value;
        break;
      case ArrayWrapper _:
        this.obj = (object) ((ArrayWrapper) obj).value;
        break;
    }
  }

  public override MethodAttributes Attributes => this.method.Attributes;

  private object[] CheckArguments(object[] args)
  {
    object[] target = args;
    if (args != null && args.Length < this.pars.Length)
    {
      target = new object[this.pars.Length];
      ArrayObject.Copy(args, target, args.Length);
      int length1 = args.Length;
      for (int length2 = this.pars.Length; length1 < length2; ++length1)
        target[length1] = Type.Missing;
    }
    return target;
  }

  internal override object Construct(object[] args)
  {
    if (this.method is JSMethod)
      return ((JSMethod) this.method).Construct(args);
    if (this.method.GetParameters().Length == 0 && this.method.ReturnType == Typeob.Object)
    {
      object obj = this.method.Invoke(this.obj, BindingFlags.SuppressChangeType, (Binder) null, (object[]) null, (CultureInfo) null);
      if (obj is ScriptFunction)
        return ((ScriptFunction) obj).Construct(args);
    }
    throw new JScriptException(JSError.NoConstructor);
  }

  public override Type DeclaringType => this.method.DeclaringType;

  internal override string GetClassFullName()
  {
    return this.method is JSMethod ? ((JSMethod) this.method).GetClassFullName() : this.method.DeclaringType.FullName;
  }

  internal override PackageScope GetPackage()
  {
    return this.method is JSMethod ? ((JSMethod) this.method).GetPackage() : (PackageScope) null;
  }

  public override ParameterInfo[] GetParameters() => this.pars;

  internal override MethodInfo GetMethodInfo(CompilerGlobals compilerGlobals)
  {
    return this.method is JSMethod ? ((JSMethod) this.method).GetMethodInfo(compilerGlobals) : this.method;
  }

  public object GetWrappedObject() => this.obj;

  [DebuggerStepThrough]
  [DebuggerHidden]
  public override object Invoke(
    object obj,
    BindingFlags options,
    Binder binder,
    object[] parameters,
    CultureInfo culture)
  {
    parameters = this.CheckArguments(parameters);
    return this.Invoke(this.obj, this.obj, options, binder, parameters, culture);
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
    parameters = this.CheckArguments(parameters);
    if (this.obj != null && (object) (this.obj as Type) == null)
      obj = this.obj;
    return this.method is JSMethod ? ((JSMethod) this.method).Invoke(obj, thisob, options, binder, parameters, culture) : this.method.Invoke(obj, options, binder, parameters, culture);
  }

  public override string Name => this.method.Name;

  public override Type ReturnType => this.method.ReturnType;

  public override string ToString() => this.method.ToString();
}
