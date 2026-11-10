// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.JSAuthor
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System.Runtime.InteropServices;
using System.Security.Permissions;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Provides services for authoring JScript code in an editor. </summary>
[Guid("0E4EFFC0-2387-11d3-B372-00105A98B7CE")]
[ComVisible(true)]
public class JSAuthor : IAuthorServices
{
    /// <summary>Gets an object that is used to colorize text when JScript code is being authored in an editor.</summary>
    /// <returns>An object that is used to colorize text.</returns>
    [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
    public virtual IColorizeText GetColorizer() => (IColorizeText)new JSColorizer();

    /// <summary>Gets an object that is used to add IntelliSense to text when JScript code is being authored in an editor.</summary>
    /// <returns>An object that is used to add IntelliSense to text.</returns>
    [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
    public virtual IParseText GetCodeSense() => (IParseText)new JSCodeSense();
}