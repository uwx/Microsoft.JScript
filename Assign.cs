// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.Assign
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace Microsoft.JScript;

internal sealed class Assign : AST
{
  internal AST lhside;
  internal AST rhside;

  internal Assign(Context context, AST lhside, AST rhside)
    : base(context)
  {
    this.lhside = lhside;
    this.rhside = rhside;
  }

  internal override object Evaluate()
  {
    try
    {
      if (this.lhside is Call)
        ((Call) this.lhside).EvaluateIndices();
      object obj = this.rhside.Evaluate();
      this.lhside.SetValue(obj);
      return obj;
    }
    catch (JScriptException ex)
    {
      if (ex.context == null)
        ex.context = this.context;
      throw ex;
    }
    catch (Exception ex)
    {
      throw new JScriptException(ex, this.context);
    }
  }

  internal override IReflect InferType(JSField inference_target)
  {
    return this.rhside.InferType(inference_target);
  }

  internal override AST PartiallyEvaluate()
  {
    AST asReference = this.lhside.PartiallyEvaluateAsReference();
    this.lhside = asReference;
    this.rhside = this.rhside.PartiallyEvaluate();
    asReference.SetPartialValue(this.rhside);
    return (AST) this;
  }

  internal override void TranslateToIL(ILGenerator il, Type rtype)
  {
    Type type1 = Convert.ToType(this.lhside.InferType((JSField) null));
    this.lhside.TranslateToILPreSet(il);
    if (rtype != Typeob.Void)
    {
      Type type2 = Convert.ToType(this.rhside.InferType((JSField) null));
      this.rhside.TranslateToIL(il, type2);
      LocalBuilder local = il.DeclareLocal(type2);
      il.Emit(OpCodes.Dup);
      il.Emit(OpCodes.Stloc, local);
      Convert.Emit((AST) this, il, type2, type1);
      this.lhside.TranslateToILSet(il);
      il.Emit(OpCodes.Ldloc, local);
      Convert.Emit((AST) this, il, type2, rtype);
    }
    else
      this.lhside.TranslateToILSet(il, this.rhside);
  }

  internal override void TranslateToILInitializer(ILGenerator il)
  {
    this.lhside.TranslateToILInitializer(il);
    this.rhside.TranslateToILInitializer(il);
  }
}
