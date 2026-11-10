// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.CompilerGlobals
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using Microsoft.JScript.Vsa;
using System;
using System.CodeDom.Compiler;
using System.Configuration.Assemblies;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Policy;
using System.Threading;

#nullable disable
namespace Microsoft.JScript;

internal sealed class CompilerGlobals
{
  internal Stack BreakLabelStack = new Stack();
  internal Stack ContinueLabelStack = new Stack();
  internal bool InsideProtectedRegion;
  internal bool InsideFinally;
  internal int FinallyStackTop;
  internal ModuleBuilder module;
  internal AssemblyBuilder assemblyBuilder;
  internal TypeBuilder classwriter;
  internal TypeBuilder globalScopeClassWriter;
  internal SimpleHashtable documents = new SimpleHashtable(8U);
  internal SimpleHashtable usedNames = new SimpleHashtable(32U /*0x20*/);
  internal Evidence compilationEvidence;

  internal CompilerGlobals(
    VsaEngine engine,
    string assemName,
    string assemblyFileName,
    PEFileKinds PEFileKind,
    bool save,
    bool run,
    bool debugOn,
    bool isCLSCompliant,
    Version version,
    Globals globals)
  {
    string fileName = (string) null;
    string dir = (string) null;
    if (assemblyFileName != null)
    {
      try
      {
        dir = Path.GetDirectoryName(Path.GetFullPath(assemblyFileName));
      }
      catch (Exception ex)
      {
        throw new JSVsaException(JSVsaError.AssemblyNameInvalid, assemblyFileName, ex);
      }
      fileName = Path.GetFileName(assemblyFileName);
      if (assemName == null || string.Empty == assemName)
      {
        assemName = Path.GetFileName(assemblyFileName);
        if (Path.HasExtension(assemName))
          assemName = assemName.Substring(0, assemName.Length - Path.GetExtension(assemName).Length);
      }
    }
    if (assemName == null || assemName == string.Empty)
      assemName = "JScriptAssembly";
    if (fileName == null)
      fileName = PEFileKind != PEFileKinds.Dll ? "JScriptModule.exe" : "JScriptModule.dll";
    AssemblyName name = new AssemblyName();
    name.CodeBase = assemblyFileName;
    if (globals.assemblyCulture != null)
      name.CultureInfo = globals.assemblyCulture;
    name.Flags = AssemblyNameFlags.None;
    if ((globals.assemblyFlags & AssemblyFlags.PublicKey) != AssemblyFlags.SideBySideCompatible)
      name.Flags = AssemblyNameFlags.PublicKey;
    switch (globals.assemblyFlags & AssemblyFlags.CompatibilityMask)
    {
      case AssemblyFlags.NonSideBySideAppDomain:
        name.VersionCompatibility = AssemblyVersionCompatibility.SameDomain;
        break;
      case AssemblyFlags.NonSideBySideProcess:
        name.VersionCompatibility = AssemblyVersionCompatibility.SameProcess;
        break;
      case AssemblyFlags.NonSideBySideMachine:
        name.VersionCompatibility = AssemblyVersionCompatibility.SameMachine;
        break;
      default:
        name.VersionCompatibility = (AssemblyVersionCompatibility) 0;
        break;
    }
    name.HashAlgorithm = globals.assemblyHashAlgorithm;
    if (globals.assemblyKeyFileName != null)
    {
      try
      {
        using (FileStream keyPairFile = new FileStream(globals.assemblyKeyFileName, FileMode.Open, FileAccess.Read))
        {
          StrongNameKeyPair strongNameKeyPair = new StrongNameKeyPair(keyPairFile);
          if (globals.assemblyDelaySign)
          {
            if (keyPairFile.Length == 160L /*0xA0*/)
            {
              byte[] numArray = new byte[160 /*0xA0*/];
              keyPairFile.Seek(0L, SeekOrigin.Begin);
              keyPairFile.Read(numArray, 0, 160 /*0xA0*/);
              name.SetPublicKey(numArray);
            }
            else
              name.SetPublicKey(strongNameKeyPair.PublicKey);
          }
          else
          {
            byte[] publicKey = strongNameKeyPair.PublicKey;
            name.KeyPair = strongNameKeyPair;
          }
        }
      }
      catch
      {
        globals.assemblyKeyFileNameContext.HandleError(JSError.InvalidAssemblyKeyFile, globals.assemblyKeyFileName);
      }
    }
    else if (globals.assemblyKeyName != null)
    {
      try
      {
        StrongNameKeyPair strongNameKeyPair = new StrongNameKeyPair(globals.assemblyKeyName);
        byte[] publicKey = strongNameKeyPair.PublicKey;
        name.KeyPair = strongNameKeyPair;
      }
      catch
      {
        globals.assemblyKeyNameContext.HandleError(JSError.InvalidAssemblyKeyFile, globals.assemblyKeyName);
      }
    }
    name.Name = assemName;
    if (version != (Version) null)
      name.Version = version;
    else if (globals.assemblyVersion != (Version) null)
      name.Version = globals.assemblyVersion;
    this.assemblyBuilder = !save
      ? AssemblyBuilder.DefineDynamicAssembly(name, AssemblyBuilderAccess.Run)
      : (AssemblyBuilder) new PersistedAssemblyBuilder(name, typeof(object).Assembly);
    this.assemblyBuilder.SetEmitDebugInfo(debugOn);
    this.module = !save ? this.assemblyBuilder.DefineDynamicModule("JScript Module", debugOn) : this.assemblyBuilder.DefineDynamicModule("JScript Module", fileName, debugOn);
    if (isCLSCompliant)
      this.module.SetCustomAttribute(new CustomAttributeBuilder(CompilerGlobals.clsCompliantAttributeCtor, new object[1]
      {
        (object) isCLSCompliant
      }));
    if (debugOn)
      this.assemblyBuilder.SetCustomAttribute(new CustomAttributeBuilder(Typeob.DebuggableAttribute.GetConstructor(new Type[2]
      {
        Typeob.Boolean,
        Typeob.Boolean
      }), new object[2]
      {
        (object) ((globals.assemblyFlags & AssemblyFlags.EnableJITcompileTracking) != 0),
        (object) ((globals.assemblyFlags & AssemblyFlags.DisableJITcompileOptimizer) != 0)
      }));
    this.compilationEvidence = globals.engine.Evidence;
    this.classwriter = (TypeBuilder) null;
  }

