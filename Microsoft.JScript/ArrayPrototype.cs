// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.ArrayPrototype
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using Microsoft.JScript.Vsa;
using System.Globalization;
using System.Text;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Provides static methods and properties for <see cref="T:Microsoft.JScript.ArrayObject" /> objects. All methods of this class are called by passing in a reference to the object that is requesting access to the method. This class belongs to the built-in object model category.</summary>
public class ArrayPrototype : ArrayObject
{
    internal static readonly ArrayPrototype ob = new ArrayPrototype(ObjectPrototype.ob);
    internal static ArrayConstructor _constructor;

    internal ArrayPrototype(ObjectPrototype parent)
        : base((ScriptObject)parent)
    {
        this.noExpando = true;
    }

    /// <summary>Concatenates objects or arrays to the end of the specified object or array.</summary>
    /// <returns>
    /// <paramref name="thisob" /> and <paramref name="args" /> concatenated.</returns>
    /// <param name="thisob">The object that this method is acting upon.</param>
    /// <param name="engine">A reference to the scripting engine.</param>
    /// <param name="args">The objects or arrays to append to <paramref name="thisob" />.</param>
    [JSFunction(
        JSFunctionAttributeEnum.HasThisObject | JSFunctionAttributeEnum.HasVarArgs | JSFunctionAttributeEnum.HasEngine,
        JSBuiltin.Array_concat)]
    public static ArrayObject concat(object thisob, VsaEngine engine, params object[] args)
    {
        ArrayObject arrayObject = engine.GetOriginalArrayConstructor().Construct();
        if (thisob is ArrayObject)
            arrayObject.Concat((ArrayObject)thisob);
        else
            arrayObject.Concat(thisob);
        for (int index = 0; index < args.Length; ++index)
        {
            object source = args[index];
            if (source is ArrayObject)
                arrayObject.Concat((ArrayObject)source);
            else
                arrayObject.Concat(source);
        }

        return arrayObject;
    }

    /// <summary>Gets a reference to a prototype-based object that is using this prototype.</summary>
    /// <returns>A reference to a prototype-based object.</returns>
    public static ArrayConstructor constructor => ArrayPrototype._constructor;

