// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.ASTList
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Reflection.Emit;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Maintains a list of <see cref="T:Microsoft.JScript.AST" /> elements. This class belongs to the abstract syntax tree category.</summary>
public sealed class ASTList : AST
{
    internal int count;
    private AST[] list;
    private object[] array;

    internal ASTList(Context context)
        : base(context)
    {
        this.count = 0;
        this.list = new AST[16 /*0x10*/];
        this.array = (object[])null;
    }

    internal ASTList Append(AST elem)
    {
        int index = this.count++;
        if (this.list.Length == index)
            this.Grow();
        this.list[index] = elem;
        this.context.UpdateWith(elem.context);
        return this;
    }

    internal override object Evaluate() => (object)this.EvaluateAsArray();

    internal object[] EvaluateAsArray()
    {
        int count = this.count;
        object[] asArray = this.array;
        if (asArray == null)
            this.array = asArray = new object[count];
        AST[] list = this.list;
        for (int index = 0; index < count; ++index)
            asArray[index] = list[index].Evaluate();
        return asArray;
    }

    private void Grow()
    {
        AST[] list = this.list;
        int length = list.Length;
        AST[] astArray = this.list = new AST[length + 16 /*0x10*/];
        for (int index = 0; index < length; ++index)
            astArray[index] = list[index];
    }

    internal override AST PartiallyEvaluate()
    {
        AST[] list = this.list;
        int index = 0;
        for (int count = this.count; index < count; ++index)
            list[index] = list[index].PartiallyEvaluate();
        return (AST)this;
    }

    internal AST this[int i]
    {
        get => this.list[i];
        set => this.list[i] = value;
    }

    internal override void TranslateToIL(ILGenerator il, Type rtype)
    {
        Type elementType = rtype.GetElementType();
        int count = this.count;
        ConstantWrapper.TranslateToILInt(il, count);
        il.Emit(OpCodes.Newarr, elementType);
        bool flag = elementType.IsValueType && !elementType.IsPrimitive;
        AST[] list = this.list;
        for (int i = 0; i < count; ++i)
        {
            il.Emit(OpCodes.Dup);
            ConstantWrapper.TranslateToILInt(il, i);
            list[i].TranslateToIL(il, elementType);
            if (flag)
                il.Emit(OpCodes.Ldelema, elementType);
            Binding.TranslateToStelem(il, elementType);
        }
    }

    internal override void TranslateToILInitializer(ILGenerator il)
    {
        AST[] list = this.list;
        int index = 0;
        for (int count = this.count; index < count; ++index)
            list[index].TranslateToILInitializer(il);
    }
}