// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.JSCodeGenerator
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using Microsoft.JScript.Vsa;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

#nullable disable
namespace Microsoft.JScript;

[DesignerCategory("code")]
internal sealed class JSCodeGenerator : CodeCompiler
{
    private const int MaxLineLength = 80 /*0x50*/;

    private const GeneratorSupport LanguageSupport = GeneratorSupport.ArraysOfArrays |
                                                     GeneratorSupport.EntryPointMethod |
                                                     GeneratorSupport.MultidimensionalArrays |
                                                     GeneratorSupport.StaticConstructors |
                                                     GeneratorSupport.TryCatchStatements |
                                                     GeneratorSupport.DeclareEnums |
                                                     GeneratorSupport.DeclareInterfaces |
                                                     GeneratorSupport.AssemblyAttributes |
                                                     GeneratorSupport.PublicStaticMembers;

    private bool forLoopHack;
    private bool isArgumentList = true;
    private static Hashtable keywords;
    private string mainClassName;
    private string mainMethodName;

    private static Regex outputReg =
        new Regex(
            "(([^(]+)(\\(([0-9]+),([0-9]+)\\))[ \\t]*:[ \\t]+)?(fatal )?(error|warning)[ \\t]+([A-Z]+[0-9]+)[ \\t]*:[ \\t]*(.*)");

    static JSCodeGenerator()
    {
        JSCodeGenerator.keywords = new Hashtable(150);
        object obj = new object();
        JSCodeGenerator.keywords[(object)"abstract"] = obj;
        JSCodeGenerator.keywords[(object)"assert"] = obj;
        JSCodeGenerator.keywords[(object)"boolean"] = obj;
        JSCodeGenerator.keywords[(object)"break"] = obj;
        JSCodeGenerator.keywords[(object)"byte"] = obj;
        JSCodeGenerator.keywords[(object)"case"] = obj;
        JSCodeGenerator.keywords[(object)"catch"] = obj;
        JSCodeGenerator.keywords[(object)"char"] = obj;
        JSCodeGenerator.keywords[(object)"class"] = obj;
        JSCodeGenerator.keywords[(object)"const"] = obj;
        JSCodeGenerator.keywords[(object)"continue"] = obj;
        JSCodeGenerator.keywords[(object)"debugger"] = obj;
        JSCodeGenerator.keywords[(object)"decimal"] = obj;
        JSCodeGenerator.keywords[(object)"default"] = obj;
        JSCodeGenerator.keywords[(object)"delete"] = obj;
        JSCodeGenerator.keywords[(object)"do"] = obj;
        JSCodeGenerator.keywords[(object)"double"] = obj;
        JSCodeGenerator.keywords[(object)"else"] = obj;
        JSCodeGenerator.keywords[(object)"ensure"] = obj;
        JSCodeGenerator.keywords[(object)"enum"] = obj;
        JSCodeGenerator.keywords[(object)"event"] = obj;
        JSCodeGenerator.keywords[(object)"export"] = obj;
        JSCodeGenerator.keywords[(object)"extends"] = obj;
        JSCodeGenerator.keywords[(object)"false"] = obj;
        JSCodeGenerator.keywords[(object)"final"] = obj;
        JSCodeGenerator.keywords[(object)"finally"] = obj;
        JSCodeGenerator.keywords[(object)"float"] = obj;
        JSCodeGenerator.keywords[(object)"for"] = obj;
        JSCodeGenerator.keywords[(object)"function"] = obj;
        JSCodeGenerator.keywords[(object)"get"] = obj;
        JSCodeGenerator.keywords[(object)"goto"] = obj;
        JSCodeGenerator.keywords[(object)"if"] = obj;
        JSCodeGenerator.keywords[(object)"implements"] = obj;
        JSCodeGenerator.keywords[(object)"import"] = obj;
        JSCodeGenerator.keywords[(object)"in"] = obj;
        JSCodeGenerator.keywords[(object)"instanceof"] = obj;
        JSCodeGenerator.keywords[(object)"int"] = obj;
        JSCodeGenerator.keywords[(object)"invariant"] = obj;
        JSCodeGenerator.keywords[(object)"interface"] = obj;
        JSCodeGenerator.keywords[(object)"internal"] = obj;
        JSCodeGenerator.keywords[(object)"long"] = obj;
        JSCodeGenerator.keywords[(object)"namespace"] = obj;
        JSCodeGenerator.keywords[(object)"native"] = obj;
        JSCodeGenerator.keywords[(object)"new"] = obj;
        JSCodeGenerator.keywords[(object)"null"] = obj;
        JSCodeGenerator.keywords[(object)"package"] = obj;
        JSCodeGenerator.keywords[(object)"private"] = obj;
        JSCodeGenerator.keywords[(object)"protected"] = obj;
        JSCodeGenerator.keywords[(object)"public"] = obj;
        JSCodeGenerator.keywords[(object)"require"] = obj;
        JSCodeGenerator.keywords[(object)"return"] = obj;
        JSCodeGenerator.keywords[(object)"sbyte"] = obj;
        JSCodeGenerator.keywords[(object)"scope"] = obj;
        JSCodeGenerator.keywords[(object)"set"] = obj;
        JSCodeGenerator.keywords[(object)"short"] = obj;
        JSCodeGenerator.keywords[(object)"static"] = obj;
        JSCodeGenerator.keywords[(object)"super"] = obj;
        JSCodeGenerator.keywords[(object)"switch"] = obj;
        JSCodeGenerator.keywords[(object)"synchronized"] = obj;
        JSCodeGenerator.keywords[(object)"this"] = obj;
        JSCodeGenerator.keywords[(object)"throw"] = obj;
        JSCodeGenerator.keywords[(object)"throws"] = obj;
        JSCodeGenerator.keywords[(object)"transient"] = obj;
        JSCodeGenerator.keywords[(object)"true"] = obj;
        JSCodeGenerator.keywords[(object)"try"] = obj;
        JSCodeGenerator.keywords[(object)"typeof"] = obj;
        JSCodeGenerator.keywords[(object)"use"] = obj;
        JSCodeGenerator.keywords[(object)"uint"] = obj;
        JSCodeGenerator.keywords[(object)"ulong"] = obj;
        JSCodeGenerator.keywords[(object)"ushort"] = obj;
        JSCodeGenerator.keywords[(object)"var"] = obj;
        JSCodeGenerator.keywords[(object)"void"] = obj;
        JSCodeGenerator.keywords[(object)"volatile"] = obj;
        JSCodeGenerator.keywords[(object)"while"] = obj;
        JSCodeGenerator.keywords[(object)"with"] = obj;
    }

