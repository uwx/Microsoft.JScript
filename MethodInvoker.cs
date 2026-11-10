// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.MethodInvoker
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Reflection;
using System.Reflection.Emit;
using System.Resources;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Represents the invoker for a particular method. Used by the <see cref="T:Microsoft.JScript.JSMethodInfo" /> class to invoke methods.</summary>
public abstract class MethodInvoker
{
  private static SimpleHashtable invokerFor = new SimpleHashtable(64U /*0x40*/);
  private static int count = 0;

  /// <summary>When overridden in a derived class, invokes the method.</summary>
  /// <returns>The result of invoking the method.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  /// <param name="parameters">The arguments to use for the method.</param>
  [DebuggerStepThrough]
  [DebuggerHidden]
  public abstract object Invoke(object thisob, object[] parameters);

  private static bool DoesCallerRequireFullTrust(MethodInfo method)
  {
    Assembly assembly = method.DeclaringType.Assembly;
    new FileIOPermission(PermissionState.None)
    {
      AllFiles = FileIOPermissionAccess.PathDiscovery
    }.Assert();
    byte[] publicKey = assembly.GetName().GetPublicKey();
    return publicKey != null && publicKey.Length != 0 && CustomAttribute.GetCustomAttributes(assembly, typeof (AllowPartiallyTrustedCallersAttribute), true).Length == 0;
  }

  internal static MethodInvoker GetInvokerFor(MethodInfo method)
  {
    if (method.DeclaringType == typeof (CodeAccessPermission) && (method.Name == "Deny" || method.Name == "Assert" || method.Name == "PermitOnly"))
      throw new JScriptException(JSError.CannotCallSecurityMethodLateBound);
    if (MethodInvoker.invokerFor[(object) method] is MethodInvoker invokerFor2)
      return invokerFor2;
    if (!MethodInvoker.SafeToCall(method))
      return (MethodInvoker) null;
    bool requiresDemand = MethodInvoker.DoesCallerRequireFullTrust(method);
    lock (MethodInvoker.invokerFor)
    {
      if (MethodInvoker.invokerFor[(object) method] is MethodInvoker invokerFor2Inner)
        return invokerFor2Inner;
      invokerFor2 = MethodInvoker.SpitAndInstantiateClassFor(method, requiresDemand);
      MethodInvoker.invokerFor[(object) method] = (object) invokerFor2;
    }
    return invokerFor2;
  }

  private static bool SafeToCall(MethodInfo meth)
  {
    Type declaringType = meth.DeclaringType;
    return declaringType != (Type) null && declaringType != typeof (Activator) && declaringType != typeof (AppDomain) && declaringType != typeof (IsolatedStorageFile) && declaringType != typeof (SecurityManager) && !typeof (Assembly).IsAssignableFrom(declaringType) && !typeof (MemberInfo).IsAssignableFrom(declaringType) && !typeof (ResourceManager).IsAssignableFrom(declaringType) && !typeof (Delegate).IsAssignableFrom(declaringType) && (declaringType.Attributes & TypeAttributes.HasSecurity) == TypeAttributes.NotPublic && (meth.Attributes & MethodAttributes.HasSecurity) == MethodAttributes.PrivateScope && (meth.Attributes & MethodAttributes.PinvokeImpl) == MethodAttributes.PrivateScope;
  }