  internal static MethodInfo constructArrayMethod => Globals.TypeRefs.constructArrayMethod;

  internal static MethodInfo isMissingMethod => Globals.TypeRefs.isMissingMethod;

  internal static ConstructorInfo bitwiseBinaryConstructor
  {
    get => Globals.TypeRefs.bitwiseBinaryConstructor;
  }

  internal static MethodInfo evaluateBitwiseBinaryMethod
  {
    get => Globals.TypeRefs.evaluateBitwiseBinaryMethod;
  }

  internal static ConstructorInfo breakOutOfFinallyConstructor
  {
    get => Globals.TypeRefs.breakOutOfFinallyConstructor;
  }

  internal static ConstructorInfo closureConstructor => Globals.TypeRefs.closureConstructor;

  internal static ConstructorInfo continueOutOfFinallyConstructor
  {
    get => Globals.TypeRefs.continueOutOfFinallyConstructor;
  }

  internal static MethodInfo checkIfDoubleIsIntegerMethod
  {
    get => Globals.TypeRefs.checkIfDoubleIsIntegerMethod;
  }

  internal static MethodInfo checkIfSingleIsIntegerMethod
  {
    get => Globals.TypeRefs.checkIfSingleIsIntegerMethod;
  }

  internal static MethodInfo coerce2Method => Globals.TypeRefs.coerce2Method;

  internal static MethodInfo coerceTMethod => Globals.TypeRefs.coerceTMethod;

  internal static MethodInfo throwTypeMismatch => Globals.TypeRefs.throwTypeMismatch;

  internal static MethodInfo doubleToBooleanMethod => Globals.TypeRefs.doubleToBooleanMethod;

  internal static MethodInfo toBooleanMethod => Globals.TypeRefs.toBooleanMethod;

