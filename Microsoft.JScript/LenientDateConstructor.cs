// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.LenientDateConstructor
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Stores objects that represent the static methods of the <see cref="T:Microsoft.JScript.DateConstructor" /> class. The <see cref="T:Microsoft.JScript.LenientDateConstructor" /> class belongs to the built-in object model category.</summary>
public sealed class LenientDateConstructor : DateConstructor
{
    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.DateConstructor.parse(System.String)" /> method of the <see cref="T:Microsoft.JScript.DateConstructor" /> class.</summary>
    public object parse;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.DateConstructor.UTC(System.Object,System.Object,System.Object,System.Object,System.Object,System.Object,System.Object)" /> method of the <see cref="T:Microsoft.JScript.DateConstructor" /> class.</summary>
    public object UTC;

    internal LenientDateConstructor(
        LenientFunctionPrototype parent,
        LenientDatePrototype prototypeProp)
        : base(parent, prototypeProp)
    {
        this.noExpando = false;
        Type type = typeof(DateConstructor);
        this.parse = (object)new BuiltinFunction(nameof(parse), (object)this, type.GetMethod(nameof(parse)),
            (ScriptFunction)parent);
        this.UTC = (object)new BuiltinFunction(nameof(UTC), (object)this, type.GetMethod(nameof(UTC)),
            (ScriptFunction)parent);
    }
}