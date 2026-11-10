// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.JSInProcCompiler
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using Microsoft.JScript.Vsa;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;

#nullable disable
namespace Microsoft.JScript;

internal class JSInProcCompiler
{
    private int codeItemCounter;
    private string debugCommandLine;
    internal Assembly CompiledAssembly;

    private void AddAssemblyReference(IJSVsaEngine engine, string filename)
    {
        ((IJSVsaReferenceItem)engine.Items.CreateItem(filename, JSVsaItemType.Reference, JSVsaItemFlag.None))
            .AssemblyName = filename;
    }

    private void AddDefinition(string def, Hashtable definitions, VsaEngine engine)
    {
        int length = def.IndexOf("=");
        string str1;
        object obj;
        if (length == -1)
        {
            str1 = def.Trim();
            obj = (object)true;
        }
        else
        {
            str1 = def.Substring(0, length).Trim();
            string str2 = def.Substring(length + 1).Trim();
            if (string.Compare(str2, "true", StringComparison.OrdinalIgnoreCase) == 0)
                obj = (object)true;
            else if (string.Compare(str2, "false", StringComparison.OrdinalIgnoreCase) == 0)
            {
                obj = (object)false;
            }
            else
            {
                try
                {
                    obj = (object)int.Parse(str2, (IFormatProvider)CultureInfo.InvariantCulture);
                }
                catch
                {
                    throw new CmdLineException(CmdLineError.InvalidDefinition, str1, engine.ErrorCultureInfo);
                }
            }
        }

        if (str1.Length == 0)
            throw new CmdLineException(CmdLineError.MissingDefineArgument, engine.ErrorCultureInfo);
        definitions[(object)str1] = obj;
    }

    private void AddResourceFile(
        ResInfo resinfo,
        Hashtable resources,
        Hashtable resourceFiles,
        VsaEngine engine)
    {
        if (!File.Exists(resinfo.fullpath))
            throw new CmdLineException(CmdLineError.ManagedResourceNotFound, resinfo.filename, engine.ErrorCultureInfo);
        if (resourceFiles[(object)resinfo.fullpath] != null)
            throw new CmdLineException(CmdLineError.DuplicateResourceFile, resinfo.filename, engine.ErrorCultureInfo);
        if (resources[(object)resinfo.name] != null)
            throw new CmdLineException(CmdLineError.DuplicateResourceName, resinfo.name, engine.ErrorCultureInfo);
        resources[(object)resinfo.name] = (object)resinfo;
        resourceFiles[(object)resinfo.fullpath] = (object)resinfo;
    }

    private void AddSourceFile(VsaEngine engine, string filename)
    {
        string name = "$SourceFile_" + this.codeItemCounter++.ToString();
        IJSVsaCodeItem jsVsaCodeItem =
            (IJSVsaCodeItem)engine.Items.CreateItem(name, JSVsaItemType.Code, JSVsaItemFlag.None);
        jsVsaCodeItem.SetOption("codebase", (object)filename);
        jsVsaCodeItem.SourceText = this.ReadFile(filename, engine);
    }

