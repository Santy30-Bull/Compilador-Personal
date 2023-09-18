using System.Runtime.Intrinsics.Arm;
using System.Reflection.Metadata.Ecma335;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//token
using static Token;
using static TokenType;
//ast
using static MainClass;
using ast;

//lexer
using static Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Text.Json;

public delegate Expression PrefixParseFn();
public delegate Expression InfixParseFn(Expression left);

public enum Precedence
{
    LOWEST = 1,
    EQUALS = 2,
    LESSGREATER = 3,
    SUM = 4,
    PRODUCT = 5,
    PREFIX = 6,
    CALL = 7,
    LET = 8,
    RETURN = 9
}

public class LetStatement : Statement
{
    public Expression ExpressionName { get; }
    public Expression ExpressionValue { get; }

    public LetStatement(Expression expressionName, Expression expressionValue, Token statementToken)
    : base(statementToken)
{
    ExpressionName = expressionName ?? throw new ArgumentNullException(nameof(expressionName));
    ExpressionValue = expressionValue ?? throw new ArgumentNullException(nameof(expressionValue));
}

    public override string TokenLiteral()
    {
        return ExpressionName.Token.Lexeme;
    }

    public override string ToString()
    {
        return $"{TokenLiteral()} {ExpressionName} = {ExpressionValue};";
    }
}

public class Expression
{
    public Token Token { get; }

    public Expression(Token token)
    {
        Token = token ?? throw new ArgumentNullException(nameof(token));
    }

    // Define a virtual TokenLiteral() method in the Expression class.
    public virtual string TokenLiteral()
    {
        return Token.Lexeme;
    }
}

public class ExpressionStatement : Statement
{
    public Token Token { get; }
    public Expression ExpressionValue { get; set; }

    public ExpressionStatement(Token token) : base(token)
    {
    }

    public override string TokenLiteral()
    {
        return Token.Lexeme;
    }

    public override string ToString()
    {
        return ExpressionValue?.ToString();
    }
}

public class BlockStatement : Statement
{
    public Token Token { get; }
    public List<Statement> Statements { get; set; }

    public BlockStatement(Token token) : base(token)
    {
        Statements = new List<Statement>();
    }

    public override string TokenLiteral()
    {
        return Token.Lexeme;
    }

    public override string ToString()
    {
        return "block statement";
    }
}

public class FunctionExpression : Expression
{
    public List<Parser.Identifier> Parameters { get; set; }
    public BlockStatement Body { get; set; }

    public FunctionExpression(Token token) : base(token)
    {
        Parameters = new List<Parser.Identifier>();
        Body = null;
    }

    public override string TokenLiteral()
    {
        return base.TokenLiteral();
    }

    public override string ToString()
    {
        return "function expression";
    }
}

public class IfExpression : Expression
{
    public Expression Condition { get; set; }
    public BlockStatement Consequence { get; set; }
    public BlockStatement Alternative { get; set; }

    public IfExpression(Token token, Expression condition, BlockStatement consequence, BlockStatement alternative)
        : base(token)
    {
        Condition = condition ?? throw new ArgumentNullException(nameof(condition));
        Consequence = consequence ?? throw new ArgumentNullException(nameof(consequence));
        Alternative = alternative ?? throw new ArgumentNullException(nameof(alternative));
    }
}

public class InfixExpression : Expression
{
    public Expression Left { get; set; }
    public string Operator { get; set; }
    public Expression Right { get; set; }

    public InfixExpression(Token token, string op, Expression left, Expression right)
        : base(token)
    {
        Operator = op;
        Left = left;
        Right = right ?? throw new ArgumentNullException(nameof(right));
    }
}

public class IntegerExpression : Expression
{
    public int Value { get; }

    public IntegerExpression(Token token, int value)
        : base(token)
    {
        Value = value;
    }

    public override string TokenLiteral()
    {
        return Token.Lexeme;
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}

public class PrefixExpression : Expression
{
    public string Operator { get; }
    public Expression Right { get; set; }

    public PrefixExpression(Token token, string op, Expression right)
        : base(token)
    {
        Operator = op;
        Right = right;
    }

    public override string TokenLiteral()
    {
        return Token.Lexeme;
    }

    public override string ToString()
    {
        return $"({Operator}{Right})";
    }
}

public class ReturnStatement : Statement
{
    public Token Token { get; }
    public Expression ReturnValue1 { get; set; }

    public ReturnStatement(Token token)
        : base(token)
    {
    }

