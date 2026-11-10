// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.HashtableEntry
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

#nullable disable
namespace Microsoft.JScript;

internal sealed class HashtableEntry
{
    internal object key;
    internal object value;
    internal uint hashCode;
    internal HashtableEntry next;

    internal HashtableEntry(object key, object value, uint hashCode, HashtableEntry next)
    {
        this.key = key;
        this.value = value;
        this.hashCode = hashCode;
        this.next = next;
    }
}