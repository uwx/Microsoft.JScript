// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.SimpleHashtableEnumerator
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System.Collections;

#nullable disable
namespace Microsoft.JScript;

internal sealed class SimpleHashtableEnumerator : IDictionaryEnumerator, IEnumerator
{
    private HashtableEntry[] table;
    private int count;
    private int index;
    private HashtableEntry currentEntry;

    internal SimpleHashtableEnumerator(HashtableEntry[] table)
    {
        this.table = table;
        this.count = table.Length;
        this.index = -1;
        this.currentEntry = (HashtableEntry)null;
    }

    public object Current => this.Key;

    public DictionaryEntry Entry => new DictionaryEntry(this.Key, this.Value);

    public object Key => this.currentEntry.key;

    public bool MoveNext()
    {
        HashtableEntry[] table = this.table;
        if (this.currentEntry != null)
        {
            this.currentEntry = this.currentEntry.next;
            if (this.currentEntry != null)
                return true;
        }

        int index = ++this.index;
        for (int count = this.count; index < count; ++index)
        {
            if (table[index] != null)
            {
                this.index = index;
                this.currentEntry = table[index];
                return true;
            }
        }

        return false;
    }

    public void Reset()
    {
        this.index = -1;
        this.currentEntry = (HashtableEntry)null;
    }

    public object Value => this.currentEntry.value;
}