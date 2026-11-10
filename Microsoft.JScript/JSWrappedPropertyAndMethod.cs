// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.JSWrappedPropertyAndMethod
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Globalization;
using System.Reflection;

#nullable disable
namespace Microsoft.JScript;

internal class JSWrappedPropertyAndMethod : JSWrappedProperty
{
    protected MethodInfo method;
    private ParameterInfo[] parameters;

    internal JSWrappedPropertyAndMethod(PropertyInfo property, MethodInfo method, object obj)
        : base(property, obj)
    {
        this.method = method;
        this.parameters = method.GetParameters();
    }

    private object[] CheckArguments(object[] arguments)
    {
        if (arguments == null)
            return arguments;
        int length1 = arguments.Length;
        int length2 = this.parameters.Length;
        if (length1 >= length2)
            return arguments;
        object[] target = new object[length2];
        ArrayObject.Copy(arguments, target, length1);
        for (int index = length1; index < length2; ++index)
            target[index] = Type.Missing;
        return target;
    }

    internal object Invoke(
        object obj,
        BindingFlags options,
        Binder binder,
        object[] parameters,
        CultureInfo culture)
    {
        parameters = this.CheckArguments(parameters);
        if (this.obj != null && (object)(this.obj as Type) == null)
            obj = this.obj;
        return this.method.Invoke(obj, options, binder, parameters, culture);
    }

    public MethodInfo Method => this.method;
}