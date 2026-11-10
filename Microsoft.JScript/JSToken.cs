// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.JSToken
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

#nullable disable
namespace Microsoft.JScript;

/// <summary>Specifies the individual units of code, or tokens, that make up the JScript language.</summary>
public enum JSToken
{
    /// <summary>No token. For example, before scanning any tokens, or in error situations.</summary>
    None = -1, // 0xFFFFFFFF

    /// <summary>The end of the file being scanned or parsed. This is the default value.</summary>
    EndOfFile = 0,

    /// <summary>The if statement.</summary>
    If = 1,

    /// <summary>The for statement.</summary>
    For = 2,

    /// <summary>The do statement.</summary>
    Do = 3,

    /// <summary>The while statement.</summary>
    While = 4,

    /// <summary>The continue statement.</summary>
    Continue = 5,

    /// <summary>The break statement.</summary>
    Break = 6,

    /// <summary>The return statement.</summary>
    Return = 7,

    /// <summary>The import statement.</summary>
    Import = 8,

    /// <summary>The with statement.</summary>
    With = 9,

    /// <summary>The switch statement.</summary>
    Switch = 10, // 0x0000000A

    /// <summary>The throw statement.</summary>
    Throw = 11, // 0x0000000B

    /// <summary>The try statement.</summary>
    Try = 12, // 0x0000000C

    /// <summary>The package statement.</summary>
    Package = 13, // 0x0000000D

    /// <summary>The internal modifier.</summary>
    Internal = 14, // 0x0000000E

    /// <summary>The abstract modifier.</summary>
    Abstract = 15, // 0x0000000F

    /// <summary>The public modifier.</summary>
    Public = 16, // 0x00000010

    /// <summary>The static modifier.</summary>
    Static = 17, // 0x00000011

    /// <summary>The private modifier.</summary>
    Private = 18, // 0x00000012

    /// <summary>The protected modifier.</summary>
    Protected = 19, // 0x00000013

    /// <summary>The final modifier.</summary>
    Final = 20, // 0x00000014

    /// <summary>The event reserved word.</summary>
    Event = 21, // 0x00000015

    /// <summary>The var statement.</summary>
    Var = 22, // 0x00000016

    /// <summary>The const statement.</summary>
    Const = 23, // 0x00000017

    /// <summary>The class statement.</summary>
    Class = 24, // 0x00000018

    /// <summary>The function statement.</summary>
    Function = 25, // 0x00000019

    /// <summary>The { symbol.</summary>
    LeftCurly = 26, // 0x0000001A

    /// <summary>The ; symbol.</summary>
    Semicolon = 27, // 0x0000001B

    /// <summary>The null literal.</summary>
    Null = 28, // 0x0000001C

    /// <summary>The true literal.</summary>
    True = 29, // 0x0000001D

    /// <summary>The false literal.</summary>
    False = 30, // 0x0000001E

    /// <summary>The this statement.</summary>
    This = 31, // 0x0000001F

    /// <summary>An identifier.</summary>
    Identifier = 32, // 0x00000020

    /// <summary>A numeric literal.</summary>
    StringLiteral = 33, // 0x00000021

    /// <summary>An integer literal.</summary>
    IntegerLiteral = 34, // 0x00000022

    /// <summary>A numeric literal.</summary>
    NumericLiteral = 35, // 0x00000023

    /// <summary>The ( symbol.</summary>
    LeftParen = 36, // 0x00000024

    /// <summary>The [ symbol.</summary>
    LeftBracket = 37, // 0x00000025

    /// <summary>The . field accessor symbol.</summary>
    AccessField = 38, // 0x00000026

    /// <summary>A placeholder enumeration member that enables comparisons such as if JSToken.FirstOp &lt;= token &amp;&amp; token &lt;= JSToken.LastOp then...</summary>
    FirstOp = 39, // 0x00000027

    /// <summary>The ! logical NOT operator.</summary>
    LogicalNot = 39, // 0x00000027

    /// <summary>The ~ bitwise NOT operator.</summary>
    BitwiseNot = 40, // 0x00000028

    /// <summary>The delete operator.</summary>
    Delete = 41, // 0x00000029

    /// <summary>The void operator.</summary>
    Void = 42, // 0x0000002A

    /// <summary>The typeof operator.</summary>
    Typeof = 43, // 0x0000002B

    /// <summary>The ++ increment operator.</summary>
    Increment = 44, // 0x0000002C

    /// <summary>The -- decrement operator.</summary>
    Decrement = 45, // 0x0000002D

    /// <summary>A placeholder enumeration member that enables comparisons such as if JSToken.FirstBinaryOp &lt;= token &amp;&amp; token &lt;= JSToken.LastBinaryOp then...</summary>
    FirstBinaryOp = 46, // 0x0000002E

    /// <summary>The + addition operator.</summary>
    Plus = 46, // 0x0000002E

