// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.COMMethodInfo
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Enables JScript to communicate with an external COM method. This class is used by an external COM implementation to expose its objects to JScript because JScript needs a managed <see cref="T:System.Reflection.MemberInfo" /> in order to bind to an external object. </summary>
[Guid("C7B9C313-2FD4-4384-8571-7ABC08BD17E5")]
[ComVisible(true)]
public class COMMethodInfo : JSMethod, MemberInfoInitializer
{
    /// <summary>Stores an array of <see cref="T:System.Reflection.ParameterInfo" /> objects that has zero elements.</summary>
    protected static readonly ParameterInfo[] EmptyParams = new ParameterInfo[0];

    /// <summary>Stores a managed object that enables JScript to communicate with this external COM method.</summary>
    protected COMMemberInfo _comObject;

    /// <summary>Stores the name of the external COM method that this object represents.</summary>
    protected string _name;

    /// <summary>Initializes a new instance of the <see cref="T:Microsoft.JScript.COMMethodInfo" /> class.</summary>
    public COMMethodInfo()
        : base((object)null)
    {
        this._comObject = (COMMemberInfo)null;
        this._name = (string)null;
    }

    /// <summary>Gets the attributes that are associated with this method.</summary>
    /// <returns>
    ///   <see cref="F:System.Reflection.MethodAttributes.Public" />
    /// </returns>
    public override MethodAttributes Attributes => MethodAttributes.Public;

    /// <summary>Initializes the information that enables JScript to communicate with this external COM method.</summary>
    /// <param name="name">The name of the external COM method that this object represents.</param>
    /// <param name="dispatch">Information that enables JScript to access the external COM method.</param>
    public virtual void Initialize(string name, COMMemberInfo dispatch)
    {
        this._name = name;
        this._comObject = dispatch;
    }

    /// <summary>Gets a managed object that enables JScript to communicate with this external COM method.</summary>
    /// <returns>A managed object that enables JScript to communicate with this external COM method.</returns>
    public COMMemberInfo GetCOMMemberInfo() => this._comObject;

    /// <summary>Calls the external COM method by using the <see cref="T:Microsoft.JScript.COMMemberInfo" /> that was specified to the <see cref="M:Microsoft.JScript.COMMethodInfo.Initialize(System.String,Microsoft.JScript.COMMemberInfo)" /> method.</summary>
    /// <returns>The return value of the call to the external COM method.</returns>
    /// <param name="obj">The object to call the method on. This parameter is ignored.</param>
    /// <param name="invokeAttr">A bitwise combination of the enumeration values that control the binding.</param>
    /// <param name="binder">An object that converts the type from actual argument type to formal argument type.</param>
    /// <param name="parameters">The argument list to pass to the external COM member.</param>
    /// <param name="culture">Provides information about a specific culture or locale. Used to format numbers, dates, and strings correctly.</param>
    public override object Invoke(
        object obj,
        BindingFlags invokeAttr,
        Binder binder,
        object[] parameters,
        CultureInfo culture)
    {
        return this._comObject.Call(invokeAttr, binder, parameters != null ? parameters : new object[0], culture);
    }

    /// <summary>Gets the type that declares this method.</summary>
    /// <returns>null, because the type that declares this method is an external COM object.</returns>
    public override Type DeclaringType => (Type)null;

    /// <summary>Returns the <see cref="T:System.Reflection.MethodInfo" /> object for the method on the direct or indirect base class in which this method was first declared.</summary>
    /// <returns>this in all cases.</returns>
    public override MethodInfo GetBaseDefinition() => (MethodInfo)this;

    /// <summary>Returns information about the implementation of this method.</summary>
    /// <returns>
    /// <see cref="F:System.Reflection.MethodImplAttributes.Managed" /> and <see cref="F:System.Reflection.MethodImplAttributes.IL" />.</returns>
    public override MethodImplAttributes GetMethodImplementationFlags() => MethodImplAttributes.IL;

    /// <summary>Gets the parameters of this external COM method.</summary>
    /// <returns>An array of <see cref="T:System.Reflection.ParameterInfo" /> objects that has zero elements.</returns>
    public override ParameterInfo[] GetParameters() => COMMethodInfo.EmptyParams;

    /// <summary>Gets the type of member that this method is. Identifies the type of class derived from <see cref="T:System.Reflection.MemberInfo" /> that this class derives from.</summary>
    /// <returns>
    /// <see cref="F:System.Reflection.MemberTypes.Method" />, because this class derives from <see cref="T:System.Reflection.MethodInfo" />.</returns>
    public override MemberTypes MemberType => MemberTypes.Method;

    /// <summary>Throws an <see cref="F:Microsoft.JScript.JSError.InternalError" /> exception in all cases.</summary>
    /// <returns>Throws an <see cref="F:Microsoft.JScript.JSError.InternalError" /> exception in all cases.</returns>
    public override RuntimeMethodHandle MethodHandle
    {
        get => throw new JScriptException(JSError.InternalError);
    }

    /// <summary>Gets the name of the external COM method that this object represents.</summary>
    /// <returns>The name of the external COM method that this object represents.</returns>
    public override string Name => this._name;

    /// <summary>Gets the class object that was used to obtain this instance through reflection.</summary>
    /// <returns>null, because this instance was not obtained through reflection.</returns>
    public override Type ReflectedType => (Type)null;

    /// <summary>Gets the type of the return value of this method.</summary>
    /// <returns>null in all cases.</returns>
    public override Type ReturnType => (Type)null;

    /// <summary>Gets the custom attributes for the type of the return value of this method.</summary>
    /// <returns>null in all cases.</returns>
    public override ICustomAttributeProvider ReturnTypeCustomAttributes
    {
        get => (ICustomAttributeProvider)null;
    }

    internal override object Construct(object[] args)
    {
        return this._comObject.Call(BindingFlags.CreateInstance, (Binder)null, args != null ? args : new object[0],
            (CultureInfo)null);
    }

    internal override MethodInfo GetMethodInfo(CompilerGlobals compilerGlobals) => (MethodInfo)null;

    internal override object Invoke(
        object obj,
        object thisob,
        BindingFlags options,
        Binder binder,
        object[] parameters,
        CultureInfo culture)
    {
        return this.Invoke(thisob, options, binder, parameters, culture);
    }

    /// <summary>Returns a string that represents the current method.</summary>
    /// <returns>An empty string.</returns>
    public override string ToString() => "";
}