  [ReflectionPermission(SecurityAction.Assert, Unrestricted = true)]
  private static MethodInvoker SpitAndInstantiateClassFor(MethodInfo method, bool requiresDemand)
  {
    TypeBuilder typeBuilder = Microsoft.JScript.Runtime.ThunkModuleBuilder.DefineType("invoker" + MethodInvoker.count++.ToString(), TypeAttributes.Public, typeof (MethodInvoker));
    MethodBuilder methodBuilder = typeBuilder.DefineMethod("Invoke", MethodAttributes.Public | MethodAttributes.Virtual, typeof (object), new Type[2]
    {
      typeof (object),
      typeof (object[])
    });
    if (requiresDemand)
      methodBuilder.AddDeclarativeSecurity(SecurityAction.Demand, (PermissionSet) new NamedPermissionSet("FullTrust"));
    methodBuilder.SetCustomAttribute(new CustomAttributeBuilder(Microsoft.JScript.Runtime.TypeRefs.debuggerStepThroughAttributeCtor, new object[0]));
    methodBuilder.SetCustomAttribute(new CustomAttributeBuilder(Microsoft.JScript.Runtime.TypeRefs.debuggerHiddenAttributeCtor, new object[0]));
    ILGenerator ilGenerator = methodBuilder.GetILGenerator();
    if (!method.DeclaringType.IsPublic)
      method = method.GetBaseDefinition();
    Type declaringType = method.DeclaringType;
    if (!method.IsStatic)
    {
      ilGenerator.Emit(OpCodes.Ldarg_1);
      if (declaringType.IsValueType)
      {
        Convert.EmitUnbox(ilGenerator, declaringType, Type.GetTypeCode(declaringType));
        Convert.EmitLdloca(ilGenerator, declaringType);
      }
      else
        ilGenerator.Emit(OpCodes.Castclass, declaringType);
    }
    ParameterInfo[] parameters = method.GetParameters();
    LocalBuilder[] localBuilderArray = (LocalBuilder[]) null;
    int i1 = 0;
    for (int length = parameters.Length; i1 < length; ++i1)
    {
      ilGenerator.Emit(OpCodes.Ldarg_2);
      ConstantWrapper.TranslateToILInt(ilGenerator, i1);
      Type parameterType = parameters[i1].ParameterType;
      if (parameterType.IsByRef)
      {
        Type elementType = parameterType.GetElementType();
        if (localBuilderArray == null)
          localBuilderArray = new LocalBuilder[length];
        localBuilderArray[i1] = ilGenerator.DeclareLocal(elementType);
        ilGenerator.Emit(OpCodes.Ldelem_Ref);
        if (elementType.IsValueType)
          Convert.EmitUnbox(ilGenerator, elementType, Type.GetTypeCode(elementType));
        ilGenerator.Emit(OpCodes.Stloc, localBuilderArray[i1]);
        ilGenerator.Emit(OpCodes.Ldloca, localBuilderArray[i1]);
      }
      else
      {
        ilGenerator.Emit(OpCodes.Ldelem_Ref);
        if (parameterType.IsValueType)
          Convert.EmitUnbox(ilGenerator, parameterType, Type.GetTypeCode(parameterType));
      }
    }
    if (!method.IsStatic && method.IsVirtual && !method.IsFinal && (!declaringType.IsSealed || !declaringType.IsValueType))
      ilGenerator.Emit(OpCodes.Callvirt, method);
    else
      ilGenerator.Emit(OpCodes.Call, method);
    Type returnType = method.ReturnType;
    if (returnType == typeof (void))
      ilGenerator.Emit(OpCodes.Ldnull);
    else if (returnType.IsValueType)
      ilGenerator.Emit(OpCodes.Box, returnType);
    if (localBuilderArray != null)
    {
      int i2 = 0;
      for (int length = parameters.Length; i2 < length; ++i2)
      {
        LocalBuilder local = localBuilderArray[i2];
        if (local != null)
        {
          ilGenerator.Emit(OpCodes.Ldarg_2);
          ConstantWrapper.TranslateToILInt(ilGenerator, i2);
          ilGenerator.Emit(OpCodes.Ldloc, local);
          Type elementType = parameters[i2].ParameterType.GetElementType();
          if (elementType.IsValueType)
            ilGenerator.Emit(OpCodes.Box, elementType);
          ilGenerator.Emit(OpCodes.Stelem_Ref);
        }
      }
    }
    ilGenerator.Emit(OpCodes.Ret);
    return (MethodInvoker) Activator.CreateInstance(typeBuilder.CreateType());
  }
}
