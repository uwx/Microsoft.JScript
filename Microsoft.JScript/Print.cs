// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.Print
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Reflection.Emit;

#nullable disable
namespace Microsoft.JScript;

internal sealed class Print : AST
{
    private ASTList operand;
    private Completion completion;

    internal Print(Context context, AST operand)
        : base(context)
    {
        this.operand = (ASTList)operand;
        this.completion = new Completion();
    }

    internal override object Evaluate()
    {
        object[] asArray = this.operand.EvaluateAsArray();
        for (int index = 0; index < asArray.Length - 1; ++index)
            ScriptStream.Out.Write(Convert.ToString(asArray[index]));
        if (asArray.Length != 0)
        {
            this.completion.value = (object)Convert.ToString(asArray[asArray.Length - 1]);
            ScriptStream.Out.WriteLine(this.completion.value);
        }
        else
        {
            ScriptStream.Out.WriteLine("");
            this.completion.value = (object)null;
        }

        return (object)this.completion;
    }

    internal override AST PartiallyEvaluate()
    {
        this.operand = (ASTList)this.operand.PartiallyEvaluate();
        return (AST)this;
    }

    internal override void TranslateToIL(ILGenerator il, Type rtype)
    {
        if (this.context.document.debugOn)
            il.Emit(OpCodes.Nop);
        ASTList operand = this.operand;
        int count = operand.count;
        for (int i = 0; i < count; ++i)
        {
            AST ast = operand[i];
            if ((object)ast.InferType((JSField)null) == (object)Typeob.String)
            {
                ast.TranslateToIL(il, Typeob.String);
            }
            else
            {
                ast.TranslateToIL(il, Typeob.Object);
                ConstantWrapper.TranslateToILInt(il, 1);
                il.Emit(OpCodes.Call, CompilerGlobals.toStringMethod);
            }

            if (i == count - 1)
                il.Emit(OpCodes.Call, CompilerGlobals.writeLineMethod);
            else
                il.Emit(OpCodes.Call, CompilerGlobals.writeMethod);
        }

        if (count == 0)
        {
            il.Emit(OpCodes.Ldstr, "");
            il.Emit(OpCodes.Call, CompilerGlobals.writeLineMethod);
        }

        if (!(rtype != Typeob.Void))
            return;
        il.Emit(OpCodes.Ldsfld, CompilerGlobals.undefinedField);
        Convert.Emit((AST)this, il, Typeob.Object, rtype);
    }

    internal override void TranslateToILInitializer(ILGenerator il)
    {
        ASTList operand = this.operand;
        for (int i = 0; i < operand.count; ++i)
            operand[i].TranslateToILInitializer(il);
    }
}