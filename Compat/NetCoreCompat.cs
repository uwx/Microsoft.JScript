// Compatibility helpers for the .NET Core port.  The .NET Framework version of
// Microsoft.JScript relies on a number of APIs that were never ported to .NET
// Core (CAS evidence, remoting call context and the COM expando interfaces).
// Saveable dynamic assemblies are provided by PersistedAssemblyBuilder (.NET 9+);
// the helpers below adapt the old AssemblyBuilder.Save shape to it.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Threading;

namespace System.Reflection.Emit
{
  /// <summary>
  /// State that <see cref="System.Reflection.Emit.PersistedAssemblyBuilder"/>
  /// cannot express through its public API until the assembly is serialized:
  /// the entry point, the resource payloads and the debug-information flag.
  /// </summary>
  internal sealed class PersistedAssemblyState
  {
    internal MethodInfo EntryPoint;
    internal System.CodeDom.Compiler.PEFileKinds FileKind = System.CodeDom.Compiler.PEFileKinds.Dll;
    internal bool EmitDebugInfo;
    internal readonly List<PersistedResource> Resources = new List<PersistedResource>();
  }

  internal sealed class PersistedResource
  {
    internal string Name;
    internal ResourceAttributes Attributes;
    internal readonly List<KeyValuePair<string, object>> Entries = new List<KeyValuePair<string, object>>();
  }

  internal static class ReflectionEmitCompat
  {
    private static readonly ConditionalWeakTable<AssemblyBuilder, PersistedAssemblyState> assemblyStates = new ConditionalWeakTable<AssemblyBuilder, PersistedAssemblyState>();
    private static readonly ConditionalWeakTable<ModuleBuilder, PersistedAssemblyState> moduleStates = new ConditionalWeakTable<ModuleBuilder, PersistedAssemblyState>();

    private static PersistedAssemblyState GetAssemblyState(AssemblyBuilder assembly)
    {
      return ReflectionEmitCompat.assemblyStates.GetOrCreateValue(assembly);
    }

    private static PersistedAssemblyState GetModuleState(ModuleBuilder module)
    {
      PersistedAssemblyState state;
      if (ReflectionEmitCompat.moduleStates.TryGetValue(module, out state))
        return state;
      AssemblyBuilder assembly = module.Assembly as AssemblyBuilder;
      state = assembly != null ? ReflectionEmitCompat.GetAssemblyState(assembly) : new PersistedAssemblyState();
      ReflectionEmitCompat.RegisterModule(module, state);
      return state;
    }

    private static void RegisterModule(ModuleBuilder module, PersistedAssemblyState state)
    {
      PersistedAssemblyState registered;
      if (!ReflectionEmitCompat.moduleStates.TryGetValue(module, out registered))
        ReflectionEmitCompat.moduleStates.Add(module, state);
    }

    internal static bool TryGetState(AssemblyBuilder assembly, out PersistedAssemblyState state)
    {
      return ReflectionEmitCompat.assemblyStates.TryGetValue(assembly, out state);
    }

    internal static void SetEmitDebugInfo(this AssemblyBuilder assembly, bool emitDebugInfo)
    {
      ReflectionEmitCompat.GetAssemblyState(assembly).EmitDebugInfo = emitDebugInfo;
    }

    internal static ModuleBuilder DefineDynamicModule(
      this AssemblyBuilder assembly,
      string name,
      bool emitSymbolInfo)
    {
      ModuleBuilder module = assembly.DefineDynamicModule(name);
      ReflectionEmitCompat.RegisterModule(module, ReflectionEmitCompat.GetAssemblyState(assembly));
      return module;
    }

    internal static ModuleBuilder DefineDynamicModule(
      this AssemblyBuilder assembly,
      string name,
      string fileName,
      bool emitSymbolInfo)
    {
      ModuleBuilder module = assembly.DefineDynamicModule(name);
      ReflectionEmitCompat.RegisterModule(module, ReflectionEmitCompat.GetAssemblyState(assembly));
      return module;
    }

    // On .NET 9+ these APIs exist on the emit types themselves, so the real
    // implementations are used.  The helpers stay for compile-time compatibility
    // with code paths that target the older surface.
    internal static void SetLocalSymInfo(this LocalBuilder local, string name)
    {
    }

    internal static void MarkSequencePoint(
      this ILGenerator il,
      ISymbolDocumentWriter document,
      int startLine,
      int startColumn,
      int endLine,
      int endColumn)
    {
    }

    internal static ISymbolDocumentWriter DefineDocument(
      this ModuleBuilder module,
      string url,
      Guid language,
      Guid vendor,
      Guid documentType)
    {
      return (ISymbolDocumentWriter) null;
    }

