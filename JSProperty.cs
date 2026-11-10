// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.JSProperty
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace Microsoft.JScript;

internal class JSProperty : PropertyInfo
{
  private string name;
  private ParameterInfo[] formal_parameters;
  internal PropertyBuilder metaData;
  internal JSMethod getter;
  internal JSMethod setter;

  internal JSProperty(string name)
  {
    this.name = name;
    this.formal_parameters = (ParameterInfo[]) null;
    this.getter = (JSMethod) null;
    this.setter = (JSMethod) null;
  }

  public override PropertyAttributes Attributes => PropertyAttributes.None;

  public override bool CanRead
  {
    get => JSProperty.GetGetMethod((PropertyInfo) this, true) != (MethodInfo) null;
  }

  public override bool CanWrite
  {
    get => JSProperty.GetSetMethod((PropertyInfo) this, true) != (MethodInfo) null;
  }

  internal virtual string GetClassFullName()
  {
    return (MethodInfo) this.getter != (MethodInfo) null ? this.getter.GetClassFullName() : this.setter.GetClassFullName();
  }

  internal bool GetterAndSetterAreConsistent()
  {
    if ((MethodInfo) this.getter == (MethodInfo) null || (MethodInfo) this.setter == (MethodInfo) null)
      return true;
    ((JSFieldMethod) this.getter).func.PartiallyEvaluate();
    ((JSFieldMethod) this.setter).func.PartiallyEvaluate();
    ParameterInfo[] parameters1 = this.getter.GetParameters();
    ParameterInfo[] parameters2 = this.setter.GetParameters();
    int length1 = parameters1.Length;
    int length2 = parameters2.Length;
    if (length1 != length2 - 1 || !((JSFieldMethod) this.getter).func.ReturnType((JSField) null).Equals((object) ((ParameterDeclaration) parameters2[length1]).type.ToIReflect()))
      return false;
    for (int index = 0; index < length1; ++index)
    {
      if (((ParameterDeclaration) parameters1[index]).type.ToIReflect() != ((ParameterDeclaration) parameters2[index]).type.ToIReflect())
        return false;
    }
    return (this.getter.Attributes & ~MethodAttributes.Abstract) == (this.setter.Attributes & ~MethodAttributes.Abstract);
  }

  public override Type DeclaringType
  {
    get
    {
      return (MethodInfo) this.getter != (MethodInfo) null ? this.getter.DeclaringType : this.setter.DeclaringType;
    }
  }

  public sealed override object[] GetCustomAttributes(Type t, bool inherit) => new object[0];

  public sealed override object[] GetCustomAttributes(bool inherit)
  {
    if ((MethodInfo) this.getter != (MethodInfo) null)
      return this.getter.GetCustomAttributes(true);
    return (MethodInfo) this.setter != (MethodInfo) null ? this.setter.GetCustomAttributes(true) : new object[0];
  }

  [DebuggerStepThrough]
  [DebuggerHidden]
  internal static object GetValue(PropertyInfo prop, object obj, object[] index)
  {
    JSProperty jsProperty = prop as JSProperty;
    if ((PropertyInfo) jsProperty != (PropertyInfo) null)
      return jsProperty.GetValue(obj, BindingFlags.ExactBinding, (Binder) null, index, (CultureInfo) null);
    JSWrappedProperty jsWrappedProperty = prop as JSWrappedProperty;
    if ((PropertyInfo) jsWrappedProperty != (PropertyInfo) null)
      return jsWrappedProperty.GetValue(obj, BindingFlags.ExactBinding, (Binder) null, index, (CultureInfo) null);
    MethodInfo getMethod = JSProperty.GetGetMethod(prop, false);
    if (!(getMethod != (MethodInfo) null))
      throw new MissingMethodException();
    try
    {
      return getMethod.Invoke(obj, BindingFlags.ExactBinding, (Binder) null, index, (CultureInfo) null);
    }
    catch (TargetInvocationException ex)
    {
      throw ex.InnerException;
    }
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
    MethodInfo methodInfo = (MethodInfo) this.getter;
    JSObject jsObject = obj as JSObject;
    if (methodInfo == (MethodInfo) null && jsObject != null)
    {
      methodInfo = jsObject.GetMethod("get_" + this.name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
      JSWrappedMethod jsWrappedMethod = methodInfo as JSWrappedMethod;
      if ((MethodInfo) jsWrappedMethod != (MethodInfo) null)
        methodInfo = jsWrappedMethod.method;
    }
    if (methodInfo == (MethodInfo) null)
      methodInfo = this.GetGetMethod(false);
    if (!(methodInfo != (MethodInfo) null))
      return (object) Missing.Value;
    try
    {
      return methodInfo.Invoke(obj, invokeAttr, binder, index == null ? new object[0] : index, culture);
    }
    catch (TargetInvocationException ex)
    {
      throw ex.InnerException;
    }
  }

