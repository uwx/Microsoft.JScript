// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.ScriptFunction
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Represents a prototype-based object that is defined by a constructor function. Most of the constructor function classes, such as <see cref="T:Microsoft.JScript.DateConstructor" />, <see cref="T:Microsoft.JScript.NumberConstructor" />, <see cref="T:Microsoft.JScript.StringConstructor" />, and <see cref="T:Microsoft.JScript.ObjectConstructor" />, inherit from this class. This class belongs to the objects and functions category.</summary>
public abstract class ScriptFunction : JSObject
{
    /// <summary>Stores the number of elements that are in the <paramref name="args" /> parameter of the <see cref="M:Microsoft.JScript.ScriptFunction.CreateInstance(System.Object[])" /> method.</summary>
    protected int ilength;

    internal string name;
    internal object proto;

    internal ScriptFunction(ScriptObject parent)
        : base(parent)
    {
        this.ilength = 0;
        this.name = "Function.prototype";
        this.proto = (object)Missing.Value;
    }

    /// <summary>Initializes a new instance of the <see cref="T:Microsoft.JScript.ScriptFunction" /> class.</summary>
    /// <param name="parent">The parent prototype of the object that this function constructs.</param>
    /// <param name="name">The name of the object that this function constructs.</param>
    protected ScriptFunction(ScriptObject parent, string name)
        : base(parent, typeof(ScriptFunction))
    {
        this.ilength = 0;
        this.name = name;
        this.proto = (object)new JSPrototypeObject(parent.GetParent(), this);
    }

    internal ScriptFunction(ScriptObject parent, string name, int length)
        : base(parent)
    {
        this.ilength = length;
        this.name = name;
        this.proto = (object)new JSPrototypeObject(parent.GetParent(), this);
    }

    [DebuggerStepThrough]
    [DebuggerHidden]
    internal abstract object Call(object[] args, object thisob);

    internal virtual object Call(object[] args, object thisob, Binder binder, CultureInfo culture)
    {
        return this.Call(args, thisob);
    }

    internal virtual object Call(
        object[] args,
        object thisob,
        ScriptObject enclosing_scope,
        Closure calleeClosure,
        Binder binder,
        CultureInfo culture)
    {
        return this.Call(args, thisob);
    }

    [DebuggerStepThrough]
    [DebuggerHidden]
    internal virtual object Construct(object[] args)
    {
        JSObject thisob = new JSObject((ScriptObject)null, false);
        thisob.SetParent(this.GetPrototypeForConstructedObject());
        object obj = this.Call(args, (object)thisob);
        return obj is ScriptObject ||
               this is BuiltinFunction && ((BuiltinFunction)this).method.Name.Equals("CreateInstance")
            ? obj
            : (object)thisob;
    }

    /// <summary>Creates an instance of the prototype-based object, passing in the specified arguments.</summary>
    /// <returns>An instance of the object.</returns>
    /// <param name="args">The arguments to pass to the constructor.</param>
    [DebuggerStepThrough]
    [DebuggerHidden]
    [JSFunction(JSFunctionAttributeEnum.HasVarArgs)]
    public object CreateInstance(params object[] args) => this.Construct(args);

    internal override string GetClassName() => "Function";

    internal virtual int GetNumberOfFormalParameters() => this.ilength;

    /// <summary>Gets the prototype object for this constructor function.</summary>
    /// <returns>The prototype object for this constructor function.</returns>
    protected ScriptObject GetPrototypeForConstructedObject()
    {
        object proto = this.proto;
        switch (proto)
        {
            case JSObject _:
                return (ScriptObject)proto;
            case ClassScope _:
                return (ScriptObject)proto;
            default:
                return this.GetParent().GetParent();
        }
    }

    internal virtual bool HasInstance(object ob)
    {
        if (!(ob is JSObject))
            return false;
        object proto = this.proto;
        if (!(proto is ScriptObject))
            throw new JScriptException(JSError.InvalidPrototype);
        ScriptObject parent = ((ScriptObject)ob).GetParent();
        ScriptObject scriptObject = (ScriptObject)proto;
        for (; parent != null; parent = parent.GetParent())
        {
            if (parent == scriptObject)
                return true;
            if (parent is WithObject)
            {
                object containedObject = ((WithObject)parent).contained_object;
                if (containedObject == scriptObject && containedObject is ClassScope)
                    return true;
            }
        }

        return false;
    }

