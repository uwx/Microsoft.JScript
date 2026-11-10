// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.JSScanner
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using Microsoft.JScript.Vsa;
using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Text;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Scans JScript code searching for individual units of code, or tokens. This class belongs to the scanning and parsing category. For more information about scanning and parsing, see Create a Language Compiler for the .NET Framework.</summary>
public sealed class JSScanner
{
  private string strSourceCode;
  private int startPos;
  private int endPos;
  private int currentPos;
  private int currentLine;
  private int startLinePos;
  private Context currentToken;
  private string escapedString;
  private StringBuilder identifier;
  private int idLastPosOnBuilder;
  private bool gotEndOfLine;
  private bool IsAuthoring;
  private bool peekModeOn;
  private bool scanForDebugger;
  private JSKeyword[] keywords;
  private static readonly JSKeyword[] s_Keywords = JSKeyword.InitKeywords();
  private bool preProcessorOn;
  private int matchIf;
  private object preProcessorValue;
  private SimpleHashtable ppTable;
  private DocumentContext currentDocument;
  private Globals globals;
  private static readonly OpPrec[] s_OperatorsPrec = JSScanner.InitOperatorsPrec();
  private static readonly OpPrec[] s_PPOperatorsPrec = JSScanner.InitPPOperatorsPrec();

  /// <summary>Initializes a new instance of the <see cref="T:Microsoft.JScript.JSScanner" /> class.</summary>
  public JSScanner()
  {
    this.keywords = JSScanner.s_Keywords;
    this.strSourceCode = (string) null;
    this.startPos = 0;
    this.endPos = 0;
    this.currentPos = 0;
    this.currentLine = 1;
    this.startLinePos = 0;
    this.currentToken = (Context) null;
    this.escapedString = (string) null;
    this.identifier = new StringBuilder(128 /*0x80*/);
    this.idLastPosOnBuilder = 0;
    this.gotEndOfLine = false;
    this.IsAuthoring = false;
    this.peekModeOn = false;
    this.preProcessorOn = false;
    this.matchIf = 0;
    this.ppTable = (SimpleHashtable) null;
    this.currentDocument = (DocumentContext) null;
    this.globals = (Globals) null;
    this.scanForDebugger = false;
  }

  /// <summary>Initializes a new instance of the <see cref="T:Microsoft.JScript.JSScanner" /> class, specifying the source code to scan.</summary>
  /// <param name="sourceContext">The source code to scan.</param>
  public JSScanner(Context sourceContext)
  {
    this.IsAuthoring = false;
    this.peekModeOn = false;
    this.keywords = JSScanner.s_Keywords;
    this.preProcessorOn = false;
    this.matchIf = 0;
    this.ppTable = (SimpleHashtable) null;
    this.SetSource(sourceContext);
    this.currentDocument = (DocumentContext) null;
    this.globals = sourceContext.document.engine.Globals;
  }

  /// <summary>Sets a flag that indicates whether the scanner is operating in authoring mode.</summary>
  /// <param name="mode">true to indicate authoring mode; otherwise, false.</param>
  public void SetAuthoringMode(bool mode) => this.IsAuthoring = mode;

  /// <summary>Sets the source code to scan.</summary>
  /// <param name="sourceContext">The source code to scan.</param>
  public void SetSource(Context sourceContext)
  {
    this.strSourceCode = sourceContext.source_string;
    this.startPos = sourceContext.startPos;
    this.startLinePos = sourceContext.startLinePos;
    this.endPos = 0 >= sourceContext.endPos || sourceContext.endPos >= this.strSourceCode.Length ? this.strSourceCode.Length : sourceContext.endPos;
    this.currentToken = sourceContext;
    this.escapedString = (string) null;
    this.identifier = new StringBuilder(128 /*0x80*/);
    this.idLastPosOnBuilder = 0;
    this.currentPos = this.startPos;
    this.currentLine = sourceContext.lineNumber > 0 ? sourceContext.lineNumber : 1;
    this.gotEndOfLine = false;
    this.scanForDebugger = sourceContext.document != null && sourceContext.document.engine != null && VsaEngine.executeForJSEE;
  }

  internal JSToken PeekToken()
  {
    int currentPos = this.currentPos;
    int currentLine = this.currentLine;
    int startLinePos = this.startLinePos;
    bool gotEndOfLine = this.gotEndOfLine;
    int lastPosOnBuilder = this.idLastPosOnBuilder;
    this.peekModeOn = true;
    Context currentToken = this.currentToken;
    this.currentToken = this.currentToken.Clone();
    try
    {
      this.GetNextToken();
      return this.currentToken.token;
    }
    finally
    {
      this.currentToken = currentToken;
      this.currentPos = currentPos;
      this.currentLine = currentLine;
      this.startLinePos = startLinePos;
      this.gotEndOfLine = gotEndOfLine;
      this.identifier.Length = 0;
      this.idLastPosOnBuilder = lastPosOnBuilder;
      this.peekModeOn = false;
      this.escapedString = (string) null;
    }
  }

