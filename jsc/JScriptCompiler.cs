// Decompiled with JetBrains decompiler
// Type: JScriptCompiler
// Assembly: jsc, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: EFC0C76F-35B7-44DC-BADC-474CA93DF1B0
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\jsc.exe

using Microsoft.JScript;
using Microsoft.JScript.Vsa;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;

#nullable disable
internal class JScriptCompiler : MarshalByRefObject
{
    private bool fPrintBanner;
    private bool fBannerPrinted;
    private bool fPrintTargets;
    private int exitCode;
    private bool fHelp;
    private bool fUtf8Output;
    private ArrayList targets;
    private CompilerOptions currentTarget;
    private int codeItemCounter;
    internal const string ContextStringDelimiter = ";;";
    private static int LCID = CultureInfo.CurrentUICulture.LCID;

    private static readonly ResourceManager resourceManager =
        new ResourceManager("JSC", typeof(JScriptCompiler).Module.Assembly);

    public int Run(string[] args)
    {
        this.exitCode = 0;
        try
        {
            this.fPrintBanner = true;
            this.fBannerPrinted = false;
            this.fPrintTargets = false;
            this.fHelp = false;
            this.fUtf8Output = false;
            this.targets = new ArrayList();
            this.currentTarget = (CompilerOptions)null;
            if (args.Length == 0)
            {
                this.PrintUsage();
                return 0;
            }

            this.ParseArguments(args, false);
            if (this.fUtf8Output)
                Console.SetOut(TextWriter.Synchronized(
                    (TextWriter)new StreamWriter(Console.OpenStandardOutput(), (Encoding)new UTF8Encoding(false))
                    {
                        AutoFlush = true
                    }));
            if (this.fHelp)
            {
                this.PrintUsage();
                return 0;
            }

            foreach (CompilerOptions target in this.targets)
            {
                if (target.SourceFileNames.Count == 0)
                    throw new CmdLineException(CmdLineError.NoInputSourcesSpecified, JScriptCompiler.GetCultureInfo());
            }

            if (this.targets.Count > 1)
                this.fPrintTargets = true;
            this.PrintBanner((CultureInfo)null);
            foreach (CompilerOptions target in this.targets)
            {
                if (!target.fForceCodepage)
                    target.codepage = JScriptCompiler.GetCultureInfo().TextInfo.ANSICodePage;
                if (target.strOutputFileName == null)
                {
                    string withoutExtension = Path.GetFileNameWithoutExtension((string)target.SourceFileNames[0]);
                    target.strOutputFileName =
                        withoutExtension + (target.PEFileKind == PEFileKinds.Dll ? ".dll" : ".exe");
                }

                if (!this.Compile(target))
                    this.exitCode = 10;
            }
        }
        catch (CmdLineException ex)
        {
            this.PrintBanner((CultureInfo)null);
            Console.WriteLine(ex.Message);
            this.exitCode = 10;
        }
        catch (Exception ex)
        {
            this.PrintBanner((CultureInfo)null);
            Console.WriteLine("fatal error JS2999: " + ex.Message);
            this.exitCode = 10;
        }

        return this.exitCode;
    }

    private void ParseArguments(string[] args, bool fromResponseFile)
    {
        int start = 0;
        while (start < args.Length)
        {
            if (this.currentTarget == null)
                this.currentTarget = new CompilerOptions();
            int options = this.ParseOptions(args, start, fromResponseFile);
            start = this.ParseSources(args, options);
            if (!fromResponseFile || start < args.Length - 1)
            {
                this.targets.Add((object)this.currentTarget);
                this.currentTarget = (CompilerOptions)null;
            }
        }
    }