    public void SetReturnValue(Expression value)
    {
        ReturnValue1 = value;
    }

    public override string TokenLiteral()
    {
        return Token.Lexeme;
    }

    public override string ToString()
    {
        return ReturnValue1?.ToString();
    }
}

public class StringExpression : Expression
{
    public string Value { get; }

    public StringExpression(Token token, string value)
        : base(token)
    {
        Value = value;
    }

    public override string TokenLiteral()
    {
        return Token.Lexeme;
    }

    public override string ToString()
    {
        return Value;
    }
}

public class WhileExpression : Expression
{
    public Expression Condition { get; set; }
    public BlockStatement Body { get; set; }

    public WhileExpression(Token token, Expression condition, BlockStatement body)
        : base(token)
    {
        Condition = condition ?? throw new ArgumentNullException(nameof(condition));
        Body = body ?? throw new ArgumentNullException(nameof(body));
    }

    public override string TokenLiteral()
    {
        return Token.Lexeme;
    }

    public override string ToString()
    {
        return $"while ({Condition}) {Body}";
    }
}

public class CallExpression : Expression
{
    public Expression Function { get; }
    public List<Expression> Arguments { get; }
    public Token Token { get; } // Add a Token property

    public CallExpression(Expression function, List<Expression> arguments, Token token)
        : base(token)
    {
        Function = function ?? throw new ArgumentNullException(nameof(function));
        Arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));
        Token = token ?? throw new ArgumentNullException(nameof(token));
    }

    // Other members of CallExpression...
}

public class Parser
{
    public Dictionary<TokenType, PrefixParseFn> PrefixParseFns = new Dictionary<TokenType, PrefixParseFn>();
    public Dictionary<TokenType, InfixParseFn> InfixParseFns = new Dictionary<TokenType, InfixParseFn>();
    public readonly Lexer lexer;
    public Token currentToken;
    public Token peekToken;

    public static readonly Dictionary<TokenType, Precedence> PRECEDENCES = new Dictionary<TokenType, Precedence>
    {
        { TokenType.Equals, Precedence.EQUALS },
        { TokenType.ASSING, Precedence.EQUALS }, // Corrected the TokenType
        { TokenType.LT, Precedence.LESSGREATER },
        { TokenType.GT, Precedence.LESSGREATER },
        { TokenType.PLUS, Precedence.SUM },
        { TokenType.MINUS, Precedence.SUM },
        { TokenType.SLASH, Precedence.PRODUCT },
        { TokenType.MULTIPLICATION, Precedence.PRODUCT },
        { TokenType.LPAREN, Precedence.CALL }
    };

    public void RegisterPrefix(TokenType tokenType, PrefixParseFn prefixFn)
    {
        PrefixParseFns[tokenType] = prefixFn;
    }
    public void RegisterInfix(TokenType tokenType, InfixParseFn infixFn)
    {
        InfixParseFns[tokenType] = infixFn;
    }

    public Parser(Lexer lexer)
    {
        this.lexer = lexer;
        currentToken = lexer.NextToken();
        peekToken = lexer.NextToken();
        AdvanceTokens();
        AdvanceTokens();
    }

    public bool Expect(TokenType expectedType)
{
    if (currentToken.Type == expectedType)
    {
        Consume(currentToken.Type);
        return true;
    }
    else
    {
        return false;
    }
}

    public void Consume(TokenType expectedType)
    {
        if (currentToken.Type == expectedType)
        {
            currentToken = lexer.NextToken();
        }
        else
        {
            throw new Exception($"Expected token type {expectedType}, but got {currentToken.Type}");
        }
    }

        public List<string> Errors { get; private set; }

    public Token PeekToken()
    {
        return peekToken;
    }

    public void AdvanceTokens()
    {
        currentToken = peekToken;
        peekToken = lexer.NextToken();
    }

    public Precedence CurrentPrecedence()
    {
        try
        {
            return PRECEDENCES[currentToken.Type];
        }
        catch (KeyNotFoundException)
        {
            return Precedence.LOWEST;
        }
    }

    public bool ExpectedToken(TokenType tokenType)
    {
        if (peekToken.Type == tokenType)
        {
            AdvanceTokens();
            return true;
        }

        ExpectedTokenError(tokenType);
        return false;
    }

    public void ExpectedTokenError(TokenType tokenType)
    {
        string error = $"Expected token type {tokenType}, but got {peekToken.Type}";
        Errors.Add(error);
    }