    protected override string CmdArgsFromParameters(CompilerParameters options)
    {
        StringBuilder stringBuilder = new StringBuilder(128 /*0x80*/);
        string str = Path.DirectorySeparatorChar == '/' ? "-" : "/";
        stringBuilder.Append(str + "utf8output ");
        object obj = new object();
        Hashtable hashtable = new Hashtable(20);
        foreach (string referencedAssembly in options.ReferencedAssemblies)
        {
            if (hashtable[(object)referencedAssembly] == null)
            {
                hashtable[(object)referencedAssembly] = obj;
                stringBuilder.Append(str + "r:");
                stringBuilder.Append("\"");
                stringBuilder.Append(referencedAssembly);
                stringBuilder.Append("\" ");
            }
        }

        stringBuilder.Append(str + "out:");
        stringBuilder.Append("\"");
        stringBuilder.Append(options.OutputAssembly);
        stringBuilder.Append("\" ");
        if (options.IncludeDebugInformation)
        {
            stringBuilder.Append(str + "d:DEBUG ");
            stringBuilder.Append(str + "debug+ ");
        }
        else
            stringBuilder.Append(str + "debug- ");

        if (options.TreatWarningsAsErrors)
            stringBuilder.Append(str + "warnaserror ");
        if (options.WarningLevel >= 0)
            stringBuilder.Append(
                $"{str}w:{options.WarningLevel.ToString((IFormatProvider)CultureInfo.InvariantCulture)} ");
        if (options.Win32Resource != null)
            stringBuilder.Append($"{str}win32res:\"{options.Win32Resource}\" ");
        return stringBuilder.ToString();
    }

    protected override string CompilerName => "jsc.exe";

    protected override string CreateEscapedIdentifier(string name)
    {
        return this.IsKeyword(name) ? "\\" + name : name;
    }

    protected override string CreateValidIdentifier(string name)
    {
        return this.IsKeyword(name) ? "$" + name : name;
    }

    protected override string FileExtension => ".js";

    protected override CompilerResults FromFileBatch(CompilerParameters options, string[] fileNames)
    {
        string str = options.TempFiles.AddExtension("out");
        CompilerResults results = new CompilerResults(options.TempFiles);
        if (options.OutputAssembly == null || options.OutputAssembly.Length == 0)
            options.OutputAssembly = results.TempFiles.AddExtension("dll", !options.GenerateInMemory);
        string partialCmdLine = (string)null;
        if (options.IncludeDebugInformation)
        {
            results.TempFiles.AddExtension("pdb");
            partialCmdLine = this.CmdArgsFromParameters(options);
        }

        results.NativeCompilerReturnValue = 0;
        JSInProcCompiler jsInProcCompiler = new JSInProcCompiler();
        try
        {
            results.NativeCompilerReturnValue = jsInProcCompiler.Compile(options, partialCmdLine, fileNames, str);
        }
        catch
        {
            results.NativeCompilerReturnValue = 10;
        }

        try
        {
            StreamReader streamReader = new StreamReader(str);
            try
            {
                for (string line = streamReader.ReadLine(); line != null; line = streamReader.ReadLine())
                {
                    results.Output.Add(line);
                    this.ProcessCompilerOutputLine(results, line);
                }
            }
            finally
            {
                streamReader.Close();
            }
        }
        catch (Exception ex)
        {
            results.Output.Add(JScriptException.Localize("No error output", CultureInfo.CurrentUICulture));
            results.Output.Add(ex.ToString());
        }

        if (results.NativeCompilerReturnValue == 0 && options.GenerateInMemory)
        {
            if (jsInProcCompiler.CompiledAssembly != (Assembly)null)
            {
                results.CompiledAssembly = jsInProcCompiler.CompiledAssembly;
            }
            else
            {
                FileStream fileStream =
                    new FileStream(options.OutputAssembly, FileMode.Open, FileAccess.Read, FileShare.Read);
                try
                {
                    int length = (int)fileStream.Length;
                    byte[] numArray = new byte[length];
                    fileStream.Read(numArray, 0, length);
                    results.CompiledAssembly = Assembly.Load(numArray, (byte[])null);
                }
                finally
                {
                    fileStream.Close();
                }
            }
        }
        else
            results.PathToAssembly = Path.GetFullPath(options.OutputAssembly);

        return results;
    }

    protected override void GenerateArgumentReferenceExpression(CodeArgumentReferenceExpression e)
    {
        this.OutputIdentifier(e.ParameterName);
    }

    protected override void GenerateArrayCreateExpression(CodeArrayCreateExpression e)
    {
        CodeExpressionCollection initializers = e.Initializers;
        if (initializers.Count > 0)
        {
            this.Output.Write("[");
            ++this.Indent;
            this.OutputExpressionList(initializers);
            --this.Indent;
            this.Output.Write("]");
        }
        else
        {
            this.Output.Write("new ");
            this.Output.Write(this.GetBaseTypeOutput(e.CreateType.BaseType));
            this.Output.Write("[");
            if (e.SizeExpression != null)
                this.GenerateExpression(e.SizeExpression);
            else
                this.Output.Write(e.Size.ToString((IFormatProvider)CultureInfo.InvariantCulture));
            this.Output.Write("]");
        }
    }

    protected override void GenerateArrayIndexerExpression(CodeArrayIndexerExpression e)
    {
        this.GenerateExpression(e.TargetObject);
        this.Output.Write("[");
        bool flag = true;
        foreach (CodeExpression index in (CollectionBase)e.Indices)
        {
            if (flag)
                flag = false;
            else
                this.Output.Write(", ");
            this.GenerateExpression(index);
        }

        this.Output.Write("]");
    }

    private void GenerateAssemblyAttributes(CodeAttributeDeclarationCollection attributes)
    {
        if (attributes.Count == 0)
            return;
        foreach (CodeAttributeDeclaration attribute in (CollectionBase)attributes)
        {
            this.Output.Write("[");
            this.Output.Write("assembly: ");
            CodeAttributeDeclaration attributeDeclaration = attribute;
            this.Output.Write(this.GetBaseTypeOutput(attributeDeclaration.Name));
            this.Output.Write("(");
            bool flag = true;
            foreach (CodeAttributeArgument attributeArgument in (CollectionBase)attributeDeclaration.Arguments)
            {
                if (flag)
                    flag = false;
                else
                    this.Output.Write(", ");
                this.OutputAttributeArgument(attributeArgument);
            }

            this.Output.Write(")");
            this.Output.Write("]");
            this.Output.WriteLine();
        }
    }

