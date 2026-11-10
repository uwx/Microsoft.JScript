// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.TypeExpression
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace Microsoft.JScript;

internal sealed class TypeExpression : AST
{
    internal AST expression;
    internal bool isArray;
    internal int rank;
    private bool recursive;
    private IReflect cachedIR;

    internal TypeExpression(AST expression)
        : base(expression.context)
    {
        this.expression = expression;
        this.isArray = false;
        this.rank = 0;
        this.recursive = false;
        this.cachedIR = (IReflect)null;
        if (!(expression is Lookup))
            return;
        object predefinedType = (object)Globals.TypeRefs.GetPredefinedType(expression.ToString());
        if (predefinedType == null)
            return;
        this.expression = (AST)new ConstantWrapper(predefinedType, expression.context);
    }

    internal override object Evaluate() => (object)this.ToIReflect();

    internal override IReflect InferType(JSField inference_target) => this.ToIReflect();

    internal bool IsCLSCompliant() => TypeExpression.TypeIsCLSCompliant(this.expression.Evaluate());

    internal override AST PartiallyEvaluate()
    {
        if (this.recursive)
        {
            if (this.expression is ConstantWrapper)
                return (AST)this;
            this.expression = (AST)new ConstantWrapper((object)Typeob.Object, this.context);
            return (AST)this;
        }

        if (this.expression is Member expression)
        {
            object asType = expression.EvaluateAsType();
            if (asType != null)
            {
                this.expression = (AST)new ConstantWrapper(asType, expression.context);
                return (AST)this;
            }
        }

        this.recursive = true;
        this.expression = this.expression.PartiallyEvaluate();
        this.recursive = false;
        if (this.expression is TypeExpression)
            return (AST)this;
        Type referenceContext;
        if (this.expression is ConstantWrapper)
        {
            object member = this.expression.Evaluate();
            if (member == null)
            {
                this.expression.context.HandleError(JSError.NeedType);
                this.expression = (AST)new ConstantWrapper((object)Typeob.Object, this.context);
                return (AST)this;
            }

            referenceContext = Globals.TypeRefs.ToReferenceContext(member.GetType());
            Binding.WarnIfObsolete((MemberInfo)(member as Type), this.expression.context);
        }
        else if (this.expression.OkToUseAsType())
        {
            referenceContext = Globals.TypeRefs.ToReferenceContext(this.expression.Evaluate().GetType());
        }
        else
        {
            this.expression.context.HandleError(JSError.NeedCompileTimeConstant);
            this.expression = (AST)new ConstantWrapper((object)Typeob.Object, this.expression.context);
            return (AST)this;
        }

        if (referenceContext == (Type)null || referenceContext != Typeob.ClassScope &&
            referenceContext != Typeob.TypedArray && !Typeob.Type.IsAssignableFrom(referenceContext))
        {
            this.expression.context.HandleError(JSError.NeedType);
            this.expression = (AST)new ConstantWrapper((object)Typeob.Object, this.expression.context);
        }

        return (AST)this;
    }

    internal IReflect ToIReflect()
    {
        if (!(this.expression is ConstantWrapper))
            this.PartiallyEvaluate();
        IReflect cachedIr = this.cachedIR;
        if (cachedIr != null)
            return cachedIr;
        object t = this.expression.Evaluate();
        IReflect elementType = t is ClassScope || t is TypedArray || this.context == null
            ? (IReflect)t
            : Convert.ToIReflect((Type)t, this.Engine);
        return this.isArray
            ? (this.cachedIR = (IReflect)new TypedArray(elementType, this.rank))
            : (this.cachedIR = elementType);
    }

    internal Type ToType()
    {
        if (!(this.expression is ConstantWrapper))
            this.PartiallyEvaluate();
        object ir = this.expression.Evaluate();
        Type elementType;
        switch (ir)
        {
            case ClassScope _:
                elementType = ((ClassScope)ir).GetTypeBuilderOrEnumBuilder();
                break;
            case TypedArray _:
                elementType = Convert.ToType((IReflect)ir);
                break;
            default:
                elementType = Globals.TypeRefs.ToReferenceContext((Type)ir);
                break;
        }

        return this.isArray ? Convert.ToType(TypedArray.ToRankString(this.rank), elementType) : elementType;
    }

    internal override void TranslateToIL(ILGenerator il, Type rtype)
    {
        this.expression.TranslateToIL(il, rtype);
    }

    internal override void TranslateToILInitializer(ILGenerator il)
    {
        this.expression.TranslateToILInitializer(il);
    }

    internal static bool TypeIsCLSCompliant(object type)
    {
        switch (type)
        {
            case ClassScope _:
                return ((ClassScope)type).IsCLSCompliant();
            case TypedArray _:
                object elementType = (object)((TypedArray)type).elementType;
                return !(elementType is TypedArray) &&
                       ((object)(elementType as Type) == null || !((Type)elementType).IsArray) &&
                       TypeExpression.TypeIsCLSCompliant(elementType);
            default:
                Type type1 = (Type)type;
                if (type1.IsPrimitive)
                    return type1 == Typeob.Boolean || type1 == Typeob.Byte || type1 == Typeob.Char ||
                           type1 == Typeob.Double || type1 == Typeob.Int16 || type1 == Typeob.Int32 ||
                           type1 == Typeob.Int64 || type1 == Typeob.Single;
                if (type1.IsArray)
                    return !type1.GetElementType().IsArray && TypeExpression.TypeIsCLSCompliant((object)type1);
                object[] customAttributes1 =
                    CustomAttribute.GetCustomAttributes((MemberInfo)type1, typeof(CLSCompliantAttribute), false);
                if (customAttributes1.Length != 0)
                    return ((CLSCompliantAttribute)customAttributes1[0]).IsCompliant;
                Module module = type1.Module;
                object[] customAttributes2 =
                    CustomAttribute.GetCustomAttributes(module, typeof(CLSCompliantAttribute), false);
                if (customAttributes2.Length != 0)
                    return ((CLSCompliantAttribute)customAttributes2[0]).IsCompliant;
                object[] customAttributes3 =
                    CustomAttribute.GetCustomAttributes(module.Assembly, typeof(CLSCompliantAttribute), false);
                return customAttributes3.Length != 0 && ((CLSCompliantAttribute)customAttributes3[0]).IsCompliant;
        }
    }
}