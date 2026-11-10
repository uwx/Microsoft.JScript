// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.TokenColorInfo
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System.Security.Permissions;

#nullable disable
namespace Microsoft.JScript;

[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
internal class TokenColorInfo : ITokenColorInfo
{
    private Context _token;
    private TokenColor _color;
    internal TokenColorInfo _next;

    internal TokenColorInfo(Context token)
    {
        this._token = token.Clone();
        this._color = TokenColorInfo.ColorFromToken(this._token);
        this._next = this;
    }

    internal static TokenColor ColorFromToken(Context context)
    {
        JSToken token = context.GetToken();
        if (JSScanner.IsKeyword(token))
            return TokenColor.COLOR_KEYWORD;
        if (JSToken.Identifier == token)
            return context.Equals("eval") ? TokenColor.COLOR_KEYWORD : TokenColor.COLOR_IDENTIFIER;
        if (JSToken.StringLiteral == token)
            return TokenColor.COLOR_STRING;
        if (JSToken.NumericLiteral == token || JSToken.IntegerLiteral == token)
            return TokenColor.COLOR_NUMBER;
        if (JSToken.Comment == token || JSToken.UnterminatedComment == token)
            return TokenColor.COLOR_COMMENT;
        return JSScanner.IsOperator(token) ? TokenColor.COLOR_OPERATOR : TokenColor.COLOR_TEXT;
    }

    internal TokenColorInfo Clone() => (TokenColorInfo)this.MemberwiseClone();

    public TokenColor Color => this._color;

    public int EndPosition => this._token.EndPosition;

    public int StartPosition => this._token.StartPosition;

    internal void UpdateToken(Context token)
    {
        this._token = token.Clone();
        this._color = TokenColorInfo.ColorFromToken(this._token);
    }
}