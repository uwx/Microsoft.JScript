// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.FunctionPrototype
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

#nullable disable
namespace Microsoft.JScript;

/// <summary>Provides static methods and properties for <see cref="T:Microsoft.JScript.FunctionObject" /> objects. All methods of this class are called by passing in a reference to the object that is requesting access to the method. This class belongs to the built-in object model category.</summary>
public class FunctionPrototype : ScriptFunction
{
    internal static readonly FunctionPrototype ob =
        new FunctionPrototype((ScriptObject)ObjectPrototype.CommonInstance());

    internal static FunctionConstructor _constructor;

    internal FunctionPrototype(ScriptObject parent)
        : base(parent)
    {
    }

    internal override object Call(object[] args, object thisob) => (object)null;

    /// <summary>Calls the function that this object represents, converting the specified argument object to an array of arguments if it has to.</summary>
    /// <returns>The return value of the call to the function that this object represents.</returns>
    /// <param name="thisob">The object that this method is acting upon.</param>
    /// <param name="thisarg">The current default scope. Either the contained object of a with statement scope, or the global scope.</param>
    /// <param name="argArray">The arguments to pass to the function, as an <see cref="T:Microsoft.JScript.ArgumentsObject" /> or an <see cref="T:Microsoft.JScript.ArrayObject" />.</param>
    [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Function_apply)]
    public static object apply(object thisob, object thisarg, object argArray)
    {
        if (!(thisob is ScriptFunction))
            throw new JScriptException(JSError.FunctionExpected);
        if (thisarg is Missing)
            thisarg = ((IActivationObject)((ScriptObject)thisob).engine.ScriptObjectStackTop()).GetDefaultThisObject();
        switch (argArray)
        {
            case Missing _:
                return ((ScriptFunction)thisob).Call(new object[0], thisarg);
            case ArgumentsObject _:
                return ((ScriptFunction)thisob).Call(((ArgumentsObject)argArray).ToArray(), thisarg);
            case ArrayObject _:
                return ((ScriptFunction)thisob).Call(((ArrayObject)argArray).ToArray(), thisarg);
            default:
                throw new JScriptException(JSError.InvalidCall);
        }
    }

    /// <summary>Calls the function that this object represents.</summary>
    /// <returns>The return value of the call to the function that this object represents.</returns>
    /// <param name="thisob">The object that this method is acting upon.</param>
    /// <param name="thisarg">The current default scope. Either the contained object of a with statement scope, or the global scope.</param>
    /// <param name="args">The arguments to pass to the function as an array.</param>
    [JSFunction(JSFunctionAttributeEnum.HasThisObject | JSFunctionAttributeEnum.HasVarArgs, JSBuiltin.Function_call)]
    public static object call(object thisob, object thisarg, params object[] args)
    {
        if (!(thisob is ScriptFunction))
            throw new JScriptException(JSError.FunctionExpected);
        if (thisarg is Missing)
            thisarg = ((IActivationObject)((ScriptObject)thisob).engine.ScriptObjectStackTop()).GetDefaultThisObject();
        return ((ScriptFunction)thisob).Call(args, thisarg);
    }

    /// <summary>Gets a reference to a prototype-based object that is using this prototype.</summary>
    /// <returns>A reference to a prototype-based object.</returns>
    public static FunctionConstructor constructor => FunctionPrototype._constructor;

    /// <summary>Creates a string representation of the specified object.</summary>
    /// <returns>A string representation of <paramref name="thisob" /> that contains the name of the function.</returns>
    /// <param name="thisob">The object that this method is acting upon.</param>
    [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Function_toString)]
    public static string toString(object thisob)
    {
        return thisob is ScriptFunction ? thisob.ToString() : throw new JScriptException(JSError.FunctionExpected);
    }
}