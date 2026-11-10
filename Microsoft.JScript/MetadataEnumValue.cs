// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.MetadataEnumValue
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;

#nullable disable
namespace Microsoft.JScript;

internal sealed class MetadataEnumValue : EnumWrapper
{
    private Type _type;
    private object _value;

    internal static object GetEnumValue(Type type, object value)
    {
        return !type.Assembly.ReflectionOnly ? Enum.ToObject(type, value) : (object)new MetadataEnumValue(type, value);
    }

    private MetadataEnumValue(Type type, object value)
    {
        this._type = type;
        this._value = value;
    }

    internal override object value => this._value;

    internal override Type type => this._type;

    internal override string name => Enum.GetName(this._type, this._value) ?? this._value.ToString();
}