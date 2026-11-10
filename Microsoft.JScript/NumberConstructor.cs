// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.NumberConstructor
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

#nullable disable
namespace Microsoft.JScript;

/// <summary>Represents a prototype-based object that is defined by a constructor function. This class belongs to the built-in object model category.</summary>
public class NumberConstructor : ScriptFunction
{
    /// <summary>Represents the largest possible value of a <see cref="T:System.Double" />. This field is constant.</summary>
    public const double MAX_VALUE = 1.7976931348623157E+308;

    /// <summary>Represents the smallest positive <see cref="T:System.Double" /> value that is greater than zero. This field is constant.</summary>
    public const double MIN_VALUE = 4.94065645841247E-324;

    /// <summary>Represents a value that is not a number (NaN). This field is constant.</summary>
    public const double NaN = double.NaN;

    /// <summary>Represents negative infinity. This field is constant.</summary>
    public const double NEGATIVE_INFINITY = double.NegativeInfinity;

    /// <summary>Represents positive infinity. This field is constant.</summary>
    public const double POSITIVE_INFINITY = double.PositiveInfinity;

    internal static readonly NumberConstructor ob = new NumberConstructor();
    private NumberPrototype originalPrototype;

    internal NumberConstructor()
        : base((ScriptObject)FunctionPrototype.ob, "Number", 1)
    {
        this.originalPrototype = NumberPrototype.ob;
        NumberPrototype._constructor = this;
        this.proto = (object)NumberPrototype.ob;
    }

    internal NumberConstructor(LenientFunctionPrototype parent, LenientNumberPrototype prototypeProp)
        : base((ScriptObject)parent, "Number", 1)
    {
        this.originalPrototype = (NumberPrototype)prototypeProp;
        prototypeProp.constructor = (object)this;
        this.proto = (object)prototypeProp;
        this.noExpando = false;
    }

    internal override object Call(object[] args, object thisob)
    {
        return args.Length == 0 ? (object)0 : (object)Convert.ToNumber(args[0]);
    }

    internal NumberObject Construct()
    {
        return new NumberObject((ScriptObject)this.originalPrototype, (object)0.0, false);
    }

    internal override object Construct(object[] args) => (object)this.CreateInstance(args);

    internal NumberObject ConstructImplicitWrapper(object arg)
    {
        return new NumberObject((ScriptObject)this.originalPrototype, arg, true);
    }

    internal NumberObject ConstructWrapper(object arg)
    {
        return new NumberObject((ScriptObject)this.originalPrototype, arg, false);
    }

    /// <summary>Creates an instance of the prototype-based object, passing in the specified arguments.</summary>
    /// <returns>An instance of the object.</returns>
    /// <param name="args">The arguments to pass to the constructor.</param>
    [JSFunction(JSFunctionAttributeEnum.HasVarArgs)]
    public NumberObject CreateInstance(params object[] args)
    {
        return args.Length == 0
            ? new NumberObject((ScriptObject)this.originalPrototype, (object)0.0, false)
            : new NumberObject((ScriptObject)this.originalPrototype, (object)Convert.ToNumber(args[0]), false);
    }

    /// <summary>Converts the specified object to a number.</summary>
    /// <returns>
    /// <paramref name="arg" /> expressed as a number, if that is possible.</returns>
    /// <param name="arg">The object to convert.</param>
    public double Invoke(object arg) => Convert.ToNumber(arg);
}