  /// <summary>Scans for the next token in the code.</summary>
public void GetNextToken()
{
  JSToken jSToken = JSToken.None;
  gotEndOfLine = false;
  try
  {
    int num = currentLine;
    while (true)
    {
      SkipBlanks();
      currentToken.startPos = currentPos;
      currentToken.lineNumber = currentLine;
      currentToken.startLinePos = startLinePos;
      char c = GetChar(currentPos++);
      switch (c)
      {
      case '\0':
        if (currentPos < endPos)
        {
          continue;
        }
        currentPos--;
        jSToken = JSToken.EndOfFile;
        if (matchIf > 0)
        {
          currentToken.endLineNumber = currentLine;
          currentToken.endLinePos = startLinePos;
          currentToken.endPos = currentPos;
          HandleError(JSError.NoCcEnd);
        }
        break;
      case '=':
        jSToken = JSToken.Assign;
        if ('=' == GetChar(currentPos))
        {
          currentPos++;
          jSToken = JSToken.Equal;
          if ('=' == GetChar(currentPos))
          {
            currentPos++;
            jSToken = JSToken.StrictEqual;
          }
        }
        break;
      case '>':
        jSToken = JSToken.GreaterThan;
        if ('>' == GetChar(currentPos))
        {
          currentPos++;
          jSToken = JSToken.RightShift;
          if ('>' == GetChar(currentPos))
          {
            currentPos++;
            jSToken = JSToken.UnsignedRightShift;
          }
        }
        if ('=' == GetChar(currentPos))
        {
          currentPos++;
          switch (jSToken)
          {
          case JSToken.GreaterThan:
            jSToken = JSToken.GreaterThanEqual;
            break;
          case JSToken.RightShift:
            jSToken = JSToken.RightShiftAssign;
            break;
          case JSToken.UnsignedRightShift:
            jSToken = JSToken.UnsignedRightShiftAssign;
            break;
          }
        }
        break;
      case '<':
        jSToken = JSToken.LessThan;
        if ('<' == GetChar(currentPos))
        {
          currentPos++;
          jSToken = JSToken.LeftShift;
        }
        if ('=' == GetChar(currentPos))
        {
          currentPos++;
          jSToken = ((jSToken != JSToken.LessThan) ? JSToken.LeftShiftAssign : JSToken.LessThanEqual);
        }
        break;
      case '!':
        jSToken = JSToken.FirstOp;
        if ('=' == GetChar(currentPos))
        {
          currentPos++;
          jSToken = JSToken.NotEqual;
          if ('=' == GetChar(currentPos))
          {
            currentPos++;
            jSToken = JSToken.StrictNotEqual;
          }
        }
        break;
      case ',':
        jSToken = JSToken.Comma;
        break;
      case '~':
        jSToken = JSToken.BitwiseNot;
        break;
      case '?':
        jSToken = JSToken.ConditionalIf;
        break;
      case ':':
        jSToken = JSToken.Colon;
        if (':' == GetChar(currentPos))
        {
          currentPos++;
          jSToken = JSToken.DoubleColon;
        }
        break;
      case '.':
        jSToken = JSToken.AccessField;
        c = GetChar(currentPos);
        if (IsDigit(c))
        {
          jSToken = ScanNumber('.');
        }
        else if ('.' == c)
        {
          c = GetChar(currentPos + 1);
          if ('.' == c)
          {
            currentPos += 2;
            jSToken = JSToken.ParamArray;
          }
        }
        break;
      case '&':
        jSToken = JSToken.BitwiseAnd;
        c = GetChar(currentPos);
        if ('&' == c)
        {
          currentPos++;
          jSToken = JSToken.LogicalAnd;
        }
        else if ('=' == c)
        {
          currentPos++;
          jSToken = JSToken.BitwiseAndAssign;
        }
        break;
      case '|':
        jSToken = JSToken.BitwiseOr;
        c = GetChar(currentPos);
        if ('|' == c)
        {
          currentPos++;
          jSToken = JSToken.LogicalOr;
        }
        else if ('=' == c)
        {
          currentPos++;
          jSToken = JSToken.BitwiseOrAssign;
        }
        break;
      case '+':
        jSToken = JSToken.FirstBinaryOp;
        c = GetChar(currentPos);
        if ('+' == c)
        {
          currentPos++;
          jSToken = JSToken.Increment;
        }
        else if ('=' == c)
        {
          currentPos++;
          jSToken = JSToken.PlusAssign;
        }
        break;
      case '-':
        jSToken = JSToken.Minus;
        c = GetChar(currentPos);
        if ('-' == c)
        {
          currentPos++;
          jSToken = JSToken.Decrement;
        }
        else if ('=' == c)
        {
          currentPos++;
          jSToken = JSToken.MinusAssign;
        }
        break;
      case '*':
        jSToken = JSToken.Multiply;
        if ('=' == GetChar(currentPos))
        {
          currentPos++;
          jSToken = JSToken.MultiplyAssign;
        }
        break;
      case '\\':
        currentPos--;
        if (IsIdentifierStartChar(ref c))
        {
          currentPos++;
          ScanIdentifier();
          jSToken = JSToken.Identifier;
          break;
        }
        currentPos++;
        c = GetChar(currentPos);
        if ('a' <= c && c <= 'z')
        {
          JSKeyword jSKeyword2 = keywords[c - 97];
          if (jSKeyword2 != null)
          {
            currentToken.startPos++;
            jSToken = ScanKeyword(jSKeyword2);
            if (jSToken != JSToken.Identifier)
            {
              jSToken = JSToken.Identifier;
              break;
            }
            currentToken.startPos--;
          }
        }
        currentPos = currentToken.startPos + 1;
        HandleError(JSError.IllegalChar);
        break;
      case '/':
      {
        jSToken = JSToken.Divide;
        c = GetChar(currentPos);
        bool flag = false;
        switch (c)
        {
        case '/':
          if (GetChar(++currentPos) == '@' && !peekModeOn)
          {
            if (preProcessorOn)
            {
              if (IsBlankSpace(GetChar(++currentPos)))
              {
                continue;
              }
              flag = true;
              break;
            }
            if ('c' == GetChar(++currentPos) && 'c' == GetChar(++currentPos) && '_' == GetChar(++currentPos) && 'o' == GetChar(++currentPos) && 'n' == GetChar(++currentPos))
            {
              char char2 = GetChar(currentPos + 1);
              if (!IsDigit(char2) && !IsAsciiLetter(char2) && !IsUnicodeLetter(char2))
              {
                SetPreProcessorOn();
                currentPos++;
                continue;
              }
            }
          }
          SkipSingleLineComment();
          if (!IsAuthoring)
          {
            continue;
          }
          jSToken = JSToken.Comment;
          break;
        case '*':
          if (GetChar(++currentPos) == '@' && !peekModeOn)
          {
            if (preProcessorOn)
            {
              if (IsBlankSpace(GetChar(++currentPos)))
              {
                continue;
              }
              flag = true;
              break;
            }
            if ('c' == GetChar(++currentPos) && 'c' == GetChar(++currentPos) && '_' == GetChar(++currentPos) && 'o' == GetChar(++currentPos) && 'n' == GetChar(++currentPos))
            {
              char @char = GetChar(currentPos + 1);
              if (!IsDigit(@char) && !IsAsciiLetter(@char) && !IsUnicodeLetter(@char))
              {
                SetPreProcessorOn();
                currentPos++;
                continue;
              }
            }
          }
          SkipMultiLineComment();
          if (!IsAuthoring)
          {
            continue;
          }
          if (currentPos > endPos)
          {
            jSToken = JSToken.UnterminatedComment;
            currentPos = endPos;
          }
          else
          {
            jSToken = JSToken.Comment;
          }
          break;
        case '=':
          currentPos++;
          jSToken = JSToken.DivideAssign;
          break;
        }
        if (!flag)
        {
          break;
        }
        goto case '@';
      }
      case '^':
        jSToken = JSToken.BitwiseXor;
        if ('=' == GetChar(currentPos))
        {
          currentPos++;
          jSToken = JSToken.BitwiseXorAssign;
        }
        break;
      case '%':
        jSToken = JSToken.Modulo;
        if ('=' == GetChar(currentPos))
        {
          currentPos++;
          jSToken = JSToken.ModuloAssign;
        }
        break;
      case '(':
        jSToken = JSToken.LeftParen;
        break;
      case ')':
        jSToken = JSToken.RightParen;
        break;
      case '{':
        jSToken = JSToken.LeftCurly;
        break;
      case '}':
        jSToken = JSToken.RightCurly;
        break;
      case '[':
        jSToken = JSToken.LeftBracket;
        break;
      case ']':
        jSToken = JSToken.RightBracket;
        break;
      case ';':
        jSToken = JSToken.Semicolon;
        break;
      case '"':
      case '\'':
        jSToken = JSToken.StringLiteral;
        ScanString(c);
        break;
      case '\r':
        if (GetChar(currentPos) == '\n')
        {
          currentPos++;
        }
        currentLine++;
        startLinePos = currentPos;
        continue;
      case '\n':
        currentLine++;
        startLinePos = currentPos;
        continue;
      case '\u2028':
        currentLine++;
        startLinePos = currentPos;
        continue;
      case '\u2029':
        currentLine++;
        startLinePos = currentPos;
        continue;
      case '@':
      {
        if (scanForDebugger)
        {
          HandleError(JSError.CcInvalidInDebugger);
        }
        if (peekModeOn)
        {
          currentToken.token = JSToken.PreProcessDirective;
          break;
        }
        int num2 = currentPos;
        currentToken.startPos = num2;
        currentToken.lineNumber = currentLine;
        currentToken.startLinePos = startLinePos;
        ScanIdentifier();
        switch (currentPos - num2)
        {
        case 0:
          if (preProcessorOn && '*' == GetChar(currentPos) && '/' == GetChar(++currentPos))
          {
            currentPos++;
          }
          else
          {
            HandleError(JSError.IllegalChar);
          }
          continue;
        case 2:
          if ('i' == strSourceCode[num2] && 'f' == strSourceCode[num2 + 1])
          {
            if (!preProcessorOn)
            {
              SetPreProcessorOn();
            }
            matchIf++;
            if (!PPTestCond())
            {
              PPSkipToNextCondition(checkCondition: true);
            }
            continue;
          }
          break;
        case 3:
          if ('s' == strSourceCode[num2] && 'e' == strSourceCode[num2 + 1] && 't' == strSourceCode[num2 + 2])
          {
            if (!preProcessorOn)
            {
              SetPreProcessorOn();
            }
            PPScanSet();
            continue;
          }
          if ('e' == strSourceCode[num2] && 'n' == strSourceCode[num2 + 1] && 'd' == strSourceCode[num2 + 2])
          {
            if (0 >= matchIf)
            {
              HandleError(JSError.CcInvalidEnd);
            }
            else
            {
              matchIf--;
            }
            continue;
          }
          break;
        case 4:
          if ('e' == strSourceCode[num2] && 'l' == strSourceCode[num2 + 1] && 's' == strSourceCode[num2 + 2] && 'e' == strSourceCode[num2 + 3])
          {
            if (0 >= matchIf)
            {
              HandleError(JSError.CcInvalidElse);
            }
            else
            {
              PPSkipToNextCondition(checkCondition: false);
            }
            continue;
          }
          if ('e' == strSourceCode[num2] && 'l' == strSourceCode[num2 + 1] && 'i' == strSourceCode[num2 + 2] && 'f' == strSourceCode[num2 + 3])
          {
            if (0 >= matchIf)
            {
              HandleError(JSError.CcInvalidElif);
            }
            else
            {
              PPSkipToNextCondition(checkCondition: false);
            }
            continue;
          }
          break;
        case 5:
          if ('c' == GetChar(num2) && 'c' == GetChar(num2 + 1) && '_' == GetChar(num2 + 2) && 'o' == GetChar(num2 + 3) && 'n' == GetChar(num2 + 4))
          {
            if (!preProcessorOn)
            {
              SetPreProcessorOn();
            }
            continue;
          }
          break;
        }
        if (!preProcessorOn)
        {
          HandleError(JSError.CcOff);
          continue;
        }
        object obj = ppTable[strSourceCode.Substring(num2, currentPos - num2)];
        if (obj == null)
        {
          preProcessorValue = double.NaN;
        }
        else
        {
          preProcessorValue = obj;
        }
        jSToken = JSToken.PreProcessorConstant;
        break;
      }
      case '$':
      case '_':
        ScanIdentifier();
        jSToken = JSToken.Identifier;
        break;
      default:
        if ('a' <= c && c <= 'z')
        {
          JSKeyword jSKeyword = keywords[c - 97];
          if (jSKeyword != null)
          {
            jSToken = ScanKeyword(jSKeyword);
            break;
          }
          jSToken = JSToken.Identifier;
          ScanIdentifier();
          break;
        }
        if (IsDigit(c))
        {
          jSToken = ScanNumber(c);
          break;
        }
        if (('A' <= c && c <= 'Z') || IsUnicodeLetter(c))
        {
          jSToken = JSToken.Identifier;
          ScanIdentifier();
          break;
        }
        HandleError(JSError.IllegalChar);
        continue;
      }
      break;
    }
    currentToken.endLineNumber = currentLine;
    currentToken.endLinePos = startLinePos;
    currentToken.endPos = currentPos;
    gotEndOfLine = ((currentLine > num || jSToken == JSToken.EndOfFile) ? true : false);
    if (gotEndOfLine && jSToken == JSToken.StringLiteral && currentToken.lineNumber == num)
    {
      gotEndOfLine = false;
    }
  }
  catch (IndexOutOfRangeException)
  {
    jSToken = JSToken.None;
    currentToken.endPos = currentPos;
    currentToken.endLineNumber = currentLine;
    currentToken.endLinePos = startLinePos;
    throw new ScannerException(JSError.ErrEOF);
  }
  currentToken.token = jSToken;
}