    private int ParseOptions(string[] args, int start, bool fromResponseFile)
    {
        int options;
        for (options = start; options < args.Length; ++options)
        {
            string str1 = args[options];
            if (str1 != null && str1.Length != 0)
            {
                if ('@' == str1[0])
                {
                    if (fromResponseFile)
                        throw new CmdLineException(CmdLineError.NestedResponseFiles, JScriptCompiler.GetCultureInfo());
                    string str2 = str1.Substring(1);
                    if (str2.Length == 0)
                        throw new CmdLineException(CmdLineError.NoFileName, args[options],
                            JScriptCompiler.GetCultureInfo());
                    string[] args1 = File.Exists(str2)
                        ? JScriptCompiler.ReadResponseFile(str2)
                        : throw new CmdLineException(CmdLineError.SourceNotFound, str2,
                            JScriptCompiler.GetCultureInfo());
                    if (args1 != null)
                        this.ParseArguments(args1, true);
                }
                else
                {
                    if ('-' == str1[0] || '/' == str1[0] && '/' != Path.DirectorySeparatorChar)
                    {
                        string option = str1.Substring(1);
                        if (option.Length >= 1)
                        {
                            switch (option[0])
                            {
                                case '?':
                                case 'H':
                                case 'h':
                                    if (CmdLineOptionParser.IsSimpleOption(option, "?") ||
                                        CmdLineOptionParser.IsSimpleOption(option, "help"))
                                    {
                                        this.fHelp = true;
                                        continue;
                                    }

                                    break;
                                case 'A':
                                case 'a':
                                    object obj1 = CmdLineOptionParser.IsBooleanOption(option, "autoref");
                                    if (obj1 != null)
                                    {
                                        if ((bool)obj1)
                                        {
                                            this.currentTarget.autoRef = true;
                                            this.currentTarget.autoRefSetExplicitly = true;
                                            continue;
                                        }

                                        this.currentTarget.autoRef = false;
                                        continue;
                                    }

                                    object obj2 =
                                        (object)CmdLineOptionParser.IsArgumentOption(option, "a.version",
                                            "assembly.version");
                                    if (obj2 != null)
                                    {
                                        if (((string)obj2).Length == 0)
                                            throw new CmdLineException(CmdLineError.MissingVersionInfo,
                                                JScriptCompiler.GetCultureInfo());
                                        try
                                        {
                                            this.currentTarget.versionInfo = new Version((string)obj2);
                                            continue;
                                        }
                                        catch
                                        {
                                            throw new CmdLineException(CmdLineError.InvalidVersion, (string)obj2,
                                                JScriptCompiler.GetCultureInfo());
                                        }
                                    }
                                    else
                                        break;
                                case 'C':
                                case 'c':
                                    object obj3 = (object)CmdLineOptionParser.IsArgumentOption(option, "codepage");
                                    if (obj3 != null)
                                    {
                                        if (((string)obj3).Length == 0)
                                            throw new CmdLineException(CmdLineError.NoCodePage, args[options],
                                                JScriptCompiler.GetCultureInfo());
                                        try
                                        {
                                            this.currentTarget.codepage = int.Parse((string)obj3,
                                                (IFormatProvider)CultureInfo.InvariantCulture);
                                            this.currentTarget.fForceCodepage = true;
                                            continue;
                                        }
                                        catch
                                        {
                                            throw new CmdLineException(CmdLineError.InvalidCodePage, (string)obj3,
                                                JScriptCompiler.GetCultureInfo());
                                        }
                                    }
                                    else
                                        break;
                                case 'D':
                                case 'd':
                                    object obj4 = CmdLineOptionParser.IsBooleanOption(option, "debug");
                                    if (obj4 != null)
                                    {
                                        this.currentTarget.fDebug = (bool)obj4;
                                        continue;
                                    }

                                    object definitionList =
                                        (object)CmdLineOptionParser.IsArgumentOption(option, "d", "define");
                                    if (definitionList != null)
                                    {
                                        IDictionaryEnumerator enumerator = JScriptCompiler
                                            .GetAllDefines((string)definitionList).GetEnumerator();
                                        try
                                        {
                                            while (enumerator.MoveNext())
                                            {
                                                DictionaryEntry current = (DictionaryEntry)enumerator.Current;
                                                this.currentTarget.Defines[current.Key] = current.Value;
                                            }

                                            continue;
                                        }
                                        finally
                                        {
                                            if (enumerator is IDisposable disposable)
                                                disposable.Dispose();
                                        }
                                    }
                                    else
                                        break;
                                case 'F':
                                case 'f':
                                    object obj5 = CmdLineOptionParser.IsBooleanOption(option, "fast");
                                    if (obj5 != null)
                                    {
                                        this.currentTarget.fFast = (bool)obj5;
                                        continue;
                                    }

                                    break;
                                case 'L':
                                case 'l':
                                    object obj6 = (object)CmdLineOptionParser.IsArgumentOption(option, "lcid");
                                    if (obj6 != null)
                                    {
                                        if (((string)obj6).Length == 0)
                                            throw new CmdLineException(CmdLineError.NoLocaleID, args[options],
                                                JScriptCompiler.GetCultureInfo());
                                        try
                                        {
                                            int culture = int.Parse((string)obj6,
                                                (IFormatProvider)CultureInfo.InvariantCulture);
                                            CultureInfo cultureInfo = new CultureInfo(culture);
                                            JScriptCompiler.LCID = culture;
                                            continue;
                                        }
                                        catch
                                        {
                                            throw new CmdLineException(CmdLineError.InvalidLocaleID, (string)obj6,
                                                JScriptCompiler.GetCultureInfo());
                                        }
                                    }
                                    else
                                    {
                                        object obj7 = (object)CmdLineOptionParser.IsArgumentOption(option, "lib");
                                        if (obj7 != null)
                                        {
                                            string str3 = (string)obj7;
                                            if (str3.Length == 0)
                                                throw new CmdLineException(CmdLineError.MissingLibArgument,
                                                    JScriptCompiler.GetCultureInfo());
                                            this.currentTarget.libpath = str3.Replace(',', Path.PathSeparator) +
                                                                         Path.PathSeparator.ToString() +
                                                                         this.currentTarget.libpath;
                                            continue;
                                        }

                                        object resinfo =
                                            (object)CmdLineOptionParser.IsArgumentOption(option, "linkres",
                                                "linkresource");
                                        if (resinfo != null)
                                        {
                                            this.AddResourceFile(new ResInfo((string)resinfo, true));
                                            continue;
                                        }

                                        break;
                                    }
                                case 'N':
                                case 'n':
                                    if (CmdLineOptionParser.IsSimpleOption(option, "nologo"))
                                    {
                                        this.fPrintBanner = false;
                                        continue;
                                    }

                                    object obj8 = CmdLineOptionParser.IsBooleanOption(option, "nostdlib");
                                    if (obj8 != null)
                                    {
                                        this.currentTarget.fNoStdlib = (bool)obj8;
                                        if ((bool)obj8 && !this.currentTarget.autoRefSetExplicitly)
                                        {
                                            this.currentTarget.autoRef = false;
                                            continue;
                                        }

                                        continue;
                                    }

                                    break;
                                case 'O':
                                case 'o':
                                    object path1 = (object)CmdLineOptionParser.IsArgumentOption(option, "out");
                                    if (path1 != null)
                                    {
                                        try
                                        {
                                            if (Path.GetFileName((string)path1) == "")
                                                throw new CmdLineException(CmdLineError.NoFileName, args[options],
                                                    JScriptCompiler.GetCultureInfo());
                                        }
                                        catch (ArgumentException ex)
                                        {
                                            throw new CmdLineException(CmdLineError.NoFileName, args[options],
                                                JScriptCompiler.GetCultureInfo());
                                        }

                                        if (Path.GetExtension((string)path1) == "")
                                            throw new CmdLineException(CmdLineError.MissingExtension, args[options],
                                                JScriptCompiler.GetCultureInfo());
                                        this.currentTarget.strOutputFileName =
                                            this.currentTarget.strOutputFileName == null
                                                ? (string)path1
                                                : throw new CmdLineException(CmdLineError.MultipleOutputNames,
                                                    JScriptCompiler.GetCultureInfo());
                                        continue;
                                    }

                                    break;
                                case 'P':
                                case 'p':
                                    object obj9 = CmdLineOptionParser.IsBooleanOption(option, "print");
                                    if (obj9 != null)
                                    {
                                        this.currentTarget.fPrint = (bool)obj9;
                                        this.currentTarget.fPrintSetExplicitly = (bool)obj9;
                                        continue;
                                    }

                                    object context = (object)CmdLineOptionParser.IsArgumentOption(option, "platform");
                                    if (context != null)
                                    {
                                        string strA = (string)context;
                                        PortableExecutableKinds portableExecutableKinds;
                                        ImageFileMachine imageFileMachine;
                                        if (string.Compare(strA, "x86", StringComparison.OrdinalIgnoreCase) == 0)
                                        {
                                            portableExecutableKinds = PortableExecutableKinds.ILOnly |
                                                                      PortableExecutableKinds.Required32Bit;
                                            imageFileMachine = ImageFileMachine.I386;
                                        }
                                        else if (string.Compare(strA, "Itanium", StringComparison.OrdinalIgnoreCase) ==
                                                 0)
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
                                                throw new CmdLineException(CmdLineError.InvalidPlatform,
                                                    (string)context, JScriptCompiler.GetCultureInfo());
                                            portableExecutableKinds = PortableExecutableKinds.ILOnly;
                                            imageFileMachine = ImageFileMachine.I386;
                                        }

                                        this.currentTarget.PEKindFlags = portableExecutableKinds;
                                        this.currentTarget.PEMachineArchitecture = imageFileMachine;
                                        continue;
                                    }

                                    break;
                                case 'R':
                                case 'r':
                                    object fileList =
                                        (object)CmdLineOptionParser.IsArgumentOption(option, "r", "reference");
                                    if (fileList != null)
                                    {
                                        StringEnumerator stringEnumerator = ((string)fileList).Length != 0
                                            ? JScriptCompiler.GetAllReferences((string)fileList).GetEnumerator()
                                            : throw new CmdLineException(CmdLineError.NoFileName, args[options],
                                                JScriptCompiler.GetCultureInfo());
                                        try
                                        {
                                            while (stringEnumerator.MoveNext())
                                            {
                                                string current = stringEnumerator.Current;
                                                bool flag = false;
                                                foreach (string importFileName in this.currentTarget.ImportFileNames)
                                                {
                                                    if (string.Compare(importFileName, current,
                                                            StringComparison.OrdinalIgnoreCase) == 0)
                                                    {
                                                        flag = true;
                                                        break;
                                                    }
                                                }

                                                if (!flag)
                                                    this.currentTarget.ImportFileNames.Add((object)current);
                                            }

                                            continue;
                                        }
                                        finally
                                        {
                                            if (stringEnumerator is IDisposable disposable)
                                                disposable.Dispose();
                                        }
                                    }
                                    else
                                    {
                                        object resinfo =
                                            (object)CmdLineOptionParser.IsArgumentOption(option, "res", "resource");
                                        if (resinfo != null)
                                        {
                                            this.AddResourceFile(new ResInfo((string)resinfo, false));
                                            continue;
                                        }

                                        break;
                                    }
                                case 'T':
                                case 't':
                                    object obj10 = (object)CmdLineOptionParser.IsArgumentOption(option, "t", "target");
                                    if (obj10 != null)
                                    {
                                        if (this.currentTarget.fTargetSpecified)
                                            throw new CmdLineException(CmdLineError.MultipleTargets,
                                                JScriptCompiler.GetCultureInfo());
                                        if (string.Compare((string)obj10, "exe", StringComparison.OrdinalIgnoreCase) ==
                                            0)
                                        {
                                            this.currentTarget.fTargetSpecified = true;
                                            this.currentTarget.PEFileKind = PEFileKinds.ConsoleApplication;
                                            continue;
                                        }

                                        if (string.Compare((string)obj10, "winexe",
                                                StringComparison.OrdinalIgnoreCase) == 0)
                                        {
                                            this.currentTarget.fTargetSpecified = true;
                                            this.currentTarget.PEFileKind = PEFileKinds.WindowApplication;
                                            this.currentTarget.fPrint = this.currentTarget.fPrintSetExplicitly;
                                            continue;
                                        }

                                        if (string.Compare((string)obj10, "library",
                                                StringComparison.OrdinalIgnoreCase) != 0)
                                            throw new CmdLineException(CmdLineError.InvalidTarget, (string)obj10,
                                                JScriptCompiler.GetCultureInfo());
                                        this.currentTarget.fTargetSpecified = true;
                                        this.currentTarget.PEFileKind = PEFileKinds.Dll;
                                        continue;
                                    }

                                    break;
                                case 'U':
                                case 'u':
                                    object obj11 = CmdLineOptionParser.IsBooleanOption(option, "utf8output");
                                    if (obj11 != null)
                                    {
                                        this.fUtf8Output = (bool)obj11;
                                        continue;
                                    }

                                    break;
                                case 'V':
                                case 'v':
                                    object obj12 = CmdLineOptionParser.IsBooleanOption(option, "VersionSafe");
                                    if (obj12 != null)
                                    {
                                        this.currentTarget.fVersionSafe = (bool)obj12;
                                        continue;
                                    }

                                    break;
                                case 'W':
                                case 'w':
                                    object obj13 = (object)CmdLineOptionParser.IsArgumentOption(option, "w", "warn");
                                    if (obj13 != null)
                                    {
                                        if (((string)obj13).Length == 0)
                                            throw new CmdLineException(CmdLineError.NoWarningLevel, args[options],
                                                JScriptCompiler.GetCultureInfo());
                                        if (1 == ((string)obj13).Length)
                                        {
                                            switch (((string)obj13)[0])
                                            {
                                                case '0':
                                                    this.currentTarget.nWarningLevel = 0;
                                                    continue;
                                                case '1':
                                                    this.currentTarget.nWarningLevel = 1;
                                                    continue;
                                                case '2':
                                                    this.currentTarget.nWarningLevel = 2;
                                                    continue;
                                                case '3':
                                                    this.currentTarget.nWarningLevel = 3;
                                                    continue;
                                                case '4':
                                                    this.currentTarget.nWarningLevel = 4;
                                                    continue;
                                            }
                                        }

                                        throw new CmdLineException(CmdLineError.InvalidWarningLevel, args[options],
                                            JScriptCompiler.GetCultureInfo());
                                    }

                                    object obj14 = CmdLineOptionParser.IsBooleanOption(option, "warnaserror");
                                    if (obj14 != null)
                                    {
                                        this.currentTarget.fTreatWarningsAsErrors = (bool)obj14;
                                        continue;
                                    }

                                    object obj15 = (object)CmdLineOptionParser.IsArgumentOption(option, "win32res");
                                    if (obj15 != null)
                                    {
                                        string path2 = (string)obj15;
                                        if (path2.Length == 0)
                                            throw new CmdLineException(CmdLineError.NoFileName, args[options],
                                                JScriptCompiler.GetCultureInfo());
                                        if (!File.Exists(path2))
                                            throw new CmdLineException(CmdLineError.ResourceNotFound, args[options],
                                                JScriptCompiler.GetCultureInfo());
                                        this.currentTarget.Win32Resource = this.currentTarget.Win32Resource == null
                                            ? path2
                                            : throw new CmdLineException(CmdLineError.MultipleWin32Resources,
                                                JScriptCompiler.GetCultureInfo());
                                        continue;
                                    }

                                    break;
                            }
                        }

                        throw new CmdLineException(CmdLineError.UnknownOption, args[options],
                            JScriptCompiler.GetCultureInfo());
                    }

                    break;
                }
            }
        }