    internal int Compile(
        CompilerParameters options,
        string partialCmdLine,
        string[] sourceFiles,
        string outputFile)
    {
        StreamWriter output = (StreamWriter)null;
        int num = 0;
        try
        {
            output = new StreamWriter(outputFile);
            output.AutoFlush = true;
            if (options.IncludeDebugInformation)
            {
                this.PrintOptions((TextWriter)output, options);
                this.debugCommandLine = partialCmdLine;
            }

            VsaEngine engine = (VsaEngine)null;
            try
            {
                engine = this.CreateAndInitEngine(options, sourceFiles, outputFile, (TextWriter)output);
            }
            catch (CmdLineException ex)
            {
                output.WriteLine(ex.Message);
                num = 10;
            }
            catch (Exception ex)
            {
                output.WriteLine("fatal error JS2999: " + ex?.ToString());
                num = 10;
            }

            if (engine == null)
                return num;
            if (options.IncludeDebugInformation)
            {
                StringBuilder stringBuilder1 = new StringBuilder(this.debugCommandLine);
                foreach (string sourceFile in sourceFiles)
                {
                    stringBuilder1.Append(" \"");
                    stringBuilder1.Append(sourceFile);
                    stringBuilder1.Append("\"");
                }

                this.debugCommandLine = stringBuilder1.ToString();
                string path = options.TempFiles.AddExtension("cmdline");
                StreamWriter streamWriter = (StreamWriter)null;
                try
                {
                    streamWriter = new StreamWriter(path);
                    streamWriter.WriteLine(this.debugCommandLine);
                    streamWriter.Flush();
                }
                finally
                {
                    streamWriter?.Close();
                }

                StringBuilder stringBuilder2 = new StringBuilder();
                stringBuilder2.Append(Environment.NewLine);
                stringBuilder2.Append(JScriptException.Localize("CmdLine helper", CultureInfo.CurrentUICulture));
                stringBuilder2.Append(":");
                stringBuilder2.Append(Environment.NewLine);
                stringBuilder2.Append("    ");
                stringBuilder2.Append(options.TempFiles.TempDir);
                stringBuilder2.Append("> jsc.exe @\"");
                stringBuilder2.Append(path);
                stringBuilder2.Append("\"");
                stringBuilder2.Append(Environment.NewLine);
                output.WriteLine(stringBuilder2.ToString());
                this.PrintBanner(engine, (TextWriter)output);
            }

            try
            {
                num = engine.Compile() ? 0 : 10;
                if (num == 0)
                    this.CompiledAssembly = engine.GetAssembly();
            }
            catch (JSVsaException ex)
            {
                if (ex.ErrorCode == JSVsaError.AssemblyExpected)
                {
                    if (ex.InnerException != null && ex.InnerException is BadImageFormatException)
                    {
                        CmdLineException cmdLineException = new CmdLineException(CmdLineError.InvalidAssembly,
                            ex.Message, engine.ErrorCultureInfo);
                        output.WriteLine(cmdLineException.Message);
                    }
                    else if (ex.InnerException != null && ex.InnerException is FileNotFoundException)
                    {
                        CmdLineException cmdLineException = new CmdLineException(CmdLineError.AssemblyNotFound,
                            ex.Message, engine.ErrorCultureInfo);
                        output.WriteLine(cmdLineException.Message);
                    }
                    else
                    {
                        CmdLineException cmdLineException =
                            new CmdLineException(CmdLineError.InvalidAssembly, engine.ErrorCultureInfo);
                        output.WriteLine(cmdLineException.Message);
                    }
                }
                else if (ex.ErrorCode == JSVsaError.SaveCompiledStateFailed)
                {
                    CmdLineException cmdLineException = new CmdLineException(CmdLineError.ErrorSavingCompiledState,
                        ex.Message, engine.ErrorCultureInfo);
                    output.WriteLine(cmdLineException.Message);
                }
                else
                {
                    output.WriteLine(JScriptException.Localize("INTERNAL COMPILER ERROR", engine.ErrorCultureInfo));
                    output.WriteLine((object)ex);
                }

                num = 10;
            }
            catch (Exception ex)
            {
                output.WriteLine(JScriptException.Localize("INTERNAL COMPILER ERROR", engine.ErrorCultureInfo));
                output.WriteLine((object)ex);
                num = 10;
            }
        }
        finally
        {
            output?.Close();
        }

        return num;
    }

