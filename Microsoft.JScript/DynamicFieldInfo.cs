// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.DynamicFieldInfo
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

#nullable disable
namespace Microsoft.JScript;

/// <summary>Represents field information. This class is used to return fields of expando classes and script arrays to the debugger. </summary>
public sealed class DynamicFieldInfo
{
    /// <summary>The name of the field.</summary>
    public string name;

    /// <summary>The value of the field.</summary>
    public object value;

    /// <summary>The type of the field.</summary>
    public string fieldTypeName;

    /// <summary>Initializes a new instance of the <see cref="T:Microsoft.JScript.DynamicFieldInfo" /> class, using the specified name and value.</summary>
    /// <param name="name">The name of the field.</param>
    /// <param name="value">The value of the field.</param>
    public DynamicFieldInfo(string name, object value)
    {
        this.name = name;
        this.value = value;
        this.fieldTypeName = "";
    }

    /// <summary>Initializes a new instance of the <see cref="T:Microsoft.JScript.DynamicFieldInfo" /> class, using the specified name, value, and type.</summary>
    /// <param name="name">The name of the field.</param>
    /// <param name="value">The value of the field.</param>
    /// <param name="fieldTypeName">The type of the field.</param>
    public DynamicFieldInfo(string name, object value, string fieldTypeName)
    {
        this.name = name;
        this.value = value;
        this.fieldTypeName = fieldTypeName;
    }
}