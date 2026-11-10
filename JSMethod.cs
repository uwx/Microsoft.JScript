// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.JSMethod
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Represents a JScript method. This class belongs to the name binding category.</summary>
[Guid("561AC104-8869-4368-902F-4E0D7DDEDDDD")]
[ComVisible(true)]
public abstract class JSMethod : MethodInfo
{
  internal object obj;

  internal JSMethod(object obj) => this.obj = obj;

  internal abstract object Construct(object[] args);

  /// <summary>Returns the <see cref="T:System.Reflection.MethodInfo" /> object for the method on the direct or indirect base class in which this method was first declared.</summary>
  /// <returns>this in all cases.</returns>
  public override MethodInfo GetBaseDefinition() => (MethodInfo) this;

  internal virtual string GetClassFullName()
  {
    return this.obj is ClassScope ? ((ClassScope) this.obj).GetFullName() : throw new JScriptException(JSError.InternalError);
  }

  /// <summary>Returns an array that contains the custom attributes that are attached to this method, searching only for attributes of the specified type.</summary>
  /// <returns>An array of <see cref="T:System.Object" /> objects that has zero elements.</returns>
  /// <param name="t">The type of attribute to search for.</param>
  /// <param name="inherit">true to search this member's inheritance hierarchy to find the attributes.</param>
  public override object[] GetCustomAttributes(Type t, bool inherit) => new object[0];

  /// <summary>Returns an array that contains the custom attributes that are attached to this method.</summary>
  /// <returns>An array of <see cref="T:System.Object" /> objects that has zero elements.</returns>
  /// <param name="inherit">true to search this member's inheritance hierarchy to find the attributes.</param>
  public override object[] GetCustomAttributes(bool inherit) => new object[0];

  /// <summary>Returns information about the implementation of this method.</summary>
  /// <returns>
  /// <see cref="F:System.Reflection.MethodImplAttributes.Managed" /> and <see cref="F:System.Reflection.MethodImplAttributes.IL" />.</returns>
  public override MethodImplAttributes GetMethodImplementationFlags() => MethodImplAttributes.IL;

  internal abstract MethodInfo GetMethodInfo(CompilerGlobals compilerGlobals);

  internal virtual PackageScope GetPackage()
  {
    return this.obj is ClassScope ? ((ClassScope) this.obj).GetPackage() : throw new JScriptException(JSError.InternalError);
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
    return this.Invoke(obj, obj, options, binder, parameters, culture);
  }

  internal abstract object Invoke(
    object obj,
    object thisob,
    BindingFlags options,
    Binder binder,
    object[] parameters,
    CultureInfo culture);

  /// <summary>Determines whether the specified attribute type is attached to this method.</summary>
  /// <returns>false in all cases.</returns>
  /// <param name="type">The attribute type to search for.</param>
  /// <param name="inherit">true to search this member's inheritance hierarchy to find the attributes.</param>
  public sealed override bool IsDefined(Type type, bool inherit) => false;

  /// <summary>Gets the type of member that this method is. Identifies the type of class derived from <see cref="T:System.Reflection.MemberInfo" /> that this class derives from.</summary>
  /// <returns>
  /// <see cref="F:System.Reflection.MemberTypes.Method" />, because this class derives from <see cref="T:System.Reflection.MethodInfo" />.</returns>
  public override MemberTypes MemberType => MemberTypes.Method;

  /// <summary>Gets a handle to the internal metadata representation of this method.</summary>
  /// <returns>A handle to the internal metadata representation of this method.</returns>
  public override RuntimeMethodHandle MethodHandle
  {
    get => this.GetMethodInfo((CompilerGlobals) null).MethodHandle;
  }

  /// <summary>Gets the class object that was used to obtain this instance through reflection.</summary>
  /// <returns>The type that declares this method.</returns>
  public override Type ReflectedType => this.DeclaringType;

  /// <summary>Gets the custom attributes for the type of the return value of this method.</summary>
  /// <returns>null in all cases.</returns>
  public override ICustomAttributeProvider ReturnTypeCustomAttributes
  {
    get => (ICustomAttributeProvider) null;
  }
}
