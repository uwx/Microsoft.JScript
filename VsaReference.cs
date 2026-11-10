// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.VsaReference
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using Microsoft.JScript.Vsa;
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

#nullable disable
namespace Microsoft.JScript;

internal class VsaReference : VsaItem, IJSVsaReferenceItem, IJSVsaItem
{
  private string assemblyName;
  private Assembly assembly;
  private bool loadFailed;

  internal VsaReference(VsaEngine engine, string itemName)
    : base(engine, itemName, JSVsaItemType.Reference, JSVsaItemFlag.None)
  {
    this.assemblyName = itemName;
    this.assembly = (Assembly) null;
    this.loadFailed = false;
  }

  public string AssemblyName
  {
    get
    {
      if (this.engine == null)
        throw new JSVsaException(JSVsaError.EngineClosed);
      return this.assemblyName;
    }
    set
    {
      if (this.engine == null)
        throw new JSVsaException(JSVsaError.EngineClosed);
      this.assemblyName = value;
      this.isDirty = true;
      this.engine.IsDirty = true;
    }
  }

  internal Assembly Assembly
  {
    get
    {
      if (this.engine == null)
        throw new JSVsaException(JSVsaError.EngineClosed);
      return this.assembly;
    }
  }

  internal Type GetType(string typeName)
  {
    if (this.assembly == (Assembly) null)
    {
      if (!this.loadFailed)
      {
        try
        {
          this.Load();
        }
        catch
        {
          this.loadFailed = true;
        }
      }
      if (this.assembly == (Assembly) null)
        return (Type) null;
    }
    Type target = this.assembly.GetType(typeName, false);
    if (target != (Type) null && (!target.IsPublic || CustomAttribute.IsDefined((MemberInfo) target, typeof (RequiredAttributeAttribute), true)))
      target = (Type) null;
    return target;
  }

  internal override void Compile() => this.Compile(true);

  internal bool Compile(bool throwOnFileNotFound)
  {
    try
    {
      string fileName = Path.GetFileName(this.assemblyName);
      string strA = fileName + ".dll";
      if (string.Compare(fileName, "mscorlib.dll", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(strA, "mscorlib.dll", StringComparison.OrdinalIgnoreCase) == 0)
        this.assembly = typeof (object).Assembly;
      if (string.Compare(fileName, "microsoft.jscript.dll", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(strA, "microsoft.jscript.dll", StringComparison.OrdinalIgnoreCase) == 0)
        this.assembly = this.engine.JScriptModule.Assembly;
      else if (this.engine.ReferenceLoaderAPI != LoaderAPI.ReflectionOnlyLoadFrom && (string.Compare(fileName, "system.dll", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(strA, "system.dll", StringComparison.OrdinalIgnoreCase) == 0))
        this.assembly = typeof (Regex).Module.Assembly;
      if (this.assembly == (Assembly) null)
      {
        string assembly = this.engine.FindAssembly(this.assemblyName);
        if (assembly == null)
        {
          string str = this.assemblyName + ".dll";
          bool flag = false;
          foreach (object obj in (IEnumerable) this.engine.Items)
          {
            if (obj is VsaReference && string.Compare(((VsaReference) obj).AssemblyName, str, StringComparison.OrdinalIgnoreCase) == 0)
            {
              flag = true;
              break;
            }
          }
          if (!flag)
          {
            assembly = this.engine.FindAssembly(str);
            if (assembly != null)
              this.assemblyName = str;
          }
        }
        if (assembly == null)
        {
          if (throwOnFileNotFound)
            throw new JSVsaException(JSVsaError.AssemblyExpected, this.assemblyName, (Exception) new FileNotFoundException());
          return false;
        }
        switch (this.engine.ReferenceLoaderAPI)
        {
          case LoaderAPI.LoadFrom:
            this.assembly = Assembly.LoadFrom(assembly);
            break;
          case LoaderAPI.LoadFile:
            this.assembly = Assembly.LoadFile(assembly);
            break;
          case LoaderAPI.ReflectionOnlyLoadFrom:
            // Reflection-only loading is not supported on .NET Core.
            this.assembly = Assembly.LoadFrom(assembly);
            break;
        }
        this.CheckCompatibility();
      }
    }
    catch (JSVsaException ex)
    {
      throw;
    }
    catch (BadImageFormatException ex)
    {
      throw new JSVsaException(JSVsaError.AssemblyExpected, this.assemblyName, (Exception) ex);
    }
    catch (FileNotFoundException ex)
    {
      if (throwOnFileNotFound)
        throw new JSVsaException(JSVsaError.AssemblyExpected, this.assemblyName, (Exception) ex);
      return false;
    }
    catch (FileLoadException ex)
    {
      throw new JSVsaException(JSVsaError.AssemblyExpected, this.assemblyName, (Exception) ex);
    }
    catch (ArgumentException ex)
    {
      throw new JSVsaException(JSVsaError.AssemblyExpected, this.assemblyName, (Exception) ex);
    }
    catch (Exception ex)
    {
      throw new JSVsaException(JSVsaError.InternalCompilerError, ex.ToString(), ex);
    }
    if (!(this.assembly == (Assembly) null))
      return true;
    if (throwOnFileNotFound)
      throw new JSVsaException(JSVsaError.AssemblyExpected, this.assemblyName);
    return false;
  }

  private void Load()
  {
    try
    {
      this.assembly = string.Compare(this.assemblyName, "mscorlib", StringComparison.OrdinalIgnoreCase) != 0 ? (string.Compare(this.assemblyName, "Microsoft.JScript", StringComparison.OrdinalIgnoreCase) != 0 ? (string.Compare(this.assemblyName, "System", StringComparison.OrdinalIgnoreCase) != 0 ? Assembly.Load(this.assemblyName) : typeof (Regex).Module.Assembly) : typeof (VsaEngine).Module.Assembly) : typeof (object).Module.Assembly;
    }
    catch (BadImageFormatException ex)
    {
      throw new JSVsaException(JSVsaError.AssemblyExpected, this.assemblyName, (Exception) ex);
    }
    catch (FileNotFoundException ex)
    {
      throw new JSVsaException(JSVsaError.AssemblyExpected, this.assemblyName, (Exception) ex);
    }
    catch (ArgumentException ex)
    {
      throw new JSVsaException(JSVsaError.AssemblyExpected, this.assemblyName, (Exception) ex);
    }
    catch (Exception ex)
    {
      throw new JSVsaException(JSVsaError.InternalCompilerError, ex.ToString(), ex);
    }
    if (this.assembly == (Assembly) null)
      throw new JSVsaException(JSVsaError.AssemblyExpected, this.assemblyName);
  }

  private void CheckCompatibility()
  {
    PortableExecutableKinds peKind;
    ImageFileMachine machine;
    this.assembly.ManifestModule.GetPEKind(out peKind, out machine);
    if (machine == ImageFileMachine.I386 && PortableExecutableKinds.ILOnly == (peKind & (PortableExecutableKinds.ILOnly | PortableExecutableKinds.Required32Bit)))
      return;
    PortableExecutableKinds peKindFlags = this.engine.PEKindFlags;
    ImageFileMachine machineArchitecture = this.engine.PEMachineArchitecture;
    if (machineArchitecture == ImageFileMachine.I386 && PortableExecutableKinds.ILOnly == (peKindFlags & (PortableExecutableKinds.ILOnly | PortableExecutableKinds.Required32Bit)) || machine == machineArchitecture)
      return;
    this.engine.OnCompilerError(new JScriptException(JSError.IncompatibleAssemblyReference)
    {
      value = (object) this.assemblyName
    });
  }
}
