// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.JSClosureProperty
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Globalization;
using System.Reflection;

#nullable disable
namespace Microsoft.JScript;

internal class JSClosureProperty : JSWrappedProperty
{
    private MethodInfo getMeth;
    private MethodInfo setMeth;

    internal JSClosureProperty(PropertyInfo property, MethodInfo getMeth, MethodInfo setMeth)
        : base(property, (object)null)
    {
        this.getMeth = getMeth;
        this.setMeth = setMeth;
    }

    public override object GetValue(
        object obj,
        BindingFlags invokeAttr,
        Binder binder,
        object[] index,
        CultureInfo culture)
    {
        if (this.getMeth == (MethodInfo)null)
            throw new MissingMethodException();
        return this.getMeth.Invoke(obj, invokeAttr, binder, index, culture);
    }

    public override MethodInfo GetGetMethod(bool nonPublic)
    {
        return nonPublic || this.getMeth != (MethodInfo)null && this.getMeth.IsPublic ? this.getMeth : (MethodInfo)null;
    }

    public override MethodInfo GetSetMethod(bool nonPublic)
    {
        return nonPublic || this.setMeth != (MethodInfo)null && this.setMeth.IsPublic ? this.setMeth : (MethodInfo)null;
    }

    public override void SetValue(
        object obj,
        object value,
        BindingFlags invokeAttr,
        Binder binder,
        object[] index,
        CultureInfo culture)
    {
        if (this.setMeth == (MethodInfo)null)
            throw new MissingMethodException();
        int length = index == null ? 0 : index.Length;
        object[] objArray = new object[length + 1];
        objArray[0] = value;
        if (length > 0)
            ArrayObject.Copy(index, 0, objArray, 1, length);
        this.setMeth.Invoke(obj, invokeAttr, binder, objArray, culture);
    }
}