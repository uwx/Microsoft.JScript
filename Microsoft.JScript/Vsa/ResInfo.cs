// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.Vsa.ResInfo
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.IO;

#nullable disable
namespace Microsoft.JScript.Vsa;

/// <summary>This class, member, or property is internal to the script engine and should not be called from your code.</summary>
[Obsolete(
    "Use of this type is not recommended because it is being deprecated in Visual Studio 2005; there will be no replacement for this feature. Please see the ICodeCompiler documentation for additional help.")]
public class ResInfo
{
    /// <summary>This class, member, or property is internal to the script engine and should not be called from your code.</summary>
    public string filename;

    /// <summary>This class, member, or property is internal to the script engine and should not be called from your code.</summary>
    public string fullpath;

    /// <summary>This class, member, or property is internal to the script engine and should not be called from your code.</summary>
    public string name;

    /// <summary>This class, member, or property is internal to the script engine and should not be called from your code.</summary>
    public bool isPublic;

    /// <summary>This class, member, or property is internal to the script engine and should not be called from your code.</summary>
    public bool isLinked;

    /// <summary>This class, member, or property is internal to the script engine and should not be called from your code.</summary>
    public ResInfo(string filename, string name, bool isPublic, bool isLinked)
    {
        this.filename = filename;
        this.fullpath = Path.GetFullPath(filename);
        this.name = name;
        this.isPublic = isPublic;
        this.isLinked = isLinked;
    }

    /// <summary>This class, member, or property is internal to the script engine and should not be called from your code.</summary>
    public ResInfo(string resinfo, bool isLinked)
    {
        string[] strArray = resinfo.Split(',');
        int length = strArray.Length;
        this.filename = strArray[0];
        this.name = Path.GetFileName(this.filename);
        this.isPublic = true;
        this.isLinked = isLinked;
        if (length == 2)
            this.name = strArray[1];
        else if (length > 2)
        {
            bool flag = false;
            if (string.Compare(strArray[length - 1], "public", StringComparison.OrdinalIgnoreCase) == 0)
                flag = true;
            else if (string.Compare(strArray[length - 1], "private", StringComparison.OrdinalIgnoreCase) == 0)
            {
                this.isPublic = false;
                flag = true;
            }

            this.name = strArray[length - (flag ? 2 : 1)];
            this.filename = string.Join(",", strArray, 0, length - (flag ? 2 : 1));
        }

        this.fullpath = Path.GetFullPath(this.filename);
    }
}