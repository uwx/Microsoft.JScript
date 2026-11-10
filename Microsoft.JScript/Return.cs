// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.Return
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace Microsoft.JScript;

internal sealed class Return : AST
{
    private Completion completion;
    private AST operand;
    private FunctionScope enclosingFunctionScope;
    private bool leavesFinally;

    internal Return(Context context, AST operand, bool leavesFinally)
        : base(context)
    {
        this.completion = new Completion();
        this.completion.Return = true;
        this.operand = operand;
        ScriptObject scriptObject = this.Globals.ScopeStack.Peek();
        while (!(scriptObject is FunctionScope))
        {
            scriptObject = scriptObject.GetParent();
            if (scriptObject == null)
            {
                this.context.HandleError(JSError.BadReturn);
                scriptObject = (ScriptObject)new FunctionScope((ScriptObject)null);
            }
        }

        this.enclosingFunctionScope = (FunctionScope)scriptObject;
        if (this.operand != null && (FieldInfo)this.enclosingFunctionScope.returnVar == (FieldInfo)null)
            this.enclosingFunctionScope.AddReturnValueField();
        this.leavesFinally = leavesFinally;
    }

    internal override object Evaluate()
    {
        if (this.operand != null)
            this.completion.value = this.operand.Evaluate();
        return (object)this.completion;
    }

    internal override bool HasReturn() => true;

    internal override AST PartiallyEvaluate()
    {
        if (this.leavesFinally)
            this.context.HandleError(JSError.BadWayToLeaveFinally);
        if (this.operand != null)
        {
            this.operand = this.operand.PartiallyEvaluate();
            if ((FieldInfo)this.enclosingFunctionScope.returnVar != (FieldInfo)null)
            {
                if (this.enclosingFunctionScope.returnVar.type == null)
                    this.enclosingFunctionScope.returnVar.SetInferredType(
                        this.operand.InferType((JSField)this.enclosingFunctionScope.returnVar), this.operand);
                else
                    Binding.AssignmentCompatible(this.enclosingFunctionScope.returnVar.type.ToIReflect(), this.operand,
                        this.operand.InferType((JSField)null), true);
            }
            else
            {
                this.context.HandleError(JSError.CannotReturnValueFromVoidFunction);
                this.operand = (AST)null;
            }
        }
        else if ((FieldInfo)this.enclosingFunctionScope.returnVar != (FieldInfo)null)
            this.enclosingFunctionScope.returnVar.SetInferredType((IReflect)Typeob.Object, (AST)null);

        return (AST)this;
    }

    internal override void TranslateToIL(ILGenerator il, Type rtype)
    {
        this.context.EmitLineInfo(il);
        if (this.operand != null)
            this.operand.TranslateToIL(il, this.enclosingFunctionScope.returnVar.FieldType);
        else if ((FieldInfo)this.enclosingFunctionScope.returnVar != (FieldInfo)null)
        {
            il.Emit(OpCodes.Ldsfld, CompilerGlobals.undefinedField);
            Convert.Emit((AST)this, il, Typeob.Object, this.enclosingFunctionScope.returnVar.FieldType);
        }

        if ((FieldInfo)this.enclosingFunctionScope.returnVar != (FieldInfo)null)
            il.Emit(OpCodes.Stloc, (LocalBuilder)this.enclosingFunctionScope.returnVar.GetMetaData());
        if (this.leavesFinally)
        {
            il.Emit(OpCodes.Newobj, CompilerGlobals.returnOutOfFinallyConstructor);
            il.Emit(OpCodes.Throw);
        }
        else if (this.compilerGlobals.InsideProtectedRegion)
            il.Emit(OpCodes.Leave, this.enclosingFunctionScope.owner.returnLabel);
        else
            il.Emit(OpCodes.Br, this.enclosingFunctionScope.owner.returnLabel);
    }

    internal override void TranslateToILInitializer(ILGenerator il)
    {
        if (this.operand == null)
            return;
        this.operand.TranslateToILInitializer(il);
    }
}