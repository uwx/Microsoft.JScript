// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.StrictEquality
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using Microsoft.JScript.Vsa;
using System;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Performs comparison operations. This class belongs to the abstract syntax tree category.</summary>
public class StrictEquality : BinaryOp
{
    internal StrictEquality(Context context, AST operand1, AST operand2, JSToken operatorTok)
        : base(context, operand1, operand2, operatorTok)
    {
    }

    internal override object Evaluate()
    {
        bool flag = StrictEquality.JScriptStrictEquals(this.operand1.Evaluate(), this.operand2.Evaluate(),
            VsaEngine.executeForJSEE);
        return this.operatorTok == JSToken.StrictEqual ? (object)flag : (object)!flag;
    }

    /// <summary>Performs a comparison operation.</summary>
    /// <returns>The result of the operation.</returns>
    /// <param name="v1">The first operand.</param>
    /// <param name="v2">The second operand.</param>
    public static bool JScriptStrictEquals(object v1, object v2)
    {
        return StrictEquality.JScriptStrictEquals(v1, v2, false);
    }

    internal static bool JScriptStrictEquals(object v1, object v2, bool checkForDebuggerObjects)
    {
        IConvertible iconvertible1 = Convert.GetIConvertible(v1);
        IConvertible iconvertible2 = Convert.GetIConvertible(v2);
        TypeCode typeCode1 = Convert.GetTypeCode(v1, iconvertible1);
        TypeCode typeCode2 = Convert.GetTypeCode(v2, iconvertible2);
        return StrictEquality.JScriptStrictEquals(v1, v2, iconvertible1, iconvertible2, typeCode1, typeCode2,
            checkForDebuggerObjects);
    }

