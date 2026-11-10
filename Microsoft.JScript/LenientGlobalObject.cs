// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.LenientGlobalObject
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using Microsoft.JScript.Vsa;
using System;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Stores constructor function objects, data type information, and objects that represent the static methods of the <see cref="T:Microsoft.JScript.GlobalObject" /> class. The <see cref="T:Microsoft.JScript.LenientGlobalObject" /> class belongs to the built-in object model category.</summary>
public sealed class LenientGlobalObject : GlobalObject
{
    /// <summary>Represents positive infinity.</summary>
    public object Infinity;

    private object MathField;

    /// <summary>Represents a value that is not a number (NaN).</summary>
    public object NaN;

    /// <summary>Represents a value that is null.</summary>
    public object undefined;

    private object ActiveXObjectField;
    private object ArrayField;
    private object BooleanField;
    private object DateField;
    private object EnumeratorField;
    private object ErrorField;
    private object EvalErrorField;
    private object FunctionField;
    private object NumberField;
    private object ObjectField;
    private object RangeErrorField;
    private object ReferenceErrorField;
    private object RegExpField;
    private object StringField;
    private object SyntaxErrorField;
    private object TypeErrorField;
    private object VBArrayField;
    private object URIErrorField;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.GlobalObject.decodeURI(System.Object)" /> method of the <see cref="T:Microsoft.JScript.GlobalObject" /> class.</summary>
    public object decodeURI;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.GlobalObject.decodeURIComponent(System.Object)" /> method of the <see cref="T:Microsoft.JScript.GlobalObject" /> class.</summary>
    public object decodeURIComponent;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.GlobalObject.encodeURI(System.Object)" /> method of the <see cref="T:Microsoft.JScript.GlobalObject" /> class.</summary>
    public object encodeURI;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.GlobalObject.encodeURIComponent(System.Object)" /> method of the <see cref="T:Microsoft.JScript.GlobalObject" /> class.</summary>
    public object encodeURIComponent;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.GlobalObject.escape(System.Object)" /> method of the <see cref="T:Microsoft.JScript.GlobalObject" /> class.</summary>
    [NotRecommended("escape")] public object escape;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.GlobalObject.eval(System.Object)" /> method of the <see cref="T:Microsoft.JScript.GlobalObject" /> class.</summary>
    public object eval;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.GlobalObject.isNaN(System.Object)" /> method of the <see cref="T:Microsoft.JScript.GlobalObject" /> class.</summary>
    public object isNaN;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.GlobalObject.isFinite(System.Double)" /> method of the <see cref="T:Microsoft.JScript.GlobalObject" /> class.</summary>
    public object isFinite;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.GlobalObject.parseInt(System.Object,System.Object)" /> method of the <see cref="T:Microsoft.JScript.GlobalObject" /> class.</summary>
    public object parseInt;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.GlobalObject.parseFloat(System.Object)" /> method of the <see cref="T:Microsoft.JScript.GlobalObject" /> class.</summary>
    public object parseFloat;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.GlobalObject.GetObject(System.Object,System.Object)" /> method of the <see cref="T:Microsoft.JScript.GlobalObject" /> class.</summary>
    public object GetObject;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.GlobalObject.ScriptEngine" /> method of the <see cref="T:Microsoft.JScript.GlobalObject" /> class.</summary>
    public object ScriptEngine;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.GlobalObject.ScriptEngineBuildVersion" /> method of the <see cref="T:Microsoft.JScript.GlobalObject" /> class.</summary>
    public object ScriptEngineBuildVersion;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.GlobalObject.ScriptEngineMajorVersion" /> method of the <see cref="T:Microsoft.JScript.GlobalObject" /> class.</summary>
    public object ScriptEngineMajorVersion;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.GlobalObject.ScriptEngineMinorVersion" /> method of the <see cref="T:Microsoft.JScript.GlobalObject" /> class.</summary>
    public object ScriptEngineMinorVersion;

    /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.GlobalObject.unescape(System.Object)" /> method of the <see cref="T:Microsoft.JScript.GlobalObject" /> class.</summary>
    [NotRecommended("unescape")] public object unescape;