  private char GetChar(int index)
  {
    return index < this.endPos ? this.strSourceCode[index] : char.MinValue;
  }

  /// <summary>Gets the current position in the code string. This is a pointer that moves as the code is scanned.</summary>
  /// <returns>The current position in the code string.</returns>
  /// <param name="absolute">This parameter is ignored.</param>
  public int GetCurrentPosition(bool absolute) => this.currentPos;

  /// <summary>Gets the current line number in the code. This is a pointer that moves as the code is scanned.</summary>
  /// <returns>The current line number in the code.</returns>
  public int GetCurrentLine() => this.currentLine;

  /// <summary>Gets the position in the code string where the first line of the current code starts. All lines of code are stored in a single string. This is a pointer that moves as the code is scanned.</summary>
  /// <returns>The position in the code string where the first line of the current code starts.</returns>
  public int GetStartLinePosition() => this.startLinePos;

  /// <summary>Gets a string that has been scanned and had all its escape sequences replaced.</summary>
  /// <returns>A string that has been scanned and had all its escape sequences replaced.</returns>
  public string GetStringLiteral() => this.escapedString;

  /// <summary>Gets the source code that is associated with this scanner object.</summary>
  /// <returns>The source code to scan.</returns>
  public string GetSourceCode() => this.strSourceCode;

  /// <summary>Indicates whether the end of a line was reached after the scanner scans or peeks for a token.</summary>
  /// <returns>true if the end of the line was reached; otherwise, false.</returns>
  public bool GotEndOfLine() => this.gotEndOfLine;

  internal string GetIdentifier()
  {
    string identifier;
    if (this.identifier.Length > 0)
    {
      identifier = this.identifier.ToString();
      this.identifier.Length = 0;
    }
    else
      identifier = this.currentToken.GetCode();
    if (identifier.Length > 500)
      identifier = identifier.Substring(0, 500) + identifier.GetHashCode().ToString((IFormatProvider) CultureInfo.InvariantCulture);
    return identifier;
  }

  private void ScanIdentifier()
  {
    while (this.IsIdentifierPartChar(this.GetChar(this.currentPos)))
      ++this.currentPos;
    if (this.idLastPosOnBuilder <= 0)
      return;
    this.identifier.Append(this.strSourceCode.Substring(this.idLastPosOnBuilder, this.currentPos - this.idLastPosOnBuilder));
    this.idLastPosOnBuilder = 0;
  }

  private JSToken ScanKeyword(JSKeyword keyword)
  {
    char c;
    while (true)
    {
      c = this.GetChar(this.currentPos);
      if ('a' <= c && c <= 'z')
        ++this.currentPos;
      else
        break;
    }
    if (!this.IsIdentifierPartChar(c))
      return keyword.GetKeyword(this.currentToken, this.currentPos - this.currentToken.startPos);
    this.ScanIdentifier();
    return JSToken.Identifier;
  }

  private JSToken ScanNumber(char leadChar)
  {
    bool flag1 = '.' == leadChar;
    JSToken jsToken = flag1 ? JSToken.NumericLiteral : JSToken.IntegerLiteral;
    bool flag2 = false;
    if ('0' == leadChar)
    {
      char ch = this.GetChar(this.currentPos);
      if ('x' == ch || 'X' == ch)
      {
        if (!JSScanner.IsHexDigit(this.GetChar(this.currentPos + 1)))
          this.HandleError(JSError.BadHexDigit);
        do
          ;
        while (JSScanner.IsHexDigit(this.GetChar(++this.currentPos)));
        return jsToken;
      }
    }
    while (true)
    {
      char c = this.GetChar(this.currentPos);
      if (!JSScanner.IsDigit(c))
      {
        if ('.' == c)
        {
          if (!flag1)
          {
            flag1 = true;
            jsToken = JSToken.NumericLiteral;
          }
          else
            break;
        }
        else if ('e' == c || 'E' == c)
        {
          if (!flag2)
          {
            flag2 = true;
            jsToken = JSToken.NumericLiteral;
          }
          else
            break;
        }
        else if ('+' == c || '-' == c)
        {
          char ch = this.GetChar(this.currentPos - 1);
          if ('e' != ch && 'E' != ch)
            break;
        }
        else
          break;
      }
      ++this.currentPos;
    }
    char ch1 = this.GetChar(this.currentPos - 1);
    if ('+' == ch1 || '-' == ch1)
    {
      --this.currentPos;
      ch1 = this.GetChar(this.currentPos - 1);
    }
    if ('e' == ch1 || 'E' == ch1)
      --this.currentPos;
    return jsToken;
  }

  internal string ScanRegExp()
  {
    int currentPos = this.currentPos;
    bool flag = false;
    while (true)
    {
      int index = this.currentPos++;
      char ch;
      if (!this.IsEndLineOrEOF(ch = this.GetChar(index), 0))
      {
        if (flag)
        {
          flag = false;
        }
        else
        {
          switch (ch)
          {
            case '/':
              goto label_4;
            case '\\':
              flag = true;
              continue;
            default:
              continue;
          }
        }
      }
      else
        goto label_9;
    }
label_4:
    if (currentPos == this.currentPos)
      return (string) null;
    this.currentToken.endPos = this.currentPos;
    this.currentToken.endLinePos = this.startLinePos;
    this.currentToken.endLineNumber = this.currentLine;
    return this.strSourceCode.Substring(this.currentToken.startPos + 1, this.currentToken.endPos - this.currentToken.startPos - 2);
label_9:
    this.currentPos = currentPos;
    return (string) null;
  }

  internal string ScanRegExpFlags()
  {
    int currentPos = this.currentPos;
    while (JSScanner.IsAsciiLetter(this.GetChar(this.currentPos)))
      ++this.currentPos;
    if (currentPos == this.currentPos)
      return (string) null;
    this.currentToken.endPos = this.currentPos;
    this.currentToken.endLineNumber = this.currentLine;
    this.currentToken.endLinePos = this.startLinePos;
    return this.strSourceCode.Substring(currentPos, this.currentToken.endPos - currentPos);
  }

