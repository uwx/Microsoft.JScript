// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.ObjectPrototype
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using Microsoft.JScript.Vsa;
using System.Reflection;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Provides static methods and properties for <see cref="T:Microsoft.JScript.JSObject" /> objects. All methods of this class are called by passing in a reference to the object that is requesting access to the method. This class belongs to the built-in object model category.</summary>
public class ObjectPrototype : JSObject
{
  internal static readonly ObjectPrototype ob = new ObjectPrototype();
  internal static ObjectConstructor _constructor;

  internal ObjectPrototype()
    : base((ScriptObject) null)
  {
    if (Globals.contextEngine == null)
    {
      this.engine = new VsaEngine(true);
      this.engine.InitVsaEngine("JS7://Microsoft.JScript.Vsa.VsaEngine", (IJSVsaSite) new DefaultVsaSite());
    }
    else
      this.engine = Globals.contextEngine;
  }

  /// <summary>Gets a reference to a prototype-based object that is using this prototype.</summary>
  /// <returns>A reference to a prototype-based object.</returns>
  public static ObjectConstructor constructor => ObjectPrototype._constructor;

  internal static ObjectPrototype CommonInstance() => ObjectPrototype.ob;

  /// <summary>Determines whether the specified object contains the specified property.</summary>
  /// <returns>true if <paramref name="name" /> is a property of <paramref name="thisob" /> and has been set; otherwise, false.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  /// <param name="name">The property to search for.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Object_hasOwnProperty)]
  public static bool hasOwnProperty(object thisob, object name)
  {
    string name1 = Convert.ToString(name);
    if (thisob is ArrayObject)
    {
      long index = ArrayObject.Array_index_for(name1);
      if (index >= 0L)
      {
        object valueAtIndex = ((ScriptObject) thisob).GetValueAtIndex((uint) index);
        return valueAtIndex != null && valueAtIndex != Missing.Value;
      }
    }
    if (!(thisob is JSObject))
      return !(LateBinding.GetMemberValue(thisob, name1) is Missing);
    MemberInfo[] member = ((ScriptObject) thisob).GetMember(name1, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
    int length = member.Length;
    if (length > 1)
      return true;
    if (length < 1)
      return false;
    return !(member[0] is JSPrototypeField) || !(((JSPrototypeField) member[0]).value is Missing);
  }

  /// <summary>Determines whether the first specified object can be instantiated as the type of the second specified object.</summary>
  /// <returns>true if <paramref name="ob" /> is in the inheritance hierarchy of <paramref name="thisob" />; otherwise, false.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  /// <param name="ob">The object that has the desired type.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Object_isPrototypeOf)]
  public static bool isPrototypeOf(object thisob, object ob)
  {
    if (thisob is ScriptObject && ob is ScriptObject)
    {
      for (; ob != null; ob = (object) ((ScriptObject) ob).GetParent())
      {
        if (ob == thisob)
          return true;
      }
    }
    return false;
  }

  /// <summary>Determines whether the specified property of the specified object is enumerable.</summary>
  /// <returns>true if <paramref name="name" /> is an enumerable property; otherwise, false.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  /// <param name="name">The property to test.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Object_propertyIsEnumerable)]
  public static bool propertyIsEnumerable(object thisob, object name)
  {
    string name1 = Convert.ToString(name);
    if (thisob is ArrayObject)
    {
      long index = ArrayObject.Array_index_for(name1);
      if (index >= 0L)
      {
        object valueAtIndex = ((ScriptObject) thisob).GetValueAtIndex((uint) index);
        return valueAtIndex != null && valueAtIndex != Missing.Value;
      }
    }
    if (!(thisob is JSObject))
      return false;
    FieldInfo field = ((ScriptObject) thisob).GetField(name1, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
    return field != (FieldInfo) null && field is JSExpandoField;
  }

  /// <summary>Creates a string representation of the specified object by using the <see cref="T:Microsoft.JScript.Convert" /> class.</summary>
  /// <returns>A string representation of <paramref name="thisob" /> that is created by the <see cref="T:Microsoft.JScript.Convert" /> class.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Object_toLocaleString)]
  public static string toLocaleString(object thisob) => Convert.ToString(thisob);

  /// <summary>Creates a string representation of the specified object.</summary>
  /// <returns>The name of the class of the object, if it has one defined; otherwise, the <see cref="P:System.Reflection.MemberInfo.Name" /> of the <see cref="T:System.Type" /> of the object.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Object_toString)]
  public static string toString(object thisob)
  {
    return thisob is JSObject ? $"[object {((JSObject) thisob).GetClassName()}]" : $"[object {thisob.GetType().Name}]";
  }

  /// <summary>Returns the specified object.</summary>
  /// <returns>
  /// <paramref name="thisob" />.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Object_valueOf)]
  public static object valueOf(object thisob) => thisob;
}
