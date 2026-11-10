// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.Vsa.VsaEngine
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Xml;

#nullable disable
namespace Microsoft.JScript.Vsa;

/// <summary>Implements <see cref="T:Microsoft.Vsa.IVsaEngine" /> interface. </summary>
[Guid("B71E484D-93ED-4b56-BFB9-CEED5134822B")]
[ComVisible(true)]
[Obsolete("Use of this type is not recommended because it is being deprecated in Visual Studio 2005; there will be no replacement for this feature. Please see the ICodeCompiler documentation for additional help.")]
public sealed class VsaEngine : BaseVsaEngine, IEngine2, IRedirectOutput
{
  internal bool alwaysGenerateIL;
  private bool autoRef;
  private Hashtable Defines;
  internal bool doCRS;
  internal bool doFast;
  internal bool doPrint;
  internal bool doSaveAfterCompile;
  private bool doWarnAsError;
  private int nWarningLevel;
  internal bool genStartupClass;
  internal bool isCLSCompliant;
  internal bool versionSafe;
  private string PEFileName;
  private Assembly persistedAssembly;
  internal PEFileKinds PEFileKind;
  internal PortableExecutableKinds PEKindFlags;
  internal ImageFileMachine PEMachineArchitecture;
  internal LoaderAPI ReferenceLoaderAPI;
  private System.Version versionInfo;
  private CultureInfo errorCultureInfo;
  internal static bool executeForJSEE = false;
  private string libpath;
  private string[] libpathList;
  private bool isCompilerSet;
  internal VsaScriptScope globalScope;
  private ArrayList packages;
  private ArrayList scopes;
  private ArrayList implicitAssemblies;
  private SimpleHashtable implicitAssemblyCache;
  private string win32resource;
  private ICollection managedResources;
  private string debugDirectory;
  private string tempDirectory;
  private RNGCryptoServiceProvider randomNumberGenerator;
  private byte[] rawPE;
  private byte[] rawPDB;
  internal int classCounter;
  private SimpleHashtable cachedTypeLookups;
  internal Thread runningThread;
  private CompilerGlobals compilerGlobals;
  private Globals globals;
  private int numberOfErrors;
  private string runtimeDirectory;
  private static readonly System.Version CurrentProjectVersion = new System.Version("1.0");
  private Hashtable typenameTable;
  private static string engineVersion = VsaEngine.GetVersionString();
  private Assembly runtimeAssembly;
  private static Hashtable assemblyReferencesTable = (Hashtable) null;
  private static Module reflectionOnlyVsaModule = (Module) null;
  private static Module reflectionOnlyJScriptModule = (Module) null;
  private static TypeReferences _reflectionOnlyTypeRefs;
  private static volatile VsaEngine exeEngine;

  private static string GetVersionString()
  {
    return $"{14.ToString()}.{0.ToString((IFormatProvider) CultureInfo.InvariantCulture).PadLeft(2, '0')}.{0.ToString((IFormatProvider) CultureInfo.InvariantCulture)}.{9222.ToString((IFormatProvider) CultureInfo.InvariantCulture).PadLeft(4, '0')}";
  }

  /// <summary>This class, member, or property is internal to the script engine and should not be called from your code.</summary>
  public VsaEngine()
    : this(true)
  {
  }

  /// <summary>This class, member, or property is internal to the script engine and should not be called from your code.</summary>
  public VsaEngine(bool fast)
    : base("JScript", VsaEngine.engineVersion, true)
  {
    this.alwaysGenerateIL = false;
    this.autoRef = false;
    this.doCRS = false;
    this.doFast = fast;
    this.genDebugInfo = false;
    this.genStartupClass = true;
    this.doPrint = false;
    this.doWarnAsError = false;
    this.nWarningLevel = 4;
    this.isCLSCompliant = false;
    this.versionSafe = false;
    this.PEFileName = (string) null;
    this.PEFileKind = PEFileKinds.Dll;
    this.PEKindFlags = PortableExecutableKinds.ILOnly;
    this.PEMachineArchitecture = ImageFileMachine.I386;
    this.ReferenceLoaderAPI = LoaderAPI.LoadFrom;
    this.errorCultureInfo = (CultureInfo) null;
    this.libpath = (string) null;
    this.libpathList = (string[]) null;
    this.globalScope = (VsaScriptScope) null;
    this.vsaItems = (IJSVsaItems) new VsaItems(this);
    this.packages = (ArrayList) null;
    this.scopes = (ArrayList) null;
    this.classCounter = 0;
    this.implicitAssemblies = (ArrayList) null;
    this.implicitAssemblyCache = (SimpleHashtable) null;
    this.cachedTypeLookups = (SimpleHashtable) null;
    this.isEngineRunning = false;
    this.isEngineCompiled = false;
    this.isCompilerSet = false;
    this.isClosed = false;
    this.runningThread = (Thread) null;
    this.compilerGlobals = (CompilerGlobals) null;
    this.globals = (Globals) null;
    this.runtimeDirectory = (string) null;
    Globals.contextEngine = this;
    this.runtimeAssembly = (Assembly) null;
    this.typenameTable = (Hashtable) null;
  }

  private VsaEngine(Assembly runtimeAssembly)
    : this(true)
  {
    this.runtimeAssembly = runtimeAssembly;
  }

  internal void EnsureReflectionOnlyModulesLoaded()
  {
    if (!(VsaEngine.reflectionOnlyVsaModule == (Module) null))
      return;
    // Reflection-only loading is not supported on .NET Core.  Use the modules of
    // the loaded assemblies; types are resolved normally.
    VsaEngine.reflectionOnlyVsaModule = typeof (IJSVsaEngine).Module;
    VsaEngine.reflectionOnlyJScriptModule = typeof (VsaEngine).Module;
  }

  internal Module VsaModule
  {
    get
    {
      if (this.ReferenceLoaderAPI != LoaderAPI.ReflectionOnlyLoadFrom)
        return typeof (IJSVsaEngine).Module;
      this.EnsureReflectionOnlyModulesLoaded();
      return VsaEngine.reflectionOnlyVsaModule;
    }
  }

  internal Module JScriptModule
  {
    get
    {
      if (this.ReferenceLoaderAPI != LoaderAPI.ReflectionOnlyLoadFrom)
        return typeof (VsaEngine).Module;
      this.EnsureReflectionOnlyModulesLoaded();
      return VsaEngine.reflectionOnlyJScriptModule;
    }
  }

  private void AddChildAndValue(XmlDocument doc, XmlElement parent, string name, string value)
  {
    XmlElement element = doc.CreateElement(name);
    this.CreateAttribute(doc, element, "Value", value);
    parent.AppendChild((XmlNode) element);
  }

  internal void AddPackage(PackageScope pscope)
  {
    if (this.packages == null)
      this.packages = new ArrayList(8);
    foreach (PackageScope package in this.packages)
    {
      if (package.name.Equals(pscope.name))
      {
        package.owner.MergeWith(pscope.owner);
        return;
      }
    }
    this.packages.Add((object) pscope);
  }

  internal void CheckForErrors()
  {
    if (!this.isClosed && !this.isEngineCompiled)
    {
      this.SetUpCompilerEnvironment();
      this.Globals.ScopeStack.Push(this.GetGlobalScope().GetObject());
      try
      {
        foreach (object vsaItem in (IEnumerable) this.vsaItems)
        {
          if (vsaItem is VsaReference)
            ((VsaItem) vsaItem).Compile();
        }
        if (this.vsaItems.Count > 0)
          this.SetEnclosingContext((ScriptObject) new WrappedNamespace("", this));
        foreach (object vsaItem in (IEnumerable) this.vsaItems)
        {
          if (!(vsaItem is VsaReference))
            ((VsaItem) vsaItem).CheckForErrors();
        }
        if (this.globalScope != null)
          this.globalScope.CheckForErrors();
      }
      finally
      {
        this.Globals.ScopeStack.Pop();
      }
    }
    this.globalScope = (VsaScriptScope) null;
  }

