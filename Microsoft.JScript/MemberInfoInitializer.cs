// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.MemberInfoInitializer
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System.Runtime.InteropServices;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Defines methods that an external COM object must implement. This interface is used by an external COM implementation to expose its objects to JScript because JScript needs a managed <see cref="T:System.Reflection.MemberInfo" /> to bind to an external object. This interface is implemented by <see cref="T:Microsoft.JScript.COMFieldInfo" />, <see cref="T:Microsoft.JScript.COMMethodInfo" />, and <see cref="T:Microsoft.JScript.COMPropertyInfo" />.</summary>
[Guid("98A3BF0A-1B56-4f32-ACE0-594FEB27EC48")]
[ComVisible(true)]
public interface MemberInfoInitializer
{
    /// <summary>When implemented in a class, initializes the information that enables JScript to communicate with the specified external COM member.</summary>
    /// <param name="name">The name of the external COM member.</param>
    /// <param name="dispatch">Information that enables JScript to call, get, and set the external COM member.</param>
    void Initialize(string name, COMMemberInfo dispatch);

    /// <summary>When implemented in a class, gets a managed object that enables JScript to communicate with an external COM member.</summary>
    /// <returns>A managed object that enables JScript to communicate with an external COM member.</returns>
    COMMemberInfo GetCOMMemberInfo();
}