  internal static MethodInfo toForInObjectMethod => Globals.TypeRefs.toForInObjectMethod;

  internal static MethodInfo toInt32Method => Globals.TypeRefs.toInt32Method;

  internal static MethodInfo toNativeArrayMethod => Globals.TypeRefs.toNativeArrayMethod;

  internal static MethodInfo toNumberMethod => Globals.TypeRefs.toNumberMethod;

  internal static MethodInfo toObjectMethod => Globals.TypeRefs.toObjectMethod;

  internal static MethodInfo toObject2Method => Globals.TypeRefs.toObject2Method;

  internal static MethodInfo doubleToStringMethod => Globals.TypeRefs.doubleToStringMethod;

  internal static MethodInfo toStringMethod => Globals.TypeRefs.toStringMethod;

  internal static FieldInfo undefinedField => Globals.TypeRefs.undefinedField;

  internal static ConstructorInfo equalityConstructor => Globals.TypeRefs.equalityConstructor;

  internal static MethodInfo evaluateEqualityMethod => Globals.TypeRefs.evaluateEqualityMethod;

  internal static MethodInfo jScriptEqualsMethod => Globals.TypeRefs.jScriptEqualsMethod;

  internal static MethodInfo jScriptEvaluateMethod1 => Globals.TypeRefs.jScriptEvaluateMethod1;

  internal static MethodInfo jScriptEvaluateMethod2 => Globals.TypeRefs.jScriptEvaluateMethod2;

  internal static MethodInfo jScriptGetEnumeratorMethod
  {
    get => Globals.TypeRefs.jScriptGetEnumeratorMethod;
  }

  internal static MethodInfo jScriptFunctionDeclarationMethod
  {
    get => Globals.TypeRefs.jScriptFunctionDeclarationMethod;
  }

  internal static MethodInfo jScriptFunctionExpressionMethod
  {
    get => Globals.TypeRefs.jScriptFunctionExpressionMethod;
  }

  internal static FieldInfo contextEngineField => Globals.TypeRefs.contextEngineField;

  internal static MethodInfo fastConstructArrayLiteralMethod
  {
    get => Globals.TypeRefs.fastConstructArrayLiteralMethod;
  }

  internal static ConstructorInfo globalScopeConstructor => Globals.TypeRefs.globalScopeConstructor;

  internal static MethodInfo getDefaultThisObjectMethod
  {
    get => Globals.TypeRefs.getDefaultThisObjectMethod;
  }

  internal static MethodInfo getFieldMethod => Globals.TypeRefs.getFieldMethod;

  internal static MethodInfo getGlobalScopeMethod => Globals.TypeRefs.getGlobalScopeMethod;

  internal static MethodInfo getMemberValueMethod => Globals.TypeRefs.getMemberValueMethod;

  internal static MethodInfo jScriptImportMethod => Globals.TypeRefs.jScriptImportMethod;

  internal static MethodInfo jScriptInMethod => Globals.TypeRefs.jScriptInMethod;

  internal static MethodInfo getEngineMethod => Globals.TypeRefs.getEngineMethod;

  internal static MethodInfo setEngineMethod => Globals.TypeRefs.setEngineMethod;

  internal static MethodInfo jScriptInstanceofMethod => Globals.TypeRefs.jScriptInstanceofMethod;

  internal static ConstructorInfo scriptExceptionConstructor
  {
    get => Globals.TypeRefs.scriptExceptionConstructor;
  }

  internal static ConstructorInfo jsFunctionAttributeConstructor
  {
    get => Globals.TypeRefs.jsFunctionAttributeConstructor;
  }

  internal static ConstructorInfo jsLocalFieldConstructor
  {
    get => Globals.TypeRefs.jsLocalFieldConstructor;
  }

  internal static MethodInfo setMemberValue2Method => Globals.TypeRefs.setMemberValue2Method;

  internal static ConstructorInfo lateBindingConstructor2
  {
    get => Globals.TypeRefs.lateBindingConstructor2;
  }

  internal static ConstructorInfo lateBindingConstructor => Globals.TypeRefs.lateBindingConstructor;

