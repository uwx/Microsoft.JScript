// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.ObjectLiteral
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Reflection.Emit;

#nullable disable
namespace Microsoft.JScript;

internal sealed class ObjectLiteral : AST
{
    internal AST[] keys;
    internal AST[] values;

    internal ObjectLiteral(Context context, ASTList propertyList)
        : base(context)
    {
        int count = propertyList.count;
        this.keys = new AST[count];
        this.values = new AST[count];
        for (int i = 0; i < count; ++i)
        {
            ASTList property = (ASTList)propertyList[i];
            this.keys[i] = property[0];
            this.values[i] = property[1];
        }
    }

    internal override void CheckIfOKToUseInSuperConstructorCall()
    {
        int index = 0;
        for (int length = this.values.Length; index < length; ++index)
            this.values[index].CheckIfOKToUseInSuperConstructorCall();
    }

    internal override object Evaluate()
    {
        JSObject jsObject = this.Engine.GetOriginalObjectConstructor().ConstructObject();
        int index = 0;
        for (int length = this.keys.Length; index < length; ++index)
            jsObject.SetMemberValue(this.keys[index].Evaluate().ToString(), this.values[index].Evaluate());
        return (object)jsObject;
    }

    internal override AST PartiallyEvaluate()
    {
        int length = this.keys.Length;
        for (int index = 0; index < length; ++index)
        {
            this.keys[index] = this.keys[index].PartiallyEvaluate();
            this.values[index] = this.values[index].PartiallyEvaluate();
        }

        return (AST)this;
    }

    internal override void TranslateToIL(ILGenerator il, Type rtype)
    {
        int length = this.keys.Length;
        this.EmitILToLoadEngine(il);
        il.Emit(OpCodes.Call, CompilerGlobals.getOriginalObjectConstructorMethod);
        il.Emit(OpCodes.Call, CompilerGlobals.constructObjectMethod);
        for (int index = 0; index < length; ++index)
        {
            il.Emit(OpCodes.Dup);
            this.keys[index].TranslateToIL(il, Typeob.String);
            this.values[index].TranslateToIL(il, Typeob.Object);
            il.Emit(OpCodes.Call, CompilerGlobals.setMemberValue2Method);
        }

        Convert.Emit((AST)this, il, Typeob.Object, rtype);
    }

    internal override void TranslateToILInitializer(ILGenerator il)
    {
        int index = 0;
        for (int length = this.keys.Length; index < length; ++index)
        {
            this.keys[index].TranslateToILInitializer(il);
            this.values[index].TranslateToILInitializer(il);
        }
    }
}