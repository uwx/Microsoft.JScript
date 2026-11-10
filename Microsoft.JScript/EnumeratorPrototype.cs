// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.EnumeratorPrototype
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

#nullable disable
namespace Microsoft.JScript;

/// <summary>Provides static methods and properties for <see cref="T:Microsoft.JScript.EnumeratorObject" /> objects. All methods of this class are called by passing in a reference to the object that is requesting access to the method. This class belongs to the built-in object model category.</summary>
public class EnumeratorPrototype : JSObject
{
    internal static readonly EnumeratorPrototype ob = new EnumeratorPrototype(ObjectPrototype.ob);
    internal static EnumeratorConstructor _constructor;

    internal EnumeratorPrototype(ObjectPrototype parent)
        : base((ScriptObject)parent)
    {
    }

    /// <summary>Determines whether the enumerator has passed the last item of the collection.</summary>
    /// <returns>true if the enumerator has passed the last item of the collection; otherwise, false.</returns>
    /// <param name="thisob">The object that this method is acting upon.</param>
    [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Enumerator_atEnd)]
    public static bool atEnd(object thisob)
    {
        return thisob is EnumeratorObject
            ? ((EnumeratorObject)thisob).atEnd()
            : throw new JScriptException(JSError.EnumeratorExpected);
    }

    /// <summary>Gets a reference to a prototype-based object that is using this prototype.</summary>
    /// <returns>A reference to a prototype-based object.</returns>
    public static EnumeratorConstructor constructor => EnumeratorPrototype._constructor;

    /// <summary>Gets the current element in the collection.</summary>
    /// <returns>The current element in the collection.</returns>
    /// <param name="thisob">The object that this method is acting upon.</param>
    [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Enumerator_item)]
    public static object item(object thisob)
    {
        return thisob is EnumeratorObject
            ? ((EnumeratorObject)thisob).item()
            : throw new JScriptException(JSError.EnumeratorExpected);
    }

    /// <summary>Moves the enumerator to its initial position, which is before the first element in the collection.</summary>
    /// <param name="thisob">The object that this method is acting upon.</param>
    [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Enumerator_moveFirst)]
    public static void moveFirst(object thisob)
    {
        if (!(thisob is EnumeratorObject))
            throw new JScriptException(JSError.EnumeratorExpected);
        ((EnumeratorObject)thisob).moveFirst();
    }

    /// <summary>Advances the enumerator to the next element of the collection.</summary>
    /// <param name="thisob">The object that this method is acting upon.</param>
    [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Enumerator_moveNext)]
    public static void moveNext(object thisob)
    {
        if (!(thisob is EnumeratorObject))
            throw new JScriptException(JSError.EnumeratorExpected);
        ((EnumeratorObject)thisob).moveNext();
    }
}