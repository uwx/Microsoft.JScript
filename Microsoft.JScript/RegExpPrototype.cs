// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.RegExpPrototype
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

#nullable disable
namespace Microsoft.JScript;

/// <summary>Provides static methods and properties for <see cref="T:Microsoft.JScript.RegExpObject" /> objects. All methods of this class are called by passing in a reference to the object that is requesting access to the method. This class belongs to the built-in object model category.</summary>
public class RegExpPrototype : JSObject
{
    internal static readonly RegExpPrototype ob = new RegExpPrototype(ObjectPrototype.ob);
    internal static RegExpConstructor _constructor;

    internal RegExpPrototype(ObjectPrototype parent)
        : base((ScriptObject)parent)
    {
    }

    /// <summary>Creates and compiles a new <see cref="T:Microsoft.JScript.RegExpObject" /> object for the specified regular expression pattern and flags.</summary>
    /// <returns>A new <see cref="T:Microsoft.JScript.RegExpObject" /> object.</returns>
    /// <param name="thisob">The object that this method is acting upon.</param>
    /// <param name="source">The regular expression pattern to search for.</param>
    /// <param name="flags">Flags for the ignore case, global, and multiline modes.</param>
    [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.RegExp_compile)]
    public static RegExpObject compile(object thisob, object source, object flags)
    {
        if (!(thisob is RegExpObject regExpObject))
            throw new JScriptException(JSError.RegExpExpected);
        string source1;
        switch (source)
        {
            case null:
            case Missing _:
                source1 = "";
                break;
            default:
                source1 = Convert.ToString(source);
                break;
        }

        string flags1;
        switch (flags)
        {
            case null:
            case Missing _:
                flags1 = "";
                break;
            default:
                flags1 = Convert.ToString(flags);
                break;
        }

        return regExpObject.compile(source1, flags1);
    }

    /// <summary>Gets a reference to a prototype-based object that is using this prototype.</summary>
    /// <returns>A reference to a prototype-based object.</returns>
    public static RegExpConstructor constructor => RegExpPrototype._constructor;

    /// <summary>Runs a search for the specified regular expression pattern on the specified object.</summary>
    /// <returns>The result of the search for the regular expression pattern.</returns>
    /// <param name="thisob">The object that this method is acting upon.</param>
    /// <param name="input">The regular expression pattern to search for.</param>
    [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.RegExp_exec)]
    public static object exec(object thisob, object input)
    {
        if (!(thisob is RegExpObject regExpObject))
            throw new JScriptException(JSError.RegExpExpected);
        if (input is Missing && !regExpObject.regExpConst.noExpando)
            input = regExpObject.regExpConst.input;
        return regExpObject.exec(Convert.ToString(input));
    }

    /// <summary>Tests the search for the specified regular expression pattern on the specified object.</summary>
    /// <returns>true if the search succeeds; otherwise, false.</returns>
    /// <param name="thisob">The object that this method is acting upon.</param>
    /// <param name="input">The regular expression pattern to search for.</param>
    [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.RegExp_test)]
    public static bool test(object thisob, object input)
    {
        if (!(thisob is RegExpObject regExpObject))
            throw new JScriptException(JSError.RegExpExpected);
        if (input is Missing && !regExpObject.regExpConst.noExpando)
            input = regExpObject.regExpConst.input;
        return regExpObject.test(Convert.ToString(input));
    }

    /// <summary>Creates a string representation of the specified object that contains the regular expression pattern and flags.</summary>
    /// <returns>A string representation of <paramref name="thisob" />.</returns>
    /// <param name="thisob">The object that this method is acting upon.</param>
    [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.RegExp_toString)]
    public static string toString(object thisob)
    {
        return thisob is RegExpObject regExpObject
            ? regExpObject.ToString()
            : throw new JScriptException(JSError.RegExpExpected);
    }
}