  public override MethodInfo[] GetAccessors(bool nonPublic)
  {
    if ((MethodInfo) this.getter != (MethodInfo) null && (nonPublic || this.getter.IsPublic))
      return (MethodInfo) this.setter != (MethodInfo) null && (nonPublic || this.setter.IsPublic) ? new MethodInfo[2]
      {
        (MethodInfo) this.getter,
        (MethodInfo) this.setter
      } : new MethodInfo[1]{ (MethodInfo) this.getter };
    if (!((MethodInfo) this.setter != (MethodInfo) null) || !nonPublic && !this.setter.IsPublic)
      return new MethodInfo[0];
    return new MethodInfo[1]{ (MethodInfo) this.setter };
  }

  internal static MethodInfo GetGetMethod(PropertyInfo prop, bool nonPublic)
  {
    if (prop == (PropertyInfo) null)
      return (MethodInfo) null;
    JSProperty jsProperty = prop as JSProperty;
    if ((PropertyInfo) jsProperty != (PropertyInfo) null)
      return jsProperty.GetGetMethod(nonPublic);
    MethodInfo getMethod1 = prop.GetGetMethod(nonPublic);
    if (getMethod1 != (MethodInfo) null)
      return getMethod1;
    Type declaringType = prop.DeclaringType;
    if (declaringType == (Type) null)
      return (MethodInfo) null;
    Type baseType = declaringType.BaseType;
    if (baseType == (Type) null)
      return (MethodInfo) null;
    MethodInfo getMethod2 = prop.GetGetMethod(nonPublic);
    if (getMethod2 == (MethodInfo) null)
      return (MethodInfo) null;
    BindingFlags bindingFlags = BindingFlags.Public;
    BindingFlags bindingAttr = !getMethod2.IsStatic ? bindingFlags | BindingFlags.Instance : bindingFlags | BindingFlags.Static | BindingFlags.FlattenHierarchy;
    if (nonPublic)
      bindingAttr |= BindingFlags.NonPublic;
    string name = prop.Name;
    prop = (PropertyInfo) null;
    try
    {
      prop = baseType.GetProperty(name, bindingAttr, (Binder) null, (Type) null, new Type[0], (ParameterModifier[]) null);
    }
    catch (AmbiguousMatchException ex)
    {
    }
    return prop != (PropertyInfo) null ? JSProperty.GetGetMethod(prop, nonPublic) : (MethodInfo) null;
  }

  public override MethodInfo GetGetMethod(bool nonPublic)
  {
    if ((MethodInfo) this.getter == (MethodInfo) null)
    {
      try
      {
        IReflect superType = ((ClassScope) this.setter.obj).GetSuperType();
        BindingFlags bindingFlags = BindingFlags.Public;
        BindingFlags bindingAttr = !this.setter.IsStatic ? bindingFlags | BindingFlags.Instance : bindingFlags | BindingFlags.Static | BindingFlags.FlattenHierarchy;
        if (nonPublic)
          bindingAttr |= BindingFlags.NonPublic;
        PropertyInfo property = superType.GetProperty(this.name, bindingAttr, (Binder) null, (Type) null, new Type[0], (ParameterModifier[]) null);
        return property is JSProperty ? property.GetGetMethod(nonPublic) : JSProperty.GetGetMethod(property, nonPublic);
      }
      catch (AmbiguousMatchException ex)
      {
      }
    }
    return nonPublic || this.getter.IsPublic ? (MethodInfo) this.getter : (MethodInfo) null;
  }

  public override ParameterInfo[] GetIndexParameters()
  {
    if (this.formal_parameters == null)
    {
      if ((MethodInfo) this.getter != (MethodInfo) null)
      {
        this.formal_parameters = this.getter.GetParameters();
      }
      else
      {
        ParameterInfo[] parameters = this.setter.GetParameters();
        int num = parameters.Length;
        if (num <= 1)
          num = 1;
        this.formal_parameters = new ParameterInfo[num - 1];
        for (int index = 0; index < num - 1; ++index)
          this.formal_parameters[index] = parameters[index];
      }
    }
    return this.formal_parameters;
  }

  internal static MethodInfo GetSetMethod(PropertyInfo prop, bool nonPublic)
  {
    if (prop == (PropertyInfo) null)
      return (MethodInfo) null;
    JSProperty jsProperty = prop as JSProperty;
    if ((PropertyInfo) jsProperty != (PropertyInfo) null)
      return jsProperty.GetSetMethod(nonPublic);
    MethodInfo setMethod = prop.GetSetMethod(nonPublic);
    if (setMethod != (MethodInfo) null)
      return setMethod;
    Type declaringType = prop.DeclaringType;
    if (declaringType == (Type) null)
      return (MethodInfo) null;
    Type baseType = declaringType.BaseType;
    if (baseType == (Type) null)
      return (MethodInfo) null;
    MethodInfo getMethod = prop.GetGetMethod(nonPublic);
    if (getMethod == (MethodInfo) null)
      return (MethodInfo) null;
    BindingFlags bindingFlags = BindingFlags.Public;
    BindingFlags bindingAttr = !getMethod.IsStatic ? bindingFlags | BindingFlags.Instance : bindingFlags | BindingFlags.Static | BindingFlags.FlattenHierarchy;
    if (nonPublic)
      bindingAttr |= BindingFlags.NonPublic;
    string name = prop.Name;
    prop = (PropertyInfo) null;
    try
    {
      prop = baseType.GetProperty(name, bindingAttr, (Binder) null, (Type) null, new Type[0], (ParameterModifier[]) null);
    }
    catch (AmbiguousMatchException ex)
    {
    }
    return prop != (PropertyInfo) null ? JSProperty.GetSetMethod(prop, nonPublic) : (MethodInfo) null;
  }

