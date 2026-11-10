// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.JSField
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Reflection;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Supports all fields in JScript. This is the base class for all fields that are defined by JScript. This class belongs to the name binding category.</summary>
public abstract class JSField : FieldInfo
{
    /// <summary>Gets the attributes that are associated with this field.</summary>
    /// <returns>
    ///   <see cref="F:System.Reflection.FieldAttributes.PrivateScope" />
    /// </returns>
    public override FieldAttributes Attributes => FieldAttributes.PrivateScope;

    /// <summary>Gets the type that declares this field.</summary>
    /// <returns>null in all cases.</returns>
    public override Type DeclaringType => (Type)null;

    /// <summary>Gets a handle to the internal metadata representation of this field.</summary>
    /// <returns>A handle to the internal metadata representation of this field.</returns>
    public override RuntimeFieldHandle FieldHandle => ((FieldInfo)this.GetMetaData()).FieldHandle;

    /// <summary>Gets the type of this field.</summary>
    /// <returns>The type of <see cref="T:System.Object" />.</returns>
    public override Type FieldType => Typeob.Object;

    /// <summary>Returns an array that contains the custom attributes that are attached to this field, searching only for attributes of the specified type.</summary>
    /// <returns>An array of <see cref="T:System.Reflection.FieldInfo" /> objects that has zero elements.</returns>
    /// <param name="t">The type of attribute to search for.</param>
    /// <param name="inherit">true to search this member's inheritance hierarchy to find the attributes.</param>
    public override object[] GetCustomAttributes(Type t, bool inherit) => (object[])new FieldInfo[0];

    /// <summary>Returns an array that contains the custom attributes that are attached to this field.</summary>
    /// <returns>An array of <see cref="T:System.Reflection.FieldInfo" /> objects that has zero elements.</returns>
    /// <param name="inherit">true to search this member's inheritance hierarchy to find the attributes.</param>
    public override object[] GetCustomAttributes(bool inherit) => (object[])new FieldInfo[0];

    internal virtual object GetMetaData() => throw new JScriptException(JSError.InternalError);

    internal virtual string GetClassFullName() => throw new JScriptException(JSError.InternalError);

    internal virtual PackageScope GetPackage() => throw new JScriptException(JSError.InternalError);

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
    public override string Name => "";

    /// <summary>Gets the class object that was used to obtain this instance through reflection.</summary>
    /// <returns>The type that declares this method.</returns>
    public override Type ReflectedType => this.DeclaringType;
}