    /// <summary>The - subtraction operator.</summary>
    Minus = 47, // 0x0000002F

    /// <summary>The || logical OR operator.</summary>
    LogicalOr = 48, // 0x00000030

    /// <summary>The &amp;&amp; logical AND operator.</summary>
    LogicalAnd = 49, // 0x00000031

    /// <summary>The | bitwise OR operator.</summary>
    BitwiseOr = 50, // 0x00000032

    /// <summary>The ^ bitwise XOR operator.</summary>
    BitwiseXor = 51, // 0x00000033

    /// <summary>The &amp; bitwise AND operator.</summary>
    BitwiseAnd = 52, // 0x00000034

    /// <summary>The == equality comparison operator.</summary>
    Equal = 53, // 0x00000035

    /// <summary>The != inequality comparison operator.</summary>
    NotEqual = 54, // 0x00000036

    /// <summary>The === identity comparison operator.</summary>
    StrictEqual = 55, // 0x00000037

    /// <summary>The !== identity comparison operator.</summary>
    StrictNotEqual = 56, // 0x00000038

    /// <summary>The &gt; relational comparison operator.</summary>
    GreaterThan = 57, // 0x00000039

    /// <summary>The &lt; relational comparison operator.</summary>
    LessThan = 58, // 0x0000003A

    /// <summary>The &lt;= relational comparison operator.</summary>
    LessThanEqual = 59, // 0x0000003B

    /// <summary>The &gt;= relational comparison operator.</summary>
    GreaterThanEqual = 60, // 0x0000003C

    /// <summary>The &lt;&lt; bitwise left shift operator.</summary>
    LeftShift = 61, // 0x0000003D

    /// <summary>The &gt;&gt; bitwise right shift operator.</summary>
    RightShift = 62, // 0x0000003E

    /// <summary>The &gt;&gt;&gt; unsigned right shift operator.</summary>
    UnsignedRightShift = 63, // 0x0000003F

    /// <summary>The * multiplication operator.</summary>
    Multiply = 64, // 0x00000040

    /// <summary>The / division operator.</summary>
    Divide = 65, // 0x00000041

    /// <summary>A placeholder enumeration member that enables comparisons such as if JSToken.FirstBinaryOp &lt;= token &amp;&amp; token &lt;= JSToken.LastPPOperator then... PPOperators are operations that can be pre-processed.</summary>
    LastPPOperator = 66, // 0x00000042

    /// <summary>The % modulus operator.</summary>
    Modulo = 66, // 0x00000042

    /// <summary>The instanceof operator.</summary>
    Instanceof = 67, // 0x00000043

    /// <summary>The in operator.</summary>
    In = 68, // 0x00000044

    /// <summary>The = assignment operator.</summary>
    Assign = 69, // 0x00000045

    /// <summary>The += addition assignment operator.</summary>
    PlusAssign = 70, // 0x00000046

    /// <summary>The -= subtraction assignment operator.</summary>
    MinusAssign = 71, // 0x00000047

    /// <summary>The *= multiplication assignment operator.</summary>
    MultiplyAssign = 72, // 0x00000048

    /// <summary>The /= division assignment operator.</summary>
    DivideAssign = 73, // 0x00000049

    /// <summary>The &amp;= bitwise AND assignment operator.</summary>
    BitwiseAndAssign = 74, // 0x0000004A

    /// <summary>The |= bitwise OR assignment operator.</summary>
    BitwiseOrAssign = 75, // 0x0000004B

    /// <summary>The ^= bitwise XOR assignment operator.</summary>
    BitwiseXorAssign = 76, // 0x0000004C

    /// <summary>The %= modulus assignment operator.</summary>
    ModuloAssign = 77, // 0x0000004D

    /// <summary>The &lt;&lt;= left shift assignment operator.</summary>
    LeftShiftAssign = 78, // 0x0000004E

    /// <summary>The &gt;&gt;= right shift assignment operator.</summary>
    RightShiftAssign = 79, // 0x0000004F

    /// <summary>A placeholder enumeration member that enables comparisons such as if JSToken.Assign &lt;= token &amp;&amp; token &lt;= JSToken.LastAssign then...</summary>
    LastAssign = 80, // 0x00000050

    /// <summary>A placeholder enumeration member that enables comparisons such as if JSToken.FirstBinaryOp &lt;= token &amp;&amp; token &lt;= JSToken.LastBinaryOp then...</summary>
    LastBinaryOp = 80, // 0x00000050

    /// <summary>The &gt;&gt;&gt;= unsigned right shift assignment operator.</summary>
    UnsignedRightShiftAssign = 80, // 0x00000050

    /// <summary>The ? ternary operator.</summary>
    ConditionalIf = 81, // 0x00000051

    /// <summary>The : ternary operator.</summary>
    Colon = 82, // 0x00000052

    /// <summary>The , comma operator.</summary>
    Comma = 83, // 0x00000053

