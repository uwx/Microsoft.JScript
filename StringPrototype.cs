// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.StringPrototype
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using Microsoft.JScript.Vsa;
using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Provides static methods and properties for <see cref="T:Microsoft.JScript.StringObject" /> objects. All methods of this class are called by passing in a reference to the object that is requesting access to the method. This class belongs to the built-in object model category.</summary>
public class StringPrototype : StringObject
{
  internal static readonly StringPrototype ob = new StringPrototype(FunctionPrototype.ob, ObjectPrototype.ob);
  internal static StringConstructor _constructor;

  internal StringPrototype(FunctionPrototype funcprot, ObjectPrototype parent)
    : base((ScriptObject) parent, "")
  {
    this.noExpando = true;
  }

  /// <summary>Converts the specified object to text enclosed in anchor tags, and gives the anchor element the specified name attribute.</summary>
  /// <returns>A string in the form "&lt;A NAME=\""+ <paramref name="anchorName" /> +"\"&gt;"+ <paramref name="thisob" /> +"&lt;/A&gt;".</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  /// <param name="anchorName">The name attribute of the anchor element.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.String_anchor)]
  public static string anchor(object thisob, object anchorName)
  {
    string str = Convert.ToString(thisob);
    return $"<A NAME=\"{Convert.ToString(anchorName)}\">{str}</A>";
  }

  /// <summary>Converts the specified object to text enclosed in bigger text markup tags.</summary>
  /// <returns>A string in the form "&lt;BIG&gt;"+ <paramref name="thisob" /> +"&lt;/BIG&gt;".</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.String_big)]
  public static string big(object thisob) => $"<BIG>{Convert.ToString(thisob)}</BIG>";

  /// <summary>Converts the specified object to text enclosed in blink markup tags.</summary>
  /// <returns>A string in the form "&lt;BLINK&gt;"+ <paramref name="thisob" /> +"&lt;/BLINK&gt;".</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.String_blink)]
  public static string blink(object thisob) => $"<BLINK>{Convert.ToString(thisob)}</BLINK>";

  /// <summary>Converts the specified object to text enclosed in bold text markup tags.</summary>
  /// <returns>A string in the form "&lt;B&gt;"+ <paramref name="thisob" /> +"&lt;/B&gt;".</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.String_bold)]
  public static string bold(object thisob) => $"<B>{Convert.ToString(thisob)}</B>";

  /// <summary>Returns the character at the specified position in the specified object.</summary>
  /// <returns>The character at <paramref name="pos" />, if there is one.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  /// <param name="pos">The position of the character to return.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.String_charAt)]
  public static string charAt(object thisob, double pos)
  {
    string str = Convert.ToString(thisob);
    double integer = Convert.ToInteger(pos);
    return integer < 0.0 || integer >= (double) str.Length ? "" : str.Substring((int) integer, 1);
  }

  /// <summary>Returns the ASCII code of the character at the specified position in the specified object.</summary>
  /// <returns>The code of the character at <paramref name="pos" />, if there is one.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  /// <param name="pos">The position of the character whose code you want to return.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.String_charCodeAt)]
  public static object charCodeAt(object thisob, double pos)
  {
    string str = Convert.ToString(thisob);
    double integer = Convert.ToInteger(pos);
    return integer < 0.0 || integer >= (double) str.Length ? (object) double.NaN : (object) (int) str[(int) integer];
  }

  /// <summary>Appends the specified arguments to the specified object.</summary>
  /// <returns>A string representation of <paramref name="thisob" />, with <paramref name="args" /> appended.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  /// <param name="args">The arguments to append to <paramref name="thisob" />.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject | JSFunctionAttributeEnum.HasVarArgs, JSBuiltin.String_concat)]
  public static string concat(object thisob, params object[] args)
  {
    StringBuilder stringBuilder = new StringBuilder(Convert.ToString(thisob));
    for (int index = 0; index < args.Length; ++index)
      stringBuilder.Append(Convert.ToString(args[index]));
    return stringBuilder.ToString();
  }

  /// <summary>Gets a reference to a prototype-based object that is using this prototype.</summary>
  /// <returns>A reference to a prototype-based object.</returns>
  public static StringConstructor constructor => StringPrototype._constructor;

  /// <summary>Converts the specified object to text enclosed in fixed-width text markup tags.</summary>
  /// <returns>A string in the form "&lt;TT&gt;"+ <paramref name="thisob" /> +"&lt;/TT&gt;".</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.String_fixed)]
  public static string @fixed(object thisob) => $"<TT>{Convert.ToString(thisob)}</TT>";

