// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.GlobalObject
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Stores constants, data type information, utility functions, and references to prototype-based objects. This class belongs to the built-in object model category.</summary>
public class GlobalObject
{
  internal static readonly GlobalObject commonInstance = new GlobalObject();
  /// <summary>Represents positive infinity. This field is constant.</summary>
  public const double Infinity = double.PositiveInfinity;
  /// <summary>Represents a value that is not a number (NaN). This field is constant.</summary>
  public const double NaN = double.NaN;
  /// <summary>Represents a value that is null.</summary>
  public static readonly Empty undefined = (Empty) null;
  /// <summary>Stores a reference to an <see cref="T:Microsoft.JScript.ActiveXObjectConstructor" /> object.</summary>
  protected ActiveXObjectConstructor originalActiveXObjectField;
  /// <summary>Stores a reference to an <see cref="T:Microsoft.JScript.ArrayConstructor" /> object.</summary>
  protected ArrayConstructor originalArrayField;
  /// <summary>Stores a reference to a <see cref="T:Microsoft.JScript.BooleanConstructor" /> object.</summary>
  protected BooleanConstructor originalBooleanField;
  /// <summary>Stores a reference to a <see cref="T:Microsoft.JScript.DateConstructor" /> object.</summary>
  protected DateConstructor originalDateField;
  /// <summary>Stores a reference to an <see cref="T:Microsoft.JScript.EnumeratorConstructor" /> object.</summary>
  protected EnumeratorConstructor originalEnumeratorField;
  /// <summary>Stores a reference to an <see cref="T:Microsoft.JScript.ErrorConstructor" /> object.</summary>
  protected ErrorConstructor originalErrorField;
  /// <summary>Stores a reference to an <see cref="T:Microsoft.JScript.ErrorConstructor" /> object. The object is initialized as an <see cref="F:Microsoft.JScript.ErrorType.EvalError" />.</summary>
  protected ErrorConstructor originalEvalErrorField;
  /// <summary>Stores a reference to a <see cref="T:Microsoft.JScript.FunctionConstructor" /> object.</summary>
  protected FunctionConstructor originalFunctionField;
  /// <summary>Stores a reference to a <see cref="T:Microsoft.JScript.NumberConstructor" /> object.</summary>
  protected NumberConstructor originalNumberField;
  /// <summary>Stores a reference to an <see cref="T:Microsoft.JScript.ObjectConstructor" /> object.</summary>
  protected ObjectConstructor originalObjectField;
  /// <summary>Stores a reference to an <see cref="T:Microsoft.JScript.ObjectPrototype" /> object.</summary>
  protected ObjectPrototype originalObjectPrototypeField;
  /// <summary>Stores a reference to an <see cref="T:Microsoft.JScript.ErrorConstructor" /> object. The object is initialized as a <see cref="F:Microsoft.JScript.ErrorType.RangeError" />.</summary>
  protected ErrorConstructor originalRangeErrorField;
  /// <summary>Stores a reference to an <see cref="T:Microsoft.JScript.ErrorConstructor" /> object. The object is initialized as a <see cref="F:Microsoft.JScript.ErrorType.ReferenceError" />.</summary>
  protected ErrorConstructor originalReferenceErrorField;
  /// <summary>Stores a reference to a <see cref="T:Microsoft.JScript.RegExpConstructor" /> object.</summary>
  protected RegExpConstructor originalRegExpField;
  /// <summary>Stores a reference to a <see cref="T:Microsoft.JScript.StringConstructor" /> object.</summary>
  protected StringConstructor originalStringField;
  /// <summary>Stores a reference to an <see cref="T:Microsoft.JScript.ErrorConstructor" /> object. The object is initialized as a <see cref="F:Microsoft.JScript.ErrorType.SyntaxError" />.</summary>
  protected ErrorConstructor originalSyntaxErrorField;
  /// <summary>Stores a reference to an <see cref="T:Microsoft.JScript.ErrorConstructor" /> object. The object is initialized as a <see cref="F:Microsoft.JScript.ErrorType.TypeError" />.</summary>
  protected ErrorConstructor originalTypeErrorField;
  /// <summary>Stores a reference to a <see cref="T:Microsoft.JScript.VBArrayConstructor" /> object.</summary>
  protected VBArrayConstructor originalVBArrayField;
  /// <summary>Stores a reference to an <see cref="T:Microsoft.JScript.ErrorConstructor" /> object. The object is initialized as a <see cref="F:Microsoft.JScript.ErrorType.URIError" />.</summary>
  protected ErrorConstructor originalURIErrorField;

