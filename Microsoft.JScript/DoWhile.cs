// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.DoWhile
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace Microsoft.JScript;

internal sealed class DoWhile : AST
{
    private AST body;
    private AST condition;
    private Completion completion;

    internal DoWhile(Context context, AST body, AST condition)
        : base(context)
    {
        this.body = body;
        this.condition = condition;
        this.completion = new Completion();
    }

    internal override object Evaluate()
    {
        this.completion.Continue = 0;
        this.completion.Exit = 0;
        this.completion.value = (object)null;
        do
        {
            Completion completion = (Completion)this.body.Evaluate();
            if (completion.value != null)
                this.completion.value = completion.value;
            if (completion.Continue > 1)
            {
                this.completion.Continue = completion.Continue - 1;
                break;
            }

            if (completion.Exit > 0)
            {
                this.completion.Exit = completion.Exit - 1;
                break;
            }

            if (completion.Return)
                return (object)completion;
        } while (Convert.ToBoolean(this.condition.Evaluate()));

        return (object)this.completion;
    }

    internal override AST PartiallyEvaluate()
    {
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
                    goto label_4;
            }
        }

        label_3:
        FunctionScope functionScope = (FunctionScope)scriptObject;
        BitArray definedFlags = functionScope.DefinedFlags;
        this.body = this.body.PartiallyEvaluate();
        functionScope.DefinedFlags = definedFlags;
        this.condition = this.condition.PartiallyEvaluate();
        functionScope.DefinedFlags = definedFlags;
        goto label_5;
        label_4:
        this.body = this.body.PartiallyEvaluate();
        this.condition = this.condition.PartiallyEvaluate();
        label_5:
        IReflect reflect = this.condition.InferType((JSField)null);
        if (reflect is FunctionPrototype || (object)reflect == (object)Typeob.ScriptFunction)
            this.context.HandleError(JSError.SuspectLoopCondition);
        return (AST)this;
    }

    internal override void TranslateToIL(ILGenerator il, Type rtype)
    {
        Label label = il.DefineLabel();
        Label loc1 = il.DefineLabel();
        Label loc2 = il.DefineLabel();
        this.compilerGlobals.BreakLabelStack.Push((object)loc2);
        this.compilerGlobals.ContinueLabelStack.Push((object)loc1);
        il.MarkLabel(label);
        this.body.TranslateToIL(il, Typeob.Void);
        il.MarkLabel(loc1);
        this.context.EmitLineInfo(il);
        this.condition.TranslateToConditionalBranch(il, true, label, false);
        il.MarkLabel(loc2);
        this.compilerGlobals.BreakLabelStack.Pop();
        this.compilerGlobals.ContinueLabelStack.Pop();
    }

    internal override void TranslateToILInitializer(ILGenerator il)
    {
        this.body.TranslateToILInitializer(il);
        this.condition.TranslateToILInitializer(il);
    }

    internal override Context GetFirstExecutableContext() => this.body.GetFirstExecutableContext();
}