    internal static void SetUserEntryPoint(this ModuleBuilder module, MethodInfo entryPoint)
    {
    }

    internal static IResourceWriter DefineResource(
      this ModuleBuilder module,
      string name,
      string description,
      ResourceAttributes attribute)
    {
      PersistedResource resource = new PersistedResource();
      resource.Name = name;
      resource.Attributes = attribute;
      ReflectionEmitCompat.GetModuleState(module).Resources.Add(resource);
      return (IResourceWriter) new BufferedResourceWriter(resource);
    }

    internal static void SetEntryPoint(
      this AssemblyBuilder assembly,
      MethodInfo entryPoint,
      System.CodeDom.Compiler.PEFileKinds fileKind)
    {
      PersistedAssemblyState state = ReflectionEmitCompat.GetAssemblyState(assembly);
      state.EntryPoint = entryPoint;
      state.FileKind = fileKind;
    }

    // Win32 and version resources are native PE resources; the persisted emitter
    // has no equivalent, so they are dropped rather than failing the compile.
    internal static void DefineUnmanagedResource(this AssemblyBuilder assembly, string resourceFileName)
    {
    }

    internal static void DefineVersionInfoResource(this AssemblyBuilder assembly)
    {
    }

    internal static void AddResourceFile(
      this AssemblyBuilder assembly,
      string name,
      string fileName,
      ResourceAttributes attribute)
    {
    }

    internal static void AddDeclarativeSecurity(
      this MethodBuilder method,
      SecurityAction action,
      PermissionSet permissionSet)
    {
    }

    internal static void Save(
      this AssemblyBuilder assembly,
      string assemblyFileName,
      PortableExecutableKinds kinds,
      ImageFileMachine machine)
    {
      PersistedAssemblyBuilder persistedAssemblyBuilder = assembly as PersistedAssemblyBuilder;
      if (persistedAssemblyBuilder == null)
      {
        throw new PlatformNotSupportedException(
          "Saving emitted assemblies requires a PersistedAssemblyBuilder.");
      }
      byte[] debugInfo;
      byte[] peImage = persistedAssemblyBuilder.SaveToBytes(out debugInfo);
      File.WriteAllBytes(assemblyFileName, peImage);
      if (debugInfo != null)
        File.WriteAllBytes(Path.ChangeExtension(assemblyFileName, ".pdb"), debugInfo);
    }

    /// <summary>
    /// Serializes a persisted assembly (and its portable PDB when debug
    /// information was requested) into memory.
    /// </summary>
    internal static byte[] SaveToBytes(this PersistedAssemblyBuilder assembly, out byte[] debugInfo)
    {
      PersistedAssemblyState state;
      ReflectionEmitCompat.assemblyStates.TryGetValue((AssemblyBuilder) assembly, out state);

      MetadataBuilder pdbMetadata;
      MetadataBuilder metadata = assembly.GenerateMetadata(out BlobBuilder ilStream, out BlobBuilder mappedFieldData, out pdbMetadata);

      BlobBuilder managedResources = (BlobBuilder) null;
      if (state != null && state.Resources.Count > 0)
      {
        managedResources = new BlobBuilder();
        foreach (PersistedResource resource in state.Resources)
        {
          byte[] data = ReflectionEmitCompat.SerializeResource(resource);
          uint offset = (uint) managedResources.Count;
          managedResources.WriteInt32(data.Length);
          managedResources.WriteBytes(data);
          metadata.AddManifestResource(
            resource.Attributes == ResourceAttributes.Public ? ManifestResourceAttributes.Public : ManifestResourceAttributes.Private,
            metadata.GetOrAddString(resource.Name),
            default(EntityHandle),
            offset);
        }
      }

      MethodDefinitionHandle entryPoint = default(MethodDefinitionHandle);
      if (state != null && state.EntryPoint != null)
        entryPoint = MetadataTokens.MethodDefinitionHandle(state.EntryPoint.MetadataToken);

      debugInfo = (byte[]) null;
      DebugDirectoryBuilder debugDirectoryBuilder = (DebugDirectoryBuilder) null;
      if (state != null && state.EmitDebugInfo && pdbMetadata != null)
      {
        PortablePdbBuilder portablePdbBuilder = new PortablePdbBuilder(pdbMetadata, metadata.GetRowCounts(), entryPoint);
        BlobBuilder pdbBlob = new BlobBuilder();
        BlobContentId pdbContentId = portablePdbBuilder.Serialize(pdbBlob);
        debugInfo = pdbBlob.ToArray();
        debugDirectoryBuilder = new DebugDirectoryBuilder();
        debugDirectoryBuilder.AddCodeViewEntry(
          Path.GetFileNameWithoutExtension(assembly.GetName().Name) + ".pdb",
          pdbContentId,
          portablePdbBuilder.FormatVersion);
      }

      bool library = state == null || state.FileKind == System.CodeDom.Compiler.PEFileKinds.Dll;
      PEHeaderBuilder header = library ? PEHeaderBuilder.CreateLibraryHeader() : PEHeaderBuilder.CreateExecutableHeader();
      ManagedPEBuilder peBuilder = new ManagedPEBuilder(
        header: header,
        metadataRootBuilder: new MetadataRootBuilder(metadata),
        ilStream: ilStream,
        mappedFieldData: mappedFieldData,
        managedResources: managedResources,
        debugDirectoryBuilder: debugDirectoryBuilder,
        entryPoint: entryPoint);
      BlobBuilder peBlob = new BlobBuilder();
      peBuilder.Serialize(peBlob);
      return peBlob.ToArray();
    }

