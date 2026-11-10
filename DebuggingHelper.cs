// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.DebuggingHelper
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using Microsoft.JScript.Vsa;
using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace Microsoft.JScript;

internal static class DebuggingHelper
{
  public static DynamicFieldInfo[] GetHashTableFields(SimpleHashtable h)
  {
    DynamicFieldInfo[] hashTableFields;
    try
    {
      int count = h.count;
      hashTableFields = new DynamicFieldInfo[count];
      IDictionaryEnumerator enumerator = h.GetEnumerator();
      for (int index = 0; index < count; ++index)
      {
        if (enumerator.MoveNext())
          hashTableFields[index] = new DynamicFieldInfo((string) enumerator.Key, enumerator.Value);
        else
          break;
      }
    }
    catch
    {
      hashTableFields = new DynamicFieldInfo[0];
    }
    return hashTableFields;
  }

  public static DynamicFieldInfo[] GetExpandoObjectFields(object o, bool hideNamespaces)
  {
    if (!(o is IReflect reflect))
      return new DynamicFieldInfo[0];
    try
    {
      FieldInfo[] fields = reflect.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
      ArrayList arrayList = new ArrayList();
      foreach (FieldInfo fieldInfo in fields)
      {
        bool flag = false;
        foreach (object obj in arrayList)
        {
          if (fieldInfo.Name == ((DynamicFieldInfo) obj).name)
            flag = true;
        }
        if (!flag)
        {
          object obj = fieldInfo.GetValue(o);
          if (!hideNamespaces || !(obj is Namespace))
            arrayList.Add((object) new DynamicFieldInfo(fieldInfo.Name, obj, fieldInfo.FieldType.Name));
        }
      }
      return (DynamicFieldInfo[]) arrayList.ToArray(typeof (DynamicFieldInfo));
    }
    catch
    {
      return new DynamicFieldInfo[0];
    }
  }

  public static object CallMethod(
    string name,
    object thisob,
    object[] arguments,
    VsaEngine engine)
  {
    if (engine == null)
      engine = VsaEngine.CreateEngine();
    return new LateBinding(name, thisob, true).Call(arguments, false, false, engine);
  }

  public static object CallStaticMethod(
    string name,
    string typename,
    object[] arguments,
    VsaEngine engine)
  {
    if (engine == null)
      engine = VsaEngine.CreateEngine();
    object type = (object) DebuggingHelper.GetType(typename);
    return new LateBinding(name, type, true).Call(arguments, false, false, engine);
  }

  public static object CallConstructor(string typename, object[] arguments, VsaEngine engine)
  {
    if (engine == null)
      engine = VsaEngine.CreateEngine();
    return LateBinding.CallValue((object) null, (object) DebuggingHelper.GetType(typename), arguments, true, false, engine);
  }

  private static Type GetTypeInCurrentAppDomain(string typename)
  {
    foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
    {
      if (!(assembly is AssemblyBuilder))
      {
        Type type = assembly.GetType(typename);
        if (type != (Type) null)
          return type;
      }
    }
    return (Type) null;
  }

  private static Type GetType(string typename)
  {
    string[] strArray = typename.Split('.');
    if (strArray == null || strArray.Length == 0)
      return (Type) null;
    string typename1 = strArray[0];
    Type type = DebuggingHelper.GetTypeInCurrentAppDomain(typename1);
    int index1;
    for (index1 = 1; index1 < strArray.Length && type == (Type) null; ++index1)
    {
      typename1 = $"{typename1}.{strArray[index1]}";
      type = DebuggingHelper.GetTypeInCurrentAppDomain(typename1);
    }
    for (int index2 = index1; index2 < strArray.Length && type != (Type) null; ++index2)
      type = type.GetNestedType(strArray[index2], BindingFlags.Public | BindingFlags.NonPublic);
    return type;
  }

  public static void SetIndexedPropertyValue(
    string name,
    object thisob,
    object[] arguments,
    object value,
    VsaEngine engine)
  {
    new LateBinding(name, thisob, true).SetIndexedPropertyValue(arguments, value);
  }

