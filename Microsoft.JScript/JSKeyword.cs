// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.JSKeyword
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

#nullable disable
namespace Microsoft.JScript;

internal sealed class JSKeyword
{
    private JSKeyword next;
    private JSToken token;
    private string name;
    private int length;

    private JSKeyword(JSToken token, string name)
    {
        this.name = name;
        this.next = (JSKeyword)null;
        this.token = token;
        this.length = this.name.Length;
    }

    private JSKeyword(JSToken token, string name, JSKeyword next)
    {
        this.name = name;
        this.next = next;
        this.token = token;
        this.length = this.name.Length;
    }

    internal static string CanBeIdentifier(JSToken keyword)
    {
        switch (keyword)
        {
            case JSToken.Package:
                return "package";
            case JSToken.Internal:
                return "internal";
            case JSToken.Abstract:
                return "abstract";
            case JSToken.Public:
                return "public";
            case JSToken.Static:
                return "static";
            case JSToken.Private:
                return "private";
            case JSToken.Protected:
                return "protected";
            case JSToken.Final:
                return "final";
            case JSToken.Event:
                return "event";
            case JSToken.Void:
                return "void";
            case JSToken.Get:
                return "get";
            case JSToken.Implements:
                return "implements";
            case JSToken.Interface:
                return "interface";
            case JSToken.Set:
                return "set";
            case JSToken.Assert:
                return "assert";
            case JSToken.Boolean:
                return "boolean";
            case JSToken.Byte:
                return "byte";
            case JSToken.Char:
                return "char";
            case JSToken.Decimal:
                return "decimal";
            case JSToken.Double:
                return "double";
            case JSToken.Enum:
                return "enum";
            case JSToken.Ensure:
                return "ensure";
            case JSToken.Float:
                return "float";
            case JSToken.Goto:
                return "goto";
            case JSToken.Int:
                return "int";
            case JSToken.Invariant:
                return "invariant";
            case JSToken.Long:
                return "long";
            case JSToken.Namespace:
                return "namespace";
            case JSToken.Native:
                return "native";
            case JSToken.Require:
                return "require";
            case JSToken.Sbyte:
                return "sbyte";
            case JSToken.Short:
                return "short";
            case JSToken.Synchronized:
                return "synchronized";
            case JSToken.Transient:
                return "transient";
            case JSToken.Throws:
                return "throws";
            case JSToken.Volatile:
                return "volatile";
            case JSToken.Ushort:
                return "ushort";
            case JSToken.Uint:
                return "uint";
            case JSToken.Ulong:
                return "ulong";
            case JSToken.Use:
                return "use";
            default:
                return (string)null;
        }
    }

    internal JSToken GetKeyword(Context token, int length)
    {
        JSKeyword jsKeyword = this;
        while (jsKeyword != null)
        {
            if (length == jsKeyword.length)
            {
                int index1 = 1;
                int index2 = token.startPos + 1;
                while (index1 < length)
                {
                    char ch1 = jsKeyword.name[index1];
                    char ch2 = token.source_string[index2];
                    if ((int)ch1 != (int)ch2)
                    {
                        if ((int)ch2 < (int)ch1)
                            return JSToken.Identifier;
                        jsKeyword = jsKeyword.next;
                        goto label_13;
                    }

                    ++index1;
                    ++index2;
                }

                return jsKeyword.token;
            }

            if (length < jsKeyword.length)
                return JSToken.Identifier;
            jsKeyword = jsKeyword.next;
            continue;
            label_13: ;
        }

        return JSToken.Identifier;
    }

