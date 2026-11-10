// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.VBArrayObject
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using Microsoft.JScript.Vsa;
using System;
using System.Collections;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Represents a VBArray object. This class belongs to the built-in object model category.</summary>
public class VBArrayObject : JSObject
{
  private Array array;

  /// <summary>Initializes a new instance of the <see cref="T:Microsoft.JScript.VBArrayObject" /> class.</summary>
  /// <param name="parent">The parent of the object.</param>
  /// <param name="array">The underlying array that the class represents.</param>
  public VBArrayObject(VBArrayPrototype parent, Array array)
    : base((ScriptObject) parent)
  {
    this.array = array;
    this.noExpando = false;
  }

  internal virtual int dimensions() => this.array.Rank;

  internal virtual object getItem(object[] args)
  {
    if (args == null || args.Length == 0)
      throw new JScriptException(JSError.TooFewParameters);
    if (args.Length == 1)
      return this.array.GetValue(Convert.ToInt32(args[0]));
    if (args.Length == 2)
      return this.array.GetValue(Convert.ToInt32(args[0]), Convert.ToInt32(args[1]));
    if (args.Length == 3)
      return this.array.GetValue(Convert.ToInt32(args[0]), Convert.ToInt32(args[1]), Convert.ToInt32(args[2]));
    int length = args.Length;
    int[] numArray = new int[length];
    for (int index = 0; index < length; ++index)
      numArray[index] = Convert.ToInt32(args[index]);
    return this.array.GetValue(numArray);
  }

  internal virtual int lbound(object dimension)
  {
    return this.array.GetLowerBound(Convert.ToInt32(dimension));
  }

  internal virtual ArrayObject toArray(VsaEngine engine)
  {
    IList array1 = (IList) this.array;
    ArrayObject array2 = engine.GetOriginalArrayConstructor().Construct();
    uint num = 0;
    int count = array1.Count;
    IEnumerator enumerator = array1.GetEnumerator();
    array2.length = (object) count;
    while (enumerator.MoveNext())
      array2.SetValueAtIndex(num++, enumerator.Current);
    return array2;
  }

  internal virtual int ubound(object dimension)
  {
    return this.array.GetUpperBound(Convert.ToInt32(dimension));
  }
}
