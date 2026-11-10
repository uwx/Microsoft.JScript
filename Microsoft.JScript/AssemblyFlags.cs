// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.AssemblyFlags
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

#nullable disable
namespace Microsoft.JScript;

internal enum AssemblyFlags
{
    SideBySideCompatible = 0,
    PublicKey = 1,
    NonSideBySideAppDomain = 16, // 0x00000010
    NonSideBySideProcess = 32, // 0x00000020
    NonSideBySideMachine = 48, // 0x00000030
    CompatibilityMask = 112, // 0x00000070
    DisableJITcompileOptimizer = 16384, // 0x00004000
    EnableJITcompileTracking = 32768, // 0x00008000
}