// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.VSAITEMTYPE2
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System.Runtime.InteropServices;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Extends the <see cref="T:Microsoft.Vsa.VsaItemType" /> enumeration. Provides extra functionality that is needed by an IActiveScript host. </summary>
[Guid("581AD3D9-2BAA-3770-B92B-38607E1B463A")]
[ComVisible(true)]
public enum VSAITEMTYPE2
{
    /// <summary>Specifies none. This is the default value.</summary>
    None = 0,

    /// <summary>Specifies a host object.</summary>
    HOSTOBJECT = 16, // 0x00000010

    /// <summary>Specifies the scope.</summary>
    HOSTSCOPE = 17, // 0x00000011

    /// <summary>Specifies both the scope and the object.</summary>
    HOSTSCOPEANDOBJECT = 18, // 0x00000012

    /// <summary>Specifies the script scope.</summary>
    SCRIPTSCOPE = 19, // 0x00000013

    /// <summary>Specifies a script block. This applies to an <see cref="T:Microsoft.JScript.IVsaScriptCodeItem" />.</summary>
    SCRIPTBLOCK = 20, // 0x00000014

    /// <summary>Specifies a single statement. This applies to an <see cref="T:Microsoft.JScript.IVsaScriptCodeItem" />.</summary>
    STATEMENT = 21, // 0x00000015

    /// <summary>Specifies an expression. This applies to an <see cref="T:Microsoft.JScript.IVsaScriptCodeItem" />.</summary>
    EXPRESSION = 22, // 0x00000016
}