    private VsaEngine CreateAndInitEngine(
        CompilerParameters options,
        string[] sourceFiles,
        string outputFile,
        TextWriter output)
    {
        VsaEngine engine = new VsaEngine(true);
        VsaSite site = new VsaSite(output);
        engine.InitVsaEngine("JSCodeGenerator://Microsoft.JScript.Vsa.VsaEngine", (IJSVsaSite)site);
        this.ValidateOptions(options, engine);
        engine.GenerateDebugInfo = options.IncludeDebugInformation;
        engine.SetOption("referenceLoaderAPI", (object)"LoadFile");
        engine.SetOption("fast", (object)true);
        engine.SetOption("print", (object)false);
        engine.SetOption("VersionSafe", (object)false);
        if (!options.GenerateInMemory)
            engine.SetOption(nameof(output), (object)options.OutputAssembly);
        if (options.GenerateExecutable)
            engine.SetOption("PEFileKind", (object)PEFileKinds.ConsoleApplication);
        else
            engine.SetOption("PEFileKind", (object)PEFileKinds.Dll);
        site.treatWarningsAsErrors = options.TreatWarningsAsErrors;
        engine.SetOption("warnaserror", (object)options.TreatWarningsAsErrors);
        site.warningLevel = options.WarningLevel;
        engine.SetOption("WarningLevel", (object)options.WarningLevel);
        if (options.Win32Resource != null && options.Win32Resource.Length > 0)
            engine.SetOption("win32resource", (object)options.Win32Resource);
        bool flag = false;
        foreach (string referencedAssembly in options.ReferencedAssemblies)
        {
            if (string.Compare(Path.GetFileName(referencedAssembly), "mscorlib.dll",
                    StringComparison.OrdinalIgnoreCase) == 0)
                flag = true;
            this.AddAssemblyReference((IJSVsaEngine)engine, referencedAssembly);
        }

        if (!flag)
            this.AddAssemblyReference((IJSVsaEngine)engine, "mscorlib.dll");
        StringCollection args = this.SplitCmdLineArguments(options.CompilerOptions);
        this.ParseCompilerOptions(engine, args, output, options.GenerateExecutable);
        for (int index = 0; index < sourceFiles.Length; ++index)
            this.AddSourceFile(engine, sourceFiles[index]);
        return engine;
    }

    private void GetAllDefines(string definitionList, Hashtable defines, VsaEngine engine)
    {
        int startIndex = 0;
        int argumentSeparatorIndex;
        do
        {
            argumentSeparatorIndex = this.GetArgumentSeparatorIndex(definitionList, startIndex);
            this.AddDefinition(
                argumentSeparatorIndex != -1
                    ? definitionList.Substring(startIndex, argumentSeparatorIndex - startIndex)
                    : definitionList.Substring(startIndex), defines, engine);
            startIndex = argumentSeparatorIndex + 1;
        } while (argumentSeparatorIndex > -1);
    }

    private int GetArgumentSeparatorIndex(string argList, int startIndex)
    {
        int num1 = argList.IndexOf(",", startIndex);
        int num2 = argList.IndexOf(";", startIndex);
        return num1 == -1 || num2 != -1 && num1 >= num2 ? num2 : num1;
    }