    internal static bool JScriptStrictEquals(
        object v1,
        object v2,
        IConvertible ic1,
        IConvertible ic2,
        TypeCode t1,
        TypeCode t2,
        bool checkForDebuggerObjects)
    {
        switch (t1)
        {
            case TypeCode.Empty:
                return t2 == TypeCode.Empty;
            case TypeCode.Object:
                if (v1 == v2)
                    return true;
                if (v1 is Missing || v1 is System.Reflection.Missing)
                    v1 = (object)null;
                if (v1 == v2)
                    return true;
                if (v2 is Missing || v2 is System.Reflection.Missing)
                    v2 = (object)null;
                return checkForDebuggerObjects && v1 is IDebuggerObject debuggerObject && v2 is IDebuggerObject o
                    ? debuggerObject.IsEqual(o)
                    : v1 == v2;
            case TypeCode.DBNull:
                return t2 == TypeCode.DBNull;
            case TypeCode.Boolean:
                return t2 == TypeCode.Boolean &&
                       ic1.ToBoolean((IFormatProvider)null) == ic2.ToBoolean((IFormatProvider)null);
            case TypeCode.Char:
                char ch = ic1.ToChar((IFormatProvider)null);
                switch (t2)
                {
                    case TypeCode.Char:
                        return (int)ch == (int)ic2.ToChar((IFormatProvider)null);
                    case TypeCode.SByte:
                    case TypeCode.Byte:
                    case TypeCode.Int16:
                    case TypeCode.UInt16:
                    case TypeCode.Int32:
                    case TypeCode.UInt32:
                    case TypeCode.Int64:
                        return (long)ch == ic2.ToInt64((IFormatProvider)null);
                    case TypeCode.UInt64:
                        return (long)ch == (long)ic2.ToUInt64((IFormatProvider)null);
                    case TypeCode.Single:
                    case TypeCode.Double:
                        return (double)ch == ic2.ToDouble((IFormatProvider)null);
                    case TypeCode.Decimal:
                        return (Decimal)(int)ch == ic2.ToDecimal((IFormatProvider)null);
                    case TypeCode.String:
                        string str1 = ic2.ToString((IFormatProvider)null);
                        return str1.Length == 1 && (int)ch == (int)str1[0];
                    default:
                        return false;
                }
            case TypeCode.SByte:
                sbyte num1 = ic1.ToSByte((IFormatProvider)null);
                switch (t2)
                {
                    case TypeCode.Char:
                        return (int)num1 == (int)ic2.ToChar((IFormatProvider)null);
                    case TypeCode.SByte:
                    case TypeCode.Byte:
                    case TypeCode.Int16:
                    case TypeCode.UInt16:
                    case TypeCode.Int32:
                    case TypeCode.UInt32:
                    case TypeCode.Int64:
                        return (long)num1 == ic2.ToInt64((IFormatProvider)null);
                    case TypeCode.UInt64:
                        return num1 >= (sbyte)0 && (long)num1 == (long)ic2.ToUInt64((IFormatProvider)null);
                    case TypeCode.Single:
                        return (double)num1 == (double)ic2.ToSingle((IFormatProvider)null);
                    case TypeCode.Double:
                        return (double)num1 == ic2.ToDouble((IFormatProvider)null);
                    case TypeCode.Decimal:
                        return (Decimal)num1 == ic2.ToDecimal((IFormatProvider)null);
                    default:
                        return false;
                }
            case TypeCode.Byte:
                byte num2 = ic1.ToByte((IFormatProvider)null);
                switch (t2)
                {
                    case TypeCode.Char:
                        return (int)num2 == (int)ic2.ToChar((IFormatProvider)null);
                    case TypeCode.SByte:
                    case TypeCode.Byte:
                    case TypeCode.Int16:
                    case TypeCode.UInt16:
                    case TypeCode.Int32:
                    case TypeCode.UInt32:
                    case TypeCode.Int64:
                        return (long)num2 == ic2.ToInt64((IFormatProvider)null);
                    case TypeCode.UInt64:
                        return (long)num2 == (long)ic2.ToUInt64((IFormatProvider)null);
                    case TypeCode.Single:
                        return (double)num2 == (double)ic2.ToSingle((IFormatProvider)null);
                    case TypeCode.Double:
                        return (double)num2 == ic2.ToDouble((IFormatProvider)null);
                    case TypeCode.Decimal:
                        return (Decimal)num2 == ic2.ToDecimal((IFormatProvider)null);
                    default:
                        return false;
                }
            case TypeCode.Int16:
                short int16 = ic1.ToInt16((IFormatProvider)null);
                switch (t2)
                {
                    case TypeCode.Char:
                        return (int)int16 == (int)ic2.ToChar((IFormatProvider)null);
                    case TypeCode.SByte:
                    case TypeCode.Byte:
                    case TypeCode.Int16:
                    case TypeCode.UInt16:
                    case TypeCode.Int32:
                    case TypeCode.UInt32:
                    case TypeCode.Int64:
                        return (long)int16 == ic2.ToInt64((IFormatProvider)null);
                    case TypeCode.UInt64:
                        return int16 >= (short)0 && (long)int16 == (long)ic2.ToUInt64((IFormatProvider)null);
                    case TypeCode.Single:
                        return (double)int16 == (double)ic2.ToSingle((IFormatProvider)null);
                    case TypeCode.Double:
                        return (double)int16 == ic2.ToDouble((IFormatProvider)null);
                    case TypeCode.Decimal:
                        return (Decimal)int16 == ic2.ToDecimal((IFormatProvider)null);
                    default:
                        return false;
                }
            case TypeCode.UInt16:
                ushort uint16 = ic1.ToUInt16((IFormatProvider)null);
                switch (t2)
                {
                    case TypeCode.Char:
                        return (int)uint16 == (int)ic2.ToChar((IFormatProvider)null);
                    case TypeCode.SByte:
                    case TypeCode.Byte:
                    case TypeCode.Int16:
                    case TypeCode.UInt16:
                    case TypeCode.Int32:
                    case TypeCode.UInt32:
                    case TypeCode.Int64:
                        return (long)uint16 == ic2.ToInt64((IFormatProvider)null);
                    case TypeCode.UInt64:
                        return (long)uint16 == (long)ic2.ToUInt64((IFormatProvider)null);
                    case TypeCode.Single:
                        return (double)uint16 == (double)ic2.ToSingle((IFormatProvider)null);
                    case TypeCode.Double:
                        return (double)uint16 == ic2.ToDouble((IFormatProvider)null);
                    case TypeCode.Decimal:
                        return (Decimal)uint16 == ic2.ToDecimal((IFormatProvider)null);
                    default:
                        return false;
                }
            case TypeCode.Int32:
                int int32 = ic1.ToInt32((IFormatProvider)null);
                switch (t2)
                {
                    case TypeCode.Char:
                        return int32 == (int)ic2.ToChar((IFormatProvider)null);
                    case TypeCode.SByte:
                    case TypeCode.Byte:
                    case TypeCode.Int16:
                    case TypeCode.UInt16:
                    case TypeCode.Int32:
                    case TypeCode.UInt32:
                    case TypeCode.Int64:
                        return (long)int32 == ic2.ToInt64((IFormatProvider)null);
                    case TypeCode.UInt64:
                        return int32 >= 0 && (long)int32 == (long)ic2.ToUInt64((IFormatProvider)null);
                    case TypeCode.Single:
                        return (double)int32 == (double)ic2.ToSingle((IFormatProvider)null);
                    case TypeCode.Double:
                        return (double)int32 == ic2.ToDouble((IFormatProvider)null);
                    case TypeCode.Decimal:
                        return (Decimal)int32 == ic2.ToDecimal((IFormatProvider)null);
                    default:
                        return false;
                }
            case TypeCode.UInt32:
                uint uint32 = ic1.ToUInt32((IFormatProvider)null);
                switch (t2)
                {
                    case TypeCode.Char:
                        return (int)uint32 == (int)ic2.ToChar((IFormatProvider)null);
                    case TypeCode.SByte:
                    case TypeCode.Byte:
                    case TypeCode.Int16:
                    case TypeCode.UInt16:
                    case TypeCode.Int32:
                    case TypeCode.UInt32:
                    case TypeCode.Int64:
                        return (long)uint32 == ic2.ToInt64((IFormatProvider)null);
                    case TypeCode.UInt64:
                        return (long)uint32 == (long)ic2.ToUInt64((IFormatProvider)null);
                    case TypeCode.Single:
                        return (double)uint32 == (double)ic2.ToSingle((IFormatProvider)null);
                    case TypeCode.Double:
                        return (double)uint32 == ic2.ToDouble((IFormatProvider)null);
                    case TypeCode.Decimal:
                        return (Decimal)uint32 == ic2.ToDecimal((IFormatProvider)null);
                    default:
                        return false;
                }
            case TypeCode.Int64:
                long int64_1 = ic1.ToInt64((IFormatProvider)null);
                switch (t2)
                {
                    case TypeCode.Char:
                        return int64_1 == (long)ic2.ToChar((IFormatProvider)null);
                    case TypeCode.SByte:
                    case TypeCode.Byte:
                    case TypeCode.Int16:
                    case TypeCode.UInt16:
                    case TypeCode.Int32:
                    case TypeCode.UInt32:
                    case TypeCode.Int64:
                        return int64_1 == ic2.ToInt64((IFormatProvider)null);
                    case TypeCode.UInt64:
                        return int64_1 >= 0L && int64_1 == (long)ic2.ToUInt64((IFormatProvider)null);
                    case TypeCode.Single:
                        return (double)int64_1 == (double)ic2.ToSingle((IFormatProvider)null);
                    case TypeCode.Double:
                        return (double)int64_1 == ic2.ToDouble((IFormatProvider)null);
                    case TypeCode.Decimal:
                        return (Decimal)int64_1 == ic2.ToDecimal((IFormatProvider)null);
                    default:
                        return false;
                }
            case TypeCode.UInt64:
                ulong uint64 = ic1.ToUInt64((IFormatProvider)null);
                switch (t2)
                {
                    case TypeCode.Char:
                        return (long)uint64 == (long)ic2.ToChar((IFormatProvider)null);
                    case TypeCode.SByte:
                    case TypeCode.Byte:
                    case TypeCode.Int16:
                    case TypeCode.UInt16:
                    case TypeCode.Int32:
                    case TypeCode.UInt32:
                    case TypeCode.Int64:
                        long int64_2 = ic2.ToInt64((IFormatProvider)null);
                        return int64_2 >= 0L && (long)uint64 == int64_2;
                    case TypeCode.UInt64:
                        return (long)uint64 == (long)ic2.ToUInt64((IFormatProvider)null);
                    case TypeCode.Single:
                        return (double)uint64 == (double)ic2.ToSingle((IFormatProvider)null);
                    case TypeCode.Double:
                        return (double)uint64 == ic2.ToDouble((IFormatProvider)null);
                    case TypeCode.Decimal:
                        return (Decimal)uint64 == ic2.ToDecimal((IFormatProvider)null);
                    default:
                        return false;
                }
            case TypeCode.Single:
                float single = ic1.ToSingle((IFormatProvider)null);
                switch (t2)
                {
                    case TypeCode.Char:
                        return (double)single == (double)ic2.ToChar((IFormatProvider)null);
                    case TypeCode.SByte:
                    case TypeCode.Byte:
                    case TypeCode.Int16:
                    case TypeCode.UInt16:
                    case TypeCode.Int32:
                    case TypeCode.UInt32:
                    case TypeCode.Int64:
                        return (double)single == (double)ic2.ToInt64((IFormatProvider)null);
                    case TypeCode.UInt64:
                        return (double)single == (double)ic2.ToUInt64((IFormatProvider)null);
                    case TypeCode.Single:
                        return (double)single == (double)ic2.ToSingle((IFormatProvider)null);
                    case TypeCode.Double:
                        return (double)single == (double)ic2.ToSingle((IFormatProvider)null);
                    case TypeCode.Decimal:
                        return (Decimal)single == ic2.ToDecimal((IFormatProvider)null);
                    default:
                        return false;
                }
            case TypeCode.Double:
                double num3 = ic1.ToDouble((IFormatProvider)null);
                switch (t2)
                {
                    case TypeCode.Char:
                        return num3 == (double)ic2.ToChar((IFormatProvider)null);
                    case TypeCode.SByte:
                    case TypeCode.Byte:
                    case TypeCode.Int16:
                    case TypeCode.UInt16:
                    case TypeCode.Int32:
                    case TypeCode.UInt32:
                    case TypeCode.Int64:
                        return num3 == (double)ic2.ToInt64((IFormatProvider)null);
                    case TypeCode.UInt64:
                        return num3 == (double)ic2.ToUInt64((IFormatProvider)null);
                    case TypeCode.Single:
                        return num3 == (double)ic2.ToSingle((IFormatProvider)null);
                    case TypeCode.Double:
                        return num3 == ic2.ToDouble((IFormatProvider)null);
                    case TypeCode.Decimal:
                        return (Decimal)num3 == ic2.ToDecimal((IFormatProvider)null);
                    default:
                        return false;
                }
            case TypeCode.Decimal:
                Decimal num4 = ic1.ToDecimal((IFormatProvider)null);
                switch (t2)
                {
                    case TypeCode.Char:
                        return num4 == (Decimal)(int)ic2.ToChar((IFormatProvider)null);
                    case TypeCode.SByte:
                    case TypeCode.Byte:
                    case TypeCode.Int16:
                    case TypeCode.UInt16:
                    case TypeCode.Int32:
                    case TypeCode.UInt32:
                    case TypeCode.Int64:
                        return num4 == (Decimal)ic2.ToInt64((IFormatProvider)null);
                    case TypeCode.UInt64:
                        return num4 == (Decimal)ic2.ToUInt64((IFormatProvider)null);
                    case TypeCode.Single:
                        return num4 == (Decimal)ic2.ToSingle((IFormatProvider)null);
                    case TypeCode.Double:
                        return num4 == (Decimal)ic2.ToDouble((IFormatProvider)null);
                    case TypeCode.Decimal:
                        return num4 == ic2.ToDecimal((IFormatProvider)null);
                    default:
                        return false;
                }
            case TypeCode.DateTime:
                return t2 == TypeCode.DateTime &&
                       ic1.ToDateTime((IFormatProvider)null) == ic2.ToDateTime((IFormatProvider)null);
            case TypeCode.String:
                switch (t2)
                {
                    case TypeCode.Char:
                        string str2 = ic1.ToString((IFormatProvider)null);
                        return str2.Length == 1 && (int)str2[0] == (int)ic2.ToChar((IFormatProvider)null);
                    case TypeCode.String:
                        return v1 == v2 || ic1.ToString((IFormatProvider)null)
                            .Equals(ic2.ToString((IFormatProvider)null));
                    default:
                        return false;
                }
            default:
                return false;
        }
    }