  public static void SetStaticIndexedPropertyValue(
    string name,
    string typename,
    object[] arguments,
    object value,
    VsaEngine engine)
  {
    object type = (object) DebuggingHelper.GetType(typename);
    new LateBinding(name, type, true).SetIndexedPropertyValue(arguments, value);
  }

  public static void SetDefaultIndexedPropertyValue(
    object thisob,
    object[] arguments,
    VsaEngine engine,
    string[] namedParameters)
  {
    object obj = (object) null;
    int length = arguments.Length;
    if (length > 0)
      obj = arguments[length - 1];
    int i = 0;
    int n = length - 1;
    if (namedParameters != null && namedParameters.Length != 0 && namedParameters[0] == "this")
    {
      --n;
      i = 1;
    }
    object[] objArray = new object[n];
    ArrayObject.Copy(arguments, i, objArray, 0, n);
    new LateBinding((string) null, thisob, true).SetIndexedPropertyValue(objArray, obj);
  }

  public static object GetDefaultIndexedPropertyValue(
    object thisob,
    object[] arguments,
    VsaEngine engine,
    string[] namedParameters)
  {
    if (engine == null)
      engine = VsaEngine.CreateEngine();
    int length = arguments == null ? 0 : arguments.Length;
    object[] objArray;
    if (namedParameters != null && namedParameters.Length != 0 && namedParameters[0] == "this" && length > 0)
    {
      objArray = new object[length - 1];
      ArrayObject.Copy(arguments, 1, objArray, 0, length - 1);
    }
    else
      objArray = arguments;
    return new LateBinding((string) null, thisob, true).Call(objArray, false, false, engine);
  }

  public static object InvokeCOMObject(
    string name,
    object obj,
    object[] arguments,
    BindingFlags invokeAttr)
  {
    return obj.GetType().InvokeMember(name, invokeAttr, (Binder) JSBinder.ob, obj, arguments, (ParameterModifier[]) null, (CultureInfo) null, (string[]) null);
  }

  public static void Print(string message, VsaEngine engine)
  {
    if (engine == null || !engine.doPrint)
      return;
    ScriptStream.Out.Write(message);
  }

  public static object GetClosureInstance(VsaEngine engine)
  {
    if (engine == null)
      return (object) null;
    return engine.ScriptObjectStackTop() is StackFrame stackFrame ? stackFrame.closureInstance : (object) null;
  }

  public static object InvokeMethodInfo(
    MethodInfo m,
    object[] arguments,
    bool construct,
    object thisob,
    VsaEngine engine)
  {
    if (engine == null)
      engine = VsaEngine.CreateEngine();
    return LateBinding.CallOneOfTheMembers(new MemberInfo[1]
    {
      (MemberInfo) m
    }, arguments, construct, thisob, (Binder) JSBinder.ob, (CultureInfo) null, (string[]) null, engine);
  }

  public static VsaEngine CreateEngine() => VsaEngine.CreateEngineForDebugger();

  public static object ToNativeArray(string elementTypename, object arrayObject)
  {
    Type type = DebuggingHelper.GetType(elementTypename);
    if (!(type != (Type) null))
      throw new JScriptException(JSError.TypeMismatch);
    return arrayObject is ArrayObject arrayObject1 ? (object) arrayObject1.ToNativeArray(type) : throw new JScriptException(JSError.TypeMismatch);
  }

  public static object[] CreateArray(int length)
  {
    object[] array = new object[length];
    for (int index = 0; index < length; ++index)
      array[index] = new object();
    return array;
  }

  public static string[] CreateStringArray(string s)
  {
    return new string[1]{ s };
  }

  public static object StringToObject(string s) => (object) s;

  public static object BooleanToObject(bool i) => (object) i;

  public static object SByteToObject(sbyte i) => (object) i;

  public static object ByteToObject(byte i) => (object) i;

  public static object Int16ToObject(short i) => (object) i;

  public static object UInt16ToObject(ushort i) => (object) i;

  public static object Int32ToObject(int i) => (object) i;

  public static object UInt32ToObject(uint i) => (object) i;

  public static object Int64ToObject(long i) => (object) i;

  public static object UInt64ToObject(ulong i) => (object) i;

  public static object SingleToObject(float i) => (object) i;

  public static object DoubleToObject(double i) => (object) i;
}
