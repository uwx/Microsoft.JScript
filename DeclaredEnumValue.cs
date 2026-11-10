// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.DeclaredEnumValue
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Reflection;

#nullable disable
namespace Microsoft.JScript;

internal sealed class DeclaredEnumValue : EnumWrapper
{
  private string _name;
  internal ClassScope _classScope;
  internal object _value;

  internal DeclaredEnumValue(object value, string name, ClassScope classScope)
  {
    this._name = name;
    this._classScope = classScope;
    this._value = value;
  }

  internal override object value => this._value;

  internal override Type type => this._classScope.GetTypeBuilderOrEnumBuilder();

  internal override string name => this._name;

  internal override IReflect classScopeOrType => (IReflect) this._classScope;

  internal void CoerceToBaseType(Type bt, Context errCtx)
  {
    object obj = (object) 0;
    AST ast = ((AST) this.value).PartiallyEvaluate();
    if (ast is ConstantWrapper)
      obj = ast.Evaluate();
    else
      ast.context.HandleError(JSError.NotConst);
    try
    {
      this._value = Convert.CoerceT(obj, bt);
    }
    catch
    {
      errCtx.HandleError(JSError.TypeMismatch);
      this._value = Convert.CoerceT((object) 0, bt);
    }
  }
}