    /// <summary>Stores the managed type that corresponds to the JScript boolean data type.</summary>
    public object boolean;

    /// <summary>Stores the managed type that corresponds to the JScript byte data type.</summary>
    public object @byte;

    /// <summary>Stores the managed type that corresponds to the JScript char data type.</summary>
    public object @char;

    /// <summary>Stores the managed type that corresponds to the JScript decimal data type.</summary>
    public object @decimal;

    /// <summary>Stores the managed type that corresponds to the JScript double data type.</summary>
    public object @double;

    /// <summary>Stores the managed type that corresponds to the JScript float data type.</summary>
    public object @float;

    /// <summary>Stores the managed type that corresponds to the JScript int data type.</summary>
    public object @int;

    /// <summary>Stores the managed type that corresponds to the JScript long data type.</summary>
    public object @long;

    /// <summary>Stores the managed type that corresponds to the JScript sbyte data type.</summary>
    public object @sbyte;

    /// <summary>Stores the managed type that corresponds to the JScript short data type.</summary>
    public object @short;

    /// <summary>Stores the managed type that corresponds to the return value of a function that does not return a value.</summary>
    public object @void;

    /// <summary>Stores the managed type that corresponds to the JScript uint data type.</summary>
    public object @uint;

    /// <summary>Stores the managed type that corresponds to the JScript ulong data type.</summary>
    public object @ulong;

    /// <summary>Stores the managed type that corresponds to the JScript ushort data type.</summary>
    public object @ushort;

    private LenientArrayPrototype arrayPrototypeField;
    private LenientFunctionPrototype functionPrototypeField;
    private LenientObjectPrototype objectPrototypeField;
    private VsaEngine engine;

