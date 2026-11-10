// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.Conditional
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Collections;
using System.Reflection.Emit;

#nullable disable
namespace Microsoft.JScript;

internal sealed class Conditional : AST
{
    private AST condition;
    private AST operand1;
    private AST operand2;

    internal Conditional(Context context, AST condition, AST operand1, AST operand2)
        : base(context)
    {
        this.condition = condition;
        this.operand1 = operand1;
        this.operand2 = operand2;
    }

    internal override object Evaluate()
    {
        return Convert.ToBoolean(this.condition.Evaluate()) ? this.operand1.Evaluate() : this.operand2.Evaluate();
    }

    internal override AST PartiallyEvaluate()
    {
        this.condition = this.condition.PartiallyEvaluate();
        ScriptObject scriptObject = this.Globals.ScopeStack.Peek();
        while (true)
        {
            switch (scriptObject)
            {
                case WithObject _:
                    scriptObject = scriptObject.GetParent();
                    continue;
                case FunctionScope _:
                    goto label_3;
                default:
                    goto label_8;
            }
        }

        label_3:
        FunctionScope functionScope = (FunctionScope)scriptObject;
        BitArray definedFlags1 = functionScope.DefinedFlags;
        this.operand1 = this.operand1.PartiallyEvaluate();
        BitArray definedFlags2 = functionScope.DefinedFlags;
        functionScope.DefinedFlags = definedFlags1;
        this.operand2 = this.operand2.PartiallyEvaluate();
        BitArray definedFlags3 = functionScope.DefinedFlags;
        int length1 = definedFlags2.Length;
        int length2 = definedFlags3.Length;
        if (length1 < length2)
            definedFlags2.Length = length2;
        if (length2 < length1)
            definedFlags3.Length = length1;
        functionScope.DefinedFlags = definedFlags2.And(definedFlags3);
        goto label_9;
        label_8:
        this.operand1 = this.operand1.PartiallyEvaluate();
        this.operand2 = this.operand2.PartiallyEvaluate();
        label_9:
        return (AST)this;
    }

    internal override void TranslateToIL(ILGenerator il, Type rtype)
    {
        Label label1 = il.DefineLabel();
        Label label2 = il.DefineLabel();
        this.condition.TranslateToConditionalBranch(il, false, label1, false);
        this.operand1.TranslateToIL(il, rtype);
        il.Emit(OpCodes.Br, label2);
        il.MarkLabel(label1);
        this.operand2.TranslateToIL(il, rtype);
        il.MarkLabel(label2);
    }

    internal override void TranslateToILInitializer(ILGenerator il)
    {
        this.condition.TranslateToILInitializer(il);
        this.operand1.TranslateToILInitializer(il);
        this.operand2.TranslateToILInitializer(il);
    }
}