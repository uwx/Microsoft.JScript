// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.RecoveryTokenException
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

#nullable disable
namespace Microsoft.JScript;

internal class RecoveryTokenException : ParserException
{
    internal JSToken _token;
    internal AST _partiallyComputedNode;

    internal RecoveryTokenException(JSToken token, AST partialAST)
    {
        this._token = token;
        this._partiallyComputedNode = partialAST;
    }
}