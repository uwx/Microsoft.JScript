// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.Expression
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Reflection.Emit;

#nullable disable
namespace Microsoft.JScript;

internal sealed class Expression : AST
{
    internal AST operand;
    private Completion completion;

    internal Expression(Context context, AST operand)
        : base(context)
    {
        this.operand = operand;
        this.completion = new Completion();
    }

    internal override object Evaluate()
    {
        this.completion.value = this.operand.Evaluate();
        return (object)this.completion;
    }

    internal override AST PartiallyEvaluate()
    {
        this.operand = this.operand.PartiallyEvaluate();
        if (this.operand is ConstantWrapper)
            this.operand.context.HandleError(JSError.UselessExpression);
        else if (this.operand is Binding)
            ((Binding)this.operand).CheckIfUseless();
        return (AST)this;
    }

    internal override void TranslateToIL(ILGenerator il, Type rtype)
    {
        this.context.EmitLineInfo(il);
        this.operand.TranslateToIL(il, rtype);
    }

    internal override void TranslateToILInitializer(ILGenerator il)
    {
        this.operand.TranslateToILInitializer(il);
    }
}