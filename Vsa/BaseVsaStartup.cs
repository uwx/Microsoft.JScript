// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.Vsa.BaseVsaStartup
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;

#nullable disable
namespace Microsoft.JScript.Vsa;

/// <summary>Used to start and reset base VSA engine.</summary>
[Obsolete("Use of this type is not recommended because it is being deprecated in Visual Studio 2005; there will be no replacement for this feature. Please see the ICodeCompiler documentation for additional help.")]
public abstract class BaseVsaStartup
{
  /// <summary>The VSA site for this engine.</summary>
  protected IJSVsaSite site;

  /// <summary>Sets a VSA site for this engine.</summary>
  /// <param name="site">A VSA site for this engine.</param>
  public void SetSite(IJSVsaSite site) => this.site = site;

  /// <summary>Used to perform operations at engine startup.</summary>
  public abstract void Startup();

  /// <summary>Used to perform operations at engine shut down.</summary>
  public abstract void Shutdown();
}
