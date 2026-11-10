// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.JSClosureMethod
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Globalization;
using System.Reflection;

#nullable disable
namespace Microsoft.JScript;

internal sealed class JSClosureMethod : JSMethod
{
    internal MethodInfo method;

    internal JSClosureMethod(MethodInfo method)
        : base((object)null)
    {
        this.method = method;
    }

    internal override object Construct(object[] args)
    {
        throw new JScriptException(JSError.InternalError);
    }

    public override MethodAttributes Attributes
    {
        get => this.method.Attributes & ~MethodAttributes.Virtual | MethodAttributes.Static;
    }

    public override Type DeclaringType => this.method.DeclaringType;

    public override ParameterInfo[] GetParameters() => this.method.GetParameters();

    internal override MethodInfo GetMethodInfo(CompilerGlobals compilerGlobals)
    {
        return this.method is JSMethod ? ((JSMethod)this.method).GetMethodInfo(compilerGlobals) : this.method;
    }

    internal override object Invoke(
        object obj,
        object thisob,
        BindingFlags options,
        Binder binder,
        object[] parameters,
        CultureInfo culture)
    {
        if (obj is StackFrame)
            return this.method.Invoke(((StackFrame)((ScriptObject)obj).engine.ScriptObjectStackTop()).closureInstance,
                options, binder, parameters, culture);
        throw new JScriptException(JSError.InternalError);
    }

    public override string Name => this.method.Name;

    public override Type ReturnType => this.method.ReturnType;
}