    private void ParseCompilerOptions(
        VsaEngine engine,
        StringCollection args,
        TextWriter output,
        bool generateExe)
    {
        string str1 = Environment.GetEnvironmentVariable("LIB");
        bool flag1 = false;
        Hashtable defines = new Hashtable(10);
        Hashtable resources = new Hashtable(10);
        Hashtable resourceFiles = new Hashtable(10);
        bool flag2 = false;
        StringBuilder stringBuilder = (StringBuilder)null;
        if (this.debugCommandLine != null)
            stringBuilder = new StringBuilder(this.debugCommandLine);
        string str2 = Path.DirectorySeparatorChar == '/' ? "-" : "/";
        int index = 0;
        for (int count = args.Count; index < count; ++index)
        {
            string context1 = args[index];
            if (context1 != null && context1.Length != 0)
            {
                if (context1[0] == '@')
                    throw new CmdLineException(CmdLineError.InvalidForCompilerOptions, "@<filename>",
                        engine.ErrorCultureInfo);
                if ('-' == context1[0] || '/' == context1[0] && Path.DirectorySeparatorChar != '/')
                {
                    string option = context1.Substring(1);
                    if (option.Length > 0)
                    {
                        switch (option[0])
                        {
                            case '?':
                                throw new CmdLineException(CmdLineError.InvalidForCompilerOptions, "/?",
                                    engine.ErrorCultureInfo);
                            case 'A':
                            case 'a':
                                object obj1 = CmdLineOptionParser.IsBooleanOption(option, "autoref");
                                if (obj1 != null)
                                {
                                    engine.SetOption("autoref", obj1);
                                    if (stringBuilder != null)
                                    {
                                        stringBuilder.Append(context1);
                                        stringBuilder.Append(" ");
                                        continue;
                                    }

                                    continue;
                                }

                                break;
                            case 'C':
                            case 'c':
                                if ((object)CmdLineOptionParser.IsArgumentOption(option, "codepage") != null)
                                    throw new CmdLineException(CmdLineError.InvalidForCompilerOptions, "/codepage:<id>",
                                        engine.ErrorCultureInfo);
                                break;
                            case 'D':
                            case 'd':
                                object obj2 = CmdLineOptionParser.IsBooleanOption(option, "debug");
                                if (obj2 != null)
                                {
                                    engine.GenerateDebugInfo = (bool)obj2;
                                    if (stringBuilder != null)
                                    {
                                        stringBuilder.Append(context1);
                                        stringBuilder.Append(" ");
                                        continue;
                                    }

                                    continue;
                                }

                                object definitionList =
                                    (object)CmdLineOptionParser.IsArgumentOption(option, "d", "define");
                                if (definitionList != null)
                                {
                                    this.GetAllDefines((string)definitionList, defines, engine);
                                    if (stringBuilder != null)
                                    {
                                        stringBuilder.Append(str2 + "d:\"");
                                        stringBuilder.Append((string)definitionList);
                                        stringBuilder.Append("\" ");
                                        continue;
                                    }

                                    continue;
                                }

                                break;
                            case 'F':
                            case 'f':
                                object obj3 = CmdLineOptionParser.IsBooleanOption(option, "fast");
                                if (obj3 != null)
                                {
                                    engine.SetOption("fast", obj3);
                                    if (stringBuilder != null)
                                    {
                                        stringBuilder.Append(context1);
                                        stringBuilder.Append(" ");
                                        continue;
                                    }

                                    continue;
                                }

                                break;
                            case 'L':
                            case 'l':
                                object obj4 = (object)CmdLineOptionParser.IsArgumentOption(option, "lcid");
                                if (obj4 != null)
                                {
                                    if (((string)obj4).Length == 0)
                                        throw new CmdLineException(CmdLineError.NoLocaleID, context1,
                                            engine.ErrorCultureInfo);
                                    try
                                    {
                                        engine.LCID = int.Parse((string)obj4,
                                            (IFormatProvider)CultureInfo.InvariantCulture);
                                        continue;
                                    }
                                    catch
                                    {
                                        throw new CmdLineException(CmdLineError.InvalidLocaleID, (string)obj4,
                                            engine.ErrorCultureInfo);
                                    }
                                }
                                else
                                {
                                    object obj5 = (object)CmdLineOptionParser.IsArgumentOption(option, "lib");
                                    if (obj5 != null)
                                    {
                                        string str3 = (string)obj5;
                                        if (str3.Length == 0)
                                            throw new CmdLineException(CmdLineError.MissingLibArgument,
                                                engine.ErrorCultureInfo);
                                        str1 = str3.Replace(',', Path.PathSeparator) + Path.PathSeparator.ToString() +
                                               str1;
                                        if (stringBuilder != null)
                                        {
                                            stringBuilder.Append(str2 + "lib:\"");
                                            stringBuilder.Append((string)obj5);
                                            stringBuilder.Append("\" ");
                                            continue;
                                        }

                                        continue;
                                    }

                                    object resinfo =
                                        (object)CmdLineOptionParser.IsArgumentOption(option, "linkres", "linkresource");
                                    if (resinfo != null)
                                    {
                                        try
                                        {
                                            this.AddResourceFile(new ResInfo((string)resinfo, true), resources,
                                                resourceFiles, engine);
                                            continue;
                                        }
                                        catch (CmdLineException ex)
                                        {
                                            throw;
                                        }
                                        catch
                                        {
                                            throw new CmdLineException(CmdLineError.ManagedResourceNotFound,
                                                engine.ErrorCultureInfo);
                                        }
                                    }
                                    else
                                        break;
                                }
                            case 'N':
                            case 'n':
                                if (CmdLineOptionParser.IsBooleanOption(option, "nologo") != null)
                                    throw new CmdLineException(CmdLineError.InvalidForCompilerOptions, "/nologo[+|-]",
                                        engine.ErrorCultureInfo);
                                if (CmdLineOptionParser.IsBooleanOption(option, "nostdlib") != null)
                                    throw new CmdLineException(CmdLineError.InvalidForCompilerOptions, "/nostdlib[+|-]",
                                        engine.ErrorCultureInfo);
                                break;
                            case 'O':
                            case 'o':
                                if ((object)CmdLineOptionParser.IsArgumentOption(option, "out") != null)
                                    throw new CmdLineException(CmdLineError.InvalidForCompilerOptions,
                                        "/out:<filename>", engine.ErrorCultureInfo);
                                break;
                            case 'P':
                            case 'p':
                                object obj6 = CmdLineOptionParser.IsBooleanOption(option, "print");
                                if (obj6 != null)
                                {
                                    engine.SetOption("print", obj6);
                                    if (stringBuilder != null)
                                    {
                                        stringBuilder.Append(context1);
                                        stringBuilder.Append(" ");
                                        continue;
                                    }

                                    continue;
                                }

                                object context2 = (object)CmdLineOptionParser.IsArgumentOption(option, "platform");
                                if (context2 != null)
                                {
                                    string strA = (string)context2;
                                    PortableExecutableKinds portableExecutableKinds;
                                    ImageFileMachine imageFileMachine;
                                    if (string.Compare(strA, "x86", StringComparison.OrdinalIgnoreCase) == 0)
                                    {
                                        portableExecutableKinds = PortableExecutableKinds.ILOnly |
                                                                  PortableExecutableKinds.Required32Bit;
                                        imageFileMachine = ImageFileMachine.I386;
                                    }
                                    else if (string.Compare(strA, "Itanium", StringComparison.OrdinalIgnoreCase) == 0)
                                    {
                                        portableExecutableKinds = PortableExecutableKinds.ILOnly |
                                                                  PortableExecutableKinds.PE32Plus;
                                        imageFileMachine = ImageFileMachine.IA64;
                                    }
                                    else if (string.Compare(strA, "x64", StringComparison.OrdinalIgnoreCase) == 0)
                                    {
                                        portableExecutableKinds = PortableExecutableKinds.ILOnly |
                                                                  PortableExecutableKinds.PE32Plus;
                                        imageFileMachine = ImageFileMachine.AMD64;
                                    }
                                    else
                                    {
                                        if (string.Compare(strA, "anycpu", StringComparison.OrdinalIgnoreCase) != 0)
                                            throw new CmdLineException(CmdLineError.InvalidPlatform, (string)context2,
                                                engine.ErrorCultureInfo);
                                        portableExecutableKinds = PortableExecutableKinds.ILOnly;
                                        imageFileMachine = ImageFileMachine.I386;
                                    }

                                    engine.SetOption("PortableExecutableKind", (object)portableExecutableKinds);
                                    engine.SetOption("ImageFileMachine", (object)imageFileMachine);
                                    if (stringBuilder != null)
                                    {
                                        stringBuilder.Append(context1);
                                        stringBuilder.Append(" ");
                                        continue;
                                    }

                                    continue;
                                }

                                break;
                            case 'R':
                            case 'r':
                                object resinfo1 =
                                    (object)CmdLineOptionParser.IsArgumentOption(option, "r", "reference") == null
                                        ? (object)CmdLineOptionParser.IsArgumentOption(option, "res", "resource")
                                        : throw new CmdLineException(CmdLineError.InvalidForCompilerOptions,
                                            "/r[eference]:<file list>", engine.ErrorCultureInfo);
                                if (resinfo1 != null)
                                {
                                    try
                                    {
                                        this.AddResourceFile(new ResInfo((string)resinfo1, false), resources,
                                            resourceFiles, engine);
                                        continue;
                                    }
                                    catch (CmdLineException ex)
                                    {
                                        throw;
                                    }
                                    catch
                                    {
                                        throw new CmdLineException(CmdLineError.ManagedResourceNotFound,
                                            engine.ErrorCultureInfo);
                                    }
                                }
                                else
                                    break;
                            case 'T':
                            case 't':
                                object obj7 = (object)CmdLineOptionParser.IsArgumentOption(option, "t", "target");
                                if (obj7 != null)
                                {
                                    if (string.Compare((string)obj7, "exe", StringComparison.OrdinalIgnoreCase) == 0)
                                    {
                                        if (!generateExe)
                                            throw new CmdLineException(CmdLineError.IncompatibleTargets, context1,
                                                engine.ErrorCultureInfo);
                                        flag2 = !flag2
                                            ? true
                                            : throw new CmdLineException(CmdLineError.MultipleTargets,
                                                engine.ErrorCultureInfo);
                                        continue;
                                    }

                                    if (string.Compare((string)obj7, "winexe", StringComparison.OrdinalIgnoreCase) == 0)
                                    {
                                        if (!generateExe)
                                            throw new CmdLineException(CmdLineError.IncompatibleTargets, context1,
                                                engine.ErrorCultureInfo);
                                        if (flag2)
                                            throw new CmdLineException(CmdLineError.MultipleTargets,
                                                engine.ErrorCultureInfo);
                                        engine.SetOption("PEFileKind", (object)PEFileKinds.WindowApplication);
                                        flag1 = true;
                                        flag2 = true;
                                        continue;
                                    }

                                    if (string.Compare((string)obj7, "library", StringComparison.OrdinalIgnoreCase) !=
                                        0)
                                        throw new CmdLineException(CmdLineError.InvalidTarget, (string)obj7,
                                            engine.ErrorCultureInfo);
                                    if (generateExe)
                                        throw new CmdLineException(CmdLineError.IncompatibleTargets,
                                            engine.ErrorCultureInfo);
                                    flag2 = !flag2
                                        ? true
                                        : throw new CmdLineException(CmdLineError.MultipleTargets,
                                            engine.ErrorCultureInfo);
                                    continue;
                                }

                                break;
                            case 'U':
                            case 'u':
                                if ((object)CmdLineOptionParser.IsArgumentOption(option, "utf8output") != null)
                                    throw new CmdLineException(CmdLineError.InvalidForCompilerOptions,
                                        "/utf8output[+|-]", engine.ErrorCultureInfo);
                                break;
                            case 'V':
                            case 'v':
                                object obj8 = CmdLineOptionParser.IsBooleanOption(option, "VersionSafe");
                                if (obj8 != null)
                                {
                                    engine.SetOption("VersionSafe", obj8);
                                    if (stringBuilder != null)
                                    {
                                        stringBuilder.Append(context1);
                                        stringBuilder.Append(" ");
                                        continue;
                                    }

                                    continue;
                                }

                                break;
                            case 'W':
                            case 'w':
                                object obj9 = (object)CmdLineOptionParser.IsArgumentOption(option, "w", "warn");
                                if (obj9 != null)
                                {
                                    if (((string)obj9).Length == 0)
                                        throw new CmdLineException(CmdLineError.NoWarningLevel, context1,
                                            engine.ErrorCultureInfo);
                                    if (((string)obj9).Length == 1)
                                    {
                                        if (stringBuilder != null)
                                        {
                                            stringBuilder.Append(context1);
                                            stringBuilder.Append(" ");
                                        }

                                        switch (((string)obj9)[0])
                                        {
                                            case '0':
                                                engine.SetOption("WarningLevel", (object)0);
                                                continue;
                                            case '1':
                                                engine.SetOption("WarningLevel", (object)1);
                                                continue;
                                            case '2':
                                                engine.SetOption("WarningLevel", (object)2);
                                                continue;
                                            case '3':
                                                engine.SetOption("WarningLevel", (object)3);
                                                continue;
                                            case '4':
                                                engine.SetOption("WarningLevel", (object)4);
                                                continue;
                                        }
                                    }

                                    throw new CmdLineException(CmdLineError.InvalidWarningLevel, context1,
                                        engine.ErrorCultureInfo);
                                }

                                object obj10 = CmdLineOptionParser.IsBooleanOption(option, "warnaserror");
                                if (obj10 != null)
                                {
                                    engine.SetOption("warnaserror", obj10);
                                    if (stringBuilder != null)
                                    {
                                        stringBuilder.Append(context1);
                                        stringBuilder.Append(" ");
                                        continue;
                                    }

                                    continue;
                                }

                                if ((object)CmdLineOptionParser.IsArgumentOption(option, "win32res") != null)
                                    throw new CmdLineException(CmdLineError.InvalidForCompilerOptions,
                                        "/win32res:<filename>", engine.ErrorCultureInfo);
                                break;
                        }
                    }

                    throw new CmdLineException(CmdLineError.UnknownOption, context1, engine.ErrorCultureInfo);
                }

                break;
            }
        }

        if (stringBuilder != null)
        {
            if (generateExe)
            {
                if (flag1)
                    stringBuilder.Append(str2 + "t:winexe ");
                else
                    stringBuilder.Append(str2 + "t:exe ");
            }
            else
                stringBuilder.Append(str2 + "t:library ");

            this.debugCommandLine = stringBuilder.ToString();
        }

        engine.SetOption("libpath", (object)str1);
        engine.SetOption("defines", (object)defines);
    }

