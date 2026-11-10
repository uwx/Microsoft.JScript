// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.ReplaceUsingFunction
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Text.RegularExpressions;

#nullable disable
namespace Microsoft.JScript;

internal class ReplaceUsingFunction : RegExpReplace
{
  private ScriptFunction function;
  private int cArgs;
  private int[] groupNumbers;
  private string source;

  internal ReplaceUsingFunction(Regex regex, ScriptFunction function, string source)
  {
    this.function = function;
    this.cArgs = function.GetNumberOfFormalParameters();
    bool flag = function is Closure && ((Closure) function).func.hasArgumentsObject;
    this.groupNumbers = (int[]) null;
    this.source = source;
    if (!(this.cArgs > 1 | flag))
      return;
    string[] groupNames = regex.GetGroupNames();
    int length = groupNames.Length - 1;
    if (flag)
      this.cArgs = length + 3;
    if (length <= 0)
      return;
    if (length > this.cArgs - 1)
      length = this.cArgs - 1;
    this.groupNumbers = new int[length];
    for (int index = 0; index < length; ++index)
      this.groupNumbers[index] = regex.GroupNumberFromName(groupNames[index + 1]);
  }

  internal override string Evaluate(Match match)
  {
    this.lastMatch = match;
    object[] args = new object[this.cArgs];
    if (this.cArgs > 0)
    {
      args[0] = (object) match.ToString();
      if (this.cArgs > 1)
      {
        int index1 = 1;
        if (this.groupNumbers != null)
        {
          for (; index1 <= this.groupNumbers.Length; ++index1)
          {
            Group group = match.Groups[this.groupNumbers[index1 - 1]];
            args[index1] = group.Success ? (object) group.ToString() : (object) (string) null;
          }
        }
        if (index1 < this.cArgs)
        {
          object[] objArray1 = args;
          int index2 = index1;
          int num = index2 + 1;
          objArray1[index2] = (object) match.Index;
          if (num < this.cArgs)
          {
            object[] objArray2 = args;
            int index4 = num;
            int index5 = index4 + 1;
            string source = this.source;
            objArray2[index4] = (object) source;
            for (; index5 < this.cArgs; ++index5)
              args[index5] = (object) null;
          }
        }
      }
    }
    object obj = this.function.Call(args, (object) null);
    return match.Result(obj is Empty ? "" : Convert.ToString(obj));
  }
}
