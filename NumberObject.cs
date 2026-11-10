// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.NumberObject
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Represents a number object. This class belongs to the built-in object model category.</summary>
public class NumberObject : JSObject
{
  internal Type baseType;
  internal object value;
  private bool implicitWrapper;

  /// <summary>Initializes a new instance of the <see cref="T:Microsoft.JScript.NumberObject" /> class.</summary>
  /// <param name="parent">The parent of the object.</param>
  /// <param name="value">The underlying value that the class represents.</param>
  protected NumberObject(ScriptObject parent, object value)
    : base(parent)
  {
    this.baseType = Globals.TypeRefs.ToReferenceContext(value.GetType());
    this.value = value;
    this.noExpando = false;
    this.implicitWrapper = false;
  }

  internal NumberObject(ScriptObject parent, object value, bool implicitWrapper)
    : base(parent, typeof (NumberObject))
  {
    this.baseType = Globals.TypeRefs.ToReferenceContext(value.GetType());
    this.value = value;
    this.noExpando = implicitWrapper;
    this.implicitWrapper = implicitWrapper;
  }

  internal NumberObject(ScriptObject parent, Type baseType)
    : base(parent)
  {
    this.baseType = baseType;
    this.value = (object) 0.0;
    this.noExpando = false;
  }

  internal override object GetDefaultValue(PreferredType preferred_type)
  {
    if (this.GetParent() is LenientNumberPrototype)
      return base.GetDefaultValue(preferred_type);
    switch (preferred_type)
    {
      case PreferredType.String:
        return !this.noExpando && this.NameTable[(object) "toString"] != null ? base.GetDefaultValue(preferred_type) : (object) Convert.ToString(this.value);
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
        return this.value;
    }
  }

  internal override string GetClassName() => "Number";

  /// <summary>Returns the type of the underlying value object, or returns <see cref="T:Microsoft.JScript.NumberObject" />.</summary>
  /// <returns>The type of the underlying value object, or <see cref="T:Microsoft.JScript.NumberObject" />.</returns>
  public new Type GetType() => !this.implicitWrapper ? Typeob.NumberObject : this.baseType;
}