    protected override void GenerateAssignStatement(CodeAssignStatement e)
    {
        this.GenerateExpression(e.Left);
        this.Output.Write(" = ");
        this.GenerateExpression(e.Right);
        if (this.forLoopHack)
            return;
        this.Output.WriteLine(";");
    }

    protected override void GenerateAttachEventStatement(CodeAttachEventStatement e)
    {
        this.GenerateExpression(e.Event.TargetObject);
        this.Output.Write(".add_");
        this.Output.Write(e.Event.EventName);
        this.Output.Write("(");
        this.GenerateExpression(e.Listener);
        this.Output.WriteLine(");");
    }

    protected override void GenerateAttributeDeclarationsEnd(
        CodeAttributeDeclarationCollection attributes)
    {
    }

    protected override void GenerateAttributeDeclarationsStart(
        CodeAttributeDeclarationCollection attributes)
    {
    }

    protected override void GenerateBaseReferenceExpression(CodeBaseReferenceExpression e)
    {
        this.Output.Write("super");
    }

    private string GetBaseTypeOutput(string baseType)
    {
        if (baseType.Length == 0)
            return "void";
        if (string.Compare(baseType, "System.Byte", StringComparison.Ordinal) == 0)
            return "byte";
        if (string.Compare(baseType, "System.Int16", StringComparison.Ordinal) == 0)
            return "short";
        if (string.Compare(baseType, "System.Int32", StringComparison.Ordinal) == 0)
            return "int";
        if (string.Compare(baseType, "System.Int64", StringComparison.Ordinal) == 0)
            return "long";
        if (string.Compare(baseType, "System.SByte", StringComparison.Ordinal) == 0)
            return "sbyte";
        if (string.Compare(baseType, "System.UInt16", StringComparison.Ordinal) == 0)
            return "ushort";
        if (string.Compare(baseType, "System.UInt32", StringComparison.Ordinal) == 0)
            return "uint";
        if (string.Compare(baseType, "System.UInt64", StringComparison.Ordinal) == 0)
            return "ulong";
        if (string.Compare(baseType, "System.Decimal", StringComparison.Ordinal) == 0)
            return "decimal";
        if (string.Compare(baseType, "System.Single", StringComparison.Ordinal) == 0)
            return "float";
        if (string.Compare(baseType, "System.Double", StringComparison.Ordinal) == 0)
            return "double";
        if (string.Compare(baseType, "System.Boolean", StringComparison.Ordinal) == 0)
            return "boolean";
        if (string.Compare(baseType, "System.Char", StringComparison.Ordinal) == 0)
            return "char";
        baseType = baseType.Replace('+', '.');
        return this.CreateEscapedIdentifier(baseType);
    }

    protected override void GenerateCastExpression(CodeCastExpression e)
    {
        this.OutputType(e.TargetType);
        this.Output.Write("(");
        this.GenerateExpression(e.Expression);
        this.Output.Write(")");
    }

    protected override void GenerateComment(CodeComment e)
    {
        string text = e.Text;
        StringBuilder stringBuilder = new StringBuilder(text.Length * 2);
        string str = e.DocComment ? "///" : "//";
        stringBuilder.Append(str);
        for (int index = 0; index < text.Length; ++index)
        {
            switch (text[index])
            {
                case '\n':
                    stringBuilder.Append("\n" + str);
                    continue;
                case '\r':
                    if (index < text.Length - 1 && text[index + 1] == '\n')
                    {
                        stringBuilder.Append("\r\n" + str);
                        ++index;
                        continue;
                    }

                    stringBuilder.Append("\r" + str);
                    continue;
                case '@':
                    continue;
                case '\u2028':
                    stringBuilder.Append("\u2028" + str);
                    continue;
                case '\u2029':
                    stringBuilder.Append("\u2029" + str);
                    continue;
                default:
                    stringBuilder.Append(text[index]);
                    continue;
            }
        }

        this.Output.WriteLine(stringBuilder.ToString());
    }

    protected override void GenerateCompileUnitStart(CodeCompileUnit e)
    {
        this.Output.WriteLine("//------------------------------------------------------------------------------");
        this.Output.WriteLine("/// <autogenerated>");
        this.Output.WriteLine("///     This code was generated by a tool.");
        this.Output.WriteLine("///     Runtime Version: " + Environment.Version.ToString());
        this.Output.WriteLine("///");
        this.Output.WriteLine("///     Changes to this file may cause incorrect behavior and will be lost if ");
        this.Output.WriteLine("///     the code is regenerated.");
        this.Output.WriteLine("/// </autogenerated>");
        this.Output.WriteLine("//------------------------------------------------------------------------------");
        this.Output.WriteLine("");
        if (e.AssemblyCustomAttributes.Count <= 0)
            return;
        this.GenerateAssemblyAttributes(e.AssemblyCustomAttributes);
        this.Output.WriteLine("");
    }

    protected override void GenerateConditionStatement(CodeConditionStatement e)
    {
        this.Output.Write("if (");
        this.Indent += 2;
        this.GenerateExpression(e.Condition);
        this.Indent -= 2;
        this.Output.Write(")");
        this.OutputStartingBrace();
        ++this.Indent;
        this.GenerateStatements(e.TrueStatements);
        --this.Indent;
        if (e.FalseStatements.Count > 0)
        {
            this.Output.Write("}");
            if (this.Options.ElseOnClosing)
                this.Output.Write(" ");
            else
                this.Output.WriteLine("");
            this.Output.Write("else");
            this.OutputStartingBrace();
            ++this.Indent;
            this.GenerateStatements(e.FalseStatements);
            --this.Indent;
        }

        this.Output.WriteLine("}");
    }

