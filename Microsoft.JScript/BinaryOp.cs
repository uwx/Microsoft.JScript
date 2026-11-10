// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.BinaryOp
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Represents the base class for classes that perform operations on two operands, such as <see cref="T:Microsoft.JScript.Plus" />, <see cref="T:Microsoft.JScript.Equality" />, and <see cref="T:Microsoft.JScript.Relational" />. This class belongs to the abstract syntax tree category.</summary>
public abstract class BinaryOp : AST
{
    /// <summary>Represents the first operand of the operation.</summary>
    protected AST operand1;

    /// <summary>Represents the second operand of the operation.</summary>
    protected AST operand2;

    /// <summary>Represents the operation that is requested. One of the <see cref="T:Microsoft.JScript.JSToken" /> enumeration values.</summary>
    protected JSToken operatorTok;

    /// <summary>Represents the data type of the first operand of the operation.</summary>
    protected Type type1;

    /// <summary>Represents the data type of the second operand of the operation.</summary>
    protected Type type2;

    /// <summary>Represents the operation that is actually performed.</summary>
    protected MethodInfo operatorMeth;

    internal BinaryOp(Context context, AST operand1, AST operand2)
        : this(context, operand1, operand2, JSToken.EndOfFile)
    {
    }

    internal BinaryOp(Context context, AST operand1, AST operand2, JSToken operatorTok)
        : base(context)
    {
        this.operand1 = operand1;
        this.operand2 = operand2;
        this.operatorTok = operatorTok;
        this.type1 = (Type)null;
        this.type2 = (Type)null;
        this.operatorMeth = (MethodInfo)null;
    }

    internal override void CheckIfOKToUseInSuperConstructorCall()
    {
        this.operand1.CheckIfOKToUseInSuperConstructorCall();
        this.operand2.CheckIfOKToUseInSuperConstructorCall();
    }

    /// <summary>Determines the best method to call based on the operation that is requested and the data types of the operands.</summary>
    /// <returns>The recommended method to call on the operands.</returns>
    /// <param name="ir1">The data type of the first operand.</param>
    /// <param name="ir2">The data type of the second operand.</param>
    protected MethodInfo GetOperator(IReflect ir1, IReflect ir2)
    {
        if (ir1 is ClassScope)
            ir1 = ((ClassScope)ir1).GetUnderlyingTypeIfEnum();
        if (ir2 is ClassScope)
            ir2 = ((ClassScope)ir2).GetUnderlyingTypeIfEnum();
        Type c1 = (object)(ir1 as Type) != null ? (Type)ir1 : Typeob.Object;
        Type c2 = (object)(ir2 as Type) != null ? (Type)ir2 : Typeob.Object;
        if (this.type1 == c1 && this.type2 == c2)
            return this.operatorMeth;
        this.type1 = c1;
        this.type2 = c2;
        this.operatorMeth = (MethodInfo)null;
        if (c1 == Typeob.String || Convert.IsPrimitiveNumericType(ir1) || Typeob.JSObject.IsAssignableFrom(c1))
            c1 = (Type)null;
        if (c2 == Typeob.String || Convert.IsPrimitiveNumericType(ir2) || Typeob.JSObject.IsAssignableFrom(c2))
            c2 = (Type)null;
        if (c1 == (Type)null && c2 == (Type)null)
            return (MethodInfo)null;
        string name = "op_NoSuchOp";
        switch (this.operatorTok)
        {
            case JSToken.FirstBinaryOp:
                name = "op_Addition";
                break;
            case JSToken.Minus:
                name = "op_Subtraction";
                break;
            case JSToken.BitwiseOr:
                name = "op_BitwiseOr";
                break;
            case JSToken.BitwiseXor:
                name = "op_ExclusiveOr";
                break;
            case JSToken.BitwiseAnd:
                name = "op_BitwiseAnd";
                break;
            case JSToken.Equal:
                name = "op_Equality";
                break;
            case JSToken.NotEqual:
                name = "op_Inequality";
                break;
            case JSToken.GreaterThan:
                name = "op_GreaterThan";
                break;
            case JSToken.LessThan:
                name = "op_LessThan";
                break;
            case JSToken.LessThanEqual:
                name = "op_LessThanOrEqual";
                break;
            case JSToken.GreaterThanEqual:
                name = "op_GreaterThanOrEqual";
                break;
            case JSToken.LeftShift:
                name = "op_LeftShift";
                break;
            case JSToken.RightShift:
                name = "op_RightShift";
                break;
            case JSToken.Multiply:
                name = "op_Multiply";
                break;
            case JSToken.Divide:
                name = "op_Division";
                break;
            case JSToken.Modulo:
                name = "op_Modulus";
                break;
        }

        Type[] types = new Type[2] { this.type1, this.type2 };
        if (c1 == c2)
        {
            MethodInfo method = c1.GetMethod(name, BindingFlags.Static | BindingFlags.Public, (Binder)JSBinder.ob,
                types, (ParameterModifier[])null);
            if (method != (MethodInfo)null &&
                (method.Attributes & MethodAttributes.SpecialName) != MethodAttributes.PrivateScope &&
                method.GetParameters().Length == 2)
                this.operatorMeth = method;
        }
        else
            this.operatorMeth = JSBinder.SelectOperator(
                c1 == (Type)null
                    ? (MethodInfo)null
                    : c1.GetMethod(name, BindingFlags.Static | BindingFlags.Public, (Binder)JSBinder.ob, types,
                        (ParameterModifier[])null),
                c2 == (Type)null
                    ? (MethodInfo)null
                    : c2.GetMethod(name, BindingFlags.Static | BindingFlags.Public, (Binder)JSBinder.ob, types,
                        (ParameterModifier[])null), this.type1, this.type2);

        if (this.operatorMeth != (MethodInfo)null)
            this.operatorMeth = (MethodInfo)new JSMethodInfo(this.operatorMeth);
        return this.operatorMeth;
    }

    internal override AST PartiallyEvaluate()
    {
        this.operand1 = this.operand1.PartiallyEvaluate();
        this.operand2 = this.operand2.PartiallyEvaluate();
        try
        {
            if (this.operand1 is ConstantWrapper)
            {
                if (this.operand2 is ConstantWrapper)
                    return (AST)new ConstantWrapper(this.Evaluate(), this.context);
                object obj = ((ConstantWrapper)this.operand1).value;
                if (obj is string)
                {
                    if (((string)obj).Length == 1)
                    {
                        if ((object)this.operand2.InferType((JSField)null) == (object)Typeob.Char)
                            ((ConstantWrapper)this.operand1).value = (object)((string)obj)[0];
                    }
                }
            }
            else if (this.operand2 is ConstantWrapper)
            {
                object obj = ((ConstantWrapper)this.operand2).value;
                if (obj is string)
                {
                    if (((string)obj).Length == 1)
                    {
                        if ((object)this.operand1.InferType((JSField)null) == (object)Typeob.Char)
                            ((ConstantWrapper)this.operand2).value = (object)((string)obj)[0];
                    }
                }
            }
        }
        catch (JScriptException ex)
        {
            this.context.HandleError((JSError)(ex.ErrorNumber & (int)ushort.MaxValue));
        }
        catch
        {
            this.context.HandleError(JSError.TypeMismatch);
        }

        return (AST)this;
    }

    internal override void TranslateToILInitializer(ILGenerator il)
    {
        this.operand1.TranslateToILInitializer(il);
        this.operand2.TranslateToILInitializer(il);
    }
}