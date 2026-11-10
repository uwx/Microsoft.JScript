// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.JSParser
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Collections;
using System.Reflection;
using System.Security.Permissions;
using System.Text.RegularExpressions;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Parses JScript code. This class belongs to the scanning and parsing category. For more information about scanning and parsing, see Create a Language Compiler for the .NET Framework.</summary>
[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
public class JSParser
{
  private bool demandFullTrustOnFunctionCreation;
  private Context sourceContext;
  private JSScanner scanner;
  private Context currentToken;
  private Context errorToken;
  private int tokensSkipped;
  private const int c_MaxSkippedTokenNumber = 50;
  private NoSkipTokenSet noSkipTokenSet;
  private long goodTokensProcessed;
  private Block program;
  private ArrayList blockType;
  private SimpleHashtable labelTable;
  private int finallyEscaped;
  private int breakRecursion;
  private static int s_cDummyName;
  private Globals Globals;
  private int Severity;

  /// <summary>Initializes a new instance of the <see cref="T:Microsoft.JScript.JSParser" /> class, specifying the source code to parse.</summary>
  /// <param name="context">The source code to parse.</param>
  public JSParser(Context context)
  {
    this.sourceContext = context;
    this.currentToken = context.Clone();
    this.scanner = new JSScanner(this.currentToken);
    this.noSkipTokenSet = new NoSkipTokenSet();
    this.errorToken = (Context) null;
    this.program = (Block) null;
    this.blockType = new ArrayList(16 /*0x10*/);
    this.labelTable = new SimpleHashtable(16U /*0x10*/);
    this.finallyEscaped = 0;
    this.Globals = context.document.engine.Globals;
    this.Severity = 5;
    this.demandFullTrustOnFunctionCreation = false;
  }

  /// <summary>Parses the source code.</summary>
  /// <returns>The root of the abstract syntax tree that is created by parsing the code.</returns>
  public ScriptBlock Parse()
  {
    Block statements = this.ParseStatements(false);
    return new ScriptBlock(this.sourceContext.Clone(), statements);
  }

  /// <summary>Parses the source code. Use this method instead of <see cref="M:Microsoft.JScript.JSParser.Parse" /> when the source code is inside an eval method call.</summary>
  /// <returns>The root of the abstract syntax tree that is created by parsing the code.</returns>
  public Block ParseEvalBody()
  {
    this.demandFullTrustOnFunctionCreation = true;
    return this.ParseStatements(true);
  }

  internal ScriptBlock ParseExpressionItem()
  {
    int i = this.Globals.ScopeStack.Size();
    try
    {
      Block statement_block = new Block(this.sourceContext.Clone());
      this.GetNextToken();
      statement_block.Append((AST) new Expression(this.sourceContext.Clone(), this.ParseExpression()));
      return new ScriptBlock(this.sourceContext.Clone(), statement_block);
    }
    catch (EndOfFile ex)
    {
    }
    catch (ScannerException ex)
    {
      this.EOFError(ex.m_errorId);
    }
    catch (StackOverflowException ex)
    {
      this.Globals.ScopeStack.TrimToSize(i);
      this.ReportError(JSError.OutOfStack, true);
    }
    return (ScriptBlock) null;
  }

  private Block ParseStatements(bool insideEval)
  {
    int i = this.Globals.ScopeStack.Size();
    this.program = new Block(this.sourceContext.Clone());
    this.blockType.Add((object) JSParser.BlockType.Block);
    this.errorToken = (Context) null;
    try
    {
      this.GetNextToken();
      this.noSkipTokenSet.Add(NoSkipTokenSet.s_StartStatementNoSkipTokenSet);
      this.noSkipTokenSet.Add(NoSkipTokenSet.s_TopLevelNoSkipTokenSet);
      try
      {
        while (this.currentToken.token != JSToken.EndOfFile)
        {
          AST elem = (AST) null;
          try
          {
            if (this.currentToken.token == JSToken.Package && !insideEval)
              elem = this.ParsePackage(this.currentToken.Clone());
            else if (this.currentToken.token == JSToken.Import && !insideEval)
            {
              this.noSkipTokenSet.Add(NoSkipTokenSet.s_EndOfStatementNoSkipTokenSet);
              try
              {
                elem = (AST) this.ParseImportStatement();
              }
              catch (RecoveryTokenException ex)
              {
                elem = this.IndexOfToken(NoSkipTokenSet.s_EndOfStatementNoSkipTokenSet, ex) != -1 ? ex._partiallyComputedNode : throw ex;
                if (ex._token == JSToken.Semicolon)
                  this.GetNextToken();
              }
              finally
              {
                this.noSkipTokenSet.Remove(NoSkipTokenSet.s_EndOfStatementNoSkipTokenSet);
              }
            }
            else
              elem = this.ParseStatement();
          }
          catch (RecoveryTokenException ex)
          {
            if (this.TokenInList(NoSkipTokenSet.s_TopLevelNoSkipTokenSet, ex) || this.TokenInList(NoSkipTokenSet.s_StartStatementNoSkipTokenSet, ex))
            {
              elem = ex._partiallyComputedNode;
            }
            else
            {
              this.errorToken = (Context) null;
              do
              {
                this.GetNextToken();
                if (this.currentToken.token != JSToken.EndOfFile)
                {
                  if (this.TokenInList(NoSkipTokenSet.s_TopLevelNoSkipTokenSet, this.currentToken.token))
                    break;
                }
                else
                  break;
              }
              while (!this.TokenInList(NoSkipTokenSet.s_StartStatementNoSkipTokenSet, this.currentToken.token));
            }
          }
          if (elem != null)
            this.program.Append(elem);
        }
      }
      finally
      {
        this.noSkipTokenSet.Remove(NoSkipTokenSet.s_TopLevelNoSkipTokenSet);
        this.noSkipTokenSet.Remove(NoSkipTokenSet.s_StartStatementNoSkipTokenSet);
      }
    }
    catch (EndOfFile ex)
    {
    }
    catch (ScannerException ex)
    {
      this.EOFError(ex.m_errorId);
    }
    catch (StackOverflowException ex)
    {
      this.Globals.ScopeStack.TrimToSize(i);
      this.ReportError(JSError.OutOfStack, true);
    }
    return this.program;
  }

  private AST ParseStatement()
  {
    AST statement1 = (AST) null;
    string str = (string) null;
    switch (this.currentToken.token)
    {
      case JSToken.EndOfFile:
        this.EOFError(JSError.ErrEOF);
        throw new EndOfFile();
      case JSToken.If:
        return (AST) this.ParseIfStatement();
      case JSToken.For:
        return this.ParseForStatement();
      case JSToken.Do:
        return (AST) this.ParseDoStatement();
      case JSToken.While:
        return (AST) this.ParseWhileStatement();
      case JSToken.Continue:
        return (AST) this.ParseContinueStatement() ?? (AST) new Block(this.CurrentPositionContext());
      case JSToken.Break:
        return (AST) this.ParseBreakStatement() ?? (AST) new Block(this.CurrentPositionContext());
      case JSToken.Return:
        return (AST) this.ParseReturnStatement() ?? (AST) new Block(this.CurrentPositionContext());
      case JSToken.Import:
        this.ReportError(JSError.InvalidImport, true);
        statement1 = (AST) new Block(this.currentToken.Clone());
        try
        {
          this.ParseImportStatement();
          break;
        }
        catch (RecoveryTokenException ex)
        {
          break;
        }
      case JSToken.With:
        return (AST) this.ParseWithStatement();
      case JSToken.Switch:
        return this.ParseSwitchStatement();
      case JSToken.Throw:
        statement1 = this.ParseThrowStatement();
        if (statement1 == null)
          return (AST) new Block(this.CurrentPositionContext());
        break;
      case JSToken.Try:
        return this.ParseTryStatement();
      case JSToken.Package:
        Context context = this.currentToken.Clone();
        statement1 = this.ParsePackage(context);
        if (statement1 is Package)
        {
          this.ReportError(JSError.PackageInWrongContext, context, true);
          statement1 = (AST) new Block(context);
          break;
        }
        break;
      case JSToken.Internal:
      case JSToken.Abstract:
      case JSToken.Public:
      case JSToken.Static:
      case JSToken.Private:
      case JSToken.Protected:
      case JSToken.Final:
        bool parsedOK1;
        AST attributes = this.ParseAttributes((AST) null, false, false, out parsedOK1);
        if (parsedOK1)
          return attributes;
        AST expression = this.ParseExpression(attributes, false, true, JSToken.None);
        statement1 = (AST) new Expression(expression.context.Clone(), expression);
        break;
      case JSToken.Var:
      case JSToken.Const:
        return this.ParseVariableStatement(FieldAttributes.PrivateScope, (CustomAttributeList) null, this.currentToken.token);
      case JSToken.Class:
      case JSToken.Interface:
        return this.ParseClass(FieldAttributes.PrivateScope, false, this.currentToken.Clone(), false, false, (CustomAttributeList) null);
      case JSToken.Function:
        return this.ParseFunction(FieldAttributes.PrivateScope, false, this.currentToken.Clone(), false, false, false, false, (CustomAttributeList) null);
      case JSToken.LeftCurly:
        return (AST) this.ParseBlock();
      case JSToken.Semicolon:
        AST statement2 = (AST) new Block(this.currentToken.Clone());
        this.GetNextToken();
        return statement2;
      case JSToken.This:
      case JSToken.Super:
        Context superCtx = this.currentToken.Clone();
        if (JSToken.LeftParen == this.scanner.PeekToken())
        {
          statement1 = this.ParseConstructorCall(superCtx);
          break;
        }
        goto default;
      case JSToken.Debugger:
        statement1 = (AST) new DebugBreak(this.currentToken.Clone());
        this.GetNextToken();
        break;
      case JSToken.Else:
        this.ReportError(JSError.InvalidElse);
        this.SkipTokensAndThrow();
        break;
      case JSToken.RightCurly:
        this.ReportError(JSError.SyntaxError);
        this.SkipTokensAndThrow();
        break;
      case JSToken.Enum:
        return this.ParseEnum(FieldAttributes.PrivateScope, this.currentToken.Clone(), (CustomAttributeList) null);
      default:
        this.noSkipTokenSet.Add(NoSkipTokenSet.s_EndOfStatementNoSkipTokenSet);
        bool flag = false;
        try
        {
          bool canBeAttribute = true;
          bool isLeftHandSideExpr;
          statement1 = this.ParseUnaryExpression(out isLeftHandSideExpr, ref canBeAttribute, false);
          if (canBeAttribute)
          {
            if (statement1 is Lookup && JSToken.Colon == this.currentToken.token)
            {
              string key = statement1.ToString();
              if (this.labelTable[(object) key] != null)
              {
                this.ReportError(JSError.BadLabel, statement1.context.Clone(), true);
                str = (string) null;
                this.GetNextToken();
                return (AST) new Block(this.CurrentPositionContext());
              }
              this.GetNextToken();
              this.labelTable[(object) key] = (object) this.blockType.Count;
              statement1 = this.currentToken.token == JSToken.EndOfFile ? (AST) new Block(this.CurrentPositionContext()) : this.ParseStatement();
              this.labelTable.Remove((object) key);
              return statement1;
            }
            if (JSToken.Semicolon != this.currentToken.token && !this.scanner.GotEndOfLine())
            {
              bool parsedOK2;
              statement1 = this.ParseAttributes(statement1, false, false, out parsedOK2);
              if (parsedOK2)
                return statement1;
            }
          }
          statement1 = this.ParseExpression(statement1, false, isLeftHandSideExpr, JSToken.None);
          statement1 = (AST) new Expression(statement1.context.Clone(), statement1);
          break;
        }
        catch (RecoveryTokenException ex)
        {
          if (ex._partiallyComputedNode != null)
            statement1 = ex._partiallyComputedNode;
          if (statement1 == null)
          {
            this.noSkipTokenSet.Remove(NoSkipTokenSet.s_EndOfStatementNoSkipTokenSet);
            flag = true;
            this.SkipTokensAndThrow();
          }
          if (this.IndexOfToken(NoSkipTokenSet.s_EndOfStatementNoSkipTokenSet, ex) == -1)
          {
            ex._partiallyComputedNode = statement1;
            throw ex;
          }
          break;
        }
        finally
        {
          if (!flag)
            this.noSkipTokenSet.Remove(NoSkipTokenSet.s_EndOfStatementNoSkipTokenSet);
        }
    }
    if (JSToken.Semicolon == this.currentToken.token)
    {
      statement1.context.UpdateWith(this.currentToken);
      this.GetNextToken();
    }
    else if (!this.scanner.GotEndOfLine() && JSToken.RightCurly != this.currentToken.token && this.currentToken.token != JSToken.EndOfFile)
      this.ReportError(JSError.NoSemicolon, true);
    return statement1;
  }

  private AST ParseAttributes(
    AST statement,
    bool unambiguousContext,
    bool isInsideClass,
    out bool parsedOK)
  {
    AST ast1 = statement;
    ArrayList arrayList1 = new ArrayList();
    ArrayList arrayList2 = new ArrayList();
    ArrayList arrayList3 = new ArrayList();
    AST ast2 = (AST) null;
    ArrayList attributes = new ArrayList();
    Context context1 = (Context) null;
    Context context2 = (Context) null;
    Context context3 = (Context) null;
    int num = 0;
    if (unambiguousContext)
      num = 2;
    FieldAttributes fieldAttributes1 = FieldAttributes.PrivateScope;
    FieldAttributes fieldAttributes2 = FieldAttributes.PrivateScope;
    Context context4;
    if (statement != null)
    {
      ast2 = statement;
      attributes.Add((object) statement);
      arrayList1.Add((object) this.CurrentPositionContext());
      context4 = statement.context.Clone();
      num = 1;
    }
    else
      context4 = this.currentToken.Clone();
    parsedOK = true;
    while (true)
    {
      do
      {
        JSToken jsToken = JSToken.None;
        switch (this.currentToken.token)
        {
          case JSToken.Internal:
          case JSToken.Abstract:
          case JSToken.Public:
          case JSToken.Static:
          case JSToken.Private:
          case JSToken.Protected:
          case JSToken.Final:
            jsToken = this.currentToken.token;
            goto case JSToken.Identifier;
          case JSToken.Var:
          case JSToken.Const:
            int index1 = 0;
            for (int count = arrayList3.Count; index1 < count; index1 += 2)
              this.ReportError((JSError) arrayList3[index1], (Context) arrayList3[index1 + 1], true);
            if (context1 != null)
              this.ReportError(JSError.IllegalVisibility, context1, true);
            if (context3 != null)
              this.ReportError(JSError.IllegalVisibility, context3, true);
            context4.UpdateWith(this.currentToken);
            return this.ParseVariableStatement(fieldAttributes1, this.FromASTListToCustomAttributeList(attributes), this.currentToken.token);
          case JSToken.Class:
            int index2 = 0;
            for (int count = arrayList3.Count; index2 < count; index2 += 2)
              this.ReportError((JSError) arrayList3[index2], (Context) arrayList3[index2 + 1], true);
            context4.UpdateWith(this.currentToken);
            if (context3 != null && context1 != null)
              context3.HandleError(JSError.FinalPrecludesAbstract);
            return this.ParseClass(fieldAttributes1, context2 != null, context4, context1 != null, context3 != null, this.FromASTListToCustomAttributeList(attributes));
          case JSToken.Function:
            int index3 = 0;
            for (int count = arrayList3.Count; index3 < count; index3 += 2)
              this.ReportError((JSError) arrayList3[index3], (Context) arrayList3[index3 + 1], true);
            context4.UpdateWith(this.currentToken);
            if (context2 != null)
            {
              if (context1 != null)
                context2.HandleError(JSError.AbstractCannotBeStatic);
              else if (context3 != null)
              {
                context3.HandleError(JSError.StaticIsAlreadyFinal);
                context3 = (Context) null;
              }
            }
            if (context1 != null)
            {
              if (context3 != null)
              {
                context3.HandleError(JSError.FinalPrecludesAbstract);
                context3 = (Context) null;
              }
              if (fieldAttributes2 == FieldAttributes.Private)
                context1.HandleError(JSError.AbstractCannotBePrivate);
            }
            return this.ParseFunction(fieldAttributes1, false, context4, isInsideClass, context1 != null, context3 != null, false, this.FromASTListToCustomAttributeList(attributes));
          case JSToken.Identifier:
            bool canBeAttribute = true;
            statement = this.ParseUnaryExpression(out bool _, ref canBeAttribute, false, jsToken == JSToken.None);
            ast2 = statement;
            if (jsToken != JSToken.None)
            {
              if (!(statement is Lookup))
              {
                if (num != 2)
                  arrayList2.Add((object) this.currentToken.Clone());
              }
              else
                break;
            }
            jsToken = JSToken.None;
            if (canBeAttribute)
            {
              attributes.Add((object) statement);
              goto label_133;
            }
            goto default;
          case JSToken.Void:
          case JSToken.Boolean:
          case JSToken.Byte:
          case JSToken.Char:
          case JSToken.Double:
          case JSToken.Float:
          case JSToken.Int:
          case JSToken.Long:
          case JSToken.Short:
            parsedOK = false;
            ast2 = (AST) new Lookup(this.currentToken);
            attributes.Add((object) ast2);
            this.GetNextToken();
            goto label_133;
          case JSToken.Interface:
            if (context1 != null)
            {
              this.ReportError(JSError.IllegalVisibility, context1, true);
              context1 = (Context) null;
            }
            if (context3 != null)
            {
              this.ReportError(JSError.IllegalVisibility, context3, true);
              context3 = (Context) null;
            }
            if (context2 != null)
            {
              this.ReportError(JSError.IllegalVisibility, context2, true);
              context2 = (Context) null;
              goto case JSToken.Class;
            }
            goto case JSToken.Class;
          case JSToken.Enum:
            int index4 = 0;
            for (int count = arrayList3.Count; index4 < count; index4 += 2)
              this.ReportError((JSError) arrayList3[index4], (Context) arrayList3[index4 + 1], true);
            context4.UpdateWith(this.currentToken);
            if (context1 != null)
              this.ReportError(JSError.IllegalVisibility, context1, true);
            if (context3 != null)
              this.ReportError(JSError.IllegalVisibility, context3, true);
            if (context2 != null)
              this.ReportError(JSError.IllegalVisibility, context2, true);
            return this.ParseEnum(fieldAttributes1, context4, this.FromASTListToCustomAttributeList(attributes));
          default:
            parsedOK = false;
            if (num != 2)
            {
              if (ast1 != statement || statement == null)
              {
                statement = ast2;
                int index5 = 0;
                for (int count = arrayList2.Count; index5 < count; ++index5)
                  this.ForceReportInfo((Context) arrayList2[index5], JSError.KeywordUsedAsIdentifier);
                int index6 = 0;
                for (int count = arrayList1.Count; index6 < count; ++index6)
                {
                  if (!this.currentToken.Equals((Context) arrayList1[index6]))
                    this.ReportError(JSError.NoSemicolon, (Context) arrayList1[index6], true);
                }
              }
              return statement;
            }
            if (attributes.Count > 0)
            {
              AST ast3 = (AST) attributes[attributes.Count - 1];
              if (ast3 is Lookup)
              {
                if (JSToken.Semicolon == this.currentToken.token || JSToken.Colon == this.currentToken.token)
                {
                  this.ReportError(JSError.BadVariableDeclaration, ast3.context.Clone());
                  this.SkipTokensAndThrow();
                }
              }
              else if (ast3 is Call && ((Call) ast3).CanBeFunctionDeclaration())
              {
                if (JSToken.Colon == this.currentToken.token || JSToken.LeftCurly == this.currentToken.token)
                {
                  this.ReportError(JSError.BadFunctionDeclaration, ast3.context.Clone(), true);
                  if (JSToken.Colon == this.currentToken.token)
                  {
                    this.noSkipTokenSet.Add(NoSkipTokenSet.s_StartBlockNoSkipTokenSet);
                    try
                    {
                      this.SkipTokensAndThrow();
                    }
                    catch (RecoveryTokenException ex)
                    {
                    }
                    finally
                    {
                      this.noSkipTokenSet.Remove(NoSkipTokenSet.s_StartBlockNoSkipTokenSet);
                    }
                  }
                  this.errorToken = (Context) null;
                  if (JSToken.LeftCurly == this.currentToken.token)
                  {
                    this.Globals.ScopeStack.Push((object) new FunctionScope(this.Globals.ScopeStack.Peek(), isInsideClass));
                    try
                    {
                      this.ParseBlock();
                    }
                    finally
                    {
                      this.Globals.ScopeStack.Pop();
                    }
                    this.SkipTokensAndThrow();
                  }
                }
                else
                  this.ReportError(JSError.SyntaxError, ast3.context.Clone());
                this.SkipTokensAndThrow();
              }
            }
            if (JSToken.LeftCurly == this.currentToken.token && isInsideClass)
            {
              int index7 = 0;
              for (int count = arrayList3.Count; index7 < count; index7 += 2)
                this.ReportError((JSError) arrayList3[index7], (Context) arrayList3[index7 + 1]);
              if (context2 == null)
                this.ReportError(JSError.StaticMissingInStaticInit, this.CurrentPositionContext());
              string name = ((ClassScope) this.Globals.ScopeStack.Peek()).name;
              bool flag = true;
              foreach (object obj in attributes)
              {
                flag = false;
                if (context2 == null || !(obj is Lookup) || !(obj.ToString() == name) || ((AST) obj).context.StartColumn <= context2.StartColumn)
                  this.ReportError(JSError.SyntaxError, ((AST) obj).context);
              }
              if (flag)
                this.ReportError(JSError.NoIdentifier, this.CurrentPositionContext());
              this.errorToken = (Context) null;
              parsedOK = true;
              return this.ParseStaticInitializer(context4);
            }
            this.ReportError(JSError.MissingConstructForAttributes, context4.CombineWith(this.currentToken));
            this.SkipTokensAndThrow();
            break;
        }
        switch (jsToken)
        {
          case JSToken.Internal:
            fieldAttributes2 = FieldAttributes.Assembly;
            break;
          case JSToken.Abstract:
            if (context1 != null)
            {
              arrayList3.Add((object) JSError.SyntaxError);
              arrayList3.Add((object) statement.context.Clone());
              goto label_133;
            }
            context1 = statement.context.Clone();
            goto label_133;
          case JSToken.Public:
            fieldAttributes2 = FieldAttributes.Public;
            break;
          case JSToken.Static:
            if (isInsideClass)
            {
              fieldAttributes2 = FieldAttributes.Static;
              if (context2 != null)
              {
                arrayList3.Add((object) JSError.SyntaxError);
                arrayList3.Add((object) statement.context.Clone());
                break;
              }
              context2 = statement.context.Clone();
              break;
            }
            arrayList3.Add((object) JSError.NotInsideClass);
            arrayList3.Add((object) statement.context.Clone());
            break;
          case JSToken.Private:
            if (isInsideClass)
            {
              fieldAttributes2 = FieldAttributes.Private;
              break;
            }
            arrayList3.Add((object) JSError.NotInsideClass);
            arrayList3.Add((object) statement.context.Clone());
            break;
          case JSToken.Protected:
            if (isInsideClass)
            {
              fieldAttributes2 = FieldAttributes.Family;
              break;
            }
            arrayList3.Add((object) JSError.NotInsideClass);
            arrayList3.Add((object) statement.context.Clone());
            break;
          case JSToken.Final:
            if (context3 != null)
            {
              arrayList3.Add((object) JSError.SyntaxError);
              arrayList3.Add((object) statement.context.Clone());
              goto label_133;
            }
            context3 = statement.context.Clone();
            goto label_133;
        }
        if ((fieldAttributes1 & FieldAttributes.FieldAccessMask) == fieldAttributes2 && fieldAttributes2 != FieldAttributes.PrivateScope)
        {
          arrayList3.Add((object) JSError.DupVisibility);
          arrayList3.Add((object) statement.context.Clone());
        }
        else if ((fieldAttributes1 & FieldAttributes.FieldAccessMask) > FieldAttributes.PrivateScope && (fieldAttributes2 & FieldAttributes.FieldAccessMask) > FieldAttributes.PrivateScope)
        {
          if (fieldAttributes2 == FieldAttributes.Family && (fieldAttributes1 & FieldAttributes.FieldAccessMask) == FieldAttributes.Assembly || fieldAttributes2 == FieldAttributes.Assembly && (fieldAttributes1 & FieldAttributes.FieldAccessMask) == FieldAttributes.Family)
          {
            fieldAttributes1 = fieldAttributes1 & ~FieldAttributes.FieldAccessMask | FieldAttributes.FamORAssem;
          }
          else
          {
            arrayList3.Add((object) JSError.IncompatibleVisibility);
            arrayList3.Add((object) statement.context.Clone());
          }
        }
        else
        {
          fieldAttributes1 |= fieldAttributes2;
          context4.UpdateWith(statement.context);
        }
label_133:;
      }
      while (num == 2);
      if (this.scanner.GotEndOfLine())
      {
        num = 0;
      }
      else
      {
        ++num;
        arrayList1.Add((object) this.currentToken.Clone());
      }
    }
  }

  private Block ParseBlock() => this.ParseBlock(out Context _);

  private Block ParseBlock(out Context closingBraceContext)
  {
    closingBraceContext = (Context) null;
    this.blockType.Add((object) JSParser.BlockType.Block);
    Block block = new Block(this.currentToken.Clone());
    this.GetNextToken();
    this.noSkipTokenSet.Add(NoSkipTokenSet.s_StartStatementNoSkipTokenSet);
    this.noSkipTokenSet.Add(NoSkipTokenSet.s_BlockNoSkipTokenSet);
    try
    {
      while (JSToken.RightCurly != this.currentToken.token)
      {
        try
        {
          block.Append(this.ParseStatement());
        }
        catch (RecoveryTokenException ex)
        {
          if (ex._partiallyComputedNode != null)
            block.Append(ex._partiallyComputedNode);
          if (this.IndexOfToken(NoSkipTokenSet.s_StartStatementNoSkipTokenSet, ex) == -1)
            throw ex;
        }
      }
    }
    catch (RecoveryTokenException ex)
    {
      if (this.IndexOfToken(NoSkipTokenSet.s_BlockNoSkipTokenSet, ex) == -1)
      {
        ex._partiallyComputedNode = (AST) block;
        throw ex;
      }
    }
    finally
    {
      this.noSkipTokenSet.Remove(NoSkipTokenSet.s_BlockNoSkipTokenSet);
      this.noSkipTokenSet.Remove(NoSkipTokenSet.s_StartStatementNoSkipTokenSet);
      this.blockType.RemoveAt(this.blockType.Count - 1);
    }
    closingBraceContext = this.currentToken.Clone();
    block.context.UpdateWith(this.currentToken);
    this.GetNextToken();
    return block;
  }

  private AST ParseVariableStatement(
    FieldAttributes visibility,
    CustomAttributeList customAttributes,
    JSToken kind)
  {
    Block variableStatement = new Block(this.currentToken.Clone());
    bool flag = true;
    AST elem = (AST) null;
    while (true)
    {
      this.noSkipTokenSet.Add(NoSkipTokenSet.s_EndOfLineToken);
      try
      {
        elem = this.ParseIdentifierInitializer(JSToken.None, visibility, customAttributes, kind);
      }
      catch (RecoveryTokenException ex)
      {
        if (ex._partiallyComputedNode != null && !flag)
        {
          variableStatement.Append(ex._partiallyComputedNode);
          variableStatement.context.UpdateWith(ex._partiallyComputedNode.context);
          ex._partiallyComputedNode = (AST) variableStatement;
        }
        if (this.IndexOfToken(NoSkipTokenSet.s_EndOfLineToken, ex) == -1)
          throw ex;
        if (flag)
          elem = ex._partiallyComputedNode;
      }
      finally
      {
        this.noSkipTokenSet.Remove(NoSkipTokenSet.s_EndOfLineToken);
      }
      if (JSToken.Semicolon != this.currentToken.token && JSToken.RightCurly != this.currentToken.token)
      {
        if (JSToken.Comma == this.currentToken.token)
        {
          flag = false;
          variableStatement.Append(elem);
        }
        else
          goto label_15;
      }
      else
        break;
    }
    if (JSToken.Semicolon == this.currentToken.token)
    {
      elem.context.UpdateWith(this.currentToken);
      this.GetNextToken();
      goto label_17;
    }
    goto label_17;
label_15:
    if (!this.scanner.GotEndOfLine())
      this.ReportError(JSError.NoSemicolon, true);
label_17:
    if (flag)
      return elem;
    variableStatement.Append(elem);
    variableStatement.context.UpdateWith(elem.context);
    return (AST) variableStatement;
  }

  private AST ParseIdentifierInitializer(
    JSToken inToken,
    FieldAttributes visibility,
    CustomAttributeList customAttributes,
    JSToken kind)
  {
    TypeExpression type = (TypeExpression) null;
    AST initializer = (AST) null;
    RecoveryTokenException recoveryTokenException = (RecoveryTokenException) null;
    this.GetNextToken();
    Lookup identifier;
    if (JSToken.Identifier != this.currentToken.token)
    {
      string name = JSKeyword.CanBeIdentifier(this.currentToken.token);
      if (name != null)
      {
        this.ForceReportInfo(JSError.KeywordUsedAsIdentifier);
        identifier = new Lookup(name, this.currentToken.Clone());
      }
      else
      {
        this.ReportError(JSError.NoIdentifier);
        identifier = new Lookup("#_Missing Identifier_#" + JSParser.s_cDummyName++.ToString(), this.CurrentPositionContext());
      }
    }
    else
      identifier = new Lookup(this.scanner.GetIdentifier(), this.currentToken.Clone());
    this.GetNextToken();
    Context context = identifier.context.Clone();
    this.noSkipTokenSet.Add(NoSkipTokenSet.s_VariableDeclNoSkipTokenSet);
    try
    {
      if (JSToken.Colon == this.currentToken.token)
      {
        try
        {
          type = this.ParseTypeExpression();
        }
        catch (RecoveryTokenException ex)
        {
          type = (TypeExpression) ex._partiallyComputedNode;
          throw ex;
        }
        finally
        {
          if (type != null)
            context.UpdateWith(type.context);
        }
      }
      if (JSToken.Assign != this.currentToken.token)
      {
        if (JSToken.Equal != this.currentToken.token)
          goto label_25;
      }
      if (JSToken.Equal == this.currentToken.token)
        this.ReportError(JSError.NoEqual, true);
      this.GetNextToken();
      try
      {
        initializer = this.ParseExpression(true, inToken);
      }
      catch (RecoveryTokenException ex)
      {
        initializer = ex._partiallyComputedNode;
        throw ex;
      }
      finally
      {
        if (initializer != null)
          context.UpdateWith(initializer.context);
      }
    }
    catch (RecoveryTokenException ex)
    {
      if (this.IndexOfToken(NoSkipTokenSet.s_VariableDeclNoSkipTokenSet, ex) == -1)
        recoveryTokenException = ex;
    }
    finally
    {
      this.noSkipTokenSet.Remove(NoSkipTokenSet.s_VariableDeclNoSkipTokenSet);
    }
label_25:
    AST target;
    if (JSToken.Var == kind)
    {
      target = (AST) new VariableDeclaration(context, identifier, type, initializer, visibility, customAttributes);
    }
    else
    {
      if (initializer == null)
        this.ForceReportInfo(JSError.NoEqual);
      target = (AST) new Constant(context, identifier, type, initializer, visibility, customAttributes);
    }
    customAttributes?.SetTarget(target);
    if (recoveryTokenException != null)
    {
      recoveryTokenException._partiallyComputedNode = target;
      throw recoveryTokenException;
    }
    return target;
  }

  private AST ParseQualifiedIdentifier(JSError error)
  {
    this.GetNextToken();
    AST qualid = (AST) null;
    Context context = this.currentToken.Clone();
    if (JSToken.Identifier != this.currentToken.token)
    {
      string name = JSKeyword.CanBeIdentifier(this.currentToken.token);
      if (name != null)
      {
        switch (this.currentToken.token)
        {
          case JSToken.Void:
          case JSToken.Boolean:
          case JSToken.Byte:
          case JSToken.Char:
          case JSToken.Double:
          case JSToken.Float:
          case JSToken.Int:
          case JSToken.Long:
          case JSToken.Short:
            qualid = (AST) new Lookup(name, context);
            break;
          default:
            this.ForceReportInfo(JSError.KeywordUsedAsIdentifier);
            goto case JSToken.Void;
        }
      }
      else
      {
        this.ReportError(error, true);
        this.SkipTokensAndThrow();
      }
    }
    else
      qualid = (AST) new Lookup(this.scanner.GetIdentifier(), context);
    this.GetNextToken();
    if (JSToken.AccessField == this.currentToken.token)
      qualid = this.ParseScopeSequence(qualid, error);
    return qualid;
  }

  private AST ParseScopeSequence(AST qualid, JSError error)
  {
    ConstantWrapper memberName = (ConstantWrapper) null;
    do
    {
      this.GetNextToken();
      if (JSToken.Identifier != this.currentToken.token)
      {
        string str = JSKeyword.CanBeIdentifier(this.currentToken.token);
        if (str != null)
        {
          this.ForceReportInfo(JSError.KeywordUsedAsIdentifier);
          memberName = new ConstantWrapper((object) str, this.currentToken.Clone());
        }
        else
        {
          this.ReportError(error, true);
          this.SkipTokensAndThrow(qualid);
        }
      }
      else
        memberName = new ConstantWrapper((object) this.scanner.GetIdentifier(), this.currentToken.Clone());
      qualid = (AST) new Member(qualid.context.CombineWith(this.currentToken), qualid, (AST) memberName);
      this.GetNextToken();
    }
    while (JSToken.AccessField == this.currentToken.token);
    return qualid;
  }

  private TypeExpression ParseTypeExpression()
  {
    AST qualifiedIdentifier;
    try
    {
      qualifiedIdentifier = this.ParseQualifiedIdentifier(JSError.NeedType);
    }
    catch (RecoveryTokenException ex)
    {
      if (ex._partiallyComputedNode != null)
        ex._partiallyComputedNode = (AST) new TypeExpression(ex._partiallyComputedNode);
      throw ex;
    }
    TypeExpression expression = new TypeExpression(qualifiedIdentifier);
    if (expression != null)
    {
      while (!this.scanner.GotEndOfLine() && JSToken.LeftBracket == this.currentToken.token)
      {
        this.GetNextToken();
        int num = 1;
        while (JSToken.Comma == this.currentToken.token)
        {
          this.GetNextToken();
          ++num;
        }
        if (JSToken.RightBracket != this.currentToken.token)
          this.ReportError(JSError.NoRightBracket);
        this.GetNextToken();
        if (expression.isArray)
          expression = new TypeExpression((AST) expression);
        expression.isArray = true;
        expression.rank = num;
      }
    }
    return expression;
  }

  private If ParseIfStatement()
  {
    Context context = this.currentToken.Clone();
    AST condition = (AST) null;
    AST true_branch = (AST) null;
    AST false_branch = (AST) null;
    this.blockType.Add((object) JSParser.BlockType.Block);
    try
    {
      this.GetNextToken();
      this.noSkipTokenSet.Add(NoSkipTokenSet.s_BlockConditionNoSkipTokenSet);
      try
      {
        if (JSToken.LeftParen != this.currentToken.token)
          this.ReportError(JSError.NoLeftParen);
        this.GetNextToken();
        condition = this.ParseExpression();
        if (JSToken.RightParen != this.currentToken.token)
        {
          context.UpdateWith(condition.context);
          this.ReportError(JSError.NoRightParen);
        }
        else
          context.UpdateWith(this.currentToken);
        this.GetNextToken();
      }
      catch (RecoveryTokenException ex)
      {
        condition = ex._partiallyComputedNode == null ? (AST) new ConstantWrapper((object) true, this.CurrentPositionContext()) : ex._partiallyComputedNode;
        if (this.IndexOfToken(NoSkipTokenSet.s_BlockConditionNoSkipTokenSet, ex) == -1)
        {
          ex._partiallyComputedNode = (AST) null;
          throw ex;
        }
        if (ex._token == JSToken.RightParen)
          this.GetNextToken();
      }
      finally
      {
        this.noSkipTokenSet.Remove(NoSkipTokenSet.s_BlockConditionNoSkipTokenSet);
      }
      if (condition is Assign)
        condition.context.HandleError(JSError.SuspectAssignment);
      if (JSToken.Semicolon == this.currentToken.token)
        this.ForceReportInfo(JSError.SuspectSemicolon);
      this.noSkipTokenSet.Add(NoSkipTokenSet.s_IfBodyNoSkipTokenSet);
      try
      {
        true_branch = this.ParseStatement();
      }
      catch (RecoveryTokenException ex)
      {
        true_branch = ex._partiallyComputedNode == null ? (AST) new Block(this.CurrentPositionContext()) : ex._partiallyComputedNode;
        if (this.IndexOfToken(NoSkipTokenSet.s_IfBodyNoSkipTokenSet, ex) == -1)
        {
          ex._partiallyComputedNode = (AST) new If(context, condition, true_branch, false_branch);
          throw ex;
        }
      }
      finally
      {
        this.noSkipTokenSet.Remove(NoSkipTokenSet.s_IfBodyNoSkipTokenSet);
      }
      if (JSToken.Else == this.currentToken.token)
      {
        this.GetNextToken();
        if (JSToken.Semicolon == this.currentToken.token)
          this.ForceReportInfo(JSError.SuspectSemicolon);
        try
        {
          false_branch = this.ParseStatement();
        }
        catch (RecoveryTokenException ex)
        {
          false_branch = ex._partiallyComputedNode == null ? (AST) new Block(this.CurrentPositionContext()) : ex._partiallyComputedNode;
          ex._partiallyComputedNode = (AST) new If(context, condition, true_branch, false_branch);
          throw ex;
        }
      }
    }
    finally
    {
      this.blockType.RemoveAt(this.blockType.Count - 1);
    }
    return new If(context, condition, true_branch, false_branch);
  }

  private AST ParseForStatement()
  {
    this.blockType.Add((object) JSParser.BlockType.Loop);
    try
    {
      Context context = this.currentToken.Clone();
      this.GetNextToken();
      if (JSToken.LeftParen != this.currentToken.token)
        this.ReportError(JSError.NoLeftParen);
      this.GetNextToken();
      bool flag1 = false;
      bool flag2 = false;
      AST var = (AST) null;
      AST ast1 = (AST) null;
      AST incrementer = (AST) null;
      AST ast2;
      try
      {
        if (JSToken.Var == this.currentToken.token)
        {
          flag1 = true;
          ast2 = this.ParseIdentifierInitializer(JSToken.In, FieldAttributes.PrivateScope, (CustomAttributeList) null, JSToken.Var);
          while (JSToken.Comma == this.currentToken.token)
          {
            flag1 = false;
            AST identifierInitializer = this.ParseIdentifierInitializer(JSToken.In, FieldAttributes.PrivateScope, (CustomAttributeList) null, JSToken.Var);
            ast2 = (AST) new Comma(ast2.context.CombineWith(identifierInitializer.context), ast2, identifierInitializer);
          }
          if (flag1)
          {
            if (JSToken.In == this.currentToken.token)
            {
              this.GetNextToken();
              ast1 = this.ParseExpression();
            }
            else
              flag1 = false;
          }
        }
        else if (JSToken.Semicolon != this.currentToken.token)
        {
          bool isLeftHandSideExpr;
          AST unaryExpression = this.ParseUnaryExpression(out isLeftHandSideExpr, false);
          if (isLeftHandSideExpr && JSToken.In == this.currentToken.token)
          {
            flag1 = true;
            var = unaryExpression;
            ast2 = (AST) null;
            this.GetNextToken();
            this.noSkipTokenSet.Add(NoSkipTokenSet.s_BlockConditionNoSkipTokenSet);
            try
            {
              ast1 = this.ParseExpression();
            }
            catch (RecoveryTokenException ex)
            {
              if (this.IndexOfToken(NoSkipTokenSet.s_BlockConditionNoSkipTokenSet, ex) == -1)
              {
                ex._partiallyComputedNode = (AST) null;
                throw ex;
              }
              ast1 = ex._partiallyComputedNode != null ? ex._partiallyComputedNode : (AST) new ConstantWrapper((object) true, this.CurrentPositionContext());
              if (ex._token == JSToken.RightParen)
              {
                this.GetNextToken();
                flag2 = true;
              }
            }
            finally
            {
              this.noSkipTokenSet.Remove(NoSkipTokenSet.s_BlockConditionNoSkipTokenSet);
            }
          }
          else
            ast2 = this.ParseExpression(unaryExpression, false, isLeftHandSideExpr, JSToken.In);
        }
        else
          ast2 = (AST) new EmptyLiteral(this.CurrentPositionContext());
      }
      catch (RecoveryTokenException ex)
      {
        ex._partiallyComputedNode = (AST) null;
        throw ex;
      }
      if (flag1)
      {
        if (!flag2)
        {
          if (JSToken.RightParen != this.currentToken.token)
            this.ReportError(JSError.NoRightParen);
          context.UpdateWith(this.currentToken);
          this.GetNextToken();
        }
        AST statement;
        try
        {
          statement = this.ParseStatement();
        }
        catch (RecoveryTokenException ex)
        {
          AST body = ex._partiallyComputedNode != null ? ex._partiallyComputedNode : (AST) new Block(this.CurrentPositionContext());
          ex._partiallyComputedNode = (AST) new ForIn(context, var, ast2, ast1, body);
          throw ex;
        }
        return (AST) new ForIn(context, var, ast2, ast1, statement);
      }
      this.noSkipTokenSet.Add(NoSkipTokenSet.s_BlockConditionNoSkipTokenSet);
      try
      {
        if (JSToken.Semicolon != this.currentToken.token)
        {
          this.ReportError(JSError.NoSemicolon);
          if (JSToken.Colon == this.currentToken.token)
          {
            this.noSkipTokenSet.Add(NoSkipTokenSet.s_VariableDeclNoSkipTokenSet);
            try
            {
              this.SkipTokensAndThrow();
            }
            catch (RecoveryTokenException ex)
            {
              if (JSToken.Semicolon != this.currentToken.token)
                throw ex;
              this.errorToken = (Context) null;
            }
            finally
            {
              this.noSkipTokenSet.Remove(NoSkipTokenSet.s_VariableDeclNoSkipTokenSet);
            }
          }
        }
        this.GetNextToken();
        if (JSToken.Semicolon != this.currentToken.token)
        {
          ast1 = this.ParseExpression();
          if (JSToken.Semicolon != this.currentToken.token)
            this.ReportError(JSError.NoSemicolon);
        }
        else
          ast1 = (AST) new ConstantWrapper((object) true, this.CurrentPositionContext());
        this.GetNextToken();
        incrementer = JSToken.RightParen == this.currentToken.token ? (AST) new EmptyLiteral(this.CurrentPositionContext()) : this.ParseExpression();
        if (JSToken.RightParen != this.currentToken.token)
          this.ReportError(JSError.NoRightParen);
        context.UpdateWith(this.currentToken);
        this.GetNextToken();
      }
      catch (RecoveryTokenException ex)
      {
        if (this.IndexOfToken(NoSkipTokenSet.s_BlockConditionNoSkipTokenSet, ex) == -1)
        {
          ex._partiallyComputedNode = (AST) null;
          throw ex;
        }
        ex._partiallyComputedNode = (AST) null;
        if (ast1 == null)
          ast1 = (AST) new ConstantWrapper((object) true, this.CurrentPositionContext());
        if (incrementer == null)
          incrementer = (AST) new EmptyLiteral(this.CurrentPositionContext());
        if (ex._token == JSToken.RightParen)
          this.GetNextToken();
      }
      finally
      {
        this.noSkipTokenSet.Remove(NoSkipTokenSet.s_BlockConditionNoSkipTokenSet);
      }
      AST statement1;
      try
      {
        statement1 = this.ParseStatement();
      }
      catch (RecoveryTokenException ex)
      {
        AST body = ex._partiallyComputedNode != null ? ex._partiallyComputedNode : (AST) new Block(this.CurrentPositionContext());
        ex._partiallyComputedNode = (AST) new For(context, ast2, ast1, incrementer, body);
        throw ex;
      }
      return (AST) new For(context, ast2, ast1, incrementer, statement1);
    }
    finally
    {
      this.blockType.RemoveAt(this.blockType.Count - 1);
    }
  }

  private DoWhile ParseDoStatement()
  {
    Context context = (Context) null;
    AST body = (AST) null;
    AST condition = (AST) null;
    this.blockType.Add((object) JSParser.BlockType.Loop);
    try
    {
      this.GetNextToken();
      this.noSkipTokenSet.Add(NoSkipTokenSet.s_DoWhileBodyNoSkipTokenSet);
      try
      {
        body = this.ParseStatement();
      }
      catch (RecoveryTokenException ex)
      {
        body = ex._partiallyComputedNode == null ? (AST) new Block(this.CurrentPositionContext()) : ex._partiallyComputedNode;
        if (this.IndexOfToken(NoSkipTokenSet.s_DoWhileBodyNoSkipTokenSet, ex) == -1)
        {
          ex._partiallyComputedNode = (AST) new DoWhile(this.CurrentPositionContext(), body, (AST) new ConstantWrapper((object) false, this.CurrentPositionContext()));
          throw ex;
        }
      }
      finally
      {
        this.noSkipTokenSet.Remove(NoSkipTokenSet.s_DoWhileBodyNoSkipTokenSet);
      }
      if (JSToken.While != this.currentToken.token)
        this.ReportError(JSError.NoWhile);
      context = this.currentToken.Clone();
      this.GetNextToken();
      if (JSToken.LeftParen != this.currentToken.token)
        this.ReportError(JSError.NoLeftParen);
      this.GetNextToken();
      this.noSkipTokenSet.Add(NoSkipTokenSet.s_BlockConditionNoSkipTokenSet);
      try
      {
        condition = this.ParseExpression();
        if (JSToken.RightParen != this.currentToken.token)
        {
          this.ReportError(JSError.NoRightParen);
          context.UpdateWith(condition.context);
        }
        else
          context.UpdateWith(this.currentToken);
        this.GetNextToken();
      }
      catch (RecoveryTokenException ex)
      {
        condition = ex._partiallyComputedNode == null ? (AST) new ConstantWrapper((object) false, this.CurrentPositionContext()) : ex._partiallyComputedNode;
        if (this.IndexOfToken(NoSkipTokenSet.s_BlockConditionNoSkipTokenSet, ex) == -1)
        {
          ex._partiallyComputedNode = (AST) new DoWhile(context, body, condition);
          throw ex;
        }
        if (JSToken.RightParen == this.currentToken.token)
          this.GetNextToken();
      }
      finally
      {
        this.noSkipTokenSet.Remove(NoSkipTokenSet.s_BlockConditionNoSkipTokenSet);
      }
      if (JSToken.Semicolon == this.currentToken.token)
      {
        context.UpdateWith(this.currentToken);
        this.GetNextToken();
      }
    }
    finally
    {
      this.blockType.RemoveAt(this.blockType.Count - 1);
    }
    return new DoWhile(context, body, condition);
  }

  private While ParseWhileStatement()
  {
    Context context = this.currentToken.Clone();
    AST condition = (AST) null;
    AST body = (AST) null;
    this.blockType.Add((object) JSParser.BlockType.Loop);
    try
    {
      this.GetNextToken();
      if (JSToken.LeftParen != this.currentToken.token)
        this.ReportError(JSError.NoLeftParen);
      this.GetNextToken();
      this.noSkipTokenSet.Add(NoSkipTokenSet.s_BlockConditionNoSkipTokenSet);
      try
      {
        condition = this.ParseExpression();
        if (JSToken.RightParen != this.currentToken.token)
        {
          this.ReportError(JSError.NoRightParen);
          context.UpdateWith(condition.context);
        }
        else
          context.UpdateWith(this.currentToken);
        this.GetNextToken();
      }
      catch (RecoveryTokenException ex)
      {
        if (this.IndexOfToken(NoSkipTokenSet.s_BlockConditionNoSkipTokenSet, ex) == -1)
        {
          ex._partiallyComputedNode = (AST) null;
          throw ex;
        }
        condition = ex._partiallyComputedNode == null ? (AST) new ConstantWrapper((object) false, this.CurrentPositionContext()) : ex._partiallyComputedNode;
        if (JSToken.RightParen == this.currentToken.token)
          this.GetNextToken();
      }
      finally
      {
        this.noSkipTokenSet.Remove(NoSkipTokenSet.s_BlockConditionNoSkipTokenSet);
      }
      try
      {
        body = this.ParseStatement();
      }
      catch (RecoveryTokenException ex)
      {
        body = ex._partiallyComputedNode == null ? (AST) new Block(this.CurrentPositionContext()) : ex._partiallyComputedNode;
        ex._partiallyComputedNode = (AST) new While(context, condition, body);
        throw ex;
      }
    }
    finally
    {
      this.blockType.RemoveAt(this.blockType.Count - 1);
    }
    return new While(context, condition, body);
  }

  private Continue ParseContinueStatement()
  {
    Context context = this.currentToken.Clone();
    this.GetNextToken();
    string key = (string) null;
    int index1;
    if (!this.scanner.GotEndOfLine() && (JSToken.Identifier == this.currentToken.token || (key = JSKeyword.CanBeIdentifier(this.currentToken.token)) != null))
    {
      context.UpdateWith(this.currentToken);
      if (key != null)
        this.ForceReportInfo(JSError.KeywordUsedAsIdentifier);
      else
        key = this.scanner.GetIdentifier();
      object obj = this.labelTable[(object) key];
      if (obj == null)
      {
        this.ReportError(JSError.NoLabel, true);
        this.GetNextToken();
        return (Continue) null;
      }
      index1 = (int) obj;
      if ((JSParser.BlockType) this.blockType[index1] != JSParser.BlockType.Loop)
        this.ReportError(JSError.BadContinue, context.Clone(), true);
      this.GetNextToken();
    }
    else
    {
      index1 = this.blockType.Count - 1;
      while (index1 >= 0 && (JSParser.BlockType) this.blockType[index1] != JSParser.BlockType.Loop)
        --index1;
      if (index1 < 0)
      {
        this.ReportError(JSError.BadContinue, context, true);
        return (Continue) null;
      }
    }
    if (JSToken.Semicolon == this.currentToken.token)
    {
      context.UpdateWith(this.currentToken);
      this.GetNextToken();
    }
    else if (JSToken.RightCurly != this.currentToken.token && !this.scanner.GotEndOfLine())
      this.ReportError(JSError.NoSemicolon, true);
    int num = 0;
    int index2 = index1;
    for (int count = this.blockType.Count; index2 < count; ++index2)
    {
      if ((JSParser.BlockType) this.blockType[index2] == JSParser.BlockType.Finally)
      {
        ++index1;
        ++num;
      }
    }
    if (num > this.finallyEscaped)
      this.finallyEscaped = num;
    return new Continue(context, this.blockType.Count - index1, num > 0);
  }

  private Break ParseBreakStatement()
  {
    Context context = this.currentToken.Clone();
    this.GetNextToken();
    string key = (string) null;
    int num1;
    if (!this.scanner.GotEndOfLine() && (JSToken.Identifier == this.currentToken.token || (key = JSKeyword.CanBeIdentifier(this.currentToken.token)) != null))
    {
      context.UpdateWith(this.currentToken);
      if (key != null)
        this.ForceReportInfo(JSError.KeywordUsedAsIdentifier);
      else
        key = this.scanner.GetIdentifier();
      object obj = this.labelTable[(object) key];
      if (obj == null)
      {
        this.ReportError(JSError.NoLabel, true);
        this.GetNextToken();
        return (Break) null;
      }
      num1 = (int) obj - 1;
      this.GetNextToken();
    }
    else
    {
      int index = this.blockType.Count - 1;
      do
        ;
      while (((JSParser.BlockType) this.blockType[index] == JSParser.BlockType.Block || (JSParser.BlockType) this.blockType[index] == JSParser.BlockType.Finally) && --index >= 0);
      num1 = index - 1;
      if (num1 < 0)
      {
        this.ReportError(JSError.BadBreak, context, true);
        return (Break) null;
      }
    }
    if (JSToken.Semicolon == this.currentToken.token)
    {
      context.UpdateWith(this.currentToken);
      this.GetNextToken();
    }
    else if (JSToken.RightCurly != this.currentToken.token && !this.scanner.GotEndOfLine())
      this.ReportError(JSError.NoSemicolon, true);
    int num2 = 0;
    int index1 = num1;
    for (int count = this.blockType.Count; index1 < count; ++index1)
    {
      if ((JSParser.BlockType) this.blockType[index1] == JSParser.BlockType.Finally)
      {
        ++num1;
        ++num2;
      }
    }
    if (num2 > this.finallyEscaped)
      this.finallyEscaped = num2;
    return new Break(context, this.blockType.Count - num1 - 1, num2 > 0);
  }

  private bool CheckForReturnFromFinally()
  {
    int num = 0;
    for (int index = this.blockType.Count - 1; index >= 0; --index)
    {
      if ((JSParser.BlockType) this.blockType[index] == JSParser.BlockType.Finally)
        ++num;
    }
    if (num > this.finallyEscaped)
      this.finallyEscaped = num;
    return num > 0;
  }

  private Return ParseReturnStatement()
  {
    Context context = this.currentToken.Clone();
    if (this.Globals.ScopeStack.Peek() is FunctionScope)
    {
      AST operand = (AST) null;
      this.GetNextToken();
      if (!this.scanner.GotEndOfLine())
      {
        if (JSToken.Semicolon != this.currentToken.token && JSToken.RightCurly != this.currentToken.token)
        {
          this.noSkipTokenSet.Add(NoSkipTokenSet.s_EndOfStatementNoSkipTokenSet);
          try
          {
            operand = this.ParseExpression();
          }
          catch (RecoveryTokenException ex)
          {
            operand = ex._partiallyComputedNode;
            if (this.IndexOfToken(NoSkipTokenSet.s_EndOfStatementNoSkipTokenSet, ex) == -1)
            {
              if (operand != null)
                context.UpdateWith(operand.context);
              ex._partiallyComputedNode = (AST) new Return(context, operand, this.CheckForReturnFromFinally());
              throw ex;
            }
          }
          finally
          {
            this.noSkipTokenSet.Remove(NoSkipTokenSet.s_EndOfStatementNoSkipTokenSet);
          }
          if (JSToken.Semicolon != this.currentToken.token && JSToken.RightCurly != this.currentToken.token && !this.scanner.GotEndOfLine())
            this.ReportError(JSError.NoSemicolon, true);
        }
        if (JSToken.Semicolon == this.currentToken.token)
        {
          context.UpdateWith(this.currentToken);
          this.GetNextToken();
        }
        else if (operand != null)
          context.UpdateWith(operand.context);
      }
      return new Return(context, operand, this.CheckForReturnFromFinally());
    }
    this.ReportError(JSError.BadReturn, context, true);
    this.GetNextToken();
    return (Return) null;
  }

  private Import ParseImportStatement()
  {
    Context context = this.currentToken.Clone();
    AST name = (AST) null;
    try
    {
      name = this.ParseQualifiedIdentifier(JSError.PackageExpected);
    }
    catch (RecoveryTokenException ex)
    {
      if (ex._partiallyComputedNode != null)
        ex._partiallyComputedNode = (AST) new Import(context, ex._partiallyComputedNode);
    }
    if (this.currentToken.token != JSToken.Semicolon && !this.scanner.GotEndOfLine())
      this.ReportError(JSError.NoSemicolon, this.currentToken.Clone());
    return new Import(context, name);
  }

  private With ParseWithStatement()
  {
    Context context = this.currentToken.Clone();
    AST ast = (AST) null;
    AST block = (AST) null;
    this.blockType.Add((object) JSParser.BlockType.Block);
    try
    {
      this.GetNextToken();
      if (JSToken.LeftParen != this.currentToken.token)
        this.ReportError(JSError.NoLeftParen);
      this.GetNextToken();
      this.noSkipTokenSet.Add(NoSkipTokenSet.s_BlockConditionNoSkipTokenSet);
      try
      {
        ast = this.ParseExpression();
        if (JSToken.RightParen != this.currentToken.token)
        {
          context.UpdateWith(ast.context);
          this.ReportError(JSError.NoRightParen);
        }
        else
          context.UpdateWith(this.currentToken);
        this.GetNextToken();
      }
      catch (RecoveryTokenException ex)
      {
        if (this.IndexOfToken(NoSkipTokenSet.s_BlockConditionNoSkipTokenSet, ex) == -1)
        {
          ex._partiallyComputedNode = (AST) null;
          throw ex;
        }
        ast = ex._partiallyComputedNode != null ? ex._partiallyComputedNode : (AST) new ConstantWrapper((object) true, this.CurrentPositionContext());
        context.UpdateWith(ast.context);
        if (ex._token == JSToken.RightParen)
          this.GetNextToken();
      }
      finally
      {
        this.noSkipTokenSet.Remove(NoSkipTokenSet.s_BlockConditionNoSkipTokenSet);
      }
      try
      {
        block = this.ParseStatement();
      }
      catch (RecoveryTokenException ex)
      {
        block = ex._partiallyComputedNode != null ? ex._partiallyComputedNode : (AST) new Block(this.CurrentPositionContext());
        ex._partiallyComputedNode = (AST) new With(context, ast, block);
      }
    }
    finally
    {
      this.blockType.RemoveAt(this.blockType.Count - 1);
    }
    return new With(context, ast, block);
  }

  private AST ParseSwitchStatement()
  {
    Context context1 = this.currentToken.Clone();
    AST expression = (AST) null;
    ASTList cases = (ASTList) null;
    this.blockType.Add((object) JSParser.BlockType.Switch);
    try
    {
      this.GetNextToken();
      if (JSToken.LeftParen != this.currentToken.token)
        this.ReportError(JSError.NoLeftParen);
      this.GetNextToken();
      this.noSkipTokenSet.Add(NoSkipTokenSet.s_BlockConditionNoSkipTokenSet);
      this.noSkipTokenSet.Add(NoSkipTokenSet.s_SwitchNoSkipTokenSet);
      try
      {
        expression = this.ParseExpression();
        if (JSToken.RightParen != this.currentToken.token)
          this.ReportError(JSError.NoRightParen);
        this.GetNextToken();
        if (JSToken.LeftCurly != this.currentToken.token)
          this.ReportError(JSError.NoLeftCurly);
        this.GetNextToken();
      }
      catch (RecoveryTokenException ex)
      {
        if (this.IndexOfToken(NoSkipTokenSet.s_BlockConditionNoSkipTokenSet, ex) == -1 && this.IndexOfToken(NoSkipTokenSet.s_SwitchNoSkipTokenSet, ex) == -1)
        {
          ex._partiallyComputedNode = (AST) null;
          throw ex;
        }
        expression = ex._partiallyComputedNode != null ? ex._partiallyComputedNode : (AST) new ConstantWrapper((object) true, this.CurrentPositionContext());
        if (this.IndexOfToken(NoSkipTokenSet.s_BlockConditionNoSkipTokenSet, ex) != -1)
        {
          if (ex._token == JSToken.RightParen)
            this.GetNextToken();
          if (JSToken.LeftCurly != this.currentToken.token)
            this.ReportError(JSError.NoLeftCurly);
          this.GetNextToken();
        }
      }
      finally
      {
        this.noSkipTokenSet.Remove(NoSkipTokenSet.s_SwitchNoSkipTokenSet);
        this.noSkipTokenSet.Remove(NoSkipTokenSet.s_BlockConditionNoSkipTokenSet);
      }
      cases = new ASTList(this.currentToken.Clone());
      bool flag = false;
      this.noSkipTokenSet.Add(NoSkipTokenSet.s_BlockNoSkipTokenSet);
      try
      {
        while (JSToken.RightCurly != this.currentToken.token)
        {
          AST case_value = (AST) null;
          Context context2 = this.currentToken.Clone();
          this.noSkipTokenSet.Add(NoSkipTokenSet.s_CaseNoSkipTokenSet);
          try
          {
            if (JSToken.Case == this.currentToken.token)
            {
              this.GetNextToken();
              case_value = this.ParseExpression();
            }
            else if (JSToken.Default == this.currentToken.token)
            {
              if (flag)
                this.ReportError(JSError.DupDefault, true);
              else
                flag = true;
              this.GetNextToken();
            }
            else
            {
              flag = true;
              this.ReportError(JSError.BadSwitch);
            }
            if (JSToken.Colon != this.currentToken.token)
              this.ReportError(JSError.NoColon);
            this.GetNextToken();
          }
          catch (RecoveryTokenException ex)
          {
            if (this.IndexOfToken(NoSkipTokenSet.s_CaseNoSkipTokenSet, ex) == -1)
            {
              ex._partiallyComputedNode = (AST) null;
              throw ex;
            }
            case_value = ex._partiallyComputedNode;
            if (ex._token == JSToken.Colon)
              this.GetNextToken();
          }
          finally
          {
            this.noSkipTokenSet.Remove(NoSkipTokenSet.s_CaseNoSkipTokenSet);
          }
          this.blockType.Add((object) JSParser.BlockType.Block);
          try
          {
            Block statements = new Block(this.currentToken.Clone());
            this.noSkipTokenSet.Add(NoSkipTokenSet.s_SwitchNoSkipTokenSet);
            this.noSkipTokenSet.Add(NoSkipTokenSet.s_StartStatementNoSkipTokenSet);
            try
            {
              while (JSToken.RightCurly != this.currentToken.token)
              {
                if (JSToken.Case != this.currentToken.token)
                {
                  if (JSToken.Default != this.currentToken.token)
                  {
                    try
                    {
                      statements.Append(this.ParseStatement());
                    }
                    catch (RecoveryTokenException ex)
                    {
                      if (ex._partiallyComputedNode != null)
                      {
                        statements.Append(ex._partiallyComputedNode);
                        ex._partiallyComputedNode = (AST) null;
                      }
                      if (this.IndexOfToken(NoSkipTokenSet.s_StartStatementNoSkipTokenSet, ex) == -1)
                        throw ex;
                    }
                  }
                  else
                    break;
                }
                else
                  break;
              }
            }
            catch (RecoveryTokenException ex)
            {
              if (this.IndexOfToken(NoSkipTokenSet.s_SwitchNoSkipTokenSet, ex) == -1)
              {
                SwitchCase elem = case_value != null ? new SwitchCase(context2, case_value, (AST) statements) : new SwitchCase(context2, (AST) statements);
                cases.Append((AST) elem);
                throw ex;
              }
            }
            finally
            {
              this.noSkipTokenSet.Remove(NoSkipTokenSet.s_StartStatementNoSkipTokenSet);
              this.noSkipTokenSet.Remove(NoSkipTokenSet.s_SwitchNoSkipTokenSet);
            }
            if (JSToken.RightCurly == this.currentToken.token)
              statements.context.UpdateWith(this.currentToken);
            SwitchCase elem1;
            if (case_value == null)
            {
              context2.UpdateWith(statements.context);
              elem1 = new SwitchCase(context2, (AST) statements);
            }
            else
            {
              context2.UpdateWith(statements.context);
              elem1 = new SwitchCase(context2, case_value, (AST) statements);
            }
            cases.Append((AST) elem1);
          }
          finally
          {
            this.blockType.RemoveAt(this.blockType.Count - 1);
          }
        }
      }
      catch (RecoveryTokenException ex)
      {
        if (this.IndexOfToken(NoSkipTokenSet.s_BlockNoSkipTokenSet, ex) == -1)
        {
          context1.UpdateWith(this.CurrentPositionContext());
          ex._partiallyComputedNode = (AST) new Switch(context1, expression, cases);
          throw ex;
        }
      }
      finally
      {
        this.noSkipTokenSet.Remove(NoSkipTokenSet.s_BlockNoSkipTokenSet);
      }
      context1.UpdateWith(this.currentToken);
      this.GetNextToken();
    }
    finally
    {
      this.blockType.RemoveAt(this.blockType.Count - 1);
    }
    return (AST) new Switch(context1, expression, cases);
  }

  private AST ParseThrowStatement()
  {
    Context context = this.currentToken.Clone();
    this.GetNextToken();
    AST operand = (AST) null;
    if (!this.scanner.GotEndOfLine() && JSToken.Semicolon != this.currentToken.token)
    {
      this.noSkipTokenSet.Add(NoSkipTokenSet.s_EndOfStatementNoSkipTokenSet);
      try
      {
        operand = this.ParseExpression();
      }
      catch (RecoveryTokenException ex)
      {
        operand = ex._partiallyComputedNode;
        if (this.IndexOfToken(NoSkipTokenSet.s_EndOfStatementNoSkipTokenSet, ex) == -1)
        {
          if (operand != null)
            ex._partiallyComputedNode = (AST) new Throw(context, ex._partiallyComputedNode);
          throw ex;
        }
      }
      finally
      {
        this.noSkipTokenSet.Remove(NoSkipTokenSet.s_EndOfStatementNoSkipTokenSet);
      }
    }
    if (operand != null)
      context.UpdateWith(operand.context);
    return (AST) new Throw(context, operand);
  }

  private AST ParseTryStatement()
  {
    Context context = this.currentToken.Clone();
    Context closingBraceContext = (Context) null;
    AST body = (AST) null;
    AST identifier = (AST) null;
    AST handler = (AST) null;
    AST finally_block = (AST) null;
    RecoveryTokenException recoveryTokenException = (RecoveryTokenException) null;
    TypeExpression type = (TypeExpression) null;
    this.blockType.Add((object) JSParser.BlockType.Block);
    try
    {
      bool flag1 = false;
      bool flag2 = false;
      this.GetNextToken();
      if (JSToken.LeftCurly != this.currentToken.token)
        this.ReportError(JSError.NoLeftCurly);
      this.noSkipTokenSet.Add(NoSkipTokenSet.s_NoTrySkipTokenSet);
      try
      {
        body = (AST) this.ParseBlock(out closingBraceContext);
      }
      catch (RecoveryTokenException ex)
      {
        body = this.IndexOfToken(NoSkipTokenSet.s_NoTrySkipTokenSet, ex) != -1 ? ex._partiallyComputedNode : throw ex;
      }
      finally
      {
        this.noSkipTokenSet.Remove(NoSkipTokenSet.s_NoTrySkipTokenSet);
      }
      while (JSToken.Catch == this.currentToken.token)
      {
        this.noSkipTokenSet.Add(NoSkipTokenSet.s_NoTrySkipTokenSet);
        try
        {
          if (handler != null)
          {
            body = (AST) new Try(context, body, identifier, type, handler, (AST) null, false, closingBraceContext);
            identifier = (AST) null;
            type = (TypeExpression) null;
            handler = (AST) null;
          }
          flag1 = true;
          this.GetNextToken();
          if (JSToken.LeftParen != this.currentToken.token)
            this.ReportError(JSError.NoLeftParen);
          this.GetNextToken();
          if (JSToken.Identifier != this.currentToken.token)
          {
            string name = JSKeyword.CanBeIdentifier(this.currentToken.token);
            if (name != null)
            {
              this.ForceReportInfo(JSError.KeywordUsedAsIdentifier);
              identifier = (AST) new Lookup(name, this.currentToken.Clone());
            }
            else
            {
              this.ReportError(JSError.NoIdentifier);
              identifier = (AST) new Lookup("##Exc##" + JSParser.s_cDummyName++.ToString(), this.CurrentPositionContext());
            }
          }
          else
            identifier = (AST) new Lookup(this.scanner.GetIdentifier(), this.currentToken.Clone());
          this.GetNextToken();
          this.noSkipTokenSet.Add(NoSkipTokenSet.s_BlockConditionNoSkipTokenSet);
          try
          {
            if (JSToken.Colon == this.currentToken.token)
            {
              type = this.ParseTypeExpression();
            }
            else
            {
              if (flag2)
                this.ForceReportInfo(identifier.context, JSError.UnreachableCatch);
              flag2 = true;
            }
            if (JSToken.RightParen != this.currentToken.token)
              this.ReportError(JSError.NoRightParen);
            this.GetNextToken();
          }
          catch (RecoveryTokenException ex)
          {
            if (this.IndexOfToken(NoSkipTokenSet.s_BlockConditionNoSkipTokenSet, ex) == -1)
            {
              ex._partiallyComputedNode = (AST) null;
              throw ex;
            }
            type = (TypeExpression) ex._partiallyComputedNode;
            if (this.currentToken.token == JSToken.RightParen)
              this.GetNextToken();
          }
          finally
          {
            this.noSkipTokenSet.Remove(NoSkipTokenSet.s_BlockConditionNoSkipTokenSet);
          }
          if (JSToken.LeftCurly != this.currentToken.token)
            this.ReportError(JSError.NoLeftCurly);
          handler = (AST) this.ParseBlock();
          context.UpdateWith(handler.context);
        }
        catch (RecoveryTokenException ex)
        {
          handler = ex._partiallyComputedNode != null ? ex._partiallyComputedNode : (AST) new Block(this.CurrentPositionContext());
          if (this.IndexOfToken(NoSkipTokenSet.s_NoTrySkipTokenSet, ex) == -1)
          {
            if (type != null)
              ex._partiallyComputedNode = (AST) new Try(context, body, identifier, type, handler, (AST) null, false, closingBraceContext);
            throw ex;
          }
        }
        finally
        {
          this.noSkipTokenSet.Remove(NoSkipTokenSet.s_NoTrySkipTokenSet);
        }
      }
      try
      {
        if (JSToken.Finally == this.currentToken.token)
        {
          this.GetNextToken();
          this.blockType.Add((object) JSParser.BlockType.Finally);
          try
          {
            finally_block = (AST) this.ParseBlock();
            flag1 = true;
          }
          finally
          {
            this.blockType.RemoveAt(this.blockType.Count - 1);
          }
          context.UpdateWith(finally_block.context);
        }
      }
      catch (RecoveryTokenException ex)
      {
        recoveryTokenException = ex;
      }
      if (!flag1)
      {
        this.ReportError(JSError.NoCatch, true);
        finally_block = (AST) new Block(this.CurrentPositionContext());
      }
    }
    finally
    {
      this.blockType.RemoveAt(this.blockType.Count - 1);
    }
    bool finallyHasControlFlowOutOfIt = false;
    if (this.finallyEscaped > 0)
    {
      --this.finallyEscaped;
      finallyHasControlFlowOutOfIt = true;
    }
    if (recoveryTokenException != null)
    {
      recoveryTokenException._partiallyComputedNode = (AST) new Try(context, body, identifier, type, handler, finally_block, finallyHasControlFlowOutOfIt, closingBraceContext);
      throw recoveryTokenException;
    }
    return (AST) new Try(context, body, identifier, type, handler, finally_block, finallyHasControlFlowOutOfIt, closingBraceContext);
  }

  private AST ParseClass(
    FieldAttributes visibilitySpec,
    bool isStatic,
    Context classCtx,
    bool isAbstract,
    bool isFinal,
    CustomAttributeList customAttributes)
  {
    AST expression = (AST) null;
    TypeExpression superTypeExpression = (TypeExpression) null;
    ArrayList arrayList = new ArrayList();
    bool isInterface = JSToken.Interface == this.currentToken.token;
    this.GetNextToken();
    AST ast;
    if (JSToken.Identifier == this.currentToken.token)
    {
      ast = (AST) new IdentifierLiteral(this.scanner.GetIdentifier(), this.currentToken.Clone());
    }
    else
    {
      this.ReportError(JSError.NoIdentifier);
      if (JSToken.Extends != this.currentToken.token && JSToken.Implements != this.currentToken.token && JSToken.LeftCurly != this.currentToken.token)
        this.SkipTokensAndThrow();
      ast = (AST) new IdentifierLiteral("##Missing Class Name##" + JSParser.s_cDummyName++.ToString(), this.CurrentPositionContext());
    }
    this.GetNextToken();
    if (JSToken.Extends == this.currentToken.token || JSToken.Implements == this.currentToken.token)
    {
      if (isInterface && JSToken.Extends == this.currentToken.token)
        this.currentToken.token = JSToken.Implements;
      if (JSToken.Extends == this.currentToken.token)
      {
        this.noSkipTokenSet.Add(NoSkipTokenSet.s_ClassExtendsNoSkipTokenSet);
        try
        {
          expression = this.ParseQualifiedIdentifier(JSError.NeedType);
        }
        catch (RecoveryTokenException ex)
        {
          if (this.IndexOfToken(NoSkipTokenSet.s_ClassExtendsNoSkipTokenSet, ex) == -1)
          {
            ex._partiallyComputedNode = (AST) null;
            throw ex;
          }
          expression = ex._partiallyComputedNode;
        }
        finally
        {
          this.noSkipTokenSet.Remove(NoSkipTokenSet.s_ClassExtendsNoSkipTokenSet);
        }
      }
      if (JSToken.Implements == this.currentToken.token)
      {
        do
        {
          this.noSkipTokenSet.Add(NoSkipTokenSet.s_ClassImplementsNoSkipTokenSet);
          try
          {
            AST qualifiedIdentifier = this.ParseQualifiedIdentifier(JSError.NeedType);
            arrayList.Add((object) new TypeExpression(qualifiedIdentifier));
          }
          catch (RecoveryTokenException ex)
          {
            if (this.IndexOfToken(NoSkipTokenSet.s_ClassImplementsNoSkipTokenSet, ex) == -1)
            {
              ex._partiallyComputedNode = (AST) null;
              throw ex;
            }
            if (ex._partiallyComputedNode != null)
              arrayList.Add((object) new TypeExpression(ex._partiallyComputedNode));
          }
          finally
          {
            this.noSkipTokenSet.Remove(NoSkipTokenSet.s_ClassImplementsNoSkipTokenSet);
          }
        }
        while (JSToken.Comma == this.currentToken.token);
      }
    }
    if (expression != null)
      superTypeExpression = new TypeExpression(expression);
    if (JSToken.LeftCurly != this.currentToken.token)
      this.ReportError(JSError.NoLeftCurly);
    ArrayList blockType = this.blockType;
    this.blockType = new ArrayList(16 /*0x10*/);
    SimpleHashtable labelTable = this.labelTable;
    this.labelTable = new SimpleHashtable(16U /*0x10*/);
    this.Globals.ScopeStack.Push((object) new ClassScope(ast, ((IActivationObject) this.Globals.ScopeStack.Peek()).GetGlobalScope()));
    try
    {
      Block classBody = this.ParseClassBody(false, isInterface);
      classCtx.UpdateWith(classBody.context);
      TypeExpression[] interfaces = new TypeExpression[arrayList.Count];
      arrayList.CopyTo((Array) interfaces);
      Class target = new Class(classCtx, ast, superTypeExpression, interfaces, classBody, visibilitySpec, isAbstract, isFinal, isStatic, isInterface, customAttributes);
      customAttributes?.SetTarget((AST) target);
      return (AST) target;
    }
    catch (RecoveryTokenException ex)
    {
      classCtx.UpdateWith(ex._partiallyComputedNode.context);
      TypeExpression[] interfaces = new TypeExpression[arrayList.Count];
      arrayList.CopyTo((Array) interfaces);
      ex._partiallyComputedNode = (AST) new Class(classCtx, ast, superTypeExpression, interfaces, (Block) ex._partiallyComputedNode, visibilitySpec, isAbstract, isFinal, isStatic, isInterface, customAttributes);
      customAttributes?.SetTarget(ex._partiallyComputedNode);
      throw ex;
    }
    finally
    {
      this.Globals.ScopeStack.Pop();
      this.blockType = blockType;
      this.labelTable = labelTable;
    }
  }

  private Block ParseClassBody(bool isEnum, bool isInterface)
  {
    this.blockType.Add((object) JSParser.BlockType.Block);
    Block classBody = new Block(this.currentToken.Clone());
    try
    {
      this.GetNextToken();
      this.noSkipTokenSet.Add(NoSkipTokenSet.s_BlockNoSkipTokenSet);
      JSToken[] tokens = !isEnum ? (!isInterface ? NoSkipTokenSet.s_ClassBodyNoSkipTokenSet : NoSkipTokenSet.s_InterfaceBodyNoSkipTokenSet) : NoSkipTokenSet.s_EnumBodyNoSkipTokenSet;
      try
      {
        while (JSToken.RightCurly != this.currentToken.token)
        {
          if (this.currentToken.token == JSToken.EndOfFile)
          {
            this.ReportError(JSError.NoRightCurly, true);
            this.SkipTokensAndThrow();
          }
          this.noSkipTokenSet.Add(tokens);
          try
          {
            AST elem = isEnum ? this.ParseEnumMember() : this.ParseClassMember(isInterface);
            if (elem != null)
              classBody.Append(elem);
          }
          catch (RecoveryTokenException ex)
          {
            if (ex._partiallyComputedNode != null)
              classBody.Append(ex._partiallyComputedNode);
            if (this.IndexOfToken(tokens, ex) == -1)
            {
              ex._partiallyComputedNode = (AST) null;
              throw ex;
            }
          }
          finally
          {
            this.noSkipTokenSet.Remove(tokens);
          }
        }
      }
      catch (RecoveryTokenException ex)
      {
        ex._partiallyComputedNode = (AST) classBody;
        throw ex;
      }
      finally
      {
        this.noSkipTokenSet.Remove(NoSkipTokenSet.s_BlockNoSkipTokenSet);
      }
      classBody.context.UpdateWith(this.currentToken);
      this.GetNextToken();
    }
    finally
    {
      this.blockType.RemoveAt(this.blockType.Count - 1);
    }
    return classBody;
  }

  private AST ParseClassMember(bool isInterface)
  {
    bool parsedOK = false;
    if (isInterface && this.currentToken.token == JSToken.Public)
      this.GetNextToken();
    switch (this.currentToken.token)
    {
      case JSToken.Import:
        this.ReportError(JSError.InvalidImport, true);
        try
        {
          this.ParseImportStatement();
        }
        catch (RecoveryTokenException ex)
        {
        }
        return (AST) null;
      case JSToken.Package:
        Context context = this.currentToken.Clone();
        if (this.ParsePackage(context) is Package)
          this.ReportError(JSError.PackageInWrongContext, context, true);
        return (AST) null;
      case JSToken.Internal:
      case JSToken.Abstract:
      case JSToken.Public:
      case JSToken.Static:
      case JSToken.Private:
      case JSToken.Protected:
      case JSToken.Final:
        if (isInterface)
        {
          this.ReportError(JSError.BadModifierInInterface, true);
          this.GetNextToken();
          this.SkipTokensAndThrow();
        }
        return this.ParseAttributes((AST) null, true, true, out parsedOK);
      case JSToken.Var:
      case JSToken.Const:
        if (isInterface)
        {
          this.ReportError(JSError.VarIllegalInInterface, true);
          this.GetNextToken();
          this.SkipTokensAndThrow();
        }
        return this.ParseVariableStatement(FieldAttributes.PrivateScope, (CustomAttributeList) null, this.currentToken.token);
      case JSToken.Class:
        if (isInterface)
        {
          this.ReportError(JSError.SyntaxError, true);
          this.GetNextToken();
          this.SkipTokensAndThrow();
        }
        return this.ParseClass(FieldAttributes.PrivateScope, false, this.currentToken.Clone(), false, false, (CustomAttributeList) null);
      case JSToken.Function:
        return this.ParseFunction(FieldAttributes.PrivateScope, false, this.currentToken.Clone(), true, isInterface, false, isInterface, (CustomAttributeList) null);
      case JSToken.Semicolon:
        this.GetNextToken();
        return this.ParseClassMember(isInterface);
      case JSToken.Identifier:
        if (isInterface)
        {
          this.ReportError(JSError.SyntaxError, true);
          this.GetNextToken();
          this.SkipTokensAndThrow();
        }
        bool canBeAttribute = true;
        AST statement = this.ParseUnaryExpression(out bool _, ref canBeAttribute, false);
        if (canBeAttribute)
        {
          statement = this.ParseAttributes(statement, true, true, out parsedOK);
          if (parsedOK)
            return statement;
        }
        this.ReportError(JSError.SyntaxError, statement.context.Clone(), true);
        this.SkipTokensAndThrow();
        return (AST) null;
      case JSToken.Interface:
        if (isInterface)
        {
          this.ReportError(JSError.InterfaceIllegalInInterface, true);
          this.GetNextToken();
          this.SkipTokensAndThrow();
        }
        return this.ParseClass(FieldAttributes.PrivateScope, false, this.currentToken.Clone(), false, false, (CustomAttributeList) null);
      case JSToken.RightCurly:
        return (AST) null;
      case JSToken.Enum:
        return this.ParseEnum(FieldAttributes.PrivateScope, this.currentToken.Clone(), (CustomAttributeList) null);
      default:
        this.ReportError(JSError.SyntaxError, true);
        this.GetNextToken();
        this.SkipTokensAndThrow();
        return (AST) null;
    }
  }

  private AST ParseEnum(
    FieldAttributes visibilitySpec,
    Context enumCtx,
    CustomAttributeList customAttributes)
  {
    AST expression = (AST) null;
    TypeExpression baseType = (TypeExpression) null;
    this.GetNextToken();
    IdentifierLiteral identifierLiteral;
    if (JSToken.Identifier == this.currentToken.token)
    {
      identifierLiteral = new IdentifierLiteral(this.scanner.GetIdentifier(), this.currentToken.Clone());
    }
    else
    {
      this.ReportError(JSError.NoIdentifier);
      if (JSToken.Colon != this.currentToken.token && JSToken.LeftCurly != this.currentToken.token)
        this.SkipTokensAndThrow();
      identifierLiteral = new IdentifierLiteral("##Missing Enum Name##" + JSParser.s_cDummyName++.ToString(), this.CurrentPositionContext());
    }
    this.GetNextToken();
    if (JSToken.Colon == this.currentToken.token)
    {
      this.noSkipTokenSet.Add(NoSkipTokenSet.s_EnumBaseTypeNoSkipTokenSet);
      try
      {
        expression = this.ParseQualifiedIdentifier(JSError.NeedType);
      }
      catch (RecoveryTokenException ex)
      {
        if (this.IndexOfToken(NoSkipTokenSet.s_ClassExtendsNoSkipTokenSet, ex) == -1)
        {
          ex._partiallyComputedNode = (AST) null;
          throw ex;
        }
        expression = ex._partiallyComputedNode;
      }
      finally
      {
        this.noSkipTokenSet.Remove(NoSkipTokenSet.s_EnumBaseTypeNoSkipTokenSet);
      }
    }
    if (expression != null)
      baseType = new TypeExpression(expression);
    if (JSToken.LeftCurly != this.currentToken.token)
      this.ReportError(JSError.NoLeftCurly);
    ArrayList blockType = this.blockType;
    this.blockType = new ArrayList(16 /*0x10*/);
    SimpleHashtable labelTable = this.labelTable;
    this.labelTable = new SimpleHashtable(16U /*0x10*/);
    this.Globals.ScopeStack.Push((object) new ClassScope((AST) identifierLiteral, ((IActivationObject) this.Globals.ScopeStack.Peek()).GetGlobalScope()));
    try
    {
      Block classBody = this.ParseClassBody(true, false);
      enumCtx.UpdateWith(classBody.context);
      EnumDeclaration target = new EnumDeclaration(enumCtx, identifierLiteral, baseType, classBody, visibilitySpec, customAttributes);
      customAttributes?.SetTarget((AST) target);
      return (AST) target;
    }
    catch (RecoveryTokenException ex)
    {
      enumCtx.UpdateWith(ex._partiallyComputedNode.context);
      ex._partiallyComputedNode = (AST) new EnumDeclaration(enumCtx, identifierLiteral, baseType, (Block) ex._partiallyComputedNode, visibilitySpec, customAttributes);
      customAttributes?.SetTarget(ex._partiallyComputedNode);
      throw ex;
    }
    finally
    {
      this.Globals.ScopeStack.Pop();
      this.blockType = blockType;
      this.labelTable = labelTable;
    }
  }

  private AST ParseEnumMember()
  {
    AST enumMember = (AST) null;
    AST ast = (AST) null;
    switch (this.currentToken.token)
    {
      case JSToken.Var:
        this.ReportError(JSError.NoVarInEnum, true);
        this.GetNextToken();
        return this.ParseEnumMember();
      case JSToken.Semicolon:
        this.GetNextToken();
        return this.ParseEnumMember();
      case JSToken.Identifier:
        Lookup identifier = new Lookup(this.currentToken.Clone());
        Context context = this.currentToken.Clone();
        this.GetNextToken();
        if (JSToken.Assign == this.currentToken.token)
        {
          this.GetNextToken();
          ast = this.ParseExpression(true);
        }
        if (JSToken.Comma == this.currentToken.token)
          this.GetNextToken();
        else if (JSToken.RightCurly != this.currentToken.token)
          this.ReportError(JSError.NoComma, true);
        return (AST) new Constant(context, identifier, (TypeExpression) null, ast, FieldAttributes.Public, (CustomAttributeList) null);
      default:
        this.ReportError(JSError.SyntaxError, true);
        this.SkipTokensAndThrow();
        return enumMember;
    }
  }

  private bool GuessIfAbstract()
  {
    switch (this.currentToken.token)
    {
      case JSToken.Package:
      case JSToken.Internal:
      case JSToken.Abstract:
      case JSToken.Public:
      case JSToken.Static:
      case JSToken.Private:
      case JSToken.Protected:
      case JSToken.Final:
      case JSToken.Const:
      case JSToken.Class:
      case JSToken.Function:
      case JSToken.Interface:
      case JSToken.RightCurly:
      case JSToken.Enum:
        return true;
      case JSToken.Semicolon:
        this.GetNextToken();
        return true;
      default:
        return false;
    }
  }

  private AST ParseFunction(
    FieldAttributes visibilitySpec,
    bool inExpression,
    Context fncCtx,
    bool isMethod,
    bool isAbstract,
    bool isFinal,
    bool isInterface,
    CustomAttributeList customAttributes)
  {
    return this.ParseFunction(visibilitySpec, inExpression, fncCtx, isMethod, isAbstract, isFinal, isInterface, customAttributes, (Call) null);
  }

  private AST ParseFunction(
    FieldAttributes visibilitySpec,
    bool inExpression,
    Context fncCtx,
    bool isMethod,
    bool isAbstract,
    bool isFinal,
    bool isInterface,
    CustomAttributeList customAttributes,
    Call function)
  {
    if (this.demandFullTrustOnFunctionCreation)
      new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
    AST ast = (AST) null;
    ArrayList parameters = (ArrayList) null;
    TypeExpression return_type = (TypeExpression) null;
    Block body = (Block) null;
    bool isGetter = false;
    bool isSetter = false;
    IdentifierLiteral id;
    if (function == null)
    {
      this.GetNextToken();
      if (isMethod)
      {
        if (JSToken.Get == this.currentToken.token)
        {
          isGetter = true;
          this.GetNextToken();
        }
        else if (JSToken.Set == this.currentToken.token)
        {
          isSetter = true;
          this.GetNextToken();
        }
      }
      if (JSToken.Identifier == this.currentToken.token)
      {
        id = new IdentifierLiteral(this.scanner.GetIdentifier(), this.currentToken.Clone());
        this.GetNextToken();
        if (JSToken.AccessField == this.currentToken.token)
        {
          if (isInterface)
            this.ReportError(JSError.SyntaxError, true);
          this.GetNextToken();
          if (JSToken.Identifier == this.currentToken.token)
          {
            ast = (AST) new Lookup(id.context);
            id = new IdentifierLiteral(this.scanner.GetIdentifier(), this.currentToken.Clone());
            this.GetNextToken();
            while (JSToken.AccessField == this.currentToken.token)
            {
              this.GetNextToken();
              if (JSToken.Identifier == this.currentToken.token)
              {
                ast = (AST) new Member(ast.context.CombineWith(this.currentToken), ast, (AST) new ConstantWrapper((object) id.ToString(), id.context));
                id = new IdentifierLiteral(this.scanner.GetIdentifier(), this.currentToken.Clone());
                this.GetNextToken();
              }
              else
                this.ReportError(JSError.NoIdentifier, true);
            }
          }
          else
            this.ReportError(JSError.NoIdentifier, true);
        }
      }
      else
      {
        string identifier1 = JSKeyword.CanBeIdentifier(this.currentToken.token);
        if (identifier1 != null)
        {
          this.ForceReportInfo(JSError.KeywordUsedAsIdentifier, isMethod);
          id = new IdentifierLiteral(identifier1, this.currentToken.Clone());
          this.GetNextToken();
        }
        else
        {
          string identifier2;
          if (!inExpression)
          {
            identifier2 = this.currentToken.GetCode();
            this.ReportError(JSError.NoIdentifier, true);
            this.GetNextToken();
          }
          else
            identifier2 = "";
          id = new IdentifierLiteral(identifier2, this.CurrentPositionContext());
        }
      }
    }
    else
      id = function.GetName();
    ArrayList blockType = this.blockType;
    this.blockType = new ArrayList(16 /*0x10*/);
    SimpleHashtable labelTable = this.labelTable;
    this.labelTable = new SimpleHashtable(16U /*0x10*/);
    FunctionScope own_scope = new FunctionScope(this.Globals.ScopeStack.Peek(), isMethod);
    this.Globals.ScopeStack.Push((object) own_scope);
    try
    {
      parameters = new ArrayList();
      Context context1 = (Context) null;
      if (function == null)
      {
        if (JSToken.LeftParen != this.currentToken.token)
          this.ReportError(JSError.NoLeftParen);
        this.GetNextToken();
        while (JSToken.RightParen != this.currentToken.token)
        {
          if (context1 != null)
          {
            this.ReportError(JSError.ParamListNotLast, context1, true);
            context1 = (Context) null;
          }
          string identifier = (string) null;
          TypeExpression type = (TypeExpression) null;
          this.noSkipTokenSet.Add(NoSkipTokenSet.s_FunctionDeclNoSkipTokenSet);
          try
          {
            if (JSToken.ParamArray == this.currentToken.token)
            {
              context1 = this.currentToken.Clone();
              this.GetNextToken();
            }
            if (JSToken.Identifier != this.currentToken.token && (identifier = JSKeyword.CanBeIdentifier(this.currentToken.token)) == null)
            {
              if (JSToken.LeftCurly == this.currentToken.token)
              {
                this.ReportError(JSError.NoRightParen);
                break;
              }
              if (JSToken.Comma == this.currentToken.token)
              {
                this.ReportError(JSError.SyntaxError, true);
              }
              else
              {
                this.ReportError(JSError.SyntaxError, true);
                this.SkipTokensAndThrow();
              }
            }
            else
            {
              if (identifier == null)
                identifier = this.scanner.GetIdentifier();
              else
                this.ForceReportInfo(JSError.KeywordUsedAsIdentifier);
              Context context2 = this.currentToken.Clone();
              this.GetNextToken();
              if (JSToken.Colon == this.currentToken.token)
              {
                type = this.ParseTypeExpression();
                if (type != null)
                  context2.UpdateWith(type.context);
              }
              CustomAttributeList customAttributes1 = (CustomAttributeList) null;
              if (context1 != null)
              {
                customAttributes1 = new CustomAttributeList(context1);
                customAttributes1.Append(new CustomAttribute(context1, (AST) new Lookup("...", context1), new ASTList((Context) null)));
              }
              parameters.Add((object) new ParameterDeclaration(context2, identifier, type, customAttributes1));
            }
            if (JSToken.RightParen != this.currentToken.token)
            {
              if (JSToken.Comma != this.currentToken.token)
              {
                if (JSToken.LeftCurly == this.currentToken.token)
                {
                  this.ReportError(JSError.NoRightParen);
                  break;
                }
                if (JSToken.Identifier == this.currentToken.token && type == null)
                  this.ReportError(JSError.NoCommaOrTypeDefinitionError);
                else
                  this.ReportError(JSError.NoComma);
              }
              this.GetNextToken();
            }
            else
              break;
          }
          catch (RecoveryTokenException ex)
          {
            if (this.IndexOfToken(NoSkipTokenSet.s_FunctionDeclNoSkipTokenSet, ex) == -1)
              throw ex;
          }
          finally
          {
            this.noSkipTokenSet.Remove(NoSkipTokenSet.s_FunctionDeclNoSkipTokenSet);
          }
        }
        fncCtx.UpdateWith(this.currentToken);
        if (isGetter && parameters.Count != 0)
        {
          this.ReportError(JSError.BadPropertyDeclaration, true);
          isGetter = false;
        }
        else if (isSetter && parameters.Count != 1)
        {
          this.ReportError(JSError.BadPropertyDeclaration, true);
          isSetter = false;
        }
        this.GetNextToken();
        if (JSToken.Colon == this.currentToken.token)
        {
          if (isSetter)
            this.ReportError(JSError.SyntaxError);
          this.noSkipTokenSet.Add(NoSkipTokenSet.s_StartBlockNoSkipTokenSet);
          try
          {
            return_type = this.ParseTypeExpression();
          }
          catch (RecoveryTokenException ex)
          {
            if (this.IndexOfToken(NoSkipTokenSet.s_StartBlockNoSkipTokenSet, ex) == -1)
            {
              ex._partiallyComputedNode = (AST) null;
              throw ex;
            }
            if (ex._partiallyComputedNode != null)
              return_type = (TypeExpression) ex._partiallyComputedNode;
          }
          finally
          {
            this.noSkipTokenSet.Remove(NoSkipTokenSet.s_StartBlockNoSkipTokenSet);
          }
          if (isSetter)
            return_type = (TypeExpression) null;
        }
      }
      else
        function.GetParameters(parameters);
      if (JSToken.LeftCurly != this.currentToken.token && (isAbstract || isMethod && this.GuessIfAbstract()))
      {
        if (!isAbstract)
        {
          isAbstract = true;
          this.ReportError(JSError.ShouldBeAbstract, fncCtx, true);
        }
        body = new Block(this.currentToken.Clone());
      }
      else
      {
        if (JSToken.LeftCurly != this.currentToken.token)
          this.ReportError(JSError.NoLeftCurly, true);
        else if (isAbstract)
          this.ReportError(JSError.AbstractWithBody, fncCtx, true);
        this.blockType.Add((object) JSParser.BlockType.Block);
        this.noSkipTokenSet.Add(NoSkipTokenSet.s_BlockNoSkipTokenSet);
        this.noSkipTokenSet.Add(NoSkipTokenSet.s_StartStatementNoSkipTokenSet);
        try
        {
          body = new Block(this.currentToken.Clone());
          this.GetNextToken();
          while (JSToken.RightCurly != this.currentToken.token)
          {
            try
            {
              body.Append(this.ParseStatement());
            }
            catch (RecoveryTokenException ex)
            {
              if (ex._partiallyComputedNode != null)
                body.Append(ex._partiallyComputedNode);
              if (this.IndexOfToken(NoSkipTokenSet.s_StartStatementNoSkipTokenSet, ex) == -1)
                throw ex;
            }
          }
          body.context.UpdateWith(this.currentToken);
          fncCtx.UpdateWith(this.currentToken);
        }
        catch (RecoveryTokenException ex)
        {
          if (this.IndexOfToken(NoSkipTokenSet.s_BlockNoSkipTokenSet, ex) == -1)
          {
            this.Globals.ScopeStack.Pop();
            try
            {
              ParameterDeclaration[] formal_parameters = new ParameterDeclaration[parameters.Count];
              parameters.CopyTo((Array) formal_parameters);
              ex._partiallyComputedNode = !inExpression ? (AST) new FunctionDeclaration(fncCtx, ast, id, formal_parameters, return_type, body, own_scope, visibilitySpec, isMethod, isGetter, isSetter, isAbstract, isFinal, customAttributes) : (AST) new FunctionExpression(fncCtx, (AST) id, formal_parameters, return_type, body, own_scope, visibilitySpec);
              customAttributes?.SetTarget(ex._partiallyComputedNode);
            }
            finally
            {
              this.Globals.ScopeStack.Push((object) own_scope);
            }
            throw ex;
          }
        }
        finally
        {
          this.blockType.RemoveAt(this.blockType.Count - 1);
          this.noSkipTokenSet.Remove(NoSkipTokenSet.s_StartStatementNoSkipTokenSet);
          this.noSkipTokenSet.Remove(NoSkipTokenSet.s_BlockNoSkipTokenSet);
        }
        this.GetNextToken();
      }
    }
    finally
    {
      this.blockType = blockType;
      this.labelTable = labelTable;
      this.Globals.ScopeStack.Pop();
    }
    ParameterDeclaration[] formal_parameters1 = new ParameterDeclaration[parameters.Count];
    parameters.CopyTo((Array) formal_parameters1);
    AST target = !inExpression ? (AST) new FunctionDeclaration(fncCtx, ast, id, formal_parameters1, return_type, body, own_scope, visibilitySpec, isMethod, isGetter, isSetter, isAbstract, isFinal, customAttributes) : (AST) new FunctionExpression(fncCtx, (AST) id, formal_parameters1, return_type, body, own_scope, visibilitySpec);
    customAttributes?.SetTarget(target);
    return target;
  }

  internal AST ParseFunctionExpression()
  {
    this.demandFullTrustOnFunctionCreation = true;
    this.GetNextToken();
    return this.ParseFunction(FieldAttributes.PrivateScope, true, this.currentToken.Clone(), false, false, false, false, (CustomAttributeList) null);
  }

  internal string[] ParseNamedBreakpoint(out int argNumber)
  {
    argNumber = 0;
    AST qualifiedIdentifier = this.ParseQualifiedIdentifier(JSError.SyntaxError);
    if (qualifiedIdentifier == null)
      return (string[]) null;
    string[] strArray = new string[4]
    {
      qualifiedIdentifier.ToString(),
      null,
      null,
      null
    };
    if (JSToken.LeftParen == this.currentToken.token)
    {
      strArray[1] = "";
      this.GetNextToken();
      while (JSToken.RightParen != this.currentToken.token)
      {
        string name = (string) null;
        if (JSToken.Identifier != this.currentToken.token && (name = JSKeyword.CanBeIdentifier(this.currentToken.token)) == null)
          return (string[]) null;
        if (name == null)
          name = this.scanner.GetIdentifier();
        AST qualid = (AST) new Lookup(name, this.currentToken.Clone());
        this.GetNextToken();
        string str;
        if (JSToken.AccessField == this.currentToken.token)
        {
          str = this.ParseScopeSequence(qualid, JSError.SyntaxError).ToString();
          while (JSToken.LeftBracket == this.currentToken.token)
          {
            this.GetNextToken();
            if (JSToken.RightBracket != this.currentToken.token)
              return (string[]) null;
            str += "[]";
            this.GetNextToken();
          }
        }
        else
        {
          if (JSToken.Colon == this.currentToken.token)
          {
            this.GetNextToken();
            if (JSToken.RightParen == this.currentToken.token)
              return (string[]) null;
            continue;
          }
          str = qualid.ToString();
        }
        ref string local = ref strArray[1];
        local = $"{local}{str} ";
        ++argNumber;
        if (JSToken.Comma == this.currentToken.token)
        {
          this.GetNextToken();
          if (JSToken.RightParen == this.currentToken.token)
            return (string[]) null;
        }
      }
      this.GetNextToken();
      if (JSToken.Colon == this.currentToken.token)
      {
        this.GetNextToken();
        string name = (string) null;
        if (JSToken.Identifier != this.currentToken.token && (name = JSKeyword.CanBeIdentifier(this.currentToken.token)) == null)
          return (string[]) null;
        if (name == null)
          name = this.scanner.GetIdentifier();
        AST qualid = (AST) new Lookup(name, this.currentToken.Clone());
        this.GetNextToken();
        if (JSToken.AccessField == this.currentToken.token)
        {
          AST scopeSequence = this.ParseScopeSequence(qualid, JSError.SyntaxError);
          strArray[2] = scopeSequence.ToString();
          while (JSToken.LeftBracket == this.currentToken.token)
          {
            this.GetNextToken();
            if (JSToken.RightBracket != this.currentToken.token)
              return (string[]) null;
            strArray[2] += "[]";
            this.GetNextToken();
          }
        }
        else
          strArray[2] = qualid.ToString();
      }
    }
    if (JSToken.FirstBinaryOp == this.currentToken.token)
    {
      this.GetNextToken();
      if (JSToken.IntegerLiteral != this.currentToken.token)
        return (string[]) null;
      strArray[3] = this.currentToken.GetCode();
      this.GetNextToken();
    }
    return this.currentToken.token != JSToken.EndOfFile ? (string[]) null : strArray;
  }

  private AST ParsePackage(Context packageContext)
  {
    this.GetNextToken();
    AST ast1 = (AST) null;
    bool flag = this.scanner.GotEndOfLine();
    if (JSToken.Identifier != this.currentToken.token)
    {
      if (JSScanner.CanParseAsExpression(this.currentToken.token))
      {
        this.ReportError(JSError.KeywordUsedAsIdentifier, packageContext.Clone(), true);
        bool isLeftHandSideExpr;
        AST expression = this.ParseExpression(this.ParsePostfixExpression(this.MemberExpression((AST) new Lookup("package", packageContext), (ArrayList) null), out isLeftHandSideExpr), false, isLeftHandSideExpr, JSToken.None);
        return (AST) new Expression(expression.context.Clone(), expression);
      }
      if (!flag)
      {
        if (JSToken.Increment == this.currentToken.token || JSToken.Decrement == this.currentToken.token)
        {
          this.ReportError(JSError.KeywordUsedAsIdentifier, packageContext.Clone(), true);
          AST expression = this.ParseExpression(this.ParsePostfixExpression((AST) new Lookup("package", packageContext), out bool _), false, false, JSToken.None);
          return (AST) new Expression(expression.context.Clone(), expression);
        }
      }
      else
      {
        this.ReportError(JSError.KeywordUsedAsIdentifier, packageContext.Clone(), true);
        return (AST) new Lookup("package", packageContext);
      }
    }
    else
    {
      this.errorToken = this.currentToken;
      ast1 = this.ParseQualifiedIdentifier(JSError.NoIdentifier);
    }
    Context context1 = (Context) null;
    if (JSToken.LeftCurly != this.currentToken.token && ast1 == null)
    {
      context1 = this.currentToken.Clone();
      this.GetNextToken();
    }
    if (JSToken.LeftCurly == this.currentToken.token)
    {
      if (ast1 == null)
      {
        if (context1 == null)
          context1 = this.currentToken.Clone();
        this.ReportError(JSError.NoIdentifier, context1, true);
      }
    }
    else if (ast1 == null)
    {
      this.ReportError(JSError.SyntaxError, packageContext);
      if (JSScanner.CanStartStatement(context1.token))
      {
        this.currentToken = context1;
        return this.ParseStatement();
      }
      if (JSScanner.CanStartStatement(this.currentToken.token))
      {
        this.errorToken = (Context) null;
        return this.ParseStatement();
      }
      this.ReportError(JSError.SyntaxError);
      this.SkipTokensAndThrow();
    }
    else
    {
      if (flag)
      {
        this.ReportError(JSError.KeywordUsedAsIdentifier, packageContext.Clone(), true);
        Block package = new Block(packageContext.Clone());
        package.Append((AST) new Lookup("package", packageContext));
        AST expression = this.ParseExpression(this.ParsePostfixExpression(this.MemberExpression(ast1, (ArrayList) null), out bool _), false, true, JSToken.None);
        package.Append((AST) new Expression(expression.context.Clone(), expression));
        package.context.UpdateWith(expression.context);
        return (AST) package;
      }
      this.ReportError(JSError.NoLeftCurly);
    }
    PackageScope packageScope = new PackageScope(this.Globals.ScopeStack.Peek());
    this.Globals.ScopeStack.Push((object) packageScope);
    try
    {
      string name = ast1 != null ? ast1.ToString() : "anonymous package";
      packageScope.name = name;
      packageContext.UpdateWith(this.currentToken);
      ASTList classList = new ASTList(packageContext);
      this.GetNextToken();
      this.noSkipTokenSet.Add(NoSkipTokenSet.s_BlockNoSkipTokenSet);
      this.noSkipTokenSet.Add(NoSkipTokenSet.s_PackageBodyNoSkipTokenSet);
      try
      {
        while (this.currentToken.token != JSToken.RightCurly)
        {
          AST ast2 = (AST) null;
          try
          {
            switch (this.currentToken.token)
            {
              case JSToken.EndOfFile:
                this.EOFError(JSError.ErrEOF);
                throw new EndOfFile();
              case JSToken.Import:
                this.ReportError(JSError.InvalidImport, true);
                try
                {
                  this.ParseImportStatement();
                  continue;
                }
                catch (RecoveryTokenException ex)
                {
                  continue;
                }
              case JSToken.Package:
                Context context2 = this.currentToken.Clone();
                if (this.ParsePackage(context2) is Package)
                {
                  this.ReportError(JSError.PackageInWrongContext, context2, true);
                  continue;
                }
                continue;
              case JSToken.Internal:
              case JSToken.Abstract:
              case JSToken.Public:
              case JSToken.Static:
              case JSToken.Private:
              case JSToken.Protected:
              case JSToken.Final:
                bool parsedOK1;
                AST attributes = this.ParseAttributes((AST) null, true, false, out parsedOK1);
                if (parsedOK1 && attributes is Class)
                {
                  classList.Append(attributes);
                  continue;
                }
                this.ReportError(JSError.OnlyClassesAllowed, attributes.context.Clone(), true);
                this.SkipTokensAndThrow();
                continue;
              case JSToken.Class:
              case JSToken.Interface:
                classList.Append(this.ParseClass(FieldAttributes.PrivateScope, false, this.currentToken.Clone(), false, false, (CustomAttributeList) null));
                continue;
              case JSToken.Semicolon:
                this.GetNextToken();
                continue;
              case JSToken.Identifier:
                bool canBeAttribute = true;
                AST ast3 = this.ParseUnaryExpression(out bool _, ref canBeAttribute, false);
                if (canBeAttribute)
                {
                  bool parsedOK2;
                  ast3 = this.ParseAttributes(ast3, true, false, out parsedOK2);
                  if (parsedOK2 && ast3 is Class)
                  {
                    classList.Append(ast3);
                    continue;
                  }
                }
                this.ReportError(JSError.OnlyClassesAllowed, ast3.context.Clone(), true);
                this.SkipTokensAndThrow();
                continue;
              case JSToken.Enum:
                classList.Append(this.ParseEnum(FieldAttributes.PrivateScope, this.currentToken.Clone(), (CustomAttributeList) null));
                continue;
              default:
                this.ReportError(JSError.OnlyClassesAllowed, ast2 != null ? ast2.context.Clone() : this.CurrentPositionContext(), true);
                this.SkipTokensAndThrow();
                continue;
            }
          }
          catch (RecoveryTokenException ex)
          {
            if (ex._partiallyComputedNode != null && ex._partiallyComputedNode is Class)
            {
              classList.Append(ex._partiallyComputedNode);
              ex._partiallyComputedNode = (AST) null;
            }
            if (this.IndexOfToken(NoSkipTokenSet.s_PackageBodyNoSkipTokenSet, ex) == -1)
              throw ex;
          }
        }
      }
      catch (RecoveryTokenException ex)
      {
        if (this.IndexOfToken(NoSkipTokenSet.s_BlockNoSkipTokenSet, ex) == -1)
        {
          this.ReportError(JSError.NoRightCurly, this.CurrentPositionContext());
          ex._partiallyComputedNode = (AST) new Package(name, ast1, classList, packageContext);
          throw ex;
        }
      }
      finally
      {
        this.noSkipTokenSet.Remove(NoSkipTokenSet.s_PackageBodyNoSkipTokenSet);
        this.noSkipTokenSet.Remove(NoSkipTokenSet.s_BlockNoSkipTokenSet);
      }
      this.GetNextToken();
      return (AST) new Package(name, ast1, classList, packageContext);
    }
    finally
    {
      this.Globals.ScopeStack.Pop();
    }
  }

  private AST ParseStaticInitializer(Context initContext)
  {
    if (this.demandFullTrustOnFunctionCreation)
      new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
    Block body = (Block) null;
    FunctionScope own_scope = new FunctionScope(this.Globals.ScopeStack.Peek());
    own_scope.isStatic = true;
    ArrayList blockType = this.blockType;
    this.blockType = new ArrayList(16 /*0x10*/);
    SimpleHashtable labelTable = this.labelTable;
    this.labelTable = new SimpleHashtable(16U /*0x10*/);
    this.blockType.Add((object) JSParser.BlockType.Block);
    this.noSkipTokenSet.Add(NoSkipTokenSet.s_BlockNoSkipTokenSet);
    this.noSkipTokenSet.Add(NoSkipTokenSet.s_StartStatementNoSkipTokenSet);
    try
    {
      this.Globals.ScopeStack.Push((object) own_scope);
      body = new Block(this.currentToken.Clone());
      this.GetNextToken();
      while (JSToken.RightCurly != this.currentToken.token)
      {
        try
        {
          body.Append(this.ParseStatement());
        }
        catch (RecoveryTokenException ex)
        {
          if (ex._partiallyComputedNode != null)
            body.Append(ex._partiallyComputedNode);
          if (this.IndexOfToken(NoSkipTokenSet.s_StartStatementNoSkipTokenSet, ex) == -1)
            throw ex;
        }
      }
    }
    catch (RecoveryTokenException ex)
    {
      if (this.IndexOfToken(NoSkipTokenSet.s_BlockNoSkipTokenSet, ex) == -1)
      {
        ex._partiallyComputedNode = (AST) new StaticInitializer(initContext, body, own_scope);
        throw ex;
      }
    }
    finally
    {
      this.noSkipTokenSet.Remove(NoSkipTokenSet.s_StartStatementNoSkipTokenSet);
      this.noSkipTokenSet.Remove(NoSkipTokenSet.s_BlockNoSkipTokenSet);
      this.blockType = blockType;
      this.labelTable = labelTable;
      this.Globals.ScopeStack.Pop();
    }
    body.context.UpdateWith(this.currentToken);
    initContext.UpdateWith(this.currentToken);
    this.GetNextToken();
    return (AST) new StaticInitializer(initContext, body, own_scope);
  }

  private AST ParseExpression()
  {
    bool isLeftHandSideExpr;
    return this.ParseExpression(this.ParseUnaryExpression(out isLeftHandSideExpr, false), false, isLeftHandSideExpr, JSToken.None);
  }

  private AST ParseExpression(bool single)
  {
    bool isLeftHandSideExpr;
    return this.ParseExpression(this.ParseUnaryExpression(out isLeftHandSideExpr, false), single, isLeftHandSideExpr, JSToken.None);
  }

  private AST ParseExpression(bool single, JSToken inToken)
  {
    bool isLeftHandSideExpr;
    return this.ParseExpression(this.ParseUnaryExpression(out isLeftHandSideExpr, false), single, isLeftHandSideExpr, inToken);
  }

  private AST ParseExpression(AST leftHandSide, bool single, bool bCanAssign, JSToken inToken)
  {
    OpListItem prev1 = new OpListItem(JSToken.None, OpPrec.precNone, (OpListItem) null);
    AstListItem prev2 = new AstListItem(leftHandSide, (AstListItem) null);
    try
    {
      while (JSScanner.IsProcessableOperator(this.currentToken.token) && inToken != this.currentToken.token)
      {
        OpPrec operatorPrecedence = JSScanner.GetOperatorPrecedence(this.currentToken.token);
        bool flag = JSScanner.IsRightAssociativeOperator(this.currentToken.token);
        while (operatorPrecedence < prev1._prec || operatorPrecedence == prev1._prec && !flag)
        {
          AST expressionNode = this.CreateExpressionNode(prev1._operator, prev2._prev._term, prev2._term);
          prev1 = prev1._prev;
          AstListItem prev3 = prev2._prev._prev;
          prev2 = new AstListItem(expressionNode, prev3);
        }
        if (JSToken.ConditionalIf == this.currentToken.token)
        {
          AST term = prev2._term;
          AstListItem prev4 = prev2._prev;
          this.GetNextToken();
          AST expression1 = this.ParseExpression(true);
          if (JSToken.Colon != this.currentToken.token)
            this.ReportError(JSError.NoColon);
          this.GetNextToken();
          AST expression2 = this.ParseExpression(true, inToken);
          prev2 = new AstListItem((AST) new Conditional(term.context.CombineWith(expression2.context), term, expression1, expression2), prev4);
        }
        else
        {
          if (JSScanner.IsAssignmentOperator(this.currentToken.token))
          {
            if (!bCanAssign)
            {
              this.ReportError(JSError.IllegalAssignment);
              this.SkipTokensAndThrow();
            }
          }
          else
            bCanAssign = false;
          prev1 = new OpListItem(this.currentToken.token, operatorPrecedence, prev1);
          this.GetNextToken();
          if (bCanAssign)
          {
            prev2 = new AstListItem(this.ParseUnaryExpression(out bCanAssign, false), prev2);
          }
          else
          {
            bool isLeftHandSideExpr;
            prev2 = new AstListItem(this.ParseUnaryExpression(out isLeftHandSideExpr, false), prev2);
            isLeftHandSideExpr = isLeftHandSideExpr;
          }
        }
      }
      while (prev1._operator != JSToken.None)
      {
        AST expressionNode = this.CreateExpressionNode(prev1._operator, prev2._prev._term, prev2._term);
        prev1 = prev1._prev;
        AstListItem prev5 = prev2._prev._prev;
        prev2 = new AstListItem(expressionNode, prev5);
      }
      if (!single && JSToken.Comma == this.currentToken.token)
      {
        this.GetNextToken();
        AST expression = this.ParseExpression(false, inToken);
        prev2._term = (AST) new Comma(prev2._term.context.CombineWith(expression.context), prev2._term, expression);
      }
      return prev2._term;
    }
    catch (RecoveryTokenException ex)
    {
      ex._partiallyComputedNode = leftHandSide;
      throw ex;
    }
  }

  private AST ParseUnaryExpression(out bool isLeftHandSideExpr, bool isMinus)
  {
    bool canBeAttribute = false;
    return this.ParseUnaryExpression(out isLeftHandSideExpr, ref canBeAttribute, isMinus, false);
  }

  private AST ParseUnaryExpression(
    out bool isLeftHandSideExpr,
    ref bool canBeAttribute,
    bool isMinus)
  {
    return this.ParseUnaryExpression(out isLeftHandSideExpr, ref canBeAttribute, isMinus, true);
  }

  private AST ParseUnaryExpression(
    out bool isLeftHandSideExpr,
    ref bool canBeAttribute,
    bool isMinus,
    bool warnForKeyword)
  {
    AST ast = (AST) null;
    isLeftHandSideExpr = false;
    bool isLeftHandSideExpr1 = false;
    AST unaryExpression1;
    switch (this.currentToken.token)
    {
      case JSToken.FirstOp:
        Context context1 = this.currentToken.Clone();
        this.GetNextToken();
        canBeAttribute = false;
        AST unaryExpression2 = this.ParseUnaryExpression(out isLeftHandSideExpr1, ref canBeAttribute, false);
        context1.UpdateWith(unaryExpression2.context);
        unaryExpression1 = (AST) new NumericUnary(context1, unaryExpression2, JSToken.FirstOp);
        break;
      case JSToken.BitwiseNot:
        Context context2 = this.currentToken.Clone();
        this.GetNextToken();
        canBeAttribute = false;
        AST unaryExpression3 = this.ParseUnaryExpression(out isLeftHandSideExpr1, ref canBeAttribute, false);
        context2.UpdateWith(unaryExpression3.context);
        unaryExpression1 = (AST) new NumericUnary(context2, unaryExpression3, JSToken.BitwiseNot);
        break;
      case JSToken.Delete:
        Context context3 = this.currentToken.Clone();
        this.GetNextToken();
        canBeAttribute = false;
        AST unaryExpression4 = this.ParseUnaryExpression(out isLeftHandSideExpr1, ref canBeAttribute, false);
        context3.UpdateWith(unaryExpression4.context);
        unaryExpression1 = (AST) new Delete(context3, unaryExpression4);
        break;
      case JSToken.Void:
        Context context4 = this.currentToken.Clone();
        this.GetNextToken();
        canBeAttribute = false;
        AST unaryExpression5 = this.ParseUnaryExpression(out isLeftHandSideExpr1, ref canBeAttribute, false);
        context4.UpdateWith(unaryExpression5.context);
        unaryExpression1 = (AST) new VoidOp(context4, unaryExpression5);
        break;
      case JSToken.Typeof:
        Context context5 = this.currentToken.Clone();
        this.GetNextToken();
        canBeAttribute = false;
        AST unaryExpression6 = this.ParseUnaryExpression(out isLeftHandSideExpr1, ref canBeAttribute, false);
        context5.UpdateWith(unaryExpression6.context);
        unaryExpression1 = (AST) new Typeof(context5, unaryExpression6);
        break;
      case JSToken.Increment:
        Context context6 = this.currentToken.Clone();
        this.GetNextToken();
        canBeAttribute = false;
        AST unaryExpression7 = this.ParseUnaryExpression(out isLeftHandSideExpr1, ref canBeAttribute, false);
        context6.UpdateWith(unaryExpression7.context);
        unaryExpression1 = (AST) new PostOrPrefixOperator(context6, unaryExpression7, PostOrPrefix.PrefixIncrement);
        break;
      case JSToken.Decrement:
        Context context7 = this.currentToken.Clone();
        this.GetNextToken();
        canBeAttribute = false;
        AST unaryExpression8 = this.ParseUnaryExpression(out isLeftHandSideExpr1, ref canBeAttribute, false);
        context7.UpdateWith(unaryExpression8.context);
        unaryExpression1 = (AST) new PostOrPrefixOperator(context7, unaryExpression8, PostOrPrefix.PrefixDecrement);
        break;
      case JSToken.FirstBinaryOp:
        Context context8 = this.currentToken.Clone();
        this.GetNextToken();
        canBeAttribute = false;
        AST unaryExpression9 = this.ParseUnaryExpression(out isLeftHandSideExpr1, ref canBeAttribute, false);
        context8.UpdateWith(unaryExpression9.context);
        unaryExpression1 = (AST) new NumericUnary(context8, unaryExpression9, JSToken.FirstBinaryOp);
        break;
      case JSToken.Minus:
        Context context9 = this.currentToken.Clone();
        this.GetNextToken();
        canBeAttribute = false;
        AST unaryExpression10 = this.ParseUnaryExpression(out isLeftHandSideExpr1, ref canBeAttribute, true);
        if (unaryExpression10.context.token == JSToken.NumericLiteral)
        {
          context9.UpdateWith(unaryExpression10.context);
          unaryExpression10.context = context9;
          unaryExpression1 = unaryExpression10;
          break;
        }
        context9.UpdateWith(unaryExpression10.context);
        unaryExpression1 = (AST) new NumericUnary(context9, unaryExpression10, JSToken.Minus);
        break;
      default:
        this.noSkipTokenSet.Add(NoSkipTokenSet.s_PostfixExpressionNoSkipTokenSet);
        try
        {
          ast = this.ParseLeftHandSideExpression(isMinus, ref canBeAttribute, warnForKeyword);
        }
        catch (RecoveryTokenException ex)
        {
          if (this.IndexOfToken(NoSkipTokenSet.s_PostfixExpressionNoSkipTokenSet, ex) == -1)
            throw ex;
          if (ex._partiallyComputedNode == null)
            this.SkipTokensAndThrow();
          else
            ast = ex._partiallyComputedNode;
        }
        finally
        {
          this.noSkipTokenSet.Remove(NoSkipTokenSet.s_PostfixExpressionNoSkipTokenSet);
        }
        unaryExpression1 = this.ParsePostfixExpression(ast, out isLeftHandSideExpr, ref canBeAttribute);
        break;
    }
    isLeftHandSideExpr1 = isLeftHandSideExpr1;
    return unaryExpression1;
  }

  private AST ParsePostfixExpression(AST ast, out bool isLeftHandSideExpr)
  {
    bool canBeAttribute = false;
    return this.ParsePostfixExpression(ast, out isLeftHandSideExpr, ref canBeAttribute);
  }

  private AST ParsePostfixExpression(AST ast, out bool isLeftHandSideExpr, ref bool canBeAttribute)
  {
    isLeftHandSideExpr = true;
    if (ast != null && !this.scanner.GotEndOfLine())
    {
      if (JSToken.Increment == this.currentToken.token)
      {
        isLeftHandSideExpr = false;
        Context context = ast.context.Clone();
        context.UpdateWith(this.currentToken);
        canBeAttribute = false;
        ast = (AST) new PostOrPrefixOperator(context, ast, PostOrPrefix.PostfixIncrement);
        this.GetNextToken();
      }
      else if (JSToken.Decrement == this.currentToken.token)
      {
        isLeftHandSideExpr = false;
        Context context = ast.context.Clone();
        context.UpdateWith(this.currentToken);
        canBeAttribute = false;
        ast = (AST) new PostOrPrefixOperator(context, ast, PostOrPrefix.PostfixDecrement);
        this.GetNextToken();
      }
    }
    return ast;
  }

  private AST ParseLeftHandSideExpression() => this.ParseLeftHandSideExpression(false);

  private AST ParseLeftHandSideExpression(bool isMinus)
  {
    bool canBeAttribute = false;
    return this.ParseLeftHandSideExpression(isMinus, ref canBeAttribute, false);
  }

  private AST ParseLeftHandSideExpression(
    bool isMinus,
    ref bool canBeAttribute,
    bool warnForKeyword)
  {
    AST expression1 = (AST) null;
    bool flag1 = false;
    ArrayList newContexts = (ArrayList) null;
    while (JSToken.New == this.currentToken.token)
    {
      if (newContexts == null)
        newContexts = new ArrayList(4);
      newContexts.Add((object) this.currentToken.Clone());
      this.GetNextToken();
    }
    switch (this.currentToken.token)
    {
      case JSToken.Function:
        canBeAttribute = false;
        expression1 = this.ParseFunction(FieldAttributes.PrivateScope, true, this.currentToken.Clone(), false, false, false, false, (CustomAttributeList) null);
        flag1 = true;
        break;
      case JSToken.LeftCurly:
        canBeAttribute = false;
        Context context1 = this.currentToken.Clone();
        this.GetNextToken();
        ASTList propertyList = new ASTList(this.currentToken.Clone());
        if (JSToken.RightCurly != this.currentToken.token)
        {
          while (true)
          {
            AST elem1;
            if (JSToken.Identifier == this.currentToken.token)
              elem1 = (AST) new ConstantWrapper((object) this.scanner.GetIdentifier(), this.currentToken.Clone());
            else if (JSToken.StringLiteral == this.currentToken.token)
              elem1 = (AST) new ConstantWrapper((object) this.scanner.GetStringLiteral(), this.currentToken.Clone());
            else if (JSToken.IntegerLiteral == this.currentToken.token || JSToken.NumericLiteral == this.currentToken.token)
            {
              elem1 = (AST) new ConstantWrapper((object) Convert.ToNumber(this.currentToken.GetCode(), true, true, (object) Missing.Value), this.currentToken.Clone());
              ((ConstantWrapper) elem1).isNumericLiteral = true;
            }
            else
            {
              this.ReportError(JSError.NoMemberIdentifier);
              elem1 = (AST) new IdentifierLiteral("_#Missing_Field#_" + JSParser.s_cDummyName++.ToString(), this.CurrentPositionContext());
            }
            ASTList elem2 = new ASTList(elem1.context.Clone());
            this.GetNextToken();
            this.noSkipTokenSet.Add(NoSkipTokenSet.s_ObjectInitNoSkipTokenSet);
            try
            {
              AST expression2;
              if (JSToken.Colon != this.currentToken.token)
              {
                this.ReportError(JSError.NoColon, true);
                expression2 = this.ParseExpression(true);
              }
              else
              {
                this.GetNextToken();
                expression2 = this.ParseExpression(true);
              }
              elem2.Append(elem1);
              elem2.Append(expression2);
              propertyList.Append((AST) elem2);
              if (JSToken.RightCurly != this.currentToken.token)
              {
                if (JSToken.Comma == this.currentToken.token)
                {
                  this.GetNextToken();
                }
                else
                {
                  if (this.scanner.GotEndOfLine())
                    this.ReportError(JSError.NoRightCurly);
                  else
                    this.ReportError(JSError.NoComma, true);
                  this.SkipTokensAndThrow();
                }
              }
              else
                break;
            }
            catch (RecoveryTokenException ex)
            {
              if (ex._partiallyComputedNode != null)
              {
                AST partiallyComputedNode = ex._partiallyComputedNode;
                elem2.Append(elem1);
                elem2.Append(partiallyComputedNode);
                propertyList.Append((AST) elem2);
              }
              if (this.IndexOfToken(NoSkipTokenSet.s_ObjectInitNoSkipTokenSet, ex) == -1)
              {
                ex._partiallyComputedNode = (AST) new ObjectLiteral(context1, propertyList);
                throw ex;
              }
              if (JSToken.Comma == this.currentToken.token)
                this.GetNextToken();
              if (JSToken.RightCurly == this.currentToken.token)
                break;
            }
            finally
            {
              this.noSkipTokenSet.Remove(NoSkipTokenSet.s_ObjectInitNoSkipTokenSet);
            }
          }
        }
        propertyList.context.UpdateWith(this.currentToken);
        context1.UpdateWith(this.currentToken);
        expression1 = (AST) new ObjectLiteral(context1, propertyList);
        break;
      case JSToken.Null:
        canBeAttribute = false;
        expression1 = (AST) new NullLiteral(this.currentToken.Clone());
        break;
      case JSToken.True:
        canBeAttribute = false;
        expression1 = (AST) new ConstantWrapper((object) true, this.currentToken.Clone());
        break;
      case JSToken.False:
        canBeAttribute = false;
        expression1 = (AST) new ConstantWrapper((object) false, this.currentToken.Clone());
        break;
      case JSToken.This:
        canBeAttribute = false;
        expression1 = (AST) new ThisLiteral(this.currentToken.Clone(), false);
        break;
      case JSToken.Identifier:
        expression1 = (AST) new Lookup(this.scanner.GetIdentifier(), this.currentToken.Clone());
        break;
      case JSToken.StringLiteral:
        canBeAttribute = false;
        expression1 = (AST) new ConstantWrapper((object) this.scanner.GetStringLiteral(), this.currentToken.Clone());
        break;
      case JSToken.IntegerLiteral:
        canBeAttribute = false;
        expression1 = (AST) new ConstantWrapper(Convert.LiteralToNumber(this.currentToken.GetCode(), this.currentToken) ?? (object) 0, this.currentToken.Clone());
        ((ConstantWrapper) expression1).isNumericLiteral = true;
        break;
      case JSToken.NumericLiteral:
        canBeAttribute = false;
        expression1 = (AST) new ConstantWrapper((object) Convert.ToNumber(isMinus ? "-" + this.currentToken.GetCode() : this.currentToken.GetCode(), false, false, (object) Missing.Value), this.currentToken.Clone());
        ((ConstantWrapper) expression1).isNumericLiteral = true;
        break;
      case JSToken.LeftParen:
        canBeAttribute = false;
        this.GetNextToken();
        this.noSkipTokenSet.Add(NoSkipTokenSet.s_ParenExpressionNoSkipToken);
        try
        {
          expression1 = this.ParseExpression();
          if (JSToken.RightParen != this.currentToken.token)
            this.ReportError(JSError.NoRightParen);
        }
        catch (RecoveryTokenException ex)
        {
          expression1 = this.IndexOfToken(NoSkipTokenSet.s_ParenExpressionNoSkipToken, ex) != -1 ? ex._partiallyComputedNode : throw ex;
        }
        finally
        {
          this.noSkipTokenSet.Remove(NoSkipTokenSet.s_ParenExpressionNoSkipToken);
        }
        if (expression1 == null)
        {
          this.SkipTokensAndThrow();
          break;
        }
        break;
      case JSToken.LeftBracket:
        canBeAttribute = false;
        Context context2 = this.currentToken.Clone();
        ASTList elements = new ASTList(this.currentToken.Clone());
        this.GetNextToken();
        if (this.currentToken.token == JSToken.Identifier && this.scanner.PeekToken() == JSToken.Colon)
        {
          this.noSkipTokenSet.Add(NoSkipTokenSet.s_BracketToken);
          try
          {
            if (this.currentToken.GetCode() == "assembly")
            {
              this.GetNextToken();
              this.GetNextToken();
              return (AST) new AssemblyCustomAttributeList(this.ParseCustomAttributeList());
            }
            this.ReportError(JSError.ExpectedAssembly);
            this.SkipTokensAndThrow();
          }
          catch (RecoveryTokenException ex)
          {
            ex._partiallyComputedNode = (AST) new Block(context2);
            return ex._partiallyComputedNode;
          }
          finally
          {
            if (this.currentToken.token == JSToken.RightBracket)
            {
              this.errorToken = (Context) null;
              this.GetNextToken();
            }
            this.noSkipTokenSet.Remove(NoSkipTokenSet.s_BracketToken);
          }
        }
        while (JSToken.RightBracket != this.currentToken.token)
        {
          if (JSToken.Comma != this.currentToken.token)
          {
            this.noSkipTokenSet.Add(NoSkipTokenSet.s_ArrayInitNoSkipTokenSet);
            try
            {
              elements.Append(this.ParseExpression(true));
              if (JSToken.Comma != this.currentToken.token)
              {
                if (JSToken.RightBracket != this.currentToken.token)
                {
                  this.ReportError(JSError.NoRightBracket);
                  break;
                }
                break;
              }
            }
            catch (RecoveryTokenException ex)
            {
              if (ex._partiallyComputedNode != null)
                elements.Append(ex._partiallyComputedNode);
              if (this.IndexOfToken(NoSkipTokenSet.s_ArrayInitNoSkipTokenSet, ex) == -1)
              {
                context2.UpdateWith(this.CurrentPositionContext());
                ex._partiallyComputedNode = (AST) new ArrayLiteral(context2, elements);
                throw ex;
              }
              if (JSToken.RightBracket == this.currentToken.token)
                break;
            }
            finally
            {
              this.noSkipTokenSet.Remove(NoSkipTokenSet.s_ArrayInitNoSkipTokenSet);
            }
          }
          else
            elements.Append((AST) new ConstantWrapper((object) Missing.Value, this.currentToken.Clone()));
          this.GetNextToken();
        }
        context2.UpdateWith(this.currentToken);
        expression1 = (AST) new ArrayLiteral(context2, elements);
        break;
      case JSToken.Divide:
        canBeAttribute = false;
        string str = this.scanner.ScanRegExp();
        if (str != null)
        {
          bool flag2 = false;
          try
          {
            Regex regex = new Regex(str, RegexOptions.ECMAScript);
          }
          catch (ArgumentException ex)
          {
            str = "";
            flag2 = true;
          }
          string flags = this.scanner.ScanRegExpFlags();
          if (flags == null)
          {
            expression1 = (AST) new RegExpLiteral(str, (string) null, this.currentToken.Clone());
          }
          else
          {
            try
            {
              expression1 = (AST) new RegExpLiteral(str, flags, this.currentToken.Clone());
            }
            catch (JScriptException ex)
            {
              expression1 = (AST) new RegExpLiteral(str, (string) null, this.currentToken.Clone());
              flag2 = true;
            }
          }
          if (flag2)
          {
            this.ReportError(JSError.RegExpSyntax, true);
            break;
          }
          break;
        }
        goto default;
      case JSToken.Super:
        canBeAttribute = false;
        expression1 = (AST) new ThisLiteral(this.currentToken.Clone(), true);
        break;
      case JSToken.PreProcessorConstant:
        canBeAttribute = false;
        expression1 = (AST) new ConstantWrapper(this.scanner.GetPreProcessorValue(), this.currentToken.Clone());
        break;
      default:
        string name = JSKeyword.CanBeIdentifier(this.currentToken.token);
        if (name != null)
        {
          if (warnForKeyword)
          {
            switch (this.currentToken.token)
            {
              case JSToken.Void:
              case JSToken.Boolean:
              case JSToken.Byte:
              case JSToken.Char:
              case JSToken.Double:
              case JSToken.Float:
              case JSToken.Int:
              case JSToken.Long:
              case JSToken.Short:
                break;
              default:
                this.ForceReportInfo(JSError.KeywordUsedAsIdentifier);
                break;
            }
          }
          canBeAttribute = false;
          expression1 = (AST) new Lookup(name, this.currentToken.Clone());
          break;
        }
        if (this.currentToken.token == JSToken.BitwiseAnd)
        {
          this.ReportError(JSError.WrongUseOfAddressOf);
          this.errorToken = (Context) null;
          this.GetNextToken();
          return this.ParseLeftHandSideExpression(isMinus, ref canBeAttribute, warnForKeyword);
        }
        this.ReportError(JSError.ExpressionExpected);
        this.SkipTokensAndThrow();
        break;
    }
    if (!flag1)
      this.GetNextToken();
    return this.MemberExpression(expression1, newContexts, ref canBeAttribute);
  }

  private AST ParseConstructorCall(Context superCtx)
  {
    bool isSuperConstructorCall = JSToken.Super == this.currentToken.token;
    this.GetNextToken();
    Context context = this.currentToken.Clone();
    ASTList arguments = new ASTList(context);
    this.noSkipTokenSet.Add(NoSkipTokenSet.s_EndOfStatementNoSkipTokenSet);
    this.noSkipTokenSet.Add(NoSkipTokenSet.s_ParenToken);
    try
    {
      arguments = this.ParseExpressionList(JSToken.RightParen);
      this.GetNextToken();
    }
    catch (RecoveryTokenException ex)
    {
      if (ex._partiallyComputedNode != null)
        arguments = (ASTList) ex._partiallyComputedNode;
      if (this.IndexOfToken(NoSkipTokenSet.s_ParenToken, ex) == -1 && this.IndexOfToken(NoSkipTokenSet.s_EndOfStatementNoSkipTokenSet, ex) == -1)
      {
        ex._partiallyComputedNode = (AST) new ConstructorCall(superCtx, arguments, isSuperConstructorCall);
        throw ex;
      }
      if (ex._token == JSToken.RightParen)
        this.GetNextToken();
    }
    finally
    {
      this.noSkipTokenSet.Remove(NoSkipTokenSet.s_ParenToken);
      this.noSkipTokenSet.Remove(NoSkipTokenSet.s_EndOfStatementNoSkipTokenSet);
    }
    superCtx.UpdateWith(context);
    return (AST) new ConstructorCall(superCtx, arguments, isSuperConstructorCall);
  }

  private CustomAttributeList ParseCustomAttributeList()
  {
    CustomAttributeList customAttributeList = new CustomAttributeList(this.currentToken.Clone());
    while (true)
    {
      Context context = this.currentToken.Clone();
      bool canBeAttribute = true;
      AST unaryExpression = this.ParseUnaryExpression(out bool _, ref canBeAttribute, false, false);
      if (canBeAttribute)
      {
        switch (unaryExpression)
        {
          case Lookup _:
          case Member _:
            customAttributeList.Append(new CustomAttribute(unaryExpression.context, unaryExpression, new ASTList((Context) null)));
            break;
          default:
            customAttributeList.Append(((Call) unaryExpression).ToCustomAttribute());
            break;
        }
      }
      else if (this.tokensSkipped == 0)
        this.ReportError(JSError.SyntaxError, context);
      if (this.currentToken.token != JSToken.RightBracket)
      {
        if (this.currentToken.token == JSToken.Comma)
        {
          this.GetNextToken();
        }
        else
        {
          this.ReportError(JSError.NoRightBracketOrComma);
          this.SkipTokensAndThrow();
        }
      }
      else
        break;
    }
    return customAttributeList;
  }

  private AST MemberExpression(AST expression, ArrayList newContexts)
  {
    bool canBeAttribute = false;
    return this.MemberExpression(expression, newContexts, ref canBeAttribute);
  }

  private AST MemberExpression(AST expression, ArrayList newContexts, ref bool canBeAttribute)
  {
    return this.MemberExpression(expression, newContexts, out bool _, ref canBeAttribute);
  }

  private AST MemberExpression(
    AST expression,
    ArrayList newContexts,
    out bool canBeQualid,
    ref bool canBeAttribute)
  {
    bool flag = false;
    canBeQualid = true;
    while (true)
    {
      this.noSkipTokenSet.Add(NoSkipTokenSet.s_MemberExprNoSkipTokenSet);
      try
      {
        ASTList astList;
        switch (this.currentToken.token)
        {
          case JSToken.LeftParen:
            if (flag)
              canBeAttribute = false;
            else
              flag = true;
            canBeQualid = false;
            astList = (ASTList) null;
            RecoveryTokenException recoveryTokenException = (RecoveryTokenException) null;
            this.noSkipTokenSet.Add(NoSkipTokenSet.s_ParenToken);
            try
            {
              astList = this.ParseExpressionList(JSToken.RightParen);
            }
            catch (RecoveryTokenException ex)
            {
              astList = (ASTList) ex._partiallyComputedNode;
              if (this.IndexOfToken(NoSkipTokenSet.s_ParenToken, ex) == -1)
                recoveryTokenException = ex;
            }
            finally
            {
              this.noSkipTokenSet.Remove(NoSkipTokenSet.s_ParenToken);
            }
            if (expression is Lookup)
            {
              string str = expression.ToString();
              if (str.Equals("eval"))
              {
                expression.context.UpdateWith(astList.context);
                expression = astList.count != 1 ? (astList.count <= 1 ? (AST) new Eval(expression.context, (AST) new ConstantWrapper((object) "", this.CurrentPositionContext()), (AST) null) : (AST) new Eval(expression.context, astList[0], astList[1])) : (AST) new Eval(expression.context, astList[0], (AST) null);
                canBeAttribute = false;
              }
              else if (this.Globals.engine.doPrint && str.Equals("print"))
              {
                expression.context.UpdateWith(astList.context);
                expression = (AST) new Print(expression.context, (AST) astList);
                canBeAttribute = false;
              }
              else
                expression = (AST) new Call(expression.context.CombineWith(astList.context), expression, astList, false);
            }
            else
              expression = (AST) new Call(expression.context.CombineWith(astList.context), expression, astList, false);
            if (newContexts != null && newContexts.Count > 0)
            {
              ((Context) newContexts[newContexts.Count - 1]).UpdateWith(expression.context);
              if (!(expression is Call))
                expression = (AST) new Call((Context) newContexts[newContexts.Count - 1], expression, new ASTList(this.CurrentPositionContext()), false);
              else
                expression.context = (Context) newContexts[newContexts.Count - 1];
              ((Call) expression).isConstructor = true;
              newContexts.RemoveAt(newContexts.Count - 1);
            }
            if (recoveryTokenException != null)
            {
              recoveryTokenException._partiallyComputedNode = expression;
              throw recoveryTokenException;
            }
            this.GetNextToken();
            continue;
          case JSToken.LeftBracket:
            canBeQualid = false;
            canBeAttribute = false;
            this.noSkipTokenSet.Add(NoSkipTokenSet.s_BracketToken);
            try
            {
              astList = this.ParseExpressionList(JSToken.RightBracket);
            }
            catch (RecoveryTokenException ex)
            {
              if (this.IndexOfToken(NoSkipTokenSet.s_BracketToken, ex) == -1)
              {
                ex._partiallyComputedNode = ex._partiallyComputedNode == null ? expression : (AST) new Call(expression.context.CombineWith(this.currentToken.Clone()), expression, (ASTList) ex._partiallyComputedNode, true);
                throw ex;
              }
              astList = (ASTList) ex._partiallyComputedNode;
            }
            finally
            {
              this.noSkipTokenSet.Remove(NoSkipTokenSet.s_BracketToken);
            }
            expression = (AST) new Call(expression.context.CombineWith(this.currentToken.Clone()), expression, astList, true);
            if (newContexts != null && newContexts.Count > 0)
            {
              ((Context) newContexts[newContexts.Count - 1]).UpdateWith(expression.context);
              expression.context = (Context) newContexts[newContexts.Count - 1];
              ((Call) expression).isConstructor = true;
              newContexts.RemoveAt(newContexts.Count - 1);
            }
            this.GetNextToken();
            continue;
          case JSToken.AccessField:
            if (flag)
              canBeAttribute = false;
            ConstantWrapper memberName = (ConstantWrapper) null;
            this.GetNextToken();
            if (JSToken.Identifier != this.currentToken.token)
            {
              string str = JSKeyword.CanBeIdentifier(this.currentToken.token);
              if (str != null)
              {
                this.ForceReportInfo(JSError.KeywordUsedAsIdentifier);
                memberName = new ConstantWrapper((object) str, this.currentToken.Clone());
              }
              else
              {
                this.ReportError(JSError.NoIdentifier);
                this.SkipTokensAndThrow(expression);
              }
            }
            else
              memberName = new ConstantWrapper((object) this.scanner.GetIdentifier(), this.currentToken.Clone());
            this.GetNextToken();
            expression = (AST) new Member(expression.context.CombineWith(memberName.context), expression, (AST) memberName);
            continue;
          default:
            if (newContexts != null)
            {
              while (newContexts.Count > 0)
              {
                ((Context) newContexts[newContexts.Count - 1]).UpdateWith(expression.context);
                expression = (AST) new Call((Context) newContexts[newContexts.Count - 1], expression, new ASTList(this.CurrentPositionContext()), false);
                ((Call) expression).isConstructor = true;
                newContexts.RemoveAt(newContexts.Count - 1);
              }
            }
            return expression;
        }
      }
      catch (RecoveryTokenException ex)
      {
        expression = this.IndexOfToken(NoSkipTokenSet.s_MemberExprNoSkipTokenSet, ex) != -1 ? ex._partiallyComputedNode : throw ex;
      }
      finally
      {
        this.noSkipTokenSet.Remove(NoSkipTokenSet.s_MemberExprNoSkipTokenSet);
      }
    }
  }

  private ASTList ParseExpressionList(JSToken terminator)
  {
    Context context1 = this.currentToken.Clone();
    this.scanner.GetCurrentLine();
    this.GetNextToken();
    ASTList expressionList = new ASTList(context1);
    if (terminator != this.currentToken.token)
    {
      while (true)
      {
        this.noSkipTokenSet.Add(NoSkipTokenSet.s_ExpressionListNoSkipTokenSet);
        try
        {
          if (JSToken.BitwiseAnd == this.currentToken.token)
          {
            Context context2 = this.currentToken.Clone();
            this.GetNextToken();
            AST handSideExpression = this.ParseLeftHandSideExpression();
            switch (handSideExpression)
            {
              case Member _:
              case Lookup _:
                context2.UpdateWith(handSideExpression.context);
                expressionList.Append((AST) new AddressOf(context2, handSideExpression));
                break;
              default:
                this.ReportError(JSError.DoesNotHaveAnAddress, context2.Clone());
                expressionList.Append(handSideExpression);
                break;
            }
          }
          else if (JSToken.Comma == this.currentToken.token)
            expressionList.Append((AST) new ConstantWrapper((object) System.Reflection.Missing.Value, this.currentToken.Clone()));
          else if (terminator != this.currentToken.token)
            expressionList.Append(this.ParseExpression(true));
          else
            break;
          if (terminator != this.currentToken.token)
          {
            if (JSToken.Comma != this.currentToken.token)
            {
              if (terminator == JSToken.RightParen)
              {
                if (JSToken.Semicolon == this.currentToken.token && JSToken.RightParen == this.scanner.PeekToken())
                {
                  this.ReportError(JSError.UnexpectedSemicolon, true);
                  this.GetNextToken();
                  break;
                }
                this.ReportError(JSError.NoRightParenOrComma);
              }
              else
                this.ReportError(JSError.NoRightBracketOrComma);
              this.SkipTokensAndThrow();
            }
          }
          else
            break;
        }
        catch (RecoveryTokenException ex)
        {
          if (ex._partiallyComputedNode != null)
            expressionList.Append(ex._partiallyComputedNode);
          if (this.IndexOfToken(NoSkipTokenSet.s_ExpressionListNoSkipTokenSet, ex) == -1)
          {
            ex._partiallyComputedNode = (AST) expressionList;
            throw ex;
          }
        }
        finally
        {
          this.noSkipTokenSet.Remove(NoSkipTokenSet.s_ExpressionListNoSkipTokenSet);
        }
        this.GetNextToken();
      }
    }
    context1.UpdateWith(this.currentToken);
    return expressionList;
  }

  private AST CreateExpressionNode(JSToken op, AST operand1, AST operand2)
  {
    Context context = operand1.context.CombineWith(operand2.context);
    switch (op)
    {
      case JSToken.FirstBinaryOp:
        return (AST) new Plus(context, operand1, operand2);
      case JSToken.Minus:
        return (AST) new NumericBinary(context, operand1, operand2, JSToken.Minus);
      case JSToken.LogicalOr:
        return (AST) new Logical_or(context, operand1, operand2);
      case JSToken.LogicalAnd:
        return (AST) new Logical_and(context, operand1, operand2);
      case JSToken.BitwiseOr:
        return (AST) new BitwiseBinary(context, operand1, operand2, JSToken.BitwiseOr);
      case JSToken.BitwiseXor:
        return (AST) new BitwiseBinary(context, operand1, operand2, JSToken.BitwiseXor);
      case JSToken.BitwiseAnd:
        return (AST) new BitwiseBinary(context, operand1, operand2, JSToken.BitwiseAnd);
      case JSToken.Equal:
        return (AST) new Equality(context, operand1, operand2, JSToken.Equal);
      case JSToken.NotEqual:
        return (AST) new Equality(context, operand1, operand2, JSToken.NotEqual);
      case JSToken.StrictEqual:
        return (AST) new StrictEquality(context, operand1, operand2, JSToken.StrictEqual);
      case JSToken.StrictNotEqual:
        return (AST) new StrictEquality(context, operand1, operand2, JSToken.StrictNotEqual);
      case JSToken.GreaterThan:
        return (AST) new Relational(context, operand1, operand2, JSToken.GreaterThan);
      case JSToken.LessThan:
        return (AST) new Relational(context, operand1, operand2, JSToken.LessThan);
      case JSToken.LessThanEqual:
        return (AST) new Relational(context, operand1, operand2, JSToken.LessThanEqual);
      case JSToken.GreaterThanEqual:
        return (AST) new Relational(context, operand1, operand2, JSToken.GreaterThanEqual);
      case JSToken.LeftShift:
        return (AST) new BitwiseBinary(context, operand1, operand2, JSToken.LeftShift);
      case JSToken.RightShift:
        return (AST) new BitwiseBinary(context, operand1, operand2, JSToken.RightShift);
      case JSToken.UnsignedRightShift:
        return (AST) new BitwiseBinary(context, operand1, operand2, JSToken.UnsignedRightShift);
      case JSToken.Multiply:
        return (AST) new NumericBinary(context, operand1, operand2, JSToken.Multiply);
      case JSToken.Divide:
        return (AST) new NumericBinary(context, operand1, operand2, JSToken.Divide);
      case JSToken.Modulo:
        return (AST) new NumericBinary(context, operand1, operand2, JSToken.Modulo);
      case JSToken.Instanceof:
        return (AST) new Instanceof(context, operand1, operand2);
      case JSToken.In:
        return (AST) new In(context, operand1, operand2);
      case JSToken.Assign:
        return (AST) new Assign(context, operand1, operand2);
      case JSToken.PlusAssign:
        return (AST) new PlusAssign(context, operand1, operand2);
      case JSToken.MinusAssign:
        return (AST) new NumericBinaryAssign(context, operand1, operand2, JSToken.Minus);
      case JSToken.MultiplyAssign:
        return (AST) new NumericBinaryAssign(context, operand1, operand2, JSToken.Multiply);
      case JSToken.DivideAssign:
        return (AST) new NumericBinaryAssign(context, operand1, operand2, JSToken.Divide);
      case JSToken.BitwiseAndAssign:
        return (AST) new BitwiseBinaryAssign(context, operand1, operand2, JSToken.BitwiseAnd);
      case JSToken.BitwiseOrAssign:
        return (AST) new BitwiseBinaryAssign(context, operand1, operand2, JSToken.BitwiseOr);
      case JSToken.BitwiseXorAssign:
        return (AST) new BitwiseBinaryAssign(context, operand1, operand2, JSToken.BitwiseXor);
      case JSToken.ModuloAssign:
        return (AST) new NumericBinaryAssign(context, operand1, operand2, JSToken.Modulo);
      case JSToken.LeftShiftAssign:
        return (AST) new BitwiseBinaryAssign(context, operand1, operand2, JSToken.LeftShift);
      case JSToken.RightShiftAssign:
        return (AST) new BitwiseBinaryAssign(context, operand1, operand2, JSToken.RightShift);
      case JSToken.UnsignedRightShiftAssign:
        return (AST) new BitwiseBinaryAssign(context, operand1, operand2, JSToken.UnsignedRightShift);
      case JSToken.Comma:
        return (AST) new Comma(context, operand1, operand2);
      default:
        return (AST) null;
    }
  }

  private void GetNextToken()
  {
    if (this.errorToken != null)
    {
      if (this.breakRecursion > 10)
      {
        this.errorToken = (Context) null;
        this.scanner.GetNextToken();
      }
      else
      {
        ++this.breakRecursion;
        this.currentToken = this.errorToken;
        this.errorToken = (Context) null;
      }
    }
    else
    {
      ++this.goodTokensProcessed;
      this.breakRecursion = 0;
      this.scanner.GetNextToken();
    }
  }

  private Context CurrentPositionContext()
  {
    Context context = this.currentToken.Clone();
    context.endPos = context.startPos < context.source_string.Length ? context.startPos + 1 : context.startPos;
    return context;
  }

  private void ReportError(JSError errorId) => this.ReportError(errorId, false);

  private void ReportError(JSError errorId, bool skipToken)
  {
    Context context = this.currentToken.Clone();
    context.endPos = context.startPos + 1;
    this.ReportError(errorId, context, skipToken);
  }

  private void ReportError(JSError errorId, Context context)
  {
    this.ReportError(errorId, context, false);
  }

  private void ReportError(JSError errorId, Context context, bool skipToken)
  {
    int severity = this.Severity;
    this.Severity = new JScriptException(errorId).Severity;
    if (context.token == JSToken.EndOfFile)
    {
      this.EOFError(errorId);
    }
    else
    {
      if (this.goodTokensProcessed > 0L || this.Severity < severity)
        context.HandleError(errorId);
      if (skipToken)
      {
        this.goodTokensProcessed = -1L;
      }
      else
      {
        this.errorToken = this.currentToken;
        this.goodTokensProcessed = 0L;
      }
    }
  }

  private void ForceReportInfo(JSError errorId)
  {
    this.ForceReportInfo(this.currentToken.Clone(), errorId);
  }

  private void ForceReportInfo(Context context, JSError errorId) => context.HandleError(errorId);

  private void ForceReportInfo(JSError errorId, bool treatAsError)
  {
    this.currentToken.Clone().HandleError(errorId, treatAsError);
  }

  private void EOFError(JSError errorId)
  {
    Context context = this.sourceContext.Clone();
    context.lineNumber = this.scanner.GetCurrentLine();
    context.endLineNumber = context.lineNumber;
    context.startLinePos = this.scanner.GetStartLinePosition();
    context.endLinePos = context.startLinePos;
    context.startPos = this.sourceContext.endPos;
    ++context.endPos;
    context.HandleError(errorId);
  }

  private void SkipTokensAndThrow() => this.SkipTokensAndThrow((AST) null);

  private void SkipTokensAndThrow(AST partialAST)
  {
    this.errorToken = (Context) null;
    bool flag = this.noSkipTokenSet.HasToken(JSToken.EndOfLine);
    while (!this.noSkipTokenSet.HasToken(this.currentToken.token))
    {
      if (flag && this.scanner.GotEndOfLine())
      {
        this.errorToken = this.currentToken;
        throw new RecoveryTokenException(JSToken.EndOfLine, partialAST);
      }
      this.GetNextToken();
      if (++this.tokensSkipped > 50)
      {
        this.ForceReportInfo(JSError.TooManyTokensSkipped);
        throw new EndOfFile();
      }
      if (this.currentToken.token == JSToken.EndOfFile)
        throw new EndOfFile();
    }
    this.errorToken = this.currentToken;
    throw new RecoveryTokenException(this.currentToken.token, partialAST);
  }

  private int IndexOfToken(JSToken[] tokens, RecoveryTokenException exc)
  {
    return this.IndexOfToken(tokens, exc._token);
  }

  private int IndexOfToken(JSToken[] tokens, JSToken token)
  {
    int index = 0;
    int length = tokens.Length;
    while (index < length && tokens[index] != token)
      ++index;
    if (index >= length)
      index = -1;
    else
      this.errorToken = (Context) null;
    return index;
  }

  private bool TokenInList(JSToken[] tokens, JSToken token)
  {
    return -1 != this.IndexOfToken(tokens, token);
  }

  private bool TokenInList(JSToken[] tokens, RecoveryTokenException exc)
  {
    return -1 != this.IndexOfToken(tokens, exc._token);
  }

  private CustomAttributeList FromASTListToCustomAttributeList(ArrayList attributes)
  {
    CustomAttributeList customAttributeList = (CustomAttributeList) null;
    if (attributes != null && attributes.Count > 0)
      customAttributeList = new CustomAttributeList(((AST) attributes[0]).context);
    int index = 0;
    for (int count = attributes.Count; index < count; ++index)
    {
      ASTList args = new ASTList((Context) null);
      if (attributes[index] is Lookup || attributes[index] is Member)
        customAttributeList.Append(new CustomAttribute(((AST) attributes[index]).context, (AST) attributes[index], args));
      else
        customAttributeList.Append(((Call) attributes[index]).ToCustomAttribute());
    }
    return customAttributeList;
  }

  internal bool HasAborted => this.tokensSkipped > 50;

  private enum BlockType
  {
    Block,
    Loop,
    Switch,
    Finally,
  }
}