    internal LenientGlobalObject(VsaEngine engine)
    {
        this.engine = engine;
        this.Infinity = (object)double.PositiveInfinity;
        this.NaN = (object)double.NaN;
        this.undefined = (object)null;
        this.ActiveXObjectField = (object)Missing.Value;
        this.ArrayField = (object)Missing.Value;
        this.BooleanField = (object)Missing.Value;
        this.DateField = (object)Missing.Value;
        this.EnumeratorField = (object)Missing.Value;
        this.ErrorField = (object)Missing.Value;
        this.EvalErrorField = (object)Missing.Value;
        this.FunctionField = (object)Missing.Value;
        this.MathField = (object)Missing.Value;
        this.NumberField = (object)Missing.Value;
        this.ObjectField = (object)Missing.Value;
        this.RangeErrorField = (object)Missing.Value;
        this.ReferenceErrorField = (object)Missing.Value;
        this.RegExpField = (object)Missing.Value;
        this.StringField = (object)Missing.Value;
        this.SyntaxErrorField = (object)Missing.Value;
        this.TypeErrorField = (object)Missing.Value;
        this.VBArrayField = (object)Missing.Value;
        this.URIErrorField = (object)Missing.Value;
        Type type = typeof(GlobalObject);
        LenientFunctionPrototype functionPrototype = this.functionPrototype;
        this.decodeURI = (object)new BuiltinFunction(nameof(decodeURI), (object)this, type.GetMethod(nameof(decodeURI)),
            (ScriptFunction)functionPrototype);
        this.decodeURIComponent = (object)new BuiltinFunction(nameof(decodeURIComponent), (object)this,
            type.GetMethod(nameof(decodeURIComponent)), (ScriptFunction)functionPrototype);
        this.encodeURI = (object)new BuiltinFunction(nameof(encodeURI), (object)this, type.GetMethod(nameof(encodeURI)),
            (ScriptFunction)functionPrototype);
        this.encodeURIComponent = (object)new BuiltinFunction(nameof(encodeURIComponent), (object)this,
            type.GetMethod(nameof(encodeURIComponent)), (ScriptFunction)functionPrototype);
        this.escape = (object)new BuiltinFunction(nameof(escape), (object)this, type.GetMethod(nameof(escape)),
            (ScriptFunction)functionPrototype);
        this.eval = (object)new BuiltinFunction(nameof(eval), (object)this, type.GetMethod(nameof(eval)),
            (ScriptFunction)functionPrototype);
        this.isNaN = (object)new BuiltinFunction(nameof(isNaN), (object)this, type.GetMethod(nameof(isNaN)),
            (ScriptFunction)functionPrototype);
        this.isFinite = (object)new BuiltinFunction(nameof(isFinite), (object)this, type.GetMethod(nameof(isFinite)),
            (ScriptFunction)functionPrototype);
        this.parseInt = (object)new BuiltinFunction(nameof(parseInt), (object)this, type.GetMethod(nameof(parseInt)),
            (ScriptFunction)functionPrototype);
        this.GetObject = (object)new BuiltinFunction(nameof(GetObject), (object)this, type.GetMethod(nameof(GetObject)),
            (ScriptFunction)functionPrototype);
        this.parseFloat = (object)new BuiltinFunction(nameof(parseFloat), (object)this,
            type.GetMethod(nameof(parseFloat)), (ScriptFunction)functionPrototype);
        this.ScriptEngine = (object)new BuiltinFunction(nameof(ScriptEngine), (object)this,
            type.GetMethod(nameof(ScriptEngine)), (ScriptFunction)functionPrototype);
        this.ScriptEngineBuildVersion = (object)new BuiltinFunction(nameof(ScriptEngineBuildVersion), (object)this,
            type.GetMethod(nameof(ScriptEngineBuildVersion)), (ScriptFunction)functionPrototype);
        this.ScriptEngineMajorVersion = (object)new BuiltinFunction(nameof(ScriptEngineMajorVersion), (object)this,
            type.GetMethod(nameof(ScriptEngineMajorVersion)), (ScriptFunction)functionPrototype);
        this.ScriptEngineMinorVersion = (object)new BuiltinFunction(nameof(ScriptEngineMinorVersion), (object)this,
            type.GetMethod(nameof(ScriptEngineMinorVersion)), (ScriptFunction)functionPrototype);
        this.unescape = (object)new BuiltinFunction(nameof(unescape), (object)this, type.GetMethod(nameof(unescape)),
            (ScriptFunction)functionPrototype);
        this.boolean = (object)Typeob.Boolean;
        this.@byte = (object)Typeob.Byte;
        this.@char = (object)Typeob.Char;
        this.@decimal = (object)Typeob.Decimal;
        this.@double = (object)Typeob.Double;
        this.@float = (object)Typeob.Single;
        this.@int = (object)Typeob.Int32;
        this.@long = (object)Typeob.Int64;
        this.@sbyte = (object)Typeob.SByte;
        this.@short = (object)Typeob.Int16;
        this.@void = (object)Typeob.Void;
        this.@uint = (object)Typeob.UInt32;
        this.@ulong = (object)Typeob.UInt64;
        this.@ushort = (object)Typeob.UInt16;
    }

    private LenientArrayPrototype arrayPrototype
    {
        get
        {
            if (this.arrayPrototypeField == null)
                this.arrayPrototypeField = new LenientArrayPrototype((FunctionPrototype)this.functionPrototype,
                    (ObjectPrototype)this.objectPrototype);
            return this.arrayPrototypeField;
        }
    }

    private LenientFunctionPrototype functionPrototype
    {
        get
        {
            if (this.functionPrototypeField == null)
            {
                object objectPrototype = (object)this.objectPrototype;
            }

            return this.functionPrototypeField;
        }
    }

    private LenientObjectPrototype objectPrototype
    {
        get
        {
            if (this.objectPrototypeField == null)
            {
                LenientObjectPrototype parent = this.objectPrototypeField = new LenientObjectPrototype(this.engine);
                LenientFunctionPrototype funcprot =
                    this.functionPrototypeField = new LenientFunctionPrototype((ScriptObject)parent);
                parent.Initialize(funcprot);
                JSObject jsObject = new JSObject((ScriptObject)parent, false);
                jsObject.AddField("constructor").SetValue((object)jsObject, (object)funcprot);
                funcprot.proto = (object)jsObject;
            }

            return this.objectPrototypeField;
        }
    }

