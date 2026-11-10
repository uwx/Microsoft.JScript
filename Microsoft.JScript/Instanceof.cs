// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.Instanceof
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Determines whether an object can be coerced to the type of another object. This class belongs to the abstract syntax tree category.</summary>
public sealed class Instanceof : BinaryOp
{
    internal Instanceof(Context context, AST operand1, AST operand2)
        : base(context, operand1, operand2)
    {
    }

    internal override object Evaluate()
    {
        object v1 = this.operand1.Evaluate();
        object v2 = this.operand2.Evaluate();
        try
        {
            return (object)Instanceof.JScriptInstanceof(v1, v2);
        }
        catch (JScriptException ex)
        {
            if (ex.context == null)
                ex.context = this.operand2.context;
            throw ex;
        }
    }

    internal override IReflect InferType(JSField inference_target) => (IReflect)Typeob.Boolean;

    /// <summary>Determines whether the first specified object can be coerced to the type of the second specified object.</summary>
    /// <returns>true if <paramref name="v1" /> can be coerced to the type of object v2; otherwise, false.</returns>
    /// <param name="v1">The object to coerce to the type of object <paramref name="v2" />.</param>
    /// <param name="v2">The object that has the desired type.</param>
    public static bool JScriptInstanceof(object v1, object v2)
    {
        if (v2 is ClassScope)
            return ((ClassScope)v2).HasInstance(v1);
        if (v2 is ScriptFunction)
            return ((ScriptFunction)v2).HasInstance(v1);
        if (v1 == null)
            return false;
        if ((object)(v2 as Type) != null)
        {
            Type type = v1.GetType();
            if (!(v1 is IConvertible))
                return ((Type)v2).IsAssignableFrom(type);
            try
            {
                Convert.CoerceT(v1, (Type)v2);
                return true;
            }
            catch (JScriptException ex)
            {
                return false;
            }
        }
        else
            return v2 is IDebugType ? ((IDebugType)v2).HasInstance(v1) : throw new JScriptException(JSError.NeedType);
    }

    internal override void TranslateToIL(ILGenerator il, Type rtype)
    {
        this.operand1.TranslateToIL(il, Typeob.Object);
        object cls = (object)null;
        if (this.operand2 is ConstantWrapper && (object)((cls = this.operand2.Evaluate()) as Type) != null &&
            !((Type)cls).IsValueType)
        {
            il.Emit(OpCodes.Isinst, (Type)cls);
            il.Emit(OpCodes.Ldnull);
            il.Emit(OpCodes.Cgt_Un);
        }
        else if (cls is ClassScope)
        {
            il.Emit(OpCodes.Isinst, ((ClassScope)cls).GetTypeBuilderOrEnumBuilder());
            il.Emit(OpCodes.Ldnull);
            il.Emit(OpCodes.Cgt_Un);
        }
        else
        {
            this.operand2.TranslateToIL(il, Typeob.Object);
            il.Emit(OpCodes.Call, CompilerGlobals.jScriptInstanceofMethod);
        }

        Convert.Emit((AST)this, il, Typeob.Boolean, rtype);
    }
}