  public override MethodInfo GetSetMethod(bool nonPublic)
  {
    if ((MethodInfo) this.setter == (MethodInfo) null)
    {
      try
      {
        IReflect superType = ((ClassScope) this.getter.obj).GetSuperType();
        BindingFlags bindingFlags = BindingFlags.Public;
        BindingFlags bindingAttr = !this.getter.IsStatic ? bindingFlags | BindingFlags.Instance : bindingFlags | BindingFlags.Static | BindingFlags.FlattenHierarchy;
        if (nonPublic)
          bindingAttr |= BindingFlags.NonPublic;
        PropertyInfo property = superType.GetProperty(this.name, bindingAttr, (Binder) null, (Type) null, new Type[0], (ParameterModifier[]) null);
        return property is JSProperty ? property.GetSetMethod(nonPublic) : JSProperty.GetSetMethod(property, nonPublic);
      }
      catch (AmbiguousMatchException ex)
      {
      }
    }
    return nonPublic || this.setter.IsPublic ? (MethodInfo) this.setter : (MethodInfo) null;
  }

  public sealed override bool IsDefined(Type type, bool inherit) => false;

  public override MemberTypes MemberType => MemberTypes.Property;

  public override string Name => this.name;

  internal IReflect PropertyIR()
  {
    if (this.getter is JSFieldMethod)
      return ((JSFieldMethod) this.getter).ReturnIR();
    if ((MethodInfo) this.setter != (MethodInfo) null)
    {
      ParameterInfo[] parameters = this.setter.GetParameters();
      if (parameters.Length != 0)
      {
        ParameterInfo parameterInfo = parameters[parameters.Length - 1];
        return parameterInfo is ParameterDeclaration ? ((ParameterDeclaration) parameterInfo).ParameterIReflect : (IReflect) parameterInfo.ParameterType;
      }
    }
    return (IReflect) Typeob.Void;
  }

  public override Type PropertyType
  {
    get
    {
      if ((MethodInfo) this.getter != (MethodInfo) null)
        return this.getter.ReturnType;
      if ((MethodInfo) this.setter != (MethodInfo) null)
      {
        ParameterInfo[] parameters = this.setter.GetParameters();
        if (parameters.Length != 0)
          return parameters[parameters.Length - 1].ParameterType;
      }
      return Typeob.Void;
    }
  }

  public override Type ReflectedType
  {
    get
    {
      return (MethodInfo) this.getter != (MethodInfo) null ? this.getter.ReflectedType : this.setter.ReflectedType;
    }
  }

  [DebuggerStepThrough]
  [DebuggerHidden]
  internal static void SetValue(PropertyInfo prop, object obj, object value, object[] index)
  {
    JSProperty jsProperty = prop as JSProperty;
    if ((PropertyInfo) jsProperty != (PropertyInfo) null)
    {
      jsProperty.SetValue(obj, value, BindingFlags.ExactBinding, (Binder) null, index, (CultureInfo) null);
    }
    else
    {
      MethodInfo setMethod = JSProperty.GetSetMethod(prop, false);
      if (!(setMethod != (MethodInfo) null))
        throw new MissingMethodException();
      int length = index == null ? 0 : index.Length;
      object[] objArray = new object[length + 1];
      if (length > 0)
        ArrayObject.Copy(index, 0, objArray, 0, length);
      objArray[length] = value;
      setMethod.Invoke(obj, BindingFlags.ExactBinding, (Binder) null, objArray, (CultureInfo) null);
    }
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
    MethodInfo methodInfo = (MethodInfo) this.setter;
    JSObject jsObject = obj as JSObject;
    if (methodInfo == (MethodInfo) null && jsObject != null)
    {
      methodInfo = jsObject.GetMethod("set_" + this.name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
      JSWrappedMethod jsWrappedMethod = methodInfo as JSWrappedMethod;
      if ((MethodInfo) jsWrappedMethod != (MethodInfo) null)
        methodInfo = jsWrappedMethod.method;
    }
    if (methodInfo == (MethodInfo) null)
      methodInfo = this.GetSetMethod(false);
    if (!(methodInfo != (MethodInfo) null))
      return;
    if (index == null || index.Length == 0)
    {
      methodInfo.Invoke(obj, invokeAttr, binder, new object[1]
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
      methodInfo.Invoke(obj, invokeAttr, binder, objArray, culture);
    }
  }
}
