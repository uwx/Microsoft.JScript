// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.BooleanObject
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Represents a Boolean object. This class belongs to the built-in object model category.</summary>
public class BooleanObject : JSObject
{
  internal bool value;
  private bool implicitWrapper;

  /// <summary>Initializes a new instance of the <see cref="T:Microsoft.JScript.BooleanObject" /> class, specifying the parent and subtype.</summary>
  /// <param name="prototype">The parent of the object.</param>
  /// <param name="subType">The subtype of the object.</param>
  protected BooleanObject(ScriptObject prototype, Type subType)
    : base(prototype, subType)
  {
    this.value = this.value;
    this.noExpando = false;
    this.implicitWrapper = false;
  }

  internal BooleanObject(ScriptObject prototype, bool value, bool implicitWrapper)
    : base(prototype, typeof (BooleanObject))
  {
    this.value = value;
    this.noExpando = implicitWrapper;
    this.implicitWrapper = implicitWrapper;
  }

  internal override string GetClassName() => "Boolean";

  internal override object GetDefaultValue(PreferredType preferred_type)
  {
    if (this.GetParent() is LenientBooleanPrototype)
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
        return (object) this.value;
    }
  }

  /// <summary>Returns <see cref="T:System.Boolean" /> or <see cref="T:Microsoft.JScript.BooleanObject" />.</summary>
  /// <returns>
  /// <see cref="T:System.Boolean" /> or <see cref="T:Microsoft.JScript.BooleanObject" />.</returns>
  public new Type GetType() => !this.implicitWrapper ? Typeob.BooleanObject : Typeob.Boolean;
}
