// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.NoSkipTokenSet
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

#nullable disable
namespace Microsoft.JScript;

internal class NoSkipTokenSet
{
    private NoSkipTokenSet.TokenSetListItem _tokenSet;

    internal static readonly JSToken[] s_ArrayInitNoSkipTokenSet = new JSToken[2]
    {
        JSToken.RightBracket,
        JSToken.Comma
    };

    internal static readonly JSToken[] s_BlockConditionNoSkipTokenSet = new JSToken[3]
    {
        JSToken.RightParen,
        JSToken.LeftCurly,
        JSToken.EndOfLine
    };

    internal static readonly JSToken[] s_BlockNoSkipTokenSet = new JSToken[1]
    {
        JSToken.RightCurly
    };

    internal static readonly JSToken[] s_BracketToken = new JSToken[1]
    {
        JSToken.RightBracket
    };

    internal static readonly JSToken[] s_CaseNoSkipTokenSet = new JSToken[4]
    {
        JSToken.Case,
        JSToken.Default,
        JSToken.Colon,
        JSToken.EndOfLine
    };

    internal static readonly JSToken[] s_ClassBodyNoSkipTokenSet = new JSToken[10]
    {
        JSToken.Class,
        JSToken.Interface,
        JSToken.Enum,
        JSToken.Function,
        JSToken.Var,
        JSToken.Const,
        JSToken.Static,
        JSToken.Public,
        JSToken.Private,
        JSToken.Protected
    };

    internal static readonly JSToken[] s_InterfaceBodyNoSkipTokenSet = new JSToken[5]
    {
        JSToken.Enum,
        JSToken.Function,
        JSToken.Public,
        JSToken.EndOfLine,
        JSToken.Semicolon
    };

    internal static readonly JSToken[] s_ClassExtendsNoSkipTokenSet = new JSToken[2]
    {
        JSToken.LeftCurly,
        JSToken.Implements
    };

    internal static readonly JSToken[] s_ClassImplementsNoSkipTokenSet = new JSToken[2]
    {
        JSToken.LeftCurly,
        JSToken.Comma
    };

    internal static readonly JSToken[] s_DoWhileBodyNoSkipTokenSet = new JSToken[1]
    {
        JSToken.While
    };

    internal static readonly JSToken[] s_EndOfLineToken = new JSToken[1]
    {
        JSToken.EndOfLine
    };

    internal static readonly JSToken[] s_EndOfStatementNoSkipTokenSet = new JSToken[2]
    {
        JSToken.Semicolon,
        JSToken.EndOfLine
    };

    internal static readonly JSToken[] s_EnumBaseTypeNoSkipTokenSet = new JSToken[1]
    {
        JSToken.LeftCurly
    };

    internal static readonly JSToken[] s_EnumBodyNoSkipTokenSet = new JSToken[1]
    {
        JSToken.Identifier
    };

    internal static readonly JSToken[] s_ExpressionListNoSkipTokenSet = new JSToken[1]
    {
        JSToken.Comma
    };

    internal static readonly JSToken[] s_FunctionDeclNoSkipTokenSet = new JSToken[3]
    {
        JSToken.RightParen,
        JSToken.LeftCurly,
        JSToken.Comma
    };

    internal static readonly JSToken[] s_IfBodyNoSkipTokenSet = new JSToken[1]
    {
        JSToken.Else
    };

    internal static readonly JSToken[] s_MemberExprNoSkipTokenSet = new JSToken[3]
    {
        JSToken.LeftBracket,
        JSToken.LeftParen,
        JSToken.AccessField
    };

    internal static readonly JSToken[] s_NoTrySkipTokenSet = new JSToken[2]
    {
        JSToken.Catch,
        JSToken.Finally
    };

    internal static readonly JSToken[] s_ObjectInitNoSkipTokenSet = new JSToken[2]
    {
        JSToken.RightCurly,
        JSToken.Comma
    };

    internal static readonly JSToken[] s_PackageBodyNoSkipTokenSet = new JSToken[3]
    {
        JSToken.Class,
        JSToken.Interface,
        JSToken.Enum
    };

    internal static readonly JSToken[] s_ParenExpressionNoSkipToken = new JSToken[1]
    {
        JSToken.RightParen
    };

    internal static readonly JSToken[] s_ParenToken = new JSToken[1]
    {
        JSToken.RightParen
    };

    internal static readonly JSToken[] s_PostfixExpressionNoSkipTokenSet = new JSToken[2]
    {
        JSToken.Increment,
        JSToken.Decrement
    };

    internal static readonly JSToken[] s_StartBlockNoSkipTokenSet = new JSToken[1]
    {
        JSToken.LeftCurly
    };

    internal static readonly JSToken[] s_StartStatementNoSkipTokenSet = new JSToken[10]
    {
        JSToken.LeftCurly,
        JSToken.Var,
        JSToken.Const,
        JSToken.If,
        JSToken.For,
        JSToken.Do,
        JSToken.While,
        JSToken.With,
        JSToken.Switch,
        JSToken.Try
    };

    internal static readonly JSToken[] s_SwitchNoSkipTokenSet = new JSToken[2]
    {
        JSToken.Case,
        JSToken.Default
    };

    internal static readonly JSToken[] s_TopLevelNoSkipTokenSet = new JSToken[6]
    {
        JSToken.Package,
        JSToken.Class,
        JSToken.Interface,
        JSToken.Enum,
        JSToken.Function,
        JSToken.Import
    };

    internal static readonly JSToken[] s_VariableDeclNoSkipTokenSet = new JSToken[2]
    {
        JSToken.Comma,
        JSToken.Semicolon
    };

    internal NoSkipTokenSet() => this._tokenSet = (NoSkipTokenSet.TokenSetListItem)null;

    internal void Add(JSToken[] tokens)
    {
        this._tokenSet = new NoSkipTokenSet.TokenSetListItem(tokens, this._tokenSet);
    }

    internal void Remove(JSToken[] tokens)
    {
        NoSkipTokenSet.TokenSetListItem tokenSetListItem1 = this._tokenSet;
        NoSkipTokenSet.TokenSetListItem tokenSetListItem2 = (NoSkipTokenSet.TokenSetListItem)null;
        for (; tokenSetListItem1 != null; tokenSetListItem1 = tokenSetListItem1._next)
        {
            if (tokenSetListItem1._tokens == tokens)
            {
                if (tokenSetListItem2 == null)
                {
                    this._tokenSet = this._tokenSet._next;
                    break;
                }

                tokenSetListItem2._next = tokenSetListItem1._next;
                break;
            }

            tokenSetListItem2 = tokenSetListItem1;
        }
    }

    internal bool HasToken(JSToken token)
    {
        for (NoSkipTokenSet.TokenSetListItem tokenSetListItem = this._tokenSet;
             tokenSetListItem != null;
             tokenSetListItem = tokenSetListItem._next)
        {
            int index = 0;
            for (int length = tokenSetListItem._tokens.Length; index < length; ++index)
            {
                if (tokenSetListItem._tokens[index] == token)
                    return true;
            }
        }

        return false;
    }

    private class TokenSetListItem
    {
        internal NoSkipTokenSet.TokenSetListItem _next;
        internal JSToken[] _tokens;

        internal TokenSetListItem(JSToken[] tokens, NoSkipTokenSet.TokenSetListItem next)
        {
            this._next = next;
            this._tokens = tokens;
        }
    }
}