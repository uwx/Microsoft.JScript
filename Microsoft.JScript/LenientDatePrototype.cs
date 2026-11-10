// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.LenientDatePrototype
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Stores objects that represent the static methods of the <see cref="T:Microsoft.JScript.DatePrototype" /> class. The <see cref="T:Microsoft.JScript.LenientDatePrototype" /> class belongs to the built-in object model category.</summary>
public sealed class LenientDatePrototype : DatePrototype
{
    /// <summary>Stores a reference to a <see cref="T:Microsoft.JScript.DateConstructor" /> object.</summary>
    public object constructor;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.DatePrototype.getTime(System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
    public object getTime;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.DatePrototype.getYear(System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
    [NotRecommended("getYear")] public object getYear;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.DatePrototype.getFullYear(System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
    public object getFullYear;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.DatePrototype.getUTCFullYear(System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
    public object getUTCFullYear;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.DatePrototype.getMonth(System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
    public object getMonth;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.DatePrototype.getUTCMonth(System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
    public object getUTCMonth;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.DatePrototype.getDate(System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
    public object getDate;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.DatePrototype.getUTCDate(System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
    public object getUTCDate;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.DatePrototype.getDay(System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
    public object getDay;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.DatePrototype.getUTCDay(System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
    public object getUTCDay;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.DatePrototype.getHours(System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
    public object getHours;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.DatePrototype.getUTCHours(System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
    public object getUTCHours;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.DatePrototype.getMinutes(System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
    public object getMinutes;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.DatePrototype.getUTCMinutes(System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
    public object getUTCMinutes;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.DatePrototype.getSeconds(System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
    public object getSeconds;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.DatePrototype.getUTCSeconds(System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
    public object getUTCSeconds;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.DatePrototype.getMilliseconds(System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
    public object getMilliseconds;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.DatePrototype.getUTCMilliseconds(System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
    public object getUTCMilliseconds;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.DatePrototype.getVarDate(System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
    public object getVarDate;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.DatePrototype.getTimezoneOffset(System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
    public object getTimezoneOffset;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.DatePrototype.setTime(System.Object,System.Double)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
    public object setTime;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.DatePrototype.setMilliseconds(System.Object,System.Double)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
    public object setMilliseconds;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.DatePrototype.setUTCMilliseconds(System.Object,System.Double)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
    public object setUTCMilliseconds;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.DatePrototype.setSeconds(System.Object,System.Double,System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
    public object setSeconds;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.DatePrototype.setUTCSeconds(System.Object,System.Double,System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
    public object setUTCSeconds;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.DatePrototype.setMinutes(System.Object,System.Double,System.Object,System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
    public object setMinutes;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.DatePrototype.setUTCMinutes(System.Object,System.Double,System.Object,System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
    public object setUTCMinutes;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.DatePrototype.setHours(System.Object,System.Double,System.Object,System.Object,System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
    public object setHours;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.DatePrototype.setUTCHours(System.Object,System.Double,System.Object,System.Object,System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
    public object setUTCHours;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.DatePrototype.setDate(System.Object,System.Double)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
    public object setDate;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.DatePrototype.setUTCDate(System.Object,System.Double)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
    public object setUTCDate;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.DatePrototype.setMonth(System.Object,System.Double,System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
    public object setMonth;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.DatePrototype.setUTCMonth(System.Object,System.Double,System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
    public object setUTCMonth;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.DatePrototype.setFullYear(System.Object,System.Double,System.Object,System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
    public object setFullYear;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.DatePrototype.setUTCFullYear(System.Object,System.Double,System.Object,System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
    public object setUTCFullYear;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.DatePrototype.setYear(System.Object,System.Double)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
    [NotRecommended("setYear")] public object setYear;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.DatePrototype.toGMTString(System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
    [NotRecommended("toGMTString")] public object toGMTString;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.DatePrototype.toDateString(System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
    public object toDateString;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.DatePrototype.toLocaleDateString(System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
    public object toLocaleDateString;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.DatePrototype.toLocaleString(System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
    public object toLocaleString;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.DatePrototype.toLocaleTimeString(System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
    public object toLocaleTimeString;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.DatePrototype.toString(System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
    public object toString;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.DatePrototype.toTimeString(System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
    public object toTimeString;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.DatePrototype.toUTCString(System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
    public object toUTCString;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.DatePrototype.valueOf(System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
    public object valueOf;

    internal LenientDatePrototype(LenientFunctionPrototype funcprot, LenientObjectPrototype parent)
        : base((ObjectPrototype)parent)
    {
        this.noExpando = false;
        Type type = typeof(DatePrototype);
        this.getTime = (object)new BuiltinFunction(nameof(getTime), (object)this, type.GetMethod(nameof(getTime)),
            (ScriptFunction)funcprot);
        this.getYear = (object)new BuiltinFunction(nameof(getYear), (object)this, type.GetMethod(nameof(getYear)),
            (ScriptFunction)funcprot);
        this.getFullYear = (object)new BuiltinFunction(nameof(getFullYear), (object)this,
            type.GetMethod(nameof(getFullYear)), (ScriptFunction)funcprot);
        this.getUTCFullYear = (object)new BuiltinFunction(nameof(getUTCFullYear), (object)this,
            type.GetMethod(nameof(getUTCFullYear)), (ScriptFunction)funcprot);
        this.getMonth = (object)new BuiltinFunction(nameof(getMonth), (object)this, type.GetMethod(nameof(getMonth)),
            (ScriptFunction)funcprot);
        this.getUTCMonth = (object)new BuiltinFunction(nameof(getUTCMonth), (object)this,
            type.GetMethod(nameof(getUTCMonth)), (ScriptFunction)funcprot);
        this.getDate = (object)new BuiltinFunction(nameof(getDate), (object)this, type.GetMethod(nameof(getDate)),
            (ScriptFunction)funcprot);
        this.getUTCDate = (object)new BuiltinFunction(nameof(getUTCDate), (object)this,
            type.GetMethod(nameof(getUTCDate)), (ScriptFunction)funcprot);
        this.getDay = (object)new BuiltinFunction(nameof(getDay), (object)this, type.GetMethod(nameof(getDay)),
            (ScriptFunction)funcprot);
        this.getUTCDay = (object)new BuiltinFunction(nameof(getUTCDay), (object)this, type.GetMethod(nameof(getUTCDay)),
            (ScriptFunction)funcprot);
        this.getHours = (object)new BuiltinFunction(nameof(getHours), (object)this, type.GetMethod(nameof(getHours)),
            (ScriptFunction)funcprot);
        this.getUTCHours = (object)new BuiltinFunction(nameof(getUTCHours), (object)this,
            type.GetMethod(nameof(getUTCHours)), (ScriptFunction)funcprot);
        this.getMinutes = (object)new BuiltinFunction(nameof(getMinutes), (object)this,
            type.GetMethod(nameof(getMinutes)), (ScriptFunction)funcprot);
        this.getUTCMinutes = (object)new BuiltinFunction(nameof(getUTCMinutes), (object)this,
            type.GetMethod(nameof(getUTCMinutes)), (ScriptFunction)funcprot);
        this.getSeconds = (object)new BuiltinFunction(nameof(getSeconds), (object)this,
            type.GetMethod(nameof(getSeconds)), (ScriptFunction)funcprot);
        this.getUTCSeconds = (object)new BuiltinFunction(nameof(getUTCSeconds), (object)this,
            type.GetMethod(nameof(getUTCSeconds)), (ScriptFunction)funcprot);
        this.getMilliseconds = (object)new BuiltinFunction(nameof(getMilliseconds), (object)this,
            type.GetMethod(nameof(getMilliseconds)), (ScriptFunction)funcprot);
        this.getUTCMilliseconds = (object)new BuiltinFunction(nameof(getUTCMilliseconds), (object)this,
            type.GetMethod(nameof(getUTCMilliseconds)), (ScriptFunction)funcprot);
        this.getVarDate = (object)new BuiltinFunction(nameof(getVarDate), (object)this,
            type.GetMethod(nameof(getVarDate)), (ScriptFunction)funcprot);
        this.getTimezoneOffset = (object)new BuiltinFunction(nameof(getTimezoneOffset), (object)this,
            type.GetMethod(nameof(getTimezoneOffset)), (ScriptFunction)funcprot);
        this.setTime = (object)new BuiltinFunction(nameof(setTime), (object)this, type.GetMethod(nameof(setTime)),
            (ScriptFunction)funcprot);
        this.setMilliseconds = (object)new BuiltinFunction(nameof(setMilliseconds), (object)this,
            type.GetMethod(nameof(setMilliseconds)), (ScriptFunction)funcprot);
        this.setUTCMilliseconds = (object)new BuiltinFunction(nameof(setUTCMilliseconds), (object)this,
            type.GetMethod(nameof(setUTCMilliseconds)), (ScriptFunction)funcprot);
        this.setSeconds = (object)new BuiltinFunction(nameof(setSeconds), (object)this,
            type.GetMethod(nameof(setSeconds)), (ScriptFunction)funcprot);
        this.setUTCSeconds = (object)new BuiltinFunction(nameof(setUTCSeconds), (object)this,
            type.GetMethod(nameof(setUTCSeconds)), (ScriptFunction)funcprot);
        this.setMinutes = (object)new BuiltinFunction(nameof(setMinutes), (object)this,
            type.GetMethod(nameof(setMinutes)), (ScriptFunction)funcprot);
        this.setUTCMinutes = (object)new BuiltinFunction(nameof(setUTCMinutes), (object)this,
            type.GetMethod(nameof(setUTCMinutes)), (ScriptFunction)funcprot);
        this.setHours = (object)new BuiltinFunction(nameof(setHours), (object)this, type.GetMethod(nameof(setHours)),
            (ScriptFunction)funcprot);
        this.setUTCHours = (object)new BuiltinFunction(nameof(setUTCHours), (object)this,
            type.GetMethod(nameof(setUTCHours)), (ScriptFunction)funcprot);
        this.setDate = (object)new BuiltinFunction(nameof(setDate), (object)this, type.GetMethod(nameof(setDate)),
            (ScriptFunction)funcprot);
        this.setUTCDate = (object)new BuiltinFunction(nameof(setUTCDate), (object)this,
            type.GetMethod(nameof(setUTCDate)), (ScriptFunction)funcprot);
        this.setMonth = (object)new BuiltinFunction(nameof(setMonth), (object)this, type.GetMethod(nameof(setMonth)),
            (ScriptFunction)funcprot);
        this.setUTCMonth = (object)new BuiltinFunction(nameof(setUTCMonth), (object)this,
            type.GetMethod(nameof(setUTCMonth)), (ScriptFunction)funcprot);
        this.setFullYear = (object)new BuiltinFunction(nameof(setFullYear), (object)this,
            type.GetMethod(nameof(setFullYear)), (ScriptFunction)funcprot);
        this.setUTCFullYear = (object)new BuiltinFunction(nameof(setUTCFullYear), (object)this,
            type.GetMethod(nameof(setUTCFullYear)), (ScriptFunction)funcprot);
        this.setYear = (object)new BuiltinFunction(nameof(setYear), (object)this, type.GetMethod(nameof(setYear)),
            (ScriptFunction)funcprot);
        this.toDateString = (object)new BuiltinFunction(nameof(toDateString), (object)this,
            type.GetMethod(nameof(toDateString)), (ScriptFunction)funcprot);
        this.toLocaleDateString = (object)new BuiltinFunction(nameof(toLocaleDateString), (object)this,
            type.GetMethod(nameof(toLocaleDateString)), (ScriptFunction)funcprot);
        this.toLocaleString = (object)new BuiltinFunction(nameof(toLocaleString), (object)this,
            type.GetMethod(nameof(toLocaleString)), (ScriptFunction)funcprot);
        this.toLocaleTimeString = (object)new BuiltinFunction(nameof(toLocaleTimeString), (object)this,
            type.GetMethod(nameof(toLocaleTimeString)), (ScriptFunction)funcprot);
        this.toGMTString = (object)new BuiltinFunction(nameof(toUTCString), (object)this,
            type.GetMethod(nameof(toUTCString)), (ScriptFunction)funcprot);
        this.toString = (object)new BuiltinFunction(nameof(toString), (object)this, type.GetMethod(nameof(toString)),
            (ScriptFunction)funcprot);
        this.toTimeString = (object)new BuiltinFunction(nameof(toTimeString), (object)this,
            type.GetMethod(nameof(toTimeString)), (ScriptFunction)funcprot);
        this.toUTCString = (object)new BuiltinFunction(nameof(toUTCString), (object)this,
            type.GetMethod(nameof(toUTCString)), (ScriptFunction)funcprot);
        this.valueOf = (object)new BuiltinFunction(nameof(valueOf), (object)this, type.GetMethod(nameof(valueOf)),
            (ScriptFunction)funcprot);
    }
}