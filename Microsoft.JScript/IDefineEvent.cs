// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.IDefineEvent
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System.Runtime.InteropServices;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Defines extra functionality that is needed by an active script host. </summary>
[Guid("D1A19408-BB6B-43eb-BB6F-E7CF6AF047D7")]
[ComVisible(true)]
public interface IDefineEvent
{
    /// <summary>Used by an active script host to define an event.</summary>
    /// <returns>The event, if it was successfully added.</returns>
    /// <param name="code">The code that contains the event.</param>
    /// <param name="startLine">The line number where the code starts.</param>
    [return: MarshalAs(UnmanagedType.Interface)]
    object AddEvent(string code, int startLine);
}