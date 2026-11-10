// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.LenientNumberPrototype
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Stores objects that represent the static methods of the <see cref="T:Microsoft.JScript.NumberPrototype" /> class. The <see cref="T:Microsoft.JScript.LenientNumberPrototype" /> class belongs to the built-in object model category.</summary>
public sealed class LenientNumberPrototype : NumberPrototype
{
    /// <summary>Stores a reference to a <see cref="T:Microsoft.JScript.NumberConstructor" /> object.</summary>
    public object constructor;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.NumberPrototype.toExponential(System.Object,System.Object)" /> method of the <see cref="T:Microsoft.JScript.NumberPrototype" /> class.</summary>
    public object toExponential;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.NumberPrototype.toFixed(System.Object,System.Double)" /> method of the <see cref="T:Microsoft.JScript.NumberPrototype" /> class.</summary>
    public object toFixed;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.NumberPrototype.toLocaleString(System.Object)" /> method of the <see cref="T:Microsoft.JScript.NumberPrototype" /> class.</summary>
    public object toLocaleString;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.NumberPrototype.toPrecision(System.Object,System.Object)" /> method of the <see cref="T:Microsoft.JScript.NumberPrototype" /> class.</summary>
    public object toPrecision;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.NumberPrototype.toString(System.Object,System.Object)" /> method of the <see cref="T:Microsoft.JScript.NumberPrototype" /> class.</summary>
    public object toString;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.NumberPrototype.valueOf(System.Object)" /> method of the <see cref="T:Microsoft.JScript.NumberPrototype" /> class.</summary>
    public object valueOf;

    internal LenientNumberPrototype(LenientFunctionPrototype funcprot, LenientObjectPrototype parent)
        : base((ObjectPrototype)parent)
    {
        this.noExpando = false;
        Type type = typeof(NumberPrototype);
        this.toExponential = (object)new BuiltinFunction(nameof(toExponential), (object)this,
            type.GetMethod(nameof(toExponential)), (ScriptFunction)funcprot);
        this.toFixed = (object)new BuiltinFunction(nameof(toFixed), (object)this, type.GetMethod(nameof(toFixed)),
            (ScriptFunction)funcprot);
        this.toLocaleString = (object)new BuiltinFunction(nameof(toLocaleString), (object)this,
            type.GetMethod(nameof(toLocaleString)), (ScriptFunction)funcprot);
        this.toPrecision = (object)new BuiltinFunction(nameof(toPrecision), (object)this,
            type.GetMethod(nameof(toPrecision)), (ScriptFunction)funcprot);
        this.toString = (object)new BuiltinFunction(nameof(toString), (object)this, type.GetMethod(nameof(toString)),
            (ScriptFunction)funcprot);
        this.valueOf = (object)new BuiltinFunction(nameof(valueOf), (object)this, type.GetMethod(nameof(valueOf)),
            (ScriptFunction)funcprot);
    }
}