    internal override ActiveXObjectConstructor originalActiveXObject
    {
        get
        {
            if (this.originalActiveXObjectField == null)
                this.originalActiveXObjectField = new ActiveXObjectConstructor(this.functionPrototype);
            return this.originalActiveXObjectField;
        }
    }

    internal override ArrayConstructor originalArray
    {
        get
        {
            if (this.originalArrayField == null)
                this.originalArrayField = new ArrayConstructor(this.functionPrototype, this.arrayPrototype);
            return this.originalArrayField;
        }
    }

    internal override BooleanConstructor originalBoolean
    {
        get
        {
            if (this.originalBooleanField == null)
                this.originalBooleanField = new BooleanConstructor(this.functionPrototype,
                    new LenientBooleanPrototype(this.functionPrototype, this.objectPrototype));
            return this.originalBooleanField;
        }
    }

    internal override DateConstructor originalDate
    {
        get
        {
            if (this.originalDateField == null)
                this.originalDateField = (DateConstructor)new LenientDateConstructor(this.functionPrototype,
                    new LenientDatePrototype(this.functionPrototype, this.objectPrototype));
            return this.originalDateField;
        }
    }

    internal override ErrorConstructor originalError
    {
        get
        {
            if (this.originalErrorField == null)
                this.originalErrorField = new ErrorConstructor(this.functionPrototype,
                    new LenientErrorPrototype(this.functionPrototype, (ScriptObject)this.objectPrototype, "Error"),
                    (GlobalObject)this);
            return this.originalErrorField;
        }
    }

    internal override EnumeratorConstructor originalEnumerator
    {
        get
        {
            if (this.originalEnumeratorField == null)
                this.originalEnumeratorField = new EnumeratorConstructor(this.functionPrototype,
                    new LenientEnumeratorPrototype(this.functionPrototype, this.objectPrototype));
            return this.originalEnumeratorField;
        }
    }

    internal override ErrorConstructor originalEvalError
    {
        get
        {
            if (this.originalEvalErrorField == null)
                this.originalEvalErrorField = new ErrorConstructor("EvalError", ErrorType.EvalError, this.originalError,
                    (GlobalObject)this);
            return this.originalEvalErrorField;
        }
    }

    internal override FunctionConstructor originalFunction
    {
        get
        {
            if (this.originalFunctionField == null)
                this.originalFunctionField = new FunctionConstructor(this.functionPrototype);
            return this.originalFunctionField;
        }
    }

    internal override NumberConstructor originalNumber
    {
        get
        {
            if (this.originalNumberField == null)
                this.originalNumberField = new NumberConstructor(this.functionPrototype,
                    new LenientNumberPrototype(this.functionPrototype, this.objectPrototype));
            return this.originalNumberField;
        }
    }

    internal override ObjectConstructor originalObject
    {
        get
        {
            if (this.originalObjectField == null)
                this.originalObjectField = new ObjectConstructor(this.functionPrototype, this.objectPrototype);
            return this.originalObjectField;
        }
    }

    internal override ObjectPrototype originalObjectPrototype
    {
        get
        {
            if (this.originalObjectPrototypeField == null)
                this.originalObjectPrototypeField = ObjectPrototype.ob;
            return this.originalObjectPrototypeField;
        }
    }

    internal override ErrorConstructor originalRangeError
    {
        get
        {
            if (this.originalRangeErrorField == null)
                this.originalRangeErrorField = new ErrorConstructor("RangeError", ErrorType.RangeError,
                    this.originalError, (GlobalObject)this);
            return this.originalRangeErrorField;
        }
    }

    internal override ErrorConstructor originalReferenceError
    {
        get
        {
            if (this.originalReferenceErrorField == null)
                this.originalReferenceErrorField = new ErrorConstructor("ReferenceError", ErrorType.ReferenceError,
                    this.originalError, (GlobalObject)this);
            return this.originalReferenceErrorField;
        }
    }

