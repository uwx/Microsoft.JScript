// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.RegExpMatch
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Globalization;
using System.Text.RegularExpressions;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Represents the results from a regular expression match. This class belongs to the built-in object model category.</summary>
public sealed class RegExpMatch : ArrayObject
{
  private bool hydrated;
  private Regex regex;
  private MatchCollection matches;
  private Match match;

  internal RegExpMatch(ArrayPrototype parent, Regex regex, Match match, string input)
    : base((ScriptObject) parent, typeof (RegExpMatch))
  {
    this.hydrated = false;
    this.regex = regex;
    this.matches = (MatchCollection) null;
    this.match = match;
    this.SetMemberValue(nameof (input), (object) input);
    this.SetMemberValue("index", (object) match.Index);
    this.SetMemberValue("lastIndex", (object) (match.Length == 0 ? match.Index + 1 : match.Index + match.Length));
    string[] groupNames = regex.GetGroupNames();
    int num1 = 0;
    for (int index = 1; index < groupNames.Length; ++index)
    {
      string str = groupNames[index];
      int num2 = regex.GroupNumberFromName(str);
      if (str.Equals(num2.ToString((IFormatProvider) CultureInfo.InvariantCulture)))
      {
        if (num2 > num1)
          num1 = num2;
      }
      else
      {
        Group group = match.Groups[str];
        this.SetMemberValue(str, group.Success ? (object) group.ToString() : (object) (string) null);
      }
    }
    this.length = (object) (num1 + 1);
  }

  internal RegExpMatch(ArrayPrototype parent, Regex regex, MatchCollection matches, string input)
    : base((ScriptObject) parent)
  {
    this.hydrated = false;
    this.length = (object) matches.Count;
    this.regex = regex;
    this.matches = matches;
    this.match = (Match) null;
    Match match = matches[matches.Count - 1];
    this.SetMemberValue(nameof (input), (object) input);
    this.SetMemberValue("index", (object) match.Index);
    this.SetMemberValue("lastIndex", (object) (match.Length == 0 ? match.Index + 1 : match.Index + match.Length));
  }

  internal override void Concat(ArrayObject source)
  {
    if (!this.hydrated)
      this.Hydrate();
    base.Concat(source);
  }

  internal override void Concat(object value)
  {
    if (!this.hydrated)
      this.Hydrate();
    base.Concat(value);
  }

  internal override bool DeleteValueAtIndex(uint index)
  {
    if (!this.hydrated)
      this.Hydrate();
    return base.DeleteValueAtIndex(index);
  }

  internal override object GetValueAtIndex(uint index)
  {
    if (!this.hydrated)
    {
      if (this.matches != null)
      {
        if ((long) index < (long) this.matches.Count)
          return (object) this.matches[(int) index].ToString();
      }
      else if (this.match != null)
      {
        int groupnum = this.regex.GroupNumberFromName(index.ToString((IFormatProvider) CultureInfo.InvariantCulture));
        if (groupnum >= 0)
        {
          Group group = this.match.Groups[groupnum];
          return !group.Success ? (object) "" : (object) group.ToString();
        }
      }
    }
    return base.GetValueAtIndex(index);
  }

  internal override object GetMemberValue(string name)
  {
    if (!this.hydrated)
    {
      long index = ArrayObject.Array_index_for(name);
      if (index >= 0L)
        return this.GetValueAtIndex((uint) index);
    }
    return base.GetMemberValue(name);
  }

  private void Hydrate()
  {
    if (this.matches != null)
    {
      int num = 0;
      for (int count = this.matches.Count; num < count; ++num)
        base.SetValueAtIndex((uint) num, (object) this.matches[num].ToString());
    }
    else if (this.match != null)
    {
      string[] groupNames = this.regex.GetGroupNames();
      int index = 1;
      for (int length = groupNames.Length; index < length; ++index)
      {
        string name = groupNames[index];
        int num = this.regex.GroupNumberFromName(name);
        Group group = this.match.Groups[num];
        object obj = group.Success ? (object) group.ToString() : (object) "";
        if (name.Equals(num.ToString((IFormatProvider) CultureInfo.InvariantCulture)))
          base.SetValueAtIndex((uint) num, obj);
      }
    }
    this.hydrated = true;
    this.regex = (Regex) null;
    this.matches = (MatchCollection) null;
    this.match = (Match) null;
  }

  internal override void SetValueAtIndex(uint index, object value)
  {
    if (!this.hydrated)
      this.Hydrate();
    base.SetValueAtIndex(index, value);
  }

  internal override object Shift()
  {
    if (!this.hydrated)
      this.Hydrate();
    return base.Shift();
  }

  internal override void Sort(ScriptFunction compareFn)
  {
    if (!this.hydrated)
      this.Hydrate();
    base.Sort(compareFn);
  }

  internal override void Splice(
    uint start,
    uint deleteItems,
    object[] args,
    ArrayObject outArray,
    uint oldLength,
    uint newLength)
  {
    if (!this.hydrated)
      this.Hydrate();
    base.Splice(start, deleteItems, args, outArray, oldLength, newLength);
  }

  internal override void SwapValues(uint pi, uint qi)
  {
    if (!this.hydrated)
      this.Hydrate();
    base.SwapValues(pi, qi);
  }

  internal override ArrayObject Unshift(object[] args)
  {
    if (!this.hydrated)
      this.Hydrate();
    return base.Unshift(args);
  }
}