  private void ScanString(char cStringTerminator)
  {
    int currentPos = this.currentPos;
    this.escapedString = (string) null;
    StringBuilder stringBuilder = (StringBuilder) null;
    char minValue;
    do
    {
      minValue = this.GetChar(this.currentPos++);
      if (minValue != '\\')
      {
        if (this.IsLineTerminator(minValue, 0))
        {
          this.HandleError(JSError.UnterminatedString);
          --this.currentPos;
          break;
        }
        if (minValue == char.MinValue)
        {
          --this.currentPos;
          this.HandleError(JSError.UnterminatedString);
          break;
        }
      }
      else
      {
        if (stringBuilder == null)
          stringBuilder = new StringBuilder(128 /*0x80*/);
        if (this.currentPos - currentPos - 1 > 0)
          stringBuilder.Append(this.strSourceCode, currentPos, this.currentPos - currentPos - 1);
        bool flag = false;
        int num1 = 0;
        minValue = this.GetChar(this.currentPos++);
        switch (minValue)
        {
          case '\n':
          case '\u2028':
          case '\u2029':
            ++this.currentLine;
            this.startLinePos = this.currentPos;
            break;
          case '\r':
            if ('\n' == this.GetChar(this.currentPos))
            {
              ++this.currentPos;
              goto case '\n';
            }
            goto case '\n';
          case '"':
            stringBuilder.Append('"');
            minValue = char.MinValue;
            break;
          case '\'':
            stringBuilder.Append('\'');
            minValue = char.MinValue;
            break;
          case '0':
          case '1':
          case '2':
          case '3':
            flag = true;
            num1 = (int) minValue - 48 /*0x30*/ << 6;
            goto case '4';
          case '4':
          case '5':
          case '6':
          case '7':
            if (!flag)
              num1 = (int) minValue - 48 /*0x30*/ << 3;
            minValue = this.GetChar(this.currentPos++);
            switch (minValue)
            {
              case '0':
              case '1':
              case '2':
              case '3':
              case '4':
              case '5':
              case '6':
              case '7':
                if (flag)
                {
                  int num2 = num1 | (int) minValue - 48 /*0x30*/ << 3;
                  minValue = this.GetChar(this.currentPos++);
                  switch (minValue)
                  {
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                      int num3 = num2 | (int) minValue - 48 /*0x30*/;
                      stringBuilder.Append((char) num3);
                      break;
                    default:
                      stringBuilder.Append((char) (num2 >> 3));
                      if ((int) minValue != (int) cStringTerminator)
                      {
                        --this.currentPos;
                        break;
                      }
                      break;
                  }
                }
                else
                {
                  int num4 = num1 | (int) minValue - 48 /*0x30*/;
                  stringBuilder.Append((char) num4);
                  break;
                }
                break;
              default:
                if (flag)
                  stringBuilder.Append((char) (num1 >> 6));
                else
                  stringBuilder.Append((char) (num1 >> 3));
                if ((int) minValue != (int) cStringTerminator)
                {
                  --this.currentPos;
                  break;
                }
                break;
            }
            break;
          case '\\':
            stringBuilder.Append('\\');
            break;
          case 'b':
            stringBuilder.Append('\b');
            break;
          case 'f':
            stringBuilder.Append('\f');
            break;
          case 'n':
            stringBuilder.Append('\n');
            break;
          case 'r':
            stringBuilder.Append('\r');
            break;
          case 't':
            stringBuilder.Append('\t');
            break;
          case 'u':
            minValue = this.GetChar(this.currentPos++);
            int num5;
            switch (minValue)
            {
              case '0':
              case '1':
              case '2':
              case '3':
              case '4':
              case '5':
              case '6':
              case '7':
              case '8':
              case '9':
                num5 = (int) minValue - 48 /*0x30*/ << 12;
                break;
              case 'A':
              case 'B':
              case 'C':
              case 'D':
              case 'E':
              case 'F':
                num5 = (int) minValue + 10 - 65 << 12;
                break;
              case 'a':
              case 'b':
              case 'c':
              case 'd':
              case 'e':
              case 'f':
                num5 = (int) minValue + 10 - 97 << 12;
                break;
              default:
                this.HandleError(JSError.BadHexDigit);
                if ((int) minValue != (int) cStringTerminator)
                {
                  --this.currentPos;
                  goto label_77;
                }
                goto label_77;
            }
            minValue = this.GetChar(this.currentPos++);
            int num6;
            switch (minValue)
            {
              case '0':
              case '1':
              case '2':
              case '3':
              case '4':
              case '5':
              case '6':
              case '7':
              case '8':
              case '9':
                num6 = num5 | (int) minValue - 48 /*0x30*/ << 8;
                break;
              case 'A':
              case 'B':
              case 'C':
              case 'D':
              case 'E':
              case 'F':
                num6 = num5 | (int) minValue + 10 - 65 << 8;
                break;
              case 'a':
              case 'b':
              case 'c':
              case 'd':
              case 'e':
              case 'f':
                num6 = num5 | (int) minValue + 10 - 97 << 8;
                break;
              default:
                this.HandleError(JSError.BadHexDigit);
                if ((int) minValue != (int) cStringTerminator)
                {
                  --this.currentPos;
                  goto label_77;
                }
                goto label_77;
            }
            minValue = this.GetChar(this.currentPos++);
            int num7;
            switch (minValue)
            {
              case '0':
              case '1':
              case '2':
              case '3':
              case '4':
              case '5':
              case '6':
              case '7':
              case '8':
              case '9':
                num7 = num6 | (int) minValue - 48 /*0x30*/ << 4;
                break;
              case 'A':
              case 'B':
              case 'C':
              case 'D':
              case 'E':
              case 'F':
                num7 = num6 | (int) minValue + 10 - 65 << 4;
                break;
              case 'a':
              case 'b':
              case 'c':
              case 'd':
              case 'e':
              case 'f':
                num7 = num6 | (int) minValue + 10 - 97 << 4;
                break;
              default:
                this.HandleError(JSError.BadHexDigit);
                if ((int) minValue != (int) cStringTerminator)
                {
                  --this.currentPos;
                  goto label_77;
                }
                goto label_77;
            }
            minValue = this.GetChar(this.currentPos++);
            int num8;
            switch (minValue)
            {
              case '0':
              case '1':
              case '2':
              case '3':
              case '4':
              case '5':
              case '6':
              case '7':
              case '8':
              case '9':
                num8 = num7 | (int) minValue - 48 /*0x30*/;
                break;
              case 'A':
              case 'B':
              case 'C':
              case 'D':
              case 'E':
              case 'F':
                num8 = num7 | (int) minValue + 10 - 65;
                break;
              case 'a':
              case 'b':
              case 'c':
              case 'd':
              case 'e':
              case 'f':
                num8 = num7 | (int) minValue + 10 - 97;
                break;
              default:
                this.HandleError(JSError.BadHexDigit);
                if ((int) minValue != (int) cStringTerminator)
                {
                  --this.currentPos;
                  goto label_77;
                }
                goto label_77;
            }
            stringBuilder.Append((char) num8);
            break;
          case 'v':
            stringBuilder.Append('\v');
            break;
          case 'x':
            minValue = this.GetChar(this.currentPos++);
            int num9;
            switch (minValue)
            {
              case '0':
              case '1':
              case '2':
              case '3':
              case '4':
              case '5':
              case '6':
              case '7':
              case '8':
              case '9':
                num9 = (int) minValue - 48 /*0x30*/ << 4;
                break;
              case 'A':
              case 'B':
              case 'C':
              case 'D':
              case 'E':
              case 'F':
                num9 = (int) minValue + 10 - 65 << 4;
                break;
              case 'a':
              case 'b':
              case 'c':
              case 'd':
              case 'e':
              case 'f':
                num9 = (int) minValue + 10 - 97 << 4;
                break;
              default:
                this.HandleError(JSError.BadHexDigit);
                if ((int) minValue != (int) cStringTerminator)
                {
                  --this.currentPos;
                  goto label_77;
                }
                goto label_77;
            }
            minValue = this.GetChar(this.currentPos++);
            int num10;
            switch (minValue)
            {
              case '0':
              case '1':
              case '2':
              case '3':
              case '4':
              case '5':
              case '6':
              case '7':
              case '8':
              case '9':
                num10 = num9 | (int) minValue - 48 /*0x30*/;
                break;
              case 'A':
              case 'B':
              case 'C':
              case 'D':
              case 'E':
              case 'F':
                num10 = num9 | (int) minValue + 10 - 65;
                break;
              case 'a':
              case 'b':
              case 'c':
              case 'd':
              case 'e':
              case 'f':
                num10 = num9 | (int) minValue + 10 - 97;
                break;
              default:
                this.HandleError(JSError.BadHexDigit);
                if ((int) minValue != (int) cStringTerminator)
                {
                  --this.currentPos;
                  goto label_77;
                }
                goto label_77;
            }
            stringBuilder.Append((char) num10);
            break;
          default:
            stringBuilder.Append(minValue);
            break;
        }
label_77:
        currentPos = this.currentPos;
      }
    }
    while ((int) minValue != (int) cStringTerminator);
    if (stringBuilder != null)
    {
      if (this.currentPos - currentPos - 1 > 0)
        stringBuilder.Append(this.strSourceCode, currentPos, this.currentPos - currentPos - 1);
      this.escapedString = stringBuilder.ToString();
    }
    else if (this.currentPos <= this.currentToken.startPos + 2)
      this.escapedString = "";
    else
      this.escapedString = this.currentToken.source_string.Substring(this.currentToken.startPos + 1, this.currentPos - this.currentToken.startPos - 2);
  }

