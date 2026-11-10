// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.ArrayObject
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Represents an array object. This class belongs to the built-in object model category.</summary>
public class ArrayObject : JSObject
{
    internal const int MaxIndex = 100000;
    internal const int MinDenseSize = 128 /*0x80*/;
    internal uint len;
    internal object[] denseArray;
    internal uint denseArrayLength;

    internal ArrayObject(ScriptObject prototype)
        : base(prototype)
    {
        this.len = 0U;
        this.denseArray = (object[])null;
        this.denseArrayLength = 0U;
        this.noExpando = false;
    }

    internal ArrayObject(ScriptObject prototype, Type subType)
        : base(prototype, subType)
    {
        this.len = 0U;
        this.denseArray = (object[])null;
        this.denseArrayLength = 0U;
        this.noExpando = false;
    }

    internal static long Array_index_for(object index)
    {
        if (index is int num1)
            return (long)num1;
        IConvertible iconvertible = Convert.GetIConvertible(index);
        if ((uint)(Convert.GetTypeCode(index, iconvertible) - 4) <= 11U)
        {
            double num2 = iconvertible.ToDouble((IFormatProvider)null);
            long num3 = (long)num2;
            if (num3 >= 0L && (double)num3 == num2)
                return num3;
        }

        return -1;
    }

    internal static long Array_index_for(string name)
    {
        int length = name.Length;
        if (length <= 0)
            return -1;
        char ch1 = name[0];
        if (ch1 < '1' || ch1 > '9')
            return ch1 == '0' && length == 1 ? 0L : -1L;
        long num = (long)((int)ch1 - 48 /*0x30*/);
        for (int index = 1; index < length; ++index)
        {
            char ch2 = name[index];
            if (ch2 < '0' || ch2 > '9')
                return -1;
            num = num * 10L + (long)((int)ch2 - 48 /*0x30*/);
            if (num > (long)uint.MaxValue)
                return -1;
        }

        return num;
    }

    internal virtual void Concat(ArrayObject source)
    {
        uint len1 = source.len;
        if (len1 == 0U)
            return;
        uint len2 = this.len;
        this.SetLength((ulong)len2 + (ulong)len1);
        uint num1 = len1;
        if (!(source is ArrayWrapper) && len1 > source.denseArrayLength)
            num1 = source.denseArrayLength;
        uint num2 = len2;
        for (uint index = 0; index < num1; ++index)
            this.SetValueAtIndex(num2++, source.GetValueAtIndex(index));
        if ((int)num1 == (int)len1)
            return;
        IDictionaryEnumerator enumerator = source.NameTable.GetEnumerator();
        while (enumerator.MoveNext())
        {
            long num3 = ArrayObject.Array_index_for(enumerator.Key.ToString());
            if (num3 >= 0L)
                this.SetValueAtIndex(len2 + (uint)num3, ((FieldInfo)enumerator.Value).GetValue((object)null));
        }
    }

    internal virtual void Concat(object value)
    {
        if (value is Array array && array.Rank == 1)
        {
            this.Concat((ArrayObject)new ArrayWrapper((ScriptObject)ArrayPrototype.ob, array, true));
        }
        else
        {
            uint len = this.len;
            this.SetLength(1UL + (ulong)len);
            this.SetValueAtIndex(len, value);
        }
    }

    internal override bool DeleteMember(string name)
    {
        long index = ArrayObject.Array_index_for(name);
        return index >= 0L ? this.DeleteValueAtIndex((uint)index) : base.DeleteMember(name);
    }

    internal virtual bool DeleteValueAtIndex(uint index)
    {
        if (index >= this.denseArrayLength)
            return base.DeleteMember(index.ToString((IFormatProvider)CultureInfo.InvariantCulture));
        if (this.denseArray[(int)index] is Missing)
            return false;
        this.denseArray[(int)index] = (object)Missing.Value;
        return true;
    }

    private void DeleteRange(uint start, uint end)
    {
        uint num1 = this.denseArrayLength;
        if (num1 > end)
            num1 = end;
        for (; start < num1; ++start)
            this.denseArray[(int)start] = (object)Missing.Value;
        if ((int)num1 == (int)end)
            return;
        IDictionaryEnumerator enumerator = this.NameTable.GetEnumerator();
        ArrayList arrayList = new ArrayList(this.name_table.count);
        while (enumerator.MoveNext())
        {
            long num2 = ArrayObject.Array_index_for(enumerator.Key.ToString());
            if (num2 >= (long)start && num2 <= (long)end)
                arrayList.Add(enumerator.Key);
        }

        foreach (string name in arrayList)
            this.DeleteMember(name);
    }

