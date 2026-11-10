// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.JSColorizer
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System.Security.Permissions;

#nullable disable
namespace Microsoft.JScript;

[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
internal class JSColorizer : IColorizeText
{
    private JSScanner _scanner;
    private SourceState _state;

    internal JSColorizer()
    {
        this._scanner = new JSScanner();
        this._scanner.SetAuthoringMode(true);
        this._state = SourceState.STATE_COLOR_NORMAL;
    }

    public virtual ITokenEnumerator Colorize(string sourceCode, SourceState state)
    {
        TokenColorInfoList tokenColorInfoList = new TokenColorInfoList();
        this._state = SourceState.STATE_COLOR_NORMAL;
        if (sourceCode.Length > 0)
        {
            Context context = new Context((DocumentContext)null, sourceCode);
            this._scanner.SetSource(context);
            try
            {
                if (SourceState.STATE_COLOR_COMMENT == state)
                {
                    int num = this._scanner.SkipMultiLineComment();
                    if (num > sourceCode.Length)
                    {
                        this._state = SourceState.STATE_COLOR_COMMENT;
                        num = sourceCode.Length;
                    }

                    tokenColorInfoList.Add(context);
                    if (num == sourceCode.Length)
                        return (ITokenEnumerator)tokenColorInfoList;
                }

                this._scanner.GetNextToken();
                JSToken jsToken = JSToken.None;
                while (context.GetToken() != JSToken.EndOfFile)
                {
                    tokenColorInfoList.Add(context);
                    jsToken = context.GetToken();
                    this._scanner.GetNextToken();
                }

                if (JSToken.UnterminatedComment == jsToken)
                    this._state = SourceState.STATE_COLOR_COMMENT;
            }
            catch (ScannerException ex)
            {
            }
        }

        return (ITokenEnumerator)tokenColorInfoList;
    }

    public virtual SourceState GetStateForText(string sourceCode, SourceState state)
    {
        if (sourceCode != null)
        {
            this._state = SourceState.STATE_COLOR_NORMAL;
            Context sourceContext = new Context((DocumentContext)null, sourceCode);
            this._scanner.SetSource(sourceContext);
            if (SourceState.STATE_COLOR_COMMENT == state && this._scanner.SkipMultiLineComment() > sourceCode.Length)
            {
                this._state = SourceState.STATE_COLOR_COMMENT;
                return this._state;
            }

            this._scanner.GetNextToken();
            JSToken jsToken = JSToken.None;
            while (sourceContext.GetToken() != JSToken.EndOfFile)
            {
                jsToken = sourceContext.GetToken();
                this._scanner.GetNextToken();
            }

            if (JSToken.UnterminatedComment == jsToken)
                this._state = SourceState.STATE_COLOR_COMMENT;
        }

        return this._state;
    }
}