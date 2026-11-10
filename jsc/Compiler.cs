// Decompiled with JetBrains decompiler
// Type: Compiler
// Assembly: jsc, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: EFC0C76F-35B7-44DC-BADC-474CA93DF1B0
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\jsc.exe

using System;
using System.Reflection;
using System.Security.Policy;
using System.Text;

#nullable disable
internal class Compiler
{
    public static int Main(string[] args)
    {
        // .NET Core does not ship the legacy code pages (1252, 932, ...) by default;
        // jsc maps /codepage and the ANSI code page through Encoding.GetEncoding.
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        try
        {
            return new JScriptCompiler().Run(args);
        }
        catch (Exception ex)
        {
            Console.WriteLine(JScriptCompiler.Localize("INTERNAL COMPILER ERROR", ex.Message,
                JScriptCompiler.GetCultureInfo()));
            return 10;
        }
    }
}