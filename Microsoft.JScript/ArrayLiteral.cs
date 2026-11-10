// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.ArrayLiteral
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using Microsoft.JScript.Vsa;
using System;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace Microsoft.JScript;

/// <summary>This class is used by the JScript parser to represent an array literal.</summary>
public sealed class ArrayLiteral : AST
{
    internal ASTList elements;

    /// <summary>Initializes a new instance of the <see cref="T:Microsoft.JScript.ArrayLiteral" /> class, specifying the context and elements.</summary>
    /// <param name="context">The current document context.</param>
    /// <param name="elements">The elements of the array.</param>
    public ArrayLiteral(Context context, ASTList elements)
        : base(context)
    {
        this.elements = elements;
    }

    internal bool AssignmentCompatible(IReflect lhir, bool reportError)
    {
        if ((object)lhir == (object)Typeob.Object || (object)lhir == (object)Typeob.Array || lhir is ArrayObject)
            return true;
        IReflect elementType;
        if ((object)lhir == (object)Typeob.Array)
            elementType = (IReflect)Typeob.Object;
        else if (lhir is TypedArray)
        {
            TypedArray typedArray = (TypedArray)lhir;
            if (typedArray.rank != 1)
            {
                this.context.HandleError(JSError.TypeMismatch, reportError);
                return false;
            }

            elementType = typedArray.elementType;
        }
        else
        {
            if ((object)(lhir as Type) == null || !((Type)lhir).IsArray)
                return false;
            Type type = (Type)lhir;
            if (type.GetArrayRank() != 1)
            {
                this.context.HandleError(JSError.TypeMismatch, reportError);
                return false;
            }

            elementType = (IReflect)type.GetElementType();
        }

        int i = 0;
        for (int count = this.elements.count; i < count; ++i)
        {
            if (!Binding.AssignmentCompatible(elementType, this.elements[i], this.elements[i].InferType((JSField)null),
                    reportError))
                return false;
        }

        return true;
    }

    internal override void CheckIfOKToUseInSuperConstructorCall()
    {
        int i = 0;
        for (int count = this.elements.count; i < count; ++i)
            this.elements[i].CheckIfOKToUseInSuperConstructorCall();
    }

    internal override object Evaluate()
    {
        if (VsaEngine.executeForJSEE)
            throw new JScriptException(JSError.NonSupportedInDebugger);
        int count = this.elements.count;
        object[] args = new object[count];
        for (int i = 0; i < count; ++i)
            args[i] = this.elements[i].Evaluate();
        return (object)this.Engine.GetOriginalArrayConstructor().ConstructArray(args);
    }

    internal bool IsOkToUseInCustomAttribute()
    {
        int count = this.elements.count;
        for (int i = 0; i < count; ++i)
        {
            object element = (object)this.elements[i];
            if (!(element is ConstantWrapper) || CustomAttribute.TypeOfArgument(((AST)element).Evaluate()) == null)
                return false;
        }

        return true;
    }

    internal override AST PartiallyEvaluate()
    {
        int count = this.elements.count;
        for (int i = 0; i < count; ++i)
            this.elements[i] = this.elements[i].PartiallyEvaluate();
        return (AST)this;
    }

    internal override IReflect InferType(JSField inference_target) => (IReflect)Typeob.ArrayObject;

    internal override void TranslateToIL(ILGenerator il, Type rtype)
    {
        if (rtype == Typeob.Array)
            this.TranslateToILArray(il, Typeob.Object);
        else if (rtype.IsArray && rtype.GetArrayRank() == 1)
        {
            this.TranslateToILArray(il, rtype.GetElementType());
        }
        else
        {
            int count = this.elements.count;
            MethodInfo meth;
            if (this.Engine.Globals.globalObject is LenientGlobalObject)
            {
                this.EmitILToLoadEngine(il);
                il.Emit(OpCodes.Call, CompilerGlobals.getOriginalArrayConstructorMethod);
                meth = CompilerGlobals.constructArrayMethod;
            }
            else
                meth = CompilerGlobals.fastConstructArrayLiteralMethod;

            ConstantWrapper.TranslateToILInt(il, count);
            il.Emit(OpCodes.Newarr, Typeob.Object);
            for (int i = 0; i < count; ++i)
            {
                il.Emit(OpCodes.Dup);
                ConstantWrapper.TranslateToILInt(il, i);
                this.elements[i].TranslateToIL(il, Typeob.Object);
                il.Emit(OpCodes.Stelem_Ref);
            }

            il.Emit(OpCodes.Call, meth);
            Convert.Emit((AST)this, il, Typeob.ArrayObject, rtype);
        }
    }

    private void TranslateToILArray(ILGenerator il, Type etype)
    {
        int count = this.elements.count;
        ConstantWrapper.TranslateToILInt(il, count);
        Type.GetTypeCode(etype);
        il.Emit(OpCodes.Newarr, etype);
        for (int i = 0; i < count; ++i)
        {
            il.Emit(OpCodes.Dup);
            ConstantWrapper.TranslateToILInt(il, i);
            if (etype.IsValueType && !etype.IsPrimitive)
                il.Emit(OpCodes.Ldelema, etype);
            this.elements[i].TranslateToIL(il, etype);
            Binding.TranslateToStelem(il, etype);
        }
    }

    internal override void TranslateToILInitializer(ILGenerator il)
    {
        int i = 0;
        for (int count = this.elements.count; i < count; ++i)
            this.elements[i].TranslateToILInitializer(il);
    }
}