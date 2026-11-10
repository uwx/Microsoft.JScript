// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.ErrorPrototype
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

#nullable disable
namespace Microsoft.JScript;

/// <summary>Provides static methods and properties for <see cref="T:Microsoft.JScript.ErrorObject" /> objects. All methods of this class are called by passing in a reference to the object that is requesting access to the method. This class belongs to the built-in object model category.</summary>
public class ErrorPrototype : JSObject
{
    /// <summary>Specifies the name of the type of the error. Possible values include Error, EvalError, RangeError, ReferenceError, SyntaxError, TypeError, and URIError.</summary>
    public readonly string name;

    internal static readonly ErrorPrototype ob = new ErrorPrototype((ScriptObject)ObjectPrototype.ob, "Error");
    internal ErrorConstructor _constructor;

    internal ErrorPrototype(ScriptObject parent, string name)
        : base(parent)
    {
        this.name = name;
    }

    /// <summary>Gets a reference to a prototype-based object that is using this prototype.</summary>
    /// <returns>A reference to a prototype-based object.</returns>
    public ErrorConstructor constructor => this._constructor;

    /// <summary>Creates a string representation of the specified object.</summary>
    /// <returns>A string representation of <paramref name="thisob" /> that contains the name of the type of the error object, and an error message if there is one.</returns>
    /// <param name="thisob">The object that this method is acting upon.</param>
    [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Error_toString)]
    public static string toString(object thisob)
    {
        if (!(thisob is ErrorObject))
            return thisob.ToString();
        string message = ((ErrorObject)thisob).Message;
        return message.Length == 0
            ? LateBinding.GetMemberValue(thisob, "name").ToString()
            : $"{LateBinding.GetMemberValue(thisob, "name").ToString()}: {message}";
    }
}