    internal override string GetClassName() => "Array";

    internal override object GetDefaultValue(PreferredType preferred_type)
    {
        if (this.GetParent() is LenientArrayPrototype)
            return base.GetDefaultValue(preferred_type);
        switch (preferred_type)
        {
            case PreferredType.String:
                return !this.noExpando && this.NameTable[(object)"toString"] != null
                    ? base.GetDefaultValue(preferred_type)
                    : (object)ArrayPrototype.toString((object)this);
            case PreferredType.LocaleString:
                return !this.noExpando && this.NameTable[(object)"toLocaleString"] != null
                    ? base.GetDefaultValue(preferred_type)
                    : (object)ArrayPrototype.toLocaleString((object)this);
            default:
                if (!this.noExpando)
                {
                    object obj = this.NameTable[(object)"valueOf"];
                    if (obj == null && preferred_type == PreferredType.Either)
                        obj = this.NameTable[(object)"toString"];
                    if (obj != null)
                        return base.GetDefaultValue(preferred_type);
                }

                return (object)ArrayPrototype.toString((object)this);
        }
    }

    internal override void GetPropertyEnumerator(ArrayList enums, ArrayList objects)
    {
        if (this.field_table == null)
            this.field_table = new ArrayList();
        enums.Add((object)new ArrayEnumerator(this, (IEnumerator)new ListEnumerator(this.field_table)));
        objects.Add((object)this);
        if (this.parent == null)
            return;
        this.parent.GetPropertyEnumerator(enums, objects);
    }

    internal override object GetValueAtIndex(uint index)
    {
        if (index < this.denseArrayLength)
        {
            object dense = this.denseArray[(int)index];
            if (dense != Missing.Value)
                return dense;
        }

        return base.GetValueAtIndex(index);
    }

    [DebuggerStepThrough]
    [DebuggerHidden]
    internal override object GetMemberValue(string name)
    {
        long index = ArrayObject.Array_index_for(name);
        return index < 0L ? base.GetMemberValue(name) : this.GetValueAtIndex((uint)index);
    }

    /// <summary>Gets or sets the length of the array.</summary>
    /// <returns>The length of the array.</returns>
    public virtual object length
    {
        get => this.len < (uint)int.MaxValue ? (object)(int)this.len : (object)(double)this.len;
        set
        {
            IConvertible iconvertible = Convert.GetIConvertible(value);
            uint uint32 = Convert.ToUint32(value, iconvertible);
            if ((double)uint32 != Convert.ToNumber(value, iconvertible))
                throw new JScriptException(JSError.ArrayLengthAssignIncorrect);
            this.SetLength((ulong)uint32);
        }
    }

    private void Realloc(uint newLength)
    {
        uint denseArrayLength = this.denseArrayLength;
        uint length = denseArrayLength * 2U;
        if (length < newLength)
            length = newLength;
        object[] target = new object[(int)length];
        if (denseArrayLength > 0U)
            ArrayObject.Copy(this.denseArray, target, (int)denseArrayLength);
        for (int index = (int)denseArrayLength; (long)index < (long)length; ++index)
            target[index] = (object)Missing.Value;
        this.denseArray = target;
        this.denseArrayLength = length;
    }

    private void SetLength(ulong newLength)
    {
        uint len = this.len;
        if (newLength < (ulong)len)
        {
            this.DeleteRange((uint)newLength, len);
        }
        else
        {
            if (newLength > (ulong)uint.MaxValue)
                throw new JScriptException(JSError.ArrayLengthAssignIncorrect);
            if (newLength > (ulong)this.denseArrayLength && len <= this.denseArrayLength && newLength <= 100000UL &&
                (newLength <= 128UL /*0x80*/ || newLength <= (ulong)(len * 2U)))
                this.Realloc((uint)newLength);
        }

        this.len = (uint)newLength;
    }