  /// <summary>This class, member, or property is internal to the script engine and should not be called from your code.</summary>
  [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
  public IJSVsaEngine Clone(System.AppDomain domain) => throw new NotImplementedException();

  /// <summary>This class, member, or property is internal to the script engine and should not be called from your code.</summary>
  [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
  public bool CompileEmpty()
  {
    this.TryObtainLock();
    try
    {
      return this.DoCompile();
    }
    finally
    {
      this.ReleaseLock();
    }
  }

  private void CreateAttribute(XmlDocument doc, XmlElement elem, string name, string value)
  {
    XmlAttribute attribute = doc.CreateAttribute(name);
    elem.SetAttributeNode(attribute);
    elem.SetAttribute(name, value);
  }

  /// <summary>This class, member, or property is internal to the script engine and should not be called from your code.</summary>
  [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
  public void ConnectEvents()
  {
  }

  internal CompilerGlobals CompilerGlobals
  {
    get
    {
      if (this.compilerGlobals == null)
        this.compilerGlobals = new CompilerGlobals(this, this.Name, this.PEFileName, this.PEFileKind, this.doSaveAfterCompile || this.genStartupClass, !this.doSaveAfterCompile || this.genStartupClass, this.genDebugInfo, this.isCLSCompliant, this.versionInfo, this.globals);
      return this.compilerGlobals;
    }
  }

  private TypeReferences TypeRefs
  {
    get
    {
      return LoaderAPI.ReflectionOnlyLoadFrom != this.ReferenceLoaderAPI ? Microsoft.JScript.Runtime.TypeRefs : VsaEngine._reflectionOnlyTypeRefs ?? (VsaEngine._reflectionOnlyTypeRefs = new TypeReferences(this.JScriptModule));
    }
  }

  /// <summary>This class, member, or property is internal to the script engine and should not be called from your code.</summary>
  public static GlobalScope CreateEngineAndGetGlobalScope(bool fast, string[] assemblyNames)
  {
    VsaEngine engine = new VsaEngine(fast);
    engine.InitVsaEngine("JScript.Vsa.VsaEngine://Microsoft.JScript.VsaEngine.Vsa", (IJSVsaSite) new DefaultVsaSite());
    engine.doPrint = true;
    engine.SetEnclosingContext((ScriptObject) new WrappedNamespace("", engine));
    foreach (string assemblyName in assemblyNames)
      ((VsaReference) engine.vsaItems.CreateItem(assemblyName, JSVsaItemType.Reference, JSVsaItemFlag.None)).AssemblyName = assemblyName;
    VsaEngine.exeEngine = engine;
    GlobalScope andGetGlobalScope = (GlobalScope) engine.GetGlobalScope().GetObject();
    andGetGlobalScope.globalObject = engine.Globals.globalObject;
    return andGetGlobalScope;
  }

  /// <summary>This class, member, or property is internal to the script engine and should not be called from your code.</summary>
  public static GlobalScope CreateEngineAndGetGlobalScopeWithType(
    bool fast,
    string[] assemblyNames,
    RuntimeTypeHandle callingTypeHandle)
  {
    return VsaEngine.CreateEngineAndGetGlobalScopeWithTypeAndRootNamespace(fast, assemblyNames, callingTypeHandle, (string) null);
  }

  /// <summary>This class, member, or property is internal to the script engine and should not be called from your code.</summary>
  /// <returns>Returns <see cref="T:Microsoft.JScript.GlobalScope" />.</returns>
  public static GlobalScope CreateEngineAndGetGlobalScopeWithTypeAndRootNamespace(
    bool fast,
    string[] assemblyNames,
    RuntimeTypeHandle callingTypeHandle,
    string rootNamespace)
  {
    VsaEngine vsaEngine = new VsaEngine(fast);
    vsaEngine.InitVsaEngine("JScript.Vsa.VsaEngine://Microsoft.JScript.VsaEngine.Vsa", (IJSVsaSite) new DefaultVsaSite());
    vsaEngine.doPrint = true;
    vsaEngine.SetEnclosingContext((ScriptObject) new WrappedNamespace("", vsaEngine));
    if (rootNamespace != null)
      vsaEngine.SetEnclosingContext((ScriptObject) new WrappedNamespace(rootNamespace, vsaEngine));
    foreach (string assemblyName in assemblyNames)
      ((VsaReference) vsaEngine.vsaItems.CreateItem(assemblyName, JSVsaItemType.Reference, JSVsaItemFlag.None)).AssemblyName = assemblyName;
    Microsoft.JScript.JScriptCallContext.SetData("JScript:" + Type.GetTypeFromHandle(callingTypeHandle).Assembly.FullName, (object) vsaEngine);
    GlobalScope andRootNamespace = (GlobalScope) vsaEngine.GetGlobalScope().GetObject();
    andRootNamespace.globalObject = vsaEngine.Globals.globalObject;
    return andRootNamespace;
  }

  /// <summary>This class, member, or property is internal to the script engine and should not be called from your code.</summary>
  public static VsaEngine CreateEngine()
  {
    if (VsaEngine.exeEngine == null)
    {
      VsaEngine vsaEngine = new VsaEngine(true);
      vsaEngine.InitVsaEngine("JScript.Vsa.VsaEngine://Microsoft.JScript.VsaEngine.Vsa", (IJSVsaSite) new DefaultVsaSite());
      VsaEngine.exeEngine = vsaEngine;
    }
    return VsaEngine.exeEngine;
  }

  internal static VsaEngine CreateEngineForDebugger()
  {
    VsaEngine engineForDebugger = new VsaEngine(true);
    engineForDebugger.InitVsaEngine("JScript.Vsa.VsaEngine://Microsoft.JScript.VsaEngine.Vsa", (IJSVsaSite) new DefaultVsaSite());
    ((GlobalScope) engineForDebugger.GetGlobalScope().GetObject()).globalObject = engineForDebugger.Globals.globalObject;
    return engineForDebugger;
  }

  /// <summary>This class, member, or property is internal to the script engine and should not be called from your code.</summary>
  public static VsaEngine CreateEngineWithType(RuntimeTypeHandle callingTypeHandle)
  {
    Assembly assembly = Type.GetTypeFromHandle(callingTypeHandle).Assembly;
    object data = Microsoft.JScript.JScriptCallContext.GetData("JScript:" + assembly.FullName);
    if (data != null && data is VsaEngine engineWithType1)
      return engineWithType1;
    VsaEngine engineWithType2 = new VsaEngine(assembly);
    engineWithType2.InitVsaEngine("JScript.Vsa.VsaEngine://Microsoft.JScript.VsaEngine.Vsa", (IJSVsaSite) new DefaultVsaSite());
    GlobalScope globalScope = (GlobalScope) engineWithType2.GetGlobalScope().GetObject();
    globalScope.globalObject = engineWithType2.Globals.globalObject;
    int num = 0;
    Type type;
    do
    {
      string name = "JScript " + num.ToString((IFormatProvider) CultureInfo.InvariantCulture);
      type = assembly.GetType(name, false);
      if (type != (Type) null)
      {
        engineWithType2.SetEnclosingContext((ScriptObject) new WrappedNamespace("", engineWithType2));
        ConstructorInfo constructor = type.GetConstructor(new Type[1]
        {
          typeof (GlobalScope)
        });
        MethodInfo method = type.GetMethod("Global Code");
        try
        {
          object obj = constructor.Invoke(new object[1]
          {
            (object) globalScope
          });
          method.Invoke(obj, new object[0]);
        }
        catch (SecurityException ex)
        {
          break;
        }
      }
      ++num;
    }
    while (type != (Type) null);
    if (data == null)
      Microsoft.JScript.JScriptCallContext.SetData("JScript:" + assembly.FullName, (object) engineWithType2);
    return engineWithType2;
  }

  private void AddReferences()
  {
    if (VsaEngine.assemblyReferencesTable == null)
      VsaEngine.assemblyReferencesTable = Hashtable.Synchronized(new Hashtable());
    if (VsaEngine.assemblyReferencesTable[(object) this.runtimeAssembly.FullName] is string[] strArray1)
    {
      for (int index = 0; index < strArray1.Length; ++index)
        ((VsaReference) this.vsaItems.CreateItem(strArray1[index], JSVsaItemType.Reference, JSVsaItemFlag.None)).AssemblyName = strArray1[index];
    }
    else
    {
      object[] customAttributes = Microsoft.JScript.CustomAttribute.GetCustomAttributes(this.runtimeAssembly, typeof (ReferenceAttribute), false);
      string[] strArray = new string[customAttributes.Length];
      for (int index = 0; index < customAttributes.Length; ++index)
      {
        string reference = ((ReferenceAttribute) customAttributes[index]).reference;
        ((VsaReference) this.vsaItems.CreateItem(reference, JSVsaItemType.Reference, JSVsaItemFlag.None)).AssemblyName = reference;
        strArray[index] = reference;
      }
      VsaEngine.assemblyReferencesTable[(object) this.runtimeAssembly.FullName] = (object) strArray;
    }
  }

  private void EmitReferences()
  {
    SimpleHashtable simpleHashtable = new SimpleHashtable((uint) (this.vsaItems.Count + (this.implicitAssemblies == null ? 0 : this.implicitAssemblies.Count)));
    foreach (object vsaItem in (IEnumerable) this.vsaItems)
    {
      if (vsaItem is VsaReference)
      {
        string fullName = ((VsaReference) vsaItem).Assembly.GetName().FullName;
        if (simpleHashtable[(object) fullName] == null)
        {
          this.CompilerGlobals.assemblyBuilder.SetCustomAttribute(new CustomAttributeBuilder(CompilerGlobals.referenceAttributeConstructor, new object[1]
          {
            (object) fullName
          }));
          simpleHashtable[(object) fullName] = vsaItem;
        }
      }
    }
    if (this.implicitAssemblies == null)
      return;
    foreach (object implicitAssembly in this.implicitAssemblies)
    {
      Assembly assembly = implicitAssembly as Assembly;
      if (assembly != (Assembly) null)
      {
        string fullName = assembly.GetName().FullName;
        if (simpleHashtable[(object) fullName] == null)
        {
          this.CompilerGlobals.assemblyBuilder.SetCustomAttribute(new CustomAttributeBuilder(CompilerGlobals.referenceAttributeConstructor, new object[1]
          {
            (object) fullName
          }));
          simpleHashtable[(object) fullName] = implicitAssembly;
        }
      }
    }
  }

  private void CreateMain()
  {
    TypeBuilder typeBuilder = this.CompilerGlobals.module.DefineType("JScript Main", TypeAttributes.Public);
    MethodBuilder entryMethod = typeBuilder.DefineMethod("Main", MethodAttributes.Public | MethodAttributes.Static, Typeob.Void, new Type[1]
    {
      Typeob.ArrayOfString
    });
    entryMethod.SetCustomAttribute(Typeob.STAThreadAttribute.GetConstructor(Type.EmptyTypes), new byte[0]);
    this.CreateEntryPointIL(entryMethod.GetILGenerator(), (FieldInfo) null);
    typeBuilder.CreateType();
    this.CompilerGlobals.assemblyBuilder.SetEntryPoint((MethodInfo) entryMethod, this.PEFileKind);
  }

  private void CreateStartupClass()
  {
    TypeBuilder startupClass = this.CompilerGlobals.module.DefineType(this.rootNamespace + "._Startup", TypeAttributes.Public, Typeob.BaseVsaStartup);
    FieldInfo field = Typeob.BaseVsaStartup.GetField("site", BindingFlags.Instance | BindingFlags.NonPublic);
    this.CreateEntryPointIL(startupClass.DefineMethod("Startup", MethodAttributes.Public | MethodAttributes.Virtual, Typeob.Void, Type.EmptyTypes).GetILGenerator(), field, startupClass);
    this.CreateShutdownIL(startupClass.DefineMethod("Shutdown", MethodAttributes.Public | MethodAttributes.Virtual, Typeob.Void, Type.EmptyTypes).GetILGenerator());
    startupClass.CreateType();
  }

  private void CreateEntryPointIL(ILGenerator il, FieldInfo site)
  {
    this.CreateEntryPointIL(il, site, (TypeBuilder) null);
  }

  private void CreateEntryPointIL(ILGenerator il, FieldInfo site, TypeBuilder startupClass)
  {
    LocalBuilder local1 = il.DeclareLocal(Typeob.GlobalScope);
    if (this.doFast)
      il.Emit(OpCodes.Ldc_I4_1);
    else
      il.Emit(OpCodes.Ldc_I4_0);
    SimpleHashtable simpleHashtable = new SimpleHashtable((uint) this.vsaItems.Count);
    ArrayList arrayList = new ArrayList();
    foreach (object vsaItem in (IEnumerable) this.vsaItems)
    {
      if (vsaItem is VsaReference)
      {
        string fullName = ((VsaReference) vsaItem).Assembly.GetName().FullName;
        if (simpleHashtable[(object) fullName] == null)
        {
          arrayList.Add((object) fullName);
          simpleHashtable[(object) fullName] = vsaItem;
        }
      }
    }
    if (this.implicitAssemblies != null)
    {
      foreach (object implicitAssembly in this.implicitAssemblies)
      {
        Assembly assembly = implicitAssembly as Assembly;
        if (assembly != (Assembly) null)
        {
          string fullName = assembly.GetName().FullName;
          if (simpleHashtable[(object) fullName] == null)
          {
            arrayList.Add((object) fullName);
            simpleHashtable[(object) fullName] = implicitAssembly;
          }
        }
      }
    }
    ConstantWrapper.TranslateToILInt(il, arrayList.Count);
    il.Emit(OpCodes.Newarr, Typeob.String);
    int num = 0;
    foreach (string str in arrayList)
    {
      il.Emit(OpCodes.Dup);
      ConstantWrapper.TranslateToILInt(il, num++);
      il.Emit(OpCodes.Ldstr, str);
      il.Emit(OpCodes.Stelem_Ref);
    }
    if ((Type) startupClass != (Type) null)
    {
      il.Emit(OpCodes.Ldtoken, (Type) startupClass);
      if (this.rootNamespace != null)
        il.Emit(OpCodes.Ldstr, this.rootNamespace);
      else
        il.Emit(OpCodes.Ldnull);
      MethodInfo method = Typeob.VsaEngine.GetMethod("CreateEngineAndGetGlobalScopeWithTypeAndRootNamespace");
      il.Emit(OpCodes.Call, method);
    }
    else
    {
      MethodInfo method = Typeob.VsaEngine.GetMethod("CreateEngineAndGetGlobalScope");
      il.Emit(OpCodes.Call, method);
    }
    il.Emit(OpCodes.Stloc, local1);
    if (site != (FieldInfo) null)
      this.CreateHostCallbackIL(il, site);
    bool flag1 = this.genDebugInfo;
    bool flag2 = false;
    foreach (object vsaItem in (IEnumerable) this.vsaItems)
    {
      Type compiledType = ((VsaItem) vsaItem).GetCompiledType();
      if ((Type) null != compiledType)
      {
        ConstructorInfo constructor = compiledType.GetConstructor(new Type[1]
        {
          Typeob.GlobalScope
        });
        MethodInfo method = compiledType.GetMethod("Global Code");
        if (flag1)
        {
          this.CompilerGlobals.module.SetUserEntryPoint(method);
          flag1 = false;
        }
        il.Emit(OpCodes.Ldloc, local1);
        il.Emit(OpCodes.Newobj, constructor);
        if (!flag2 && vsaItem is VsaStaticCode)
        {
          LocalBuilder local2 = il.DeclareLocal(compiledType);
          il.Emit(OpCodes.Stloc, local2);
          il.Emit(OpCodes.Ldloc, local1);
          il.Emit(OpCodes.Ldfld, CompilerGlobals.engineField);
          il.Emit(OpCodes.Ldloc, local2);
          il.Emit(OpCodes.Call, CompilerGlobals.pushScriptObjectMethod);
          il.Emit(OpCodes.Ldloc, local2);
          flag2 = true;
        }
        il.Emit(OpCodes.Call, method);
        il.Emit(OpCodes.Pop);
      }
    }
    if (flag2)
    {
      il.Emit(OpCodes.Ldloc, local1);
      il.Emit(OpCodes.Ldfld, CompilerGlobals.engineField);
      il.Emit(OpCodes.Call, CompilerGlobals.popScriptObjectMethod);
      il.Emit(OpCodes.Pop);
    }
    il.Emit(OpCodes.Ret);
  }

  private void CreateHostCallbackIL(ILGenerator il, FieldInfo site)
  {
    MethodInfo method = site.FieldType.GetMethod("GetGlobalInstance");
    site.FieldType.GetMethod("GetEventSourceInstance");
    foreach (object vsaItem in (IEnumerable) this.vsaItems)
    {
      if (vsaItem is VsaHostObject)
      {
        VsaHostObject vsaHostObject = (VsaHostObject) vsaItem;
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldfld, site);
        il.Emit(OpCodes.Ldstr, vsaHostObject.Name);
        il.Emit(OpCodes.Callvirt, method);
        Type fieldType = vsaHostObject.Field.FieldType;
        il.Emit(OpCodes.Ldtoken, fieldType);
        il.Emit(OpCodes.Call, CompilerGlobals.getTypeFromHandleMethod);
        ConstantWrapper.TranslateToILInt(il, 0);
        il.Emit(OpCodes.Call, CompilerGlobals.coerceTMethod);
        if (fieldType.IsValueType)
          Microsoft.JScript.Convert.EmitUnbox(il, fieldType, Type.GetTypeCode(fieldType));
        else
          il.Emit(OpCodes.Castclass, fieldType);
        il.Emit(OpCodes.Stsfld, vsaHostObject.Field);
      }
    }
  }

  private void CreateShutdownIL(ILGenerator il)
  {
    foreach (object vsaItem in (IEnumerable) this.vsaItems)
    {
      if (vsaItem is VsaHostObject)
      {
        il.Emit(OpCodes.Ldnull);
        il.Emit(OpCodes.Stsfld, ((VsaHostObject) vsaItem).Field);
      }
    }
    il.Emit(OpCodes.Ret);
  }

  /// <summary>This class, member, or property is internal to the script engine and should not be called from your code.</summary>
  [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
  public void DisconnectEvents()
  {
  }

  protected override void DoClose()
  {
    ((VsaItems) this.vsaItems).Close();
    if (this.globalScope != null)
      this.globalScope.Close();
    this.vsaItems = (IJSVsaItems) null;
    this.engineSite = (IJSVsaSite) null;
    this.globalScope = (VsaScriptScope) null;
    this.runningThread = (Thread) null;
    this.compilerGlobals = (CompilerGlobals) null;
    this.globals = (Globals) null;
    ScriptStream.Out = Console.Out;
    ScriptStream.Error = Console.Error;
    this.rawPE = (byte[]) null;
    this.rawPDB = (byte[]) null;
    this.persistedAssembly = (Assembly) null;
    this.isClosed = true;
    if (this.tempDirectory == null || !Directory.Exists(this.tempDirectory))
      return;
    Directory.Delete(this.tempDirectory);
  }

  protected override bool DoCompile()
  {
    if (!this.isClosed && !this.isEngineCompiled)
    {
      this.SetUpCompilerEnvironment();
      if (this.PEFileName == null)
        this.PEFileName = this.GenerateRandomPEFileName();
      this.SaveSourceForDebugging();
      this.numberOfErrors = 0;
      this.isEngineCompiled = true;
      this.Globals.ScopeStack.Push(this.GetGlobalScope().GetObject());
      try
      {
        foreach (object vsaItem in (IEnumerable) this.vsaItems)
        {
          if (vsaItem is VsaReference)
            ((VsaItem) vsaItem).Compile();
        }
        if (this.vsaItems.Count > 0)
          this.SetEnclosingContext((ScriptObject) new WrappedNamespace("", this));
        foreach (object vsaItem in (IEnumerable) this.vsaItems)
        {
          if (vsaItem is VsaHostObject)
            ((VsaItem) vsaItem).Compile();
        }
        foreach (object vsaItem in (IEnumerable) this.vsaItems)
        {
          if (vsaItem is VsaStaticCode)
            ((VsaStaticCode) vsaItem).Parse();
        }
        foreach (object vsaItem in (IEnumerable) this.vsaItems)
        {
          if (vsaItem is VsaStaticCode)
            ((VsaStaticCode) vsaItem).ProcessAssemblyAttributeLists();
        }
        foreach (object vsaItem in (IEnumerable) this.vsaItems)
        {
          if (vsaItem is VsaStaticCode)
            ((VsaStaticCode) vsaItem).PartiallyEvaluate();
        }
        foreach (object vsaItem in (IEnumerable) this.vsaItems)
        {
          if (vsaItem is VsaStaticCode)
            ((VsaStaticCode) vsaItem).TranslateToIL();
        }
        foreach (object vsaItem in (IEnumerable) this.vsaItems)
        {
          if (vsaItem is VsaStaticCode)
            ((VsaItem) vsaItem).GetCompiledType();
        }
        if (this.globalScope != null)
          this.globalScope.Compile();
      }
      catch (JScriptException ex)
      {
        this.OnCompilerError(ex);
      }
      catch (FileLoadException ex)
      {
        this.OnCompilerError(new JScriptException(JSError.ImplicitlyReferencedAssemblyNotFound)
        {
          value = (object) ex.FileName
        });
        this.isEngineCompiled = false;
      }
      catch (EndOfFile ex)
      {
      }
      catch
      {
        this.isEngineCompiled = false;
        throw;
      }
      finally
      {
        this.Globals.ScopeStack.Pop();
      }
      if (this.isEngineCompiled)
        this.isEngineCompiled = this.numberOfErrors == 0 || this.alwaysGenerateIL;
    }
    if (this.win32resource != null)
      this.CompilerGlobals.assemblyBuilder.DefineUnmanagedResource(this.win32resource);
    else if (this.compilerGlobals != null)
      this.compilerGlobals.assemblyBuilder.DefineVersionInfoResource();
    if (this.managedResources != null)
    {
      foreach (ResInfo managedResource in (IEnumerable) this.managedResources)
      {
        if (managedResource.isLinked)
        {
          this.CompilerGlobals.assemblyBuilder.AddResourceFile(managedResource.name, Path.GetFileName(managedResource.filename), managedResource.isPublic ? ResourceAttributes.Public : ResourceAttributes.Private);
        }
        else
        {
          try
          {
            using (ResourceReader resourceReader = new ResourceReader(managedResource.filename))
            {
              IResourceWriter resourceWriter = this.CompilerGlobals.module.DefineResource(managedResource.name, managedResource.filename, managedResource.isPublic ? ResourceAttributes.Public : ResourceAttributes.Private);
              foreach (DictionaryEntry dictionaryEntry in resourceReader)
                resourceWriter.AddResource((string) dictionaryEntry.Key, dictionaryEntry.Value);
            }
          }
          catch (ArgumentException ex)
          {
            this.OnCompilerError(new JScriptException(JSError.InvalidResource)
            {
              value = (object) managedResource.filename
            });
            this.isEngineCompiled = false;
            return false;
          }
        }
      }
    }
    if (this.isEngineCompiled)
      this.EmitReferences();
    if (this.isEngineCompiled)
    {
      if (this.doSaveAfterCompile)
      {
        if (this.PEFileKind != PEFileKinds.Dll)
          this.CreateMain();
        try
        {
          this.compilerGlobals.assemblyBuilder.Save(this.PEFileName, this.PEKindFlags, this.PEMachineArchitecture);
        }
        catch (Exception ex)
        {
          throw new JSVsaException(JSVsaError.SaveCompiledStateFailed, ex.Message, ex);
        }
      }
      else if (this.genStartupClass)
        this.CreateStartupClass();
    }
    return this.isEngineCompiled;
  }

  internal CultureInfo ErrorCultureInfo
  {
    get
    {
      if (this.errorCultureInfo == null || this.errorCultureInfo.LCID != this.errorLocale)
        this.errorCultureInfo = new CultureInfo(this.errorLocale);
      return this.errorCultureInfo;
    }
  }

  private string GenerateRandomPEFileName()
  {
    if (this.randomNumberGenerator == null)
      this.randomNumberGenerator = new RNGCryptoServiceProvider();
    byte[] numArray = new byte[6];
    this.randomNumberGenerator.GetBytes(numArray);
    string str1 = System.Convert.ToBase64String(numArray).Replace('/', '-').Replace('+', '_');
    if (this.tempDirectory == null)
      this.tempDirectory = Path.GetTempPath() + str1;
    string str2 = str1 + (this.PEFileKind == PEFileKinds.Dll ? ".dll" : ".exe");
    return this.tempDirectory + Path.DirectorySeparatorChar.ToString() + str2;
  }

  /// <summary>This class, member, or property is internal to the script engine and should not be called from your code.</summary>
  [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
  public Assembly GetAssembly()
  {
    this.TryObtainLock();
    try
    {
      if (this.persistedAssembly != null)
        return this.persistedAssembly;
      if (this.PEFileName != null && File.Exists(this.PEFileName))
        return Assembly.LoadFrom(this.PEFileName);
      if (this.compilerGlobals != null && this.compilerGlobals.assemblyBuilder is PersistedAssemblyBuilder)
      {
        byte[] pe;
        byte[] debugInfo;
        this.DoSaveCompiledState(out pe, out debugInfo);
        this.persistedAssembly = Assembly.Load(pe, debugInfo);
        return this.persistedAssembly;
      }
      return this.compilerGlobals != null ? (Assembly) this.compilerGlobals.assemblyBuilder : (Assembly) null;
    }
    finally
    {
      this.ReleaseLock();
    }
  }

  internal ClassScope GetClass(string className)
  {
    if (this.packages != null)
    {
      int index = 0;
      for (int count = this.packages.Count; index < count; ++index)
      {
        object memberValue = ((ActivationObject) this.packages[index]).GetMemberValue(className, 1);
        if (!(memberValue is Microsoft.JScript.Missing))
          return (ClassScope) memberValue;
      }
    }
    return (ClassScope) null;
  }

  /// <summary>This class, member, or property is internal to the script engine and should not be called from your code.</summary>
  [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
  public IJSVsaItem GetItem(string itemName) => this.vsaItems[itemName];

  /// <summary>This class, member, or property is internal to the script engine and should not be called from your code.</summary>
  [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
  public IJSVsaItem GetItemAtIndex(int index) => this.vsaItems[index];

  /// <summary>This class, member, or property is internal to the script engine and should not be called from your code.</summary>
  [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
  public int GetItemCount() => this.vsaItems.Count;

  /// <summary>This class, member, or property is internal to the script engine and should not be called from your code.</summary>
  public IVsaScriptScope GetGlobalScope()
  {
    if (this.globalScope == null)
    {
      this.globalScope = new VsaScriptScope(this, "Global", (VsaScriptScope) null);
      GlobalScope globalScope = (GlobalScope) this.globalScope.GetObject();
      globalScope.globalObject = this.Globals.globalObject;
      globalScope.fast = this.doFast;
      globalScope.isKnownAtCompileTime = this.doFast;
    }
    return (IVsaScriptScope) this.globalScope;
  }

  /// <summary>This class, member, or property is internal to the script engine and should not be called from your code.</summary>
  public GlobalScope GetMainScope()
  {
    ScriptObject mainScope = this.ScriptObjectStackTop();
    while (true)
    {
      switch (mainScope)
      {
        case null:
        case GlobalScope _:
          goto label_3;
        default:
          mainScope = mainScope.GetParent();
          continue;
      }
    }
label_3:
    return (GlobalScope) mainScope;
  }

  /// <summary>This class, member, or property is internal to the script engine and should not be called from your code.</summary>
  public Module GetModule()
  {
    return this.PEFileName != null ? this.GetAssembly().GetModules()[0] : (Module) this.CompilerGlobals.module;
  }

  /// <summary>This class, member, or property is internal to the script engine and should not be called from your code.</summary>
  public ArrayConstructor GetOriginalArrayConstructor() => this.Globals.globalObject.originalArray;

  /// <summary>This class, member, or property is internal to the script engine and should not be called from your code.</summary>
  public ObjectConstructor GetOriginalObjectConstructor()
  {
    return this.Globals.globalObject.originalObject;
  }

  /// <summary>This class, member, or property is internal to the script engine and should not be called from your code.</summary>
  public RegExpConstructor GetOriginalRegExpConstructor()
  {
    return this.Globals.globalObject.originalRegExp;
  }

  protected override object GetCustomOption(string name)
  {
    if (string.Compare(name, "CLSCompliant", StringComparison.OrdinalIgnoreCase) == 0)
      return (object) this.isCLSCompliant;
    if (string.Compare(name, "fast", StringComparison.OrdinalIgnoreCase) == 0)
      return (object) this.doFast;
    if (string.Compare(name, "output", StringComparison.OrdinalIgnoreCase) == 0)
      return (object) this.PEFileName;
    if (string.Compare(name, "PEFileKind", StringComparison.OrdinalIgnoreCase) == 0)
      return (object) this.PEFileKind;
    if (string.Compare(name, "PortableExecutableKind", StringComparison.OrdinalIgnoreCase) == 0)
      return (object) this.PEKindFlags;
    if (string.Compare(name, "ImageFileMachine", StringComparison.OrdinalIgnoreCase) == 0)
      return (object) this.PEMachineArchitecture;
    if (string.Compare(name, "ReferenceLoaderAPI", StringComparison.OrdinalIgnoreCase) == 0)
    {
      switch (this.ReferenceLoaderAPI)
      {
        case LoaderAPI.LoadFrom:
          return (object) "LoadFrom";
        case LoaderAPI.LoadFile:
          return (object) "LoadFile";
        case LoaderAPI.ReflectionOnlyLoadFrom:
          return (object) "ReflectionOnlyLoadFrom";
        default:
          throw new JSVsaException(JSVsaError.OptionNotSupported);
      }
    }
    else
    {
      if (string.Compare(name, "print", StringComparison.OrdinalIgnoreCase) == 0)
        return (object) this.doPrint;
      if (string.Compare(name, "UseContextRelativeStatics", StringComparison.OrdinalIgnoreCase) == 0)
        return (object) this.doCRS;
      if (string.Compare(name, "optimize", StringComparison.OrdinalIgnoreCase) == 0)
        return (object) null;
      if (string.Compare(name, "define", StringComparison.OrdinalIgnoreCase) == 0)
        return (object) null;
      if (string.Compare(name, "defines", StringComparison.OrdinalIgnoreCase) == 0)
        return (object) this.Defines;
      if (string.Compare(name, "ee", StringComparison.OrdinalIgnoreCase) == 0)
        return (object) VsaEngine.executeForJSEE;
      if (string.Compare(name, "version", StringComparison.OrdinalIgnoreCase) == 0)
        return (object) this.versionInfo;
      if (string.Compare(name, "VersionSafe", StringComparison.OrdinalIgnoreCase) == 0)
        return (object) this.versionSafe;
      if (string.Compare(name, "warnaserror", StringComparison.OrdinalIgnoreCase) == 0)
        return (object) this.doWarnAsError;
      if (string.Compare(name, "WarningLevel", StringComparison.OrdinalIgnoreCase) == 0)
        return (object) this.nWarningLevel;
      if (string.Compare(name, "win32resource", StringComparison.OrdinalIgnoreCase) == 0)
        return (object) this.win32resource;
      if (string.Compare(name, "managedResources", StringComparison.OrdinalIgnoreCase) == 0)
        return (object) this.managedResources;
      if (string.Compare(name, "alwaysGenerateIL", StringComparison.OrdinalIgnoreCase) == 0)
        return (object) this.alwaysGenerateIL;
      if (string.Compare(name, "DebugDirectory", StringComparison.OrdinalIgnoreCase) == 0)
        return (object) this.debugDirectory;
      if (string.Compare(name, "AutoRef", StringComparison.OrdinalIgnoreCase) == 0)
        return (object) this.autoRef;
      throw new JSVsaException(JSVsaError.OptionNotSupported);
    }
  }

  internal int GetStaticCodeBlockCount() => ((VsaItems) this.vsaItems).staticCodeBlockCount;

  internal Type GetType(string typeName)
  {
    if (this.cachedTypeLookups == null)
      this.cachedTypeLookups = new SimpleHashtable(1000U);
    object cachedTypeLookup = this.cachedTypeLookups[(object) typeName];
    if (cachedTypeLookup != null)
      return cachedTypeLookup as Type;
    int index1 = 0;
    for (int count = this.Scopes.Count; index1 < count; ++index1)
    {
      Type type = Globals.TypeRefs.ToReferenceContext(((GlobalScope) this.scopes[index1]).GetType()).Assembly.GetType(typeName, false);
      if (type != (Type) null)
      {
        this.cachedTypeLookups[(object) typeName] = (object) type;
        return type;
      }
    }
    if (this.runtimeAssembly != (Assembly) null)
    {
      this.AddReferences();
      this.runtimeAssembly = (Assembly) null;
    }
    int index2 = 0;
    for (int count = this.vsaItems.Count; index2 < count; ++index2)
    {
      object vsaItem = (object) this.vsaItems[index2];
      if (vsaItem is VsaReference)
      {
        Type type = ((VsaReference) vsaItem).GetType(typeName);
        if (type != (Type) null)
        {
          this.cachedTypeLookups[(object) typeName] = (object) type;
          return type;
        }
      }
    }
    if (this.implicitAssemblies == null)
    {
      this.cachedTypeLookups[(object) typeName] = (object) false;
      return (Type) null;
    }
    int index3 = 0;
    for (int count = this.implicitAssemblies.Count; index3 < count; ++index3)
    {
      Type type = ((Assembly) this.implicitAssemblies[index3]).GetType(typeName, false);
      if (type != (Type) null)
      {
        if (type.IsPublic && !Microsoft.JScript.CustomAttribute.IsDefined((MemberInfo) type, typeof (RequiredAttributeAttribute), true))
        {
          this.cachedTypeLookups[(object) typeName] = (object) type;
          return type;
        }
      }
    }
    this.cachedTypeLookups[(object) typeName] = (object) false;
    return (Type) null;
  }

  internal Globals Globals
  {
    get
    {
      if (this.globals == null)
        this.globals = new Globals(this.doFast, this);
      return this.globals;
    }
  }

  internal bool HasErrors => this.numberOfErrors != 0;

  private JSScanner GetScannerInstance(string name)
  {
    char[] anyOf = new char[21]
    {
      '\t',
      '\n',
      '\v',
      '\f',
      '\r',
      ' ',
      ' ',
      ' ',
      ' ',
      ' ',
      ' ',
      ' ',
      ' ',
      ' ',
      ' ',
      ' ',
      ' ',
      ' ',
      '\u200B',
      '　',
      '\uFEFF'
    };
    if (name == null || name.IndexOfAny(anyOf) > -1)
      return (JSScanner) null;
    Context sourceContext = new Context(new DocumentContext((VsaItem) new VsaStaticCode(this, "itemName", JSVsaItemFlag.None)), name);
    sourceContext.errorReported = -1;
    JSScanner scannerInstance = new JSScanner();
    scannerInstance.SetSource(sourceContext);
    return scannerInstance;
  }

  /// <summary>This class, member, or property is internal to the script engine and should not be called from your code.</summary>
  [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
  public void InitVsaEngine(string rootMoniker, IJSVsaSite site)
  {
    this.genStartupClass = false;
    this.engineMoniker = rootMoniker;
    this.engineSite = site;
    this.isEngineInitialized = true;
    this.rootNamespace = "JScript.DefaultNamespace";
    this.isEngineDirty = true;
    this.isEngineCompiled = false;
  }

  /// <summary>This class, member, or property is internal to the script engine and should not be called from your code.</summary>
  [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
  public void Interrupt()
  {
    if (this.runningThread == null)
      return;
    // Thread.Abort is not supported on .NET Core; there is no safe way to
    // terminate the running script from another thread.
    throw new PlatformNotSupportedException("Interrupting a running script is not supported on .NET Core.");
  }

  protected override bool IsValidNamespaceName(string name)
  {
    JSScanner scannerInstance = this.GetScannerInstance(name);
    if (scannerInstance == null)
      return false;
    while (scannerInstance.PeekToken() == JSToken.Identifier)
    {
      scannerInstance.GetNextToken();
      if (scannerInstance.PeekToken() == JSToken.EndOfFile)
        return true;
      if (scannerInstance.PeekToken() != JSToken.AccessField)
        return false;
      scannerInstance.GetNextToken();
    }
    return false;
  }

  /// <summary>This class, member, or property is internal to the script engine and should not be called from your code.</summary>
  [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
  public override bool IsValidIdentifier(string ident)
  {
    JSScanner scannerInstance = this.GetScannerInstance(ident);
    if (scannerInstance == null || scannerInstance.PeekToken() != JSToken.Identifier)
      return false;
    scannerInstance.GetNextToken();
    return scannerInstance.PeekToken() == JSToken.EndOfFile;
  }

  /// <summary>This class, member, or property is internal to the script engine and should not be called from your code.</summary>
  public LenientGlobalObject LenientGlobalObject => (LenientGlobalObject) this.Globals.globalObject;

  protected override Assembly LoadCompiledState()
  {
    if ((Assembly) this.persistedAssembly != (Assembly) null)
      return this.persistedAssembly;
    if (this.compilerGlobals != null && this.compilerGlobals.assemblyBuilder is PersistedAssemblyBuilder)
    {
      byte[] pe;
      byte[] debugInfo;
      this.DoSaveCompiledState(out pe, out debugInfo);
      this.persistedAssembly = Assembly.Load(pe, debugInfo);
      return this.persistedAssembly;
    }
    if (this.compilerGlobals != null && (Assembly) this.compilerGlobals.assemblyBuilder != (Assembly) null)
      return (Assembly) this.compilerGlobals.assemblyBuilder;
    byte[] peImage;
    byte[] debugInfoBytes;
    this.DoSaveCompiledState(out peImage, out debugInfoBytes);
    return Assembly.Load(peImage, debugInfoBytes);
  }

  protected override void DoLoadSourceState(IJSVsaPersistSite site)
  {
    string xml = site.LoadElement((string) null);
    try
    {
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml(xml);
      XmlElement documentElement = xmlDocument.DocumentElement;
      if (!(this.LoadProjectVersion(documentElement) == VsaEngine.CurrentProjectVersion))
        return;
      this.LoadVsaEngineState(documentElement);
      this.isEngineDirty = false;
    }
    catch (Exception ex)
    {
      throw new JSVsaException(JSVsaError.UnknownError, ex.ToString(), ex);
    }
  }

  private System.Version LoadProjectVersion(XmlElement root)
  {
    return new System.Version(root["ProjectVersion"].GetAttribute("Version"));
  }

  private void LoadVsaEngineState(XmlElement parent)
  {
    XmlElement parent1 = parent[nameof (VsaEngine)];
    this.applicationPath = parent1.GetAttribute("ApplicationBase");
    this.genDebugInfo = bool.Parse(parent1.GetAttribute("GenerateDebugInfo"));
    this.scriptLanguage = parent1.GetAttribute("Language");
    this.LCID = int.Parse(parent1.GetAttribute("LCID"), (IFormatProvider) CultureInfo.InvariantCulture);
    this.Name = parent1.GetAttribute("Name");
    this.rootNamespace = parent1.GetAttribute("RootNamespace");
    this.assemblyVersion = parent1.GetAttribute("Version");
    this.LoadCustomOptions(parent1);
    this.LoadVsaItems(parent1);
  }

  private void LoadCustomOptions(XmlElement parent)
  {
    XmlElement parent1 = parent["Options"];
    this.doFast = bool.Parse(parent1.GetAttribute("fast"));
    this.doPrint = bool.Parse(parent1.GetAttribute("print"));
    this.doCRS = bool.Parse(parent1.GetAttribute("UseContextRelativeStatics"));
    this.versionSafe = bool.Parse(parent1.GetAttribute("VersionSafe"));
    this.libpath = parent1.GetAttribute("libpath");
    this.doWarnAsError = bool.Parse(parent1.GetAttribute("warnaserror"));
    this.nWarningLevel = int.Parse(parent1.GetAttribute("WarningLevel"), (IFormatProvider) CultureInfo.InvariantCulture);
    if (parent1.HasAttribute("win32resource"))
      this.win32resource = parent1.GetAttribute("win32resource");
    this.LoadUserDefines(parent1);
    this.LoadManagedResources(parent1);
  }

  private void LoadUserDefines(XmlElement parent)
  {
    foreach (XmlElement childNode in parent["Defines"].ChildNodes)
      this.Defines[(object) childNode.Name] = (object) childNode.GetAttribute("Value");
  }

  private void LoadManagedResources(XmlElement parent)
  {
    XmlNodeList childNodes = parent["ManagedResources"].ChildNodes;
    if (childNodes.Count <= 0)
      return;
    this.managedResources = (ICollection) new ArrayList(childNodes.Count);
    foreach (XmlElement xmlElement in childNodes)
    {
      string attribute1 = xmlElement.GetAttribute("Name");
      string attribute2 = xmlElement.GetAttribute("FileName");
      bool isPublic = bool.Parse(xmlElement.GetAttribute("Public"));
      bool isLinked = bool.Parse(xmlElement.GetAttribute("Linked"));
      ((ArrayList) this.managedResources).Add((object) new ResInfo(attribute2, attribute1, isPublic, isLinked));
    }
  }

  private void LoadVsaItems(XmlElement parent)
  {
    XmlNodeList childNodes = parent["VsaItems"].ChildNodes;
    string strB1 = JSVsaItemType.Reference.ToString();
    string strB2 = JSVsaItemType.AppGlobal.ToString();
    string strB3 = JSVsaItemType.Code.ToString();
    foreach (XmlElement xmlElement in childNodes)
    {
      string attribute1 = xmlElement.GetAttribute("Name");
      string attribute2 = xmlElement.GetAttribute("ItemType");
      IJSVsaItem jsVsaItem;
      if (string.Compare(attribute2, strB1, StringComparison.OrdinalIgnoreCase) == 0)
      {
        jsVsaItem = this.vsaItems.CreateItem(attribute1, JSVsaItemType.Reference, JSVsaItemFlag.None);
        ((IJSVsaReferenceItem) jsVsaItem).AssemblyName = xmlElement.GetAttribute("AssemblyName");
      }
      else if (string.Compare(attribute2, strB2, StringComparison.OrdinalIgnoreCase) == 0)
      {
        jsVsaItem = this.vsaItems.CreateItem(attribute1, JSVsaItemType.AppGlobal, JSVsaItemFlag.None);
        ((IJSVsaGlobalItem) jsVsaItem).ExposeMembers = bool.Parse(xmlElement.GetAttribute("ExposeMembers"));
        ((IJSVsaGlobalItem) jsVsaItem).TypeString = xmlElement.GetAttribute("TypeString");
      }
      else
      {
        if (string.Compare(attribute2, strB3, StringComparison.OrdinalIgnoreCase) != 0)
          throw new JSVsaException(JSVsaError.LoadElementFailed);
        jsVsaItem = this.vsaItems.CreateItem(attribute1, JSVsaItemType.Code, JSVsaItemFlag.None);
        string str = xmlElement.FirstChild.Value.Replace(" >", ">");
        ((IJSVsaCodeItem) jsVsaItem).SourceText = str;
      }
      foreach (XmlElement childNode in xmlElement["Options"].ChildNodes)
        jsVsaItem.SetOption(childNode.Name, (object) childNode.GetAttribute("Value"));
      ((VsaItem) jsVsaItem).IsDirty = false;
    }
  }

  internal bool OnCompilerError(JScriptException se)
  {
    if (se.Severity == 0 || this.doWarnAsError && se.Severity <= this.nWarningLevel)
      ++this.numberOfErrors;
    bool flag = this.engineSite.OnCompilerError((IJSVsaError) se);
    if (!flag)
      this.isEngineCompiled = false;
    return flag;
  }

  /// <summary>This class, member, or property is internal to the script engine and should not be called from your code.</summary>
  public ScriptObject PopScriptObject() => (ScriptObject) this.Globals.ScopeStack.Pop();

  /// <summary>This class, member, or property is internal to the script engine and should not be called from your code.</summary>
  public void PushScriptObject(ScriptObject obj) => this.Globals.ScopeStack.Push((object) obj);

  /// <summary>This class, member, or property is internal to the script engine and should not be called from your code.</summary>
  [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
  public void RegisterEventSource(string name)
  {
  }

  /// <summary>This class, member, or property is internal to the script engine and should not be called from your code.</summary>
  [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
  public override void Reset()
  {
    if (this.genStartupClass)
      base.Reset();
    else
      this.ResetCompiledState();
  }

  protected override void ResetCompiledState()
  {
    if (this.globalScope != null)
    {
      this.globalScope.Reset();
      this.globalScope = (VsaScriptScope) null;
    }
    this.classCounter = 0;
    this.haveCompiledState = false;
    this.failedCompilation = true;
    this.compiledRootNamespace = (string) null;
    this.startupClass = (Type) null;
    this.compilerGlobals = (CompilerGlobals) null;
    this.globals = (Globals) null;
    foreach (VsaItem vsaItem in (IEnumerable) this.vsaItems)
      vsaItem.Reset();
    this.implicitAssemblies = (ArrayList) null;
    this.implicitAssemblyCache = (SimpleHashtable) null;
    this.cachedTypeLookups = (SimpleHashtable) null;
    this.isEngineCompiled = false;
    this.isEngineRunning = false;
    this.isCompilerSet = false;
    this.packages = (ArrayList) null;
    if (!this.doSaveAfterCompile)
      this.PEFileName = (string) null;
    this.rawPE = (byte[]) null;
    this.rawPDB = (byte[]) null;
    this.persistedAssembly = (Assembly) null;
  }

  /// <summary>This class, member, or property is internal to the script engine and should not be called from your code.</summary>
  [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
  public void Restart()
  {
    this.TryObtainLock();
    try
    {
      ((VsaItems) this.vsaItems).Close();
      if (this.globalScope != null)
        this.globalScope.Close();
      this.globalScope = (VsaScriptScope) null;
      this.vsaItems = (IJSVsaItems) new VsaItems(this);
      this.isEngineRunning = false;
      this.isEngineCompiled = false;
      this.isCompilerSet = false;
      this.isClosed = false;
      this.runningThread = (Thread) null;
      this.globals = (Globals) null;
    }
    finally
    {
      this.ReleaseLock();
    }
  }

  /// <summary>This class, member, or property is internal to the script engine and should not be called from your code.</summary>
  [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
  public void RunEmpty()
  {
    this.TryObtainLock();
    try
    {
      this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed | BaseVsaEngine.Pre.RootMonikerSet | BaseVsaEngine.Pre.SiteSet);
      this.isEngineRunning = true;
      this.runningThread = Thread.CurrentThread;
      if (this.globalScope != null)
        this.globalScope.Run();
      foreach (VsaItem vsaItem in (IEnumerable) this.vsaItems)
        vsaItem.Run();
    }
    finally
    {
      this.runningThread = (Thread) null;
      this.ReleaseLock();
    }
  }

  /// <summary>This class, member, or property is internal to the script engine and should not be called from your code.</summary>
  [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
  public void Run(System.AppDomain domain) => throw new NotImplementedException();

  protected override void DoSaveCompiledState(out byte[] pe, out byte[] pdb)
  {
    pe = (byte[]) null;
    pdb = (byte[]) null;
    if (this.rawPE == null && this.compilerGlobals != null && this.compilerGlobals.assemblyBuilder is PersistedAssemblyBuilder)
    {
      try
      {
        if (this.PEFileName != null && File.Exists(this.PEFileName))
        {
          this.rawPE = File.ReadAllBytes(this.PEFileName);
          string pdbFileName = Path.ChangeExtension(this.PEFileName, ".pdb");
          this.rawPDB = File.Exists(pdbFileName) ? File.ReadAllBytes(pdbFileName) : (byte[]) null;
        }
        else
        {
          this.rawPE = ((PersistedAssemblyBuilder) this.compilerGlobals.assemblyBuilder).SaveToBytes(out this.rawPDB);
        }
      }
      catch (Exception ex)
      {
        throw new JSVsaException(JSVsaError.SaveCompiledStateFailed, ex.ToString(), ex);
      }
    }
    else if (this.rawPE == null)
    {
      try
      {
        if (!Directory.Exists(this.tempDirectory))
          Directory.CreateDirectory(this.tempDirectory);
        this.compilerGlobals.assemblyBuilder.Save(this.PEFileName, this.PEKindFlags, this.PEMachineArchitecture);
        string path = Path.ChangeExtension(this.PEFileName, ".pdb");
        try
        {
          FileStream fileStream1 = new FileStream(this.PEFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
          try
          {
            this.rawPE = new byte[(int) fileStream1.Length];
            fileStream1.Read(this.rawPE, 0, this.rawPE.Length);
          }
          finally
          {
            fileStream1.Close();
          }
          if (File.Exists(path))
          {
            FileStream fileStream2 = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            try
            {
              this.rawPDB = new byte[(int) fileStream2.Length];
              fileStream2.Read(this.rawPDB, 0, this.rawPDB.Length);
            }
            finally
            {
              fileStream2.Close();
            }
          }
        }
        finally
        {
          File.Delete(this.PEFileName);
          if (File.Exists(path))
            File.Delete(path);
        }
      }
      catch (Exception ex)
      {
        throw new JSVsaException(JSVsaError.SaveCompiledStateFailed, ex.ToString(), ex);
      }
    }
    pe = this.rawPE;
    pdb = this.rawPDB;
  }

  protected override void DoSaveSourceState(IJSVsaPersistSite site)
  {
    XmlDocument project = new XmlDocument();
    project.LoadXml("<project></project>");
    XmlElement documentElement = project.DocumentElement;
    this.SaveProjectVersion(project, documentElement);
    this.SaveVsaEngineState(project, documentElement);
    site.SaveElement((string) null, project.OuterXml);
    this.SaveSourceForDebugging();
    this.isEngineDirty = false;
  }

  private void SaveSourceForDebugging()
  {
    if (!this.GenerateDebugInfo || this.debugDirectory == null || !this.isEngineDirty)
      return;
    foreach (VsaItem vsaItem in (IEnumerable) this.vsaItems)
    {
      if (vsaItem is VsaStaticCode)
      {
        string path = $"{this.debugDirectory}{vsaItem.Name}.js";
        try
        {
          using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
          {
            using (StreamWriter streamWriter = new StreamWriter((Stream) fileStream))
              streamWriter.Write(((VsaStaticCode) vsaItem).SourceText);
            vsaItem.SetOption("codebase", (object) path);
          }
        }
        catch
        {
        }
      }
    }
  }

  private void SaveProjectVersion(XmlDocument project, XmlElement root)
  {
    XmlElement element = project.CreateElement("ProjectVersion");
    this.CreateAttribute(project, element, "Version", VsaEngine.CurrentProjectVersion.ToString());
    root.AppendChild((XmlNode) element);
  }

  private void SaveVsaEngineState(XmlDocument project, XmlElement parent)
  {
    XmlElement element = project.CreateElement(nameof (VsaEngine));
    this.CreateAttribute(project, element, "ApplicationBase", this.applicationPath);
    this.CreateAttribute(project, element, "GenerateDebugInfo", this.genDebugInfo.ToString());
    this.CreateAttribute(project, element, "Language", this.scriptLanguage);
    this.CreateAttribute(project, element, "LCID", this.errorLocale.ToString((IFormatProvider) CultureInfo.InvariantCulture));
    this.CreateAttribute(project, element, "Name", this.engineName);
    this.CreateAttribute(project, element, "RootNamespace", this.rootNamespace);
    this.CreateAttribute(project, element, "Version", this.assemblyVersion);
    this.SaveCustomOptions(project, element);
    this.SaveVsaItems(project, element);
    parent.AppendChild((XmlNode) element);
  }

  private void SaveCustomOptions(XmlDocument project, XmlElement parent)
  {
    XmlElement element = project.CreateElement("Options");
    this.CreateAttribute(project, element, "fast", this.doFast.ToString());
    this.CreateAttribute(project, element, "print", this.doPrint.ToString());
    this.CreateAttribute(project, element, "UseContextRelativeStatics", this.doCRS.ToString());
    this.CreateAttribute(project, element, "VersionSafe", this.versionSafe.ToString());
    this.CreateAttribute(project, element, "libpath", this.libpath);
    this.CreateAttribute(project, element, "warnaserror", this.doWarnAsError.ToString());
    this.CreateAttribute(project, element, "WarningLevel", this.nWarningLevel.ToString((IFormatProvider) CultureInfo.InvariantCulture));
    if (this.win32resource != null)
      this.CreateAttribute(project, element, "win32resource", this.win32resource);
    this.SaveUserDefines(project, element);
    this.SaveManagedResources(project, element);
    parent.AppendChild((XmlNode) element);
  }

  private void SaveUserDefines(XmlDocument project, XmlElement parent)
  {
    XmlElement element = project.CreateElement("Defines");
    if (this.Defines != null)
    {
      foreach (string key in (IEnumerable) this.Defines.Keys)
        this.AddChildAndValue(project, element, key, (string) this.Defines[(object) key]);
    }
    parent.AppendChild((XmlNode) element);
  }

  private void SaveManagedResources(XmlDocument project, XmlElement parent)
  {
    XmlElement element1 = project.CreateElement("ManagedResources");
    if (this.managedResources != null)
    {
      foreach (ResInfo managedResource in (IEnumerable) this.managedResources)
      {
        XmlElement element2 = project.CreateElement(managedResource.name);
        this.CreateAttribute(project, element2, "File", managedResource.filename);
        this.CreateAttribute(project, element2, "Public", managedResource.isPublic.ToString());
        this.CreateAttribute(project, element2, "Linked", managedResource.isLinked.ToString());
        element1.AppendChild((XmlNode) element2);
      }
    }
    parent.AppendChild((XmlNode) element1);
  }

  private void SaveVsaItems(XmlDocument project, XmlElement parent)
  {
    XmlElement element1 = project.CreateElement("VsaItems");
    foreach (IJSVsaItem vsaItem in (IEnumerable) this.vsaItems)
    {
      XmlElement element2 = project.CreateElement("IJSVsaItem");
      this.CreateAttribute(project, element2, "Name", vsaItem.Name);
      this.CreateAttribute(project, element2, "ItemType", vsaItem.ItemType.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      XmlElement element3 = project.CreateElement("Options");
      switch (vsaItem)
      {
        case VsaHostObject _:
          this.CreateAttribute(project, element2, "TypeString", ((VsaHostObject) vsaItem).TypeString);
          this.CreateAttribute(project, element2, "ExposeMembers", ((VsaHostObject) vsaItem).ExposeMembers.ToString((IFormatProvider) CultureInfo.InvariantCulture));
          break;
        case VsaReference _:
          this.CreateAttribute(project, element2, "AssemblyName", ((VsaReference) vsaItem).AssemblyName);
          break;
        case VsaStaticCode _:
          string data = ((VsaStaticCode) vsaItem).SourceText.Replace(">", " >");
          XmlCDataSection cdataSection = project.CreateCDataSection(data);
          element2.AppendChild((XmlNode) cdataSection);
          string option = (string) vsaItem.GetOption("codebase");
          if (option != null)
          {
            this.AddChildAndValue(project, element3, "codebase", option);
            break;
          }
          break;
        default:
          throw new JSVsaException(JSVsaError.SaveElementFailed);
      }
      ((VsaItem) vsaItem).IsDirty = false;
      element2.AppendChild((XmlNode) element3);
      element1.AppendChild((XmlNode) element2);
    }
    parent.AppendChild((XmlNode) element1);
  }

  internal ArrayList Scopes
  {
    get
    {
      if (this.scopes == null)
        this.scopes = new ArrayList(8);
      return this.scopes;
    }
  }

  /// <summary>This class, member, or property is internal to the script engine and should not be called from your code.</summary>
  public ScriptObject ScriptObjectStackTop() => this.Globals.ScopeStack.Peek();

  internal void SetEnclosingContext(ScriptObject ob)
  {
    ScriptObject scriptObject = this.Globals.ScopeStack.Peek();
    while (scriptObject.GetParent() != null)
      scriptObject = scriptObject.GetParent();
    scriptObject.SetParent(ob);
  }

  /// <summary>This class, member, or property is internal to the script engine and should not be called from your code.</summary>
  public void SetOutputStream(IMessageReceiver output)
  {
    StreamWriter streamWriter = new StreamWriter((Stream) new COMCharStream(output), Encoding.Default);
    streamWriter.AutoFlush = true;
    ScriptStream.Out = (TextWriter) streamWriter;
    ScriptStream.Error = (TextWriter) streamWriter;
  }

  protected override void SetCustomOption(string name, object value)
  {
    try
    {
      if (string.Compare(name, "CLSCompliant", StringComparison.OrdinalIgnoreCase) == 0)
        this.isCLSCompliant = (bool) value;
      else if (string.Compare(name, "fast", StringComparison.OrdinalIgnoreCase) == 0)
        this.doFast = (bool) value;
      else if (string.Compare(name, "output", StringComparison.OrdinalIgnoreCase) == 0)
      {
        if (!(value is string))
          return;
        this.PEFileName = (string) value;
        this.doSaveAfterCompile = true;
      }
      else if (string.Compare(name, "PEFileKind", StringComparison.OrdinalIgnoreCase) == 0)
        this.PEFileKind = (PEFileKinds) value;
      else if (string.Compare(name, "PortableExecutableKind", StringComparison.OrdinalIgnoreCase) == 0)
        this.PEKindFlags = (PortableExecutableKinds) value;
      else if (string.Compare(name, "ImageFileMachine", StringComparison.OrdinalIgnoreCase) == 0)
        this.PEMachineArchitecture = (ImageFileMachine) value;
      else if (string.Compare(name, "ReferenceLoaderAPI", StringComparison.OrdinalIgnoreCase) == 0)
      {
        string strA = (string) value;
        if (string.Compare(strA, "LoadFrom", StringComparison.OrdinalIgnoreCase) == 0)
          this.ReferenceLoaderAPI = LoaderAPI.LoadFrom;
        else if (string.Compare(strA, "LoadFile", StringComparison.OrdinalIgnoreCase) == 0)
        {
          this.ReferenceLoaderAPI = LoaderAPI.LoadFile;
        }
        else
        {
          if (string.Compare(strA, "ReflectionOnlyLoadFrom", StringComparison.OrdinalIgnoreCase) != 0)
            throw new JSVsaException(JSVsaError.OptionInvalid);
          this.ReferenceLoaderAPI = LoaderAPI.ReflectionOnlyLoadFrom;
        }
      }
      else if (string.Compare(name, "print", StringComparison.OrdinalIgnoreCase) == 0)
        this.doPrint = (bool) value;
      else if (string.Compare(name, "UseContextRelativeStatics", StringComparison.OrdinalIgnoreCase) == 0)
      {
        this.doCRS = (bool) value;
      }
      else
      {
        if (string.Compare(name, "optimize", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(name, "define", StringComparison.OrdinalIgnoreCase) == 0)
          return;
        if (string.Compare(name, "defines", StringComparison.OrdinalIgnoreCase) == 0)
          this.Defines = (Hashtable) value;
        else if (string.Compare(name, "ee", StringComparison.OrdinalIgnoreCase) == 0)
          VsaEngine.executeForJSEE = (bool) value;
        else if (string.Compare(name, "version", StringComparison.OrdinalIgnoreCase) == 0)
          this.versionInfo = (System.Version) value;
        else if (string.Compare(name, "VersionSafe", StringComparison.OrdinalIgnoreCase) == 0)
          this.versionSafe = (bool) value;
        else if (string.Compare(name, "libpath", StringComparison.OrdinalIgnoreCase) == 0)
          this.libpath = (string) value;
        else if (string.Compare(name, "warnaserror", StringComparison.OrdinalIgnoreCase) == 0)
          this.doWarnAsError = (bool) value;
        else if (string.Compare(name, "WarningLevel", StringComparison.OrdinalIgnoreCase) == 0)
          this.nWarningLevel = (int) value;
        else if (string.Compare(name, "win32resource", StringComparison.OrdinalIgnoreCase) == 0)
          this.win32resource = (string) value;
        else if (string.Compare(name, "managedResources", StringComparison.OrdinalIgnoreCase) == 0)
          this.managedResources = (ICollection) value;
        else if (string.Compare(name, "alwaysGenerateIL", StringComparison.OrdinalIgnoreCase) == 0)
          this.alwaysGenerateIL = (bool) value;
        else if (string.Compare(name, "DebugDirectory", StringComparison.OrdinalIgnoreCase) == 0)
        {
          if (value == null)
          {
            this.debugDirectory = (string) null;
          }
          else
          {
            if (!(value is string str1))
              throw new JSVsaException(JSVsaError.OptionInvalid);
            string fullPath;
            try
            {
              string str2 = Path.DirectorySeparatorChar.ToString();
              fullPath = Path.GetFullPath(str1 + str2);
              if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);
            }
            catch (Exception ex)
            {
              throw new JSVsaException(JSVsaError.OptionInvalid, "", ex);
            }
            this.debugDirectory = fullPath;
          }
        }
        else
        {
          if (string.Compare(name, "AutoRef", StringComparison.OrdinalIgnoreCase) != 0)
            throw new JSVsaException(JSVsaError.OptionNotSupported);
          this.autoRef = (bool) value;
        }
      }
    }
    catch (JSVsaException ex)
    {
      throw;
    }
    catch
    {
      throw new JSVsaException(JSVsaError.OptionInvalid);
    }
  }

  internal void SetUpCompilerEnvironment()
  {
    if (this.isCompilerSet)
      return;
    Globals.TypeRefs = this.TypeRefs;
    this.globals = this.Globals;
    this.isCompilerSet = true;
  }

  internal void TryToAddImplicitAssemblyReference(string name)
  {
    if (!this.autoRef)
      return;
    SimpleHashtable implicitAssemblyCache = this.implicitAssemblyCache;
    if (this.implicitAssemblyCache == null)
    {
      this.implicitAssemblyCache = new SimpleHashtable(50U);
      this.implicitAssemblyCache[(object) Path.GetFileNameWithoutExtension(this.PEFileName).ToLowerInvariant()] = (object) true;
      foreach (object vsaItem in (IEnumerable) this.vsaItems)
      {
        if (vsaItem is VsaReference vsaReference && vsaReference.AssemblyName != null)
        {
          string key = Path.GetFileName(vsaReference.AssemblyName).ToLowerInvariant();
          if (key.EndsWith(".dll", StringComparison.Ordinal))
            key = key.Substring(0, key.Length - 4);
          this.implicitAssemblyCache[(object) key] = (object) true;
        }
      }
      this.implicitAssemblyCache = this.implicitAssemblyCache;
    }
    string lowerInvariant = name.ToLowerInvariant();
    if (this.implicitAssemblyCache[(object) lowerInvariant] != null)
      return;
    this.implicitAssemblyCache[(object) lowerInvariant] = (object) true;
    try
    {
      VsaReference vsaReference = new VsaReference(this, name + ".dll");
      if (!vsaReference.Compile(false))
        return;
      ArrayList arrayList = this.implicitAssemblies;
      if (arrayList == null)
      {
        arrayList = new ArrayList();
        this.implicitAssemblies = arrayList;
      }
      arrayList.Add((object) vsaReference.Assembly);
    }
    catch (JSVsaException ex)
    {
    }
  }

  internal string RuntimeDirectory
  {
    get
    {
      if (this.runtimeDirectory == null)
        this.runtimeDirectory = Path.GetDirectoryName(typeof (object).Module.FullyQualifiedName);
      return this.runtimeDirectory;
    }
  }

  internal string[] LibpathList
  {
    get
    {
      if (this.libpathList == null)
      {
        if (this.libpath == null)
          this.libpathList = new string[1]
          {
            typeof (object).Module.Assembly.Location
          };
        else
          this.libpathList = this.libpath.Split(Path.PathSeparator);
      }
      return this.libpathList;
    }
  }

  internal string FindAssembly(string name)
  {
    string path1 = name;
    if (Path.GetFileName(name) == name)
    {
      if (File.Exists(name))
      {
        path1 = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar.ToString() + name;
      }
      else
      {
        string path2 = this.RuntimeDirectory + Path.DirectorySeparatorChar.ToString() + name;
        if (File.Exists(path2))
        {
          path1 = path2;
        }
        else
        {
          foreach (string libpath in this.LibpathList)
          {
            if (libpath.Length > 0)
            {
              string path3 = libpath + Path.DirectorySeparatorChar.ToString() + name;
              if (File.Exists(path3))
              {
                path1 = path3;
                break;
              }
            }
          }
        }
      }
    }
    return !File.Exists(path1) ? (string) null : path1;
  }

  protected override void ValidateRootMoniker(string rootMoniker)
  {
    if (this.genStartupClass)
      base.ValidateRootMoniker(rootMoniker);
    else if (rootMoniker == null || rootMoniker.Length == 0)
      throw new JSVsaException(JSVsaError.RootMonikerInvalid);
  }

  internal static bool CheckIdentifierForCLSCompliance(string name)
  {
    if (name[0] == '_')
      return false;
    for (int index = 0; index < name.Length; ++index)
    {
      if (name[index] == '$')
        return false;
    }
    return true;
  }

  internal void CheckTypeNameForCLSCompliance(string name, string fullname, Context context)
  {
    if (!this.isCLSCompliant)
      return;
    if (name[0] == '_')
      context.HandleError(JSError.NonCLSCompliantType);
    else if (!VsaEngine.CheckIdentifierForCLSCompliance(fullname))
    {
      context.HandleError(JSError.NonCLSCompliantType);
    }
    else
    {
      if (this.typenameTable == null)
        this.typenameTable = new Hashtable((IEqualityComparer) StringComparer.OrdinalIgnoreCase);
      if (this.typenameTable[(object) fullname] == null)
        this.typenameTable[(object) fullname] = (object) fullname;
      else
        context.HandleError(JSError.NonCLSCompliantType);
    }
  }
}
