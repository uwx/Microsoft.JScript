// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.JSFieldInfo
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Globalization;
using System.Reflection;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Wraps a <see cref="T:System.Reflection.FieldInfo" /> object. </summary>
public sealed class JSFieldInfo : FieldInfo
{
    internal FieldInfo field;
    private FieldAttributes attributes;
    private Type declaringType;
    private Type fieldType;
    private FieldAccessor fieldAccessor;

    internal JSFieldInfo(FieldInfo field)
    {
        this.field = field;
        this.attributes = field.Attributes;
    }

    /// <summary>Gets the attributes that are associated with this field.</summary>
    /// <returns>A bitwise combination of the enumeration values.</returns>
    public override FieldAttributes Attributes => this.attributes;

    /// <summary>Gets the type that declares this field.</summary>
    /// <returns>The Type object for the class that declares this member.</returns>
    public override Type DeclaringType
    {
        get
        {
            Type declaringType = this.declaringType;
            if (declaringType == (Type)null)
                this.declaringType = declaringType = this.field.DeclaringType;
            return declaringType;
        }
    }

    /// <summary>Gets a handle to the internal metadata representation of this field.</summary>
    /// <returns>A handle to the internal metadata representation of this field.</returns>
    public override RuntimeFieldHandle FieldHandle => this.field.FieldHandle;

    /// <summary>Gets the type of this field.</summary>
    /// <returns>The type of this field.</returns>
    public override Type FieldType
    {
        get
        {
            Type fieldType = this.fieldType;
            if (fieldType == (Type)null)
                this.fieldType = fieldType = this.field.FieldType;
            return fieldType;
        }
    }

    /// <summary>Returns an array that contains the custom attributes that are attached to this field, searching only for attributes of the specified type.</summary>
    /// <returns>An array of <see cref="T:System.Reflection.FieldInfo" /> objects that has zero elements.</returns>
    /// <param name="t">The type of attribute to search for.</param>
    /// <param name="inherit">true to search this member's inheritance hierarchy to find the attributes.</param>
    public override object[] GetCustomAttributes(Type t, bool inherit) => (object[])new FieldInfo[0];

    /// <summary>Returns an array that contains the custom attributes that are attached to this field.</summary>
    /// <returns>An array of <see cref="T:System.Reflection.FieldInfo" /> objects that has zero elements.</returns>
    /// <param name="inherit">true to search this member's inheritance hierarchy to find the attributes.</param>
    public override object[] GetCustomAttributes(bool inherit) => (object[])new FieldInfo[0];

    /// <summary>Gets the value of the field.</summary>
    /// <returns>The value of the field.</returns>
    /// <param name="obj">The object whose field value will be returned.</param>
    public override object GetValue(object obj)
    {
        FieldAccessor fieldAccessor = this.fieldAccessor;
        if (fieldAccessor == null)
            this.fieldAccessor =
                fieldAccessor = FieldAccessor.GetAccessorFor(TypeReferences.ToExecutionContext(this.field));
        return fieldAccessor.GetValue(obj);
    }

    /// <summary>Determines whether the specified attribute type is attached to this field.</summary>
    /// <returns>false in all cases.</returns>
    /// <param name="type">The attribute type to search for.</param>
    /// <param name="inherit">true to search this member's inheritance hierarchy to find the attributes.</param>
    public override bool IsDefined(Type type, bool inherit) => false;

    /// <summary>Gets the type of member that this field is. Identifies the type of class derived from <see cref="T:System.Reflection.MemberInfo" /> that this class derives from.</summary>
    /// <returns>
    /// <see cref="F:System.Reflection.MemberTypes.Field" />, because this class derives from <see cref="T:System.Reflection.FieldInfo" />.</returns>
    public override MemberTypes MemberType => MemberTypes.Field;

    /// <summary>Gets the name of this field.</summary>
    /// <returns>The name of this field.</returns>
    public override string Name => this.field.Name;

    /// <summary>Gets the class object that was used to obtain this instance through reflection.</summary>
    /// <returns>The Type object that was used to obtain this method.</returns>
    public override Type ReflectedType => this.field.ReflectedType;

    /// <summary>Sets the value of the field, using the specified value.</summary>
    /// <param name="obj">The object whose field value will be set.</param>
    /// <param name="value">The value to assign to the field.</param>
    public new void SetValue(object obj, object value)
    {
        if ((this.attributes & FieldAttributes.InitOnly) != FieldAttributes.PrivateScope)
            throw new JScriptException(JSError.AssignmentToReadOnly);
        this.SetValue(obj, value, BindingFlags.SetField, (Binder)null, (CultureInfo)null);
    }

    /// <summary>Sets the value of the field, using the specified value, culture, and binding information.</summary>
    /// <param name="obj">The object whose field value will be set.</param>
    /// <param name="value">The value to assign to the field.</param>
    /// <param name="invokeAttr">A bitwise combination of the enumeration values that control the binding.</param>
    /// <param name="binder">A set of properties that enables the binding, coercion of argument types, and invocation of members through reflection.</param>
    /// <param name="culture">Provides information about a specific culture or locale. Used to format numbers, dates, and strings correctly.</param>
    public override void SetValue(
        object obj,
        object value,
        BindingFlags invokeAttr,
        Binder binder,
        CultureInfo culture)
    {
        FieldAccessor fieldAccessor = this.fieldAccessor;
        if (fieldAccessor == null)
            this.fieldAccessor = fieldAccessor = FieldAccessor.GetAccessorFor(this.field);
        fieldAccessor.SetValue(obj, value);
    }
}