  internal GlobalObject()
  {
    this.originalActiveXObjectField = (ActiveXObjectConstructor) null;
    this.originalArrayField = (ArrayConstructor) null;
    this.originalBooleanField = (BooleanConstructor) null;
    this.originalDateField = (DateConstructor) null;
    this.originalEnumeratorField = (EnumeratorConstructor) null;
    this.originalEvalErrorField = (ErrorConstructor) null;
    this.originalErrorField = (ErrorConstructor) null;
    this.originalFunctionField = (FunctionConstructor) null;
    this.originalNumberField = (NumberConstructor) null;
    this.originalObjectField = (ObjectConstructor) null;
    this.originalObjectPrototypeField = (ObjectPrototype) null;
    this.originalRangeErrorField = (ErrorConstructor) null;
    this.originalReferenceErrorField = (ErrorConstructor) null;
    this.originalRegExpField = (RegExpConstructor) null;
    this.originalStringField = (StringConstructor) null;
    this.originalSyntaxErrorField = (ErrorConstructor) null;
    this.originalTypeErrorField = (ErrorConstructor) null;
    this.originalVBArrayField = (VBArrayConstructor) null;
    this.originalURIErrorField = (ErrorConstructor) null;
  }

  /// <summary>Gets a reference to an <see cref="T:Microsoft.JScript.ActiveXObjectConstructor" /> object.</summary>
  /// <returns>A reference to an <see cref="T:Microsoft.JScript.ActiveXObjectConstructor" /> object.</returns>
  public static ActiveXObjectConstructor ActiveXObject => ActiveXObjectConstructor.ob;

  private static void AppendInHex(StringBuilder bs, int value)
  {
    bs.Append('%');
    int num1 = value >> 4 & 15;
    bs.Append(num1 >= 10 ? (char) (num1 - 10 + 65) : (char) (num1 + 48 /*0x30*/));
    int num2 = value & 15;
    bs.Append(num2 >= 10 ? (char) (num2 - 10 + 65) : (char) (num2 + 48 /*0x30*/));
  }

  /// <summary>Gets a reference to an <see cref="T:Microsoft.JScript.ArrayConstructor" /> object.</summary>
  /// <returns>A reference to an <see cref="T:Microsoft.JScript.ArrayConstructor" /> object.</returns>
  public static ArrayConstructor Array => ArrayConstructor.ob;

  /// <summary>Gets a reference to a <see cref="T:Microsoft.JScript.BooleanConstructor" /> object.</summary>
  /// <returns>A reference to a <see cref="T:Microsoft.JScript.BooleanConstructor" /> object.</returns>
  public static BooleanConstructor Boolean => BooleanConstructor.ob;

  /// <summary>Gets the managed type that corresponds to the JScript boolean data type.</summary>
  /// <returns>The managed type that corresponds to the JScript boolean data type.</returns>
  public static Type boolean => Typeob.Boolean;

  /// <summary>Gets the managed type that corresponds to the JScript byte data type.</summary>
  /// <returns>The managed type that corresponds to the JScript byte data type.</returns>
  public static Type @byte => Typeob.Byte;

  /// <summary>Gets the managed type that corresponds to the JScript char data type.</summary>
  /// <returns>The managed type that corresponds to the JScript char data type.</returns>
  public static Type @char => Typeob.Char;