    internal void PrintBanner(VsaEngine engine, TextWriter output)
    {
        string str1 =
            $"{14.ToString((IFormatProvider)CultureInfo.InvariantCulture)}.{0.ToString((IFormatProvider)CultureInfo.InvariantCulture).PadLeft(2, '0')}.{9222.ToString((IFormatProvider)CultureInfo.InvariantCulture).PadLeft(4, '0')}";
        Version version = Environment.Version;
        string str2 =
            $"{version.Major.ToString((IFormatProvider)CultureInfo.InvariantCulture)}.{version.Minor.ToString((IFormatProvider)CultureInfo.InvariantCulture)}.{version.Build.ToString((IFormatProvider)CultureInfo.InvariantCulture).PadLeft(4, '0')}";
        output.WriteLine(string.Format((IFormatProvider)engine.ErrorCultureInfo,
            JScriptException.Localize("Banner line 1", engine.ErrorCultureInfo), new object[1]
            {
                (object)str1
            }));
        output.WriteLine(string.Format((IFormatProvider)engine.ErrorCultureInfo,
            JScriptException.Localize("Banner line 2", engine.ErrorCultureInfo), new object[1]
            {
                (object)str2
            }));
        output.WriteLine(JScriptException.Localize("Banner line 3", engine.ErrorCultureInfo) + Environment.NewLine);
    }