    protected override void GenerateConstructor(CodeConstructor e, CodeTypeDeclaration c)
    {
        if (!this.IsCurrentClass && !this.IsCurrentStruct)
            return;
        this.OutputMemberAccessModifier(e.Attributes);
        if (e.CustomAttributes.Count > 0)
            this.OutputAttributeDeclarations(e.CustomAttributes);
        this.Output.Write("function ");
        this.OutputIdentifier(this.CurrentTypeName);
        this.Output.Write("(");
        this.OutputParameters(e.Parameters);
        this.Output.Write(")");
        CodeExpressionCollection baseConstructorArgs = e.BaseConstructorArgs;
        CodeExpressionCollection chainedConstructorArgs = e.ChainedConstructorArgs;
        this.OutputStartingBrace();
        ++this.Indent;
        if (baseConstructorArgs.Count > 0)
        {
            this.Output.Write("super(");
            this.OutputExpressionList(baseConstructorArgs);
            this.Output.WriteLine(");");
        }

        if (chainedConstructorArgs.Count > 0)
        {
            this.Output.Write("this(");
            this.OutputExpressionList(chainedConstructorArgs);
            this.Output.WriteLine(");");
        }

        this.GenerateStatements(e.Statements);
        this.Output.WriteLine();
        --this.Indent;
        this.Output.WriteLine("}");
    }

    protected override void GenerateDelegateCreateExpression(CodeDelegateCreateExpression e)
    {
        bool flag = e.DelegateType != null;
        if (flag)
        {
            this.OutputType(e.DelegateType);
            this.Output.Write("(");
        }

        this.GenerateExpression(e.TargetObject);
        this.Output.Write(".");
        this.OutputIdentifier(e.MethodName);
        if (!flag)
            return;
        this.Output.Write(")");
    }

    protected override void GenerateDelegateInvokeExpression(CodeDelegateInvokeExpression e)
    {
        if (e.TargetObject != null)
            this.GenerateExpression(e.TargetObject);
        this.Output.Write("(");
        this.OutputExpressionList(e.Parameters);
        this.Output.Write(")");
    }

    protected override void GenerateEntryPointMethod(CodeEntryPointMethod e, CodeTypeDeclaration c)
    {
        this.Output.Write("public static ");
        if (e.CustomAttributes.Count > 0)
            this.OutputAttributeDeclarations(e.CustomAttributes);
        this.Output.Write("function Main()");
        this.OutputStartingBrace();
        ++this.Indent;
        this.GenerateStatements(e.Statements);
        --this.Indent;
        this.Output.WriteLine("}");
        this.mainClassName = this.CurrentTypeName;
        this.mainMethodName = "Main";
    }

    protected override void GenerateEvent(CodeMemberEvent e, CodeTypeDeclaration c)
    {
        throw new Exception(JScriptException.Localize("No event declarations", CultureInfo.CurrentUICulture));
    }

    protected override void GenerateEventReferenceExpression(CodeEventReferenceExpression e)
    {
        throw new Exception(JScriptException.Localize("No event references", CultureInfo.CurrentUICulture));
    }

    protected override void GenerateExpressionStatement(CodeExpressionStatement e)
    {
        this.GenerateExpression(e.Expression);
        if (this.forLoopHack)
            return;
        this.Output.WriteLine(";");
    }

    protected override void GenerateField(CodeMemberField e)
    {
        if (this.IsCurrentDelegate || this.IsCurrentInterface)
            throw new Exception(JScriptException.Localize("Only methods on interfaces", CultureInfo.CurrentUICulture));
        if (this.IsCurrentEnum)
        {
            this.OutputIdentifier(e.Name);
            if (e.InitExpression != null)
            {
                this.Output.Write(" = ");
                this.GenerateExpression(e.InitExpression);
            }

            this.Output.WriteLine(",");
        }
        else
        {
            this.OutputMemberAccessModifier(e.Attributes);
            if ((e.Attributes & MemberAttributes.ScopeMask) == MemberAttributes.Static)
                this.Output.Write("static ");
            if (e.CustomAttributes.Count > 0)
            {
                this.OutputAttributeDeclarations(e.CustomAttributes);
                this.Output.WriteLine("");
            }

            if ((e.Attributes & MemberAttributes.Const) == MemberAttributes.Const)
            {
                if ((e.Attributes & MemberAttributes.ScopeMask) != MemberAttributes.Static)
                    this.Output.Write("static ");
                this.Output.Write("const ");
            }
            else
                this.Output.Write("var ");

            this.OutputTypeNamePair(e.Type, e.Name);
            if (e.InitExpression != null)
            {
                this.Output.Write(" = ");
                this.GenerateExpression(e.InitExpression);
            }

            this.Output.WriteLine(";");
        }
    }

    protected override void GenerateFieldReferenceExpression(CodeFieldReferenceExpression e)
    {
        if (e.TargetObject != null)
        {
            this.GenerateExpression(e.TargetObject);
            this.Output.Write(".");
        }

        this.OutputIdentifier(e.FieldName);
    }

    protected override void GenerateGotoStatement(CodeGotoStatement e)
    {
        throw new Exception(JScriptException.Localize("No goto statements", CultureInfo.CurrentUICulture));
    }

    protected override void GenerateIndexerExpression(CodeIndexerExpression e)
    {
        this.GenerateExpression(e.TargetObject);
        this.Output.Write("[");
        bool flag = true;
        foreach (CodeExpression index in (CollectionBase)e.Indices)
        {
            if (flag)
                flag = false;
            else
                this.Output.Write(", ");
            this.GenerateExpression(index);
        }

        this.Output.Write("]");
    }

    protected override void GenerateIterationStatement(CodeIterationStatement e)
    {
        this.forLoopHack = true;
        this.Output.Write("for (");
        this.GenerateStatement(e.InitStatement);
        this.Output.Write("; ");
        this.GenerateExpression(e.TestExpression);
        this.Output.Write("; ");
        this.GenerateStatement(e.IncrementStatement);
        this.Output.Write(")");
        this.OutputStartingBrace();
        this.forLoopHack = false;
        ++this.Indent;
        this.GenerateStatements(e.Statements);
        --this.Indent;
        this.Output.WriteLine("}");
    }

    protected override void GenerateLabeledStatement(CodeLabeledStatement e)
    {
        throw new Exception(JScriptException.Localize("No goto statements", CultureInfo.CurrentUICulture));
    }

    protected override void GenerateLinePragmaStart(CodeLinePragma e)
    {
        this.Output.WriteLine("");
        this.Output.WriteLine("//@cc_on");
        this.Output.Write("//@set @position(file=\"");
        this.Output.Write(Regex.Replace(e.FileName, "\\\\", "\\\\"));
        this.Output.Write("\";line=");
        this.Output.Write(e.LineNumber.ToString((IFormatProvider)CultureInfo.InvariantCulture));
        this.Output.WriteLine(")");
    }

