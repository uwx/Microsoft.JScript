// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.LenientFunctionPrototype
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Stores objects that represent the static methods of the <see cref="T:Microsoft.JScript.FunctionPrototype" /> class. The <see cref="T:Microsoft.JScript.LenientFunctionPrototype" /> class belongs to the built-in object model category.</summary>
public sealed class LenientFunctionPrototype : FunctionPrototype
{
    /// <summary>Stores a reference to a <see cref="T:Microsoft.JScript.FunctionConstructor" /> object.</summary>
    public object constructor;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.FunctionPrototype.apply(System.Object,System.Object,System.Object)" /> method of the <see cref="T:Microsoft.JScript.FunctionPrototype" /> class.</summary>
    public object apply;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.FunctionPrototype.call(System.Object,System.Object,System.Object[])" /> method of the <see cref="T:Microsoft.JScript.FunctionPrototype" /> class.</summary>
    public object call;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.FunctionPrototype.toString(System.Object)" /> method of the <see cref="T:Microsoft.JScript.FunctionPrototype" /> class.</summary>
    public object toString;

    internal LenientFunctionPrototype(ScriptObject parent)
        : base(parent)
    {
        this.noExpando = false;
        Type type = typeof(FunctionPrototype);
        this.apply = (object)new BuiltinFunction(nameof(apply), (object)this, type.GetMethod(nameof(apply)),
            (ScriptFunction)this);
        this.call = (object)new BuiltinFunction(nameof(call), (object)this, type.GetMethod(nameof(call)),
            (ScriptFunction)this);
        this.toString = (object)new BuiltinFunction(nameof(toString), (object)this, type.GetMethod(nameof(toString)),
            (ScriptFunction)this);
    }
}