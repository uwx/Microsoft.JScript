// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.FieldAccessor
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Permissions;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Represents a field accessor.</summary>
public abstract class FieldAccessor
{
    private static SimpleHashtable accessorFor = new SimpleHashtable(32U /*0x20*/);
    private static int count = 0;

    /// <summary>When overridden in a derived class, gets the value of the field for the specified object.</summary>
    /// <returns>The value of the field for <paramref name="thisob" />.</returns>
    /// <param name="thisob">The object that this method is acting upon.</param>
    [DebuggerStepThrough]
    [DebuggerHidden]
    public abstract object GetValue(object thisob);

    /// <summary>When overridden in a derived class, sets the specified value of the field for the specified object.</summary>
    /// <param name="thisob">The object that this method is acting upon.</param>
    /// <param name="value">The value to set for the field.</param>
    [DebuggerStepThrough]
    [DebuggerHidden]
    public abstract void SetValue(object thisob, object value);

    internal static FieldAccessor GetAccessorFor(FieldInfo field)
    {
        if (FieldAccessor.accessorFor[(object)field] is FieldAccessor accessorFor2)
            return accessorFor2;
        lock (FieldAccessor.accessorFor)
        {
            if (FieldAccessor.accessorFor[(object)field] is FieldAccessor accessorFor2Inner)
                return accessorFor2Inner;
            accessorFor2 = FieldAccessor.SpitAndInstantiateClassFor(field);
            FieldAccessor.accessorFor[(object)field] = (object)accessorFor2;
        }

        return accessorFor2;
    }

    [ReflectionPermission(SecurityAction.Assert, Unrestricted = true)]
    private static FieldAccessor SpitAndInstantiateClassFor(FieldInfo field)
    {
        Type fieldType = field.FieldType;
        TypeBuilder typeBuilder = Runtime.ThunkModuleBuilder.DefineType("accessor" + FieldAccessor.count++.ToString(),
            TypeAttributes.Public, typeof(FieldAccessor));
        MethodBuilder methodBuilder1 = typeBuilder.DefineMethod("GetValue",
            MethodAttributes.Public | MethodAttributes.Virtual, typeof(object), new Type[1]
            {
                typeof(object)
            });
        methodBuilder1.SetCustomAttribute(new CustomAttributeBuilder(Runtime.TypeRefs.debuggerStepThroughAttributeCtor,
            new object[0]));
        methodBuilder1.SetCustomAttribute(new CustomAttributeBuilder(Runtime.TypeRefs.debuggerHiddenAttributeCtor,
            new object[0]));
        ILGenerator ilGenerator1 = methodBuilder1.GetILGenerator();
        if (field.IsLiteral)
            new ConstantWrapper(TypeReferences.GetConstantValue(field), (Context)null).TranslateToIL(ilGenerator1,
                fieldType);
        else if (field.IsStatic)
        {
            ilGenerator1.Emit(OpCodes.Ldsfld, field);
        }
        else
        {
            ilGenerator1.Emit(OpCodes.Ldarg_1);
            ilGenerator1.Emit(OpCodes.Ldfld, field);
        }

        if (fieldType.IsValueType)
            ilGenerator1.Emit(OpCodes.Box, fieldType);
        ilGenerator1.Emit(OpCodes.Ret);
        MethodBuilder methodBuilder2 = typeBuilder.DefineMethod("SetValue",
            MethodAttributes.Public | MethodAttributes.Virtual, typeof(void), new Type[2]
            {
                typeof(object),
                typeof(object)
            });
        methodBuilder2.SetCustomAttribute(new CustomAttributeBuilder(Runtime.TypeRefs.debuggerStepThroughAttributeCtor,
            new object[0]));
        methodBuilder2.SetCustomAttribute(new CustomAttributeBuilder(Runtime.TypeRefs.debuggerHiddenAttributeCtor,
            new object[0]));
        ILGenerator ilGenerator2 = methodBuilder2.GetILGenerator();
        if (!field.IsLiteral)
        {
            if (!field.IsStatic)
                ilGenerator2.Emit(OpCodes.Ldarg_1);
            ilGenerator2.Emit(OpCodes.Ldarg_2);
            if (fieldType.IsValueType)
                Convert.EmitUnbox(ilGenerator2, fieldType, Type.GetTypeCode(fieldType));
            if (field.IsStatic)
                ilGenerator2.Emit(OpCodes.Stsfld, field);
            else
                ilGenerator2.Emit(OpCodes.Stfld, field);
        }

        ilGenerator2.Emit(OpCodes.Ret);
        return (FieldAccessor)Activator.CreateInstance(typeBuilder.CreateType());
    }
}