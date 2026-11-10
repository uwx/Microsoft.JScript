// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.While
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

internal sealed class While : AST
{
    private AST condition;
    private AST body;
    private Completion completion;

    internal While(Context context, AST condition, AST body)
        : base(context)
    {
        this.condition = condition;
        this.body = body;
        this.completion = new Completion();
    }

    internal override object Evaluate()
    {
        this.completion.Continue = 0;
        this.completion.Exit = 0;
        this.completion.value = (object)null;
        while (Convert.ToBoolean(this.condition.Evaluate()))
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
        }

        return (object)this.completion;
    }

    internal override AST PartiallyEvaluate()
    {
        this.condition = this.condition.PartiallyEvaluate();
        IReflect reflect = this.condition.InferType((JSField)null);
        if (reflect is FunctionPrototype || (object)reflect == (object)Typeob.ScriptFunction)
            this.context.HandleError(JSError.SuspectLoopCondition);
        ScriptObject scriptObject = this.Globals.ScopeStack.Peek();
        while (true)
        {
            switch (scriptObject)
            {
                case WithObject _:
                    scriptObject = scriptObject.GetParent();
                    continue;
                case FunctionScope _:
                    goto label_5;
                default:
                    goto label_6;
            }
        }

        label_5:
        FunctionScope functionScope = (FunctionScope)scriptObject;
        BitArray definedFlags = functionScope.DefinedFlags;
        this.body = this.body.PartiallyEvaluate();
        functionScope.DefinedFlags = definedFlags;
        goto label_7;
        label_6:
        this.body = this.body.PartiallyEvaluate();
        label_7:
        return (AST)this;
    }

    internal override void TranslateToIL(ILGenerator il, Type rtype)
    {
        Label label1 = il.DefineLabel();
        Label loc = il.DefineLabel();
        Label label2 = il.DefineLabel();
        this.compilerGlobals.BreakLabelStack.Push((object)loc);
        this.compilerGlobals.ContinueLabelStack.Push((object)label1);
        il.Emit(OpCodes.Br, label1);
        il.MarkLabel(label2);
        this.body.TranslateToIL(il, Typeob.Void);
        il.MarkLabel(label1);
        this.context.EmitLineInfo(il);
        this.condition.TranslateToConditionalBranch(il, true, label2, false);
        il.MarkLabel(loc);
        this.compilerGlobals.BreakLabelStack.Pop();
        this.compilerGlobals.ContinueLabelStack.Pop();
    }

    internal override void TranslateToILInitializer(ILGenerator il)
    {
        this.condition.TranslateToILInitializer(il);
        this.body.TranslateToILInitializer(il);
    }
}