    public bool ParseBoolean()
{
    return currentToken.Type == TokenType.TRUE;
}

    public CallExpression ParseCall(Expression function)
{
    if (!ExpectedToken(TokenType.LPAREN))
    {
        return null;
    }

    AdvanceTokens();

    List<Expression> arguments = ParseCallArguments();

    if (!ExpectedToken(TokenType.RPAREN))
    {
        return null;
    }

    return new CallExpression(function, arguments, currentToken);
}

    public List<Expression> ParseCallArguments()
    {
        List<Expression> arguments = new List<Expression>();

        if (peekToken.Type == TokenType.RPAREN)
        {
            AdvanceTokens();
            return arguments;
        }

        AdvanceTokens();
        Expression expression = ParseExpression(Precedence.LOWEST);

        if (expression != null)
        {
            arguments.Add(expression);
        }

        while (peekToken.Type == TokenType.COMMA)
        {
            AdvanceTokens();
            AdvanceTokens();

            expression = ParseExpression(Precedence.LOWEST);

            if (expression != null)
            {
                arguments.Add(expression);
            }
        }

        if (!ExpectedToken(TokenType.RPAREN))
        {
            return null;
        }

        return arguments;
    }

    public Expression ParseExpression(Precedence precedence)
    {
        try
        {
            PrefixParseFn prefixParseFn = PrefixParseFns[currentToken.Type];
            Expression leftExpression = prefixParseFn();

            while (peekToken.Type != TokenType.SEMICOLON && precedence < PeekPrecedence())
            {
                try
                {
                    InfixParseFn infixParseFn = InfixParseFns[peekToken.Type];
                    AdvanceTokens();
                    leftExpression = infixParseFn(leftExpression);
                }
                catch (KeyNotFoundException)
                {
                    return leftExpression;
                }
            }

            return leftExpression;
        }
        catch (KeyNotFoundException)
        {
            string message = $"No parse function found for {currentToken.Lexeme}";
            Errors.Add(message);
            return null;
        }
    }

    public ExpressionStatement ParseExpressionStatement()
    {
        ExpressionStatement expressionStatement = new ExpressionStatement(currentToken);
        expressionStatement.ExpressionValue = ParseExpression(Precedence.LOWEST);

        if (peekToken.Type == TokenType.SEMICOLON)
        {
            AdvanceTokens();
        }

        return expressionStatement;
    }

    public Expression ParseGroupedExpression()
    {
        AdvanceTokens();
        Expression expression = ParseExpression(Precedence.LOWEST);

        if (!ExpectedToken(TokenType.RPAREN))
        {
            return null;
        }

        return expression;
    }

    public List<Identifier> ParseFunctionParameters()
{
    List<Identifier> parameters = new List<Identifier>();

    if (peekToken.Type == TokenType.RPAREN)
    {
        AdvanceTokens();
        return parameters;
    }

    AdvanceTokens();

    Identifier param = new Identifier(currentToken, currentToken.Lexeme);
    parameters.Add(param);

    while (peekToken.Type == TokenType.COMMA)
    {
        AdvanceTokens(); // Consume the comma
        AdvanceTokens(); // Move to the next parameter name

        param = new Identifier(currentToken, currentToken.Lexeme);
        parameters.Add(param);
    }

    if (!ExpectedToken(TokenType.RPAREN))
    {
        return null;
    }

    return parameters;
}

public BlockStatement ParseBlock()
{
    BlockStatement block = new BlockStatement(currentToken);
    AdvanceTokens();

    block.Statements = new List<Statement>();

    while (currentToken.Type != TokenType.RBRACE && currentToken.Type != TokenType.EOF)
    {
        Statement stmt = ParseStatement();
        if (stmt != null)
        {
            block.Statements.Add(stmt);
        }
        AdvanceTokens();
    }

    return block;
}

public FunctionExpression ParseFunction()
{
    FunctionExpression function = new FunctionExpression(currentToken);
    if (!ExpectedToken(TokenType.LPAREN))
    {
        return null;
    }

    function.Parameters = ParseFunctionParameters();

    if (!ExpectedToken(TokenType.LBRACE))
    {
        return null;
    }

    function.Body = ParseBlock();

    return function;
}

    public class Identifier
{
    public Token Token { get; }
    public string Value { get; }

    public Identifier(Token token, string value)
    {
        Token = token;
        Value = value;
    }
}

    public class IdentifierExpression : Expression
{
    public string Value { get; }

