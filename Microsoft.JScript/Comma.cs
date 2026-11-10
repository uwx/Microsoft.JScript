// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.Comma
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace Microsoft.JScript;

internal sealed class Comma : BinaryOp
{
    internal Comma(Context context, AST operand1, AST operand2)
        : base(context, operand1, operand2)
    {
    }

    internal override object Evaluate()
    {
        this.operand1.Evaluate();
        return this.operand2.Evaluate();
    }

    internal override IReflect InferType(JSField inference_target)
    {
        return this.operand2.InferType(inference_target);
    }

    internal override void TranslateToIL(ILGenerator il, Type rtype)
    {
        this.operand1.TranslateToIL(il, Typeob.Void);
        this.operand2.TranslateToIL(il, rtype);
    }
}