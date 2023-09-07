using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//token
using static Token;
using static TokenType;
//ast
using static Program;
using static Statement; 
using static Expression;
using static ExpressionStatement;
using static Function;
using static Identifier;   
using static If;
using static Infix;
using static Inte;
using static LetStatement;
using static Prefix;
using static ReturnStatement;
using static Block;
using static Boolean;
using static Call;
//lexer
using static Lexer;

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
    CALL = 7
}

public class PrecedenceDemo
{
    // Definir el diccionario PRECEDENCES como una propiedad estática
    private static readonly Dictionary<TokenType, Precedence> PRECEDENCES = new Dictionary<TokenType, Precedence>
    {
        { TokenType.Equals, Precedence.EQUALS },
        { TokenType.ASSING, Precedence.EQUALS },
        { TokenType.LT, Precedence.LESSGREATER },
        { TokenType.GT, Precedence.LESSGREATER },
        { TokenType.PLUS, Precedence.SUM },
        { TokenType.MINUS, Precedence.SUM },
        { TokenType.SLASH, Precedence.PRODUCT },
        { TokenType.MULTIPLICATION, Precedence.PRODUCT },
        { TokenType.LPAREN, Precedence.CALL }
    };
}

public class Parser
{
    private Dictionary<TokenType, PrefixParseFn> PrefixParseFns = new Dictionary<TokenType, PrefixParseFn>();
    private Dictionary<TokenType, InfixParseFn> InfixParseFns = new Dictionary<TokenType, InfixParseFn>();
    private readonly Lexer lexer;
    private Token currentToken;

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
    }

    private void Consume(TokenType expectedType)
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