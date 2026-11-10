// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.LenientArrayPrototype
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Stores objects that represent the static methods of the <see cref="T:Microsoft.JScript.ArrayPrototype" /> class. The <see cref="T:Microsoft.JScript.LenientArrayPrototype" /> class belongs to the built-in object model category.</summary>
public sealed class LenientArrayPrototype : ArrayPrototype
{
    /// <summary>Stores a reference to an <see cref="T:Microsoft.JScript.ArrayConstructor" /> object.</summary>
    public object constructor;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.ArrayPrototype.concat(System.Object,Microsoft.JScript.Vsa.VsaEngine,System.Object[])" /> method of the <see cref="T:Microsoft.JScript.ArrayPrototype" /> class.</summary>
    public object concat;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.ArrayPrototype.join(System.Object,System.Object)" /> method of the <see cref="T:Microsoft.JScript.ArrayPrototype" /> class.</summary>
    public object join;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.ArrayPrototype.pop(System.Object)" /> method of the <see cref="T:Microsoft.JScript.ArrayPrototype" /> class.</summary>
    public object pop;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.ArrayPrototype.push(System.Object,System.Object[])" /> method of the <see cref="T:Microsoft.JScript.ArrayPrototype" /> class.</summary>
    public object push;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.ArrayPrototype.reverse(System.Object)" /> method of the <see cref="T:Microsoft.JScript.ArrayPrototype" /> class.</summary>
    public object reverse;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.ArrayPrototype.shift(System.Object)" /> method of the <see cref="T:Microsoft.JScript.ArrayPrototype" /> class.</summary>
    public object shift;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.ArrayPrototype.slice(System.Object,Microsoft.JScript.Vsa.VsaEngine,System.Double,System.Object)" /> method of the <see cref="T:Microsoft.JScript.ArrayPrototype" /> class.</summary>
    public object slice;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.ArrayPrototype.sort(System.Object,System.Object)" /> method of the <see cref="T:Microsoft.JScript.ArrayPrototype" /> class.</summary>
    public object sort;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.ArrayPrototype.splice(System.Object,Microsoft.JScript.Vsa.VsaEngine,System.Double,System.Double,System.Object[])" /> method of the <see cref="T:Microsoft.JScript.ArrayPrototype" /> class.</summary>
    public object splice;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.ArrayPrototype.unshift(System.Object,System.Object[])" /> method of the <see cref="T:Microsoft.JScript.ArrayPrototype" /> class.</summary>
    public object unshift;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.ArrayPrototype.toLocaleString(System.Object)" /> method of the <see cref="T:Microsoft.JScript.ArrayPrototype" /> class.</summary>
    public object toLocaleString;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.ArrayPrototype.toString(System.Object)" /> method of the <see cref="T:Microsoft.JScript.ArrayPrototype" /> class.</summary>
    public object toString;

    internal LenientArrayPrototype(FunctionPrototype funcprot, ObjectPrototype parent)
        : base(parent)
    {
        this.noExpando = false;
        Type type = typeof(ArrayPrototype);
        this.concat = (object)new BuiltinFunction(nameof(concat), (object)this, type.GetMethod(nameof(concat)),
            (ScriptFunction)funcprot);
        this.join = (object)new BuiltinFunction(nameof(join), (object)this, type.GetMethod(nameof(join)),
            (ScriptFunction)funcprot);
        this.pop = (object)new BuiltinFunction(nameof(pop), (object)this, type.GetMethod(nameof(pop)),
            (ScriptFunction)funcprot);
        this.push = (object)new BuiltinFunction(nameof(push), (object)this, type.GetMethod(nameof(push)),
            (ScriptFunction)funcprot);
        this.reverse = (object)new BuiltinFunction(nameof(reverse), (object)this, type.GetMethod(nameof(reverse)),
            (ScriptFunction)funcprot);
        this.shift = (object)new BuiltinFunction(nameof(shift), (object)this, type.GetMethod(nameof(shift)),
            (ScriptFunction)funcprot);
        this.slice = (object)new BuiltinFunction(nameof(slice), (object)this, type.GetMethod(nameof(slice)),
            (ScriptFunction)funcprot);
        this.sort = (object)new BuiltinFunction(nameof(sort), (object)this, type.GetMethod(nameof(sort)),
            (ScriptFunction)funcprot);
        this.splice = (object)new BuiltinFunction(nameof(splice), (object)this, type.GetMethod(nameof(splice)),
            (ScriptFunction)funcprot);
        this.unshift = (object)new BuiltinFunction(nameof(unshift), (object)this, type.GetMethod(nameof(unshift)),
            (ScriptFunction)funcprot);
        this.toLocaleString = (object)new BuiltinFunction(nameof(toLocaleString), (object)this,
            type.GetMethod(nameof(toLocaleString)), (ScriptFunction)funcprot);
        this.toString = (object)new BuiltinFunction(nameof(toString), (object)this, type.GetMethod(nameof(toString)),
            (ScriptFunction)funcprot);
    }
}