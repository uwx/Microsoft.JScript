// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.JScriptException
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using Microsoft.JScript.Vsa;
using System;
using System.Globalization;
using System.Resources;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;
using System.Text;

#nullable disable
namespace Microsoft.JScript;

/// <summary>The exception that is thrown by JScript to notify a common language runtime (CLR) host or program that an error occurred. A <see cref="T:Microsoft.JScript.JScriptException" /> usually takes a <see cref="T:Microsoft.JScript.JSError" /> enumeration value. </summary>
[Serializable]
public class JScriptException : ApplicationException, IVsaFullErrorInfo, IJSVsaError
{
  internal object value;
  [NonSerialized]
  internal Context context;
  internal bool isError;
  internal static readonly string ContextStringDelimiter = ";;";
  private int code;

  /// <summary>Initializes a new instance of the <see cref="T:Microsoft.JScript.JScriptException" /> class.</summary>
  public JScriptException()
    : this(JSError.NoError)
  {
  }

  /// <summary>Initializes a new instance of the <see cref="T:Microsoft.JScript.JScriptException" /> class, using the specified error message.</summary>
  /// <param name="m">The message that describes the error.</param>
  public JScriptException(string m)
    : this(m, (Exception) null)
  {
  }

  /// <summary>Initializes a new instance of the <see cref="T:Microsoft.JScript.JScriptException" /> class, using the specified error message and a reference to the inner exception that is the cause of this exception.</summary>
  /// <param name="m">The message that describes the error.</param>
  /// <param name="e">The exception that is the cause of the current exception.</param>
  public JScriptException(string m, Exception e)
    : this(m, e, (Context) null)
  {
  }

  /// <summary>Initializes a new instance of the <see cref="T:Microsoft.JScript.JScriptException" /> class, using the specified <see cref="T:Microsoft.JScript.JSError" />.</summary>
  /// <param name="errorNumber">One of the enumeration values. The value specifies the error.</param>
  public JScriptException(JSError errorNumber)
    : this(errorNumber, (Context) null)
  {
  }

  internal JScriptException(JSError errorNumber, Context context)
  {
    this.value = (object) Missing.Value;
    this.context = context;
    this.code = this.HResult = (int) (2148139008L /*0x800A0000*/ + (long) errorNumber);
  }

  internal JScriptException(object value, Context context)
  {
    this.value = value;
    this.context = context;
    this.code = this.HResult = -2146823266;
  }

  internal JScriptException(Exception e, Context context)
    : this((string) null, e, context)
  {
  }

  internal JScriptException(string m, Exception e, Context context)
    : base(m, e)
  {
    this.value = (object) e;
    this.context = context;
    switch (e)
    {
      case StackOverflowException _:
        this.code = this.HResult = -2146828260;
        this.value = (object) Missing.Value;
        break;
      case OutOfMemoryException _:
        this.code = this.HResult = -2146828281 /*0x800A0007*/;
        this.value = (object) Missing.Value;
        break;
      case ExternalException _:
        this.code = this.HResult = ((ExternalException) e).ErrorCode;
        if (((long) this.HResult & 4294901760L) != 2148139008L /*0x800A0000*/ || !Enum.IsDefined(typeof (JSError), (object) (this.HResult & (int) ushort.MaxValue)))
          break;
        this.value = (object) Missing.Value;
        break;
      default:
        int hrForException = Marshal.GetHRForException(e);
        if (((long) hrForException & 4294901760L) == 2148139008L /*0x800A0000*/ && Enum.IsDefined(typeof (JSError), (object) (hrForException & (int) ushort.MaxValue)))
        {
          this.code = this.HResult = hrForException;
          this.value = (object) Missing.Value;
          break;
        }
        this.code = this.HResult = -2146823266;
        break;
    }
  }

  /// <summary>Initializes a new instance of the <see cref="T:Microsoft.JScript.JScriptException" /> class, using the specified serialized data.</summary>
  /// <param name="info">The serialized object data about the exception that is being thrown.</param>
  /// <param name="context">Contextual information about the source or destination.</param>
  protected JScriptException(SerializationInfo info, StreamingContext context)
    : base(info, context)
  {
    this.code = this.HResult = info.GetInt32("Code");
    this.value = (object) Missing.Value;
    this.isError = info.GetBoolean("IsError");
  }