  internal static FieldInfo objectField => Globals.TypeRefs.objectField;

  internal static MethodInfo callMethod => Globals.TypeRefs.callMethod;

  internal static MethodInfo callValueMethod => Globals.TypeRefs.callValueMethod;

  internal static MethodInfo callValue2Method => Globals.TypeRefs.callValue2Method;

  internal static MethodInfo deleteMethod => Globals.TypeRefs.deleteMethod;

  internal static MethodInfo deleteMemberMethod => Globals.TypeRefs.deleteMemberMethod;

  internal static MethodInfo getNonMissingValueMethod => Globals.TypeRefs.getNonMissingValueMethod;

  internal static MethodInfo getValue2Method => Globals.TypeRefs.getValue2Method;

  internal static MethodInfo setIndexedPropertyValueStaticMethod
  {
    get => Globals.TypeRefs.setIndexedPropertyValueStaticMethod;
  }

  internal static MethodInfo setValueMethod => Globals.TypeRefs.setValueMethod;

  internal static FieldInfo missingField => Globals.TypeRefs.missingField;

  internal static MethodInfo getNamespaceMethod => Globals.TypeRefs.getNamespaceMethod;

  internal static ConstructorInfo numericBinaryConstructor
  {
    get => Globals.TypeRefs.numericBinaryConstructor;
  }

  internal static MethodInfo numericbinaryDoOpMethod => Globals.TypeRefs.numericbinaryDoOpMethod;

  internal static MethodInfo evaluateNumericBinaryMethod
  {
    get => Globals.TypeRefs.evaluateNumericBinaryMethod;
  }

  internal static ConstructorInfo numericUnaryConstructor
  {
    get => Globals.TypeRefs.numericUnaryConstructor;
  }

  internal static MethodInfo evaluateUnaryMethod => Globals.TypeRefs.evaluateUnaryMethod;

  internal static MethodInfo constructObjectMethod => Globals.TypeRefs.constructObjectMethod;

  internal static MethodInfo jScriptPackageMethod => Globals.TypeRefs.jScriptPackageMethod;

  internal static ConstructorInfo plusConstructor => Globals.TypeRefs.plusConstructor;

  internal static MethodInfo plusDoOpMethod => Globals.TypeRefs.plusDoOpMethod;

  internal static MethodInfo evaluatePlusMethod => Globals.TypeRefs.evaluatePlusMethod;

  internal static ConstructorInfo postOrPrefixConstructor
  {
    get => Globals.TypeRefs.postOrPrefixConstructor;
  }

  internal static MethodInfo evaluatePostOrPrefixOperatorMethod
  {
    get => Globals.TypeRefs.evaluatePostOrPrefixOperatorMethod;
  }

  internal static ConstructorInfo referenceAttributeConstructor
  {
    get => Globals.TypeRefs.referenceAttributeConstructor;
  }

  internal static MethodInfo regExpConstructMethod => Globals.TypeRefs.regExpConstructMethod;

  internal static ConstructorInfo relationalConstructor => Globals.TypeRefs.relationalConstructor;

  internal static MethodInfo evaluateRelationalMethod => Globals.TypeRefs.evaluateRelationalMethod;

  internal static MethodInfo jScriptCompareMethod => Globals.TypeRefs.jScriptCompareMethod;

  internal static ConstructorInfo returnOutOfFinallyConstructor
  {
    get => Globals.TypeRefs.returnOutOfFinallyConstructor;
  }

  internal static MethodInfo doubleToInt64 => Globals.TypeRefs.doubleToInt64;

  internal static MethodInfo uncheckedDecimalToInt64Method
  {
    get => Globals.TypeRefs.uncheckedDecimalToInt64Method;
  }

  internal static FieldInfo engineField => Globals.TypeRefs.engineField;

  internal static MethodInfo getParentMethod => Globals.TypeRefs.getParentMethod;

  internal static MethodInfo writeMethod => Globals.TypeRefs.writeMethod;

  internal static MethodInfo writeLineMethod => Globals.TypeRefs.writeLineMethod;