    private void PrintOptions(TextWriter output, CompilerParameters options)
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("CompilerParameters.CompilerOptions        : \"");
        stringBuilder.Append(options.CompilerOptions);
        stringBuilder.Append("\"");
        stringBuilder.Append(Environment.NewLine);
        stringBuilder.Append("CompilerParameters.GenerateExecutable     : ");
        stringBuilder.Append(options.GenerateExecutable.ToString((IFormatProvider)CultureInfo.InvariantCulture));
        stringBuilder.Append(Environment.NewLine);
        stringBuilder.Append("CompilerParameters.GenerateInMemory       : ");
        stringBuilder.Append(options.GenerateInMemory.ToString((IFormatProvider)CultureInfo.InvariantCulture));
        stringBuilder.Append(Environment.NewLine);
        stringBuilder.Append("CompilerParameters.IncludeDebugInformation: ");
        stringBuilder.Append(options.IncludeDebugInformation.ToString((IFormatProvider)CultureInfo.InvariantCulture));
        stringBuilder.Append(Environment.NewLine);
        stringBuilder.Append("CompilerParameters.MainClass              : \"");
        stringBuilder.Append(options.MainClass);
        stringBuilder.Append("\"");
        stringBuilder.Append(Environment.NewLine);
        stringBuilder.Append("CompilerParameters.OutputAssembly         : \"");
        stringBuilder.Append(options.OutputAssembly);
        stringBuilder.Append("\"");
        stringBuilder.Append(Environment.NewLine);
        stringBuilder.Append("CompilerParameters.ReferencedAssemblies   : ");
        foreach (string referencedAssembly in options.ReferencedAssemblies)
        {
            stringBuilder.Append(Environment.NewLine);
            stringBuilder.Append("        \"");
            stringBuilder.Append(referencedAssembly);
            stringBuilder.Append("\"");
        }

