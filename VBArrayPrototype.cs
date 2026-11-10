// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.VBArrayPrototype
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using Microsoft.JScript.Vsa;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Provides static methods and properties for <see cref="T:Microsoft.JScript.VBArrayObject" /> objects. All methods of this class are called by passing in a reference to the object that is requesting access to the method. This class belongs to the built-in object model category.</summary>
public class VBArrayPrototype : JSObject
{
  internal static readonly VBArrayPrototype ob = new VBArrayPrototype(FunctionPrototype.ob, ObjectPrototype.ob);
  internal static VBArrayConstructor _constructor;

  internal VBArrayPrototype(FunctionPrototype funcprot, ObjectPrototype parent)
    : base((ScriptObject) parent)
  {
  }

  /// <summary>Gets a reference to a prototype-based object that is using this prototype.</summary>
  /// <returns>A reference to a prototype-based object.</returns>
  public static VBArrayConstructor constructor => VBArrayPrototype._constructor;

  /// <summary>Determines the number of dimensions in the specified array.</summary>
  /// <returns>The number of dimensions in <paramref name="thisob" />.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.VBArray_dimensions)]
  public static int dimensions(object thisob)
  {
    return thisob is VBArrayObject ? ((VBArrayObject) thisob).dimensions() : throw new JScriptException(JSError.VBArrayExpected);
  }

  /// <summary>Gets the value at the specified position in the specified array.</summary>
  /// <returns>The value at the specified position in <paramref name="thisob" />.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  /// <param name="args">An array that represents the indexes of each dimension that specifies the position of the element to get.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject | JSFunctionAttributeEnum.HasVarArgs, JSBuiltin.VBArray_getItem)]
  public static object getItem(object thisob, params object[] args)
  {
    return thisob is VBArrayObject ? ((VBArrayObject) thisob).getItem(args) : throw new JScriptException(JSError.VBArrayExpected);
  }

  /// <summary>Gets the lower bound of the specified dimension in the specified array.</summary>
  /// <returns>The lower bound of <paramref name="dimension" /> in <paramref name="thisob" />.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  /// <param name="dimension">The dimension whose lower bound is determined.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.VBArray_lbound)]
  public static int lbound(object thisob, object dimension)
  {
    return thisob is VBArrayObject ? ((VBArrayObject) thisob).lbound(dimension) : throw new JScriptException(JSError.VBArrayExpected);
  }

  /// <summary>Converts a <see cref="T:Microsoft.JScript.VBArrayObject" /> object to an <see cref="T:Microsoft.JScript.ArrayObject" /> object.</summary>
  /// <returns>
  /// <paramref name="thisob" /> expressed as an <see cref="T:Microsoft.JScript.ArrayObject" />.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  /// <param name="engine">A reference to the scripting engine.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject | JSFunctionAttributeEnum.HasEngine, JSBuiltin.VBArray_toArray)]
  public static ArrayObject toArray(object thisob, VsaEngine engine)
  {
    return thisob is VBArrayObject ? ((VBArrayObject) thisob).toArray(engine) : throw new JScriptException(JSError.VBArrayExpected);
  }

  /// <summary>Gets the upper bound of the specified dimension in the specified array.</summary>
  /// <returns>The upper bound of <paramref name="dimension" /> in <paramref name="thisob" />.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  /// <param name="dimension">The dimension whose upper bound is determined.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.VBArray_ubound)]
  public static int ubound(object thisob, object dimension)
  {
    return thisob is VBArrayObject ? ((VBArrayObject) thisob).ubound(dimension) : throw new JScriptException(JSError.VBArrayExpected);
  }
}
