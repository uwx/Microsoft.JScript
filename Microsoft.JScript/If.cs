// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.If
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Collections;
using System.Reflection.Emit;

#nullable disable
namespace Microsoft.JScript;

internal sealed class If : AST
{
    private AST condition;
    private AST operand1;
    private AST operand2;
    private Completion completion;

    internal If(Context context, AST condition, AST true_branch, AST false_branch)
        : base(context)
    {
        this.condition = condition;
        this.operand1 = true_branch;
        this.operand2 = false_branch;
        this.completion = new Completion();
    }

    internal override object Evaluate()
    {
        if (this.operand1 == null && this.operand2 == null)
            return (object)this.completion;
        Completion completion = this.condition == null
            ? (this.operand1 == null ? (Completion)this.operand2.Evaluate() : (Completion)this.operand1.Evaluate())
            : (!Convert.ToBoolean(this.condition.Evaluate())
                ? (this.operand2 == null ? new Completion() : (Completion)this.operand2.Evaluate())
                : (Completion)this.operand1.Evaluate());
        this.completion.value = completion.value;
        this.completion.Continue = completion.Continue <= 1 ? 0 : completion.Continue - 1;
        this.completion.Exit = completion.Exit <= 0 ? 0 : completion.Exit - 1;
        return completion.Return ? (object)completion : (object)this.completion;
    }

    internal override bool HasReturn()
    {
        return this.operand1 != null
            ? this.operand1.HasReturn() && this.operand2 != null && this.operand2.HasReturn()
            : this.operand2 != null && this.operand2.HasReturn();
    }

    internal override AST PartiallyEvaluate()
    {
        this.condition = this.condition.PartiallyEvaluate();
        if (this.condition is ConstantWrapper)
        {
            if (Convert.ToBoolean(this.condition.Evaluate()))
                this.operand2 = (AST)null;
            else
                this.operand1 = (AST)null;
            this.condition = (AST)null;
        }

        ScriptObject scriptObject = this.Globals.ScopeStack.Peek();
        while (scriptObject is WithObject)
            scriptObject = scriptObject.GetParent();
        if (scriptObject is FunctionScope)
        {
            FunctionScope functionScope = (FunctionScope)scriptObject;
            BitArray bitArray1 = functionScope.DefinedFlags;
            BitArray bitArray2 = bitArray1;
            if (this.operand1 != null)
            {
                this.operand1 = this.operand1.PartiallyEvaluate();
                bitArray2 = functionScope.DefinedFlags;
                functionScope.DefinedFlags = bitArray1;
            }

            if (this.operand2 != null)
            {
                this.operand2 = this.operand2.PartiallyEvaluate();
                BitArray definedFlags = functionScope.DefinedFlags;
                int length1 = bitArray2.Length;
                int length2 = definedFlags.Length;
                if (length1 < length2)
                    bitArray2.Length = length2;
                if (length2 < length1)
                    definedFlags.Length = length1;
                bitArray1 = bitArray2.And(definedFlags);
            }

            functionScope.DefinedFlags = bitArray1;
        }
        else
        {
            if (this.operand1 != null)
                this.operand1 = this.operand1.PartiallyEvaluate();
            if (this.operand2 != null)
                this.operand2 = this.operand2.PartiallyEvaluate();
        }

        return (AST)this;
    }

    internal override void TranslateToIL(ILGenerator il, Type rtype)
    {
        if (this.operand1 == null && this.operand2 == null)
            return;
        Label label1 = il.DefineLabel();
        Label label2 = il.DefineLabel();
        this.compilerGlobals.BreakLabelStack.Push((object)label2);
        this.compilerGlobals.ContinueLabelStack.Push((object)label2);
        if (this.condition != null)
        {
            this.context.EmitLineInfo(il);
            if (this.operand2 != null)
                this.condition.TranslateToConditionalBranch(il, false, label1, false);
            else
                this.condition.TranslateToConditionalBranch(il, false, label2, false);
            if (this.operand1 != null)
                this.operand1.TranslateToIL(il, Typeob.Void);
            if (this.operand2 != null)
            {
                if (this.operand1 != null && !this.operand1.HasReturn())
                    il.Emit(OpCodes.Br, label2);
                il.MarkLabel(label1);
                this.operand2.TranslateToIL(il, Typeob.Void);
            }
        }
        else if (this.operand1 != null)
            this.operand1.TranslateToIL(il, Typeob.Void);
        else
            this.operand2.TranslateToIL(il, Typeob.Void);

        il.MarkLabel(label2);
        this.compilerGlobals.BreakLabelStack.Pop();
        this.compilerGlobals.ContinueLabelStack.Pop();
    }

    internal override void TranslateToILInitializer(ILGenerator il)
    {
        if (this.condition != null)
            this.condition.TranslateToILInitializer(il);
        if (this.operand1 != null)
            this.operand1.TranslateToILInitializer(il);
        if (this.operand2 == null)
            return;
        this.operand2.TranslateToILInitializer(il);
    }
}