  private void SkipSingleLineComment()
  {
    do
      ;
    while (!this.IsEndLineOrEOF(this.GetChar(this.currentPos++), 0));
    if (this.IsAuthoring)
    {
      this.currentToken.endPos = this.currentPos;
      this.currentToken.endLineNumber = this.currentLine;
      this.currentToken.endLinePos = this.startLinePos;
      this.gotEndOfLine = true;
    }
    ++this.currentLine;
    this.startLinePos = this.currentPos;
  }

  /// <summary>Skips a comment that spans multiple lines, and advances the pointers that identify the current position in the code.</summary>
  /// <returns>The new current position in the code string.</returns>
  public int SkipMultiLineComment()
  {
    while (true)
    {
      char c = this.GetChar(this.currentPos);
      while ('*' == c)
      {
        c = this.GetChar(++this.currentPos);
        if ('/' == c)
        {
          ++this.currentPos;
          return this.currentPos;
        }
        if (c != char.MinValue)
        {
          if (this.IsLineTerminator(c, 1))
          {
            c = this.GetChar(++this.currentPos);
            ++this.currentLine;
            this.startLinePos = this.currentPos + 1;
          }
        }
        else
          break;
      }
      if (c != char.MinValue || this.currentPos < this.endPos)
      {
        if (this.IsLineTerminator(c, 1))
        {
          ++this.currentLine;
          this.startLinePos = this.currentPos + 1;
        }
        ++this.currentPos;
      }
      else
        break;
    }
    if (!this.IsAuthoring)
    {
      this.currentToken.endPos = --this.currentPos;
      this.currentToken.endLinePos = this.startLinePos;
      this.currentToken.endLineNumber = this.currentLine;
      throw new ScannerException(JSError.NoCommentEnd);
    }
    return this.currentPos;
  }

  private void SkipBlanks()
  {
    char c = this.GetChar(this.currentPos);
    while (JSScanner.IsBlankSpace(c))
      c = this.GetChar(++this.currentPos);
  }

  private static bool IsBlankSpace(char c)
  {
    switch (c)
    {
      case '\t':
      case '\v':
      case '\f':
      case ' ':
      case ' ':
        return true;
      default:
        return c >= '\u0080' && char.GetUnicodeCategory(c) == UnicodeCategory.SpaceSeparator;
    }
  }

  private bool IsLineTerminator(char c, int increment)
  {
    switch (c)
    {
      case '\n':
        return true;
      case '\r':
        if ('\n' == this.GetChar(this.currentPos + increment))
          ++this.currentPos;
        return true;
      case '\u2028':
        return true;
      case '\u2029':
        return true;
      default:
        return false;
    }
  }

  private bool IsEndLineOrEOF(char c, int increment)
  {
    if (this.IsLineTerminator(c, increment))
      return true;
    return c == char.MinValue && this.currentPos >= this.endPos;
  }

  private int GetHexValue(char hex)
  {
    return '0' > hex || hex > '9' ? ('a' > hex || hex > 'f' ? (int) hex - 65 + 10 : (int) hex - 97 + 10) : (int) hex - 48 /*0x30*/;
  }

  internal bool IsIdentifierPartChar(char c)
  {
    if (this.IsIdentifierStartChar(ref c) || '0' <= c && c <= '9')
      return true;
    if (c < '\u0080')
      return false;
    switch (char.GetUnicodeCategory(c))
    {
      case UnicodeCategory.NonSpacingMark:
      case UnicodeCategory.SpacingCombiningMark:
      case UnicodeCategory.DecimalDigitNumber:
      case UnicodeCategory.ConnectorPunctuation:
        return true;
      default:
        return false;
    }
  }

  internal bool IsIdentifierStartChar(ref char c)
  {
    bool flag = false;
    if ('\\' == c && 'u' == this.GetChar(this.currentPos + 1))
    {
      char ch1 = this.GetChar(this.currentPos + 2);
      if (JSScanner.IsHexDigit(ch1))
      {
        char ch2 = this.GetChar(this.currentPos + 3);
        if (JSScanner.IsHexDigit(ch2))
        {
          char ch3 = this.GetChar(this.currentPos + 4);
          if (JSScanner.IsHexDigit(ch3))
          {
            char ch4 = this.GetChar(this.currentPos + 5);
            if (JSScanner.IsHexDigit(ch4))
            {
              flag = true;
              c = (char) (this.GetHexValue(ch1) << 12 | this.GetHexValue(ch2) << 8 | this.GetHexValue(ch3) << 4 | this.GetHexValue(ch4));
            }
          }
        }
      }
    }
    if (('a' > c || c > 'z') && ('A' > c || c > 'Z') && '_' != c && '$' != c)
    {
      if (c < '\u0080')
        return false;
      switch (char.GetUnicodeCategory(c))
      {
        case UnicodeCategory.UppercaseLetter:
        case UnicodeCategory.LowercaseLetter:
        case UnicodeCategory.TitlecaseLetter:
        case UnicodeCategory.ModifierLetter:
        case UnicodeCategory.OtherLetter:
        case UnicodeCategory.LetterNumber:
          break;
        default:
          return false;
      }
    }
    if (flag)
    {
      int startIndex = this.idLastPosOnBuilder > 0 ? this.idLastPosOnBuilder : this.currentToken.startPos;
      if (this.currentPos - startIndex > 0)
        this.identifier.Append(this.strSourceCode.Substring(startIndex, this.currentPos - startIndex));
      this.identifier.Append(c);
      this.currentPos += 5;
      this.idLastPosOnBuilder = this.currentPos + 1;
    }
    return true;
  }

  internal static bool IsDigit(char c) => '0' <= c && c <= '9';

  internal static bool IsHexDigit(char c)
  {
    if (JSScanner.IsDigit(c) || 'A' <= c && c <= 'F')
      return true;
    return 'a' <= c && c <= 'f';
  }

  internal static bool IsAsciiLetter(char c)
  {
    if ('A' <= c && c <= 'Z')
      return true;
    return 'a' <= c && c <= 'z';
  }

  internal static bool IsUnicodeLetter(char c) => c >= '\u0080' && char.IsLetter(c);

  private void SetPreProcessorOn()
  {
    this.preProcessorOn = true;
    this.ppTable = new SimpleHashtable(16U /*0x10*/);
    this.ppTable[(object) "_debug"] = (object) this.globals.engine.GenerateDebugInfo;
    this.ppTable[(object) "_fast"] = (object) ((IActivationObject) this.globals.ScopeStack.Peek()).GetGlobalScope().fast;
    this.ppTable[(object) "_jscript"] = (object) true;
    this.ppTable[(object) "_jscript_build"] = (object) GlobalObject.ScriptEngineBuildVersion();
    SimpleHashtable ppTable = this.ppTable;
    int num = GlobalObject.ScriptEngineMajorVersion();
    string str1 = num.ToString();
    num = GlobalObject.ScriptEngineMinorVersion();
    string str2 = num.ToString();
    ppTable[(object) "_jscript_version"] = (object) Convert.ToNumber($"{str1}.{str2}");
    this.ppTable[(object) "_microsoft"] = (object) true;
    if (this.globals.engine.PEMachineArchitecture == ImageFileMachine.I386 && this.globals.engine.PEKindFlags == PortableExecutableKinds.ILOnly)
    {
      this.ppTable[(object) "_win32"] = (object) Environment.OSVersion.Platform.ToString().StartsWith("Win32", StringComparison.Ordinal);
      this.ppTable[(object) "_x86"] = (object) true;
    }
    Hashtable option = (Hashtable) this.globals.engine.GetOption("defines");
    if (option == null)
      return;
    foreach (DictionaryEntry dictionaryEntry in option)
      this.ppTable[dictionaryEntry.Key] = dictionaryEntry.Value;
  }