    private static byte[] SerializeResource(PersistedResource resource)
    {
      using (MemoryStream stream = new MemoryStream())
      {
        using (ResourceWriter writer = new ResourceWriter(stream))
        {
          foreach (KeyValuePair<string, object> entry in resource.Entries)
            writer.AddResource(entry.Key, entry.Value);
        }
        return stream.ToArray();
      }
    }

    private sealed class BufferedResourceWriter : IResourceWriter
    {
      private readonly PersistedResource resource;

      internal BufferedResourceWriter(PersistedResource resource)
      {
        this.resource = resource;
      }

      public void AddResource(string name, string value)
      {
        this.resource.Entries.Add(new KeyValuePair<string, object>(name, (object) value));
      }

      public void AddResource(string name, object value)
      {
        this.resource.Entries.Add(new KeyValuePair<string, object>(name, value));
      }

      public void AddResource(string name, byte[] value)
      {
        this.resource.Entries.Add(new KeyValuePair<string, object>(name, (object) value));
      }

      public void Close()
      {
      }

      public void Generate()
      {
      }

      public void Dispose()
      {
      }
    }
  }
}

namespace Microsoft.JScript
{
  /// <summary>
  /// Replacement for System.Runtime.Remoting.Messaging.CallContext, which is
  /// not available on .NET Core.  The script engine only uses the call context
  /// to flow an engine instance to code running in the same logical call.
  /// </summary>
  internal static class JScriptCallContext
  {
    private static readonly AsyncLocal<Hashtable> data = new AsyncLocal<Hashtable>();

    private static Hashtable Data
    {
      get
      {
        Hashtable hashtable = JScriptCallContext.data.Value;
        if (hashtable == null)
        {
          hashtable = new Hashtable();
          JScriptCallContext.data.Value = hashtable;
        }
        return hashtable;
      }
    }

    internal static void SetData(string name, object value)
    {
      if (value == null)
        JScriptCallContext.Data.Remove((object) name);
      else
        JScriptCallContext.Data[(object) name] = value;
    }

    internal static object GetData(string name)
    {
      return JScriptCallContext.data.Value?[(object) name];
    }
  }

  /// <summary>Replacement for the .NET Framework UCOMIPersistFile interface.</summary>
  [ComImport]
  [Guid("0000010b-0000-0000-C000-000000000046")]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  internal interface UCOMIPersistFile
  {
    void GetClassID(out Guid pClassID);

    [PreserveSig]
    int IsDirty();

    void Load([MarshalAs(UnmanagedType.LPWStr)] string pszFileName, uint dwMode);

    [PreserveSig]
    int Save([MarshalAs(UnmanagedType.LPWStr)] string pszFileName, [MarshalAs(UnmanagedType.Bool)] bool fRemember);

    void SaveCompleted([MarshalAs(UnmanagedType.LPWStr)] string pszFileName);

    void GetCurFile([MarshalAs(UnmanagedType.LPWStr)] out string ppszFileName);
  }

  /// <summary>Replacement for Marshal.GetActiveObject, which is Windows-only and absent from .NET Core.</summary>
  internal static class ActiveObject
  {
    [DllImport("ole32.dll", PreserveSig = false)]
    private static extern void CLSIDFromProgID(
      [MarshalAs(UnmanagedType.LPWStr)] string progId,
      out Guid clsid);

    [DllImport("oleaut32.dll", PreserveSig = false)]
    private static extern void GetActiveObject(
      ref Guid rclsid,
      IntPtr pvReserved,
      [MarshalAs(UnmanagedType.IUnknown)] out object ppunk);

    internal static object GetActiveObject(string progID)
    {
      Guid clsid;
      ActiveObject.CLSIDFromProgID(progID, out clsid);
      object obj;
      ActiveObject.GetActiveObject(ref clsid, IntPtr.Zero, out obj);
      return obj;
    }
  }
}
