// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.StringObject
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Represents a string object. This class belongs to the built-in object model category.</summary>
public class StringObject : JSObject
{
  internal string value;
  private bool implicitWrapper;

  /// <summary>Initializes a new instance of the <see cref="T:Microsoft.JScript.StringObject" /> class. </summary>
  /// <param name="prototype">The parent of the object.</param>
  /// <param name="value">The underlying value that the class represents.</param>
  protected StringObject(ScriptObject prototype, string value)
    : base(prototype)
  {
    this.value = value;
    this.noExpando = false;
    this.implicitWrapper = false;
  }

  internal StringObject(ScriptObject prototype, string value, bool implicitWrapper)
    : base(prototype, typeof (StringObject))
  {
    this.value = value;
    this.noExpando = implicitWrapper;
    this.implicitWrapper = implicitWrapper;
  }

  internal override string GetClassName() => "String";

  /// <summary>Gets the number of characters in the string.</summary>
  /// <returns>The number of characters in the string.</returns>
  public int length => this.value.Length;

  /// <summary>Determines whether this string and the specified string have the same value.</summary>
  /// <returns>true if this string and <paramref name="ob" /> have the same value; otherwise, false.</returns>
  /// <param name="ob">The string to compare.</param>
  public override bool Equals(object ob)
  {
    if (ob is StringObject)
      ob = (object) ((StringObject) ob).value;
    return this.value.Equals(ob);
  }

  internal override object GetDefaultValue(PreferredType preferred_type)
  {
    if (this.GetParent() is LenientStringPrototype)
      return base.GetDefaultValue(preferred_type);
    switch (preferred_type)
    {
      case PreferredType.String:
        return !this.noExpando && this.NameTable[(object) "toString"] != null ? base.GetDefaultValue(preferred_type) : (object) this.value;
      case PreferredType.LocaleString:
        return base.GetDefaultValue(preferred_type);
      default:
        if (!this.noExpando)
        {
          object obj = this.NameTable[(object) "valueOf"];
          if (obj == null && preferred_type == PreferredType.Either)
            obj = this.NameTable[(object) "toString"];
          if (obj != null)
            return base.GetDefaultValue(preferred_type);
        }
        return (object) this.value;
    }
  }

  /// <summary>Returns the hash code for this string.</summary>
  /// <returns>A hash code for this string.</returns>
  public override int GetHashCode() => this.value.GetHashCode();

  /// <summary>Returns <see cref="T:System.String" /> or <see cref="T:Microsoft.JScript.StringObject" />.</summary>
  /// <returns>
  /// <see cref="T:System.String" /> or <see cref="T:Microsoft.JScript.StringObject" />.</returns>
  public new Type GetType() => !this.implicitWrapper ? Typeob.StringObject : Typeob.String;

  internal override object GetValueAtIndex(uint index)
  {
    return this.implicitWrapper && (long) index < (long) this.value.Length ? (object) this.value[(int) index] : base.GetValueAtIndex(index);
  }
}
