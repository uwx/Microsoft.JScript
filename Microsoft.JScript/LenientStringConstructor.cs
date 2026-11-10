// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.LenientStringConstructor
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

#nullable disable
namespace Microsoft.JScript;

/// <summary>Stores objects that represent the static methods of the <see cref="T:Microsoft.JScript.StringConstructor" /> class. The <see cref="T:Microsoft.JScript.LenientStringConstructor" /> class belongs to the built-in object model category.</summary>
public class LenientStringConstructor : StringConstructor
{
    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.StringConstructor.fromCharCode(System.Object[])" /> method of the <see cref="T:Microsoft.JScript.StringConstructor" /> class.</summary>
    public object fromCharCode;

    internal LenientStringConstructor(
        LenientFunctionPrototype parent,
        LenientStringPrototype prototypeProp)
        : base((FunctionPrototype)parent, prototypeProp)
    {
        this.noExpando = false;
        this.fromCharCode = (object)new BuiltinFunction(nameof(fromCharCode), (object)this,
            typeof(StringConstructor).GetMethod(nameof(fromCharCode)), (ScriptFunction)parent);
    }
}