// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.EnumeratorConstructor
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System.Collections;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Represents a prototype-based object that is defined by a constructor function. This class belongs to the built-in object model category.</summary>
public sealed class EnumeratorConstructor : ScriptFunction
{
    internal static readonly EnumeratorConstructor ob = new EnumeratorConstructor();
    private EnumeratorPrototype originalPrototype;

    internal EnumeratorConstructor()
        : base((ScriptObject)FunctionPrototype.ob, "Enumerator", 1)
    {
        this.originalPrototype = EnumeratorPrototype.ob;
        EnumeratorPrototype._constructor = this;
        this.proto = (object)EnumeratorPrototype.ob;
    }

    internal EnumeratorConstructor(
        LenientFunctionPrototype parent,
        LenientEnumeratorPrototype prototypeProp)
        : base((ScriptObject)parent, "Enumerator", 1)
    {
        this.originalPrototype = (EnumeratorPrototype)prototypeProp;
        prototypeProp.constructor = (object)this;
        this.proto = (object)prototypeProp;
        this.noExpando = false;
    }

    internal override object Call(object[] args, object thisob) => (object)null;

    internal override object Construct(object[] args) => (object)this.CreateInstance(args);

    /// <summary>Creates an instance of the prototype-based object, passing in the specified arguments.</summary>
    /// <returns>The instance of the object.</returns>
    /// <param name="args">The arguments to pass to the constructor.</param>
    [JSFunction(JSFunctionAttributeEnum.HasVarArgs)]
    public EnumeratorObject CreateInstance(params object[] args)
    {
        if (args.Length == 0)
            return new EnumeratorObject(this.originalPrototype, (IEnumerable)null);
        object collection = args[0];
        return collection is IEnumerable
            ? new EnumeratorObject(this.originalPrototype, (IEnumerable)collection)
            : throw new JScriptException(JSError.NotCollection);
    }

    /// <summary>Returns null in all cases.</summary>
    /// <returns>null.</returns>
    public object Invoke() => (object)null;
}