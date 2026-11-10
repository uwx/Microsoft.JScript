// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.JSCodeSense
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using Microsoft.JScript.Vsa;
using System.Security.Permissions;

#nullable disable
namespace Microsoft.JScript;

[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
internal class JSCodeSense : IJSVsaSite, IParseText
{
  private VsaEngine _engine;
  private IJSVsaCodeItem _codeBlock;
  private IErrorHandler _errorHandler;

  internal JSCodeSense()
  {
    this._engine = new VsaEngine(true);
    this._engine.InitVsaEngine("JSC://Microsoft.JScript.Vsa.VsaEngine", (IJSVsaSite) this);
    this._codeBlock = (IJSVsaCodeItem) this._engine.Items.CreateItem("Code", JSVsaItemType.Code, JSVsaItemFlag.None);
    this._errorHandler = (IErrorHandler) null;
  }

  public virtual void GetCompiledState(out byte[] pe, out byte[] debugInfo)
  {
    pe = (byte[]) null;
    debugInfo = (byte[]) null;
  }

  public virtual object GetGlobalInstance(string Name) => (object) null;

  public virtual object GetEventSourceInstance(string ItemName, string EventSourceName)
  {
    return (object) null;
  }

  public virtual bool OnCompilerError(IJSVsaError error)
  {
    return !(error is IVsaFullErrorInfo) || this._errorHandler.OnCompilerError((IVsaFullErrorInfo) error);
  }

  public virtual void Parse(string code, IErrorHandler errorHandler)
  {
    this._engine.Reset();
    this._errorHandler = errorHandler;
    this._codeBlock.SourceText = code;
    this._engine.CheckForErrors();
  }

  public virtual void Notify(string notification, object value)
  {
  }
}
