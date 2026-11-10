// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.JSFieldMethod
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

internal sealed class JSFieldMethod : JSMethod
{
  internal FieldInfo field;
  internal FunctionObject func;
  private static readonly ParameterInfo[] EmptyParams = new ParameterInfo[0];

  internal JSFieldMethod(FieldInfo field, object obj)
    : base(obj)
  {
    this.field = field;
    this.func = (FunctionObject) null;
    if (!field.IsLiteral)
      return;
    object obj1 = field is JSVariableField ? ((JSVariableField) field).value : field.GetValue((object) null);
    if (!(obj1 is FunctionObject))
      return;
    this.func = (FunctionObject) obj1;
  }

  internal override object Construct(object[] args)
  {
    return LateBinding.CallValue(this.field.GetValue(this.obj), args, true, false, ((ScriptObject) this.obj).engine, (object) null, (Binder) JSBinder.ob, (CultureInfo) null, (string[]) null);
  }

  public override MethodAttributes Attributes
  {
    get
    {
      if (this.func != null)
        return this.func.attributes;
      if (this.field.IsPublic)
        return MethodAttributes.Public;
      if (this.field.IsFamily)
        return MethodAttributes.Family;
      return this.field.IsAssembly ? MethodAttributes.Assembly : MethodAttributes.Private;
    }
  }

  public override Type DeclaringType
  {
    get => this.func != null ? Convert.ToType((IReflect) this.func.enclosing_scope) : Typeob.Object;
  }

  internal ScriptObject EnclosingScope()
  {
    return this.func != null ? this.func.enclosing_scope : (ScriptObject) null;
  }

  public override object[] GetCustomAttributes(bool inherit)
  {
    if (this.func != null)
    {
      CustomAttributeList customAttributes = this.func.customAttributes;
      if (customAttributes != null)
        return (object[]) customAttributes.Evaluate(inherit);
    }
    return new object[0];
  }

  public override ParameterInfo[] GetParameters()
  {
    return this.func != null ? (ParameterInfo[]) this.func.parameter_declarations : JSFieldMethod.EmptyParams;
  }

  internal override MethodInfo GetMethodInfo(CompilerGlobals compilerGlobals)
  {
    return this.func.GetMethodInfo(compilerGlobals);
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
    bool construct = (options & BindingFlags.CreateInstance) != 0;
    bool brackets = (options & BindingFlags.GetProperty) != BindingFlags.Default && (options & BindingFlags.InvokeMethod) == BindingFlags.Default;
    object val = (object) this.func ?? this.field.GetValue(this.obj);
    FunctionObject functionObject = val as FunctionObject;
    if (!(obj is JSObject ob) || functionObject == null || !functionObject.isMethod || (functionObject.attributes & MethodAttributes.Virtual) == MethodAttributes.PrivateScope || ob.GetParent() == functionObject.enclosing_scope || !((ClassScope) functionObject.enclosing_scope).HasInstance((object) ob))
      return LateBinding.CallValue(val, parameters, construct, brackets, ((ScriptObject) this.obj).engine, thisob, binder, culture, (string[]) null);
    return new LateBinding(functionObject.name)
    {
      obj = ((object) ob)
    }.Call(parameters, construct, brackets, ((ScriptObject) this.obj).engine);
  }

  internal bool IsAccessibleFrom(ScriptObject scope)
  {
    return ((JSMemberField) this.field).IsAccessibleFrom(scope);
  }

  public override string Name => this.field.Name;

  public override Type ReturnType
  {
    get => this.func != null ? Convert.ToType(this.func.ReturnType((JSField) null)) : Typeob.Object;
  }

  internal IReflect ReturnIR()
  {
    return this.func != null ? this.func.ReturnType((JSField) null) : (IReflect) Typeob.Object;
  }
}
