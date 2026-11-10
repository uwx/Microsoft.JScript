// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.BooleanPrototype
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Provides static methods and properties for <see cref="T:Microsoft.JScript.BooleanObject" /> objects. All methods of this class are called by passing in a reference to the object that is requesting access to the method. This class belongs to the built-in object model category.</summary>
public class BooleanPrototype : BooleanObject
{
    internal static readonly BooleanPrototype ob = new BooleanPrototype(ObjectPrototype.ob, typeof(BooleanPrototype));
    internal static BooleanConstructor _constructor;

    /// <summary>Initializes a new instance of the <see cref="T:Microsoft.JScript.BooleanPrototype" /> class.</summary>
    /// <param name="parent">A static instance of the parent class of this class.</param>
    /// <param name="baseType">An instance of the type of this class.</param>
    protected BooleanPrototype(ObjectPrototype parent, Type baseType)
        : base((ScriptObject)parent, baseType)
    {
        this.noExpando = true;
    }

    /// <summary>Gets a reference to a prototype-based object that is using this prototype.</summary>
    /// <returns>A reference to a prototype-based object.</returns>
    public static BooleanConstructor constructor => BooleanPrototype._constructor;

    /// <summary>Creates a string representation of the specified object by using the <see cref="T:Microsoft.JScript.Convert" /> class.</summary>
    /// <returns>A string representation of <paramref name="thisob" /> that is created by the <see cref="T:Microsoft.JScript.Convert" /> class.</returns>
    /// <param name="thisob">The object that this method is acting upon.</param>
    [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Boolean_toString)]
    public static string toString(object thisob)
    {
        if (thisob is BooleanObject)
            return Convert.ToString(((BooleanObject)thisob).value);
        return Convert.GetTypeCode(thisob) == TypeCode.Boolean
            ? Convert.ToString(thisob)
            : throw new JScriptException(JSError.BooleanExpected);
    }

    /// <summary>Returns the specified object as a <see cref="T:Microsoft.JScript.BooleanObject" /> or a <see cref="T:System.Boolean" />.</summary>
    /// <returns>
    /// <paramref name="thisob" /> as a <see cref="T:Microsoft.JScript.BooleanObject" /> or a <see cref="T:System.Boolean" />.</returns>
    /// <param name="thisob">The object that this method is acting upon.</param>
    [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Boolean_valueOf)]
    public static object valueOf(object thisob)
    {
        if (thisob is BooleanObject)
            return (object)((BooleanObject)thisob).value;
        return Convert.GetTypeCode(thisob) == TypeCode.Boolean
            ? thisob
            : throw new JScriptException(JSError.BooleanExpected);
    }
}