  /// <summary>Converts the specified object to text enclosed in font tags, and gives the element the specified color attribute.</summary>
  /// <returns>A string in the form "&lt;FONT COLOR=\""+ <paramref name="colorName" /> +"\"&gt;"+ <paramref name="thisob" /> +"&lt;/FONT&gt;".</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  /// <param name="colorName">The color attribute of the font element.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.String_fontcolor)]
  public static string fontcolor(object thisob, object colorName)
  {
    string str = Convert.ToString(thisob);
    return $"<FONT COLOR=\"{Convert.ToString(thisob)}\">{str}</FONT>";
  }

  /// <summary>Converts the specified object to text enclosed in font tags, and gives the element the specified size attribute.</summary>
  /// <returns>A string in the form "&lt;FONT SIZE=\""+ <paramref name="fontSize" /> +"\"&gt;"+ <paramref name="thisob" /> +"&lt;/FONT&gt;".</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  /// <param name="fontSize">The size attribute of the font element.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.String_fontsize)]
  public static string fontsize(object thisob, object fontSize)
  {
    string str = Convert.ToString(thisob);
    return $"<FONT SIZE=\"{Convert.ToString(fontSize)}\">{str}</FONT>";
  }

  /// <summary>Searches for the first occurrence of the specified string in the specified object, starting from the specified character position.</summary>
  /// <returns>The position of the first occurrence of <paramref name="searchString" />, if it is found.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  /// <param name="searchString">The string to search for.</param>
  /// <param name="position">The position at which to start the search.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.String_indexOf)]
  public static int indexOf(object thisob, object searchString, double position)
  {
    string str1 = Convert.ToString(thisob);
    string str2 = Convert.ToString(searchString);
    double startIndex = Convert.ToInteger(position);
    int length = str1.Length;
    if (startIndex < 0.0)
      startIndex = 0.0;
    if (startIndex < (double) length)
      return str1.IndexOf(str2, (int) startIndex);
    return str2.Length != 0 ? -1 : 0;
  }

  /// <summary>Converts the specified object to text enclosed in italic text markup tags.</summary>
  /// <returns>A string in the form "&lt;I&gt;"+ <paramref name="thisob" /> +"&lt;/I&gt;".</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.String_italics)]
  public static string italics(object thisob) => $"<I>{Convert.ToString(thisob)}</I>";

  /// <summary>Searches for the last occurrence of the specified string in the specified object, starting from the specified character position.</summary>
  /// <returns>The position of the last occurrence of <paramref name="searchString" />, if it is found.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  /// <param name="searchString">The string to search for.</param>
  /// <param name="position">The position at which to start the search.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.String_lastIndexOf)]
  public static int lastIndexOf(object thisob, object searchString, double position)
  {
    string str1 = Convert.ToString(thisob);
    string str2 = Convert.ToString(searchString);
    int length1 = str1.Length;
    int num = position != position || position > (double) length1 ? length1 : (int) position;
    if (num < 0)
      num = 0;
    if (num >= length1)
      num = length1;
    int length2 = str2.Length;
    if (length2 == 0)
      return num;
    int startIndex = num - 1 + length2;
    if (startIndex >= length1)
      startIndex = length1 - 1;
    return startIndex < 0 ? -1 : str1.LastIndexOf(str2, startIndex);
  }

  /// <summary>Converts the specified object to text enclosed in anchor tags, and gives the anchor element the link target attribute that is specified.</summary>
  /// <returns>A string in the form "&lt;A HREF=\""+ <paramref name="linkRef" /> +"\"&gt;"+ <paramref name="thisob" /> +"&lt;/A&gt;".</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  /// <param name="linkRef">The target attribute of the anchor element.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.String_link)]
  public static string link(object thisob, object linkRef)
  {
    string str = Convert.ToString(thisob);
    return $"<A HREF=\"{Convert.ToString(linkRef)}\">{str}</A>";
  }

  /// <summary>Compares the two specified strings and returns an integer that indicates the lexical relationship between them.</summary>
  /// <returns>A negative value when <paramref name="thisob" /> is less than <paramref name="thatob" />; zero when <paramref name="thisob" /> equals <paramref name="thatob" />; a positive value when <paramref name="thisob" /> is greater than <paramref name="thatob" />.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  /// <param name="thatob">The string to compare to <paramref name="thisob" />.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.String_localeCompare)]
  public static int localeCompare(object thisob, object thatob)
  {
    return string.Compare(Convert.ToString(thisob), Convert.ToString(thatob), StringComparison.CurrentCulture);
  }