    internal override RegExpConstructor originalRegExp
    {
        get
        {
            if (this.originalRegExpField == null)
                this.originalRegExpField = new RegExpConstructor(this.functionPrototype,
                    new LenientRegExpPrototype(this.functionPrototype, this.objectPrototype), this.arrayPrototype);
            return this.originalRegExpField;
        }
    }

    internal override StringConstructor originalString
    {
        get
        {
            if (this.originalStringField == null)
                this.originalStringField = (StringConstructor)new LenientStringConstructor(this.functionPrototype,
                    new LenientStringPrototype(this.functionPrototype, this.objectPrototype));
            return this.originalStringField;
        }
    }

    internal override ErrorConstructor originalSyntaxError
    {
        get
        {
            if (this.originalSyntaxErrorField == null)
                this.originalSyntaxErrorField = new ErrorConstructor("SyntaxError", ErrorType.SyntaxError,
                    this.originalError, (GlobalObject)this);
            return this.originalSyntaxErrorField;
        }
    }

    internal override ErrorConstructor originalTypeError
    {
        get
        {
            if (this.originalTypeErrorField == null)
                this.originalTypeErrorField = new ErrorConstructor("TypeError", ErrorType.TypeError, this.originalError,
                    (GlobalObject)this);
            return this.originalTypeErrorField;
        }
    }

    internal override ErrorConstructor originalURIError
    {
        get
        {
            if (this.originalURIErrorField == null)
                this.originalURIErrorField = new ErrorConstructor("URIError", ErrorType.URIError, this.originalError,
                    (GlobalObject)this);
            return this.originalURIErrorField;
        }
    }

    internal override VBArrayConstructor originalVBArray
    {
        get
        {
            if (this.originalVBArrayField == null)
                this.originalVBArrayField = new VBArrayConstructor(this.functionPrototype,
                    new LenientVBArrayPrototype(this.functionPrototype, this.objectPrototype));
            return this.originalVBArrayField;
        }
    }

    /// <summary>Gets or sets a prototype-based object that is defined by an <see cref="T:Microsoft.JScript.ActiveXObjectConstructor" /> constructor function object.</summary>
    /// <returns>An <see cref="T:Microsoft.JScript.ActiveXObjectConstructor" /> object.</returns>
    public object ActiveXObject
    {
        get
        {
            if (this.ActiveXObjectField is Missing)
                this.ActiveXObjectField = (object)this.originalActiveXObject;
            return this.ActiveXObjectField;
        }
        set => this.ActiveXObjectField = value;
    }

    /// <summary>Gets or sets a prototype-based object that is defined by an <see cref="T:Microsoft.JScript.ArrayConstructor" /> constructor function object.</summary>
    /// <returns>An <see cref="T:Microsoft.JScript.ArrayConstructor" /> object.</returns>
    public object Array
    {
        get
        {
            if (this.ArrayField is Missing)
                this.ArrayField = (object)this.originalArray;
            return this.ArrayField;
        }
        set => this.ArrayField = value;
    }

    /// <summary>Gets or sets a prototype-based object that is defined by a <see cref="T:Microsoft.JScript.BooleanConstructor" /> constructor function object.</summary>
    /// <returns>A <see cref="T:Microsoft.JScript.BooleanConstructor" /> object.</returns>
    public object Boolean
    {
        get
        {
            if (this.BooleanField is Missing)
                this.BooleanField = (object)this.originalBoolean;
            return this.BooleanField;
        }
        set => this.BooleanField = value;
    }

    /// <summary>Gets or sets a prototype-based object that is defined by a <see cref="T:Microsoft.JScript.LenientDateConstructor" /> constructor function object.</summary>
    /// <returns>A <see cref="T:Microsoft.JScript.LenientDateConstructor" /> object.</returns>
    public object Date
    {
        get
        {
            if (this.DateField is Missing)
                this.DateField = (object)this.originalDate;
            return this.DateField;
        }
        set => this.DateField = value;
    }

