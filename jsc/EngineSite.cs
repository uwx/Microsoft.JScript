// Decompiled with JetBrains decompiler
// Type: EngineSite
// Assembly: jsc, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: EFC0C76F-35B7-44DC-BADC-474CA93DF1B0
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\jsc.exe

using Microsoft.JScript.Vsa;

#nullable disable
internal class EngineSite : IJSVsaSite
{
    private CompilerOptions options;

    public EngineSite(CompilerOptions options) => this.options = options;

    public virtual bool OnCompilerError(IJSVsaError error)
    {
        int severity = error.Severity;
        if (severity > this.options.nWarningLevel)
            return true;
        bool fIsWarning = severity != 0 && !this.options.fTreatWarningsAsErrors;
        JScriptCompiler.PrintError(error.SourceMoniker, error.Line, error.StartColumn, fIsWarning, error.Number,
            error.Description);
        return true;
    }

    public virtual object GetItemInfo(string strItemName)
    {
        throw new JSVsaException(JSVsaError.CallbackUnexpected);
    }

    public virtual object GetGlobalInstance(string name)
    {
        throw new JSVsaException(JSVsaError.CallbackUnexpected);
    }

    public virtual object GetEventSourceInstance(string ItemName, string EventSourceName)
    {
        throw new JSVsaException(JSVsaError.CallbackUnexpected);
    }

    public virtual void Notify(string notification, object value)
    {
        throw new JSVsaException(JSVsaError.CallbackUnexpected);
    }

    public virtual void GetCompiledState(out byte[] pe, out byte[] debugInfo)
    {
        pe = (byte[])null;
        debugInfo = (byte[])null;
        throw new JSVsaException(JSVsaError.CallbackUnexpected);
    }
}