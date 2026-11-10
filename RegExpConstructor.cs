// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.RegExpConstructor
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System.Text.RegularExpressions;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Represents a prototype-based object that is defined by a constructor function. This class belongs to the built-in object model category.</summary>
public sealed class RegExpConstructor : ScriptFunction
{
  internal static readonly RegExpConstructor ob = new RegExpConstructor();
  private RegExpPrototype originalPrototype;
  internal ArrayPrototype arrayPrototype;
  private Regex regex;
  private Match lastRegexMatch;
  internal object inputString;
  private string lastInput;

  internal RegExpConstructor()
    : base((ScriptObject) FunctionPrototype.ob, "RegExp", 2)
  {
    this.originalPrototype = RegExpPrototype.ob;
    RegExpPrototype._constructor = this;
    this.proto = (object) RegExpPrototype.ob;
    this.arrayPrototype = ArrayPrototype.ob;
    this.regex = (Regex) null;
    this.lastRegexMatch = (Match) null;
    this.inputString = (object) "";
    this.lastInput = (string) null;
  }

  internal RegExpConstructor(
    LenientFunctionPrototype parent,
    LenientRegExpPrototype prototypeProp,
    LenientArrayPrototype arrayPrototype)
    : base((ScriptObject) parent, "RegExp", 2)
  {
    this.originalPrototype = (RegExpPrototype) prototypeProp;
    prototypeProp.constructor = (object) this;
    this.proto = (object) prototypeProp;
    this.arrayPrototype = (ArrayPrototype) arrayPrototype;
    this.regex = (Regex) null;
    this.lastRegexMatch = (Match) null;
    this.inputString = (object) "";
    this.lastInput = (string) null;
    this.noExpando = false;
  }

  internal override object Call(object[] args, object thisob) => (object) this.Invoke(args);

  internal override object Construct(object[] args) => (object) this.CreateInstance(args);

  private RegExpObject ConstructNew(object[] args)
  {
    string source = args.Length == 0 || args[0] == null ? "" : Convert.ToString(args[0]);
    if (args.Length != 0 && args[0] is Regex)
      throw new JScriptException(JSError.TypeMismatch);
    bool ignoreCase = false;
    bool global = false;
    bool multiline = false;
    if (args.Length >= 2 && args[1] != null)
    {
      foreach (char ch in Convert.ToString(args[1]))
      {
        switch (ch)
        {
          case 'g':
            global = true;
            break;
          case 'i':
            ignoreCase = true;
            break;
          case 'm':
            multiline = true;
            break;
          default:
            throw new JScriptException(JSError.RegExpSyntax);
        }
      }
    }
    return new RegExpObject(this.originalPrototype, source, ignoreCase, global, multiline, this);
  }

  /// <summary>Creates a new <see cref="T:Microsoft.JScript.RegExpObject" /> object for the specified regular expression pattern and flags.</summary>
  /// <returns>A new <see cref="T:Microsoft.JScript.RegExpObject" /> object.</returns>
  /// <param name="pattern">The regular expression pattern to match.</param>
  /// <param name="ignoreCase">true to use case-insensitive matching; otherwise, false.</param>
  /// <param name="global">true to use a global match; otherwise, false.</param>
  /// <param name="multiline">true to use a multiline match; otherwise, false.</param>
  public object Construct(string pattern, bool ignoreCase, bool global, bool multiline)
  {
    return (object) new RegExpObject(this.originalPrototype, pattern, ignoreCase, global, multiline, this);
  }

  /// <summary>Creates an instance of the prototype-based object, passing in the specified arguments.</summary>
  /// <returns>An instance of the object.</returns>
  /// <param name="args">The arguments to pass to the constructor.</param>
  [JSFunction(JSFunctionAttributeEnum.HasVarArgs)]
  public RegExpObject CreateInstance(params object[] args)
  {
    if (args == null || args.Length == 0 || !(args[0] is RegExpObject regExpObject))
      return this.ConstructNew(args);
    if (args.Length > 1 && args[1] != null)
      throw new JScriptException(JSError.RegExpSyntax);
    return new RegExpObject(this.originalPrototype, regExpObject.source, regExpObject.ignoreCase, regExpObject.global, regExpObject.multiline, this);
  }

  /// <summary>Creates an instance of a prototype-based object, passing in the specified arguments.</summary>
  /// <returns>An instance of the object.</returns>
  /// <param name="args">The arguments to pass to the constructor.</param>
  [JSFunction(JSFunctionAttributeEnum.HasVarArgs)]
  public RegExpObject Invoke(params object[] args)
  {
    if (args == null || args.Length == 0 || !(args[0] is RegExpObject regExpObject))
      return this.ConstructNew(args);
    if (args.Length > 1 && args[1] != null)
      throw new JScriptException(JSError.RegExpSyntax);
    return regExpObject;
  }

