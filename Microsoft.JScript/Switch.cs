// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.Switch
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Collections;
using System.Reflection.Emit;

#nullable disable
namespace Microsoft.JScript;

internal sealed class Switch : AST
{
    private AST expression;
    private ASTList cases;
    private int default_case;
    private Completion completion;

    internal Switch(Context context, AST expression, ASTList cases)
        : base(context)
    {
        this.expression = expression;
        this.cases = cases;
        this.default_case = -1;
        int i = 0;
        for (int count = this.cases.count; i < count; ++i)
        {
            if (((SwitchCase)this.cases[i]).IsDefault())
            {
                this.default_case = i;
                break;
            }
        }

        this.completion = new Completion();
    }

    internal override object Evaluate()
    {
        this.completion.Continue = 0;
        this.completion.Exit = 0;
        this.completion.value = (object)null;
        object expression = this.expression.Evaluate();
        Completion completion = (Completion)null;
        int count = this.cases.count;
        int i;
        for (i = 0; i < count; ++i)
        {
            if (i != this.default_case)
            {
                completion = ((SwitchCase)this.cases[i]).Evaluate(expression);
                if (completion != null)
                    break;
            }
        }

        if (completion == null)
        {
            if (this.default_case < 0)
                return (object)this.completion;
            i = this.default_case;
            completion = (Completion)this.cases[i].Evaluate();
        }

        while (true)
        {
            if (completion.value != null)
                this.completion.value = completion.value;
            if (completion.Continue <= 0)
            {
                if (completion.Exit <= 0)
                {
                    if (!completion.Return)
                    {
                        if (i < count - 1)
                            completion = (Completion)this.cases[++i].Evaluate();
                        else
                            goto label_18;
                    }
                    else
                        goto label_16;
                }
                else
                    goto label_14;
            }
            else
                break;
        }

        this.completion.Continue = completion.Continue - 1;
        goto label_20;
        label_14:
        this.completion.Exit = completion.Exit - 1;
        goto label_20;
        label_16:
        return (object)completion;
        label_18:
        return (object)this.completion;
        label_20:
        return (object)this.completion;
    }

    internal override AST PartiallyEvaluate()
    {
        this.expression = this.expression.PartiallyEvaluate();
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
                    goto label_6;
            }
        }

        label_3:
        FunctionScope functionScope = (FunctionScope)scriptObject;
        BitArray definedFlags = functionScope.DefinedFlags;
        int i1 = 0;
        for (int count = this.cases.count; i1 < count; ++i1)
        {
            this.cases[i1] = this.cases[i1].PartiallyEvaluate();
            functionScope.DefinedFlags = definedFlags;
        }

        goto label_9;
        label_6:
        int i2 = 0;
        for (int count = this.cases.count; i2 < count; ++i2)
            this.cases[i2] = this.cases[i2].PartiallyEvaluate();
        label_9:
        return (AST)this;
    }

    internal override void TranslateToIL(ILGenerator il, Type rtype)
    {
        Type type = Convert.ToType(this.expression.InferType((JSField)null));
        this.expression.context.EmitLineInfo(il);
        this.expression.TranslateToIL(il, type);
        LocalBuilder local = il.DeclareLocal(type);
        il.Emit(OpCodes.Stloc, local);
        int count = this.cases.count;
        Label[] labelArray = new Label[this.cases.count];
        for (int i = 0; i < count; ++i)
        {
            labelArray[i] = il.DefineLabel();
            if (i != this.default_case)
            {
                il.Emit(OpCodes.Ldloc, local);
                ((SwitchCase)this.cases[i]).TranslateToConditionalBranch(il, type, true, labelArray[i], false);
            }
        }

        Label label = il.DefineLabel();
        if (this.default_case >= 0)
            il.Emit(OpCodes.Br, labelArray[this.default_case]);
        else
            il.Emit(OpCodes.Br, label);
        this.compilerGlobals.BreakLabelStack.Push((object)label);
        this.compilerGlobals.ContinueLabelStack.Push((object)label);
        for (int i = 0; i < count; ++i)
        {
            il.MarkLabel(labelArray[i]);
            this.cases[i].TranslateToIL(il, Typeob.Void);
        }

        il.MarkLabel(label);
        this.compilerGlobals.BreakLabelStack.Pop();
        this.compilerGlobals.ContinueLabelStack.Pop();
    }

    internal override void TranslateToILInitializer(ILGenerator il)
    {
        this.expression.TranslateToILInitializer(il);
        int i = 0;
        for (int count = this.cases.count; i < count; ++i)
            this.cases[i].TranslateToILInitializer(il);
    }

    internal override Context GetFirstExecutableContext() => this.expression.context;
}