  private bool PPTestCond()
  {
    this.SkipBlanks();
    if ('(' != this.GetChar(this.currentPos))
    {
      this.currentToken.startPos = this.currentPos - 1;
      this.currentToken.lineNumber = this.currentLine;
      this.currentToken.startLinePos = this.startLinePos;
      this.HandleError(JSError.NoLeftParen);
    }
    else
      ++this.currentPos;
    object obj = this.PPScanExpr();
    if (')' != this.GetChar(this.currentPos++))
    {
      this.currentToken.startPos = this.currentPos - 1;
      this.currentToken.lineNumber = this.currentLine;
      this.currentToken.startLinePos = this.startLinePos;
      this.HandleError(JSError.NoRightParen);
      --this.currentPos;
    }
    return Convert.ToBoolean(obj);
  }

  private void PPSkipToNextCondition(bool checkCondition)
  {
    int num = 0;
    do
    {
      do
      {
        do
        {
          char ch = this.GetChar(this.currentPos++);
          if (ch <= '\r')
          {
            if (ch != char.MinValue)
            {
              if (ch != '\n')
              {
                if (ch == '\r')
                {
                  if (this.GetChar(this.currentPos) == '\n')
                    ++this.currentPos;
                  ++this.currentLine;
                  this.startLinePos = this.currentPos;
                }
              }
              else
              {
                ++this.currentLine;
                this.startLinePos = this.currentPos;
              }
            }
            else if (this.currentPos >= this.endPos)
            {
              --this.currentPos;
              this.currentToken.endPos = this.currentPos;
              this.currentToken.endLineNumber = this.currentLine;
              this.currentToken.endLinePos = this.startLinePos;
              this.HandleError(JSError.NoCcEnd);
              throw new ScannerException(JSError.ErrEOF);
            }
          }
          else if (ch != '@')
          {
            if (ch != '\u2028')
            {
              if (ch == '\u2029')
              {
                ++this.currentLine;
                this.startLinePos = this.currentPos;
              }
            }
            else
            {
              ++this.currentLine;
              this.startLinePos = this.currentPos;
            }
          }
          else
          {
            this.currentToken.startPos = this.currentPos;
            this.currentToken.lineNumber = this.currentLine;
            this.currentToken.startLinePos = this.startLinePos;
            this.ScanIdentifier();
            switch (this.currentPos - this.currentToken.startPos)
            {
              case 2:
                continue;
              case 3:
                goto label_20;
              case 4:
                goto label_24;
              default:
                continue;
            }
          }
        }
        while ('i' != this.strSourceCode[this.currentToken.startPos] || 'f' != this.strSourceCode[this.currentToken.startPos + 1]);
        ++num;
        continue;
label_20:;
      }
      while ('e' != this.strSourceCode[this.currentToken.startPos] || 'n' != this.strSourceCode[this.currentToken.startPos + 1] || 'd' != this.strSourceCode[this.currentToken.startPos + 2]);
      if (num == 0)
      {
        --this.matchIf;
        break;
      }
      --num;
      continue;
label_24:
      if ('e' == this.strSourceCode[this.currentToken.startPos] && 'l' == this.strSourceCode[this.currentToken.startPos + 1] && 's' == this.strSourceCode[this.currentToken.startPos + 2] && 'e' == this.strSourceCode[this.currentToken.startPos + 3])
      {
        if (num == 0 & checkCondition)
          break;
      }
    }
    while ('e' != this.strSourceCode[this.currentToken.startPos] || 'l' != this.strSourceCode[this.currentToken.startPos + 1] || 'i' != this.strSourceCode[this.currentToken.startPos + 2] || 'f' != this.strSourceCode[this.currentToken.startPos + 3] || !(num == 0 & checkCondition) || !this.PPTestCond());
  }

  private void PPScanSet()
  {
    this.SkipBlanks();
    if ('@' != this.GetChar(this.currentPos++))
    {
      this.HandleError(JSError.NoAt);
      --this.currentPos;
    }
    int currentPos = this.currentPos;
    this.ScanIdentifier();
    int length = this.currentPos - currentPos;
    string key;
    if (length == 0)
    {
      this.currentToken.startPos = this.currentPos - 1;
      this.currentToken.lineNumber = this.currentLine;
      this.currentToken.startLinePos = this.startLinePos;
      this.HandleError(JSError.NoIdentifier);
      key = "#_Missing CC Identifier_#";
    }
    else
      key = this.strSourceCode.Substring(currentPos, length);
    this.SkipBlanks();
    char ch = this.GetChar(this.currentPos++);
    if ('(' == ch)
    {
      switch (key)
      {
        case "position":
          this.PPRemapPositionInfo();
          break;
        case "option":
          this.PPLanguageOption();
          break;
        case "debug":
          this.PPDebugDirective();
          break;
        default:
          this.currentToken.startPos = this.currentPos - 1;
          this.currentToken.lineNumber = this.currentLine;
          this.currentToken.startLinePos = this.startLinePos;
          this.HandleError(JSError.NoEqual);
          --this.currentPos;
          break;
      }
    }
    else
    {
      if ('=' != ch)
      {
        this.currentToken.startPos = this.currentPos - 1;
        this.currentToken.lineNumber = this.currentLine;
        this.currentToken.startLinePos = this.startLinePos;
        this.HandleError(JSError.NoEqual);
        --this.currentPos;
      }
      object obj = this.PPScanConstant();
      this.ppTable[(object) key] = obj;
    }
  }

  private object PPScanExpr()
  {
    OpListItem prev1 = new OpListItem(JSToken.None, OpPrec.precNone, (OpListItem) null);
    ConstantListItem prev2 = new ConstantListItem(this.PPScanConstant(), (ConstantListItem) null);
    while (true)
    {
      this.GetNextToken();
      if (JSScanner.IsPPOperator(this.currentToken.token))
      {
        OpPrec operatorPrecedence = JSScanner.GetPPOperatorPrecedence(this.currentToken.token);
        while (operatorPrecedence < prev1._prec)
        {
          object term = this.PPGetValue(prev1._operator, prev2.prev.term, prev2.term);
          prev1 = prev1._prev;
          ConstantListItem prev3 = prev2.prev.prev;
          prev2 = new ConstantListItem(term, prev3);
        }
        prev1 = new OpListItem(this.currentToken.token, operatorPrecedence, prev1);
        prev2 = new ConstantListItem(this.PPScanConstant(), prev2);
      }
      else
        break;
    }
    while (prev1._operator != JSToken.None)
    {
      object term = this.PPGetValue(prev1._operator, prev2.prev.term, prev2.term);
      prev1 = prev1._prev;
      ConstantListItem prev4 = prev2.prev.prev;
      prev2 = new ConstantListItem(term, prev4);
    }
    this.currentPos = this.currentToken.startPos;
    return prev2.term;
  }

