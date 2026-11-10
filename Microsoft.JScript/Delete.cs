// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.Delete
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace Microsoft.JScript;

internal sealed class Delete : UnaryOp
{
    internal Delete(Context context, AST operand)
        : base(context, operand)
    {
    }

    internal override void CheckIfOKToUseInSuperConstructorCall()
    {
        this.context.HandleError(JSError.NotAllowedInSuperConstructorCall);
    }

    internal override object Evaluate()
    {
        try
        {
            return (object)this.operand.Delete();
        }
        catch (JScriptException ex)
        {
            return (object)true;
        }
    }

    internal override IReflect InferType(JSField inference_target) => (IReflect)Typeob.Boolean;

    internal override AST PartiallyEvaluate()
    {
        this.operand = this.operand.PartiallyEvaluate();
        if (this.operand is Binding)
            ((Binding)this.operand).CheckIfDeletable();
        else if (this.operand is Call)
            ((Call)this.operand).MakeDeletable();
        else
            this.operand.context.HandleError(JSError.NotDeletable);
        return (AST)this;
    }

    internal override void TranslateToIL(ILGenerator il, Type rtype)
    {
        this.operand.TranslateToILDelete(il, rtype);
    }
}