    /// <summary>Gets or sets a prototype-based object that is defined by an <see cref="T:Microsoft.JScript.EnumeratorConstructor" /> constructor function object.</summary>
    /// <returns>An <see cref="T:Microsoft.JScript.EnumeratorConstructor" /> object.</returns>
    public object Enumerator
    {
        get
        {
            if (this.EnumeratorField is Missing)
                this.EnumeratorField = (object)this.originalEnumerator;
            return this.EnumeratorField;
        }
        set => this.EnumeratorField = value;
    }

    /// <summary>Gets or sets a prototype-based object that is defined by an <see cref="T:Microsoft.JScript.ErrorConstructor" /> constructor function object.</summary>
    /// <returns>An <see cref="T:Microsoft.JScript.ErrorConstructor" /> object.</returns>
    public object Error
    {
        get
        {
            if (this.ErrorField is Missing)
                this.ErrorField = (object)this.originalError;
            return this.ErrorField;
        }
        set => this.ErrorField = value;
    }

    /// <summary>Gets or sets a prototype-based object that is defined by an <see cref="T:Microsoft.JScript.ErrorConstructor" /> constructor function object, and that is initialized as an <see cref="F:Microsoft.JScript.ErrorType.EvalError" />.</summary>
    /// <returns>An <see cref="T:Microsoft.JScript.ErrorConstructor" /> object.</returns>
    public object EvalError
    {
        get
        {
            if (this.EvalErrorField is Missing)
                this.EvalErrorField = (object)this.originalEvalError;
            return this.EvalErrorField;
        }
        set => this.EvalErrorField = value;
    }

    /// <summary>Gets or sets a prototype-based object that is defined by a <see cref="T:Microsoft.JScript.FunctionConstructor" /> constructor function object.</summary>
    /// <returns>A <see cref="T:Microsoft.JScript.FunctionConstructor" /> object.</returns>
    public object Function
    {
        get
        {
            if (this.FunctionField is Missing)
                this.FunctionField = (object)this.originalFunction;
            return this.FunctionField;
        }
        set => this.FunctionField = value;
    }

    /// <summary>Gets or sets a <see cref="T:Microsoft.JScript.LenientMathObject" /> object.</summary>
    /// <returns>A <see cref="T:Microsoft.JScript.LenientMathObject" /> object.</returns>
    public object Math
    {
        get
        {
            if (this.MathField is Missing)
                this.MathField = (object)new LenientMathObject((ScriptObject)this.objectPrototype,
                    (FunctionPrototype)this.functionPrototype);
            return this.MathField;
        }
        set => this.MathField = value;
    }

    /// <summary>Gets or sets a prototype-based object that is defined by a <see cref="T:Microsoft.JScript.NumberConstructor" /> constructor function object.</summary>
    /// <returns>A <see cref="T:Microsoft.JScript.NumberConstructor" /> object.</returns>
    public object Number
    {
        get
        {
            if (this.NumberField is Missing)
                this.NumberField = (object)this.originalNumber;
            return this.NumberField;
        }
        set => this.NumberField = value;
    }

    /// <summary>Gets or sets a prototype-based object that is defined by an <see cref="T:Microsoft.JScript.ObjectConstructor" /> constructor function object.</summary>
    /// <returns>An <see cref="T:Microsoft.JScript.ObjectConstructor" /> object.</returns>
    public object Object
    {
        get
        {
            if (this.ObjectField is Missing)
                this.ObjectField = (object)this.originalObject;
            return this.ObjectField;
        }
        set => this.ObjectField = value;
    }

    /// <summary>Gets or sets a prototype-based object that is defined by an <see cref="T:Microsoft.JScript.ErrorConstructor" /> constructor function object, and that is initialized as a <see cref="F:Microsoft.JScript.ErrorType.RangeError" />.</summary>
    /// <returns>An <see cref="T:Microsoft.JScript.ErrorConstructor" /> object.</returns>
    public object RangeError
    {
        get
        {
            if (this.RangeErrorField is Missing)
                this.RangeErrorField = (object)this.originalRangeError;
            return this.RangeErrorField;
        }
        set => this.RangeErrorField = value;
    }

