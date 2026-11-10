// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.JSPrototypeField
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System.Globalization;
using System.Reflection;

#nullable disable
namespace Microsoft.JScript;

internal sealed class JSPrototypeField : JSField
{
    private object prototypeObject;
    internal FieldInfo prototypeField;
    internal object value;

    internal JSPrototypeField(object prototypeObject, FieldInfo prototypeField)
    {
        this.prototypeObject = prototypeObject;
        this.prototypeField = prototypeField;
        this.value = (object)Missing.Value;
    }

    public override FieldAttributes Attributes => FieldAttributes.Public | FieldAttributes.Static;

    public override object GetValue(object obj)
    {
        return this.value is Missing ? this.prototypeField.GetValue(this.prototypeObject) : this.value;
    }

    public override string Name => this.prototypeField.Name;

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