// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.StringConstructor
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System.Text;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Represents a prototype-based object that is defined by a constructor function. This class belongs to the built-in object model category.</summary>
public class StringConstructor : ScriptFunction
{
    internal static readonly StringConstructor ob = new StringConstructor();
    private StringPrototype originalPrototype;

    internal StringConstructor()
        : base((ScriptObject)FunctionPrototype.ob, "String", 1)
    {
        this.originalPrototype = StringPrototype.ob;
        StringPrototype._constructor = this;
        this.proto = (object)StringPrototype.ob;
    }

    internal StringConstructor(FunctionPrototype parent, LenientStringPrototype prototypeProp)
        : base((ScriptObject)parent, "String", 1)
    {
        this.originalPrototype = (StringPrototype)prototypeProp;
        prototypeProp.constructor = (object)this;
        this.proto = (object)prototypeProp;
        this.noExpando = false;
    }

    internal override object Call(object[] args, object thisob)
    {
        return args.Length == 0 ? (object)"" : (object)Convert.ToString(args[0]);
    }

    internal StringObject Construct()
    {
        return new StringObject((ScriptObject)this.originalPrototype, "", false);
    }

    internal override object Construct(object[] args) => (object)this.CreateInstance(args);

    internal StringObject ConstructImplicitWrapper(string arg)
    {
        return new StringObject((ScriptObject)this.originalPrototype, arg, true);
    }

    internal StringObject ConstructWrapper(string arg)
    {
        return new StringObject((ScriptObject)this.originalPrototype, arg, false);
    }

    /// <summary>Creates an instance of the prototype-based object, passing in the specified arguments.</summary>
    /// <returns>An instance of the object.</returns>
    /// <param name="args">The arguments to pass to the constructor.</param>
    [JSFunction(JSFunctionAttributeEnum.HasVarArgs)]
    public StringObject CreateInstance(params object[] args)
    {
        return new StringObject((ScriptObject)this.originalPrototype, args.Length == 0 ? "" : Convert.ToString(args[0]),
            false);
    }

    /// <summary>Converts the specified object to a string.</summary>
    /// <returns>A string representation of <paramref name="arg" />.</returns>
    /// <param name="arg">The object to convert to a string.</param>
    public string Invoke(object arg) => Convert.ToString(arg);

    /// <summary>Converts each element of the specified array to a character and concatenates them.</summary>
    /// <returns>A string representation of <paramref name="arg" />.</returns>
    /// <param name="args">The elements to convert to a string.</param>
    [JSFunction(JSFunctionAttributeEnum.HasVarArgs, JSBuiltin.String_fromCharCode)]
    public static string fromCharCode(params object[] args)
    {
        StringBuilder stringBuilder = new StringBuilder(args.Length);
        for (int index = 0; index < args.Length; ++index)
            stringBuilder.Append(Convert.ToChar(args[index]));
        return stringBuilder.ToString();
    }
}