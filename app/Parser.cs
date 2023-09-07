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
                Console.WriteLine($"Found Identifier: {currentToken.Lexeme}");
            }
            else if (currentToken.Type == TokenType.IntegerLiteral)
            {
                Console.WriteLine($"Found Integer : {currentToken.Lexeme}");
            }
            else if (currentToken.Type == TokenType.And)
            {
                Console.WriteLine($"Found AND operator: {currentToken.Lexeme}");
            }
            else if (currentToken.Type == TokenType.Equals)
            {
                Console.WriteLine($"Found equals operator: {currentToken.Lexeme}");
            }
            else if (currentToken.Type == TokenType.LT)
            {
                Console.WriteLine($"Found less-than operator: {currentToken.Lexeme}");
            }
            else if (currentToken.Type == TokenType.GT)
            {
                Console.WriteLine($"Found greater-than operator: {currentToken.Lexeme}");
            }
            else if (currentToken.Type == TokenType.LPAREN)
            {
                Console.WriteLine($"Found left parenthesis: {currentToken.Lexeme}");
            }
            else if (currentToken.Type == TokenType.RPAREN)
            {
                Console.WriteLine($"Found right parenthesis: {currentToken.Lexeme}");
            }
            else if (currentToken.Type == TokenType.LBRACE)
            {
                Console.WriteLine($"Found left brace: {currentToken.Lexeme}");
            }
            else if (currentToken.Type == TokenType.RBRACE)
            {
                Console.WriteLine($"Found right brace: {currentToken.Lexeme}");
            }
            else if (currentToken.Type == TokenType.SEMICOLON)
            {
                Console.WriteLine($"Found semicolon: {currentToken.Lexeme}");
            }
            else if (currentToken.Type == TokenType.PLUS)
            {
                Console.WriteLine($"Found plus operator: {currentToken.Lexeme}");
            }
            else if (currentToken.Type == TokenType.NEGATION)
            {
                Console.WriteLine($"Found Negation operator: {currentToken.Lexeme}");
            }
            else if (currentToken.Type == TokenType.LTE)
            {
                Console.WriteLine($"Found less-than-equals operator: {currentToken.Lexeme}");
            }
            else if (currentToken.Type == TokenType.GTE)
            {
                Console.WriteLine($"Found greater-than-equals operator: {currentToken.Lexeme}");
            }
            else if (currentToken.Type == TokenType.ASSING)
            {
                Console.WriteLine($"Found assignment operator: {currentToken.Lexeme}");
            }
            else if (currentToken.Type == TokenType.COMMA)
            {
                Console.WriteLine($"Found comma: {currentToken.Lexeme}");
            }
            else if (currentToken.Type == TokenType.MINUS)
            {
                Console.WriteLine($"Found minus operator: {currentToken.Lexeme}");
            }
            else if (currentToken.Type==TokenType.IF)
            {
                Console.WriteLine($"Found if statement: {currentToken.Lexeme}");
            }
            else if (currentToken.Type==TokenType.ELSE)
            {
                Console.WriteLine($"Found else statement: {currentToken.Lexeme}");
            }
            else if (currentToken.Type==TokenType.TRUE)
            {
                Console.WriteLine($"Found true statement: {currentToken.Lexeme}");
            }
            else if (currentToken.Type==TokenType.RETURN)
            {
                Console.WriteLine($"Found return statement: {currentToken.Lexeme}");
            }
            else if (currentToken.Type==TokenType.FUNCTION)
            {
                Console.WriteLine($"Found function statement: {currentToken.Lexeme}");
            }
            else if (currentToken.Type==TokenType.FALSE)
            {
                Console.WriteLine($"Found false statement: {currentToken.Lexeme}");
            }
            else if (currentToken.Type==TokenType.SLASH)
            {
                Console.WriteLine($"Found slash statement: {currentToken.Lexeme}");
            }
            else if (currentToken.Type==TokenType.MULTIPLICATION)
            {
                Console.WriteLine($"Found multiplication statement: {currentToken.Lexeme}");
            }
            else
            {
                Console.WriteLine($"Found illegal token: {currentToken.Lexeme} ");
            }

            currentToken = lexer.NextToken();
        }
    }
}