    protected override void GenerateLinePragmaEnd(CodeLinePragma e)
    {
        this.Output.WriteLine("");
        this.Output.WriteLine("//@set @position(end)");
    }

    protected override void GenerateMethod(CodeMemberMethod e, CodeTypeDeclaration c)
    {
        if (!this.IsCurrentInterface)
        {
            if (e.PrivateImplementationType == null)
            {
                this.OutputMemberAccessModifier(e.Attributes);
                this.OutputMemberVTableModifier(e.Attributes);
                this.OutputMemberScopeModifier(e.Attributes);
            }
        }
        else
            this.OutputMemberVTableModifier(e.Attributes);

        if (e.CustomAttributes.Count > 0)
            this.OutputAttributeDeclarations(e.CustomAttributes);
        this.Output.Write("function ");
        if (e.PrivateImplementationType != null && !this.IsCurrentInterface)
        {
            this.Output.Write(e.PrivateImplementationType.BaseType);
            this.Output.Write(".");
        }

        this.OutputIdentifier(e.Name);
        this.Output.Write("(");
        this.isArgumentList = false;
        try
        {
            this.OutputParameters(e.Parameters);
        }
        finally
        {
            this.isArgumentList = true;
        }

        this.Output.Write(")");
        if (e.ReturnType.BaseType.Length > 0 &&
            string.Compare(e.ReturnType.BaseType, typeof(void).FullName, StringComparison.Ordinal) != 0)
        {
            this.Output.Write(" : ");
            this.OutputType(e.ReturnType);
        }

        if (!this.IsCurrentInterface && (e.Attributes & MemberAttributes.ScopeMask) != MemberAttributes.Abstract)
        {
            this.OutputStartingBrace();
            ++this.Indent;
            this.GenerateStatements(e.Statements);
            --this.Indent;
            this.Output.WriteLine("}");
        }
        else
            this.Output.WriteLine(";");
    }

    protected override void GenerateMethodInvokeExpression(CodeMethodInvokeExpression e)
    {
        this.GenerateMethodReferenceExpression(e.Method);
        this.Output.Write("(");
        this.OutputExpressionList(e.Parameters);
        this.Output.Write(")");
    }

    protected override void GenerateMethodReferenceExpression(CodeMethodReferenceExpression e)
    {
        if (e.TargetObject != null)
        {
            if (e.TargetObject is CodeBinaryOperatorExpression)
            {
                this.Output.Write("(");
                this.GenerateExpression(e.TargetObject);
                this.Output.Write(")");
            }
            else
                this.GenerateExpression(e.TargetObject);

            this.Output.Write(".");
        }

        this.OutputIdentifier(e.MethodName);
    }

    protected override void GenerateMethodReturnStatement(CodeMethodReturnStatement e)
    {
        this.Output.Write("return");
        if (e.Expression != null)
        {
            this.Output.Write(" ");
            this.GenerateExpression(e.Expression);
        }

        this.Output.WriteLine(";");
    }

    protected override void GenerateNamespace(CodeNamespace e)
    {
        this.Output.WriteLine("//@cc_on");
        this.Output.WriteLine("//@set @debug(off)");
        this.Output.WriteLine("");
        this.GenerateNamespaceImports(e);
        this.Output.WriteLine("");
        this.GenerateCommentStatements(e.Comments);
        this.GenerateNamespaceStart(e);
        this.GenerateTypes(e);
        this.GenerateNamespaceEnd(e);
    }

    protected override void GenerateNamespaceEnd(CodeNamespace e)
    {
        if (e.Name != null && e.Name.Length > 0)
        {
            --this.Indent;
            this.Output.WriteLine("}");
        }

        if (this.mainClassName == null)
            return;
        if (e.Name != null)
        {
            this.OutputIdentifier(e.Name);
            this.Output.Write(".");
        }

        this.OutputIdentifier(this.mainClassName);
        this.Output.Write(".");
        this.OutputIdentifier(this.mainMethodName);
        this.Output.WriteLine("();");
        this.mainClassName = (string)null;
    }

    protected override void GenerateNamespaceImport(CodeNamespaceImport e)
    {
        this.Output.Write("import ");
        this.OutputIdentifier(e.Namespace);
        this.Output.WriteLine(";");
    }

    protected override void GenerateNamespaceStart(CodeNamespace e)
    {
        if (e.Name == null || e.Name.Length <= 0)
            return;
        this.Output.Write("package ");
        this.OutputIdentifier(e.Name);
        this.OutputStartingBrace();
        ++this.Indent;
    }

    protected override void GenerateObjectCreateExpression(CodeObjectCreateExpression e)
    {
        this.Output.Write("new ");
        this.OutputType(e.CreateType);
        this.Output.Write("(");
        this.OutputExpressionList(e.Parameters);
        this.Output.Write(")");
    }

    protected override void GenerateParameterDeclarationExpression(
        CodeParameterDeclarationExpression e)
    {
        if (e.CustomAttributes.Count > 0)
        {
            if (!(e.CustomAttributes[0].Name == "ParamArrayAttribute"))
                throw new Exception(JScriptException.Localize("No parameter attributes", CultureInfo.CurrentUICulture));
            this.Output.Write("... ");
        }

        this.OutputDirection(e.Direction);
        this.OutputTypeNamePair(e.Type, e.Name);
    }