    /// <summary>Gets or sets a prototype-based object that is defined by an <see cref="T:Microsoft.JScript.ErrorConstructor" /> constructor function object, and that is initialized as a <see cref="F:Microsoft.JScript.ErrorType.ReferenceError" />.</summary>
    /// <returns>An <see cref="T:Microsoft.JScript.ErrorConstructor" /> object.</returns>
    public object ReferenceError
    {
        get
        {
            if (this.ReferenceErrorField is Missing)
                this.ReferenceErrorField = (object)this.originalReferenceError;
            return this.ReferenceErrorField;
        }
        set => this.ReferenceErrorField = value;
    }

    /// <summary>Gets or sets a prototype-based object that is defined by a <see cref="T:Microsoft.JScript.RegExpConstructor" /> constructor function object.</summary>
    /// <returns>A <see cref="T:Microsoft.JScript.RegExpConstructor" /> object.</returns>
    public object RegExp
    {
        get
        {
            if (this.RegExpField is Missing)
                this.RegExpField = (object)this.originalRegExp;
            return this.RegExpField;
        }
        set => this.RegExpField = value;
    }

    /// <summary>Gets or sets a prototype-based object that is defined by a <see cref="T:Microsoft.JScript.LenientStringConstructor" /> constructor function object.</summary>
    /// <returns>A <see cref="T:Microsoft.JScript.LenientStringConstructor" /> object.</returns>
    public object String
    {
        get
        {
            if (this.StringField is Missing)
                this.StringField = (object)this.originalString;
            return this.StringField;
        }
        set => this.StringField = value;
    }

    /// <summary>Gets or sets a prototype-based object that is defined by an <see cref="T:Microsoft.JScript.ErrorConstructor" /> constructor function object, and that is initialized as a <see cref="F:Microsoft.JScript.ErrorType.SyntaxError" />.</summary>
    /// <returns>An <see cref="T:Microsoft.JScript.ErrorConstructor" /> object.</returns>
    public object SyntaxError
    {
        get
        {
            if (this.SyntaxErrorField is Missing)
                this.SyntaxErrorField = (object)this.originalSyntaxError;
            return this.SyntaxErrorField;
        }
        set => this.SyntaxErrorField = value;
    }

    /// <summary>Gets or sets a prototype-based object that is defined by an <see cref="T:Microsoft.JScript.ErrorConstructor" /> constructor function object, and that is initialized as a <see cref="F:Microsoft.JScript.ErrorType.TypeError" />.</summary>
    /// <returns>An <see cref="T:Microsoft.JScript.ErrorConstructor" /> object.</returns>
    public object TypeError
    {
        get
        {
            if (this.TypeErrorField is Missing)
                this.TypeErrorField = (object)this.originalTypeError;
            return this.TypeErrorField;
        }
        set => this.TypeErrorField = value;
    }

    /// <summary>Gets or sets a prototype-based object that is defined by an <see cref="T:Microsoft.JScript.ErrorConstructor" /> constructor function object, and that is initialized as a <see cref="F:Microsoft.JScript.ErrorType.URIError" />.</summary>
    /// <returns>An <see cref="T:Microsoft.JScript.ErrorConstructor" /> object.</returns>
    public object URIError
    {
        get
        {
            if (this.URIErrorField is Missing)
                this.URIErrorField = (object)this.originalURIError;
            return this.URIErrorField;
        }
        set => this.URIErrorField = value;
    }

    /// <summary>Gets or sets a prototype-based object that is defined by a <see cref="T:Microsoft.JScript.VBArrayConstructor" /> constructor function object.</summary>
    /// <returns>A <see cref="T:Microsoft.JScript.VBArrayConstructor" /> object.</returns>
    public object VBArray
    {
        get
        {
            if (this.VBArrayField is Missing)
                this.VBArrayField = (object)this.originalVBArray;
            return this.VBArrayField;
        }
        set => this.VBArrayField = value;
    }
}