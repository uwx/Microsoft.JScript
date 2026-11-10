// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.ObjectConstructor
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Globalization;
using System.Reflection;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Represents a prototype-based object that is defined by a constructor function. This class belongs to the built-in object model category.</summary>
public sealed class ObjectConstructor : ScriptFunction
{
  internal static readonly ObjectConstructor ob = new ObjectConstructor();
  internal ObjectPrototype originalPrototype;

  internal ObjectConstructor()
    : base((ScriptObject) FunctionPrototype.ob, "Object", 1)
  {
    this.originalPrototype = ObjectPrototype.ob;
    ObjectPrototype._constructor = this;
    this.proto = (object) ObjectPrototype.ob;
  }

  internal ObjectConstructor(LenientFunctionPrototype parent, LenientObjectPrototype prototypeProp)
    : base((ScriptObject) parent, "Object", 1)
  {
    this.originalPrototype = (ObjectPrototype) prototypeProp;
    prototypeProp.constructor = (object) this;
    this.proto = (object) prototypeProp;
    this.noExpando = false;
  }

  internal override object Call(object[] args, object thisob)
  {
    if (args.Length == 0)
      return (object) this.ConstructObject();
    object obj = args[0];
    return obj == null || obj == DBNull.Value ? this.Construct(args) : Convert.ToObject3(obj, this.engine);
  }

  internal override object Construct(object[] args)
  {
    if (args.Length == 0)
      return (object) this.ConstructObject();
    object obj = args[0];
    switch (Convert.GetTypeCode(obj))
    {
      case TypeCode.Empty:
      case TypeCode.DBNull:
        return (object) this.ConstructObject();
      case TypeCode.Object:
        if (obj is ScriptObject)
          return obj;
        return (!(obj is IReflect) ? (IReflect) obj.GetType() : (IReflect) obj).InvokeMember(string.Empty, BindingFlags.Public | BindingFlags.CreateInstance | BindingFlags.OptionalParamBinding, (Binder) JSBinder.ob, obj, new object[0], (ParameterModifier[]) null, (CultureInfo) null, (string[]) null);
      default:
        return Convert.ToObject3(obj, this.engine);
    }
  }

  /// <summary>Creates an instance of the prototype-based object.</summary>
  /// <returns>The instance of the object.</returns>
  public JSObject ConstructObject() => new JSObject((ScriptObject) this.originalPrototype, false);

  /// <summary>Creates an instance of the prototype-based object, passing in the specified arguments.</summary>
  /// <returns>The instance of the object.</returns>
  /// <param name="args">The arguments to pass to the constructor.</param>
  [JSFunction(JSFunctionAttributeEnum.HasVarArgs)]
  public new object CreateInstance(params object[] args) => this.Construct(args);

  /// <summary>Invokes a method of the prototype-based object, passing in the specified arguments.</summary>
  /// <returns>The result of the call to the method.</returns>
  /// <param name="args">The arguments to pass to the method.</param>
  [JSFunction(JSFunctionAttributeEnum.HasVarArgs)]
  public object Invoke(params object[] args) => this.Call(args, (object) null);
}
