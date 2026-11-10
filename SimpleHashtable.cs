// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.SimpleHashtable
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Collections;

#nullable disable
namespace Microsoft.JScript;

/// <summary>A hash table for the internal storage of items.</summary>
public sealed class SimpleHashtable
{
  private HashtableEntry[] table;
  internal int count;
  private uint threshold;

  /// <summary>Initializes a new instance of the <see cref="T:Microsoft.JScript.SimpleHashtable" /> class, using the specified threshold.</summary>
  /// <param name="threshold">The number of items at which the hash table must be resized.</param>
  public SimpleHashtable(uint threshold)
  {
    if (threshold < 8U)
      threshold = 8U;
    this.table = new HashtableEntry[(int) threshold * 2 - 1];
    this.count = 0;
    this.threshold = threshold;
  }

  /// <summary>Gets an enumerator for the hash table.</summary>
  /// <returns>An enumerator for the hash table.</returns>
  public IDictionaryEnumerator GetEnumerator()
  {
    return (IDictionaryEnumerator) new SimpleHashtableEnumerator(this.table);
  }

  private HashtableEntry GetHashtableEntry(object key, uint hashCode)
  {
    HashtableEntry hashtableEntry1 = this.table[(int) (hashCode % (uint) this.table.Length)];
    if (hashtableEntry1 == null)
      return (HashtableEntry) null;
    if (hashtableEntry1.key == key)
      return hashtableEntry1;
    HashtableEntry hashtableEntry2 = hashtableEntry1;
    for (HashtableEntry next = hashtableEntry1.next; next != null; next = next.next)
    {
      if (next.key == key)
        return next;
      hashtableEntry2 = next;
    }
    if ((int) hashtableEntry1.hashCode == (int) hashCode && hashtableEntry1.key.Equals(key))
    {
      hashtableEntry1.key = key;
      return hashtableEntry1;
    }
    hashtableEntry2 = hashtableEntry1;
    for (HashtableEntry next = hashtableEntry1.next; next != null; next = next.next)
    {
      if ((int) next.hashCode == (int) hashCode && next.key.Equals(key))
      {
        next.key = key;
        return next;
      }
      hashtableEntry2 = next;
    }
    return (HashtableEntry) null;
  }

  internal object IgnoreCaseGet(string name)
  {
    uint index = 0;
    for (uint length = (uint) this.table.Length; index < length; ++index)
    {
      for (HashtableEntry next = this.table[(int) index]; next != null; next = next.next)
      {
        if (string.Compare((string) next.key, name, StringComparison.OrdinalIgnoreCase) == 0)
          return next.value;
      }
    }
    return (object) null;
  }

  private void Rehash()
  {
    HashtableEntry[] table = this.table;
    uint num = this.threshold = (uint) (table.Length + 1);
    uint length = (uint) ((int) num * 2 - 1);
    HashtableEntry[] hashtableEntryArray = this.table = new HashtableEntry[(int) length];
    uint index1 = num - 1U;
    while (index1-- > 0U)
    {
      HashtableEntry next = table[(int) index1];
      while (next != null)
      {
        HashtableEntry hashtableEntry = next;
        next = next.next;
        int index2 = (int) (hashtableEntry.hashCode % length);
        hashtableEntry.next = hashtableEntryArray[index2];
        hashtableEntryArray[index2] = hashtableEntry;
      }
    }
  }

  /// <summary>Removes the item with the specified key from the hash table.</summary>
  /// <param name="key">The key of the item to remove.</param>
  public void Remove(object key)
  {
    uint hashCode = (uint) key.GetHashCode();
    int index = (int) (hashCode % (uint) this.table.Length);
    HashtableEntry hashtableEntry = this.table[index];
    --this.count;
    while (hashtableEntry != null && (int) hashtableEntry.hashCode == (int) hashCode && (hashtableEntry.key == key || hashtableEntry.key.Equals(key)))
      hashtableEntry = hashtableEntry.next;
    this.table[index] = hashtableEntry;
    HashtableEntry next;
    for (; hashtableEntry != null; hashtableEntry = next)
    {
      next = hashtableEntry.next;
      while (next != null && (int) next.hashCode == (int) hashCode && (next.key == key || next.key.Equals(key)))
        next = next.next;
      hashtableEntry.next = next;
    }
  }

  /// <summary>Gets or sets the item that has the specified key.</summary>
  /// <returns>The value of the item that has the specified <paramref name="key" />.</returns>
  /// <param name="key">The key of the item to get or set.</param>
  public object this[object key]
  {
    get => this.GetHashtableEntry(key, (uint) key.GetHashCode())?.value;
    set
    {
      uint hashCode = (uint) key.GetHashCode();
      HashtableEntry hashtableEntry = this.GetHashtableEntry(key, hashCode);
      if (hashtableEntry != null)
      {
        hashtableEntry.value = value;
      }
      else
      {
        if ((long) ++this.count >= (long) this.threshold)
          this.Rehash();
        int index = (int) (hashCode % (uint) this.table.Length);
        this.table[index] = new HashtableEntry(key, value, hashCode, this.table[index]);
      }
    }
  }
}