    public IdentifierExpression(Token token, string value)
        : base(token)
    {
        Value = value;
    }
}

    public IfExpression ParseIf()
{
    if (!ExpectedToken(TokenType.LPAREN))
    {
        return null;
    }

    AdvanceTokens();

    Expression condition = ParseExpression(Precedence.LOWEST);

    if (!ExpectedToken(TokenType.RPAREN))
    {
        return null;
    }

    if (!ExpectedToken(TokenType.LBRACE))
    {
        return null;
    }

    BlockStatement consequence = ParseBlock();

    BlockStatement alternative = null;
    if (peekToken.Type == TokenType.ELSE)
    {
        AdvanceTokens();

        if (!ExpectedToken(TokenType.LBRACE))
        {
            return null;
        }

        alternative = ParseBlock();
    }

    return new IfExpression(currentToken, condition, consequence, alternative);
}

    public InfixExpression ParseInfixExpression(Expression left)
{
    string operatorLexeme = currentToken.Lexeme;
    Precedence precedence = CurrentPrecedence();

    AdvanceTokens();

    Expression right = ParseExpression(precedence);

    return new InfixExpression(currentToken, operatorLexeme, left, right);
}

    public IntegerExpression ParseInteger()
{
    int parsedValue;
    if (!int.TryParse(currentToken.Lexeme, out parsedValue))
    {
        string message = $"Unable to parse {currentToken.Lexeme} as an integer.";
        Errors.Add(message);
        return null;
    }

    return new IntegerExpression(currentToken, parsedValue);
}

    public Statement ParseStatement()
{
    switch (currentToken.Type)
    {
        case TokenType.LET:
            return ParseLetStatement();
        case TokenType.RETURN:
            return ParseReturnStatement();
        default:
            // All other cases, including if, while, and expression statements
            ExpressionStatement expressionStatement = new ExpressionStatement(currentToken);
            expressionStatement.ExpressionValue = ParseExpression(Precedence.LOWEST);

            if (peekToken.Type == TokenType.SEMICOLON)
            {
                AdvanceTokens();
            }

            return expressionStatement;
    }
}

    public LetStatement ParseLetStatement()
{
    if (!ExpectedToken(TokenType.Identifier))
    {
        return null;
    }

    var name = ParseExpression(Precedence.LOWEST);

    if (!ExpectedToken(TokenType.ASSING))
    {
        return null;
    }

    AdvanceTokens();

    var value = ParseExpression(Precedence.LOWEST);

    if (peekToken.Type == TokenType.SEMICOLON)
    {
        AdvanceTokens();
    }

    return new LetStatement(name, value, currentToken);
}

    public PrefixExpression ParsePrefixExpression()
{
    PrefixExpression prefix = new PrefixExpression(currentToken, currentToken.Lexeme, null);

    AdvanceTokens();

    prefix.Right = ParseExpression(Precedence.PREFIX);

    return prefix;
}

    public ReturnStatement ParseReturnStatement()
{
    ReturnStatement returnStatement = new ReturnStatement(currentToken);

    AdvanceTokens();

    Expression returnValue = ParseExpression(Precedence.LOWEST);

    if (returnValue != null)
    {
        returnStatement.SetReturnValue(returnValue);
    }
    else
    {
        return null;
    }

    if (peekToken.Type == TokenType.SEMICOLON)
    {
        AdvanceTokens();
    }

    return returnStatement;
}

    public Expression ParseString()
    {
        return new StringExpression(currentToken, currentToken.Lexeme);
    }

    public WhileExpression ParseWhileStatement()
{
    if (!ExpectedToken(TokenType.LPAREN))
    {
        return null;
    }

    AdvanceTokens();

    Expression condition = ParseExpression(Precedence.LOWEST);

    if (!ExpectedToken(TokenType.RPAREN))
    {
        return null;
    }

    if (!ExpectedToken(TokenType.LBRACE))
    {
        return null;
    }

    BlockStatement body = ParseBlock();

    return new WhileExpression(currentToken, condition, body);
}

    public void PeekError(TokenType tokenType)
    {
        string message = $"Expected next token to be {tokenType}, but got {peekToken.Type} instead.";
        Errors.Add(message);
    }

    public Precedence PeekPrecedence()
    {
        try
        {
            return PRECEDENCES[peekToken.Type];
        }
        catch (KeyNotFoundException)
        {
            return Precedence.LOWEST;
        }
    }