        stringBuilder.Append(Environment.NewLine);
        stringBuilder.Append("CompilerParameters.TreatWarningsAsErrors  : ");
        stringBuilder.Append(options.TreatWarningsAsErrors.ToString((IFormatProvider)CultureInfo.InvariantCulture));
        stringBuilder.Append(Environment.NewLine);
        stringBuilder.Append("CompilerParameters.WarningLevel           : ");
        stringBuilder.Append(options.WarningLevel.ToString((IFormatProvider)CultureInfo.InvariantCulture));
        stringBuilder.Append(Environment.NewLine);
        stringBuilder.Append("CompilerParameters.Win32Resource          : \"");
        stringBuilder.Append(options.Win32Resource);
        stringBuilder.Append("\"");
        stringBuilder.Append(Environment.NewLine);
        output.WriteLine(stringBuilder.ToString());
    }

    protected string ReadFile(string fileName, VsaEngine engine)
    {
        string str = "";
        FileStream fileStream;
        try
        {
            fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
        }
        catch (ArgumentException ex)
        {
            throw new CmdLineException(CmdLineError.InvalidCharacters, fileName, engine.ErrorCultureInfo);
        }
        catch (FileNotFoundException ex)
        {
            throw new CmdLineException(CmdLineError.SourceNotFound, fileName, engine.ErrorCultureInfo);
        }

        try
        {
            if (fileStream.Length != 0L)
            {
                StreamReader streamReader = new StreamReader((Stream)fileStream, true);
                try
                {
                    str = streamReader.ReadToEnd();
                }
                finally
                {
                    streamReader.Close();
                }
            }
        }
        finally
        {
            fileStream.Close();
        }

        return str;
    }

    private StringCollection SplitCmdLineArguments(string argumentString)
    {
        StringCollection stringCollection = new StringCollection();
        if (argumentString == null || argumentString.Length == 0)
            return stringCollection;
        MatchCollection matchCollection = new Regex("\\s*([^\\s\\\"]|(\\\"[^\\\"\\n]*\\\"))+").Matches(argumentString);
        if (matchCollection != null && matchCollection.Count != 0)
        {
            foreach (Match match in matchCollection)
            {
                string str = match.ToString().Trim();
                int startIndex = 0;
                while ((startIndex = str.IndexOf("\"", startIndex)) != -1)
                {
                    if (startIndex == 0)
                        str = str.Substring(1);
                    else if (str[startIndex - 1] == '\\')
                        ++startIndex;
                    else
                        str = str.Remove(startIndex, 1);
                }

                stringCollection.Add(str);
            }
        }

        return stringCollection;
    }

    private void ValidateOptions(CompilerParameters options, VsaEngine engine)
    {
        string outputAssembly = options.OutputAssembly;
        try
        {
            if (Path.GetFileName(outputAssembly).Length == 0)
                throw new CmdLineException(CmdLineError.NoFileName, outputAssembly, engine.ErrorCultureInfo);
        }
        catch (ArgumentException ex)
        {
            throw new CmdLineException(CmdLineError.NoFileName, engine.ErrorCultureInfo);
        }

        if (Path.GetExtension(outputAssembly).Length == 0)
            throw new CmdLineException(CmdLineError.MissingExtension, outputAssembly, engine.ErrorCultureInfo);
        if (options.WarningLevel == -1)
            options.WarningLevel = 4;
        if (options.WarningLevel < 0 || options.WarningLevel > 4)
            throw new CmdLineException(CmdLineError.InvalidWarningLevel,
                options.WarningLevel.ToString((IFormatProvider)CultureInfo.InvariantCulture), engine.ErrorCultureInfo);
        if (options.Win32Resource != null && options.Win32Resource.Length > 0 && !File.Exists(options.Win32Resource))
            throw new CmdLineException(CmdLineError.ResourceNotFound, options.Win32Resource, engine.ErrorCultureInfo);
    }
}