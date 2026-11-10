// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.ArrayEnumerator
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Collections;
using System.Globalization;

#nullable disable
namespace Microsoft.JScript;

internal class ArrayEnumerator : IEnumerator
{
    private int curr;
    private bool doDenseEnum;
    private bool didDenseEnum;
    private ArrayObject arrayOb;
    private IEnumerator denseEnum;

    internal ArrayEnumerator(ArrayObject arrayOb, IEnumerator denseEnum)
    {
        this.curr = -1;
        this.doDenseEnum = false;
        this.didDenseEnum = false;
        this.arrayOb = arrayOb;
        this.denseEnum = denseEnum;
    }

    public virtual bool MoveNext()
    {
        if (this.doDenseEnum)
        {
            if (this.denseEnum.MoveNext())
                return true;
            this.doDenseEnum = false;
            this.didDenseEnum = true;
        }

        int index = this.curr + 1;
        if ((long)index >= (long)this.arrayOb.len || (long)index >= (long)this.arrayOb.denseArrayLength)
        {
            this.doDenseEnum = !this.didDenseEnum;
            return this.denseEnum.MoveNext();
        }

        this.curr = index;
        return !(this.arrayOb.GetValueAtIndex((uint)index) is Missing) || this.MoveNext();
    }

    public virtual object Current
    {
        get
        {
            return this.doDenseEnum || (long)this.curr >= (long)this.arrayOb.len ||
                   (long)this.curr >= (long)this.arrayOb.denseArrayLength
                ? this.denseEnum.Current
                : (object)this.curr.ToString((IFormatProvider)CultureInfo.InvariantCulture);
        }
    }

    public virtual void Reset()
    {
        this.curr = -1;
        this.doDenseEnum = false;
        this.didDenseEnum = false;
        this.denseEnum.Reset();
    }
}