  /// <summary>Runs a search for the specified regular expression pattern on the specified object.</summary>
  /// <returns>The result of the search for the regular expression pattern.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  /// <param name="engine">A reference to the scripting engine.</param>
  /// <param name="regExp">The regular expression pattern to search for.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject | JSFunctionAttributeEnum.HasEngine, JSBuiltin.String_match)]
  public static object match(object thisob, VsaEngine engine, object regExp)
  {
    string input = Convert.ToString(thisob);
    RegExpObject regExpObject = StringPrototype.ToRegExpObject(regExp, engine);
    if (!regExpObject.globalInt)
    {
      Match match = regExpObject.regex.Match(input);
      if (!match.Success)
      {
        regExpObject.lastIndexInt = (object) 0;
        return (object) DBNull.Value;
      }
      if (regExpObject.regExpConst == null)
        return (object) new RegExpMatch(engine.Globals.globalObject.originalRegExp.arrayPrototype, regExpObject.regex, match, input);
      regExpObject.lastIndexInt = (object) regExpObject.regExpConst.UpdateConstructor(regExpObject.regex, match, input);
      return (object) new RegExpMatch(regExpObject.regExpConst.arrayPrototype, regExpObject.regex, match, input);
    }
    MatchCollection matches = regExpObject.regex.Matches(input);
    if (matches.Count == 0)
    {
      regExpObject.lastIndexInt = (object) 0;
      return (object) DBNull.Value;
    }
    Match match1 = matches[matches.Count - 1];
    regExpObject.lastIndexInt = (object) regExpObject.regExpConst.UpdateConstructor(regExpObject.regex, match1, input);
    return (object) new RegExpMatch(regExpObject.regExpConst.arrayPrototype, regExpObject.regex, matches, input);
  }

  /// <summary>Runs a search for the specified regular expression pattern on the first specified object, and replaces any matches with the third specified object.</summary>
  /// <returns>The new string, after the replacement is made.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  /// <param name="regExp">The regular expression pattern to search for.</param>
  /// <param name="replacement">The replacement for any matches.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.String_replace)]
  public static string replace(object thisob, object regExp, object replacement)
  {
    string thisob1 = Convert.ToString(thisob);
    switch (regExp)
    {
      case RegExpObject regExpObject:
        return StringPrototype.ReplaceWithRegExp(thisob1, regExpObject, replacement);
      case Regex regex:
        return StringPrototype.ReplaceWithRegExp(thisob1, new RegExpObject(regex), replacement);
      default:
        return StringPrototype.ReplaceWithString(thisob1, Convert.ToString(regExp), Convert.ToString(replacement));
    }
  }

  private static string ReplaceWithRegExp(
    string thisob,
    RegExpObject regExpObject,
    object replacement)
  {
    RegExpReplace regExpReplace = replacement is ScriptFunction ? (RegExpReplace) new ReplaceUsingFunction(regExpObject.regex, (ScriptFunction) replacement, thisob) : (RegExpReplace) new Microsoft.JScript.ReplaceWithString(Convert.ToString(replacement));
    MatchEvaluator evaluator = new MatchEvaluator(regExpReplace.Evaluate);
    string str = regExpObject.globalInt ? regExpObject.regex.Replace(thisob, evaluator) : regExpObject.regex.Replace(thisob, evaluator, 1);
    regExpObject.lastIndexInt = (object) (regExpReplace.lastMatch == null ? 0 : regExpObject.regExpConst.UpdateConstructor(regExpObject.regex, regExpReplace.lastMatch, thisob));
    return str;
  }

  private static string ReplaceWithString(string thisob, string searchString, string replaceString)
  {
    int length = thisob.IndexOf(searchString);
    if (length < 0)
      return thisob;
    StringBuilder stringBuilder = new StringBuilder(thisob.Substring(0, length));
    stringBuilder.Append(replaceString);
    stringBuilder.Append(thisob.Substring(length + searchString.Length));
    return stringBuilder.ToString();
  }

