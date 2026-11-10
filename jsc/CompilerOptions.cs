// Decompiled with JetBrains decompiler
// Type: CompilerOptions
// Assembly: jsc, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: EFC0C76F-35B7-44DC-BADC-474CA93DF1B0
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\jsc.exe

using System;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
internal class CompilerOptions
{
    public bool autoRef;
    public bool fDebug;
    public bool fFast;
    public bool fNoStdlib;
    public PEFileKinds PEFileKind;
    public PortableExecutableKinds PEKindFlags;
    public ImageFileMachine PEMachineArchitecture;
    public bool fTreatWarningsAsErrors;
    public bool fVersionSafe;
    public bool fPrint;
    public int nWarningLevel;
    public string strOutputFileName;
    public ArrayList SourceFileNames;
    public ArrayList ImportFileNames;
    public Hashtable ManagedResourceFileNames;
    public Hashtable ManagedResources;
    public string Win32Resource;
    public int codepage;
    public bool fForceCodepage;
    public Version versionInfo;
    public string libpath;
    public Hashtable Defines;
    public bool autoRefSetExplicitly;
    public bool fTargetSpecified;
    public bool fPrintSetExplicitly;

    public CompilerOptions()
    {
        this.autoRef = true;
        this.PEFileKind = PEFileKinds.ConsoleApplication;
        this.PEKindFlags = PortableExecutableKinds.ILOnly;
        this.PEMachineArchitecture = ImageFileMachine.I386;
        this.fFast = true;
        this.fPrint = true;
        this.nWarningLevel = 4;
        this.SourceFileNames = new ArrayList();
        this.ImportFileNames = new ArrayList();
        this.ManagedResourceFileNames = new Hashtable(10);
        this.ManagedResources = new Hashtable(10);
        this.Defines = new Hashtable();
        string environmentVariable = Environment.GetEnvironmentVariable("LIB");
        if (environmentVariable != null)
            this.libpath = environmentVariable;
        else
            this.libpath = "";
    }
}