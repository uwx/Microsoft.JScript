// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.ITokenColorInfo
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System.Runtime.InteropServices;
using System.Security.Permissions;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Defines properties for coloring specific units of code.</summary>
[Guid("0F20D5C8-CBDB-4b64-AB7F-10B158407323")]
[ComVisible(true)]
public interface ITokenColorInfo
{
    /// <summary>When implemented in a class, gets the position where a unit of code starts.</summary>
    /// <returns>The position where the unit of code starts.</returns>
    int StartPosition
    {
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
        get;
    }

    /// <summary>When implemented in a class, gets the position where a unit of code ends.</summary>
    /// <returns>The position where the unit of code ends.</returns>
    int EndPosition
    {
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
        get;
    }

    /// <summary>When implemented in a class, gets the color that should be used to show a unit of code.</summary>
    /// <returns>The color that should be used to show the unit of code.</returns>
    TokenColor Color
    {
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
        get;
    }
}