  /// <summary>Gets the name of the source code file that contains the error.</summary>
  /// <returns>The name of the source code file, or the string literal "no source" if the <see cref="T:Microsoft.JScript.Context" /> is null.</returns>
  public string SourceMoniker
  {
    [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")] get
    {
      return this.context != null ? this.context.document.documentName : "no source";
    }
  }

  /// <summary>Gets the column in the code in which the error occurs.</summary>
  /// <returns>The column in the code in which the error occurs, or 0 if the <see cref="T:Microsoft.JScript.Context" /> is null.</returns>
  public int StartColumn
  {
    [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")] get => this.Column;
  }

  /// <summary>Gets the column in the code in which the error occurs.</summary>
  /// <returns>The column in the code in which the error occurs, or 0 if the <see cref="T:Microsoft.JScript.Context" /> is null.</returns>
  public int Column
  {
    get => this.context != null ? this.context.StartColumn + this.context.document.startCol + 1 : 0;
  }

  /// <summary>Gets a message that describes the error.</summary>
  /// <returns>A message that describes the error.</returns>
  string IJSVsaError.Description
  {
    [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")] get => this.Description;
  }

  /// <summary>Gets a message that describes the error.</summary>
  /// <returns>A message that describes the error.</returns>
  public string Description => this.Message;

  /// <summary>Gets the line of code in which the error ends.</summary>
  /// <returns>The line of code in which the error ends, or 0 if the <see cref="T:Microsoft.JScript.Context" /> is null.</returns>
  public int EndLine
  {
    get
    {
      return this.context != null ? this.context.EndLine + this.context.document.startLine - this.context.document.lastLineInSource : 0;
    }
  }

  /// <summary>Gets the column in the code in which the error ends.</summary>
  /// <returns>The column in the code in which the error ends, or 0 if the <see cref="T:Microsoft.JScript.Context" /> is null.</returns>
  public int EndColumn
  {
    [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")] get
    {
      return this.context != null ? this.context.EndColumn + this.context.document.startCol + 1 : 0;
    }
  }

  /// <summary>Gets the <see cref="P:System.Exception.HResult" /> code of the error.</summary>
  /// <returns>The <see cref="P:System.Exception.HResult" /> code of the error.</returns>
  int IJSVsaError.Number
  {
    [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")] get => this.Number;
  }

  /// <summary>Gets the <see cref="P:System.Exception.HResult" /> code of the error.</summary>
  /// <returns>The <see cref="P:System.Exception.HResult" /> code of the error.</returns>
  public int Number => this.ErrorNumber;

  /// <summary>Gets the <see cref="P:System.Exception.HResult" /> code of the error.</summary>
  /// <returns>The <see cref="P:System.Exception.HResult" /> code of the error.</returns>
  public int ErrorNumber => this.HResult;

  internal ErrorType GetErrorType()
  {
    int hresult = this.HResult;
    if (((long) hresult & 4294901760L) != 2148139008L /*0x800A0000*/)
      return ErrorType.OtherError;
    switch ((JSError) (hresult & (int) ushort.MaxValue))
    {
      case JSError.InvalidCall:
        return ErrorType.TypeError;
      case JSError.TypeMismatch:
        return ErrorType.TypeError;
      case JSError.OLENoPropOrMethod:
        return ErrorType.TypeError;
      case JSError.NotCollection:
        return ErrorType.TypeError;
      case JSError.SyntaxError:
        return ErrorType.SyntaxError;
      case JSError.NoColon:
        return ErrorType.SyntaxError;
      case JSError.NoSemicolon:
        return ErrorType.SyntaxError;
      case JSError.NoLeftParen:
        return ErrorType.SyntaxError;
      case JSError.NoRightParen:
        return ErrorType.SyntaxError;
      case JSError.NoRightBracket:
        return ErrorType.SyntaxError;
      case JSError.NoLeftCurly:
        return ErrorType.SyntaxError;
      case JSError.NoRightCurly:
        return ErrorType.SyntaxError;
      case JSError.NoIdentifier:
        return ErrorType.SyntaxError;
      case JSError.NoEqual:
        return ErrorType.SyntaxError;
      case JSError.IllegalChar:
        return ErrorType.SyntaxError;
      case JSError.UnterminatedString:
        return ErrorType.SyntaxError;
      case JSError.NoCommentEnd:
        return ErrorType.SyntaxError;
      case JSError.BadReturn:
        return ErrorType.SyntaxError;
      case JSError.BadBreak:
        return ErrorType.SyntaxError;
      case JSError.BadContinue:
        return ErrorType.SyntaxError;
      case JSError.BadHexDigit:
        return ErrorType.SyntaxError;
      case JSError.NoWhile:
        return ErrorType.SyntaxError;
      case JSError.BadLabel:
        return ErrorType.SyntaxError;
      case JSError.NoLabel:
        return ErrorType.SyntaxError;
      case JSError.DupDefault:
        return ErrorType.SyntaxError;
      case JSError.NoMemberIdentifier:
        return ErrorType.SyntaxError;
      case JSError.NoCcEnd:
        return ErrorType.SyntaxError;
      case JSError.CcOff:
        return ErrorType.SyntaxError;
      case JSError.NotConst:
        return ErrorType.SyntaxError;
      case JSError.NoAt:
        return ErrorType.SyntaxError;
      case JSError.NoCatch:
        return ErrorType.SyntaxError;
      case JSError.InvalidElse:
        return ErrorType.SyntaxError;
      case JSError.NoComma:
        return ErrorType.SyntaxError;
      case JSError.BadSwitch:
        return ErrorType.SyntaxError;
      case JSError.CcInvalidEnd:
        return ErrorType.SyntaxError;
      case JSError.CcInvalidElse:
        return ErrorType.SyntaxError;
      case JSError.CcInvalidElif:
        return ErrorType.SyntaxError;
      case JSError.ErrEOF:
        return ErrorType.SyntaxError;
      case JSError.ClassNotAllowed:
        return ErrorType.SyntaxError;
      case JSError.NeedCompileTimeConstant:
        return ErrorType.ReferenceError;
      case JSError.NeedType:
        return ErrorType.TypeError;
      case JSError.NotInsideClass:
        return ErrorType.SyntaxError;
      case JSError.InvalidPositionDirective:
        return ErrorType.SyntaxError;
      case JSError.MustBeEOL:
        return ErrorType.SyntaxError;
      case JSError.WrongDirective:
        return ErrorType.SyntaxError;
      case JSError.CannotNestPositionDirective:
        return ErrorType.SyntaxError;
      case JSError.CircularDefinition:
        return ErrorType.SyntaxError;
      case JSError.NotAccessible:
        return ErrorType.ReferenceError;
      case JSError.NeedInterface:
        return ErrorType.TypeError;
      case JSError.UnreachableCatch:
        return ErrorType.SyntaxError;
      case JSError.TypeCannotBeExtended:
        return ErrorType.ReferenceError;
      case JSError.UndeclaredVariable:
        return ErrorType.ReferenceError;
      case JSError.KeywordUsedAsIdentifier:
        return ErrorType.SyntaxError;
      case JSError.InvalidCustomAttribute:
        return ErrorType.TypeError;
      case JSError.InvalidCustomAttributeArgument:
        return ErrorType.TypeError;
      case JSError.InvalidCustomAttributeClassOrCtor:
        return ErrorType.TypeError;
      case JSError.NoSuchMember:
        return ErrorType.ReferenceError;
      case JSError.ItemNotAllowedOnExpandoClass:
        return ErrorType.SyntaxError;
      case JSError.NotIndexable:
        return ErrorType.TypeError;
      case JSError.StaticMissingInStaticInit:
        return ErrorType.SyntaxError;
      case JSError.MissingConstructForAttributes:
        return ErrorType.SyntaxError;
      case JSError.OnlyClassesAllowed:
        return ErrorType.SyntaxError;
      case JSError.PackageExpected:
        return ErrorType.SyntaxError;
      case JSError.DifferentReturnTypeFromBase:
        return ErrorType.TypeError;
      case JSError.ClashWithProperty:
        return ErrorType.SyntaxError;
      case JSError.CannotReturnValueFromVoidFunction:
        return ErrorType.TypeError;
      case JSError.AmbiguousMatch:
        return ErrorType.ReferenceError;
      case JSError.AmbiguousConstructorCall:
        return ErrorType.ReferenceError;
      case JSError.SuperClassConstructorNotAccessible:
        return ErrorType.ReferenceError;
      case JSError.NoCommaOrTypeDefinitionError:
        return ErrorType.SyntaxError;
      case JSError.AbstractWithBody:
        return ErrorType.SyntaxError;
      case JSError.NoRightParenOrComma:
        return ErrorType.SyntaxError;
      case JSError.NoRightBracketOrComma:
        return ErrorType.SyntaxError;
      case JSError.ExpressionExpected:
        return ErrorType.SyntaxError;
      case JSError.UnexpectedSemicolon:
        return ErrorType.SyntaxError;
      case JSError.TooManyTokensSkipped:
        return ErrorType.SyntaxError;
      case JSError.BadVariableDeclaration:
        return ErrorType.SyntaxError;
      case JSError.BadFunctionDeclaration:
        return ErrorType.SyntaxError;
      case JSError.BadPropertyDeclaration:
        return ErrorType.SyntaxError;
      case JSError.DoesNotHaveAnAddress:
        return ErrorType.ReferenceError;
      case JSError.TooFewParameters:
        return ErrorType.TypeError;
      case JSError.ImpossibleConversion:
        return ErrorType.TypeError;
      case JSError.NeedInstance:
        return ErrorType.ReferenceError;
      case JSError.InvalidBaseTypeForEnum:
        return ErrorType.TypeError;
      case JSError.CannotInstantiateAbstractClass:
        return ErrorType.TypeError;
      case JSError.ShouldBeAbstract:
        return ErrorType.SyntaxError;
      case JSError.BadModifierInInterface:
        return ErrorType.SyntaxError;
      case JSError.VarIllegalInInterface:
        return ErrorType.SyntaxError;
      case JSError.InterfaceIllegalInInterface:
        return ErrorType.SyntaxError;
      case JSError.NoVarInEnum:
        return ErrorType.SyntaxError;
      case JSError.EnumNotAllowed:
        return ErrorType.SyntaxError;
      case JSError.PackageInWrongContext:
        return ErrorType.SyntaxError;
      case JSError.ConstructorMayNotHaveReturnType:
        return ErrorType.SyntaxError;
      case JSError.OnlyClassesAndPackagesAllowed:
        return ErrorType.SyntaxError;
      case JSError.InvalidDebugDirective:
        return ErrorType.SyntaxError;
      case JSError.NestedInstanceTypeCannotBeExtendedByStatic:
        return ErrorType.ReferenceError;
      case JSError.PropertyLevelAttributesMustBeOnGetter:
        return ErrorType.ReferenceError;
      case JSError.ParamListNotLast:
        return ErrorType.SyntaxError;
      case JSError.InstanceNotAccessibleFromStatic:
        return ErrorType.ReferenceError;
      case JSError.StaticRequiresTypeName:
        return ErrorType.ReferenceError;
      case JSError.NonStaticWithTypeName:
        return ErrorType.ReferenceError;
      case JSError.NoSuchStaticMember:
        return ErrorType.ReferenceError;
      case JSError.ExpectedAssembly:
        return ErrorType.SyntaxError;
      case JSError.AssemblyAttributesMustBeGlobal:
        return ErrorType.SyntaxError;
      case JSError.DuplicateMethod:
        return ErrorType.TypeError;
      case JSError.NotAnExpandoFunction:
        return ErrorType.ReferenceError;
      case JSError.CcInvalidInDebugger:
        return ErrorType.SyntaxError;
      case JSError.TypeNameTooLong:
        return ErrorType.SyntaxError;
      case JSError.MemberInitializerCannotContainFuncExpr:
        return ErrorType.SyntaxError;
      case JSError.CantAssignThis:
        return ErrorType.ReferenceError;
      case JSError.NumberExpected:
        return ErrorType.TypeError;
      case JSError.FunctionExpected:
        return ErrorType.TypeError;
      case JSError.StringExpected:
        return ErrorType.TypeError;
      case JSError.DateExpected:
        return ErrorType.TypeError;
      case JSError.ObjectExpected:
        return ErrorType.TypeError;
      case JSError.IllegalAssignment:
        return ErrorType.ReferenceError;
      case JSError.UndefinedIdentifier:
        return ErrorType.ReferenceError;
      case JSError.BooleanExpected:
        return ErrorType.TypeError;
      case JSError.VBArrayExpected:
        return ErrorType.TypeError;
      case JSError.EnumeratorExpected:
        return ErrorType.TypeError;
      case JSError.RegExpExpected:
        return ErrorType.TypeError;
      case JSError.RegExpSyntax:
        return ErrorType.SyntaxError;
      case JSError.InvalidPrototype:
        return ErrorType.TypeError;
      case JSError.URIEncodeError:
        return ErrorType.URIError;
      case JSError.URIDecodeError:
        return ErrorType.URIError;
      case JSError.FractionOutOfRange:
        return ErrorType.RangeError;
      case JSError.PrecisionOutOfRange:
        return ErrorType.RangeError;
      case JSError.ArrayLengthConstructIncorrect:
        return ErrorType.RangeError;
      case JSError.ArrayLengthAssignIncorrect:
        return ErrorType.RangeError;
      case JSError.NeedArrayObject:
        return ErrorType.TypeError;
      case JSError.NoConstructor:
        return ErrorType.TypeError;
      case JSError.IllegalEval:
        return ErrorType.EvalError;
      case JSError.MustProvideNameForNamedParameter:
        return ErrorType.ReferenceError;
      case JSError.DuplicateNamedParameter:
        return ErrorType.ReferenceError;
      case JSError.MissingNameParameter:
        return ErrorType.ReferenceError;
      case JSError.MoreNamedParametersThanArguments:
        return ErrorType.ReferenceError;
      case JSError.AssignmentToReadOnly:
        return ErrorType.ReferenceError;
      case JSError.WriteOnlyProperty:
        return ErrorType.ReferenceError;
      case JSError.IncorrectNumberOfIndices:
        return ErrorType.ReferenceError;
      default:
        return ErrorType.OtherError;
    }
  }

  /// <summary>Stores information about the exception, including IsError and Code values, in the <see cref="T:System.Runtime.Serialization.SerializationInfo" />.</summary>
  /// <param name="info">The serialized object data about the exception that is being thrown.</param>
  /// <param name="context">Contextual information about the source or destination.</param>
  [SecurityCritical]
  [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
  public override void GetObjectData(SerializationInfo info, StreamingContext context)
  {
    if (info == null)
      throw new ArgumentNullException(nameof (info));
    base.GetObjectData(info, context);
    info.AddValue("IsError", this.isError);
    info.AddValue("Code", this.code);
  }

  /// <summary>Gets the line of code in which the error occurs.</summary>
  /// <returns>The line of code in which the error occurs, or 0 if the <see cref="T:Microsoft.JScript.Context" /> is null.</returns>
  public int Line
  {
    [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")] get
    {
      return this.context != null ? this.context.StartLine + this.context.document.startLine - this.context.document.lastLineInSource : 0;
    }
  }

  /// <summary>Gets the text of the code in which the error occurs.</summary>
  /// <returns>The text of the code, or an empty string if the <see cref="T:Microsoft.JScript.Context" /> is null.</returns>
  public string LineText
  {
    [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")] get
    {
      return this.context != null ? this.context.source_string : "";
    }
  }

  internal static string Localize(string key, CultureInfo culture)
  {
    return JScriptException.Localize(key, (string) null, culture);
  }

  internal static string Localize(string key, string context, CultureInfo culture)
  {
    try
    {
      string str = new ResourceManager("Microsoft.JScript", typeof (JScriptException).Module.Assembly).GetString(key, culture);
      if (str != null)
      {
        int length = str.IndexOf(JScriptException.ContextStringDelimiter);
        if (length == -1)
          return str;
        if (context == null)
          return str.Substring(0, length);
        return string.Format((IFormatProvider) culture, str.Substring(length + 2), new object[1]
        {
          (object) context
        });
      }
    }
    catch (MissingManifestResourceException ex)
    {
    }
    return key;
  }

  /// <summary>Gets a message that describes the error.</summary>
  /// <returns>A message that describes the error.</returns>
  public override string Message
  {
    get
    {
      if (this.value is Exception)
      {
        Exception exception = (Exception) this.value;
        string message = exception.Message;
        return message != null && message.Length > 0 ? message : exception.ToString();
      }
      string key = (this.HResult & (int) ushort.MaxValue).ToString((IFormatProvider) CultureInfo.InvariantCulture);
      CultureInfo culture = (CultureInfo) null;
      if (this.context != null && this.context.document != null)
      {
        VsaEngine engine = this.context.document.engine;
        if (engine != null)
          culture = engine.ErrorCultureInfo;
      }
      if (this.value is ErrorObject)
      {
        string message = ((ErrorObject) this.value).Message;
        return message != null && message.Length > 0 ? message : $"{JScriptException.Localize("No description available", culture)}: {key}";
      }
      if (this.value is string)
      {
        switch ((JSError) (this.HResult & (int) ushort.MaxValue))
        {
          case JSError.TypeMismatch:
          case JSError.DuplicateName:
          case JSError.Deprecated:
          case JSError.MustImplementMethod:
          case JSError.TypeCannotBeExtended:
          case JSError.NoSuchMember:
          case JSError.NotIndexable:
          case JSError.HidesParentMember:
          case JSError.HidesAbstractInBase:
          case JSError.DifferentReturnTypeFromBase:
          case JSError.CannotBeAbstract:
          case JSError.InvalidCustomAttributeTarget:
          case JSError.NoSuchType:
          case JSError.NoSuchStaticMember:
          case JSError.ImplicitlyReferencedAssemblyNotFound:
          case JSError.InvalidResource:
          case JSError.IncompatibleAssemblyReference:
          case JSError.InvalidAssemblyKeyFile:
          case JSError.TypeNameTooLong:
            return JScriptException.Localize(key, (string) this.value, culture);
          default:
            return (string) this.value;
        }
      }
      else
      {
        if (this.context != null)
        {
          switch ((JSError) (this.HResult & (int) ushort.MaxValue))
          {
            case JSError.DuplicateName:
            case JSError.NotAccessible:
            case JSError.UndeclaredVariable:
            case JSError.VariableLeftUninitialized:
            case JSError.KeywordUsedAsIdentifier:
            case JSError.NotMeantToBeCalledDirectly:
            case JSError.AmbiguousBindingBecauseOfWith:
            case JSError.AmbiguousBindingBecauseOfEval:
            case JSError.NotDeletable:
            case JSError.VariableMightBeUnitialized:
            case JSError.NeedInstance:
            case JSError.InstanceNotAccessibleFromStatic:
            case JSError.StaticRequiresTypeName:
            case JSError.NonStaticWithTypeName:
            case JSError.ObjectExpected:
            case JSError.UndefinedIdentifier:
            case JSError.AssignmentToReadOnly:
              return JScriptException.Localize(key, this.context.GetCode(), culture);
          }
        }
        return JScriptException.Localize((this.HResult & (int) ushort.MaxValue).ToString((IFormatProvider) CultureInfo.InvariantCulture), culture);
      }
    }
  }

  /// <summary>Gets a number between 0 (most severe) and 4 (least severe) that indicates the severity of the error. 0 is the default.</summary>
  /// <returns>A number between 0 and 4.</returns>
  public int Severity
  {
    [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")] get
    {
      int hresult = this.HResult;
      if (((long) hresult & 4294901760L) != 2148139008L /*0x800A0000*/ || this.isError)
        return 0;
      switch ((JSError) (hresult & (int) ushort.MaxValue))
      {
        case JSError.DupVisibility:
          return 1;
        case JSError.IncompatibleVisibility:
          return 1;
        case JSError.DuplicateName:
          return 1;
        case JSError.Deprecated:
          return 2;
        case JSError.UndeclaredVariable:
          return 3;
        case JSError.VariableLeftUninitialized:
          return 3;
        case JSError.KeywordUsedAsIdentifier:
          return 2;
        case JSError.NotMeantToBeCalledDirectly:
          return 1;
        case JSError.GetAndSetAreInconsistent:
          return 1;
        case JSError.TooManyParameters:
          return 1;
        case JSError.AmbiguousBindingBecauseOfWith:
          return 4;
        case JSError.AmbiguousBindingBecauseOfEval:
          return 4;
        case JSError.BaseClassIsExpandoAlready:
          return 1;
        case JSError.NotDeletable:
          return 1;
        case JSError.UselessExpression:
          return 1;
        case JSError.HidesParentMember:
          return 1;
        case JSError.NewNotSpecifiedInMethodDeclaration:
          return 1;
        case JSError.DifferentReturnTypeFromBase:
          return 1;
        case JSError.OctalLiteralsAreDeprecated:
          return 2;
        case JSError.VariableMightBeUnitialized:
          return 3;
        case JSError.BadWayToLeaveFinally:
          return 3;
        case JSError.TooFewParameters:
          return 1;
        case JSError.UselessAssignment:
          return 1;
        case JSError.SuspectAssignment:
          return 1;
        case JSError.SuspectSemicolon:
          return 1;
        case JSError.ArrayMayBeCopied:
          return 1;
        case JSError.ShouldBeAbstract:
          return 1;
        case JSError.BadOctalLiteral:
          return 1;
        case JSError.SuspectLoopCondition:
          return 1;
        case JSError.StringConcatIsSlow:
          return 3;
        case JSError.PossibleBadConversion:
          return 1;
        case JSError.PossibleBadConversionFromString:
          return 4;
        case JSError.IncompatibleAssemblyReference:
          return 1;
        case JSError.AssignmentToReadOnly:
          return 1;
        default:
          return 0;
      }
    }
  }

  /// <summary>Gets the source code item that caused the error.</summary>
  /// <returns>The source code item; or throws a <see cref="T:Microsoft.JScript.NoContextException" /> if the <see cref="T:Microsoft.JScript.Context" /> is null.</returns>
  public IJSVsaItem SourceItem
  {
    [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")] get
    {
      if (this.context != null)
        return (IJSVsaItem) this.context.document.sourceItem;
      throw new NoContextException();
    }
  }

  /// <summary>Gets the stack trace information at the time of the error.</summary>
  /// <returns>The stack trace information.</returns>
  public override string StackTrace
  {
    get
    {
      if (this.context == null)
        return this.Message + Environment.NewLine + base.StackTrace;
      StringBuilder stringBuilder = new StringBuilder();
      Context context = this.context;
      string documentName = context.document.documentName;
      if (documentName != null && documentName.Length > 0)
        stringBuilder.Append(documentName + ": ");
      CultureInfo culture = (CultureInfo) null;
      if (this.context != null && this.context.document != null)
      {
        VsaEngine engine = this.context.document.engine;
        if (engine != null)
          culture = engine.ErrorCultureInfo;
      }
      stringBuilder.Append(JScriptException.Localize("Line", culture));
      stringBuilder.Append(' ');
      stringBuilder.Append(context.StartLine);
      stringBuilder.Append(" - ");
      stringBuilder.Append(JScriptException.Localize("Error", culture));
      stringBuilder.Append(": ");
      stringBuilder.Append(this.Message);
      stringBuilder.Append(Environment.NewLine);
      if (context.document.engine != null)
      {
        Stack callContextStack = context.document.engine.Globals.CallContextStack;
        int i = 0;
        for (int index = callContextStack.Size(); i < index; ++i)
        {
          CallContext callContext = (CallContext) callContextStack.Peek(i);
          stringBuilder.Append("    ");
          stringBuilder.Append(JScriptException.Localize("at call to", culture));
          stringBuilder.Append(callContext.FunctionName());
          stringBuilder.Append(' ');
          stringBuilder.Append(JScriptException.Localize("in line", culture));
          stringBuilder.Append(": ");
          stringBuilder.Append(callContext.sourceContext.EndLine);
        }
      }
      return stringBuilder.ToString();
    }
  }
}