  /// <summary>Runs a search for the specified regular expression pattern on the specified object.</summary>
  /// <returns>If the search is successful, the index of the match; otherwise, -1.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  /// <param name="engine">A reference to the scripting engine.</param>
  /// <param name="regExp">The regular expression pattern to search for.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject | JSFunctionAttributeEnum.HasEngine, JSBuiltin.String_search)]
  public static int search(object thisob, VsaEngine engine, object regExp)
  {
    string input = Convert.ToString(thisob);
    RegExpObject regExpObject = StringPrototype.ToRegExpObject(regExp, engine);
    Match match = regExpObject.regex.Match(input);
    if (!match.Success)
    {
      regExpObject.lastIndexInt = (object) 0;
      return -1;
    }
    regExpObject.lastIndexInt = (object) regExpObject.regExpConst.UpdateConstructor(regExpObject.regex, match, input);
    return match.Index;
  }

  /// <summary>Creates a new string that contains the specified consecutive characters from the specified string.</summary>
  /// <returns>A new string that contains the consecutive characters between <paramref name="start" /> and <paramref name="end" />.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  /// <param name="start">The first consecutive character to get.</param>
  /// <param name="end">The last consecutive character to get.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.String_slice)]
  public static string slice(object thisob, double start, object end)
  {
    string str = Convert.ToString(thisob);
    int length1 = str.Length;
    double startIndex = Convert.ToInteger(start);
    double num1;
    switch (end)
    {
      case null:
      case Missing _:
        num1 = (double) length1;
        break;
      default:
        num1 = Convert.ToInteger(end);
        break;
    }
    double num2 = num1;
    if (startIndex < 0.0)
    {
      startIndex = (double) length1 + startIndex;
      if (startIndex < 0.0)
        startIndex = 0.0;
    }
    else if (startIndex > (double) length1)
      startIndex = (double) length1;
    if (num2 < 0.0)
    {
      num2 = (double) length1 + num2;
      if (num2 < 0.0)
        num2 = 0.0;
    }
    else if (num2 > (double) length1)
      num2 = (double) length1;
    int length2 = (int) (num2 - startIndex);
    return length2 <= 0 ? "" : str.Substring((int) startIndex, length2);
  }

  /// <summary>Converts the specified object to text enclosed in smaller text markup tags.</summary>
  /// <returns>A string in the form "&lt;SMALL&gt;"+ <paramref name="thisob" /> +"&lt;/SMALL&gt;".</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.String_small)]
  public static string small(object thisob) => $"<SMALL>{Convert.ToString(thisob)}</SMALL>";

  /// <summary>Returns an array that contains the substrings in the specified object that are delimited by the specified characters. The number of substrings in the array is constrained by the specified limit.</summary>
  /// <returns>An array that contains the substrings in <paramref name="thisob" /> that are delimited by <paramref name="separator" />.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  /// <param name="engine">A reference to the scripting engine.</param>
  /// <param name="separator">The characters that delimit the substrings in <paramref name="thisob" />.</param>
  /// <param name="limit">The maximum number of substrings to return.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject | JSFunctionAttributeEnum.HasEngine, JSBuiltin.String_split)]
  public static ArrayObject split(object thisob, VsaEngine engine, object separator, object limit)
  {
    string thisob1 = Convert.ToString(thisob);
    uint limit1 = uint.MaxValue;
    if (limit != null && !(limit is Missing) && limit != DBNull.Value)
    {
      double integer = Convert.ToInteger(limit);
      if (integer >= 0.0 && integer < (double) uint.MaxValue)
        limit1 = (uint) integer;
    }
    if (limit1 == 0U)
      return engine.GetOriginalArrayConstructor().Construct();
    switch (separator)
    {
      case null:
      case Missing _:
        ArrayObject arrayObject = engine.GetOriginalArrayConstructor().Construct();
        arrayObject.SetValueAtIndex(0U, thisob);
        return arrayObject;
      case RegExpObject regExpObject:
        return StringPrototype.SplitWithRegExp(thisob1, engine, regExpObject, limit1);
      case Regex regex:
        return StringPrototype.SplitWithRegExp(thisob1, engine, new RegExpObject(regex), limit1);
      default:
        return StringPrototype.SplitWithString(thisob1, engine, Convert.ToString(separator), limit1);
    }
  }

