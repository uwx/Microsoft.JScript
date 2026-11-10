// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.Throw
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Throws an exception. This class belongs to the abstract syntax tree category.</summary>
public sealed class Throw : AST
{
    private AST operand;

    internal Throw(Context context, AST operand)
        : base(context)
    {
        this.operand = operand;
    }

    internal override object Evaluate()
    {
        if (this.operand == null)
        {
            ScriptObject scriptObject = this.Engine.ScriptObjectStackTop();
            while (scriptObject != null)
            {
                if (scriptObject is BlockScope blockScope && blockScope.catchHanderScope)
                    throw (Exception)blockScope.GetFields(BindingFlags.Static | BindingFlags.Public)[0]
                        .GetValue((object)null);
            }
        }

        throw Throw.JScriptThrow(this.operand.Evaluate());
    }

    internal override bool HasReturn() => true;

    /// <summary>Returns the specified object as an exception, converting if it has to.</summary>
    /// <returns>An exception that represents <paramref name="value" />.</returns>
    /// <param name="value">The exception to throw. </param>
    public static Exception JScriptThrow(object value)
    {
        switch (value)
        {
            case Exception _:
                return (Exception)value;
            case ErrorObject _:
                if (((ErrorObject)value).exception is Exception)
                    return (Exception)((ErrorObject)value).exception;
                break;
        }

        return (Exception)new JScriptException(value, (Context)null);
    }

    internal override AST PartiallyEvaluate()
    {
        if (this.operand == null)
        {
            BlockScope blockScope = (BlockScope)null;
            for (ScriptObject scriptObject = this.Engine.ScriptObjectStackTop();
                 scriptObject != null;
                 scriptObject = scriptObject.GetParent())
            {
                if (!(scriptObject is WithObject))
                {
                    blockScope = scriptObject as BlockScope;
                    if (blockScope == null || blockScope.catchHanderScope)
                        break;
                }
            }

            if (blockScope == null)
            {
                this.context.HandleError(JSError.BadThrow);
                this.operand = (AST)new ConstantWrapper((object)null, this.context);
            }
        }
        else
            this.operand = this.operand.PartiallyEvaluate();

        return (AST)this;
    }

    internal override void TranslateToIL(ILGenerator il, Type rtype)
    {
        this.context.EmitLineInfo(il);
        if (this.operand == null)
        {
            il.Emit(OpCodes.Rethrow);
        }
        else
        {
            IReflect reflect = this.operand.InferType((JSField)null);
            if ((object)(reflect as Type) != null && Typeob.Exception.IsAssignableFrom((Type)reflect))
            {
                this.operand.TranslateToIL(il, (Type)reflect);
            }
            else
            {
                this.operand.TranslateToIL(il, Typeob.Object);
                il.Emit(OpCodes.Call, CompilerGlobals.jScriptThrowMethod);
            }

            il.Emit(OpCodes.Throw);
        }
    }

    internal override void TranslateToILInitializer(ILGenerator il)
    {
        if (this.operand == null)
            return;
        this.operand.TranslateToILInitializer(il);
    }
}