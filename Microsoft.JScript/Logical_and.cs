// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.Logical_and
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace Microsoft.JScript;

internal sealed class Logical_and : BinaryOp
{
    internal Logical_and(Context context, AST operand1, AST operand2)
        : base(context, operand1, operand2)
    {
    }

    internal override object Evaluate()
    {
        object obj1 = this.operand1.Evaluate();
        MethodInfo method1 = (MethodInfo)null;
        Type type1 = (Type)null;
        switch (obj1)
        {
            case null:
            case IConvertible _:
                if (method1 == (MethodInfo)null)
                    return !Convert.ToBoolean(obj1) ? obj1 : this.operand2.Evaluate();
                if ((bool)new JSMethodInfo(method1).Invoke((object)null, BindingFlags.SuppressChangeType, (Binder)null,
                        new object[1]
                        {
                            obj1
                        }, (CultureInfo)null))
                    return obj1;
                object obj2 = this.operand2.Evaluate();
                switch (obj2)
                {
                    case null:
                    case IConvertible _:
                        return obj2;
                    default:
                        Type type2 = obj2.GetType();
                        if (type1 == type2)
                        {
                            MethodInfo method2 = type1.GetMethod("op_BitwiseAnd",
                                BindingFlags.Static | BindingFlags.Public | BindingFlags.ExactBinding, (Binder)null,
                                new Type[2]
                                {
                                    type1,
                                    type1
                                }, (ParameterModifier[])null);
                            if (method2 != (MethodInfo)null && (method2.Attributes & MethodAttributes.SpecialName) !=
                                MethodAttributes.PrivateScope)
                                return new JSMethodInfo(method2).Invoke((object)null, BindingFlags.SuppressChangeType,
                                    (Binder)null, new object[2]
                                    {
                                        obj1,
                                        obj2
                                    }, (CultureInfo)null);
                            goto case null;
                        }

                        goto case null;
                }
            default:
                type1 = obj1.GetType();
                method1 = type1.GetMethod("op_False",
                    BindingFlags.Static | BindingFlags.Public | BindingFlags.ExactBinding, (Binder)null, new Type[1]
                    {
                        type1
                    }, (ParameterModifier[])null);
                if (method1 == (MethodInfo)null ||
                    (method1.Attributes & MethodAttributes.SpecialName) == MethodAttributes.PrivateScope ||
                    method1.ReturnType != Typeob.Boolean)
                {
                    method1 = (MethodInfo)null;
                    goto case null;
                }

                goto case null;
        }
    }

    internal override IReflect InferType(JSField inference_target)
    {
        IReflect reflect1 = this.operand1.InferType(inference_target);
        IReflect reflect2 = this.operand2.InferType(inference_target);
        return reflect1 == reflect2 ? reflect1 : (IReflect)Typeob.Object;
    }

    internal override void TranslateToConditionalBranch(
        ILGenerator il,
        bool branchIfTrue,
        Label label,
        bool shortForm)
    {
        // Short-form branches corrupt the IL emitted by PersistedAssemblyBuilder on
        // .NET 10.0.x (ControlFlowBuilder desyncs on an operand at a blob boundary).
        shortForm = false;
        Label label1 = il.DefineLabel();
        if (branchIfTrue)
        {
            this.operand1.TranslateToConditionalBranch(il, false, label1, shortForm);
            this.operand2.TranslateToConditionalBranch(il, true, label, shortForm);
            il.MarkLabel(label1);
        }
        else
        {
            this.operand1.TranslateToConditionalBranch(il, false, label, shortForm);
            this.operand2.TranslateToConditionalBranch(il, false, label, shortForm);
        }
    }

    internal override void TranslateToIL(ILGenerator il, Type rtype)
    {
        Type type1 = Convert.ToType(this.operand1.InferType((JSField)null));
        Type type2 = Convert.ToType(this.operand2.InferType((JSField)null));
        if (type1 != type2)
            type1 = Typeob.Object;
        MethodInfo meth1 = type1.GetMethod("op_False",
            BindingFlags.Static | BindingFlags.Public | BindingFlags.ExactBinding, (Binder)null, new Type[1]
            {
                type1
            }, (ParameterModifier[])null);
        if (meth1 == (MethodInfo)null ||
            (meth1.Attributes & MethodAttributes.SpecialName) == MethodAttributes.PrivateScope ||
            meth1.ReturnType != Typeob.Boolean)
            meth1 = (MethodInfo)null;
        MethodInfo meth2 = (MethodInfo)null;
        if (meth1 != (MethodInfo)null)
            meth2 = type1.GetMethod("op_BitwiseAnd",
                BindingFlags.Static | BindingFlags.Public | BindingFlags.ExactBinding, (Binder)null, new Type[2]
                {
                    type1,
                    type1
                }, (ParameterModifier[])null);
        if (meth2 == (MethodInfo)null ||
            (meth2.Attributes & MethodAttributes.SpecialName) == MethodAttributes.PrivateScope)
            meth1 = (MethodInfo)null;
        Label label = il.DefineLabel();
        this.operand1.TranslateToIL(il, type1);
        il.Emit(OpCodes.Dup);
        if (meth1 != (MethodInfo)null)
        {
            if (type1.IsValueType)
                Convert.EmitLdloca(il, type1);
            il.Emit(OpCodes.Call, meth1);
            il.Emit(OpCodes.Brtrue, label);
            this.operand2.TranslateToIL(il, type1);
            il.Emit(OpCodes.Call, meth2);
            il.MarkLabel(label);
            Convert.Emit((AST)this, il, meth2.ReturnType, rtype);
        }
        else
        {
            Convert.Emit((AST)this, il, type1, Typeob.Boolean, true);
            il.Emit(OpCodes.Brfalse, label);
            il.Emit(OpCodes.Pop);
            this.operand2.TranslateToIL(il, type1);
            il.MarkLabel(label);
            Convert.Emit((AST)this, il, type1, rtype);
        }
    }
}