  /// <summary>Calls the garbage collector.</summary>
  [JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Global_CollectGarbage)]
  public static void CollectGarbage() => GC.Collect();

  /// <summary>Gets a reference to a <see cref="T:Microsoft.JScript.DateConstructor" /> object.</summary>
  /// <returns>A reference to a <see cref="T:Microsoft.JScript.DateConstructor" /> object.</returns>
  public static DateConstructor Date => DateConstructor.ob;

  /// <summary>Gets the managed type that corresponds to the JScript decimal data type.</summary>
  /// <returns>The managed type that corresponds to the JScript decimal data type.</returns>
  public static Type @decimal => Typeob.Decimal;

  private static string Decode(object encodedURI, GlobalObject.URISetType flags)
  {
    string str = Convert.ToString(encodedURI);
    StringBuilder stringBuilder = new StringBuilder();
    for (int index = 0; index < str.Length; ++index)
    {
      char ch1 = str[index];
      if (ch1 != '%')
      {
        stringBuilder.Append(ch1);
      }
      else
      {
        int startIndex = index;
        if (index + 2 >= str.Length)
          throw new JScriptException(JSError.URIDecodeError);
        byte num1 = GlobalObject.HexValue(str[index + 1], str[index + 2]);
        index += 2;
        char ch2;
        if (((int) num1 & 128 /*0x80*/) == 0)
        {
          ch2 = (char) num1;
        }
        else
        {
          int num2 = 1;
          while (((int) num1 << num2 & 128 /*0x80*/) != 0)
            ++num2;
          if (num2 == 1 || num2 > 4 || index + (num2 - 1) * 3 >= str.Length)
            throw new JScriptException(JSError.URIDecodeError);
          int num3 = (int) num1 & (int) byte.MaxValue >> num2 + 1;
          for (; num2 > 1; --num2)
          {
            byte num4 = str[index + 1] == '%' ? GlobalObject.HexValue(str[index + 2], str[index + 3]) : throw new JScriptException(JSError.URIDecodeError);
            index += 3;
            if (((int) num4 & 192 /*0xC0*/) != 128 /*0x80*/)
              throw new JScriptException(JSError.URIDecodeError);
            num3 = num3 << 6 | (int) num4 & 63 /*0x3F*/;
          }
          if (num3 >= 55296 && num3 < 57344 /*0xE000*/)
            throw new JScriptException(JSError.URIDecodeError);
          if (num3 < 65536 /*0x010000*/)
          {
            ch2 = (char) num3;
          }
          else
          {
            if (num3 > 1114111)
              throw new JScriptException(JSError.URIDecodeError);
            stringBuilder.Append((char) ((num3 - 65536 /*0x010000*/ >> 10 & 1023 /*0x03FF*/) + 55296));
            stringBuilder.Append((char) ((num3 - 65536 /*0x010000*/ & 1023 /*0x03FF*/) + 56320));
            continue;
          }
        }
        if (GlobalObject.InURISet(ch2, flags))
          stringBuilder.Append(str, startIndex, index - startIndex + 1);
        else
          stringBuilder.Append(ch2);
      }
    }
    return stringBuilder.ToString();
  }

  /// <summary>Decodes the specified Uniform Resource Identifier (URI), accounting for reserved characters.</summary>
  /// <returns>The decoded version of <paramref name="encodedURI" />.</returns>
  /// <param name="encodedURI">The URI to decode.</param>
  [JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Global_decodeURI)]
  public static string decodeURI(object encodedURI)
  {
    return GlobalObject.Decode(encodedURI, GlobalObject.URISetType.Reserved);
  }

  /// <summary>Decodes the specified Uniform Resource Identifier (URI).</summary>
  /// <returns>The decoded version of <paramref name="encodedURI" />.</returns>
  /// <param name="encodedURI">The URI to decode.</param>
  [JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Global_decodeURIComponent)]
  public static string decodeURIComponent(object encodedURI)
  {
    return GlobalObject.Decode(encodedURI, GlobalObject.URISetType.None);
  }

  /// <summary>Gets the managed type that corresponds to the JScript double data type.</summary>
  /// <returns>The managed type that corresponds to the JScript double data type.</returns>
  public static Type @double => Typeob.Double;

  private static string Encode(object uri, GlobalObject.URISetType flags)
  {
    string str = Convert.ToString(uri);
    StringBuilder bs = new StringBuilder();
    for (int index = 0; index < str.Length; ++index)
    {
      char ch = str[index];
      if (GlobalObject.InURISet(ch, flags))
      {
        bs.Append(ch);
      }
      else
      {
        int num1 = (int) ch;
        if (num1 >= 0 && num1 <= (int) sbyte.MaxValue)
          GlobalObject.AppendInHex(bs, num1);
        else if (num1 >= 128 /*0x80*/ && num1 <= 2047 /*0x07FF*/)
        {
          GlobalObject.AppendInHex(bs, num1 >> 6 | 192 /*0xC0*/);
          GlobalObject.AppendInHex(bs, num1 & 63 /*0x3F*/ | 128 /*0x80*/);
        }
        else if (num1 < 55296 || num1 > 57343 /*0xDFFF*/)
        {
          GlobalObject.AppendInHex(bs, num1 >> 12 | 224 /*0xE0*/);
          GlobalObject.AppendInHex(bs, num1 >> 6 & 63 /*0x3F*/ | 128 /*0x80*/);
          GlobalObject.AppendInHex(bs, num1 & 63 /*0x3F*/ | 128 /*0x80*/);
        }
        else
        {
          if (num1 >= 56320 && num1 <= 57343 /*0xDFFF*/)
            throw new JScriptException(JSError.URIEncodeError);
          if (++index >= str.Length)
            throw new JScriptException(JSError.URIEncodeError);
          int num2 = (int) str[index];
          if (num2 < 56320 || num2 > 57343 /*0xDFFF*/)
            throw new JScriptException(JSError.URIEncodeError);
          int num3 = (num1 - 55296 << 10) + num2 + 9216;
          GlobalObject.AppendInHex(bs, num3 >> 18 | 240 /*0xF0*/);
          GlobalObject.AppendInHex(bs, num3 >> 12 & 63 /*0x3F*/ | 128 /*0x80*/);
          GlobalObject.AppendInHex(bs, num3 >> 6 & 63 /*0x3F*/ | 128 /*0x80*/);
          GlobalObject.AppendInHex(bs, num3 & 63 /*0x3F*/ | 128 /*0x80*/);
        }
      }
    }
    return bs.ToString();
  }

  /// <summary>Encodes the specified Uniform Resource Identifier (URI), accounting for reserved characters.</summary>
  /// <returns>The encoded version of <paramref name="uri" />.</returns>
  /// <param name="uri">The URI to encode.</param>
  [JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Global_encodeURI)]
  public static string encodeURI(object uri)
  {
    return GlobalObject.Encode(uri, GlobalObject.URISetType.Reserved | GlobalObject.URISetType.Unescaped);
  }

  /// <summary>Encodes the specified Uniform Resource Identifier (URI).</summary>
  /// <returns>The encoded version of <paramref name="uriComponent" />.</returns>
  /// <param name="uriComponent">The URI to encode.</param>
  [JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Global_encodeURIComponent)]
  public static string encodeURIComponent(object uriComponent)
  {
    return GlobalObject.Encode(uriComponent, GlobalObject.URISetType.Unescaped);
  }

  /// <summary>Gets a reference to an <see cref="T:Microsoft.JScript.EnumeratorConstructor" /> object.</summary>
  /// <returns>A reference to an <see cref="T:Microsoft.JScript.EnumeratorConstructor" /> object.</returns>
  public static EnumeratorConstructor Enumerator => EnumeratorConstructor.ob;

  /// <summary>Gets a reference to an <see cref="T:Microsoft.JScript.ErrorConstructor" /> object.</summary>
  /// <returns>A reference to an <see cref="T:Microsoft.JScript.ErrorConstructor" /> object.</returns>
  public static ErrorConstructor Error => ErrorConstructor.ob;

  /// <summary>Converts the specified string to use the % character to escape reserved characters (@, *, _, +, -, ., /) and express them in Unicode notation.</summary>
  /// <returns>A new copy of <paramref name="string" /> in which the reserved characters are escaped by the % character and expressed in Unicode.</returns>
  /// <param name="string">The string to convert.</param>
  [NotRecommended("escape")]
  [JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Global_escape)]
  public static string escape(object @string)
  {
    string str1 = Convert.ToString(@string);
    string str2 = "0123456789ABCDEF";
    int length = str1.Length;
    StringBuilder stringBuilder = new StringBuilder(length * 2);
    int index = -1;
    while (++index < length)
    {
      char ch = str1[index];
      int num = (int) ch;
      if ((65 > num || num > 90) && (97 > num || num > 122) && (48 /*0x30*/ > num || num > 57) && ch != '@' && ch != '*' && ch != '_' && ch != '+' && ch != '-' && ch != '.' && ch != '/')
      {
        stringBuilder.Append('%');
        if (num < 256 /*0x0100*/)
        {
          stringBuilder.Append(str2[num / 16 /*0x10*/]);
          ch = str2[num % 16 /*0x10*/];
        }
        else
        {
          stringBuilder.Append('u');
          stringBuilder.Append(str2[(num >> 12) % 16 /*0x10*/]);
          stringBuilder.Append(str2[(num >> 8) % 16 /*0x10*/]);
          stringBuilder.Append(str2[(num >> 4) % 16 /*0x10*/]);
          ch = str2[num % 16 /*0x10*/];
        }
      }
      stringBuilder.Append(ch);
    }
    return stringBuilder.ToString();
  }

  /// <summary>Throws an <see cref="F:Microsoft.JScript.JSError.IllegalEval" /> exception in all cases.</summary>
  /// <returns>Throws an <see cref="F:Microsoft.JScript.JSError.IllegalEval" /> exception in all cases.</returns>
  /// <param name="x">The expression to evaluate.</param>
  [JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Global_eval)]
  public static object eval(object x) => throw new JScriptException(JSError.IllegalEval);

  /// <summary>Gets a reference to an <see cref="T:Microsoft.JScript.ErrorConstructor" /> object. The object is initialized as an <see cref="F:Microsoft.JScript.ErrorType.EvalError" />.</summary>
  /// <returns>A reference to an <see cref="T:Microsoft.JScript.ErrorConstructor" /> object. The object is initialized as an <see cref="F:Microsoft.JScript.ErrorType.EvalError" />.</returns>
  public static ErrorConstructor EvalError => ErrorConstructor.evalOb;

  /// <summary>Gets the managed type that corresponds to the JScript float data type.</summary>
  /// <returns>The managed type that corresponds to the JScript float data type.</returns>
  public static Type @float => Typeob.Single;

  /// <summary>Gets a reference to a <see cref="T:Microsoft.JScript.FunctionConstructor" /> object.</summary>
  /// <returns>A reference to a <see cref="T:Microsoft.JScript.FunctionConstructor" /> object.</returns>
  public static FunctionConstructor Function => FunctionConstructor.ob;

  /// <summary>Creates a managed object from an unmanaged object by using either the moniker or the ProgID.</summary>
  /// <returns>The managed object.</returns>
  /// <param name="moniker">The moniker of the object.</param>
  /// <param name="progId">The ProgID of the object.</param>
  [JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Global_GetObject)]
  [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
  public static object GetObject(object moniker, object progId)
  {
    moniker = Convert.ToPrimitive(moniker, PreferredType.Either);
    if (!(progId is Missing))
      progId = Convert.ToPrimitive(progId, PreferredType.Either);
    string str = Convert.GetTypeCode(moniker) == TypeCode.String ? moniker.ToString() : (string) null;
    string progID = Convert.GetTypeCode(progId) == TypeCode.String ? progId.ToString() : (string) null;
    switch (str)
    {
      case null:
        throw new JScriptException(JSError.TypeMismatch);
      case "":
        if (progID != null)
          break;
        goto case null;
    }
    if (progID == null && !(progId is Missing))
      throw new JScriptException(JSError.TypeMismatch);
    if (progID != null && progID.Length == 0)
      throw new JScriptException(JSError.InvalidCall);
    if (progID == null || progID.Length == 0)
      return Marshal.BindToMoniker(str);
    if (str == null || str.Length == 0)
      return ActiveObject.GetActiveObject(progID);
    object instance = Activator.CreateInstance(Type.GetTypeFromProgID(progID));
    if (!(instance is UCOMIPersistFile))
      throw new JScriptException(JSError.FileNotFound);
    ((UCOMIPersistFile) instance).Load(str, 0);
    return instance;
  }

  internal static int HexDigit(char c)
  {
    if (c >= '0' && c <= '9')
      return (int) c - 48 /*0x30*/;
    if (c >= 'A' && c <= 'F')
      return 10 + (int) c - 65;
    return c >= 'a' && c <= 'f' ? 10 + (int) c - 97 : -1;
  }

  private static byte HexValue(char ch1, char ch2)
  {
    int num1;
    int num2;
    if ((num1 = GlobalObject.HexDigit(ch1)) < 0 || (num2 = GlobalObject.HexDigit(ch2)) < 0)
      throw new JScriptException(JSError.URIDecodeError);
    return (byte) (num1 << 4 | num2);
  }

  /// <summary>Gets the managed type that corresponds to the JScript int data type.</summary>
  /// <returns>The managed type that corresponds to the JScript int data type.</returns>
  public static Type @int => Typeob.Int32;

  private static bool InURISet(char ch, GlobalObject.URISetType flags)
  {
    if ((flags & GlobalObject.URISetType.Unescaped) != GlobalObject.URISetType.None)
    {
      if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'Z' || ch >= 'a' && ch <= 'z')
        return true;
      if (ch <= '.')
      {
        if (ch != '!')
        {
          switch ((int) ch - 39)
          {
            case 0:
            case 1:
            case 2:
            case 3:
            case 6:
            case 7:
              break;
            default:
              goto label_8;
          }
        }
      }
      else if (ch != '_' && ch != '~')
        goto label_8;
      return true;
    }
label_8:
    if ((flags & GlobalObject.URISetType.Reserved) != GlobalObject.URISetType.None)
    {
      switch (ch)
      {
        case '#':
        case '$':
        case '&':
        case '+':
        case ',':
        case '/':
        case ':':
        case ';':
        case '=':
        case '?':
        case '@':
          return true;
      }
    }
    return false;
  }

  /// <summary>Determines whether the specified number can be converted to a JScript number.</summary>
  /// <returns>true if <paramref name="num" /> can be converted to a JScript number; otherwise, false.</returns>
  /// <param name="num">The number to check.</param>
  [JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Global_isNaN)]
  public static bool isNaN(object num)
  {
    double number = Convert.ToNumber(num);
    return number != number;
  }

  /// <summary>Determines whether the specified number represents infinity.</summary>
  /// <returns>true if <paramref name="number" /> is not <see cref="F:System.Double.NaN" />, not <see cref="F:System.Double.PositiveInfinity" />, and not <see cref="F:System.Double.NegativeInfinity" />; otherwise, false.</returns>
  /// <param name="number">The number to check.</param>
  [JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Global_isFinite)]
  public static bool isFinite(double number) => !double.IsInfinity(number) && !double.IsNaN(number);

  /// <summary>Gets the managed type that corresponds to the JScript long data type.</summary>
  /// <returns>The managed type that corresponds to the JScript long data type.</returns>
  public static Type @long => Typeob.Int64;

  /// <summary>Gets a reference to a <see cref="T:Microsoft.JScript.MathObject" /> object.</summary>
  /// <returns>A reference to a <see cref="T:Microsoft.JScript.MathObject" /> object.</returns>
  public static MathObject Math
  {
    get
    {
      if (MathObject.ob == null)
        MathObject.ob = new MathObject((ScriptObject) ObjectPrototype.ob);
      return MathObject.ob;
    }
  }

  /// <summary>Gets a reference to a <see cref="T:Microsoft.JScript.NumberConstructor" /> object.</summary>
  /// <returns>A reference to a <see cref="T:Microsoft.JScript.NumberConstructor" /> object.</returns>
  public static NumberConstructor Number => NumberConstructor.ob;

  /// <summary>Gets a reference to an <see cref="T:Microsoft.JScript.ObjectConstructor" /> object.</summary>
  /// <returns>A reference to an <see cref="T:Microsoft.JScript.ObjectConstructor" /> object.</returns>
  public static ObjectConstructor Object => ObjectConstructor.ob;

  internal virtual ActiveXObjectConstructor originalActiveXObject
  {
    get
    {
      if (this.originalActiveXObjectField == null)
        this.originalActiveXObjectField = ActiveXObjectConstructor.ob;
      return this.originalActiveXObjectField;
    }
  }

  internal virtual ArrayConstructor originalArray
  {
    get
    {
      if (this.originalArrayField == null)
        this.originalArrayField = ArrayConstructor.ob;
      return this.originalArrayField;
    }
  }

  internal virtual BooleanConstructor originalBoolean
  {
    get
    {
      if (this.originalBooleanField == null)
        this.originalBooleanField = BooleanConstructor.ob;
      return this.originalBooleanField;
    }
  }

  internal virtual DateConstructor originalDate
  {
    get
    {
      if (this.originalDateField == null)
        this.originalDateField = DateConstructor.ob;
      return this.originalDateField;
    }
  }

  internal virtual EnumeratorConstructor originalEnumerator
  {
    get
    {
      if (this.originalEnumeratorField == null)
        this.originalEnumeratorField = EnumeratorConstructor.ob;
      return this.originalEnumeratorField;
    }
  }

  internal virtual ErrorConstructor originalError
  {
    get
    {
      if (this.originalErrorField == null)
        this.originalErrorField = ErrorConstructor.ob;
      return this.originalErrorField;
    }
  }

  internal virtual ErrorConstructor originalEvalError
  {
    get
    {
      if (this.originalEvalErrorField == null)
        this.originalEvalErrorField = ErrorConstructor.evalOb;
      return this.originalEvalErrorField;
    }
  }

  internal virtual FunctionConstructor originalFunction
  {
    get
    {
      if (this.originalFunctionField == null)
        this.originalFunctionField = FunctionConstructor.ob;
      return this.originalFunctionField;
    }
  }

  internal virtual NumberConstructor originalNumber
  {
    get
    {
      if (this.originalNumberField == null)
        this.originalNumberField = NumberConstructor.ob;
      return this.originalNumberField;
    }
  }

  internal virtual ObjectConstructor originalObject
  {
    get
    {
      if (this.originalObjectField == null)
        this.originalObjectField = ObjectConstructor.ob;
      return this.originalObjectField;
    }
  }

  internal virtual ObjectPrototype originalObjectPrototype
  {
    get
    {
      if (this.originalObjectPrototypeField == null)
        this.originalObjectPrototypeField = ObjectPrototype.ob;
      return this.originalObjectPrototypeField;
    }
  }

  internal virtual ErrorConstructor originalRangeError
  {
    get
    {
      if (this.originalRangeErrorField == null)
        this.originalRangeErrorField = ErrorConstructor.rangeOb;
      return this.originalRangeErrorField;
    }
  }

  internal virtual ErrorConstructor originalReferenceError
  {
    get
    {
      if (this.originalReferenceErrorField == null)
        this.originalReferenceErrorField = ErrorConstructor.referenceOb;
      return this.originalReferenceErrorField;
    }
  }

  internal virtual RegExpConstructor originalRegExp
  {
    get
    {
      if (this.originalRegExpField == null)
        this.originalRegExpField = RegExpConstructor.ob;
      return this.originalRegExpField;
    }
  }

  internal virtual StringConstructor originalString
  {
    get
    {
      if (this.originalStringField == null)
        this.originalStringField = StringConstructor.ob;
      return this.originalStringField;
    }
  }

  internal virtual ErrorConstructor originalSyntaxError
  {
    get
    {
      if (this.originalSyntaxErrorField == null)
        this.originalSyntaxErrorField = ErrorConstructor.syntaxOb;
      return this.originalSyntaxErrorField;
    }
  }

  internal virtual ErrorConstructor originalTypeError
  {
    get
    {
      if (this.originalTypeErrorField == null)
        this.originalTypeErrorField = ErrorConstructor.typeOb;
      return this.originalTypeErrorField;
    }
  }

  internal virtual ErrorConstructor originalURIError
  {
    get
    {
      if (this.originalURIErrorField == null)
        this.originalURIErrorField = ErrorConstructor.uriOb;
      return this.originalURIErrorField;
    }
  }

  internal virtual VBArrayConstructor originalVBArray
  {
    get
    {
      if (this.originalVBArrayField == null)
        this.originalVBArrayField = VBArrayConstructor.ob;
      return this.originalVBArrayField;
    }
  }

  /// <summary>Converts the specified string to its <see cref="T:System.Double" /> equivalent.</summary>
  /// <returns>A value equivalent to the value that is contained in <paramref name="string" />.</returns>
  /// <param name="string">The string to convert.</param>
  [JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Global_parseFloat)]
  public static double parseFloat(object @string)
  {
    return Convert.ToNumber(Convert.ToString(@string), false, false, (object) Missing.Value);
  }

  /// <summary>Converts the specified string to its <see cref="T:System.Double" /> equivalent, using the specified base.</summary>
  /// <returns>A value that is equivalent to the value that is contained in <paramref name="string" />.</returns>
  /// <param name="string">The string to convert.</param>
  /// <param name="radix">A number between 2 and 36 that represents the base of the number system that <paramref name="string" /> is expressed in. The default is 10.</param>
  [JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Global_parseInt)]
  public static double parseInt(object @string, object radix)
  {
    return Convert.ToNumber(Convert.ToString(@string), true, true, radix);
  }

  /// <summary>Gets a reference to an <see cref="T:Microsoft.JScript.ErrorConstructor" /> object. The object is initialized as a <see cref="F:Microsoft.JScript.ErrorType.RangeError" />.</summary>
  /// <returns>A reference to an <see cref="T:Microsoft.JScript.ErrorConstructor" /> object. The object is initialized as a <see cref="F:Microsoft.JScript.ErrorType.RangeError" />.</returns>
  public static ErrorConstructor RangeError => ErrorConstructor.rangeOb;

  /// <summary>Gets a reference to an <see cref="T:Microsoft.JScript.ErrorConstructor" /> object. The object is initialized as a <see cref="F:Microsoft.JScript.ErrorType.ReferenceError" />.</summary>
  /// <returns>A reference to an <see cref="T:Microsoft.JScript.ErrorConstructor" /> object. The object is initialized as a <see cref="F:Microsoft.JScript.ErrorType.ReferenceError" />.</returns>
  public static ErrorConstructor ReferenceError => ErrorConstructor.referenceOb;

  /// <summary>Gets a reference to a <see cref="T:Microsoft.JScript.RegExpConstructor" /> object.</summary>
  /// <returns>A reference to a <see cref="T:Microsoft.JScript.RegExpConstructor" /> object.</returns>
  public static RegExpConstructor RegExp => RegExpConstructor.ob;

  /// <summary>Gets the name of the scripting engine.</summary>
  /// <returns>The string literal "JScript".</returns>
  [JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Global_ScriptEngine)]
  public static string ScriptEngine() => "JScript";

  /// <summary>Gets the build number of the scripting engine.</summary>
  /// <returns>The build number of the scripting engine.</returns>
  [JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Global_ScriptEngineBuildVersion)]
  public static int ScriptEngineBuildVersion() => 9222;

  /// <summary>Gets the major version number of the scripting engine.</summary>
  /// <returns>The major version number of the scripting engine.</returns>
  [JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Global_ScriptEngineMajorVersion)]
  public static int ScriptEngineMajorVersion() => 14;

  /// <summary>Gets the minor version number of the scripting engine.</summary>
  /// <returns>The minor version number of the scripting engine.</returns>
  [JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Global_ScriptEngineMinorVersion)]
  public static int ScriptEngineMinorVersion() => 0;

  /// <summary>Gets the managed type that corresponds to the JScript sbyte data type.</summary>
  /// <returns>The managed type that corresponds to the JScript sbyte data type.</returns>
  public static Type @sbyte => Typeob.SByte;

  /// <summary>Gets the managed type that corresponds to the JScript short data type.</summary>
  /// <returns>The managed type that corresponds to the JScript short data type.</returns>
  public static Type @short => Typeob.Int16;

  /// <summary>Gets a reference to a <see cref="T:Microsoft.JScript.StringConstructor" /> object.</summary>
  /// <returns>A reference to a <see cref="T:Microsoft.JScript.StringConstructor" /> object.</returns>
  public static StringConstructor String => StringConstructor.ob;

  /// <summary>Gets a reference to an <see cref="T:Microsoft.JScript.ErrorConstructor" /> object. The object is initialized as a <see cref="F:Microsoft.JScript.ErrorType.SyntaxError" />.</summary>
  /// <returns>A reference to an <see cref="T:Microsoft.JScript.ErrorConstructor" /> object. The object is initialized as a <see cref="F:Microsoft.JScript.ErrorType.SyntaxError" />.</returns>
  public static ErrorConstructor SyntaxError => ErrorConstructor.syntaxOb;

  /// <summary>Gets a reference to an <see cref="T:Microsoft.JScript.ErrorConstructor" /> object. The object is initialized as a <see cref="F:Microsoft.JScript.ErrorType.TypeError" />.</summary>
  /// <returns>A reference to an <see cref="T:Microsoft.JScript.ErrorConstructor" /> object. The object is initialized as a <see cref="F:Microsoft.JScript.ErrorType.TypeError" />.</returns>
  public static ErrorConstructor TypeError => ErrorConstructor.typeOb;

  /// <summary>Converts % escaped characters (@, *, _, +, -, ., /) in the specified string to their original form. The escaped characters are expressed in Unicode notation.</summary>
  /// <returns>A new copy of <paramref name="string" /> in which the escaped characters are converted to their original form.</returns>
  /// <param name="string">The string to convert.</param>
  [NotRecommended("unescape")]
  [JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Global_unescape)]
  public static string unescape(object @string)
  {
    string str = Convert.ToString(@string);
    int length = str.Length;
    StringBuilder stringBuilder = new StringBuilder(length);
    int index = -1;
    while (++index < length)
    {
      char ch = str[index];
      if (ch == '%')
      {
        int num1;
        int num2;
        int num3;
        int num4;
        if (index + 5 < length && str[index + 1] == 'u' && (num1 = GlobalObject.HexDigit(str[index + 2])) != -1 && (num2 = GlobalObject.HexDigit(str[index + 3])) != -1 && (num3 = GlobalObject.HexDigit(str[index + 4])) != -1 && (num4 = GlobalObject.HexDigit(str[index + 5])) != -1)
        {
          ch = (char) ((num1 << 12) + (num2 << 8) + (num3 << 4) + num4);
          index += 5;
        }
        else
        {
          int num5;
          int num6;
          if (index + 2 < length && (num5 = GlobalObject.HexDigit(str[index + 1])) != -1 && (num6 = GlobalObject.HexDigit(str[index + 2])) != -1)
          {
            ch = (char) ((num5 << 4) + num6);
            index += 2;
          }
        }
      }
      stringBuilder.Append(ch);
    }
    return stringBuilder.ToString();
  }

  /// <summary>Gets a reference to an <see cref="T:Microsoft.JScript.ErrorConstructor" /> object. The object is initialized as a <see cref="F:Microsoft.JScript.ErrorType.URIError" />.</summary>
  /// <returns>A reference to an <see cref="T:Microsoft.JScript.ErrorConstructor" /> object. The object is initialized as a <see cref="F:Microsoft.JScript.ErrorType.URIError" />.</returns>
  public static ErrorConstructor URIError => ErrorConstructor.uriOb;

  /// <summary>Gets a reference to a <see cref="T:Microsoft.JScript.VBArrayConstructor" /> object.</summary>
  /// <returns>A reference to a <see cref="T:Microsoft.JScript.VBArrayConstructor" /> object.</returns>
  public static VBArrayConstructor VBArray => VBArrayConstructor.ob;

  /// <summary>Gets the managed type that corresponds to the return value of a function that does not return a value.</summary>
  /// <returns>The managed type that corresponds to the return value of a function that does not return a value.</returns>
  public static Type @void => Typeob.Void;

  /// <summary>Gets the managed type that corresponds to the JScript uint data type.</summary>
  /// <returns>The managed type that corresponds to the JScript uint data type.</returns>
  public static Type @uint => Typeob.UInt32;

  /// <summary>Gets the managed type that corresponds to the JScript ulong data type.</summary>
  /// <returns>The managed type that corresponds to the JScript ulong data type.</returns>
  public static Type @ulong => Typeob.UInt64;

  /// <summary>Gets the managed type that corresponds to the JScript ushort data type.</summary>
  /// <returns>The managed type that corresponds to the JScript ushort data type.</returns>
  public static Type @ushort => Typeob.UInt16;

  private enum URISetType
  {
    None,
    Reserved,
    Unescaped,
  }
}