        return options;
    }

    private int ParseSources(string[] args, int start)
    {
        int sources;
        for (sources = start; sources < args.Length; ++sources)
        {
            if (args[sources].Length != 0)
            {
                if ('-' != args[sources][0] && ('/' != args[sources][0] || '/' == Path.DirectorySeparatorChar))
                {
                    try
                    {
                        if (Path.GetFileName(args[sources]) == "")
                            throw new CmdLineException(CmdLineError.InvalidSourceFile, args[sources],
                                JScriptCompiler.GetCultureInfo());
                    }
                    catch (ArgumentException ex)
                    {
                        throw new CmdLineException(CmdLineError.InvalidSourceFile, args[sources],
                            JScriptCompiler.GetCultureInfo());
                    }

                    foreach (string importFileName in this.currentTarget.ImportFileNames)
                    {
                        if (string.Compare(importFileName, args[sources], StringComparison.OrdinalIgnoreCase) == 0)
                            throw new CmdLineException(CmdLineError.DuplicateFileAsSourceAndAssembly, args[sources],
                                JScriptCompiler.GetCultureInfo());
                    }

                    foreach (string sourceFileName in this.currentTarget.SourceFileNames)
                    {
                        if (string.Compare(sourceFileName, args[sources], StringComparison.OrdinalIgnoreCase) == 0)
                            throw new CmdLineException(CmdLineError.DuplicateSourceFile, args[sources],
                                JScriptCompiler.GetCultureInfo());
                    }

                    this.currentTarget.SourceFileNames.Add((object)args[sources]);
                }
                else
                    break;
            }
        }

        return sources;
    }

    private bool Compile(CompilerOptions options)
    {
        if (this.fPrintTargets)
            Console.WriteLine(JScriptCompiler.Localize("Compiling", options.strOutputFileName,
                JScriptCompiler.GetCultureInfo()));
        VsaEngine engine = new VsaEngine();
        if (engine == null)
            throw new CmdLineException(CmdLineError.CannotCreateEngine, JScriptCompiler.GetCultureInfo());
        engine.InitVsaEngine("JSC://Microsoft.JScript.Vsa.VsaEngine", (IJSVsaSite)new EngineSite(options));
        engine.LCID = JScriptCompiler.GetCultureInfo().LCID;
        engine.GenerateDebugInfo = options.fDebug;
        engine.SetOption("ReferenceLoaderAPI", (object)"ReflectionOnlyLoadFrom");
        engine.SetOption("AutoRef", (object)options.autoRef);
        engine.SetOption("fast", (object)options.fFast);
        engine.SetOption("output", (object)options.strOutputFileName);
        engine.SetOption("PEFileKind", (object)options.PEFileKind);
        engine.SetOption("PortableExecutableKind", (object)options.PEKindFlags);
        engine.SetOption("ImageFileMachine", (object)options.PEMachineArchitecture);
        engine.SetOption("print", (object)options.fPrint);
        engine.SetOption("libpath", (object)options.libpath);
        if (options.versionInfo != (Version)null)
            engine.SetOption("version", (object)options.versionInfo);
        engine.SetOption("VersionSafe", (object)options.fVersionSafe);
        engine.SetOption("defines", (object)options.Defines);
        engine.SetOption("warnaserror", (object)options.fTreatWarningsAsErrors);
        engine.SetOption("WarningLevel", (object)options.nWarningLevel);
        if (options.Win32Resource != null)
            engine.SetOption("win32resource", (object)options.Win32Resource);
        if (options.ManagedResources.Count > 0)
            engine.SetOption("managedResources", (object)options.ManagedResources.Values);
        bool flag1 = false;
        bool flag2 = false;
        foreach (string importFileName in options.ImportFileNames)
        {
            JScriptCompiler.AddAssemblyReference((IJSVsaEngine)engine, importFileName);
            string fileName = Path.GetFileName(importFileName);
            if (string.Compare(fileName, "mscorlib.dll", StringComparison.OrdinalIgnoreCase) == 0)
                flag1 = true;
            else if (string.Compare(fileName, "System.Windows.Forms.dll", StringComparison.OrdinalIgnoreCase) == 0)
                flag2 = true;
        }

        if (!options.fNoStdlib && !flag1)
            JScriptCompiler.AddAssemblyReference((IJSVsaEngine)engine, "mscorlib.dll");
        if (options.PEFileKind == PEFileKinds.WindowApplication && !options.fNoStdlib && !flag2)
            JScriptCompiler.AddAssemblyReference((IJSVsaEngine)engine, "System.Windows.Forms.dll");
        for (int index = 0; index < options.SourceFileNames.Count; ++index)
            this.AddSourceFile((IJSVsaEngine)engine, (string)options.SourceFileNames[index], options.codepage);
        try
        {
            return engine.Compile();
        }
        catch (JSVsaException ex)
        {
            if (ex.ErrorCode == JSVsaError.AssemblyExpected)
            {
                if (ex.InnerException != null && ex.InnerException is BadImageFormatException)
                    Console.WriteLine(new CmdLineException(CmdLineError.InvalidAssembly, ex.Message,
                        JScriptCompiler.GetCultureInfo()).Message);
                else if (ex.InnerException != null &&
                         (ex.InnerException is FileNotFoundException || ex.InnerException is FileLoadException))
                    Console.WriteLine(new CmdLineException(CmdLineError.AssemblyNotFound, ex.Message,
                        JScriptCompiler.GetCultureInfo()).Message);
                else
                    Console.WriteLine(new CmdLineException(CmdLineError.InvalidAssembly,
                        JScriptCompiler.GetCultureInfo()).Message);
            }
            else if (ex.ErrorCode == JSVsaError.SaveCompiledStateFailed)
                Console.WriteLine(new CmdLineException(CmdLineError.ErrorSavingCompiledState, ex.Message,
                    JScriptCompiler.GetCultureInfo()).Message);
            else if (ex.ErrorCode == JSVsaError.AssemblyNameInvalid && ex.InnerException != null)
            {
                Console.WriteLine(new CmdLineException(CmdLineError.InvalidCharacters, ex.Message,
                    JScriptCompiler.GetCultureInfo()).Message);
            }
            else
            {
                Console.WriteLine(JScriptCompiler.Localize("INTERNAL COMPILER ERROR",
                    JScriptCompiler.GetCultureInfo()));
                Console.WriteLine((object)ex);
            }

            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine(JScriptCompiler.Localize("INTERNAL COMPILER ERROR", JScriptCompiler.GetCultureInfo()));
            Console.WriteLine((object)ex);
            return false;
        }
    }

    private static void AddAssemblyReference(IJSVsaEngine engine, string fileName)
    {
        ((IJSVsaReferenceItem)engine.Items.CreateItem(fileName, JSVsaItemType.Reference, JSVsaItemFlag.None))
            .AssemblyName = fileName;
    }

    private void AddSourceFile(IJSVsaEngine engine, string fileName, int codepage)
    {
        string name = "$SourceFile_" + this.codeItemCounter++.ToString();
        IJSVsaCodeItem jsVsaCodeItem =
            (IJSVsaCodeItem)engine.Items.CreateItem(name, JSVsaItemType.Code, JSVsaItemFlag.None);
        jsVsaCodeItem.SetOption("codebase", (object)fileName);
        jsVsaCodeItem.SourceText = JScriptCompiler.ReadFile(fileName, codepage);
    }

    private void AddResourceFile(ResInfo resinfo)
    {
        if (!File.Exists(resinfo.fullpath))
            throw new CmdLineException(CmdLineError.ManagedResourceNotFound, resinfo.filename,
                JScriptCompiler.GetCultureInfo());
        if (this.currentTarget.ManagedResourceFileNames[(object)resinfo.fullpath] != null)
            throw new CmdLineException(CmdLineError.DuplicateResourceFile, resinfo.filename,
                JScriptCompiler.GetCultureInfo());
        if (this.currentTarget.ManagedResources[(object)resinfo.name] != null)
            throw new CmdLineException(CmdLineError.DuplicateResourceName, resinfo.name,
                JScriptCompiler.GetCultureInfo());
        this.currentTarget.ManagedResources[(object)resinfo.name] = (object)resinfo;
        this.currentTarget.ManagedResourceFileNames[(object)resinfo.fullpath] = (object)resinfo;
    }

    internal void PrintBanner(CultureInfo culture)
    {
        if (this.fBannerPrinted || !this.fPrintBanner)
            return;
        string[] strArray1 = new string[5];
        int num1 = 14;
        strArray1[0] = num1.ToString();
        strArray1[1] = ".";
        num1 = 0;
        strArray1[2] = num1.ToString((IFormatProvider)CultureInfo.InvariantCulture).PadLeft(2, '0');
        strArray1[3] = ".";
        num1 = 9222;
        strArray1[4] = num1.ToString((IFormatProvider)CultureInfo.InvariantCulture).PadLeft(4, '0');
        string str1 = string.Concat(strArray1);
        Version version = Environment.Version;
        string[] strArray2 = new string[5];
        int num2 = version.Major;
        strArray2[0] = num2.ToString();
        strArray2[1] = ".";
        num2 = version.Minor;
        strArray2[2] = num2.ToString((IFormatProvider)CultureInfo.InvariantCulture);
        strArray2[3] = ".";
        num2 = version.Build;
        strArray2[4] = num2.ToString((IFormatProvider)CultureInfo.InvariantCulture).PadLeft(4, '0');
        string str2 = string.Concat(strArray2);
        this.fBannerPrinted = true;
        Console.WriteLine(string.Format((IFormatProvider)culture, JScriptCompiler.Localize("Banner line 1", culture),
            new object[1]
            {
                (object)str1
            }));
        Console.WriteLine(string.Format((IFormatProvider)culture, JScriptCompiler.Localize("Banner line 2", culture),
            new object[1]
            {
                (object)str2
            }));
        Console.WriteLine(JScriptCompiler.Localize("Banner line 3", culture) + "\r\n");
    }

    internal void PrintUsage()
    {
        string str = Path.DirectorySeparatorChar == '/' ? "-" : "/";
        CultureInfo cultureInfo = JScriptCompiler.GetCultureInfo();
        this.PrintBanner(cultureInfo);
        Console.WriteLine(JScriptCompiler.Localize("Usage format", cultureInfo));
        Console.WriteLine();
        Console.Write("                         ");
        Console.WriteLine(JScriptCompiler.Localize("Usage options", cultureInfo));
        Console.WriteLine("");
        Console.Write("                           ");
        Console.WriteLine(JScriptCompiler.Localize("Output Files", cultureInfo));
        Console.Write($"  {str}out:<file>              ");
        Console.WriteLine(JScriptCompiler.Localize("Usage /out", cultureInfo));
        Console.Write($"  {str}t[arget]:exe            ");
        Console.WriteLine(JScriptCompiler.Localize("Usage /t:exe", cultureInfo));
        Console.Write($"  {str}t[arget]:winexe         ");
        Console.WriteLine(JScriptCompiler.Localize("Usage /t:winexe", cultureInfo));
        Console.Write($"  {str}t[arget]:library        ");
        Console.WriteLine(JScriptCompiler.Localize("Usage /t:library", cultureInfo));
        Console.Write($"  {str}platform:<platform>     ");
        Console.WriteLine(JScriptCompiler.Localize("Usage /platform", cultureInfo));
        Console.WriteLine();
        Console.Write("                           ");
        Console.WriteLine(JScriptCompiler.Localize("Input Files", cultureInfo));
        Console.Write($"  {str}autoref[+|-]            ");
        Console.WriteLine(JScriptCompiler.Localize("Usage /autoref", cultureInfo));
        Console.Write($"  {str}lib:<path>              ");
        Console.WriteLine(JScriptCompiler.Localize("Usage /lib", cultureInfo));
        Console.Write($"  {str}r[eference]:<file list> ");
        Console.WriteLine(JScriptCompiler.Localize("Usage /reference", cultureInfo));
        Console.Write("                           ");
        Console.WriteLine("<file list>: <assembly name>[;<assembly name>...]");
        Console.WriteLine();
        Console.Write("                           ");
        Console.WriteLine(JScriptCompiler.Localize("Resources", cultureInfo));
        Console.Write($"  {str}win32res:<file>         ");
        Console.WriteLine(JScriptCompiler.Localize("Usage /win32res", cultureInfo));
        Console.Write($"  {str}res[ource]:<info>       ");
        Console.WriteLine(JScriptCompiler.Localize("Usage /resource", cultureInfo));
        Console.WriteLine("                           <info>: <filename>[,<name>[,public|private]]");
        Console.Write($"  {str}linkres[ource]:<info>   ");
        Console.WriteLine(JScriptCompiler.Localize("Usage /linkresource", cultureInfo));
        Console.WriteLine("                           <info>: <filename>[,<name>[,public|private]]");
        Console.WriteLine();
        Console.Write("                           ");
        Console.WriteLine(JScriptCompiler.Localize("Code Generation", cultureInfo));
        Console.Write($"  {str}debug[+|-]              ");
        Console.WriteLine(JScriptCompiler.Localize("Usage /debug", cultureInfo));
        Console.Write($"  {str}fast[+|-]               ");
        Console.WriteLine(JScriptCompiler.Localize("Usage /fast", cultureInfo));
        Console.Write($"  {str}warnaserror[+|-]        ");
        Console.WriteLine(JScriptCompiler.Localize("Usage /warnaserror", cultureInfo));
        Console.Write($"  {str}w[arn]:<level>          ");
        Console.WriteLine(JScriptCompiler.Localize("Usage /warn", cultureInfo));
        Console.WriteLine();
        Console.Write("                           ");
        Console.WriteLine(JScriptCompiler.Localize("Miscellaneous", cultureInfo));
        Console.Write("  @<filename>              ");
        Console.WriteLine(JScriptCompiler.Localize("Usage @file", cultureInfo));
        Console.Write($"  {str}?                       ");
        Console.WriteLine(JScriptCompiler.Localize("Usage /?", cultureInfo));
        Console.Write($"  {str}help                    ");
        Console.WriteLine(JScriptCompiler.Localize("Usage /?", cultureInfo));
        Console.Write($"  {str}d[efine]:<symbols>      ");
        Console.WriteLine(JScriptCompiler.Localize("Usage /define", cultureInfo));
        Console.Write($"  {str}nologo                  ");
        Console.WriteLine(JScriptCompiler.Localize("Usage /nologo", cultureInfo));
        Console.Write($"  {str}print[+|-]              ");
        Console.WriteLine(JScriptCompiler.Localize("Usage /print", cultureInfo));
        Console.WriteLine();
        Console.Write("                           ");
        Console.WriteLine(JScriptCompiler.Localize("Advanced", cultureInfo));
        Console.Write($"  {str}codepage:<id>           ");
        Console.WriteLine(JScriptCompiler.Localize("Usage /codepage", cultureInfo));
        Console.Write($"  {str}lcid:<id>               ");
        Console.WriteLine(JScriptCompiler.Localize("Usage /lcid", cultureInfo));
        Console.Write($"  {str}nostdlib[+|-]           ");
        Console.WriteLine(JScriptCompiler.Localize("Usage /nostdlib", cultureInfo));
        Console.Write($"  {str}utf8output[+|-]         ");
        Console.WriteLine(JScriptCompiler.Localize("Usage /utf8output", cultureInfo));
        Console.Write($"  {str}versionsafe[+|-]        ");
        Console.WriteLine(JScriptCompiler.Localize("Usage /versionsafe", cultureInfo));
        Console.WriteLine();
    }

    internal static void PrintError(
        string sourceFile,
        int line,
        int column,
        bool fIsWarning,
        int number,
        string message)
    {
        string str = (10000 + (number & (int)ushort.MaxValue)).ToString((IFormatProvider)CultureInfo.InvariantCulture)
            .Substring(1);
        if (string.Compare(sourceFile, "no source", StringComparison.Ordinal) != 0)
            Console.Write($"{sourceFile}({line.ToString()},{column.ToString()}) : ");
        Console.WriteLine($"{(fIsWarning ? "warning JS" : "error JS")}{str}: {message}");
    }

    private static int GetArgumentSeparatorIndex(string argList, int startIndex)
    {
        int num1 = argList.IndexOf(",", startIndex, StringComparison.Ordinal);
        int num2 = argList.IndexOf(";", startIndex, StringComparison.Ordinal);
        return num1 == -1 || num2 != -1 && num1 >= num2 ? num2 : num1;
    }

    private static Hashtable GetAllDefines(string definitionList)
    {
        Hashtable definitions = new Hashtable();
        int startIndex = 0;
        int argumentSeparatorIndex;
        do
        {
            argumentSeparatorIndex = JScriptCompiler.GetArgumentSeparatorIndex(definitionList, startIndex);
            JScriptCompiler.AddDefinition(
                argumentSeparatorIndex != -1
                    ? definitionList.Substring(startIndex, argumentSeparatorIndex - startIndex)
                    : definitionList.Substring(startIndex), definitions);
            startIndex = argumentSeparatorIndex + 1;
        } while (argumentSeparatorIndex > -1);

        return definitions;
    }

    private static StringCollection GetAllReferences(string fileList)
    {
        StringCollection allReferences = new StringCollection();
        int startIndex = 0;
        int argumentSeparatorIndex;
        do
        {
            argumentSeparatorIndex = JScriptCompiler.GetArgumentSeparatorIndex(fileList, startIndex);
            string str = argumentSeparatorIndex != -1
                ? fileList.Substring(startIndex, argumentSeparatorIndex - startIndex).Trim()
                : fileList.Substring(startIndex).Trim();
            if (str.Length == 0)
                throw new CmdLineException(CmdLineError.MissingReference, fileList, JScriptCompiler.GetCultureInfo());
            allReferences.Add(str);
            startIndex = argumentSeparatorIndex + 1;
        } while (argumentSeparatorIndex > -1);

        return allReferences;
    }

    private static void AddDefinition(string def, Hashtable definitions)
    {
        int length = def.IndexOf("=", StringComparison.Ordinal);
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
                    throw new CmdLineException(CmdLineError.InvalidDefinition, str1, JScriptCompiler.GetCultureInfo());
                }
            }
        }

        if (str1.Length == 0)
            throw new CmdLineException(CmdLineError.MissingDefineArgument, JScriptCompiler.GetCultureInfo());
        if (str1[0] == '_' && (string.Compare(str1, "_debug", StringComparison.Ordinal) == 0 ||
                               string.Compare(str1, "_fast", StringComparison.Ordinal) == 0 ||
                               string.Compare(str1, "_jscript", StringComparison.Ordinal) == 0 ||
                               string.Compare(str1, "_jscript_build", StringComparison.Ordinal) == 0 ||
                               string.Compare(str1, "_jscript_version", StringComparison.Ordinal) == 0 ||
                               string.Compare(str1, "_microsoft", StringComparison.Ordinal) == 0 ||
                               string.Compare(str1, "_win32", StringComparison.Ordinal) == 0 ||
                               string.Compare(str1, "_x86", StringComparison.Ordinal) == 0))
            throw new CmdLineException(CmdLineError.CompilerConstant, str1, JScriptCompiler.GetCultureInfo());
        definitions[(object)str1] = obj;
    }

    internal static string ReadFile(string fileName, int codepage)
    {
        FileStream fileStream;
        try
        {
            fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
        }
        catch (ArgumentException ex)
        {
            throw new CmdLineException(CmdLineError.InvalidCharacters, fileName, JScriptCompiler.GetCultureInfo());
        }
        catch (DirectoryNotFoundException ex)
        {
            throw new CmdLineException(CmdLineError.SourceNotFound, fileName, JScriptCompiler.GetCultureInfo());
        }
        catch (FileNotFoundException ex)
        {
            throw new CmdLineException(CmdLineError.SourceNotFound, fileName, JScriptCompiler.GetCultureInfo());
        }

        try
        {
            long length = fileStream.Length;
            if (length == 0L)
                return "";
            Encoding encoding;
            try
            {
                encoding = Encoding.GetEncoding(codepage);
            }
            catch (ArgumentException ex)
            {
                throw new CmdLineException(CmdLineError.InvalidCodePage,
                    codepage.ToString((IFormatProvider)CultureInfo.InvariantCulture), JScriptCompiler.GetCultureInfo());
            }

            if (length > (long)int.MaxValue)
                throw new CmdLineException(CmdLineError.SourceFileTooBig, JScriptCompiler.GetCultureInfo());
            using (StreamReader streamReader = new StreamReader((Stream)fileStream, encoding, true))
                return streamReader.ReadToEnd();
        }
        finally
        {
            fileStream.Close();
        }
    }

    private static string[] ReadResponseFile(string strFileName)
    {
        FileStream fileStream = new FileStream(strFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
        if (fileStream.Length == 0L)
            return (string[])null;
        StreamReader streamReader = new StreamReader((Stream)fileStream);
        string input = streamReader.ReadLine();
        Regex regex = new Regex("\\s*([^\\s\\\"]|(\\\"[^\\\"\\n]*\\\"))+");
        StringCollection stringCollection = new StringCollection();
        for (; input != null; input = streamReader.ReadLine())
        {
            if (!input.Trim().StartsWith("#", StringComparison.Ordinal))
            {
                MatchCollection matchCollection = regex.Matches(input);
                if (matchCollection != null && matchCollection.Count != 0)
                {
                    foreach (Match match in matchCollection)
                    {
                        string str = match.ToString().Trim();
                        int startIndex = 0;
                        while ((startIndex = str.IndexOf("\"", startIndex, StringComparison.Ordinal)) != -1)
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
            }
        }

        if (stringCollection.Count == 0)
            return (string[])null;
        string[] array = new string[stringCollection.Count];
        stringCollection.CopyTo(array, 0);
        return array;
    }

    internal static string Localize(string s)
    {
        return JScriptCompiler.Localize(s, (string)null, (CultureInfo)null);
    }

    internal static string Localize(string s, string context)
    {
        return JScriptCompiler.Localize(s, context, (CultureInfo)null);
    }

    internal static string Localize(string s, CultureInfo culture)
    {
        return JScriptCompiler.Localize(s, (string)null, culture);
    }

    internal static string Localize(string s, string context, CultureInfo culture)
    {
        try
        {
            if (culture == null)
                culture = JScriptCompiler.GetCultureInfo();
            string str = JScriptCompiler.resourceManager.GetString(s, culture);
            if (str != null)
            {
                int length = str.IndexOf(";;", StringComparison.CurrentCulture);
                if (length == -1)
                    return str;
                if (context == null)
                    return str.Substring(0, length);
                return string.Format((IFormatProvider)culture, str.Substring(length + 2), new object[1]
                {
                    (object)context
                });
            }
        }
        catch (MissingManifestResourceException ex)
        {
        }

        return s;
    }

    internal static CultureInfo GetCultureInfo()
    {
        CultureInfo cultureInfo = (CultureInfo)null;
        try
        {
            cultureInfo = new CultureInfo(JScriptCompiler.LCID);
        }
        catch (ArgumentException ex)
        {
        }

        if (cultureInfo == null)
            cultureInfo = CultureInfo.CurrentUICulture;
        int codePage = Console.Out.Encoding.CodePage;
        if (codePage != cultureInfo.TextInfo.OEMCodePage && codePage != cultureInfo.TextInfo.ANSICodePage)
            cultureInfo = new CultureInfo("en-US");
        return cultureInfo;
    }
}