  internal static ConstructorInfo hashtableCtor => Globals.TypeRefs.hashtableCtor;

  internal static MethodInfo hashtableGetItem => Globals.TypeRefs.hashtableGetItem;

  internal static MethodInfo hashTableGetEnumerator => Globals.TypeRefs.hashTableGetEnumerator;

  internal static MethodInfo hashtableRemove => Globals.TypeRefs.hashtableRemove;

  internal static MethodInfo hashtableSetItem => Globals.TypeRefs.hashtableSetItem;

  internal static FieldInfo closureInstanceField => Globals.TypeRefs.closureInstanceField;

  internal static FieldInfo localVarsField => Globals.TypeRefs.localVarsField;

  internal static MethodInfo pushStackFrameForMethod => Globals.TypeRefs.pushStackFrameForMethod;

  internal static MethodInfo pushStackFrameForStaticMethod
  {
    get => Globals.TypeRefs.pushStackFrameForStaticMethod;
  }

  internal static MethodInfo jScriptStrictEqualsMethod
  {
    get => Globals.TypeRefs.jScriptStrictEqualsMethod;
  }

  internal static MethodInfo jScriptThrowMethod => Globals.TypeRefs.jScriptThrowMethod;

  internal static MethodInfo jScriptExceptionValueMethod
  {
    get => Globals.TypeRefs.jScriptExceptionValueMethod;
  }

  internal static MethodInfo jScriptTypeofMethod => Globals.TypeRefs.jScriptTypeofMethod;

  internal static ConstructorInfo vsaEngineConstructor => Globals.TypeRefs.vsaEngineConstructor;

  internal static MethodInfo createVsaEngine => Globals.TypeRefs.createVsaEngine;

  internal static MethodInfo createVsaEngineWithType => Globals.TypeRefs.createVsaEngineWithType;

  internal static MethodInfo getOriginalArrayConstructorMethod
  {
    get => Globals.TypeRefs.getOriginalArrayConstructorMethod;
  }

  internal static MethodInfo getOriginalObjectConstructorMethod
  {
    get => Globals.TypeRefs.getOriginalObjectConstructorMethod;
  }

  internal static MethodInfo getOriginalRegExpConstructorMethod
  {
    get => Globals.TypeRefs.getOriginalRegExpConstructorMethod;
  }

  internal static MethodInfo popScriptObjectMethod => Globals.TypeRefs.popScriptObjectMethod;

  internal static MethodInfo pushScriptObjectMethod => Globals.TypeRefs.pushScriptObjectMethod;

  internal static MethodInfo scriptObjectStackTopMethod
  {
    get => Globals.TypeRefs.scriptObjectStackTopMethod;
  }

  internal static MethodInfo getLenientGlobalObjectMethod
  {
    get => Globals.TypeRefs.getLenientGlobalObjectMethod;
  }

  internal static MethodInfo jScriptWithMethod => Globals.TypeRefs.jScriptWithMethod;

  internal static ConstructorInfo clsCompliantAttributeCtor
  {
    get => Globals.TypeRefs.clsCompliantAttributeCtor;
  }

  internal static MethodInfo getEnumeratorMethod => Globals.TypeRefs.getEnumeratorMethod;

  internal static MethodInfo moveNextMethod => Globals.TypeRefs.moveNextMethod;

  internal static MethodInfo getCurrentMethod => Globals.TypeRefs.getCurrentMethod;

  internal static ConstructorInfo contextStaticAttributeCtor
  {
    get => Globals.TypeRefs.contextStaticAttributeCtor;
  }

  internal static MethodInfo changeTypeMethod => Globals.TypeRefs.changeTypeMethod;

  internal static MethodInfo convertCharToStringMethod
  {
    get => Globals.TypeRefs.convertCharToStringMethod;
  }

  internal static ConstructorInfo dateTimeConstructor => Globals.TypeRefs.dateTimeConstructor;

  internal static MethodInfo dateTimeToStringMethod => Globals.TypeRefs.dateTimeToStringMethod;

  internal static MethodInfo dateTimeToInt64Method => Globals.TypeRefs.dateTimeToInt64Method;