    public void Parse()
    {
        while (currentToken.Type != TokenType.EOF)
        {
            if (currentToken.Type == TokenType.Identifier)
            {
                Console.WriteLine($"Se encontró Identifier: {currentToken.Lexeme}");
            }
            else if (currentToken.Type == TokenType.IntegerLiteral)
            {
                Console.WriteLine($"Se encontró Integer : {currentToken.Lexeme}");
            }
            else if (currentToken.Type == TokenType.And)
            {
                Console.WriteLine($"Se encontró AND operator: {currentToken.Lexeme}");
            }
            else if (currentToken.Type == TokenType.Equals)
            {
                Console.WriteLine($"Se encontró Equals operator: {currentToken.Lexeme}");
            }
            else if (currentToken.Type == TokenType.LT)
            {
                Console.WriteLine($"Se encontró Less-than operator: {currentToken.Lexeme}");
            }
            else if (currentToken.Type == TokenType.GT)
            {
                Console.WriteLine($"Se encontró Greater-than operator: {currentToken.Lexeme}");
            }
            else if (currentToken.Type == TokenType.LPAREN)
            {
                Console.WriteLine($"Se encontró Left parenthesis: {currentToken.Lexeme}");
            }
            else if (currentToken.Type == TokenType.RPAREN)
            {
                Console.WriteLine($"Se encontró Right parenthesis: {currentToken.Lexeme}");
            }
            else if (currentToken.Type == TokenType.LBRACE)
            {
                Console.WriteLine($"Se encontró Left brace: {currentToken.Lexeme}");
            }
            else if (currentToken.Type == TokenType.RBRACE)
            {
                Console.WriteLine($"Se encontró Right brace: {currentToken.Lexeme}");
            }
            else if (currentToken.Type == TokenType.SEMICOLON)
            {
                Console.WriteLine($"Se encontró Semicolon: {currentToken.Lexeme}");
            }
            else if (currentToken.Type == TokenType.PLUS)
            {
                Console.WriteLine($"Se encontró Plus operator: {currentToken.Lexeme}");
            }
            else if (currentToken.Type == TokenType.NEGATION)
            {
                Console.WriteLine($"Se encontró Negation operator: {currentToken.Lexeme}");
            }
            else if (currentToken.Type == TokenType.LTE)
            {
                Console.WriteLine($"Se encontró Less-than-Equals operator: {currentToken.Lexeme}");
            }
            else if (currentToken.Type == TokenType.GTE)
            {
                Console.WriteLine($"Se encontró Greater-than-Equals operator: {currentToken.Lexeme}");
            }
            else if (currentToken.Type == TokenType.ASSING)
            {
                Console.WriteLine($"Se encontró Assignment operator: {currentToken.Lexeme}");
            }
            else if (currentToken.Type == TokenType.COMMA)
            {
                Console.WriteLine($"Se encontró Comma: {currentToken.Lexeme}");
            }
            else if (currentToken.Type == TokenType.MINUS)
            {
                Console.WriteLine($"Se encontró Minus operator: {currentToken.Lexeme}");
            }
            else if (currentToken.Type==TokenType.IF)
            {
                Console.WriteLine($"Se encontró If statement: {currentToken.Lexeme}");
            }
            else if (currentToken.Type==TokenType.ELSE)
            {
                Console.WriteLine($"Se encontró Else statement: {currentToken.Lexeme}");
            }
            else if (currentToken.Type==TokenType.TRUE)
            {
                Console.WriteLine($"Se encontró True statement: {currentToken.Lexeme}");
            }
            else if (currentToken.Type==TokenType.RETURN)
            {
                Console.WriteLine($"Se encontró Return statement: {currentToken.Lexeme}");
            }
            else if (currentToken.Type==TokenType.FUNCTION)
            {
                Console.WriteLine($"Se encontró Function statement: {currentToken.Lexeme}");
            }
            else if (currentToken.Type==TokenType.FALSE)
            {
                Console.WriteLine($"Se encontró False statement: {currentToken.Lexeme}");
            }
            else if (currentToken.Type==TokenType.SLASH)
            {
                Console.WriteLine($"Se encontró Slash statement: {currentToken.Lexeme}");
            }
            else if (currentToken.Type==TokenType.MULTIPLICATION)
            {
                Console.WriteLine($"Se encontró Multiplication statement: {currentToken.Lexeme}");
            }
            else
            {
                Console.WriteLine($"No se encontro ningun token (illegal token) : {currentToken.Lexeme} ");
            }
            currentToken = lexer.NextToken();
        }
    }
}