    protected override void GenerateProperty(CodeMemberProperty e, CodeTypeDeclaration c)
    {
        if (!this.IsCurrentClass && !this.IsCurrentStruct && !this.IsCurrentInterface)
            return;
        if (e.HasGet)
        {
            if (!this.IsCurrentInterface)
            {
                if (e.PrivateImplementationType == null)
                {
                    this.OutputMemberAccessModifier(e.Attributes);
                    this.OutputMemberVTableModifier(e.Attributes);
                    this.OutputMemberScopeModifier(e.Attributes);
                }
            }
            else
                this.OutputMemberVTableModifier(e.Attributes);

            if (e.CustomAttributes.Count > 0)
            {
                if (this.IsCurrentInterface)
                    this.Output.Write("public ");
                this.OutputAttributeDeclarations(e.CustomAttributes);
                this.Output.WriteLine("");
            }

            this.Output.Write("function get ");
            if (e.PrivateImplementationType != null && !this.IsCurrentInterface)
            {
                this.Output.Write(e.PrivateImplementationType.BaseType);
                this.Output.Write(".");
            }

            this.OutputIdentifier(e.Name);
            if (e.Parameters.Count > 0)
                throw new Exception(JScriptException.Localize("No indexer declarations", CultureInfo.CurrentUICulture));
            this.Output.Write("() : ");
            this.OutputType(e.Type);
            if (this.IsCurrentInterface || (e.Attributes & MemberAttributes.ScopeMask) == MemberAttributes.Abstract)
            {
                this.Output.WriteLine(";");
            }
            else
            {
                this.OutputStartingBrace();
                ++this.Indent;
                this.GenerateStatements(e.GetStatements);
                --this.Indent;
                this.Output.WriteLine("}");
            }
        }

        if (!e.HasSet)
            return;
        if (!this.IsCurrentInterface)
        {
            if (e.PrivateImplementationType == null)
            {
                this.OutputMemberAccessModifier(e.Attributes);
                this.OutputMemberVTableModifier(e.Attributes);
                this.OutputMemberScopeModifier(e.Attributes);
            }
        }
        else
            this.OutputMemberVTableModifier(e.Attributes);

        if (e.CustomAttributes.Count > 0 && !e.HasGet)
        {
            if (this.IsCurrentInterface)
                this.Output.Write("public ");
            this.OutputAttributeDeclarations(e.CustomAttributes);
            this.Output.WriteLine("");
        }

        this.Output.Write("function set ");
        if (e.PrivateImplementationType != null && !this.IsCurrentInterface)
        {
            this.Output.Write(e.PrivateImplementationType.BaseType);
            this.Output.Write(".");
        }

        this.OutputIdentifier(e.Name);
        this.Output.Write("(");
        this.OutputTypeNamePair(e.Type, "value");
        if (e.Parameters.Count > 0)
            throw new Exception(JScriptException.Localize("No indexer declarations", CultureInfo.CurrentUICulture));
        this.Output.Write(")");
        if (this.IsCurrentInterface || (e.Attributes & MemberAttributes.ScopeMask) == MemberAttributes.Abstract)
        {
            this.Output.WriteLine(";");
        }
        else
        {
            this.OutputStartingBrace();
            ++this.Indent;
            this.GenerateStatements(e.SetStatements);
            --this.Indent;
            this.Output.WriteLine("}");
        }
    }

    protected override void GeneratePropertyReferenceExpression(CodePropertyReferenceExpression e)
    {
        if (e.TargetObject != null)
        {
            this.GenerateExpression(e.TargetObject);
            this.Output.Write(".");
        }

        this.OutputIdentifier(e.PropertyName);
    }

    protected override void GeneratePropertySetValueReferenceExpression(
        CodePropertySetValueReferenceExpression e)
    {
        this.Output.Write("value");
    }

    protected override void GenerateRemoveEventStatement(CodeRemoveEventStatement e)
    {
        this.GenerateExpression(e.Event.TargetObject);
        this.Output.Write(".remove_");
        this.Output.Write(e.Event.EventName);
        this.Output.Write("(");
        this.GenerateExpression(e.Listener);
        this.Output.WriteLine(");");
    }

    protected override void GenerateSingleFloatValue(float s)
    {
        this.Output.Write("float(");
        this.Output.Write(s.ToString((IFormatProvider)CultureInfo.InvariantCulture));
        this.Output.Write(")");
    }

    protected override void GenerateSnippetExpression(CodeSnippetExpression e)
    {
        this.Output.Write(e.Value);
    }

    protected override void GenerateSnippetMember(CodeSnippetTypeMember e)
    {
        this.Output.Write(e.Text);
    }

    protected override void GenerateSnippetStatement(CodeSnippetStatement e)
    {
        this.Output.WriteLine(e.Value);
    }

    protected override void GenerateThisReferenceExpression(CodeThisReferenceExpression e)
    {
        this.Output.Write("this");
    }

    protected override void GenerateThrowExceptionStatement(CodeThrowExceptionStatement e)
    {
        this.Output.Write("throw");
        if (e.ToThrow != null)
        {
            this.Output.Write(" ");
            this.GenerateExpression(e.ToThrow);
        }

        this.Output.WriteLine(";");
    }

    protected override void GenerateTryCatchFinallyStatement(CodeTryCatchFinallyStatement e)
    {
        this.Output.Write("try");
        this.OutputStartingBrace();
        ++this.Indent;
        this.GenerateStatements(e.TryStatements);
        --this.Indent;
        CodeCatchClauseCollection catchClauses = e.CatchClauses;
        if (catchClauses.Count > 0)
        {
            foreach (CodeCatchClause codeCatchClause1 in (CollectionBase)catchClauses)
            {
                this.Output.Write("}");
                if (this.Options.ElseOnClosing)
                    this.Output.Write(" ");
                else
                    this.Output.WriteLine("");
                CodeCatchClause codeCatchClause2 = codeCatchClause1;
                this.Output.Write("catch (");
                this.OutputIdentifier(codeCatchClause2.LocalName);
                this.Output.Write(" : ");
                this.OutputType(codeCatchClause2.CatchExceptionType);
                this.Output.Write(")");
                this.OutputStartingBrace();
                ++this.Indent;
                this.GenerateStatements(codeCatchClause2.Statements);
                --this.Indent;
            }
        }

        CodeStatementCollection finallyStatements = e.FinallyStatements;
        if (finallyStatements.Count > 0)
        {
            this.Output.Write("}");
            if (this.Options.ElseOnClosing)
                this.Output.Write(" ");
            else
                this.Output.WriteLine("");
            this.Output.Write("finally");
            this.OutputStartingBrace();
            ++this.Indent;
            this.GenerateStatements(finallyStatements);
            --this.Indent;
        }

        this.Output.WriteLine("}");
    }

    protected override void GenerateTypeConstructor(CodeTypeConstructor e)
    {
        if (!this.IsCurrentClass && !this.IsCurrentStruct)
            return;
        this.Output.Write("static ");
        this.OutputIdentifier(this.CurrentTypeName);
        this.OutputStartingBrace();
        ++this.Indent;
        this.GenerateStatements(e.Statements);
        --this.Indent;
        this.Output.WriteLine("}");
    }

    protected override void GenerateTypeEnd(CodeTypeDeclaration e)
    {
        if (this.IsCurrentDelegate)
            return;
        --this.Indent;
        this.Output.WriteLine("}");
    }