  private static ArrayObject SplitWithRegExp(
    string thisob,
    VsaEngine engine,
    RegExpObject regExpObject,
    uint limit)
  {
    ArrayObject arrayObject = engine.GetOriginalArrayConstructor().Construct();
    Match match1 = regExpObject.regex.Match(thisob);
    if (!match1.Success)
    {
      arrayObject.SetValueAtIndex(0U, (object) thisob);
      regExpObject.lastIndexInt = (object) 0;
      return arrayObject;
    }
    int startIndex = 0;
    uint index = 0;
    Match match2;
    do
    {
      int length = match1.Index - startIndex;
      if (length > 0)
      {
        arrayObject.SetValueAtIndex(index++, (object) thisob.Substring(startIndex, length));
        if (limit > 0U && index >= limit)
        {
          regExpObject.lastIndexInt = (object) regExpObject.regExpConst.UpdateConstructor(regExpObject.regex, match1, thisob);
          return arrayObject;
        }
      }
      startIndex = match1.Index + match1.Length;
      match2 = match1;
      match1 = match1.NextMatch();
    }
    while (match1.Success);
    if (startIndex < thisob.Length)
      arrayObject.SetValueAtIndex(index, (object) thisob.Substring(startIndex));
    regExpObject.lastIndexInt = (object) regExpObject.regExpConst.UpdateConstructor(regExpObject.regex, match2, thisob);
    return arrayObject;
  }

  private static ArrayObject SplitWithString(
    string thisob,
    VsaEngine engine,
    string separator,
    uint limit)
  {
    ArrayObject arrayObject = engine.GetOriginalArrayConstructor().Construct();
    if (separator.Length == 0)
    {
      if ((long) limit > (long) thisob.Length)
        limit = (uint) thisob.Length;
      for (int index = 0; (long) index < (long) limit; ++index)
        arrayObject.SetValueAtIndex((uint) index, (object) thisob[index].ToString());
    }
    else
    {
      int startIndex = 0;
      uint index = 0;
      int num;
      for (; (num = thisob.IndexOf(separator, startIndex)) >= 0; startIndex = num + separator.Length)
      {
        arrayObject.SetValueAtIndex(index++, (object) thisob.Substring(startIndex, num - startIndex));
        if (index >= limit)
          return arrayObject;
      }
      if (index == 0U)
        arrayObject.SetValueAtIndex(0U, (object) thisob);
      else
        arrayObject.SetValueAtIndex(index, (object) thisob.Substring(startIndex));
    }
    return arrayObject;
  }

  /// <summary>Converts the specified object to text enclosed in strikethrough markup tags.</summary>
  /// <returns>A string in the form "&lt;STRIKE&gt;"+ <paramref name="thisob" /> +"&lt;/STRIKE&gt;".</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.String_strike)]
  public static string strike(object thisob) => $"<STRIKE>{Convert.ToString(thisob)}</STRIKE>";

  /// <summary>Converts the specified object to text enclosed in subscript markup tags.</summary>
  /// <returns>A string in the form "&lt;SUB&gt;"+ <paramref name="thisob" /> +"&lt;/SUB&gt;".</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.String_sub)]
  public static string sub(object thisob) => $"<SUB>{Convert.ToString(thisob)}</SUB>";

