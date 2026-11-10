// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.Break
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Reflection.Emit;

#nullable disable
namespace Microsoft.JScript;

internal sealed class Break : AST
{
    private Completion completion;
    private bool leavesFinally;

    internal Break(Context context, int count, bool leavesFinally)
        : base(context)
    {
        this.completion = new Completion();
        this.completion.Exit = count;
        this.leavesFinally = leavesFinally;
    }

    internal override object Evaluate() => (object)this.completion;

    internal override AST PartiallyEvaluate()
    {
        if (this.leavesFinally)
            this.context.HandleError(JSError.BadWayToLeaveFinally);
        return (AST)this;
    }

    internal override void TranslateToIL(ILGenerator il, Type rtype)
    {
        Label label = (Label)this.compilerGlobals.BreakLabelStack.Peek(this.completion.Exit - 1);
        this.context.EmitLineInfo(il);
        if (this.leavesFinally)
        {
            ConstantWrapper.TranslateToILInt(il, this.compilerGlobals.BreakLabelStack.Size() - this.completion.Exit);
            il.Emit(OpCodes.Newobj, CompilerGlobals.breakOutOfFinallyConstructor);
            il.Emit(OpCodes.Throw);
        }
        else if (this.compilerGlobals.InsideProtectedRegion)
            il.Emit(OpCodes.Leave, label);
        else
            il.Emit(OpCodes.Br, label);
    }

    internal override void TranslateToILInitializer(ILGenerator il)
    {
    }
}