    protected override void GenerateTypeOfExpression(CodeTypeOfExpression e)
    {
        this.OutputType(e.Type);
    }

    protected override string GetTypeOutput(CodeTypeReference typeRef)
    {
        string typeOutput = typeRef.ArrayElementType == null
            ? this.GetBaseTypeOutput(typeRef.BaseType)
            : this.GetTypeOutput(typeRef.ArrayElementType);
        if (typeRef.ArrayRank > 0)
        {
            char[] chArray = new char[typeRef.ArrayRank + 1];
            chArray[0] = '[';
            chArray[typeRef.ArrayRank] = ']';
            for (int index = 1; index < typeRef.ArrayRank; ++index)
                chArray[index] = ',';
            typeOutput += new string(chArray);
        }

        return typeOutput;
    }

    protected override void GenerateTypeStart(CodeTypeDeclaration e)
    {
        if (this.IsCurrentDelegate)
            throw new Exception(JScriptException.Localize("No delegate declarations", CultureInfo.CurrentUICulture));
        this.OutputTypeVisibility(e.TypeAttributes);
        if (e.CustomAttributes.Count > 0)
        {
            this.OutputAttributeDeclarations(e.CustomAttributes);
            this.Output.WriteLine("");
        }

        this.OutputTypeAttributes(e.TypeAttributes, this.IsCurrentStruct, this.IsCurrentEnum);
        this.OutputIdentifier(e.Name);
        if (this.IsCurrentEnum)
        {
            if (e.BaseTypes.Count > 1)
                throw new Exception(JScriptException.Localize("Too many base types", CultureInfo.CurrentUICulture));
            if (e.BaseTypes.Count == 1)
            {
                this.Output.Write(" : ");
                this.OutputType(e.BaseTypes[0]);
            }
        }
        else
        {
            bool flag1 = true;
            bool flag2 = false;
            foreach (CodeTypeReference baseType in (CollectionBase)e.BaseTypes)
            {
                if (flag1)
                {
                    this.Output.Write(" extends ");
                    flag1 = false;
                    flag2 = true;
                }
                else if (flag2)
                {
                    this.Output.Write(" implements ");
                    flag2 = false;
                }
                else
                    this.Output.Write(", ");

                this.OutputType(baseType);
            }
        }

        this.OutputStartingBrace();
        ++this.Indent;
    }

    protected override void GenerateVariableDeclarationStatement(CodeVariableDeclarationStatement e)
    {
        this.Output.Write("var ");
        this.OutputTypeNamePair(e.Type, e.Name);
        if (e.InitExpression != null)
        {
            this.Output.Write(" = ");
            this.GenerateExpression(e.InitExpression);
        }

        this.Output.WriteLine(";");
    }

    protected override void GenerateVariableReferenceExpression(CodeVariableReferenceExpression e)
    {
        this.OutputIdentifier(e.VariableName);
    }

    private bool IsKeyword(string value) => JSCodeGenerator.keywords.ContainsKey((object)value);

    protected override bool IsValidIdentifier(string value)
    {
        return value != null && value.Length != 0 && VsaEngine.CreateEngine().IsValidIdentifier(value);
    }

    protected override string NullToken => "null";

    protected override void OutputAttributeDeclarations(CodeAttributeDeclarationCollection attributes)
    {
        if (attributes.Count == 0)
            return;
        this.GenerateAttributeDeclarationsStart(attributes);
        foreach (CodeAttributeDeclaration attribute in (CollectionBase)attributes)
        {
            this.Output.Write(this.GetBaseTypeOutput(attribute.Name));
            this.Output.Write("(");
            bool flag = true;
            foreach (CodeAttributeArgument attributeArgument in (CollectionBase)attribute.Arguments)
            {
                if (flag)
                    flag = false;
                else
                    this.Output.Write(", ");
                this.OutputAttributeArgument(attributeArgument);
            }

            this.Output.Write(") ");
        }

        this.GenerateAttributeDeclarationsEnd(attributes);
    }

    protected override void OutputDirection(FieldDirection dir)
    {
        switch (dir)
        {
            case FieldDirection.Out:
            case FieldDirection.Ref:
                if (!this.isArgumentList)
                    throw new Exception(JScriptException.Localize("No parameter direction",
                        CultureInfo.CurrentUICulture));
                this.Output.Write("&");
                break;
        }
    }

    protected override void OutputIdentifier(string ident)
    {
        this.Output.Write(this.CreateEscapedIdentifier(ident));
    }

    protected override void OutputMemberAccessModifier(MemberAttributes attributes)
    {
        switch (attributes & MemberAttributes.AccessMask)
        {
            case MemberAttributes.Assembly:
                this.Output.Write("internal ");
                break;
            case MemberAttributes.FamilyAndAssembly:
                this.Output.Write("internal ");
                break;
            case MemberAttributes.Family:
                this.Output.Write("protected ");
                break;
            case MemberAttributes.FamilyOrAssembly:
                this.Output.Write("protected internal ");
                break;
            case MemberAttributes.Public:
                this.Output.Write("public ");
                break;
            default:
                this.Output.Write("private ");
                break;
        }
    }

    protected override void OutputMemberScopeModifier(MemberAttributes attributes)
    {
        switch (attributes & MemberAttributes.ScopeMask)
        {
            case MemberAttributes.Abstract:
                this.Output.Write("abstract ");
                break;
            case MemberAttributes.Final:
                this.Output.Write("final ");
                break;
            case MemberAttributes.Static:
                this.Output.Write("static ");
                break;
            case MemberAttributes.Override:
                this.Output.Write("override ");
                break;
        }
    }

    private void OutputMemberVTableModifier(MemberAttributes attributes)
    {
        if ((attributes & MemberAttributes.VTableMask) != MemberAttributes.New)
            return;
        this.Output.Write("hide ");
    }

    protected override void OutputParameters(
        CodeParameterDeclarationExpressionCollection parameters)
    {
        bool flag = true;
        foreach (CodeParameterDeclarationExpression parameter in (CollectionBase)parameters)
        {
            if (flag)
                flag = false;
            else
                this.Output.Write(", ");
            this.GenerateExpression((CodeExpression)parameter);
        }
    }

    private void OutputStartingBrace()
    {
        if (this.Options.BracingStyle == "C")
        {
            this.Output.WriteLine("");
            this.Output.WriteLine("{");
        }
        else
            this.Output.WriteLine(" {");
    }