    /// <summary>Concatenates the elements of the specified array into a string. The elements are separated by the specified separator.</summary>
    /// <returns>A string that concatenates the elements of <paramref name="thisob" />.</returns>
    /// <param name="thisob">The object that this method is acting upon.</param>
    /// <param name="separator">The string to use to separate the elements. The default is a comma.</param>
    [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Array_join)]
    public static string join(object thisob, object separator)
    {
        return separator is Missing
            ? ArrayPrototype.Join(thisob, ",", false)
            : ArrayPrototype.Join(thisob, Convert.ToString(separator), false);
    }

    internal static string Join(object thisob, string separator, bool localize)
    {
        StringBuilder stringBuilder = new StringBuilder();
        uint uint32 = Convert.ToUint32(LateBinding.GetMemberValue(thisob, "length"));
        if (uint32 > (uint)int.MaxValue)
            throw new JScriptException(JSError.OutOfMemory);
        if ((long)uint32 > (long)stringBuilder.Capacity)
            stringBuilder.Capacity = (int)uint32;
        for (uint index = 0; index < uint32; ++index)
        {
            object valueAtIndex = LateBinding.GetValueAtIndex(thisob, (ulong)index);
            if (valueAtIndex != null && !(valueAtIndex is Missing))
            {
                if (localize)
                    stringBuilder.Append(Convert.ToLocaleString(valueAtIndex));
                else
                    stringBuilder.Append(Convert.ToString(valueAtIndex));
            }

            if (index < uint32 - 1U)
                stringBuilder.Append(separator);
        }

        return stringBuilder.ToString();
    }

    /// <summary>Removes the last element from the specified array and returns it.</summary>
    /// <returns>The last element of <paramref name="thisob" />.</returns>
    /// <param name="thisob">The object that this method is acting upon.</param>
    [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Array_pop)]
    public static object pop(object thisob)
    {
        uint uint32 = Convert.ToUint32(LateBinding.GetMemberValue(thisob, "length"));
        if (uint32 == 0U)
        {
            LateBinding.SetMemberValue(thisob, "length", (object)0);
            return (object)null;
        }

        object valueAtIndex = LateBinding.GetValueAtIndex(thisob, (ulong)(uint32 - 1U));
        LateBinding.DeleteValueAtIndex(thisob, (ulong)(uint32 - 1U));
        LateBinding.SetMemberValue(thisob, "length", (object)(uint)((int)uint32 - 1));
        return valueAtIndex;
    }

    /// <summary>Adds new elements to the end of the specified array.</summary>
    /// <returns>The new length of <paramref name="thisob" />.</returns>
    /// <param name="thisob">The object that this method is acting upon.</param>
    /// <param name="args">The elements to add to <paramref name="thisob" />.</param>
    [JSFunction(JSFunctionAttributeEnum.HasThisObject | JSFunctionAttributeEnum.HasVarArgs, JSBuiltin.Array_push)]
    public static long push(object thisob, params object[] args)
    {
        uint uint32 = Convert.ToUint32(LateBinding.GetMemberValue(thisob, "length"));
        for (uint index = 0; (long)index < (long)args.Length; ++index)
            LateBinding.SetValueAtIndex(thisob, (ulong)index + (ulong)uint32, args[(int)index]);
        long num = (long)uint32 + (long)args.Length;
        LateBinding.SetMemberValue(thisob, "length", (object)num);
        return num;
    }

    /// <summary>Reverses the elements of the specified array.</summary>
    /// <returns>
    /// <paramref name="thisob" />.</returns>
    /// <param name="thisob">The object that this method is acting upon.</param>
    [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Array_reverse)]
    public static object reverse(object thisob)
    {
        uint uint32 = Convert.ToUint32(LateBinding.GetMemberValue(thisob, "length"));
        uint num = uint32 / 2U;
        uint left = 0;
        uint right = uint32 - 1U;
        while (left < num)
        {
            LateBinding.SwapValues(thisob, left, right);
            ++left;
            --right;
        }

        return thisob;
    }

    internal override void SetMemberValue(string name, object value)
    {
        if (this.noExpando)
            throw new JScriptException(JSError.OLENoPropOrMethod);
        base.SetMemberValue(name, value);
    }

    internal override void SetValueAtIndex(uint index, object value)
    {
        if (this.noExpando)
            throw new JScriptException(JSError.OLENoPropOrMethod);
        base.SetValueAtIndex(index, value);
    }

    /// <summary>Removes the first element from the specified array and returns it. Shifts the remaining elements to the previous positions. </summary>
    /// <returns>The first element of <paramref name="thisob" />.</returns>
    /// <param name="thisob">The object that this method is acting upon.</param>
    [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Array_shift)]
    public static object shift(object thisob)
    {
        object obj = (object)null;
        if (thisob is ArrayObject)
            return ((ArrayObject)thisob).Shift();
        uint uint32 = Convert.ToUint32(LateBinding.GetMemberValue(thisob, "length"));
        if (uint32 == 0U)
        {
            LateBinding.SetMemberValue(thisob, "length", (object)0);
            return obj;
        }

        object valueAtIndex1 = LateBinding.GetValueAtIndex(thisob, 0UL);
        for (uint index = 1; index < uint32; ++index)
        {
            object valueAtIndex2 = LateBinding.GetValueAtIndex(thisob, (ulong)index);
            if (valueAtIndex2 is Missing)
                LateBinding.DeleteValueAtIndex(thisob, (ulong)(index - 1U));
            else
                LateBinding.SetValueAtIndex(thisob, (ulong)(index - 1U), valueAtIndex2);
        }

        LateBinding.DeleteValueAtIndex(thisob, (ulong)(uint32 - 1U));
        LateBinding.SetMemberValue(thisob, "length", (object)(uint)((int)uint32 - 1));
        return valueAtIndex1;
    }

    /// <summary>Creates a new array that contains the specified consecutive elements from the specified array.</summary>
    /// <returns>A new array that contains the consecutive elements between <paramref name="thisob" />[<paramref name="start" />] and <paramref name="thisob" />[<paramref name="end" />].</returns>
    /// <param name="thisob">The object that this method is acting upon.</param>
    /// <param name="engine">A reference to the scripting engine.</param>
    /// <param name="start">The first consecutive element to get.</param>
    /// <param name="end">The last consecutive element to get.</param>
    [JSFunction(JSFunctionAttributeEnum.HasThisObject | JSFunctionAttributeEnum.HasEngine, JSBuiltin.Array_slice)]
    public static ArrayObject slice(object thisob, VsaEngine engine, double start, object end)
    {
        ArrayObject arrayObject = engine.GetOriginalArrayConstructor().Construct();
        uint uint32 = Convert.ToUint32(LateBinding.GetMemberValue(thisob, "length"));
        long num1 = Runtime.DoubleToInt64(Convert.ToInteger(start));
        if (num1 < 0L)
        {
            num1 = (long)uint32 + num1;
            if (num1 < 0L)
                num1 = 0L;
        }
        else if (num1 > (long)uint32)
            num1 = (long)uint32;

        long num2 = (long)uint32;
        switch (end)
        {
            case null:
            case Missing _:
                if (num2 > num1)
                {
                    arrayObject.length = (object)(num2 - num1);
                    ulong index1 = (ulong)num1;
                    ulong index2 = 0;
                    while (index1 < (ulong)num2)
                    {
                        object valueAtIndex = LateBinding.GetValueAtIndex(thisob, index1);
                        if (!(valueAtIndex is Missing))
                            LateBinding.SetValueAtIndex((object)arrayObject, index2, valueAtIndex);
                        ++index1;
                        ++index2;
                    }
                }

                return arrayObject;
            default:
                num2 = Runtime.DoubleToInt64(Convert.ToInteger(end));
                if (num2 < 0L)
                {
                    num2 = (long)uint32 + num2;
                    if (num2 < 0L)
                    {
                        num2 = 0L;
                        goto case null;
                    }

                    goto case null;
                }

                if (num2 > (long)uint32)
                {
                    num2 = (long)uint32;
                    goto case null;
                }

                goto case null;
        }
    }

    /// <summary>Sorts the elements of the specified array by using the specified function to compare the elements.</summary>
    /// <returns>
    /// <paramref name="thisob" />.</returns>
    /// <param name="thisob">The object that this method is acting upon.</param>
    /// <param name="function">The function to use to compare elements.</param>
    [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Array_sort)]
    public static object sort(object thisob, object function)
    {
        ScriptFunction compareFn = (ScriptFunction)null;
        if (function is ScriptFunction)
            compareFn = (ScriptFunction)function;
        uint uint32 = Convert.ToUint32(LateBinding.GetMemberValue(thisob, "length"));
        if (thisob is ArrayObject)
            ((ArrayObject)thisob).Sort(compareFn);
        else if (uint32 <= (uint)int.MaxValue)
            new QuickSort(thisob, compareFn).SortObject(0L, (long)uint32);
        return thisob;
    }

    /// <summary>Adds and removes elements from an array at the specified position in the array. You can add elements, remove elements, or both.</summary>
    /// <returns>A new array that contains the elements removed from <paramref name="thisob" />, if any have been removed.</returns>
    /// <param name="thisob">The object that this method is acting upon.</param>
    /// <param name="engine">A reference to the scripting engine.</param>
    /// <param name="start">The position in the array to add and remove elements.</param>
    /// <param name="deleteCnt">The number of elements to remove, if any.</param>
    /// <param name="args">The elements to add, if any.</param>
    [JSFunction(
        JSFunctionAttributeEnum.HasThisObject | JSFunctionAttributeEnum.HasVarArgs | JSFunctionAttributeEnum.HasEngine,
        JSBuiltin.Array_splice)]
    public static ArrayObject splice(
        object thisob,
        VsaEngine engine,
        double start,
        double deleteCnt,
        params object[] args)
    {
        uint uint32 = Convert.ToUint32(LateBinding.GetMemberValue(thisob, "length"));
        long start1 = Runtime.DoubleToInt64(Convert.ToInteger(start));
        if (start1 < 0L)
        {
            start1 = (long)uint32 + start1;
            if (start1 < 0L)
                start1 = 0L;
        }
        else if (start1 > (long)uint32)
            start1 = (long)uint32;

        long deleteCount = Runtime.DoubleToInt64(Convert.ToInteger(deleteCnt));
        if (deleteCount < 0L)
            deleteCount = 0L;
        else if (deleteCount > (long)uint32 - start1)
            deleteCount = (long)uint32 - start1;
        long newLength = (long)uint32 + (long)args.Length - deleteCount;
        ArrayObject outArray = engine.GetOriginalArrayConstructor().Construct();
        outArray.length = (object)deleteCount;
        if (thisob is ArrayObject)
        {
            ((ArrayObject)thisob).Splice((uint)start1, (uint)deleteCount, args, outArray, uint32, (uint)newLength);
            return outArray;
        }

        for (ulong index = 0; index < (ulong)deleteCount; ++index)
            outArray.SetValueAtIndex((uint)index, LateBinding.GetValueAtIndex(thisob, index + (ulong)start1));
        long num = (long)uint32 - start1 - deleteCount;
        if (newLength < (long)uint32)
        {
            for (long index = 0; index < num; ++index)
                LateBinding.SetValueAtIndex(thisob, (ulong)(index + start1) + (ulong)args.Length,
                    LateBinding.GetValueAtIndex(thisob, (ulong)(index + start1 + deleteCount)));
            LateBinding.SetMemberValue(thisob, "length", (object)newLength);
        }
        else
        {
            LateBinding.SetMemberValue(thisob, "length", (object)newLength);
            for (long index = num - 1L; index >= 0L; --index)
                LateBinding.SetValueAtIndex(thisob, (ulong)(index + start1) + (ulong)args.Length,
                    LateBinding.GetValueAtIndex(thisob, (ulong)(index + start1 + deleteCount)));
        }

        int length = args == null ? 0 : args.Length;
        for (uint index = 0; (long)index < (long)length; ++index)
            LateBinding.SetValueAtIndex(thisob, (ulong)index + (ulong)start1, args[(int)index]);
        return outArray;
    }

    /// <summary>Creates a string representation of the specified object that concatenates the elements and separates them with a <see cref="P:System.Globalization.TextInfo.ListSeparator" /> and a space.</summary>
    /// <returns>A string that concatenates the elements of <paramref name="thisob" /> and separates them with a <see cref="P:System.Globalization.TextInfo.ListSeparator" /> and a space.</returns>
    /// <param name="thisob">The object that this method is acting upon.</param>
    [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Array_toLocaleString)]
    public static string toLocaleString(object thisob)
    {
        if (!(thisob is ArrayObject))
            throw new JScriptException(JSError.NeedArrayObject);
        StringBuilder stringBuilder = new StringBuilder(CultureInfo.CurrentCulture.TextInfo.ListSeparator);
        if (stringBuilder[stringBuilder.Length - 1] != ' ')
            stringBuilder.Append(' ');
        return ArrayPrototype.Join(thisob, stringBuilder.ToString(), true);
    }

    /// <summary>Creates a string representation of the specified object that concatenates the elements and separates them with a comma.</summary>
    /// <returns>A string that concatenates the elements of <paramref name="thisob" /> and separates them with a comma.</returns>
    /// <param name="thisob">The object that this method is acting upon.</param>
    [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Array_toString)]
    public static string toString(object thisob)
    {
        return thisob is ArrayObject
            ? ArrayPrototype.Join(thisob, ",", false)
            : throw new JScriptException(JSError.NeedArrayObject);
    }

    /// <summary>Adds new elements to the start of the specified array. Shifts the existing elements to the next positions.</summary>
    /// <returns>The new length of <paramref name="thisob" />.</returns>
    /// <param name="thisob">The object that this method is acting upon.</param>
    /// <param name="args">The elements to add to <paramref name="thisob" />.</param>
    [JSFunction(JSFunctionAttributeEnum.HasThisObject | JSFunctionAttributeEnum.HasVarArgs, JSBuiltin.Array_unshift)]
    public static object unshift(object thisob, params object[] args)
    {
        if (args == null || args.Length == 0)
            return thisob;
        if (thisob is ArrayObject)
            return (object)((ArrayObject)thisob).Unshift(args);
        uint uint32 = Convert.ToUint32(LateBinding.GetMemberValue(thisob, "length"));
        long num = (long)uint32 + (long)args.Length;
        LateBinding.SetMemberValue(thisob, "length", (object)num);
        for (long index = (long)(uint32 - 1U); index >= 0L; --index)
        {
            object valueAtIndex = LateBinding.GetValueAtIndex(thisob, (ulong)index);
            if (valueAtIndex is Missing)
                LateBinding.DeleteValueAtIndex(thisob, (ulong)index + (ulong)args.Length);
            else
                LateBinding.SetValueAtIndex(thisob, (ulong)index + (ulong)args.Length, valueAtIndex);
        }

        for (uint index = 0; (long)index < (long)args.Length; ++index)
            LateBinding.SetValueAtIndex(thisob, (ulong)index, args[(int)index]);
        return thisob;
    }
}