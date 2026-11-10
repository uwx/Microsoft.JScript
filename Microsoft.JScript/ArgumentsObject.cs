// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.ArgumentsObject
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System.Diagnostics;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Represents the information that is used to call a function. This includes the function, the caller, the arguments, and information about the stack frame. This class belongs to the objects and functions category.</summary>
public sealed class ArgumentsObject : JSObject
{
    private object[] arguments;
    private string[] formal_names;
    private ScriptObject scope;

    /// <summary>A closure function that represents the function that is being called.</summary>
    public object callee;

    /// <summary>The calling scope information from the stack frame.</summary>
    public object caller;

    /// <summary>The number of arguments.</summary>
    public object length;

    internal ArgumentsObject(
        ScriptObject parent,
        object[] arguments,
        FunctionObject function,
        Closure callee,
        ScriptObject scope,
        ArgumentsObject caller)
        : base(parent)
    {
        this.arguments = arguments;
        this.formal_names = function.formal_parameters;
        this.scope = scope;
        this.callee = (object)callee;
        this.caller = (object)caller;
        this.length = (object)arguments.Length;
        this.noExpando = false;
    }

    internal override object GetValueAtIndex(uint index)
    {
        return (long)index < (long)this.arguments.Length ? this.arguments[(int)index] : base.GetValueAtIndex(index);
    }

    [DebuggerStepThrough]
    [DebuggerHidden]
    internal override object GetMemberValue(string name)
    {
        long index = ArrayObject.Array_index_for(name);
        return index < 0L ? base.GetMemberValue(name) : this.GetValueAtIndex((uint)index);
    }

    internal override void SetValueAtIndex(uint index, object value)
    {
        if ((long)index < (long)this.arguments.Length)
            this.arguments[(int)index] = value;
        else
            base.SetValueAtIndex(index, value);
    }

    internal object[] ToArray() => this.arguments;
}