  private object GetIndex()
  {
    return (object) (this.lastRegexMatch == null ? -1 : this.lastRegexMatch.Index);
  }

  private object GetInput() => this.inputString;

  private object GetLastIndex()
  {
    return (object) (this.lastRegexMatch == null ? -1 : (this.lastRegexMatch.Length == 0 ? this.lastRegexMatch.Index + 1 : this.lastRegexMatch.Index + this.lastRegexMatch.Length));
  }

  private object GetLastMatch()
  {
    return this.lastRegexMatch != null ? (object) this.lastRegexMatch.ToString() : (object) "";
  }

  private object GetLastParen()
  {
    if (this.regex == null || this.lastRegexMatch == null)
      return (object) "";
    string[] groupNames = this.regex.GetGroupNames();
    if (groupNames.Length <= 1)
      return (object) "";
    Group group = this.lastRegexMatch.Groups[this.regex.GroupNumberFromName(groupNames[groupNames.Length - 1])];
    return !group.Success ? (object) "" : (object) group.ToString();
  }

  private object GetLeftContext()
  {
    return this.lastRegexMatch != null && this.lastInput != null ? (object) this.lastInput.Substring(0, this.lastRegexMatch.Index) : (object) "";
  }

  internal override object GetMemberValue(string name)
  {
    if (name.Length == 2 && name[0] == '$')
    {
      char ch = name[1];
      switch (ch)
      {
        case '&':
          return this.GetLastMatch();
        case '\'':
          return this.GetRightContext();
        case '+':
          return this.GetLastParen();
        case '1':
        case '2':
        case '3':
        case '4':
        case '5':
        case '6':
        case '7':
        case '8':
        case '9':
          if (this.lastRegexMatch == null)
            return (object) "";
          Group group = this.lastRegexMatch.Groups[ch.ToString()];
          return !group.Success ? (object) "" : (object) group.ToString();
        case '_':
          return this.GetInput();
        case '`':
          return this.GetLeftContext();
      }
    }
    return base.GetMemberValue(name);
  }

  private object GetRightContext()
  {
    return this.lastRegexMatch != null && this.lastInput != null ? (object) this.lastInput.Substring(this.lastRegexMatch.Index + this.lastRegexMatch.Length) : (object) "";
  }

  private void SetInput(object value) => this.inputString = value;

  internal override void SetMemberValue(string name, object value)
  {
    if (this.noExpando)
      throw new JScriptException(JSError.AssignmentToReadOnly);
    if (name.Length == 2 && name[0] == '$')
    {
      switch (name[1])
      {
        case '&':
          return;
        case '\'':
          return;
        case '+':
          return;
        case '1':
          return;
        case '2':
          return;
        case '3':
          return;
        case '4':
          return;
        case '5':
          return;
        case '6':
          return;
        case '7':
          return;
        case '8':
          return;
        case '9':
          return;
        case '_':
          this.SetInput(value);
          return;
        case '`':
          return;
      }
    }
    base.SetMemberValue(name, value);
  }

  internal int UpdateConstructor(Regex regex, Match match, string input)
  {
    if (!this.noExpando)
    {
      this.regex = regex;
      this.lastRegexMatch = match;
      this.inputString = (object) input;
      this.lastInput = input;
    }
    return match.Length != 0 ? match.Index + match.Length : match.Index + 1;
  }

  /// <summary>Gets the index of the last successful match, before advancing the index.</summary>
  /// <returns>The index of the last successful match, if there was one. </returns>
  public object index => this.GetIndex();

  /// <summary>Gets or sets the input string for a regular expression search.</summary>
  /// <returns>The input string for a regular expression search.</returns>
  public object input
  {
    get => this.GetInput();
    set
    {
      if (this.noExpando)
        throw new JScriptException(JSError.AssignmentToReadOnly);
      this.SetInput(value);
    }
  }

  /// <summary>Gets the index of the last successful match, after advancing the index.</summary>
  /// <returns>The index of the last successful match, if there was one.</returns>
  public object lastIndex => this.GetLastIndex();

  /// <summary>Gets a string representation of the last successful match.</summary>
  /// <returns>A string representation of the last successful match, if there was one.</returns>
  public object lastMatch => this.GetLastMatch();

  /// <summary>Gets the last <see cref="T:System.Text.RegularExpressions.Group" /> from the <see cref="P:System.Text.RegularExpressions.Match.Groups" /> of the last match.</summary>
  /// <returns>A string representation of the last group, if there was one.</returns>
  public object lastParen => this.GetLastParen();

  /// <summary>Gets a substring of the input string between the start of the input and the start of the last successful match.</summary>
  /// <returns>A substring of the input string, if there was one.</returns>
  public object leftContext => this.GetLeftContext();

  /// <summary>Gets a substring of the input string between the start of the last successful match and the end of the input.</summary>
  /// <returns>A substring of the input string, if there was one.</returns>
  public object rightContext => this.GetRightContext();
}