    protected override void OutputType(CodeTypeReference typeRef)
    {
        this.Output.Write(this.GetTypeOutput(typeRef));
    }

    protected override void OutputTypeAttributes(
        TypeAttributes attributes,
        bool isStruct,
        bool isEnum)
    {
        if (isEnum)
        {
            this.Output.Write("enum ");
        }
        else
        {
            switch (attributes & TypeAttributes.ClassSemanticsMask)
            {
                case TypeAttributes.NotPublic:
                    if ((attributes & TypeAttributes.Sealed) == TypeAttributes.Sealed)
                        this.Output.Write("final ");
                    if ((attributes & TypeAttributes.Abstract) == TypeAttributes.Abstract)
                        this.Output.Write("abstract ");
                    this.Output.Write("class ");
                    break;
                case TypeAttributes.ClassSemanticsMask:
                    this.Output.Write("interface ");
                    break;
            }
        }
    }

    protected override void OutputTypeNamePair(CodeTypeReference typeRef, string name)
    {
        this.OutputIdentifier(name);
        this.Output.Write(" : ");
        this.OutputType(typeRef);
    }

    private void OutputTypeVisibility(TypeAttributes attributes)
    {
        switch (attributes & TypeAttributes.VisibilityMask)
        {
            case TypeAttributes.NotPublic:
                this.Output.Write("internal ");
                break;
            case TypeAttributes.NestedPublic:
                this.Output.Write("public static ");
                break;
            case TypeAttributes.NestedPrivate:
                this.Output.Write("private static ");
                break;
            case TypeAttributes.NestedFamily:
                this.Output.Write("protected static ");
                break;
            case TypeAttributes.NestedAssembly:
            case TypeAttributes.NestedFamANDAssem:
                this.Output.Write("internal static ");
                break;
            case TypeAttributes.VisibilityMask:
                this.Output.Write("protected internal static ");
                break;
            default:
                this.Output.Write("public ");
                break;
        }
    }

    protected override void ProcessCompilerOutputLine(CompilerResults results, string line)
    {
        Match match = JSCodeGenerator.outputReg.Match(line);
        if (!match.Success)
            return;
        CompilerError compilerError = new CompilerError();
        if (match.Groups[1].Success)
        {
            compilerError.FileName = match.Groups[2].Value;
            compilerError.Line = int.Parse(match.Groups[4].Value, (IFormatProvider)CultureInfo.InvariantCulture);
            compilerError.Column = int.Parse(match.Groups[5].Value, (IFormatProvider)CultureInfo.InvariantCulture);
        }

        if (string.Compare(match.Groups[7].Value, "warning", StringComparison.OrdinalIgnoreCase) == 0)
            compilerError.IsWarning = true;
        compilerError.ErrorNumber = match.Groups[8].Value;
        compilerError.ErrorText = match.Groups[9].Value;
        results.Errors.Add(compilerError);
    }

    protected override string QuoteSnippetString(string value)
    {
        return this.QuoteSnippetStringCStyle(value);
    }

    private string QuoteSnippetStringCStyle(string value)
    {
        char[] charArray = value.ToCharArray();
        StringBuilder stringBuilder = new StringBuilder(value.Length + 5);
        stringBuilder.Append("\"");
        int num = 80 /*0x50*/;
        for (int index = 0; index < charArray.Length; ++index)
        {
            switch (charArray[index])
            {
                case char.MinValue:
                    stringBuilder.Append("\\0");
                    break;
                case '\t':
                    stringBuilder.Append("\\t");
                    break;
                case '\n':
                    stringBuilder.Append("\\n");
                    break;
                case '\r':
                    stringBuilder.Append("\\r");
                    break;
                case '"':
                    stringBuilder.Append("\\\"");
                    break;
                case '\'':
                    stringBuilder.Append("\\'");
                    break;
                case '\\':
                    stringBuilder.Append("\\\\");
                    break;
                case '\u2028':
                    stringBuilder.Append("\\u2028");
                    break;
                case '\u2029':
                    stringBuilder.Append("\\u2029");
                    break;
                default:
                    stringBuilder.Append(charArray[index]);
                    break;
            }

            if (index >= num && index + 1 < charArray.Length && (!this.IsSurrogateStart(charArray[index]) ||
                                                                 !this.IsSurrogateEnd(charArray[index + 1])))
            {
                num = index + 80 /*0x50*/;
                stringBuilder.Append("\" + \r\n\"");
            }
        }

        stringBuilder.Append("\"");
        return stringBuilder.ToString();
    }

    private bool IsSurrogateStart(char c) => '\uD800' <= c && c <= '\uDBFF';

    private bool IsSurrogateEnd(char c) => '\uDC00' <= c && c <= '\uDFFF';

    protected override void GeneratePrimitiveExpression(CodePrimitiveExpression e)
    {
        if (e.Value == null)
            this.Output.Write("undefined");
        else if (e.Value is DBNull)
            this.Output.Write("null");
        else if (e.Value is char)
            this.GeneratePrimitiveChar((char)e.Value);
        else
            base.GeneratePrimitiveExpression(e);
    }

    private void GeneratePrimitiveChar(char c)
    {
        this.Output.Write('\'');
        switch (c)
        {
            case char.MinValue:
                this.Output.Write("\\0");
                break;
            case '\t':
                this.Output.Write("\\t");
                break;
            case '\n':
                this.Output.Write("\\n");
                break;
            case '\r':
                this.Output.Write("\\r");
                break;
            case '"':
                this.Output.Write("\\\"");
                break;
            case '\'':
                this.Output.Write("\\'");
                break;
            case '\\':
                this.Output.Write("\\\\");
                break;
            case '\u2028':
                this.Output.Write("\\u2028");
                break;
            case '\u2029':
                this.Output.Write("\\u2029");
                break;
            default:
                this.Output.Write(c);
                break;
        }

        this.Output.Write('\'');
    }

    protected override bool Supports(GeneratorSupport support)
    {
        return (support & (GeneratorSupport.ArraysOfArrays | GeneratorSupport.EntryPointMethod |
                           GeneratorSupport.MultidimensionalArrays | GeneratorSupport.StaticConstructors |
                           GeneratorSupport.TryCatchStatements | GeneratorSupport.DeclareEnums |
                           GeneratorSupport.DeclareInterfaces | GeneratorSupport.AssemblyAttributes |
                           GeneratorSupport.PublicStaticMembers)) == support;
    }
}