// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.AddressOf
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace Microsoft.JScript;

internal sealed class AddressOf : UnaryOp
{
    internal AddressOf(Context context, AST operand)
        : base(context, operand)
    {
    }

    internal override object Evaluate() => this.operand.Evaluate();

    internal FieldInfo GetField()
    {
        if (!(this.operand is Binding))
            return (FieldInfo)null;
        MemberInfo member = ((Binding)this.operand).member;
        return (object)(member as FieldInfo) != null ? (FieldInfo)member : (FieldInfo)null;
    }

    internal override IReflect InferType(JSField inference_target)
    {
        return this.operand.InferType(inference_target);
    }

    internal override AST PartiallyEvaluate()
    {
        this.operand = this.operand.PartiallyEvaluate();
        if (!(this.operand is Binding) || !((Binding)this.operand).RefersToMemoryLocation())
            this.context.HandleError(JSError.DoesNotHaveAnAddress);
        return (AST)this;
    }

    internal override void TranslateToIL(ILGenerator il, Type rtype)
    {
        this.operand.TranslateToIL(il, rtype);
    }

    internal override void TranslateToILPreSet(ILGenerator il)
    {
        this.operand.TranslateToILPreSet(il);
    }

    internal override object TranslateToILReference(ILGenerator il, Type rtype)
    {
        return this.operand.TranslateToILReference(il, rtype);
    }

    internal override void TranslateToILSet(ILGenerator il, AST rhvalue)
    {
        this.operand.TranslateToILSet(il, rhvalue);
    }
}