    [DebuggerStepThrough]
    [DebuggerHidden]
    internal override void SetMemberValue(string name, object value)
    {
        if (name.Equals("length"))
        {
            this.length = value;
        }
        else
        {
            long index = ArrayObject.Array_index_for(name);
            if (index < 0L)
                base.SetMemberValue(name, value);
            else
                this.SetValueAtIndex((uint)index, value);
        }
    }

    internal override void SetValueAtIndex(uint index, object value)
    {
        if (index >= this.len && index < uint.MaxValue)
            this.SetLength((ulong)(index + 1U));
        if (index < this.denseArrayLength)
            this.denseArray[(int)index] = value;
        else
            base.SetMemberValue(index.ToString((IFormatProvider)CultureInfo.InvariantCulture), value);
    }

    internal virtual object Shift()
    {
        object obj1 = (object)null;
        uint len = this.len;
        if (len == 0U)
            return obj1;
        uint num = this.denseArrayLength >= len ? len : this.denseArrayLength;
        object obj2;
        if (num > 0U)
        {
            obj2 = this.denseArray[0];
            ArrayObject.Copy(this.denseArray, 1, this.denseArray, 0, (int)num - 1);
        }
        else
            obj2 = base.GetValueAtIndex(0U);

        for (uint index = num; index < len; ++index)
            this.SetValueAtIndex(index - 1U, this.GetValueAtIndex(index));
        this.SetValueAtIndex(len - 1U, (object)Missing.Value);
        this.SetLength((ulong)(len - 1U));
        return obj2 is Missing ? (object)null : obj2;
    }

    internal virtual void Sort(ScriptFunction compareFn)
    {
        QuickSort quickSort = new QuickSort((object)this, compareFn);
        uint len = this.len;
        if (len <= this.denseArrayLength)
            quickSort.SortArray(0, (int)len - 1);
        else
            quickSort.SortObject(0L, (long)(len - 1U));
    }

    internal virtual void Splice(
        uint start,
        uint deleteCount,
        object[] args,
        ArrayObject outArray,
        uint oldLength,
        uint newLength)
    {
        if (oldLength > this.denseArrayLength)
        {
            this.SpliceSlowly(start, deleteCount, args, outArray, oldLength, newLength);
        }
        else
        {
            if (newLength > oldLength)
            {
                this.SetLength((ulong)newLength);
                if (newLength > this.denseArrayLength)
                {
                    this.SpliceSlowly(start, deleteCount, args, outArray, oldLength, newLength);
                    return;
                }
            }

            if (deleteCount > oldLength)
                deleteCount = oldLength;
            if (deleteCount > 0U)
                ArrayObject.Copy(this.denseArray, (int)start, outArray.denseArray, 0, (int)deleteCount);
            if (oldLength > 0U)
                ArrayObject.Copy(this.denseArray, (int)start + (int)deleteCount, this.denseArray,
                    (int)start + args.Length, (int)oldLength - (int)start - (int)deleteCount);
            if (args != null)
            {
                int length = args.Length;
                if (length > 0)
                    ArrayObject.Copy(args, 0, this.denseArray, (int)start, length);
                if ((long)length >= (long)deleteCount)
                    return;
                this.SetLength((ulong)newLength);
            }
            else
            {
                if (deleteCount <= 0U)
                    return;
                this.SetLength((ulong)newLength);
            }
        }
    }

    /// <summary>Adds elements to and removes elements from an array, at the specified position in the array. You can add elements, remove elements, or both.</summary>
    /// <param name="start">The position in this array at which to add and remove elements.</param>
    /// <param name="deleteCount">The number of elements to remove, if any.</param>
    /// <param name="args">The elements to add, if any.</param>
    /// <param name="outArray">An array that contains the elements removed from this array, if any have been removed.</param>
    /// <param name="oldLength">The previous length of this array.</param>
    /// <param name="newLength">The new length of this array.</param>
    protected void SpliceSlowly(
        uint start,
        uint deleteCount,
        object[] args,
        ArrayObject outArray,
        uint oldLength,
        uint newLength)
    {
        for (uint index = 0; index < deleteCount; ++index)
            outArray.SetValueAtIndex(index, this.GetValueAtIndex(index + start));
        uint num = oldLength - start - deleteCount;
        if (newLength < oldLength)
        {
            for (uint index = 0; index < num; ++index)
                this.SetValueAtIndex((uint)((int)index + (int)start + args.Length),
                    this.GetValueAtIndex(index + start + deleteCount));
            this.SetLength((ulong)newLength);
        }
        else
        {
            if (newLength > oldLength)
                this.SetLength((ulong)newLength);
            for (uint index = num; index > 0U; --index)
                this.SetValueAtIndex((uint)((int)index + (int)start + args.Length - 1),
                    this.GetValueAtIndex((uint)((int)index + (int)start + (int)deleteCount - 1)));
        }

        int length = args == null ? 0 : args.Length;
        for (uint index = 0; (long)index < (long)length; ++index)
            this.SetValueAtIndex(index + start, args[(int)index]);
    }

