// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.JSMethodInfo
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

/// <summary>Wraps a <see cref="T:System.Reflection.MethodInfo" /> object. </summary>
public sealed class JSMethodInfo : MethodInfo
{
    internal MethodInfo method;
    private MethodAttributes methAttributes;
    private string name;
    private Type declaringType;
    private ParameterInfo[] parameters;
    private object[] attributes;
    private MethodInvoker methodInvoker;

    internal JSMethodInfo(MethodInfo method)
    {
        this.method = method;
        this.methAttributes = method.Attributes;
    }

    /// <summary>Gets the attributes that are associated with this method.</summary>
    /// <returns>A bitwise combination of the enumeration values.</returns>
    public override MethodAttributes Attributes => this.methAttributes;

    /// <summary>Gets the type that declares this method.</summary>
    /// <returns>The Type object for the class that declares this member.</returns>
    public override Type DeclaringType
    {
        get
        {
            Type declaringType = this.declaringType;
            if (declaringType == (Type)null)
                this.declaringType = declaringType = this.method.DeclaringType;
            return declaringType;
        }
    }

    /// <summary>Returns the <see cref="T:System.Reflection.MethodInfo" /> object for the method of the direct or indirect base class in which this method was first declared.</summary>
    /// <returns>The first implementation of this method in the hierarchy.</returns>
    public override MethodInfo GetBaseDefinition() => this.method.GetBaseDefinition();

    /// <summary>Returns an array that contains the custom attributes that are attached to this method.</summary>
    /// <returns>An array that contains the custom attributes that are attached to this method, or, if there are none, an array that has zero elements.</returns>
    /// <param name="inherit">true to search this member's inheritance hierarchy to find the attributes.</param>
    public sealed override object[] GetCustomAttributes(bool inherit)
    {
        return this.attributes ?? (this.attributes = this.method.GetCustomAttributes(true));
    }

    /// <summary>Returns an array that contains the custom attributes that are attached to this method, searching only for attributes of the specified type.</summary>
    /// <returns>An array that contains the custom attributes that are attached to this method, or, if there are none, an array that has zero elements.</returns>
    /// <param name="type">The type of attribute to search for.</param>
    /// <param name="inherit">true to search this member's inheritance hierarchy to find the attributes.</param>
    public sealed override object[] GetCustomAttributes(Type type, bool inherit)
    {
        return type != typeof(JSFunctionAttribute)
            ? (object[])null
            : this.attributes ??
              (this.attributes = CustomAttribute.GetCustomAttributes((MemberInfo)this.method, type, true));
    }

    /// <summary>Returns information about the implementation of this method.</summary>
    /// <returns>A bitwise combination of the enumeration values.</returns>
    public override MethodImplAttributes GetMethodImplementationFlags()
    {
        return this.method.GetMethodImplementationFlags();
    }

    /// <summary>Gets the parameters of this method.</summary>
    /// <returns>An array that contains information that matches the signature of this method.</returns>
    public override ParameterInfo[] GetParameters()
    {
        ParameterInfo[] parameters1 = this.parameters;
        if (parameters1 != null)
            return parameters1;
        ParameterInfo[] parameters2 = this.method.GetParameters();
        int index = 0;
        for (int length = parameters2.Length; index < length; ++index)
            parameters2[index] = (ParameterInfo)new JSParameterInfo(parameters2[index]);
        return this.parameters = parameters2;
    }

    /// <summary>Invokes the method, using the specified information.</summary>
    /// <returns>The return value of the call to the method.</returns>
    /// <param name="obj">The object to call the method on.</param>
    /// <param name="options">A bitwise combination of the enumeration values that control the binding.</param>
    /// <param name="binder">An object that converts the type from actual argument type to formal argument type.</param>
    /// <param name="parameters">The argument list to pass to the method.</param>
    /// <param name="culture">Provides information about a specific culture or locale. Used to format numbers, dates, and strings correctly.</param>
    [DebuggerStepThrough]
    [DebuggerHidden]
    public override object Invoke(
        object obj,
        BindingFlags options,
        Binder binder,
        object[] parameters,
        CultureInfo culture)
    {
        MethodInfo executionContext = TypeReferences.ToExecutionContext(this.method);
        if (binder != null)
        {
            try
            {
                return executionContext.Invoke(obj, options, binder, parameters, culture);
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
        }
        else
        {
            MethodInvoker methodInvoker = this.methodInvoker;
            if (methodInvoker == null)
            {
                this.methodInvoker = methodInvoker = MethodInvoker.GetInvokerFor(executionContext);
                if (methodInvoker == null)
                {
                    try
                    {
                        return executionContext.Invoke(obj, options, binder, parameters, culture);
                    }
                    catch (TargetInvocationException ex)
                    {
                        throw ex.InnerException;
                    }
                }
            }

            return methodInvoker.Invoke(obj, parameters);
        }
    }

    /// <summary>Determines whether the specified attribute type is attached to this method.</summary>
    /// <returns>true if one or more instances of the specified attribute <paramref name="type" /> is attached to this method; otherwise, false.</returns>
    /// <param name="type">The attribute type to search for.</param>
    /// <param name="inherit">true to search this member's inheritance hierarchy to find the attributes.</param>
    public sealed override bool IsDefined(Type type, bool inherit)
    {
        object[] objArray = this.attributes;
        if (objArray == null)
            this.attributes = objArray = CustomAttribute.GetCustomAttributes((MemberInfo)this.method, type, true);
        return objArray.Length != 0;
    }

    /// <summary>Gets the type of member that this method is. Identifies the type of class derived from <see cref="T:System.Reflection.MemberInfo" /> that this class derives from.</summary>
    /// <returns>
    /// <see cref="F:System.Reflection.MemberTypes.Method" />, because this class derives from <see cref="T:System.Reflection.MethodInfo" />.</returns>
    public override MemberTypes MemberType => MemberTypes.Method;

    /// <summary>Gets a handle to the internal metadata representation of this method.</summary>
    /// <returns>A handle to the internal metadata representation of this method.</returns>
    public override RuntimeMethodHandle MethodHandle => this.method.MethodHandle;

    /// <summary>Gets the name of this method.</summary>
    /// <returns>The name of this method.</returns>
    public override string Name
    {
        get
        {
            string name = this.name;
            if (name == null)
                this.name = name = this.method.Name;
            return name;
        }
    }

    /// <summary>Gets the class object that was used to obtain this instance through reflection.</summary>
    /// <returns>The Type object that was used to obtain this method.</returns>
    public override Type ReflectedType => this.method.ReflectedType;

    /// <summary>Gets the type of the return value of this method.</summary>
    /// <returns>The type of the return value of this method.</returns>
    public override Type ReturnType => this.method.ReturnType;

    /// <summary>Gets the custom attributes for the type of the return value of this method.</summary>
    /// <returns>The custom attributes for the type of the return value of this method.</returns>
    public override ICustomAttributeProvider ReturnTypeCustomAttributes
    {
        get => this.method.ReturnTypeCustomAttributes;
    }

    /// <summary>Returns a string that represents the method.</summary>
    /// <returns>A string that represents the method.</returns>
    public override string ToString() => this.method.ToString();
}