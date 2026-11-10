// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.JSError
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System.Runtime.InteropServices;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Specifies the JScript error messages. This class belongs to the compilation and run-time state category.</summary>
[Guid("268CA962-2FEF-3152-BA46-E18658B7FA4F")]
[ComVisible(true)]
public enum JSError
{
  /// <summary>No error. This is the default (0) value for the enumeration.</summary>
  NoError = 0,
  /// <summary>Invalid procedure call or argument.</summary>
  InvalidCall = 5,
  /// <summary>Out of memory.</summary>
  OutOfMemory = 7,
  /// <summary>Type mismatch.</summary>
  TypeMismatch = 13, // 0x0000000D
  /// <summary>Out of stack space.</summary>
  OutOfStack = 28, // 0x0000001C
  /// <summary>An internal error has been thrown.</summary>
  InternalError = 51, // 0x00000033
  /// <summary>File not found.</summary>
  FileNotFound = 53, // 0x00000035
  /// <summary>An object is required.</summary>
  NeedObject = 424, // 0x000001A8
  /// <summary>Cannot create object.</summary>
  CantCreateObject = 429, // 0x000001AD
  /// <summary>Object does not support this property or method.</summary>
  OLENoPropOrMethod = 438, // 0x000001B6
  /// <summary>The object does not support this action.</summary>
  ActionNotSupported = 445, // 0x000001BD
  /// <summary>This object is not a collection.</summary>
  NotCollection = 451, // 0x000001C3
  /// <summary>Discovered syntax error.</summary>
  SyntaxError = 1002, // 0x000003EA
  /// <summary>Expected ":".</summary>
  NoColon = 1003, // 0x000003EB
  /// <summary>Expected ";".</summary>
  NoSemicolon = 1004, // 0x000003EC
  /// <summary>Expected "(".</summary>
  NoLeftParen = 1005, // 0x000003ED
  /// <summary>Expected ")".</summary>
  NoRightParen = 1006, // 0x000003EE
  /// <summary>Expected "]".</summary>
  NoRightBracket = 1007, // 0x000003EF
  /// <summary>Expected "{".</summary>
  NoLeftCurly = 1008, // 0x000003F0
  /// <summary>Expected "}".</summary>
  NoRightCurly = 1009, // 0x000003F1
  /// <summary>Expected identifier.</summary>
  NoIdentifier = 1010, // 0x000003F2
  /// <summary>Expected "=".</summary>
  NoEqual = 1011, // 0x000003F3
  /// <summary>Invalid character.</summary>
  IllegalChar = 1014, // 0x000003F6
  /// <summary>String constant not terminated.</summary>
  UnterminatedString = 1015, // 0x000003F7
  /// <summary>Comment not terminated.</summary>
  NoCommentEnd = 1016, // 0x000003F8
  /// <summary>Return statement cannot appear outside the function.</summary>
  BadReturn = 1018, // 0x000003FA
  /// <summary>Cannot have break outside the loop.</summary>
  BadBreak = 1019, // 0x000003FB
  /// <summary>Cannot have continue outside the loop.</summary>
  BadContinue = 1020, // 0x000003FC
  /// <summary>Expected hexadecimal digit.</summary>
  BadHexDigit = 1023, // 0x000003FF
  /// <summary>Expected while.</summary>
  NoWhile = 1024, // 0x00000400
  /// <summary>There is already a label that has that name.</summary>
  BadLabel = 1025, // 0x00000401
  /// <summary>Label not found.</summary>
  NoLabel = 1026, // 0x00000402
  /// <summary>default can appear only once in a switch statement.</summary>
  DupDefault = 1027, // 0x00000403
  /// <summary>Expected identifier or string.</summary>
  NoMemberIdentifier = 1028, // 0x00000404
  /// <summary>Expected @end.</summary>
  NoCcEnd = 1029, // 0x00000405
  /// <summary>Conditional compilation is turned off.</summary>
  CcOff = 1030, // 0x00000406
  /// <summary>Expected constant.</summary>
  NotConst = 1031, // 0x00000407
  /// <summary>Expected @.</summary>
  NoAt = 1032, // 0x00000408
  /// <summary>Expected catch.</summary>
  NoCatch = 1033, // 0x00000409
  /// <summary>Unmatched else; no if defined.</summary>
  InvalidElse = 1034, // 0x0000040A
  /// <summary>Expected ",".</summary>
  NoComma = 1100, // 0x0000044C
  /// <summary>Visibility modifier already defined.</summary>
  DupVisibility = 1101, // 0x0000044D
  /// <summary>Invalid visibility modifier.</summary>
  IllegalVisibility = 1102, // 0x0000044E
  /// <summary>Missing case or default statement.</summary>
  BadSwitch = 1103, // 0x0000044F
  /// <summary>Unmatched @end; no @if defined.</summary>
  CcInvalidEnd = 1104, // 0x00000450
  /// <summary>Unmatched @else; no @if defined.</summary>
  CcInvalidElse = 1105, // 0x00000451
  /// <summary>Unmatched @elif; no @if defined.</summary>
  CcInvalidElif = 1106, // 0x00000452
  /// <summary>Expecting more source characters.</summary>
  ErrEOF = 1107, // 0x00000453
  /// <summary>Incompatible visibility modifier.</summary>
  IncompatibleVisibility = 1108, // 0x00000454
  /// <summary>Class definition not allowed in this context.</summary>
  ClassNotAllowed = 1109, // 0x00000455
  /// <summary>An expression must be a compile-time constant.</summary>
  NeedCompileTimeConstant = 1110, // 0x00000456
  /// <summary>Identifier already in use.</summary>
  DuplicateName = 1111, // 0x00000457
  /// <summary>A type name is expected.</summary>
  NeedType = 1112, // 0x00000458
  /// <summary>Valid only inside a class definition.</summary>
  NotInsideClass = 1113, // 0x00000459
  /// <summary>An unknown position directive was discovered.</summary>
  InvalidPositionDirective = 1114, // 0x0000045A
  /// <summary>Directive cannot be followed by other code on the same line.</summary>
  MustBeEOL = 1115, // 0x0000045B
  /// <summary>Wrong debugger directive or wrong position for the directive.</summary>
  WrongDirective = 1118, // 0x0000045E
  /// <summary>Position directive must be ended before a new one can be started.</summary>
  CannotNestPositionDirective = 1119, // 0x0000045F
  /// <summary>Circular definition.</summary>
  CircularDefinition = 1120, // 0x00000460
  /// <summary>The specified type is deprecated.</summary>
  Deprecated = 1121, // 0x00000461
  /// <summary>It is not valid to use this in the current context.</summary>
  IllegalUseOfThis = 1122, // 0x00000462
  /// <summary>The object or member is not accessible from this scope.</summary>
  NotAccessible = 1123, // 0x00000463
  /// <summary>Only a constructor function can have the same name as the class it appears in.</summary>
  CannotUseNameOfClass = 1124, // 0x00000464
  /// <summary>The class must provide an implementation of the method.</summary>
  MustImplementMethod = 1128, // 0x00000468
  /// <summary>An interface name is expected.</summary>
  NeedInterface = 1129, // 0x00000469
  /// <summary>Catch clause will never be reached.</summary>
  UnreachableCatch = 1133, // 0x0000046D
  /// <summary>Type cannot be extended.</summary>
  TypeCannotBeExtended = 1134, // 0x0000046E
  /// <summary>Variable has not been declared.</summary>
  UndeclaredVariable = 1135, // 0x0000046F
  /// <summary>Uninitialized variables are dangerous and slow to use. Did you intend to leave it uninitialized?</summary>
  VariableLeftUninitialized = 1136, // 0x00000470
  /// <summary>You cannot use a reserved word as an identifier.</summary>
  KeywordUsedAsIdentifier = 1137, // 0x00000471
  /// <summary>Object or member is not allowed in a call to the base class constructor.</summary>
  NotAllowedInSuperConstructorCall = 1140, // 0x00000474
  /// <summary>You cannot call this method directly. If possible, use property accessors instead.</summary>
  NotMeantToBeCalledDirectly = 1141, // 0x00000475
  /// <summary>The get and set methods of this property do not match each other.</summary>
  GetAndSetAreInconsistent = 1142, // 0x00000476
  /// <summary>A custom attribute class must derive from <see cref="T:System.Attribute" />.</summary>
  InvalidCustomAttribute = 1143, // 0x00000477
  /// <summary>Only primitive types are allowed in a custom attribute constructor arguments list.</summary>
  InvalidCustomAttributeArgument = 1144, // 0x00000478
  /// <summary>Unknown custom attribute class or constructor.</summary>
  InvalidCustomAttributeClassOrCtor = 1146, // 0x0000047A
  /// <summary>There are too many actual parameters. The excess parameters will be ignored.</summary>
  TooManyParameters = 1148, // 0x0000047C
  /// <summary>The <see cref="T:Microsoft.JScript.With" /> statement has made the use of this name ambiguous.</summary>
  AmbiguousBindingBecauseOfWith = 1149, // 0x0000047D
  /// <summary>The presence of <see cref="T:Microsoft.JScript.Eval" /> has made the use of this name ambiguous.</summary>
  AmbiguousBindingBecauseOfEval = 1150, // 0x0000047E
  /// <summary>Objects of this type do not have such a member.</summary>
  NoSuchMember = 1151, // 0x0000047F
  /// <summary>Cannot define the property Item on an <see cref="T:Microsoft.JScript.Expando" /> class. This Item is reserved for the <see cref="T:Microsoft.JScript.Expando" /> fields.</summary>
  ItemNotAllowedOnExpandoClass = 1152, // 0x00000480
  /// <summary>Cannot define a property named Item on an <see cref="T:Microsoft.JScript.Expando" /> class.</summary>
  MethodNotAllowedOnExpandoClass = 1153, // 0x00000481
  /// <summary>Cannot create the <see cref="T:Microsoft.JScript.Expando" /> class because a property named Item is already defined in the class hierarchy.</summary>
  MethodClashOnExpandoSuperClass = 1155, // 0x00000483
  /// <summary>A base class is already marked <see cref="T:Microsoft.JScript.Expando" />; current specification will be ignored.</summary>
  BaseClassIsExpandoAlready = 1156, // 0x00000484
  /// <summary>An abstract method cannot be private.</summary>
  AbstractCannotBePrivate = 1157, // 0x00000485
  /// <summary>Objects of this type are not indexable.</summary>
  NotIndexable = 1158, // 0x00000486
  /// <summary>Static initializer must specify the static keyword.</summary>
  StaticMissingInStaticInit = 1159, // 0x00000487
  /// <summary>The list of attributes does not apply to the current context.</summary>
  MissingConstructForAttributes = 1160, // 0x00000488
  /// <summary>Only classes are allowed inside a package.</summary>
  OnlyClassesAllowed = 1161, // 0x00000489
  /// <summary>
  /// <see cref="T:Microsoft.JScript.Expando" /> classes should not implement <see cref="T:System.Collections.IEnumerable" />. The interface is implicitly defined on <see cref="T:Microsoft.JScript.Expando" /> classes.</summary>
  ExpandoClassShouldNotImpleEnumerable = 1162, // 0x0000048A
  /// <summary>The specified member is not CLS compliant.</summary>
  NonCLSCompliantMember = 1163, // 0x0000048B
  /// <summary>The object or member cannot be deleted.</summary>
  NotDeletable = 1164, // 0x0000048C
  /// <summary>Package name expected.</summary>
  PackageExpected = 1165, // 0x0000048D
  /// <summary>Expression has no effect.</summary>
  UselessExpression = 1169, // 0x00000491
  /// <summary>base class already contains a member by this name.</summary>
  HidesParentMember = 1170, // 0x00000492
  /// <summary>Cannot change visibility specification of a base method.</summary>
  CannotChangeVisibility = 1171, // 0x00000493
  /// <summary>Method hides abstract in a base class.</summary>
  HidesAbstractInBase = 1172, // 0x00000494
  /// <summary>A method matches a method in a base class. Must specify override or hide.</summary>
  NewNotSpecifiedInMethodDeclaration = 1173, // 0x00000495
  /// <summary>A method in a base class that is final or not virtualoverride is ignored. Specify hide.</summary>
  MethodInBaseIsNotVirtual = 1174, // 0x00000496
  /// <summary>There is no member in a base class to hide.</summary>
  NoMethodInBaseToNew = 1175, // 0x00000497
  /// <summary>Method in base has a different return type.</summary>
  DifferentReturnTypeFromBase = 1176, // 0x00000498
  /// <summary>The name of the field conflicts with the name of the property.</summary>
  ClashWithProperty = 1177, // 0x00000499
  /// <summary>Cannot use override and hide together in a member declaration.</summary>
  OverrideAndHideUsedTogether = 1178, // 0x0000049A
  /// <summary>Must specify either "fast" or "versionSafe" language option.</summary>
  InvalidLanguageOption = 1179, // 0x0000049B
  /// <summary>There is no member in a base class to override.</summary>
  NoMethodInBaseToOverride = 1180, // 0x0000049C
  /// <summary>Not valid for a constructor.</summary>
  NotValidForConstructor = 1181, // 0x0000049D
  /// <summary>Cannot return a value from a void function or constructor.</summary>
  CannotReturnValueFromVoidFunction = 1182, // 0x0000049E
  /// <summary>More than one method or property matches this parameter list.</summary>
  AmbiguousMatch = 1183, // 0x0000049F
  /// <summary>More than one constructor matches this parameter list.</summary>
  AmbiguousConstructorCall = 1184, // 0x000004A0
  /// <summary>Base class constructor is not accessible from this scope.</summary>
  SuperClassConstructorNotAccessible = 1185, // 0x000004A1
  /// <summary>Octal literals are deprecated.</summary>
  OctalLiteralsAreDeprecated = 1186, // 0x000004A2
  /// <summary>Variable might not be initialized.</summary>
  VariableMightBeUnitialized = 1187, // 0x000004A3
  /// <summary>It is not valid to call a base class constructor from this location.</summary>
  NotOKToCallSuper = 1188, // 0x000004A4
  /// <summary>It is not valid to use the base class in this way.</summary>
  IllegalUseOfSuper = 1189, // 0x000004A5
  /// <summary>It is slow and potentially confusing to leave a finally block this way.</summary>
  BadWayToLeaveFinally = 1190, // 0x000004A6
  /// <summary>Expected "," or invalid type declaration; write "&lt;Identifier&gt; : &lt;Type&gt;" not "&lt;Type&gt; &lt;Identifier&gt;".</summary>
  NoCommaOrTypeDefinitionError = 1191, // 0x000004A7
  /// <summary>An abstract function cannot have a body.</summary>
  AbstractWithBody = 1192, // 0x000004A8
  /// <summary>Expected "," or ")".</summary>
  NoRightParenOrComma = 1193, // 0x000004A9
  /// <summary>Expected "," or "]".</summary>
  NoRightBracketOrComma = 1194, // 0x000004AA
  /// <summary>Expected expression.</summary>
  ExpressionExpected = 1195, // 0x000004AB
  /// <summary>Unexpected ";".</summary>
  UnexpectedSemicolon = 1196, // 0x000004AC
  /// <summary>Too many errors. The file might not be a JScript file.</summary>
  TooManyTokensSkipped = 1197, // 0x000004AD
  /// <summary>Possible invalid variable declaration, var missing, or unrecognized syntax error.</summary>
  BadVariableDeclaration = 1198, // 0x000004AE
  /// <summary>Possible invalid function declaration, function missing, or unrecognized syntax error.</summary>
  BadFunctionDeclaration = 1199, // 0x000004AF
  /// <summary>Invalid property declaration. The get accessor must not have arguments and the set accessor must have one argument.</summary>
  BadPropertyDeclaration = 1200, // 0x000004B0
  /// <summary>Expression does not have an address.</summary>
  DoesNotHaveAnAddress = 1203, // 0x000004B3
  /// <summary>Not all required parameters have been supplied.</summary>
  TooFewParameters = 1204, // 0x000004B4
  /// <summary>An assignment creates an <see cref="T:Microsoft.JScript.Expando" /> property that is immediately thrown away.</summary>
  UselessAssignment = 1205, // 0x000004B5
  /// <summary>If condition cannot contain an assignment.</summary>
  SuspectAssignment = 1206, // 0x000004B6
  /// <summary>Empty statement found in if statement.</summary>
  SuspectSemicolon = 1207, // 0x000004B7
  /// <summary>The specified conversion or coercion is not possible.</summary>
  ImpossibleConversion = 1208, // 0x000004B8
  /// <summary>final and abstract cannot be used together.</summary>
  FinalPrecludesAbstract = 1209, // 0x000004B9
  /// <summary>An instance is expected.</summary>
  NeedInstance = 1210, // 0x000004BA
  /// <summary>Cannot be abstract unless class is marked as abstract.</summary>
  CannotBeAbstract = 1212, // 0x000004BC
  /// <summary>enum base type must be a primitive integral type.</summary>
  InvalidBaseTypeForEnum = 1213, // 0x000004BD
  /// <summary>It is not possible to construct an instance of an abstract class.</summary>
  CannotInstantiateAbstractClass = 1214, // 0x000004BE
  /// <summary>Assigning a JScript array to a <see cref="T:System.Array" /> may cause the array to be copied.</summary>
  ArrayMayBeCopied = 1215, // 0x000004BF
  /// <summary>Static methods cannot be abstract.</summary>
  AbstractCannotBeStatic = 1216, // 0x000004C0
  /// <summary>Static methods cannot be final.</summary>
  StaticIsAlreadyFinal = 1217, // 0x000004C1
  /// <summary>Static methods cannot override base class methods.</summary>
  StaticMethodsCannotOverride = 1218, // 0x000004C2
  /// <summary>Static methods cannot hide base class methods.</summary>
  StaticMethodsCannotHide = 1219, // 0x000004C3
  /// <summary>
  /// <see cref="T:Microsoft.JScript.Expando" /> methods cannot override base class methods.</summary>
  ExpandoPrecludesOverride = 1220, // 0x000004C4
  /// <summary>A variable argument list must be of an array type.</summary>
  IllegalParamArrayAttribute = 1221, // 0x000004C5
  /// <summary>
  /// <see cref="T:Microsoft.JScript.Expando" /> methods cannot be abstract.</summary>
  ExpandoPrecludesAbstract = 1222, // 0x000004C6
  /// <summary>A function without a body should be abstract.</summary>
  ShouldBeAbstract = 1223, // 0x000004C7
  /// <summary>This modifier cannot be used on an interface member.</summary>
  BadModifierInInterface = 1224, // 0x000004C8
  /// <summary>Variables cannot be declared in an interface.</summary>
  VarIllegalInInterface = 1226, // 0x000004CA
  /// <summary>Interfaces cannot be declared in an interface.</summary>
  InterfaceIllegalInInterface = 1227, // 0x000004CB
  /// <summary>enum member declarations should not use the var keyword.</summary>
  NoVarInEnum = 1228, // 0x000004CC
  /// <summary>The import statement is not valid in this context.</summary>
  InvalidImport = 1229, // 0x000004CD
  /// <summary>enum definition not allowed in this context.</summary>
  EnumNotAllowed = 1230, // 0x000004CE
  /// <summary>This attribute is not valid for this type of declaration.</summary>
  InvalidCustomAttributeTarget = 1231, // 0x000004CF
  /// <summary>Package definition is not allowed in this context.</summary>
  PackageInWrongContext = 1232, // 0x000004D0
  /// <summary>A constructor cannot have a return type.</summary>
  ConstructorMayNotHaveReturnType = 1233, // 0x000004D1
  /// <summary>Only classes and packages are allowed inside a library.</summary>
  OnlyClassesAndPackagesAllowed = 1234, // 0x000004D2
  /// <summary>Invalid debug directive.</summary>
  InvalidDebugDirective = 1235, // 0x000004D3
  /// <summary>This type of attribute must be unique.</summary>
  CustomAttributeUsedMoreThanOnce = 1236, // 0x000004D4
  /// <summary>A non-static nested type can be extended only by a non-static type nested in the same class.</summary>
  NestedInstanceTypeCannotBeExtendedByStatic = 1237, // 0x000004D5
  /// <summary>An attribute that targets the property must be specified on the get accessor, if the get accessor is present.</summary>
  PropertyLevelAttributesMustBeOnGetter = 1238, // 0x000004D6
  /// <summary>A throw statement must have an argument unless it is inside the catch block of a try statement.</summary>
  BadThrow = 1239, // 0x000004D7
  /// <summary>A variable argument list must be the last argument.</summary>
  ParamListNotLast = 1240, // 0x000004D8
  /// <summary>Type could not be found. An assembly reference might be missing.</summary>
  NoSuchType = 1241, // 0x000004D9
  /// <summary>Malformed octal literal treated as decimal literal.</summary>
  BadOctalLiteral = 1242, // 0x000004DA
  /// <summary>A non-static member is not accessible from a static scope.</summary>
  InstanceNotAccessibleFromStatic = 1243, // 0x000004DB
  /// <summary>A static member must be accessed by the class name.</summary>
  StaticRequiresTypeName = 1244, // 0x000004DC
  /// <summary>A non-static member cannot be accessed with the class name.</summary>
  NonStaticWithTypeName = 1245, // 0x000004DD
  /// <summary>Type does not have such a static member.</summary>
  NoSuchStaticMember = 1246, // 0x000004DE
  /// <summary>Loop condition cannot contain a function call.</summary>
  SuspectLoopCondition = 1247, // 0x000004DF
  /// <summary>Expected assembly.</summary>
  ExpectedAssembly = 1248, // 0x000004E0
  /// <summary>Assembly custom attributes cannot be part of another construct.</summary>
  AssemblyAttributesMustBeGlobal = 1249, // 0x000004E1
  /// <summary>
  /// <see cref="T:Microsoft.JScript.Expando" /> methods cannot be static.</summary>
  ExpandoPrecludesStatic = 1250, // 0x000004E2
  /// <summary>This method has the same name, parameter types, and return type as another method in this class.</summary>
  DuplicateMethod = 1251, // 0x000004E3
  /// <summary>Class members used as constructors should be marked as <see cref="T:Microsoft.JScript.Expando" /> functions.</summary>
  NotAnExpandoFunction = 1252, // 0x000004E4
  /// <summary>Not a valid version string.</summary>
  NotValidVersionString = 1253, // 0x000004E5
  /// <summary>Executables cannot be localized; <see cref="P:System.Reflection.AssemblyCultureAttribute.Culture" /> should always be empty.</summary>
  ExecutablesCannotBeLocalized = 1254, // 0x000004E6
  /// <summary>The plus operator is a slow way to concatenate strings. Consider using <see cref="T:System.Text.StringBuilder" /> instead.</summary>
  StringConcatIsSlow = 1255, // 0x000004E7
  /// <summary>Conditional compilation directives and variables cannot be used in the debugger.</summary>
  CcInvalidInDebugger = 1256, // 0x000004E8
  /// <summary>
  /// <see cref="T:Microsoft.JScript.Expando" /> methods must be public.</summary>
  ExpandoMustBePublic = 1257, // 0x000004E9
  /// <summary>Delegates should not be explicitly constructed; simply use the method name.</summary>
  DelegatesShouldNotBeExplicitlyConstructed = 1258, // 0x000004EA
  /// <summary>A referenced assembly depends on another assembly that is not referenced or could not be found.</summary>
  ImplicitlyReferencedAssemblyNotFound = 1259, // 0x000004EB
  /// <summary>This conversion may fail at runtime.</summary>
  PossibleBadConversion = 1260, // 0x000004EC
  /// <summary>Converting a string to a number or Boolean is slow and may fail at run time.</summary>
  PossibleBadConversionFromString = 1261, // 0x000004ED
  /// <summary>This is not a valid .resources file.</summary>
  InvalidResource = 1262, // 0x000004EE
  /// <summary>The address of operator can be used only in a list of arguments.</summary>
  WrongUseOfAddressOf = 1263, // 0x000004EF
  /// <summary>The specified type is not CLS compliant.</summary>
  NonCLSCompliantType = 1264, // 0x000004F0
  /// <summary>Class member cannot be marked Common Language Specification (CLS) compliant because the class is not marked CLS compliant.</summary>
  MemberTypeCLSCompliantMismatch = 1265, // 0x000004F1
  /// <summary>Type cannot be marked CLS compliant because the assembly is not marked as CLS compliant.</summary>
  TypeAssemblyCLSCompliantMismatch = 1266, // 0x000004F2
  /// <summary>The assembly referenced is incompatible.</summary>
  IncompatibleAssemblyReference = 1267, // 0x000004F3
  /// <summary>An invalid assembly key file has been used.</summary>
  InvalidAssemblyKeyFile = 1268, // 0x000004F4
  /// <summary>The fully qualified type name is too long. It must be less than 1,024 characters.</summary>
  TypeNameTooLong = 1269, // 0x000004F5
  /// <summary>A member initializer cannot contain a function expression.</summary>
  MemberInitializerCannotContainFuncExpr = 1270, // 0x000004F6
  /// <summary>Cannot assign to this.</summary>
  CantAssignThis = 5000, // 0x00001388
  /// <summary>Expected a number.</summary>
  NumberExpected = 5001, // 0x00001389
  /// <summary>A function was expected.</summary>
  FunctionExpected = 5002, // 0x0000138A
  /// <summary>Cannot assign to a function result.</summary>
  CannotAssignToFunctionResult = 5003, // 0x0000138B
  /// <summary>String expected.</summary>
  StringExpected = 5005, // 0x0000138D
  /// <summary>Date object expected.</summary>
  DateExpected = 5006, // 0x0000138E
  /// <summary>Expected an object.</summary>
  ObjectExpected = 5007, // 0x0000138F
  /// <summary>Invalid assignment.</summary>
  IllegalAssignment = 5008, // 0x00001390
  /// <summary>Undefined identifier.</summary>
  UndefinedIdentifier = 5009, // 0x00001391
  /// <summary>No Boolean value found.</summary>
  BooleanExpected = 5010, // 0x00001392
  /// <summary>A VBArray is expected.</summary>
  VBArrayExpected = 5013, // 0x00001395
  /// <summary>Enumerator object expected.</summary>
  EnumeratorExpected = 5015, // 0x00001397
  /// <summary>
  /// <see cref="T:Microsoft.JScript.RegExpObject" /> object expected.</summary>
  RegExpExpected = 5016, // 0x00001398
  /// <summary>Syntax error in regular expression.</summary>
  RegExpSyntax = 5017, // 0x00001399
  /// <summary>Exception thrown and not caught.</summary>
  UncaughtException = 5022, // 0x0000139E
  /// <summary>This function does not have a valid prototype object.</summary>
  InvalidPrototype = 5023, // 0x0000139F
  /// <summary>The URI to be encoded contains an invalid character.</summary>
  URIEncodeError = 5024, // 0x000013A0
  /// <summary>The URI to be decoded is not a valid encoding.</summary>
  URIDecodeError = 5025, // 0x000013A1
  /// <summary>The number of fractional digits is out of range.</summary>
  FractionOutOfRange = 5026, // 0x000013A2
  /// <summary>The precision is out of range.</summary>
  PrecisionOutOfRange = 5027, // 0x000013A3
  /// <summary>Array length must be a finite positive integer.</summary>
  ArrayLengthConstructIncorrect = 5029, // 0x000013A5
  /// <summary>Array length must be assigned a finite positive number.</summary>
  ArrayLengthAssignIncorrect = 5030, // 0x000013A6
  /// <summary>A "|" is not an array object. Array object expected.</summary>
  NeedArrayObject = 5031, // 0x000013A7
  /// <summary>No such constructor.</summary>
  NoConstructor = 5032, // 0x000013A8
  /// <summary>An <see cref="T:Microsoft.JScript.Eval" /> cannot be called by an alias.</summary>
  IllegalEval = 5033, // 0x000013A9
  /// <summary>Not yet implemented.</summary>
  NotYetImplemented = 5034, // 0x000013AA
  /// <summary>Cannot provide null or empty named parameter name.</summary>
  MustProvideNameForNamedParameter = 5035, // 0x000013AB
  /// <summary>Duplicate named parameter.</summary>
  DuplicateNamedParameter = 5036, // 0x000013AC
  /// <summary>The specified named parameter name is not one of the formal parameters.</summary>
  MissingNameParameter = 5037, // 0x000013AD
  /// <summary>Too few arguments specified. The number of named parameter names cannot exceed the number of arguments passed in.</summary>
  MoreNamedParametersThanArguments = 5038, // 0x000013AE
  /// <summary>The expression cannot be evaluated in the debugger.</summary>
  NonSupportedInDebugger = 5039, // 0x000013AF
  /// <summary>Cannot assign to read-only field or property.</summary>
  AssignmentToReadOnly = 5040, // 0x000013B0
  /// <summary>The property is write-only.</summary>
  WriteOnlyProperty = 5041, // 0x000013B1
  /// <summary>The number of indices does not match the dimension of the array.</summary>
  IncorrectNumberOfIndices = 5042, // 0x000013B2
  /// <summary>Methods with ref parameters cannot be called in the debugger.</summary>
  RefParamsNonSupportedInDebugger = 5043, // 0x000013B3
  /// <summary>The <see cref="M:System.Security.CodeAccessPermission.Deny" />, <see cref="M:System.Security.CodeAccessPermission.PermitOnly" />, and <see cref="M:System.Security.CodeAccessPermission.Assert" /> security methods cannot be called using late binding.</summary>
  CannotCallSecurityMethodLateBound = 5044, // 0x000013B4
  /// <summary>JScript does not support static security attributes.</summary>
  CannotUseStaticSecurityAttribute = 5045, // 0x000013B5
  /// <summary>A target threw a non-CLS exception.</summary>
  NonClsException = 5046, // 0x000013B6
  /// <summary>Function evaluation was aborted.</summary>
  FuncEvalAborted = 6000, // 0x00001770
  /// <summary>Function evaluation timed out.</summary>
  FuncEvalTimedout = 6001, // 0x00001771
  /// <summary>Function evaluation failed. The thread is suspended.</summary>
  FuncEvalThreadSuspended = 6002, // 0x00001772
  /// <summary>Function evaluation failed. The thread is sleeping, waiting for an object, or waiting for another thread to finish.</summary>
  FuncEvalThreadSleepWaitJoin = 6003, // 0x00001773
  /// <summary>Function evaluation failed. The thread can contain incorrect data.</summary>
  FuncEvalBadThreadState = 6004, // 0x00001774
  /// <summary>Function evaluation failed and the thread has not started.</summary>
  FuncEvalBadThreadNotStarted = 6005, // 0x00001775
  /// <summary>Function evaluation aborted. To turn on property evaluation, click Options on the Tools menu and then select the Debugging in the tree view.</summary>
  NoFuncEvalAllowed = 6006, // 0x00001776
  /// <summary>Function evaluation cannot be done when stopped at this point in the program.</summary>
  FuncEvalBadLocation = 6007, // 0x00001777
  /// <summary>Cannot call a Web method in the debugger.</summary>
  FuncEvalWebMethod = 6008, // 0x00001778
  /// <summary>A static variable is not available.</summary>
  StaticVarNotAvailable = 6009, // 0x00001779
  /// <summary>The type object for this type is not available.</summary>
  TypeObjectNotAvailable = 6010, // 0x0000177A
  /// <summary>Exception from HRESULT.</summary>
  ExceptionFromHResult = 6011, // 0x0000177B
  /// <summary>The expression causes side effects and will not be evaluated.</summary>
  SideEffectsDisallowed = 6012, // 0x0000177C
}
