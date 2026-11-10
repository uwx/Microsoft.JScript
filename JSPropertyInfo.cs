// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.JSPropertyInfo
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Globalization;
using System.Reflection;

#nullable disable
namespace Microsoft.JScript;

internal class JSPropertyInfo : PropertyInfo
{
  private PropertyInfo property;
  private Type declaringType;
  internal MethodInfo getter;
  internal MethodInfo setter;

  internal JSPropertyInfo(PropertyInfo property) => this.property = property;

  public override PropertyAttributes Attributes => this.property.Attributes;

  public override bool CanRead => this.property.CanRead;

  public override bool CanWrite => this.property.CanWrite;

  public override Type DeclaringType
  {
    get
    {
      Type declaringType = this.declaringType;
      if (declaringType == (Type) null)
        this.declaringType = declaringType = this.property.DeclaringType;
      return declaringType;
    }
  }

  public override MethodInfo GetGetMethod(bool nonPublic)
  {
    MethodInfo method = this.getter;
    if (method == (MethodInfo) null)
    {
      method = this.property.GetGetMethod(nonPublic);
      if (method != (MethodInfo) null)
        method = (MethodInfo) new JSMethodInfo(method);
      this.getter = method;
    }
    return method;
  }

  public override ParameterInfo[] GetIndexParameters()
  {
    MethodInfo getMethod = this.GetGetMethod(false);
    return getMethod != (MethodInfo) null ? getMethod.GetParameters() : this.property.GetIndexParameters();
  }

  public override MethodInfo GetSetMethod(bool nonPublic)
  {
    MethodInfo method = this.setter;
    if (method == (MethodInfo) null)
    {
      method = this.property.GetSetMethod(nonPublic);
      if (method != (MethodInfo) null)
        method = (MethodInfo) new JSMethodInfo(method);
      this.setter = method;
    }
    return method;
  }

  public override MethodInfo[] GetAccessors(bool nonPublic)
  {
    throw new JScriptException(JSError.InternalError);
  }

  public override object[] GetCustomAttributes(Type t, bool inherit)
  {
    return CustomAttribute.GetCustomAttributes((MemberInfo) this.property, t, inherit);
  }

  public override object[] GetCustomAttributes(bool inherit)
  {
    return this.property.GetCustomAttributes(inherit);
  }

  public override object GetValue(
    object obj,
    BindingFlags invokeAttr,
    Binder binder,
    object[] index,
    CultureInfo culture)
  {
    return this.GetGetMethod(false).Invoke(obj, invokeAttr, binder, index == null ? new object[0] : index, culture);
  }

  public override bool IsDefined(Type type, bool inherit)
  {
    return CustomAttribute.IsDefined((MemberInfo) this.property, type, inherit);
  }

  public override string Name => this.property.Name;

  public override Type PropertyType => this.property.PropertyType;

  public override Type ReflectedType => this.property.ReflectedType;

  public override void SetValue(
    object obj,
    object value,
    BindingFlags invokeAttr,
    Binder binder,
    object[] index,
    CultureInfo culture)
  {
    if (index == null || index.Length == 0)
    {
      this.GetSetMethod(false).Invoke(obj, invokeAttr, binder, new object[1]
      {
        value
      }, culture);
    }
    else
    {
      int length = index.Length;
      object[] objArray = new object[length + 1];
      ArrayObject.Copy(index, 0, objArray, 0, length);
      objArray[length] = value;
      this.GetSetMethod(false).Invoke(obj, invokeAttr, binder, objArray, culture);
    }
  }
}
