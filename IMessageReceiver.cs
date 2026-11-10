// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.IMessageReceiver
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System.Runtime.InteropServices;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Defines methods that receive output from a scripting engine. This interface acts as a callback object to capture output that would otherwise go to the console.</summary>
[Guid("F062C7FB-53BF-4f0d-B0F6-D66C5948E63F")]
[ComVisible(true)]
public interface IMessageReceiver
{
  /// <summary>When implemented in a class, captures the specified output.</summary>
  /// <param name="strValue">The output to capture.</param>
  void Message(string strValue);
}
