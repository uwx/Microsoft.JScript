// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.LenientVBArrayPrototype
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Stores objects that represent the static methods of the <see cref="T:Microsoft.JScript.VBArrayPrototype" /> class. The <see cref="T:Microsoft.JScript.LenientVBArrayPrototype" /> class belongs to the built-in object model category.</summary>
public sealed class LenientVBArrayPrototype : VBArrayPrototype
{
    /// <summary>Stores a reference to a <see cref="T:Microsoft.JScript.VBArrayConstructor" /> object.</summary>
    public object constructor;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.VBArrayPrototype.dimensions(System.Object)" /> method of the <see cref="T:Microsoft.JScript.VBArrayPrototype" /> class.</summary>
    public object dimensions;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.VBArrayPrototype.getItem(System.Object,System.Object[])" /> method of the <see cref="T:Microsoft.JScript.VBArrayPrototype" /> class.</summary>
    public object getItem;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.VBArrayPrototype.lbound(System.Object,System.Object)" /> method of the <see cref="T:Microsoft.JScript.VBArrayPrototype" /> class.</summary>
    public object lbound;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.VBArrayPrototype.toArray(System.Object,Microsoft.JScript.Vsa.VsaEngine)" /> method of the <see cref="T:Microsoft.JScript.VBArrayPrototype" /> class.</summary>
    public object toArray;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.VBArrayPrototype.ubound(System.Object,System.Object)" /> method of the <see cref="T:Microsoft.JScript.VBArrayPrototype" /> class.</summary>
    public object ubound;

    internal LenientVBArrayPrototype(LenientFunctionPrototype funcprot, LenientObjectPrototype parent)
        : base((FunctionPrototype)funcprot, (ObjectPrototype)parent)
    {
        this.noExpando = false;
        Type type = typeof(VBArrayPrototype);
        this.dimensions = (object)new BuiltinFunction(nameof(dimensions), (object)this,
            type.GetMethod(nameof(dimensions)), (ScriptFunction)funcprot);
        this.getItem = (object)new BuiltinFunction(nameof(getItem), (object)this, type.GetMethod(nameof(getItem)),
            (ScriptFunction)funcprot);
        this.lbound = (object)new BuiltinFunction(nameof(lbound), (object)this, type.GetMethod(nameof(lbound)),
            (ScriptFunction)funcprot);
        this.toArray = (object)new BuiltinFunction(nameof(toArray), (object)this, type.GetMethod(nameof(toArray)),
            (ScriptFunction)funcprot);
        this.ubound = (object)new BuiltinFunction(nameof(ubound), (object)this, type.GetMethod(nameof(ubound)),
            (ScriptFunction)funcprot);
    }
}