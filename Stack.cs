// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.Stack
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

#nullable disable
namespace Microsoft.JScript;

internal sealed class Stack
{
  private object[] elements;
  private int top;

  internal Stack()
  {
    this.elements = new object[32 /*0x20*/];
    this.top = -1;
  }

  internal void GuardedPush(object item)
  {
    if (this.top > 500)
      throw new JScriptException(JSError.OutOfStack);
    if (++this.top >= this.elements.Length)
    {
      object[] target = new object[this.elements.Length + 32 /*0x20*/];
      ArrayObject.Copy(this.elements, target, this.elements.Length);
      this.elements = target;
    }
    this.elements[this.top] = item;
  }

  internal void Push(object item)
  {
    if (++this.top >= this.elements.Length)
    {
      object[] target = new object[this.elements.Length + 32 /*0x20*/];
      ArrayObject.Copy(this.elements, target, this.elements.Length);
      this.elements = target;
    }
    this.elements[this.top] = item;
  }

  internal object Pop()
  {
    object element = this.elements[this.top];
    this.elements[this.top--] = (object) null;
    return element;
  }

  internal ScriptObject Peek()
  {
    return this.top < 0 ? (ScriptObject) null : (ScriptObject) this.elements[this.top];
  }

  internal object Peek(int i) => this.elements[this.top - i];

  internal int Size() => this.top + 1;

  internal void TrimToSize(int i) => this.top = i - 1;
}
