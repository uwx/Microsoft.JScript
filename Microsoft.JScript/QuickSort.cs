// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.QuickSort
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

#nullable disable
namespace Microsoft.JScript;

internal sealed class QuickSort
{
    internal ScriptFunction compareFn;
    internal object obj;

    internal QuickSort(object obj, ScriptFunction compareFn)
    {
        this.compareFn = compareFn;
        this.obj = obj;
    }

    private int Compare(object x, object y)
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

    internal void SortObject(long left, long right)
    {
        if (right <= left)
            return;
        LateBinding.SwapValues(this.obj,
            (uint)((ulong)left + (ulong)(long)((double)(right - left) * MathObject.random())), (uint)right);
        object valueAtIndex1 = LateBinding.GetValueAtIndex(this.obj, (ulong)right);
        long left1 = left - 1L;
        long right1 = right;
        while (true)
        {
            object valueAtIndex2;
            do
            {
                valueAtIndex2 = LateBinding.GetValueAtIndex(this.obj, (ulong)++left1);
            } while (left1 < right1 && this.Compare(valueAtIndex1, valueAtIndex2) >= 0);

            object valueAtIndex3;
            do
            {
                valueAtIndex3 = LateBinding.GetValueAtIndex(this.obj, (ulong)--right1);
            } while (right1 > left1 && this.Compare(valueAtIndex1, valueAtIndex3) <= 0);

            if (left1 < right1)
                LateBinding.SwapValues(this.obj, (uint)left1, (uint)right1);
            else
                break;
        }

        LateBinding.SwapValues(this.obj, (uint)left1, (uint)right);
        this.SortObject(left, left1 - 1L);
        this.SortObject(left1 + 1L, right);
    }

    internal void SortArray(int left, int right)
    {
        ArrayObject arrayObject = (ArrayObject)this.obj;
        if (right <= left)
            return;
        int index = left + (int)((double)(right - left) * MathObject.random());
        object dense1 = arrayObject.denseArray[index];
        arrayObject.denseArray[index] = arrayObject.denseArray[right];
        arrayObject.denseArray[right] = dense1;
        int i = left - 1;
        int j = right;
        while (true)
        {
            object dense2;
            do
            {
                dense2 = arrayObject.denseArray[++i];
            } while (i < j && this.Compare(dense1, dense2) >= 0);

            object dense3;
            do
            {
                dense3 = arrayObject.denseArray[--j];
            } while (j > i && this.Compare(dense1, dense3) <= 0);

            if (i < j)
                QuickSort.Swap(arrayObject.denseArray, i, j);
            else
                break;
        }

        QuickSort.Swap(arrayObject.denseArray, i, right);
        this.SortArray(left, i - 1);
        this.SortArray(i + 1, right);
    }

    private static void Swap(object[] array, int i, int j)
    {
        object obj = array[i];
        array[i] = array[j];
        array[j] = obj;
    }
}