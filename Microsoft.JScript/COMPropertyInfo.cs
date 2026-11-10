// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.COMPropertyInfo
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

/// <summary>Enables JScript to communicate with an external COM property. This class is used by an external COM implementation to expose its objects to JScript because JScript needs a managed <see cref="T:System.Reflection.MemberInfo" /> in order to bind to an external object. </summary>
[Guid("6A02951C-B129-4d26-AB92-B9CA19BDCA26")]
[ComVisible(true)]
public sealed class COMPropertyInfo : PropertyInfo, MemberInfoInitializer
{
    private COMMemberInfo _comObject;
    private string _name;

    /// <summary>Initializes a new instance of the <see cref="T:Microsoft.JScript.COMPropertyInfo" /> class.</summary>
    public COMPropertyInfo()
    {
        this._comObject = (COMMemberInfo)null;
        this._name = (string)null;
    }

    /// <summary>Gets the attributes that are associated with this property.</summary>
    /// <returns>
    ///   <see cref="F:System.Reflection.PropertyAttributes.None" />
    /// </returns>
    public override PropertyAttributes Attributes => PropertyAttributes.None;

    /// <summary>Gets a value that indicates whether the property can be read.</summary>
    /// <returns>true in all cases.</returns>
    public override bool CanRead => true;

    /// <summary>Gets a value that indicates whether the property can be written to.</summary>
    /// <returns>true in all cases.</returns>
    public override bool CanWrite => true;

    /// <summary>Gets the type that declares this property.</summary>
    /// <returns>null, because the type that declares this property is an external COM object.</returns>
    public override Type DeclaringType => (Type)null;

    /// <summary>Gets an array whose elements contain the get and set accessors of the external COM property that this object represents.</summary>
    /// <returns>A two-element array of <see cref="T:System.Reflection.MethodInfo" /> objects that contains the get and set methods of this property.</returns>
    /// <param name="nonPublic">true to include non-public accessors.</param>
    public override MethodInfo[] GetAccessors(bool nonPublic)
    {
        return new MethodInfo[2]
        {
            this.GetGetMethod(nonPublic),
            this.GetSetMethod(nonPublic)
        };
    }

    /// <summary>Returns an array that contains the custom attributes that are attached to this property, searching only for attributes of the specified type.</summary>
    /// <returns>An array of <see cref="T:System.Reflection.FieldInfo" /> objects that has zero elements.</returns>
    /// <param name="t">The type of attribute to search for.</param>
    /// <param name="inherit">true to search this member's inheritance hierarchy to find the attributes.</param>
    public override object[] GetCustomAttributes(Type t, bool inherit) => (object[])new FieldInfo[0];

    /// <summary>Returns an array that contains the custom attributes that are attached to this property.</summary>
    /// <returns>An array of <see cref="T:System.Reflection.FieldInfo" /> objects that has zero elements.</returns>
    /// <param name="inherit">true to search this member's inheritance hierarchy to find the attributes.</param>
    public override object[] GetCustomAttributes(bool inherit) => (object[])new FieldInfo[0];

    /// <summary>Returns the get accessor for this property.</summary>
    /// <returns>The get accessor for this property.</returns>
    /// <param name="nonPublic">true to include non-public accessors.</param>
    public override MethodInfo GetGetMethod(bool nonPublic)
    {
        COMGetterMethod getMethod = new COMGetterMethod();
        getMethod.Initialize(this._name, this._comObject);
        return (MethodInfo)getMethod;
    }

    /// <summary>Returns an array of all the index parameters for the property.</summary>
    /// <returns>An array of <see cref="T:System.Reflection.ParameterInfo" /> objects that has zero elements.</returns>
    public override ParameterInfo[] GetIndexParameters() => new ParameterInfo[0];

    /// <summary>Returns the set accessor for this property.</summary>
    /// <returns>The set accessor for this property.</returns>
    /// <param name="nonPublic">true to include non-public accessors.</param>
    public override MethodInfo GetSetMethod(bool nonPublic)
    {
        COMSetterMethod setMethod = new COMSetterMethod();
        setMethod.Initialize(this._name, this._comObject);
        return (MethodInfo)setMethod;
    }

