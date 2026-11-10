// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.BlockScope
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System.Collections;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Represents the scope of a block of code that is taking part in a scope hierarchy and that can be pushed onto the global scope stack. For example, this class can represent the scope of a try block. </summary>
public class BlockScope : ActivationObject
{
  private static int counter;
  internal bool catchHanderScope;
  internal int scopeId;
  private ArrayList localFieldsForDebugInfo;

  internal BlockScope(ScriptObject parent)
    : base(parent)
  {
    this.scopeId = BlockScope.counter++;
    this.isKnownAtCompileTime = true;
    this.fast = !(parent is ActivationObject) || ((ActivationObject) parent).fast;
    this.localFieldsForDebugInfo = new ArrayList();
  }

  /// <summary>Initializes a new instance of the <see cref="T:Microsoft.JScript.BlockScope" /> class.</summary>
  /// <param name="parent">The parent of the scope.</param>
  /// <param name="name">The name of the scope.</param>
  /// <param name="scopeId">The ID of the scope.</param>
  public BlockScope(ScriptObject parent, string name, int scopeId)
    : base(parent)
  {
    this.scopeId = scopeId;
    JSField field = (JSField) this.parent.GetField($"{name}:{this.scopeId.ToString()}", BindingFlags.Public);
    this.name_table[(object) name] = (object) field;
    this.field_table.Add((object) field);
  }

  internal void AddFieldForLocalScopeDebugInfo(JSLocalField field)
  {
    this.localFieldsForDebugInfo.Add((object) field);
  }

  /// <summary>Creates a new scope variable, using the specified name, value, and attributes.</summary>
  /// <returns>A scope variable that has the specified name, value, and attributes.</returns>
  /// <param name="name">The name of the field.</param>
  /// <param name="attributeFlags">A bitwise combination of the enumeration values that describe the attributes of the field.</param>
  /// <param name="value">The value of the field.</param>
  protected override JSVariableField CreateField(
    string name,
    FieldAttributes attributeFlags,
    object value)
  {
    if (!(this.parent is ActivationObject))
      return base.CreateField(name, attributeFlags, value);
    JSVariableField field = ((ActivationObject) this.parent).AddNewField($"{name}:{this.scopeId.ToString()}", value, attributeFlags);
    field.debuggerName = name;
    return field;
  }

  internal void EmitLocalInfoForFields(ILGenerator il)
  {
    foreach (JSLocalField jsLocalField in this.localFieldsForDebugInfo)
      ((LocalBuilder) jsLocalField.metaData).SetLocalSymInfo(jsLocalField.debuggerName);
    if (!(this.parent is GlobalScope))
      return;
    LocalBuilder local = il.DeclareLocal(Typeob.Int32);
    local.SetLocalSymInfo("scopeId for catch block");
    ConstantWrapper.TranslateToILInt(il, this.scopeId);
    il.Emit(OpCodes.Stloc, local);
  }
}