    internal override void SwapValues(uint pi, uint qi)
    {
        if (pi > qi)
            this.SwapValues(qi, pi);
        else if (pi >= this.denseArrayLength)
        {
            base.SwapValues(pi, qi);
        }
        else
        {
            object dense = this.denseArray[(int)pi];
            this.denseArray[(int)pi] = this.GetValueAtIndex(qi);
            if (dense == Missing.Value)
                this.DeleteValueAtIndex(qi);
            else
                this.SetValueAtIndex(qi, dense);
        }
    }

    internal virtual object[] ToArray()
    {
        int len = (int)this.len;
        if (len == 0)
            return new object[0];
        if ((long)len == (long)this.denseArrayLength)
            return this.denseArray;
        if ((long)len < (long)this.denseArrayLength)
        {
            object[] target = new object[len];
            ArrayObject.Copy(this.denseArray, 0, target, 0, len);
            return target;
        }

        object[] target1 = new object[len];
        ArrayObject.Copy(this.denseArray, 0, target1, 0, (int)this.denseArrayLength);
        for (uint denseArrayLength = this.denseArrayLength; (long)denseArrayLength < (long)len; ++denseArrayLength)
            target1[(int)denseArrayLength] = this.GetValueAtIndex(denseArrayLength);
        return target1;
    }

    internal virtual Array ToNativeArray(Type elementType)
    {
        uint len = this.len;
        if (len > (uint)int.MaxValue)
            throw new JScriptException(JSError.OutOfMemory);
        if (elementType == (Type)null)
            elementType = typeof(object);
        uint num = this.denseArrayLength;
        if (num > len)
            num = len;
        Array instance = Array.CreateInstance(elementType, (int)len);
        for (int index = 0; (long)index < (long)num; ++index)
            instance.SetValue(Convert.CoerceT(this.denseArray[index], elementType), index);
        for (int index = (int)num; (long)index < (long)len; ++index)
            instance.SetValue(Convert.CoerceT(this.GetValueAtIndex((uint)index), elementType), index);
        return instance;
    }

    internal static void Copy(object[] source, object[] target, int n)
    {
        ArrayObject.Copy(source, 0, target, 0, n);
    }

    internal static void Copy(object[] source, int i, object[] target, int j, int n)
    {
        if (i < j)
        {
            for (int index = n - 1; index >= 0; --index)
                target[j + index] = source[i + index];
        }
        else
        {
            for (int index = 0; index < n; ++index)
                target[j + index] = source[i + index];
        }
    }

    internal virtual ArrayObject Unshift(object[] args)
    {
        uint len = this.len;
        int length = args.Length;
        ulong newLength = (ulong)len + (ulong)length;
        this.SetLength(newLength);
        if (newLength <= (ulong)this.denseArrayLength)
        {
            for (int index = (int)len - 1; index >= 0; --index)
                this.denseArray[index + length] = this.denseArray[index];
            ArrayObject.Copy(args, 0, this.denseArray, 0, args.Length);
        }
        else
        {
            for (long index = (long)(len - 1U); index >= 0L; --index)
                this.SetValueAtIndex((uint)((ulong)index + (ulong)length), this.GetValueAtIndex((uint)index));
            for (uint index = 0; (long)index < (long)length; ++index)
                this.SetValueAtIndex(index, args[(int)index]);
        }

        return this;
    }

    internal DebugArrayFieldEnumerator DebugGetEnumerator()
    {
        return new DebugArrayFieldEnumerator(new ScriptObjectPropertyEnumerator((ScriptObject)this), this);
    }

    internal object DebugGetValueAtIndex(int index) => this.GetValueAtIndex((uint)index);

    internal void DebugSetValueAtIndex(int index, object value)
    {
        this.SetValueAtIndex((uint)index, value);
    }
}