// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.JSWrappedProperty
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

internal class JSWrappedProperty : PropertyInfo, IWrappedMember
{
  internal object obj;
  internal PropertyInfo property;

  internal JSWrappedProperty(PropertyInfo property, object obj)
  {
    this.obj = obj;
    this.property = property;
    if (!(obj is JSObject))
      return;
    Type declaringType = property.DeclaringType;
    if (!(declaringType == Typeob.Object) && !(declaringType == Typeob.String) && !declaringType.IsPrimitive && !(declaringType == Typeob.Array))
      return;
    switch (obj)
    {
      case BooleanObject _:
        this.obj = (object) ((BooleanObject) obj).value;
        break;
      case NumberObject _:
        this.obj = ((NumberObject) obj).value;
        break;
      case StringObject _:
        this.obj = (object) ((StringObject) obj).value;
        break;
      case ArrayWrapper _:
        this.obj = (object) ((ArrayWrapper) obj).value;
        break;
    }
  }

  internal virtual string GetClassFullName()
  {
    return this.property is JSProperty ? ((JSProperty) this.property).GetClassFullName() : this.property.DeclaringType.FullName;
  }

  public override MemberTypes MemberType => MemberTypes.Property;

  public override string Name
  {
    get
    {
      return this.obj is LenientGlobalObject && this.property.Name.StartsWith("Slow", StringComparison.Ordinal) ? this.property.Name.Substring(4) : this.property.Name;
    }
  }

  public override Type DeclaringType => this.property.DeclaringType;

  public override Type ReflectedType => this.property.ReflectedType;

  public override PropertyAttributes Attributes => this.property.Attributes;

  public override bool CanRead => this.property.CanRead;

  public override bool CanWrite => this.property.CanWrite;

  public override object[] GetCustomAttributes(Type t, bool inherit)
  {
    return CustomAttribute.GetCustomAttributes((MemberInfo) this.property, t, inherit);
  }

  public override object[] GetCustomAttributes(bool inherit)
  {
    return this.property.GetCustomAttributes(inherit);
  }

  [DebuggerStepThrough]
  [DebuggerHidden]
  public override object GetValue(
    object obj,
    BindingFlags invokeAttr,
    Binder binder,
    object[] index,
    CultureInfo culture)
  {
    return this.property.GetValue(this.obj, invokeAttr, binder, index, culture);
  }

  [DebuggerStepThrough]
  [DebuggerHidden]
  public override void SetValue(
    object obj,
    object value,
    BindingFlags invokeAttr,
    Binder binder,
    object[] index,
    CultureInfo culture)
  {
    this.property.SetValue(this.obj, value, invokeAttr, binder, index, culture);
  }

  public override MethodInfo[] GetAccessors(bool nonPublic)
  {
    return this.property.GetAccessors(nonPublic);
  }

  public override MethodInfo GetGetMethod(bool nonPublic)
  {
    MethodInfo getMethod = JSProperty.GetGetMethod(this.property, nonPublic);
    return getMethod == (MethodInfo) null ? (MethodInfo) null : (MethodInfo) new JSWrappedMethod(getMethod, this.obj);
  }

  public override ParameterInfo[] GetIndexParameters() => this.property.GetIndexParameters();

  public override MethodInfo GetSetMethod(bool nonPublic)
  {
    MethodInfo setMethod = JSProperty.GetSetMethod(this.property, nonPublic);
    return setMethod == (MethodInfo) null ? (MethodInfo) null : (MethodInfo) new JSWrappedMethod(setMethod, this.obj);
  }

  public object GetWrappedObject() => this.obj;

  public override Type PropertyType => this.property.PropertyType;

  public override bool IsDefined(Type type, bool inherit)
  {
    return CustomAttribute.IsDefined((MemberInfo) this.property, type, inherit);
  }
}