    internal override IReflect InferType(JSField inference_target) => (IReflect)Typeob.Boolean;

    internal override void TranslateToConditionalBranch(
        ILGenerator il,
        bool branchIfTrue,
        Label label,
        bool shortForm)
    {
        // Short-form branches corrupt the IL emitted by PersistedAssemblyBuilder on
        // .NET 10.0.x (ControlFlowBuilder desyncs on an operand at a blob boundary).
        shortForm = false;
        Type type1 = Convert.ToType(this.operand1.InferType((JSField)null));
        Type type2 = Convert.ToType(this.operand2.InferType((JSField)null));
        if (this.operand1 is ConstantWrapper && this.operand1.Evaluate() == null)
            type1 = Typeob.Empty;
        if (this.operand2 is ConstantWrapper && this.operand2.Evaluate() == null)
            type2 = Typeob.Empty;
        if (type1 != type2 && type1.IsPrimitive && type2.IsPrimitive)
        {
            if (type1 == Typeob.Single)
                type2 = type1;
            else if (type2 == Typeob.Single)
                type1 = type2;
            else if (Convert.IsPromotableTo((IReflect)type2, (IReflect)type1))
                type2 = type1;
            else if (Convert.IsPromotableTo((IReflect)type1, (IReflect)type2))
                type1 = type2;
        }

        bool flag = true;
        if (type1 == type2 && type1 != Typeob.Object)
        {
            Type rtype = type1;
            if (!type1.IsPrimitive)
                rtype = Typeob.Object;
            this.operand1.TranslateToIL(il, rtype);
            this.operand2.TranslateToIL(il, rtype);
            if (type1 == Typeob.String)
                il.Emit(OpCodes.Call, CompilerGlobals.stringEqualsMethod);
            else if (!type1.IsPrimitive)
                il.Emit(OpCodes.Callvirt, CompilerGlobals.equalsMethod);
            else
                flag = false;
        }
        else if (type1 == Typeob.Empty)
        {
            this.operand2.TranslateToIL(il, Typeob.Object);
            branchIfTrue = !branchIfTrue;
        }
        else if (type2 == Typeob.Empty)
        {
            this.operand1.TranslateToIL(il, Typeob.Object);
            branchIfTrue = !branchIfTrue;
        }
        else
        {
            this.operand1.TranslateToIL(il, Typeob.Object);
            this.operand2.TranslateToIL(il, Typeob.Object);
            il.Emit(OpCodes.Call, CompilerGlobals.jScriptStrictEqualsMethod);
        }

        if (branchIfTrue)
        {
            if (this.operatorTok == JSToken.StrictEqual)
            {
                if (flag)
                    il.Emit(shortForm ? OpCodes.Brtrue_S : OpCodes.Brtrue, label);
                else
                    il.Emit(shortForm ? OpCodes.Beq_S : OpCodes.Beq, label);
            }
            else if (flag)
                il.Emit(shortForm ? OpCodes.Brfalse_S : OpCodes.Brfalse, label);
            else
                il.Emit(shortForm ? OpCodes.Bne_Un_S : OpCodes.Bne_Un, label);
        }
        else if (this.operatorTok == JSToken.StrictEqual)
        {
            if (flag)
                il.Emit(shortForm ? OpCodes.Brfalse_S : OpCodes.Brfalse, label);
            else
                il.Emit(shortForm ? OpCodes.Bne_Un_S : OpCodes.Bne_Un, label);
        }
        else if (flag)
            il.Emit(shortForm ? OpCodes.Brtrue_S : OpCodes.Brtrue, label);
        else
            il.Emit(shortForm ? OpCodes.Beq_S : OpCodes.Beq, label);
    }

    internal override void TranslateToIL(ILGenerator il, Type rtype)
    {
        Label label1 = il.DefineLabel();
        Label label2 = il.DefineLabel();
        this.TranslateToConditionalBranch(il, true, label1, true);
        il.Emit(OpCodes.Ldc_I4_0);
        il.Emit(OpCodes.Br, label2);
        il.MarkLabel(label1);
        il.Emit(OpCodes.Ldc_I4_1);
        il.MarkLabel(label2);
        Convert.Emit((AST)this, il, Typeob.Boolean, rtype);
    }
}