  private void PPRemapPositionInfo()
  {
    this.GetNextToken();
    string documentName = (string) null;
    int startLine = 0;
    int startCol = -1;
    bool flag = false;
    while (JSToken.RightParen != this.currentToken.token)
    {
      if (JSToken.Identifier == this.currentToken.token)
      {
        if (this.currentToken.Equals("file"))
        {
          if (this.currentDocument == null)
          {
            if (documentName == null)
            {
              this.GetNextToken();
              if (JSToken.Assign != this.currentToken.token)
              {
                this.HandleError(JSError.InvalidPositionDirective);
                goto label_49;
              }
              this.GetNextToken();
              if (JSToken.StringLiteral != this.currentToken.token)
              {
                this.HandleError(JSError.InvalidPositionDirective);
                goto label_49;
              }
              documentName = this.GetStringLiteral();
              if (documentName == this.currentToken.document.documentName)
              {
                documentName = (string) null;
                this.HandleError(JSError.InvalidPositionDirective);
                goto label_49;
              }
            }
            else
            {
              this.HandleError(JSError.InvalidPositionDirective);
              goto label_49;
            }
          }
          else
          {
            this.HandleError(JSError.CannotNestPositionDirective);
            goto label_49;
          }
        }
        else if (this.currentToken.Equals("line"))
        {
          if (this.currentDocument == null)
          {
            if (startLine == 0)
            {
              this.GetNextToken();
              if (JSToken.Assign != this.currentToken.token)
              {
                this.HandleError(JSError.InvalidPositionDirective);
                goto label_49;
              }
              this.GetNextToken();
              if (JSToken.IntegerLiteral != this.currentToken.token)
              {
                this.HandleError(JSError.InvalidPositionDirective);
                goto label_49;
              }
              double number = Convert.ToNumber(this.currentToken.GetCode(), true, true, (object) Missing.Value);
              if ((double) (int) number == number && number > 0.0)
              {
                startLine = (int) number;
              }
              else
              {
                startLine = 1;
                this.HandleError(JSError.InvalidPositionDirective);
                goto label_49;
              }
            }
            else
            {
              this.HandleError(JSError.InvalidPositionDirective);
              goto label_49;
            }
          }
          else
          {
            this.HandleError(JSError.CannotNestPositionDirective);
            goto label_49;
          }
        }
        else if (this.currentToken.Equals("column"))
        {
          if (this.currentDocument == null)
          {
            if (startCol == -1)
            {
              this.GetNextToken();
              if (JSToken.Assign != this.currentToken.token)
              {
                this.HandleError(JSError.InvalidPositionDirective);
                goto label_49;
              }
              this.GetNextToken();
              if (JSToken.IntegerLiteral != this.currentToken.token)
              {
                this.HandleError(JSError.InvalidPositionDirective);
                goto label_49;
              }
              double number = Convert.ToNumber(this.currentToken.GetCode(), true, true, (object) Missing.Value);
              if ((double) (int) number == number && number >= 0.0)
              {
                startCol = (int) number;
              }
              else
              {
                startCol = 0;
                this.HandleError(JSError.InvalidPositionDirective);
                goto label_49;
              }
            }
            else
            {
              this.HandleError(JSError.InvalidPositionDirective);
              goto label_49;
            }
          }
          else
          {
            this.HandleError(JSError.CannotNestPositionDirective);
            goto label_49;
          }
        }
        else
        {
          if (this.currentToken.Equals("end"))
          {
            if (this.currentDocument != null)
            {
              this.GetNextToken();
              if (JSToken.RightParen != this.currentToken.token)
              {
                this.HandleError(JSError.InvalidPositionDirective);
                goto label_49;
              }
              this.currentToken.document = this.currentDocument;
              this.currentDocument = (DocumentContext) null;
              flag = true;
              break;
            }
            this.HandleError(JSError.WrongDirective);
            goto label_49;
          }
          this.HandleError(JSError.InvalidPositionDirective);
          goto label_49;
        }
        this.GetNextToken();
        if (JSToken.RightParen != this.currentToken.token)
        {
          if (JSToken.Semicolon == this.currentToken.token)
          {
            this.GetNextToken();
            continue;
          }
          continue;
        }
        break;
      }
      this.HandleError(JSError.InvalidPositionDirective);
label_49:
      while (JSToken.RightParen != this.currentToken.token && this.currentToken.token != JSToken.EndOfFile)
        this.GetNextToken();
      break;
    }
    this.SkipBlanks();
    if (';' == this.GetChar(this.currentPos))
    {
      ++this.currentPos;
      this.SkipBlanks();
    }
    if (this.currentPos < this.endPos && !this.IsLineTerminator(this.GetChar(this.currentPos++), 0))
    {
      this.HandleError(JSError.MustBeEOL);
      while (this.currentPos < this.endPos && !this.IsLineTerminator(this.GetChar(this.currentPos++), 0))
        ;
    }
    ++this.currentLine;
    this.startLinePos = this.currentPos;
    if (flag)
      return;
    if (documentName == null && startLine == 0 && startCol == -1)
    {
      this.HandleError(JSError.InvalidPositionDirective);
    }
    else
    {
      if (documentName == null)
        documentName = this.currentToken.document.documentName;
      if (startLine == 0)
        startLine = 1;
      if (startCol == -1)
        startCol = 0;
      this.currentDocument = this.currentToken.document;
      this.currentToken.document = new DocumentContext(documentName, startLine, startCol, this.currentLine, this.currentDocument.sourceItem);
    }
  }

  private void PPDebugDirective()
  {
    this.GetNextToken();
    if (JSToken.Identifier == this.currentToken.token)
    {
      bool flag;
      if (this.currentToken.Equals("off"))
        flag = false;
      else if (this.currentToken.Equals("on"))
      {
        flag = true;
      }
      else
      {
        this.HandleError(JSError.InvalidDebugDirective);
        goto label_11;
      }
      this.GetNextToken();
      if (JSToken.RightParen != this.currentToken.token)
      {
        this.HandleError(JSError.InvalidDebugDirective);
      }
      else
      {
        this.currentToken.document.debugOn = flag && this.globals.engine.GenerateDebugInfo;
        this.ppTable[(object) "_debug"] = (object) flag;
      }
    }
    else
      this.HandleError(JSError.InvalidDebugDirective);
label_11:
    while (JSToken.RightParen != this.currentToken.token)
      this.GetNextToken();
    this.SkipBlanks();
    if (';' == this.GetChar(this.currentPos))
    {
      ++this.currentPos;
      this.SkipBlanks();
    }
    if (!this.IsLineTerminator(this.GetChar(this.currentPos++), 0))
    {
      this.HandleError(JSError.MustBeEOL);
      while (!this.IsLineTerminator(this.GetChar(this.currentPos++), 0))
        ;
    }
    ++this.currentLine;
    this.startLinePos = this.currentPos;
  }

  private void PPLanguageOption()
  {
    this.GetNextToken();
    this.HandleError(JSError.InvalidLanguageOption);
    this.GetNextToken();
    Context context = (Context) null;
    while (JSToken.RightParen != this.currentToken.token)
    {
      if (context == null)
        context = this.currentToken.Clone();
      else
        context.UpdateWith(this.currentToken);
      this.GetNextToken();
    }
    if (context == null)
      return;
    this.HandleError(JSError.NoRightParen);
  }

  private object PPScanConstant()
  {
    this.GetNextToken();
    object obj;
    switch (this.currentToken.token)
    {
      case JSToken.True:
        obj = (object) true;
        break;
      case JSToken.False:
        obj = (object) false;
        break;
      case JSToken.IntegerLiteral:
        obj = (object) Convert.ToNumber(this.currentToken.GetCode(), true, true, (object) Missing.Value);
        break;
      case JSToken.NumericLiteral:
        obj = (object) Convert.ToNumber(this.currentToken.GetCode(), false, false, (object) Missing.Value);
        break;
      case JSToken.LeftParen:
        obj = this.PPScanExpr();
        this.GetNextToken();
        if (JSToken.RightParen != this.currentToken.token)
        {
          this.currentToken.endPos = this.currentToken.startPos + 1;
          this.currentToken.endLineNumber = this.currentLine;
          this.currentToken.endLinePos = this.startLinePos;
          this.HandleError(JSError.NoRightParen);
          this.currentPos = this.currentToken.startPos;
          break;
        }
        break;
      case JSToken.FirstOp:
        obj = (object) !Convert.ToBoolean(this.PPScanConstant());
        break;
      case JSToken.BitwiseNot:
        obj = (object) ~Convert.ToInt32(this.PPScanConstant());
        break;
      case JSToken.FirstBinaryOp:
        obj = (object) Convert.ToNumber(this.PPScanConstant());
        break;
      case JSToken.Minus:
        obj = (object) -Convert.ToNumber(this.PPScanConstant());
        break;
      case JSToken.PreProcessorConstant:
        obj = this.preProcessorValue;
        break;
      default:
        this.HandleError(JSError.NotConst);
        this.currentPos = this.currentToken.startPos;
        obj = (object) true;
        break;
    }
    return obj;
  }

