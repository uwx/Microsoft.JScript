// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.Runtime
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Security;
using System.Security.Permissions;
using System.Threading;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Provides utility functions.</summary>
public static class Runtime
{
    private const Decimal DecimalTwoToThe64 = 18446744073709551616M;
    private static TypeReferences _typeRefs;
    private static ModuleBuilder _thunkModuleBuilder;

    /// <summary>Determines whether two specified values are equal.</summary>
    /// <returns>true if the values are equal; false if the values are not equal.</returns>
    /// <param name="v1">The first operand.</param>
    /// <param name="v2">The second operand.</param>
    public new static bool Equals(object v1, object v2) => new Equality(53).EvaluateEquality(v1, v2);

    /// <summary>Converts the specified <see cref="T:System.Double" /> value to an <see cref="T:System.Int64" />.</summary>
    /// <returns>
    /// <paramref name="val" /> converted to an <see cref="T:System.Int64" />.</returns>
    /// <param name="val">The value to convert.</param>
    public static long DoubleToInt64(double val)
    {
        if (double.IsNaN(val))
            return 0;
        if ((double)long.MinValue <= val && val <= (double)long.MaxValue)
            return (long)val;
        if (double.IsInfinity(val))
            return 0;
        double num = Math.IEEERemainder((double)Math.Sign(val) * Math.Floor(Math.Abs(val)), 1.8446744073709552E+19);
        return num == (double)long.MaxValue ? long.MinValue : (long)num;
    }

    /// <summary>Converts the specified <see cref="T:System.Decimal" /> value to an <see cref="T:System.Int64" />.</summary>
    /// <returns>
    /// <paramref name="val" /> converted to an <see cref="T:System.Int64" />.</returns>
    /// <param name="val">The value to convert.</param>
    public static long UncheckedDecimalToInt64(Decimal val)
    {
        val = Decimal.Truncate(val);
        if (val < -9223372036854775808M || 9223372036854775807M < val)
        {
            val = Decimal.Remainder(val, 18446744073709551616M);
            if (val < -9223372036854775808M)
                val += 18446744073709551616M;
            else if (val > 9223372036854775807M)
                val -= 18446744073709551616M;
        }

        return (long)val;
    }

    internal static TypeReferences TypeRefs
    {
        get => Runtime._typeRefs ?? (Runtime._typeRefs = new TypeReferences(typeof(Runtime).Module));
    }

    internal static ModuleBuilder ThunkModuleBuilder
    {
        get
        {
            ModuleBuilder thunkModuleBuilder = Runtime._thunkModuleBuilder;
            if ((Module)null == (Module)thunkModuleBuilder)
                thunkModuleBuilder = Runtime._thunkModuleBuilder = Runtime.CreateThunkModuleBuilder();
            return thunkModuleBuilder;
        }
    }

    [ReflectionPermission(SecurityAction.Assert, ReflectionEmit = true)]
    [FileIOPermission(SecurityAction.Assert, Unrestricted = true)]
    private static ModuleBuilder CreateThunkModuleBuilder()
    {
        ModuleBuilder thunkModuleBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName()
        {
            Name = "JScript Thunk Assembly"
        }, AssemblyBuilderAccess.Run).DefineDynamicModule("JScript Thunk Module");
        thunkModuleBuilder.SetCustomAttribute(
            new CustomAttributeBuilder(typeof(SecurityTransparentAttribute).GetConstructor(new Type[0]),
                new object[0]));
        return thunkModuleBuilder;
    }
}