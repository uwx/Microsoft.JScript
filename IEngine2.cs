// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.IEngine2
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using Microsoft.JScript.Vsa;
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Defines extra functionality that is needed by an IActiveScript host. Implemented by the <see cref="T:Microsoft.JScript.Vsa.VsaEngine" /> engine.  </summary>
[Guid("BFF6C97F-0705-4394-88B8-A03A4B8B4CD7")]
[ComVisible(true)]
public interface IEngine2
{
  /// <summary>When implemented in a class, gets the compiled assembly that is produced by the compilation step.</summary>
  /// <returns>The compiled assembly.</returns>
  Assembly GetAssembly();

  /// <summary>When implemented in a class, runs the compiled code in the specified application domain. This is not implemented in JScript because managed engines do not support running code in user-provided application domains.</summary>
  /// <param name="domain">The application domain in which to run the compiled code.</param>
  [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
  void Run(AppDomain domain);

  /// <summary>When implemented in a class, triggers compilation of all the state currently in the engine. All references, code items, host items, and resources are run through the compiler.</summary>
  /// <returns>true if the compilation was successful, otherwise, false.</returns>
  bool CompileEmpty();

  /// <summary>When implemented in a class, runs the compiled code in the current <see cref="T:System.AppDomain" />.</summary>
  [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
  void RunEmpty();

  /// <summary>When implemented in a class, directs the engine to unhook event sources from event handlers. This is not implemented in JScript.</summary>
  [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
  void DisconnectEvents();

  /// <summary>When implemented in a class, communicates to the engine that it can connect any registered event sources that have events to any global event handlers. This is not implemented in JScript. </summary>
  [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
  void ConnectEvents();

  /// <summary>When implemented in a class, specifies the name of an item that is marked as an event source, so that the <see cref="M:Microsoft.JScript.IEngine2.ConnectEvents" /> method can determine which events to connect. This is not implemented in JScript.</summary>
  /// <param name="name">The name of an added item that provides events.</param>
  [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
  void RegisterEventSource(string name);

  /// <summary>When implemented in a class, calls <see cref="M:System.Threading.Thread.Abort" /> on the thread that is running the script code.</summary>
  [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
  void Interrupt();

  /// <summary>When implemented in a class, initializes the <see cref="T:Microsoft.JScript.Vsa.VsaEngine" />.</summary>
  /// <param name="rootMoniker">The root moniker of the script engine. For more information, see <see cref="P:Microsoft.Vsa.IVsaEngine.RootMoniker" />.</param>
  /// <param name="site">The host-implemented <see cref="T:Microsoft.Vsa.IVsaSite" /> object that is used by the script engine to communicate with the host. For more information, see <see cref="P:Microsoft.Vsa.IVsaEngine.Site" />.</param>
  [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
  void InitVsaEngine(string rootMoniker, IJSVsaSite site);

  /// <summary>When implemented in a class, gets the global scope object.</summary>
  /// <returns>The global scope object.</returns>
  IVsaScriptScope GetGlobalScope();

  /// <summary>When implemented in a class, gets the code module from the compiled assembly.</summary>
  /// <returns>The code module from the compiled assembly.</returns>
  Module GetModule();

  /// <summary>When implemented in a class, creates a new identical engine that runs in a new application domain, with the same compiled state and named items. This is not implemented in JScript.</summary>
  /// <returns>The new engine.</returns>
  /// <param name="domain">The application domain in which to create the new engine.</param>
  [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
  IJSVsaEngine Clone(AppDomain domain);

  /// <summary>When implemented in a class, this method is called by the debugger when it has to reset the engine to a clean state before evaluating a new expression. The method resets the added items and the global scope back to their original state.</summary>
  [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
  void Restart();
}
