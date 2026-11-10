// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.INeedEngine
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using Microsoft.JScript.Vsa;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Defines methods that get and set references to the scripting engine. </summary>
public interface INeedEngine
{
    /// <summary>When implemented in a class, gets a reference to the scripting engine.</summary>
    /// <returns>A reference to the scripting engine.</returns>
    VsaEngine GetEngine();

    /// <summary>When implemented in a class, sets a reference to the scripting engine.</summary>
    /// <param name="engine">A reference to the scripting engine.</param>
    void SetEngine(VsaEngine engine);
}