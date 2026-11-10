// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.LenientMathObject
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Stores objects that represent the static methods of the <see cref="T:Microsoft.JScript.MathObject" /> class. The <see cref="T:Microsoft.JScript.LenientMathObject" /> class belongs to the built-in object model category.</summary>
public sealed class LenientMathObject : MathObject
{
    /// <summary>Stores a value that represents Euler's constant, the base of the natural logarithm, specified by the constant e.</summary>
    public new const double E = 2.7182818284590451;

    /// <summary>Stores a value that represents the logarithm base e of 10, the natural logarithm of 10.</summary>
    public new const double LN10 = 2.3025850929940459;

    /// <summary>Stores a value that represents the logarithm base e of 2, the natural logarithm of 2.</summary>
    public new const double LN2 = 0.69314718055994529;

    /// <summary>Stores a value that represents the logarithm base 2 of e, the binary logarithm of e.</summary>
    public new const double LOG2E = 1.4426950408889634;

    /// <summary>Stores a value that represents the logarithm base 10 of e, the common logarithm of e.</summary>
    public new const double LOG10E = 0.43429448190325182;

    /// <summary>Stores a value that represents the ratio of the circumference of a circle to its diameter, specified by the constant π.</summary>
    public new const double PI = 3.1415926535897931;

    /// <summary>Stores a value that represents the following equivalent values: (√2)/2, √(½), and 1/(√2).</summary>
    public new const double SQRT1_2 = 0.70710678118654757;

    /// <summary>Stores a value that represents √2.</summary>
    public new const double SQRT2 = 1.4142135623730951;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.MathObject.abs(System.Double)" /> method of the <see cref="T:Microsoft.JScript.MathObject" /> class.</summary>
    public object abs;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.MathObject.acos(System.Double)" /> method of the <see cref="T:Microsoft.JScript.MathObject" /> class.</summary>
    public object acos;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.MathObject.asin(System.Double)" /> method of the <see cref="T:Microsoft.JScript.MathObject" /> class.</summary>
    public object asin;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.MathObject.atan(System.Double)" /> method of the <see cref="T:Microsoft.JScript.MathObject" /> class.</summary>
    public object atan;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.MathObject.atan2(System.Double,System.Double)" /> method of the <see cref="T:Microsoft.JScript.MathObject" /> class.</summary>
    public object atan2;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.MathObject.ceil(System.Double)" /> method of the <see cref="T:Microsoft.JScript.MathObject" /> class.</summary>
    public object ceil;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.MathObject.cos(System.Double)" /> method of the <see cref="T:Microsoft.JScript.MathObject" /> class.</summary>
    public object cos;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.MathObject.exp(System.Double)" /> method of the <see cref="T:Microsoft.JScript.MathObject" /> class.</summary>
    public object exp;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.MathObject.floor(System.Double)" /> method of the <see cref="T:Microsoft.JScript.MathObject" /> class.</summary>
    public object floor;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.MathObject.log(System.Double)" /> method of the <see cref="T:Microsoft.JScript.MathObject" /> class.</summary>
    public object log;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.MathObject.max(System.Object,System.Object,System.Object[])" /> method of the <see cref="T:Microsoft.JScript.MathObject" /> class.</summary>
    public object max;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.MathObject.min(System.Object,System.Object,System.Object[])" /> method of the <see cref="T:Microsoft.JScript.MathObject" /> class.</summary>
    public object min;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.MathObject.pow(System.Double,System.Double)" /> method of the <see cref="T:Microsoft.JScript.MathObject" /> class.</summary>
    public object pow;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.MathObject.random" /> method of the <see cref="T:Microsoft.JScript.MathObject" /> class.</summary>
    public object random;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.MathObject.round(System.Double)" /> method of the <see cref="T:Microsoft.JScript.MathObject" /> class.</summary>
    public object round;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.MathObject.sin(System.Double)" /> method of the <see cref="T:Microsoft.JScript.MathObject" /> class.</summary>
    public object sin;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.MathObject.sqrt(System.Double)" /> method of the <see cref="T:Microsoft.JScript.MathObject" /> class.</summary>
    public object sqrt;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.MathObject.tan(System.Double)" /> method of the <see cref="T:Microsoft.JScript.MathObject" /> class.</summary>
    public object tan;

    internal LenientMathObject(ScriptObject parent, FunctionPrototype funcprot)
        : base(parent)
    {
        this.noExpando = false;
        Type type = typeof(MathObject);
        this.abs = (object)new BuiltinFunction(nameof(abs), (object)this, type.GetMethod(nameof(abs)),
            (ScriptFunction)funcprot);
        this.acos = (object)new BuiltinFunction(nameof(acos), (object)this, type.GetMethod(nameof(acos)),
            (ScriptFunction)funcprot);
        this.asin = (object)new BuiltinFunction(nameof(asin), (object)this, type.GetMethod(nameof(asin)),
            (ScriptFunction)funcprot);
        this.atan = (object)new BuiltinFunction(nameof(atan), (object)this, type.GetMethod(nameof(atan)),
            (ScriptFunction)funcprot);
        this.atan2 = (object)new BuiltinFunction(nameof(atan2), (object)this, type.GetMethod(nameof(atan2)),
            (ScriptFunction)funcprot);
        this.ceil = (object)new BuiltinFunction(nameof(ceil), (object)this, type.GetMethod(nameof(ceil)),
            (ScriptFunction)funcprot);
        this.cos = (object)new BuiltinFunction(nameof(cos), (object)this, type.GetMethod(nameof(cos)),
            (ScriptFunction)funcprot);
        this.exp = (object)new BuiltinFunction(nameof(exp), (object)this, type.GetMethod(nameof(exp)),
            (ScriptFunction)funcprot);
        this.floor = (object)new BuiltinFunction(nameof(floor), (object)this, type.GetMethod(nameof(floor)),
            (ScriptFunction)funcprot);
        this.log = (object)new BuiltinFunction(nameof(log), (object)this, type.GetMethod(nameof(log)),
            (ScriptFunction)funcprot);
        this.max = (object)new BuiltinFunction(nameof(max), (object)this, type.GetMethod(nameof(max)),
            (ScriptFunction)funcprot);
        this.min = (object)new BuiltinFunction(nameof(min), (object)this, type.GetMethod(nameof(min)),
            (ScriptFunction)funcprot);
        this.pow = (object)new BuiltinFunction(nameof(pow), (object)this, type.GetMethod(nameof(pow)),
            (ScriptFunction)funcprot);
        this.random = (object)new BuiltinFunction(nameof(random), (object)this, type.GetMethod(nameof(random)),
            (ScriptFunction)funcprot);
        this.round = (object)new BuiltinFunction(nameof(round), (object)this, type.GetMethod(nameof(round)),
            (ScriptFunction)funcprot);
        this.sin = (object)new BuiltinFunction(nameof(sin), (object)this, type.GetMethod(nameof(sin)),
            (ScriptFunction)funcprot);
        this.sqrt = (object)new BuiltinFunction(nameof(sqrt), (object)this, type.GetMethod(nameof(sqrt)),
            (ScriptFunction)funcprot);
        this.tan = (object)new BuiltinFunction(nameof(tan), (object)this, type.GetMethod(nameof(tan)),
            (ScriptFunction)funcprot);
    }
}