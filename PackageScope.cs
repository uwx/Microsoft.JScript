// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.PackageScope
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Globalization;
using System.Reflection;

#nullable disable
namespace Microsoft.JScript;

internal class PackageScope : ActivationObject
{
  internal string name;
  internal Package owner;

  public PackageScope(ScriptObject parent)
    : base(parent)
  {
    this.fast = true;
    this.name = (string) null;
    this.owner = (Package) null;
    this.isKnownAtCompileTime = true;
  }

  internal override JSVariableField AddNewField(
    string name,
    object value,
    FieldAttributes attributeFlags)
  {
    base.AddNewField($"{this.name}.{name}", value, attributeFlags);
    return base.AddNewField(name, value, attributeFlags);
  }

  internal void AddOwnName()
  {
    string name = this.name;
    int length = name.IndexOf('.');
    if (length > 0)
      name = name.Substring(0, length);
    base.AddNewField(name, (object) Namespace.GetNamespace(name, this.engine), FieldAttributes.Public | FieldAttributes.Literal);
  }

  protected override JSVariableField CreateField(
    string name,
    FieldAttributes attributeFlags,
    object value)
  {
    return (JSVariableField) new JSGlobalField((ScriptObject) this, name, value, attributeFlags);
  }

  internal override string GetName() => this.name;

  internal void MergeWith(PackageScope p)
  {
    foreach (JSGlobalField jsGlobalField in p.field_table)
    {
      ClassScope classScope = jsGlobalField.value as ClassScope;
      if (this.name_table[(object) jsGlobalField.Name] != null)
      {
        if (classScope != null)
        {
          classScope.owner.context.HandleError(JSError.DuplicateName, jsGlobalField.Name, true);
          classScope.owner.name += p.GetHashCode().ToString((IFormatProvider) CultureInfo.InvariantCulture);
        }
      }
      else
      {
        this.field_table.Add((object) jsGlobalField);
        this.name_table[(object) jsGlobalField.Name] = (object) jsGlobalField;
        if (classScope != null)
        {
          classScope.owner.enclosingScope = (ScriptObject) this;
          classScope.package = this;
        }
      }
    }
  }
}
