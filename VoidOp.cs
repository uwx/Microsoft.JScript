// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.VoidOp
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace Microsoft.JScript;

internal sealed class VoidOp : UnaryOp
{
  internal VoidOp(Context context, AST operand)
    : base(context, operand)
  {
  }

  internal override object Evaluate()
  {
    this.operand.Evaluate();
    return (object) null;
  }

  internal override IReflect InferType(JSField inference_target) => (IReflect) Typeob.Empty;

  internal override AST PartiallyEvaluate()
  {
    return (AST) new ConstantWrapper((object) null, this.context);
  }

  internal override void TranslateToIL(ILGenerator il, Type rtype)
  {
    this.operand.TranslateToIL(il, Typeob.Object);
    if (rtype != Typeob.Void)
    {
      il.Emit(OpCodes.Ldsfld, CompilerGlobals.undefinedField);
      Convert.Emit((AST) this, il, Typeob.Object, rtype);
    }
    else
      il.Emit(OpCodes.Pop);
  }
}