  /// <summary>Creates a new string that contains the specified number of consecutive characters from the specified string, starting from the specified character position. Use <see cref="M:Microsoft.JScript.StringPrototype.substring(System.Object,System.Double,System.Object)" /> instead, if you can.</summary>
  /// <returns>A new string that contains <paramref name="count" /> consecutive characters, starting at <paramref name="start" />.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  /// <param name="start">The first consecutive character to get.</param>
  /// <param name="count">The number of characters to get.</param>
  [NotRecommended("substr")]
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.String_substr)]
  public static string substr(object thisob, double start, object count)
  {
    if (!(thisob is string str))
      str = Convert.ToString(thisob);
    int length1 = str.Length;
    double num1 = Convert.ToInteger(start);
    if (num1 < 0.0)
      num1 += (double) length1;
    if (num1 < 0.0)
      num1 = 0.0;
    else if (num1 > (double) length1)
      num1 = (double) length1;
    int length2;
    if (count is int num2)
      length2 = num2;
    else if (count == null || count is Missing)
      length2 = length1 - (int) Runtime.DoubleToInt64(num1);
    else
      length2 = (int) Runtime.DoubleToInt64(Convert.ToInteger(count));
    if (num1 + (double) length2 > (double) length1)
      length2 = length1 - (int) num1;
    return length2 <= 0 ? "" : str.Substring((int) num1, length2);
  }

  /// <summary>Creates a new string that contains the specified consecutive characters from the specified string.</summary>
  /// <returns>A new string that contains the consecutive characters between <paramref name="start" /> and <paramref name="end" />.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  /// <param name="start">The first consecutive character to get.</param>
  /// <param name="end">The last consecutive character to get.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.String_substring)]
  public static string substring(object thisob, double start, object end)
  {
    if (!(thisob is string str))
      str = Convert.ToString(thisob);
    int length = str.Length;
    double startIndex = Convert.ToInteger(start);
    if (startIndex < 0.0)
      startIndex = 0.0;
    else if (startIndex > (double) length)
      startIndex = (double) length;
    double num1;
    switch (end)
    {
      case null:
      case Missing _:
        num1 = (double) length;
        break;
      default:
        num1 = Convert.ToInteger(end);
        break;
    }
    double num2 = num1;
    if (num2 < 0.0)
      num2 = 0.0;
    else if (num2 > (double) length)
      num2 = (double) length;
    if (startIndex > num2)
    {
      double num3 = startIndex;
      startIndex = num2;
      num2 = num3;
    }
    return str.Substring((int) startIndex, (int) (num2 - startIndex));
  }

  /// <summary>Converts the specified object to text enclosed in superscript markup tags.</summary>
  /// <returns>A string in the form "&lt;SUP&gt;"+ <paramref name="thisob" /> +"&lt;/SUP&gt;".</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.String_sup)]
  public static string sup(object thisob) => $"<SUP>{Convert.ToString(thisob)}</SUP>";

  /// <summary>Returns a copy of the specified string converted to lowercase, based on the casing rules of <see cref="P:System.Globalization.CultureInfo.CurrentUICulture" />.</summary>
  /// <returns>A lowercase string.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.String_toLocaleLowerCase)]
  public static string toLocaleLowerCase(object thisob)
  {
    return Convert.ToString(thisob).ToLower(CultureInfo.CurrentUICulture);
  }

  /// <summary>Returns a copy of the specified string converted to uppercase, based on the casing rules of <see cref="P:System.Globalization.CultureInfo.CurrentUICulture" />.</summary>
  /// <returns>An uppercase string.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.String_toLocaleUpperCase)]
  public static string toLocaleUpperCase(object thisob)
  {
    return Convert.ToString(thisob).ToUpperInvariant();
  }

  /// <summary>Returns a copy of the specified string converted to lowercase, based on the casing rules of <see cref="P:System.Globalization.CultureInfo.InvariantCulture" />.</summary>
  /// <returns>A lowercase string.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.String_toLowerCase)]
  public static string toLowerCase(object thisob) => Convert.ToString(thisob).ToLowerInvariant();

  private static RegExpObject ToRegExpObject(object regExp, VsaEngine engine)
  {
    switch (regExp)
    {
      case null:
      case Missing _:
        return (RegExpObject) engine.GetOriginalRegExpConstructor().Construct("", false, false, false);
      case RegExpObject regExpObject:
        return regExpObject;
      case Regex regex:
        return new RegExpObject(regex);
      default:
        return (RegExpObject) engine.GetOriginalRegExpConstructor().Construct(Convert.ToString(regExp), false, false, false);
    }
  }

  /// <summary>Creates a string representation of the specified object.</summary>
  /// <returns>A string representation of <paramref name="thisob" />.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.String_toString)]
  public static string toString(object thisob)
  {
    switch (thisob)
    {
      case StringObject stringObject:
        return stringObject.value;
      case ConcatString concatString:
        return concatString.ToString();
      default:
        IConvertible iconvertible = Convert.GetIConvertible(thisob);
        return Convert.GetTypeCode(thisob, iconvertible) == TypeCode.String ? iconvertible.ToString((IFormatProvider) null) : throw new JScriptException(JSError.StringExpected);
    }
  }

  /// <summary>Returns a copy of the specified string converted to uppercase, based on the casing rules of <see cref="P:System.Globalization.CultureInfo.InvariantCulture" />.</summary>
  /// <returns>An uppercase string.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.String_toUpperCase)]
  public static string toUpperCase(object thisob) => Convert.ToString(thisob).ToUpperInvariant();

  /// <summary>Returns the value of the specified object.</summary>
  /// <returns>The value of <paramref name="thisob" />.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.String_valueOf)]
  public static object valueOf(object thisob) => (object) StringPrototype.toString(thisob);
}