  internal static ConstructorInfo decimalConstructor => Globals.TypeRefs.decimalConstructor;

  internal static FieldInfo decimalZeroField => Globals.TypeRefs.decimalZeroField;

  internal static MethodInfo decimalCompare => Globals.TypeRefs.decimalCompare;

  internal static MethodInfo doubleToDecimalMethod => Globals.TypeRefs.doubleToDecimalMethod;

  internal static MethodInfo int32ToDecimalMethod => Globals.TypeRefs.int32ToDecimalMethod;

  internal static MethodInfo int64ToDecimalMethod => Globals.TypeRefs.int64ToDecimalMethod;

  internal static MethodInfo uint32ToDecimalMethod => Globals.TypeRefs.uint32ToDecimalMethod;

  internal static MethodInfo uint64ToDecimalMethod => Globals.TypeRefs.uint64ToDecimalMethod;

  internal static MethodInfo decimalToDoubleMethod => Globals.TypeRefs.decimalToDoubleMethod;

  internal static MethodInfo decimalToInt32Method => Globals.TypeRefs.decimalToInt32Method;

  internal static MethodInfo decimalToInt64Method => Globals.TypeRefs.decimalToInt64Method;

  internal static MethodInfo decimalToStringMethod => Globals.TypeRefs.decimalToStringMethod;

  internal static MethodInfo decimalToUInt32Method => Globals.TypeRefs.decimalToUInt32Method;

  internal static MethodInfo decimalToUInt64Method => Globals.TypeRefs.decimalToUInt64Method;

  internal static MethodInfo debugBreak => Globals.TypeRefs.debugBreak;

  internal static ConstructorInfo debuggerHiddenAttributeCtor
  {
    get => Globals.TypeRefs.debuggerHiddenAttributeCtor;
  }

  internal static ConstructorInfo debuggerStepThroughAttributeCtor
  {
    get => Globals.TypeRefs.debuggerStepThroughAttributeCtor;
  }

  internal static MethodInfo int32ToStringMethod => Globals.TypeRefs.int32ToStringMethod;

  internal static MethodInfo int64ToStringMethod => Globals.TypeRefs.int64ToStringMethod;

  internal static MethodInfo equalsMethod => Globals.TypeRefs.equalsMethod;

  internal static ConstructorInfo defaultMemberAttributeCtor
  {
    get => Globals.TypeRefs.defaultMemberAttributeCtor;
  }

  internal static MethodInfo getFieldValueMethod => Globals.TypeRefs.getFieldValueMethod;

  internal static MethodInfo setFieldValueMethod => Globals.TypeRefs.setFieldValueMethod;

  internal static FieldInfo systemReflectionMissingField
  {
    get => Globals.TypeRefs.systemReflectionMissingField;
  }

  internal static ConstructorInfo compilerGlobalScopeAttributeCtor
  {
    get => Globals.TypeRefs.compilerGlobalScopeAttributeCtor;
  }

  internal static MethodInfo stringConcatArrMethod => Globals.TypeRefs.stringConcatArrMethod;

  internal static MethodInfo stringConcat4Method => Globals.TypeRefs.stringConcat4Method;

  internal static MethodInfo stringConcat3Method => Globals.TypeRefs.stringConcat3Method;

  internal static MethodInfo stringConcat2Method => Globals.TypeRefs.stringConcat2Method;

  internal static MethodInfo stringEqualsMethod => Globals.TypeRefs.stringEqualsMethod;

  internal static MethodInfo stringLengthMethod => Globals.TypeRefs.stringLengthMethod;

  internal static MethodInfo getMethodMethod => Globals.TypeRefs.getMethodMethod;

  internal static MethodInfo getTypeMethod => Globals.TypeRefs.getTypeMethod;

  internal static MethodInfo getTypeFromHandleMethod => Globals.TypeRefs.getTypeFromHandleMethod;

  internal static MethodInfo uint32ToStringMethod => Globals.TypeRefs.uint32ToStringMethod;

  internal static MethodInfo uint64ToStringMethod => Globals.TypeRefs.uint64ToStringMethod;
}