    /// <summary>Invokes a method of the prototype-based object, passing in the specified arguments.</summary>
    /// <returns>The result of the call to the method.</returns>
    /// <param name="thisob">The object that this method is acting upon.</param>
    /// <param name="args">The arguments to pass to the method.</param>
    [DebuggerStepThrough]
    [DebuggerHidden]
    [JSFunction(JSFunctionAttributeEnum.HasThisObject | JSFunctionAttributeEnum.HasVarArgs)]
    public object Invoke(object thisob, params object[] args) => this.Call(args, thisob);

    /// <summary>Invokes the specified member.</summary>
    /// <returns>The return value of the call to the member.</returns>
    /// <param name="name">The name of the member to invoke.</param>
    /// <param name="invokeAttr">A bitwise combination of the enumeration values that specifies how the search for members is conducted by reflection.</param>
    /// <param name="binder">An object that performs type conversion from actual argument type to formal argument type.</param>
    /// <param name="target">The object on which to invoke the member. This must be the current script object.</param>
    /// <param name="args">An array of objects that contains the number, order, and type of the parameters of the member to be invoked. This is an empty array if there are no parameters.</param>
    /// <param name="modifiers">An array that has the same length as the <paramref name="args" /> parameter. It represents the invoked member's argument attributes in the metadata.</param>
    /// <param name="culture">Provides information about a specific culture or locale. Used to format numbers, dates, and strings correctly.</param>
    /// <param name="namedParameters">An array of parameters.</param>
    [DebuggerStepThrough]
    [DebuggerHidden]
    public override object InvokeMember(
        string name,
        BindingFlags invokeAttr,
        Binder binder,
        object target,
        object[] args,
        ParameterModifier[] modifiers,
        CultureInfo culture,
        string[] namedParameters)
    {
        if (target != this)
            throw new TargetException();
        string str = "this";
        if (name.Equals("[DISPID=0]"))
        {
            name = string.Empty;
            if (namedParameters != null)
                str = "[DISPID=-613]";
        }

        if (name == null || name == string.Empty)
        {
            if ((invokeAttr & BindingFlags.CreateInstance) != BindingFlags.Default)
            {
                if ((invokeAttr & (BindingFlags.InvokeMethod | BindingFlags.GetField | BindingFlags.SetField |
                                   BindingFlags.GetProperty | BindingFlags.SetProperty |
                                   BindingFlags.PutDispProperty)) != BindingFlags.Default)
                    throw new ArgumentException();
                return this.Construct(args);
            }

            if ((invokeAttr & BindingFlags.InvokeMethod) != BindingFlags.Default)
            {
                object thisob = (object)null;
                if (namedParameters != null)
                {
                    int num = Array.IndexOf<string>(namedParameters, str);
                    if (num == 0)
                    {
                        thisob = args[0];
                        int n = args.Length - 1;
                        object[] target1 = new object[n];
                        ArrayObject.Copy(args, 1, target1, 0, n);
                        args = target1;
                    }

                    if (num != 0 || namedParameters.Length != 1)
                        throw new ArgumentException();
                }

                if (args.Length != 0 || (invokeAttr & (BindingFlags.GetField | BindingFlags.GetProperty)) ==
                    BindingFlags.Default)
                    return this.Call(args, thisob, binder, culture);
            }
        }

        return base.InvokeMember(name, invokeAttr, binder, target, args, modifiers, culture, namedParameters);
    }

    /// <summary>Gets or sets the number of elements that are in the <paramref name="args" /> parameter of the <see cref="M:Microsoft.JScript.ScriptFunction.CreateInstance(System.Object[])" /> method.</summary>
    /// <returns>The number of elements that are in the <paramref name="args" /> parameter of the <see cref="M:Microsoft.JScript.ScriptFunction.CreateInstance(System.Object[])" /> method.</returns>
    public virtual int length
    {
        get => this.ilength;
        set { }
    }

    /// <summary>Returns a string representation of the constructor function.</summary>
    /// <returns>A string that contains the name of the constructor function and a placeholder for the code.</returns>
    public override string ToString() => $"function {this.name}() {{\n    [native code]\n}}";

    /// <summary>Gets or sets the prototype object for this constructor function.</summary>
    /// <returns>The prototype object for this constructor function.</returns>
    public object prototype
    {
        get => this.proto;
        set
        {
            if (this.noExpando)
                return;
            this.proto = value;
        }
    }
}