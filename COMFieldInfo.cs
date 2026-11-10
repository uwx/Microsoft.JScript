// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.COMFieldInfo
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

/// <summary>Enables JScript to communicate with an external COM field. This class is used by an external COM implementation to expose its objects to JScript because JScript needs a managed <see cref="T:System.Reflection.MemberInfo" /> in order to bind to an external object. </summary>
[Guid("CA0F511A-FAF2-4942-B9A8-17D5E46514E8")]
[ComVisible(true)]
public class COMFieldInfo : FieldInfo, MemberInfoInitializer
{
  private COMMemberInfo _comObject;
  private string _name;

  /// <summary>Initializes a new instance of the <see cref="T:Microsoft.JScript.COMFieldInfo" /> class.</summary>
  public COMFieldInfo()
  {
    this._comObject = (COMMemberInfo) null;
    this._name = (string) null;
  }

  /// <summary>Gets the attributes that are associated with this field.</summary>
  /// <returns>
  ///   <see cref="F:System.Reflection.FieldAttributes.Public" />
  /// </returns>
  public override FieldAttributes Attributes => FieldAttributes.Public;

  /// <summary>Gets the type that declares this field.</summary>
  /// <returns>null, because the type that declares this field is an external COM object.</returns>
  public override Type DeclaringType => (Type) null;

  /// <summary>Throws an <see cref="F:Microsoft.JScript.JSError.InternalError" /> exception in all cases.</summary>
  /// <returns>Throws an <see cref="F:Microsoft.JScript.JSError.InternalError" /> exception in all cases.</returns>
  public override RuntimeFieldHandle FieldHandle
  {
    get => throw new JScriptException(JSError.InternalError);
  }

  /// <summary>Gets the type of this field.</summary>
  /// <returns>The type of <see cref="T:System.Object" />.</returns>
  public override Type FieldType => typeof (object);

  /// <summary>Gets the value of the external COM field by using the <see cref="T:Microsoft.JScript.COMMemberInfo" /> that was specified to the <see cref="M:Microsoft.JScript.COMFieldInfo.Initialize(System.String,Microsoft.JScript.COMMemberInfo)" /> method.</summary>
  /// <returns>The value of the external COM field.</returns>
  /// <param name="obj">The argument list to pass to the external COM field. This parameter is ignored.</param>
  public override object GetValue(object obj)
  {
    return this._comObject.GetValue(BindingFlags.Default, (Binder) null, new object[0], (CultureInfo) null);
  }

  /// <summary>Initializes the information that enables JScript to communicate with this external COM field.</summary>
  /// <param name="name">The name of the external COM field that this object represents.</param>
  /// <param name="dispatch">Information that enables JScript to access the external COM field.</param>
  public virtual void Initialize(string name, COMMemberInfo dispatch)
  {
    this._name = name;
    this._comObject = dispatch;
  }

  /// <summary>Gets a managed object that enables JScript to communicate with this external COM field.</summary>
  /// <returns>A managed object that enables JScript to communicate with this external COM field.</returns>
  public COMMemberInfo GetCOMMemberInfo() => this._comObject;

  /// <summary>Gets the type of member that this field is. Identifies the type of class derived from <see cref="T:System.Reflection.MemberInfo" /> that this class derives from.</summary>
  /// <returns>
  /// <see cref="F:System.Reflection.MemberTypes.Field" />, because this class derives from <see cref="T:System.Reflection.FieldInfo" />.</returns>
  public override MemberTypes MemberType => MemberTypes.Field;

  /// <summary>Gets the name of the external COM field that this object represents.</summary>
  /// <returns>The name of the external COM field that this object represents.</returns>
  public override string Name => this._name;

  /// <summary>Gets the class object that was used to obtain this instance through reflection.</summary>
  /// <returns>null, because this instance was not obtained through reflection.</returns>
  public override Type ReflectedType => (Type) null;

  /// <summary>Sets the value of the external COM field by using the <see cref="T:Microsoft.JScript.COMMemberInfo" /> that was specified to the <see cref="M:Microsoft.JScript.COMFieldInfo.Initialize(System.String,Microsoft.JScript.COMMemberInfo)" /> method.</summary>
  /// <param name="obj">The object whose field value will be set. This parameter is ignored.</param>
  /// <param name="value">The new value of the external COM member.</param>
  /// <param name="invokeAttr">A bitwise combination of the enumeration values that control the binding.</param>
  /// <param name="binder">An object that converts the type from actual argument type to formal argument type.</param>
  /// <param name="culture">Provides information about a specific culture or locale. Used to format numbers, dates, and strings correctly.</param>
  public override void SetValue(
    object obj,
    object value,
    BindingFlags invokeAttr,
    Binder binder,
    CultureInfo culture)
  {
    this._comObject.SetValue(value, invokeAttr, binder, new object[0], culture);
  }

  /// <summary>Returns an array that contains the custom attributes that are attached to this field, searching only for attributes of the specified type.</summary>
  /// <returns>An array of <see cref="T:System.Reflection.FieldInfo" /> objects that has zero elements.</returns>
  /// <param name="t">The type of attribute to search for.</param>
  /// <param name="inherit">true to search this member's inheritance hierarchy to find the attributes.</param>
  public override object[] GetCustomAttributes(Type t, bool inherit) => (object[]) new FieldInfo[0];

  /// <summary>Returns an array that contains the custom attributes that are attached to this field.</summary>
  /// <returns>An array of <see cref="T:System.Reflection.FieldInfo" /> objects that has zero elements.</returns>
  /// <param name="inherit">true to search this member's inheritance hierarchy to find the attributes.</param>
  public override object[] GetCustomAttributes(bool inherit) => (object[]) new FieldInfo[0];

  /// <summary>Determines whether the specified attribute type is attached to this field.</summary>
  /// <returns>false in all cases.</returns>
  /// <param name="t">The attribute type to search for.</param>
  /// <param name="inherit">true to search this member's inheritance hierarchy to find the attributes.</param>
  public override bool IsDefined(Type t, bool inherit) => false;
}