    /// <summary>A placeholder enumeration member that enables comparisons such as if JSToken.FirstBinaryOp &lt;= token &amp;&amp; token &lt;= JSToken.LastOp then...</summary>
    LastOp = 83, // 0x00000053

    /// <summary>The case keyword of a switch statement.</summary>
    Case = 84, // 0x00000054

    /// <summary>The catch keyword of a try statement.</summary>
    Catch = 85, // 0x00000055

    /// <summary>The debugger statement.</summary>
    Debugger = 86, // 0x00000056

    /// <summary>The default keyword of a switch statement.</summary>
    Default = 87, // 0x00000057

    /// <summary>The else keyword of an if statement.</summary>
    Else = 88, // 0x00000058

    /// <summary>The export reserved word.</summary>
    Export = 89, // 0x00000059

    /// <summary>The extends keyword of a class statement.</summary>
    Extends = 90, // 0x0000005A

    /// <summary>The finally keyword of a try statement.</summary>
    Finally = 91, // 0x0000005B

    /// <summary>The get keyword of a function get statement.</summary>
    Get = 92, // 0x0000005C

    /// <summary>The implements keyword of a class or interface statement.</summary>
    Implements = 93, // 0x0000005D

    /// <summary>The interface statement.</summary>
    Interface = 94, // 0x0000005E

    /// <summary>The new operator.</summary>
    New = 95, // 0x0000005F

    /// <summary>The set keyword of a function set statement.</summary>
    Set = 96, // 0x00000060

    /// <summary>The super statement.</summary>
    Super = 97, // 0x00000061

    /// <summary>The ) symbol.</summary>
    RightParen = 98, // 0x00000062

    /// <summary>The } symbol.</summary>
    RightCurly = 99, // 0x00000063

    /// <summary>The ] symbol.</summary>
    RightBracket = 100, // 0x00000064

    /// <summary>A preprocessor constant.</summary>
    PreProcessorConstant = 101, // 0x00000065

    /// <summary>Comment text.</summary>
    Comment = 102, // 0x00000066

    /// <summary>An un-terminated comment.</summary>
    UnterminatedComment = 103, // 0x00000067

    /// <summary>The assert reserved word.</summary>
    Assert = 104, // 0x00000068

    /// <summary>The boolean data type.</summary>
    Boolean = 105, // 0x00000069

    /// <summary>The byte data type.</summary>
    Byte = 106, // 0x0000006A

    /// <summary>The char data type.</summary>
    Char = 107, // 0x0000006B

    /// <summary>The decimal data type.</summary>
    Decimal = 108, // 0x0000006C

    /// <summary>The double data type.</summary>
    Double = 109, // 0x0000006D

    /// <summary>The :: reserved word.</summary>
    DoubleColon = 110, // 0x0000006E

    /// <summary>The enum statement.</summary>
    Enum = 111, // 0x0000006F

    /// <summary>The ensure reserved word.</summary>
    Ensure = 112, // 0x00000070

    /// <summary>The float data type.</summary>
    Float = 113, // 0x00000071

    /// <summary>The goto reserved word.</summary>
    Goto = 114, // 0x00000072

    /// <summary>The int data type.</summary>
    Int = 115, // 0x00000073

    /// <summary>The invariant reserved word.</summary>
    Invariant = 116, // 0x00000074

    /// <summary>The long data type.</summary>
    Long = 117, // 0x00000075

    /// <summary>The namespace reserved word.</summary>
    Namespace = 118, // 0x00000076

    /// <summary>The native reserved word.</summary>
    Native = 119, // 0x00000077

    /// <summary>The require reserved word.</summary>
    Require = 120, // 0x00000078

    /// <summary>The sbyte data type.</summary>
    Sbyte = 121, // 0x00000079

    /// <summary>The short data type.</summary>
    Short = 122, // 0x0000007A

    /// <summary>The synchronized reserved word.</summary>
    Synchronized = 123, // 0x0000007B

    /// <summary>The transient reserved word.</summary>
    Transient = 124, // 0x0000007C

    /// <summary>The throws reserved word.</summary>
    Throws = 125, // 0x0000007D

    /// <summary>The ... reserved word.</summary>
    ParamArray = 126, // 0x0000007E

    /// <summary>The volatile reserved word.</summary>
    Volatile = 127, // 0x0000007F

    /// <summary>The ushort data type.</summary>
    Ushort = 128, // 0x00000080

    /// <summary>The uint data type.</summary>
    Uint = 129, // 0x00000081

    /// <summary>The ulong data type.</summary>
    Ulong = 130, // 0x00000082

    /// <summary>The use reserved word.</summary>
    Use = 131, // 0x00000083

    /// <summary>The end of the line being parsed.</summary>
    EndOfLine = 132, // 0x00000084

    /// <summary>A preprocessor directive.</summary>
    PreProcessDirective = 133, // 0x00000085
}