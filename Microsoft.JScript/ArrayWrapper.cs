// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.ArrayWrapper
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Collections;
using System.Diagnostics;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Wraps an array to create a JScript <see cref="T:Microsoft.JScript.ArrayObject" />. This class wraps either a .NET Framework <see cref="T:System.Array" /> object or a JScript <see cref="T:Microsoft.JScript.ArrayObject" /> object. </summary>
public class ArrayWrapper : ArrayObject
{
    internal Array value;
    private bool implicitWrapper;

    internal ArrayWrapper(ScriptObject prototype, Array value, bool implicitWrapper)
        : base(prototype, typeof(ArrayWrapper))
    {
        this.value = value;
        this.implicitWrapper = implicitWrapper;
        if (value != null)
            this.len = value.Rank == 1 ? (uint)value.Length : throw new JScriptException(JSError.TypeMismatch);
        else
            this.len = 0U;
    }

    internal override void Concat(ArrayObject source)
    {
        throw new JScriptException(JSError.ActionNotSupported);
    }

    internal override void Concat(object value)
    {
        throw new JScriptException(JSError.ActionNotSupported);
    }

    internal override void GetPropertyEnumerator(ArrayList enums, ArrayList objects)
    {
        enums.Add(
            (object)new ArrayEnumerator((ArrayObject)this, (IEnumerator)new RangeEnumerator(0, (int)this.len - 1)));
        objects.Add((object)this);
        if (this.parent == null)
            return;
        this.parent.GetPropertyEnumerator(enums, objects);
    }

    /// <summary>Gets the type of the array.</summary>
    /// <returns>The type of <see cref="T:System.Array" /> or <see cref="T:Microsoft.JScript.ArrayObject" />.</returns>
    public new Type GetType() => !this.implicitWrapper ? typeof(ArrayObject) : this.value.GetType();

    internal override object GetValueAtIndex(uint index)
    {
        return this.value.GetValue(checked((int)index));
    }

    /// <summary>Gets the length of the array.</summary>
    /// <returns>The length of the array.</returns>
    public override object length
    {
        get => (object)this.len;
        set => throw new JScriptException(JSError.AssignmentToReadOnly);
    }

    [DebuggerStepThrough]
    [DebuggerHidden]
    internal override void SetMemberValue(string name, object val)
    {
        long index = !name.Equals("length")
            ? ArrayObject.Array_index_for(name)
            : throw new JScriptException(JSError.AssignmentToReadOnly);
        if (index < 0L)
            base.SetMemberValue(name, val);
        else
            this.value.SetValue(val, (int)index);
    }

    internal override void SetValueAtIndex(uint index, object val)
    {
        Type type = this.value.GetType();
        this.value.SetValue(Convert.CoerceT(val, type.GetElementType()), checked((int)index));
    }

    internal override object Shift() => throw new JScriptException(JSError.ActionNotSupported);

    internal override void Splice(
        uint start,
        uint deleteCount,
        object[] args,
        ArrayObject outArray,
        uint oldLength,
        uint newLength)
    {
        if ((int)oldLength != (int)newLength)
            throw new JScriptException(JSError.ActionNotSupported);
        this.SpliceSlowly(start, deleteCount, args, outArray, oldLength, newLength);
    }

    internal override void Sort(ScriptFunction compareFn)
    {
        Array.Sort(this.value, (IComparer)new ArrayWrapper.SortComparer(compareFn));
    }

    internal override void SwapValues(uint pi, uint qi)
    {
        object valueAtIndex1 = this.GetValueAtIndex(pi);
        object valueAtIndex2 = this.GetValueAtIndex(qi);
        this.SetValueAtIndex(pi, valueAtIndex2);
        this.SetValueAtIndex(qi, valueAtIndex1);
    }

    internal override Array ToNativeArray(Type elementType) => this.value;

    internal override object[] ToArray()
    {
        object[] array = new object[checked((int)this.len)];
        for (uint index = 0; index < this.len; ++index)
            array[(int)index] = this.GetValueAtIndex(index);
        return array;
    }

    internal override ArrayObject Unshift(object[] args)
    {
        throw new JScriptException(JSError.ActionNotSupported);
    }

    internal sealed class SortComparer : IComparer
    {
        internal ScriptFunction compareFn;

        internal SortComparer(ScriptFunction compareFn) => this.compareFn = compareFn;

        public int Compare(object x, object y)
        {
            if (x == null || x is Missing)
            {
                switch (y)
                {
                    case null:
                    case Missing _:
                        return 0;
                    default:
                        return 1;
                }
            }
            else
            {
                if (y == null || y is Missing)
                    return -1;
                if (this.compareFn == null)
                    return string.CompareOrdinal(Convert.ToString(x), Convert.ToString(y));
                double number = Convert.ToNumber(this.compareFn.Call(new object[2]
                {
                    x,
                    y
                }, (object)null));
                return number == number
                    ? (int)Runtime.DoubleToInt64(number)
                    : throw new JScriptException(JSError.NumberExpected);
            }
        }
    }
}