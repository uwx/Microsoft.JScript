// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.DateObject
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Represents a date object. This class belongs to the built-in object model category.</summary>
public class DateObject : JSObject
{
    internal double value;

    internal DateObject(ScriptObject parent, double value)
        : base(parent)
    {
        this.value = value != value || value > (double)long.MaxValue || value < (double)long.MinValue
            ? double.NaN
            : Math.Round(value);
        this.noExpando = false;
    }

    internal override string GetClassName() => "Date";

    internal override object GetDefaultValue(PreferredType preferred_type)
    {
        if (this.GetParent() is LenientDatePrototype)
            return base.GetDefaultValue(preferred_type);
        switch (preferred_type)
        {
            case PreferredType.Either:
            case PreferredType.String:
                return !this.noExpando && this.NameTable[(object)"toString"] != null
                    ? base.GetDefaultValue(preferred_type)
                    : (object)DatePrototype.toString((object)this);
            case PreferredType.LocaleString:
                return !this.noExpando && this.NameTable[(object)"toLocaleString"] != null
                    ? base.GetDefaultValue(preferred_type)
                    : (object)DatePrototype.toLocaleString((object)this);
            default:
                if (!this.noExpando)
                {
                    object obj = this.NameTable[(object)"valueOf"];
                    if (obj == null && preferred_type == PreferredType.Either)
                        obj = this.NameTable[(object)"toString"];
                    if (obj != null)
                        return base.GetDefaultValue(preferred_type);
                }

                return (object)this.value;
        }
    }
}