    internal static JSKeyword[] InitKeywords()
    {
        JSKeyword[] jsKeywordArray = new JSKeyword[26];
        JSKeyword jsKeyword1 = new JSKeyword(JSToken.Assert, "assert", new JSKeyword(JSToken.Abstract, "abstract"));
        jsKeywordArray[0] = jsKeyword1;
        JSKeyword jsKeyword2 = new JSKeyword(JSToken.Byte, "byte",
            new JSKeyword(JSToken.Break, "break", new JSKeyword(JSToken.Boolean, "boolean")));
        jsKeywordArray[1] = jsKeyword2;
        JSKeyword jsKeyword3 = new JSKeyword(JSToken.Case, "case",
            new JSKeyword(JSToken.Char, "char",
                new JSKeyword(JSToken.Catch, "catch",
                    new JSKeyword(JSToken.Class, "class",
                        new JSKeyword(JSToken.Const, "const", new JSKeyword(JSToken.Continue, "continue"))))));
        jsKeywordArray[2] = jsKeyword3;
        JSKeyword jsKeyword4 = new JSKeyword(JSToken.Do, "do",
            new JSKeyword(JSToken.Delete, "delete",
                new JSKeyword(JSToken.Double, "double",
                    new JSKeyword(JSToken.Default, "default", new JSKeyword(JSToken.Debugger, "debugger")))));
        jsKeywordArray[3] = jsKeyword4;
        JSKeyword jsKeyword5 = new JSKeyword(JSToken.Else, "else",
            new JSKeyword(JSToken.Enum, "enum",
                new JSKeyword(JSToken.Event, "event",
                    new JSKeyword(JSToken.Ensure, "ensure",
                        new JSKeyword(JSToken.Export, "export", new JSKeyword(JSToken.Extends, "extends"))))));
        jsKeywordArray[4] = jsKeyword5;
        JSKeyword jsKeyword6 = new JSKeyword(JSToken.For, "for",
            new JSKeyword(JSToken.False, "false",
                new JSKeyword(JSToken.Final, "final",
                    new JSKeyword(JSToken.Float, "float",
                        new JSKeyword(JSToken.Finally, "finally", new JSKeyword(JSToken.Function, "function"))))));
        jsKeywordArray[5] = jsKeyword6;
        JSKeyword jsKeyword7 = new JSKeyword(JSToken.Get, "get", new JSKeyword(JSToken.Goto, "goto"));
        jsKeywordArray[6] = jsKeyword7;
        JSKeyword jsKeyword8 = new JSKeyword(JSToken.If, "if",
            new JSKeyword(JSToken.In, "in",
                new JSKeyword(JSToken.Int, "int",
                    new JSKeyword(JSToken.Import, "import",
                        new JSKeyword(JSToken.Internal, "internal",
                            new JSKeyword(JSToken.Interface, "interface",
                                new JSKeyword(JSToken.Invariant, "invariant",
                                    new JSKeyword(JSToken.Implements, "implements",
                                        new JSKeyword(JSToken.Instanceof, "instanceof")))))))));
        jsKeywordArray[8] = jsKeyword8;
        JSKeyword jsKeyword9 = new JSKeyword(JSToken.Long, "long");
        jsKeywordArray[11] = jsKeyword9;
        JSKeyword jsKeyword10 = new JSKeyword(JSToken.New, "new",
            new JSKeyword(JSToken.Null, "null",
                new JSKeyword(JSToken.Native, "native", new JSKeyword(JSToken.Namespace, "namespace"))));
        jsKeywordArray[13] = jsKeyword10;
        JSKeyword jsKeyword11 = new JSKeyword(JSToken.Public, "public",
            new JSKeyword(JSToken.Package, "package",
                new JSKeyword(JSToken.Private, "private", new JSKeyword(JSToken.Protected, "protected"))));
        jsKeywordArray[15] = jsKeyword11;
        JSKeyword jsKeyword12 = new JSKeyword(JSToken.Return, "return", new JSKeyword(JSToken.Require, "require"));
        jsKeywordArray[17] = jsKeyword12;
        JSKeyword jsKeyword13 = new JSKeyword(JSToken.Set, "set",
            new JSKeyword(JSToken.Short, "short",
                new JSKeyword(JSToken.Super, "super",
                    new JSKeyword(JSToken.Static, "static",
                        new JSKeyword(JSToken.Switch, "switch",
                            new JSKeyword(JSToken.Synchronized, "synchronized"))))));
        jsKeywordArray[18] = jsKeyword13;
        JSKeyword jsKeyword14 = new JSKeyword(JSToken.Try, "try",
            new JSKeyword(JSToken.This, "this",
                new JSKeyword(JSToken.True, "true",
                    new JSKeyword(JSToken.Throw, "throw",
                        new JSKeyword(JSToken.Throws, "throws",
                            new JSKeyword(JSToken.Typeof, "typeof", new JSKeyword(JSToken.Transient, "transient")))))));
        jsKeywordArray[19] = jsKeyword14;
        JSKeyword jsKeyword15 = new JSKeyword(JSToken.Var, "var",
            new JSKeyword(JSToken.Void, "void", new JSKeyword(JSToken.Volatile, "volatile")));
        jsKeywordArray[21] = jsKeyword15;
        JSKeyword jsKeyword16 = new JSKeyword(JSToken.Use, "use");
        jsKeywordArray[20] = jsKeyword16;
        JSKeyword jsKeyword17 = new JSKeyword(JSToken.With, "with", new JSKeyword(JSToken.While, "while"));
        jsKeywordArray[22] = jsKeyword17;
        return jsKeywordArray;
    }
}