    /// <summary>Gets the value of the external COM property by using the <see cref="T:Microsoft.JScript.COMMemberInfo" /> that was specified to the <see cref="M:Microsoft.JScript.COMPropertyInfo.Initialize(System.String,Microsoft.JScript.COMMemberInfo)" /> method.</summary>
    /// <returns>The value of the external COM property.</returns>
    /// <param name="obj">The object whose property value you want to get. This parameter is ignored.</param>
    /// <param name="invokeAttr">A bitwise combination of the enumeration values that control the binding.</param>
    /// <param name="binder">An object that converts the type from actual argument type to formal argument type.</param>
    /// <param name="index">The argument list to pass to the external COM property.</param>
    /// <param name="culture">Provides information about a specific culture or locale. Used to format numbers, dates, and strings correctly.</param>
    public override object GetValue(
        object obj,
        BindingFlags invokeAttr,
        Binder binder,
        object[] index,
        CultureInfo culture)
    {
        return this._comObject.GetValue(invokeAttr, binder, index != null ? index : new object[0], culture);
    }

    /// <summary>Initializes the information that enables JScript to communicate with this external COM property.</summary>
    /// <param name="name">The name of the external COM property that this object represents.</param>
    /// <param name="dispatch">Information that enables JScript to access the external COM property.</param>
    public void Initialize(string name, COMMemberInfo dispatch)
    {
        this._name = name;
        this._comObject = dispatch;
    }

    /// <summary>Gets a managed object that enables JScript to communicate with this external COM property.</summary>
    /// <returns>A managed object that enables JScript to communicate with this external COM property.</returns>
    public COMMemberInfo GetCOMMemberInfo() => this._comObject;

    /// <summary>Gets the type of member that this property is. Identifies the type of class derived from <see cref="T:System.Reflection.MemberInfo" /> that this class derives from.</summary>
    /// <returns>
    /// <see cref="F:System.Reflection.MemberTypes.Property" />, because this class derives from <see cref="T:System.Reflection.PropertyInfo" />.</returns>
    public override MemberTypes MemberType => MemberTypes.Property;

    /// <summary>Gets the name of the external COM property that this object represents.</summary>
    /// <returns>The name of the external COM property that this object represents.</returns>
    public override string Name => this._name;

    /// <summary>Gets the class object that was used to obtain this instance through reflection.</summary>
    /// <returns>null, because this instance was not obtained through reflection.</returns>
    public override Type ReflectedType => (Type)null;

    /// <summary>Sets the value of the external COM property by using the <see cref="T:Microsoft.JScript.COMMemberInfo" /> that was specified to the <see cref="M:Microsoft.JScript.COMPropertyInfo.Initialize(System.String,Microsoft.JScript.COMMemberInfo)" /> method.</summary>
    /// <param name="obj">The object whose property value will be set. This parameter is ignored.</param>
    /// <param name="value">The new value of the external COM member.</param>
    /// <param name="invokeAttr">A bitwise combination of the enumeration values that control the binding.</param>
    /// <param name="binder">An object that converts the type from actual argument type to formal argument type.</param>
    /// <param name="index">The argument list to pass to the external COM property.</param>
    /// <param name="culture">Provides information about a specific culture or locale. Used to format numbers, dates, and strings correctly.</param>
    public override void SetValue(
        object obj,
        object value,
        BindingFlags invokeAttr,
        Binder binder,
        object[] index,
        CultureInfo culture)
    {
        this._comObject.SetValue(value, invokeAttr, binder, index != null ? index : new object[0], culture);
    }

    /// <summary>Gets the type of this property.</summary>
    /// <returns>The type of <see cref="T:System.Object" />.</returns>
    public override Type PropertyType => typeof(object);

    /// <summary>Determines whether the specified attribute type is attached to this property.</summary>
    /// <returns>false in all cases.</returns>
    /// <param name="t">The attribute type to search for.</param>
    /// <param name="inherit">true to search this member's inheritance hierarchy to find the attributes.</param>
    public override bool IsDefined(Type t, bool inherit) => false;
}