  private object PPGetValue(JSToken op, object op1, object op2)
  {
    switch (op)
    {
      case JSToken.FirstBinaryOp:
        return (object) (Convert.ToNumber(op1) + Convert.ToNumber(op2));
      case JSToken.Minus:
        return (object) (Convert.ToNumber(op1) - Convert.ToNumber(op2));
      case JSToken.LogicalOr:
        return (object) (Convert.ToBoolean(op1) || Convert.ToBoolean(op2));
      case JSToken.LogicalAnd:
        return (object) (Convert.ToBoolean(op1) && Convert.ToBoolean(op2));
      case JSToken.BitwiseOr:
        return (object) (Convert.ToInt32(op1) | Convert.ToInt32(op2));
      case JSToken.BitwiseXor:
        return (object) (Convert.ToInt32(op1) ^ Convert.ToInt32(op2));
      case JSToken.BitwiseAnd:
        return (object) (Convert.ToInt32(op1) & Convert.ToInt32(op2));
      case JSToken.Equal:
        return (object) (Convert.ToNumber(op1) == Convert.ToNumber(op2));
      case JSToken.NotEqual:
        return (object) (Convert.ToNumber(op1) != Convert.ToNumber(op2));
      case JSToken.StrictEqual:
        return (object) (op1 == op2);
      case JSToken.StrictNotEqual:
        return (object) (op1 != op2);
      case JSToken.GreaterThan:
        return (object) (Convert.ToNumber(op1) > Convert.ToNumber(op2));
      case JSToken.LessThan:
        return (object) (Convert.ToNumber(op1) < Convert.ToNumber(op2));
      case JSToken.LessThanEqual:
        return (object) (Convert.ToNumber(op1) <= Convert.ToNumber(op2));
      case JSToken.GreaterThanEqual:
        return (object) (Convert.ToNumber(op1) >= Convert.ToNumber(op2));
      case JSToken.LeftShift:
        return (object) (Convert.ToInt32(op1) << Convert.ToInt32(op2));
      case JSToken.RightShift:
        return (object) (Convert.ToInt32(op1) >> Convert.ToInt32(op2));
      case JSToken.UnsignedRightShift:
        return (object) (uint) (Convert.ToInt32(op1) >>> (Convert.ToInt32(op2) & 31 /*0x1F*/));
      case JSToken.Multiply:
        return (object) (Convert.ToNumber(op1) * Convert.ToNumber(op2));
      case JSToken.Divide:
        return (object) (Convert.ToNumber(op1) / Convert.ToNumber(op2));
      case JSToken.Modulo:
        return (object) (Convert.ToInt32(op1) % Convert.ToInt32(op2));
      default:
        return (object) null;
    }
  }

  internal object GetPreProcessorValue() => this.preProcessorValue;

  private void HandleError(JSError error)
  {
    if (this.IsAuthoring)
      return;
    this.currentToken.HandleError(error);
  }

  /// <summary>Determines whether the specified token is an operator in the JScript language.</summary>
  /// <returns>true if the token is an operator; otherwise, false.</returns>
  /// <param name="token">The token to check.</param>
  public static bool IsOperator(JSToken token)
  {
    return JSToken.FirstOp <= token && token <= JSToken.Comma;
  }

  internal static bool IsAssignmentOperator(JSToken token)
  {
    return JSToken.Assign <= token && token <= JSToken.UnsignedRightShiftAssign;
  }

  internal static bool CanStartStatement(JSToken token)
  {
    return JSToken.If <= token && token <= JSToken.Function;
  }

  internal static bool CanParseAsExpression(JSToken token)
  {
    if (JSToken.FirstBinaryOp <= token && token <= JSToken.Comma)
      return true;
    return JSToken.LeftParen <= token && token <= JSToken.AccessField;
  }

  internal static bool IsRightAssociativeOperator(JSToken token)
  {
    return JSToken.Assign <= token && token <= JSToken.ConditionalIf;
  }

  /// <summary>Determines whether the specified token is a keyword in the JScript language.</summary>
  /// <returns>true if the token is a keyword; otherwise, false.</returns>
  /// <param name="token">The token to check.</param>
  public static bool IsKeyword(JSToken token)
  {
    switch (token)
    {
      case JSToken.If:
      case JSToken.For:
      case JSToken.Do:
      case JSToken.While:
      case JSToken.Continue:
      case JSToken.Break:
      case JSToken.Return:
      case JSToken.Import:
      case JSToken.With:
      case JSToken.Switch:
      case JSToken.Throw:
      case JSToken.Try:
      case JSToken.Package:
      case JSToken.Abstract:
      case JSToken.Public:
      case JSToken.Static:
      case JSToken.Private:
      case JSToken.Protected:
      case JSToken.Final:
      case JSToken.Var:
      case JSToken.Const:
      case JSToken.Class:
      case JSToken.Function:
      case JSToken.Null:
      case JSToken.True:
      case JSToken.False:
      case JSToken.This:
      case JSToken.Delete:
      case JSToken.Void:
      case JSToken.Typeof:
      case JSToken.Instanceof:
      case JSToken.In:
      case JSToken.Case:
      case JSToken.Catch:
      case JSToken.Debugger:
      case JSToken.Default:
      case JSToken.Else:
      case JSToken.Export:
      case JSToken.Extends:
      case JSToken.Finally:
      case JSToken.Get:
      case JSToken.Implements:
      case JSToken.Interface:
      case JSToken.New:
      case JSToken.Set:
      case JSToken.Super:
      case JSToken.Boolean:
      case JSToken.Byte:
      case JSToken.Char:
      case JSToken.Double:
      case JSToken.Enum:
      case JSToken.Float:
      case JSToken.Goto:
      case JSToken.Int:
      case JSToken.Long:
      case JSToken.Native:
      case JSToken.Short:
      case JSToken.Synchronized:
      case JSToken.Transient:
      case JSToken.Throws:
      case JSToken.Volatile:
        return true;
      default:
        return false;
    }
  }

  internal static bool IsProcessableOperator(JSToken token)
  {
    return JSToken.FirstBinaryOp <= token && token <= JSToken.ConditionalIf;
  }

  internal static bool IsPPOperator(JSToken token)
  {
    return JSToken.FirstBinaryOp <= token && token <= JSToken.Modulo;
  }

  internal static OpPrec GetOperatorPrecedence(JSToken token)
  {
    return JSScanner.s_OperatorsPrec[(int) (token - 46)];
  }

  internal static OpPrec GetPPOperatorPrecedence(JSToken token)
  {
    return JSScanner.s_PPOperatorsPrec[(int) (token - 46)];
  }

  private static OpPrec[] InitOperatorsPrec()
  {
    OpPrec[] opPrecArray = new OpPrec[36];
    opPrecArray[0] = OpPrec.precAdditive;
    opPrecArray[1] = OpPrec.precAdditive;
    opPrecArray[2] = OpPrec.precLogicalOr;
    opPrecArray[3] = OpPrec.precLogicalAnd;
    opPrecArray[4] = OpPrec.precBitwiseOr;
    opPrecArray[5] = OpPrec.precBitwiseXor;
    opPrecArray[6] = OpPrec.precBitwiseAnd;
    opPrecArray[7] = OpPrec.precEquality;
    opPrecArray[8] = OpPrec.precEquality;
    opPrecArray[9] = OpPrec.precEquality;
    opPrecArray[10] = OpPrec.precEquality;
    opPrecArray[21] = OpPrec.precRelational;
    opPrecArray[22] = OpPrec.precRelational;
    opPrecArray[11] = OpPrec.precRelational;
    opPrecArray[12] = OpPrec.precRelational;
    opPrecArray[13] = OpPrec.precRelational;
    opPrecArray[14] = OpPrec.precRelational;
    opPrecArray[15] = OpPrec.precShift;
    opPrecArray[16 /*0x10*/] = OpPrec.precShift;
    opPrecArray[17] = OpPrec.precShift;
    opPrecArray[18] = OpPrec.precMultiplicative;
    opPrecArray[19] = OpPrec.precMultiplicative;
    opPrecArray[20] = OpPrec.precMultiplicative;
    opPrecArray[23] = OpPrec.precAssignment;
    opPrecArray[24] = OpPrec.precAssignment;
    opPrecArray[25] = OpPrec.precAssignment;
    opPrecArray[26] = OpPrec.precAssignment;
    opPrecArray[27] = OpPrec.precAssignment;
    opPrecArray[28] = OpPrec.precAssignment;
    opPrecArray[29] = OpPrec.precAssignment;
    opPrecArray[30] = OpPrec.precAssignment;
    opPrecArray[31 /*0x1F*/] = OpPrec.precAssignment;
    opPrecArray[32 /*0x20*/] = OpPrec.precAssignment;
    opPrecArray[33] = OpPrec.precAssignment;
    opPrecArray[34] = OpPrec.precAssignment;
    opPrecArray[35] = OpPrec.precConditional;
    return opPrecArray;
  }

  private static OpPrec[] InitPPOperatorsPrec()
  {
    return new OpPrec[21]
    {
      OpPrec.precAdditive,
      OpPrec.precAdditive,
      OpPrec.precLogicalOr,
      OpPrec.precLogicalAnd,
      OpPrec.precBitwiseOr,
      OpPrec.precBitwiseXor,
      OpPrec.precBitwiseAnd,
      OpPrec.precEquality,
      OpPrec.precEquality,
      OpPrec.precEquality,
      OpPrec.precEquality,
      OpPrec.precRelational,
      OpPrec.precRelational,
      OpPrec.precRelational,
      OpPrec.precRelational,
      OpPrec.precShift,
      OpPrec.precShift,
      OpPrec.precShift,
      OpPrec.precMultiplicative,
      OpPrec.precMultiplicative,
      OpPrec.precMultiplicative
    };
  }
}
