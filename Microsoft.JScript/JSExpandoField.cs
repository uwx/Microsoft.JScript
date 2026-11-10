// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.JSExpandoField
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System.Globalization;
using System.Reflection;

#nullable disable
namespace Microsoft.JScript;

internal sealed class JSExpandoField : JSField
{
    private object value;
    private string name;

    internal JSExpandoField(string name)
        : this(name, (object)null)
    {
    }

    internal JSExpandoField(string name, object value)
    {
        this.value = value;
        this.name = name;
    }

    public override FieldAttributes Attributes => FieldAttributes.Public | FieldAttributes.Static;

    public override object GetValue(object obj) => this.value;

    public override string Name => this.name;

    public override void SetValue(
        object obj,
        object value,
        BindingFlags invokeAttr,
        Binder binder,
        CultureInfo locale)
    {
        this.value = value;
    }
}