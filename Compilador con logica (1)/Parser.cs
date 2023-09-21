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
    RETURN = 9,
    INFIX = 10,
    INTEGER = 11,
    IF = 12
}

public class Parser
{
    private readonly Lexer lexer;
    private /*readonly*/ List<Token> tokens;
    private int currentTokenIndex = 0;

    private List<int> logicTokensIndex = new List<int>();

    public Parser(List<Token> tokens)
    {
        this.tokens = tokens;
    }

    public double Parse()
    {
        foreach (Token token in tokens)
        {
            if (token.Type == TokenType.Equals ||   //=
              //token.Type == TokenType.ASSING ||   //==
                token.Type == TokenType.GT ||       //>
                token.Type == TokenType.LT ||       //<
                token.Type == TokenType.GTE ||      //>=
                token.Type == TokenType.LTE ||        //<=
                token.Type == TokenType.NEGATION     //!
                ) 
            { 
                logicTokensIndex.Add(tokens.IndexOf(token));
            }
        }
        if(logicTokensIndex.Count != 0)
        {
            if (logicTokensIndex.Count > 1) throw new ArgumentException("More than 1 logic operator is not allowed");
            List<Token> tempTokens = tokens;

            tokens = tempTokens.Take(logicTokensIndex[0]).ToList();

            double left = ParseExpression();

            tokens = tempTokens.Skip(logicTokensIndex[0]+1).ToList();

            currentTokenIndex = 0;

            double right = ParseExpression();

            tokens = tempTokens;

            return logicOperator(tokens[logicTokensIndex[0]], left, right) ? 1 : 0;
        }
        return ParseExpression();
    }

    public void ParseOther()
    {
        var token = tokens[currentTokenIndex];
        while (token.Type != TokenType.EOF)
        {
            if (token.Type == TokenType.Identifier)
            {
                Console.WriteLine($"Found identifier: {token.Lexeme}");
            }
            else if (token.Type == TokenType.IntegerLiteral)
            {
                Console.WriteLine($"Found integer literal: {token.Lexeme}");
            }
            else if (token.Type == TokenType.And)
            {
                Console.WriteLine($"Found AND operator: {token.Lexeme}");
            }
            else if (token.Type == TokenType.Equals)
            {
                Console.WriteLine($"Found equals operator: {token.Lexeme}");
            }
            else if (token.Type == TokenType.LT)
            {
                Console.WriteLine($"Found less-than operator: {token.Lexeme}");
            }
            else if (token.Type == TokenType.GT)
            {
                Console.WriteLine($"Found greater-than operator: {token.Lexeme}");
            }
            else if (token.Type == TokenType.LPAREN)
            {
                Console.WriteLine($"Found left parenthesis: {token.Lexeme}");
            }
            else if (token.Type == TokenType.RPAREN)
            {
                Console.WriteLine($"Found right parenthesis: {token.Lexeme}");
            }
            else if (token.Type == TokenType.LBRACE)
            {
                Console.WriteLine($"Found left brace: {token.Lexeme}");
            }
            else if (token.Type == TokenType.RBRACE)
            {
                Console.WriteLine($"Found right brace: {token.Lexeme}");
            }
            else if (token.Type == TokenType.SEMICOLON)
            {
                Console.WriteLine($"Found semicolon: {token.Lexeme}");
            }
            else if (token.Type == TokenType.PLUS)
            {
                Console.WriteLine($"Found plus operator: {token.Lexeme}");
            }
            else if (token.Type == TokenType.NEGATION)
            {
                Console.WriteLine($"Found Negation operator: {token.Lexeme}");
            }
            else if (token.Type == TokenType.LTE)
            {
                Console.WriteLine($"Found less-than-equals operator: {token.Lexeme}");
            }
            else if (token.Type == TokenType.GTE)
            {
                Console.WriteLine($"Found greater-than-equals operator: {token.Lexeme}");
            }
            else if (token.Type == TokenType.ASSING)
            {
                Console.WriteLine($"Found assignment operator: {token.Lexeme}");
            }
            else if (token.Type == TokenType.COMMA)
            {
                Console.WriteLine($"Found comma: {token.Lexeme}");
            }
            else if (token.Type == TokenType.MINUS)
            {
                Console.WriteLine($"Found minus operator: {token.Lexeme}");
            }
            else
            {
                Console.WriteLine($"Found illegal token: {token.Lexeme} ");
            }

            token = lexer.NextToken();
        }
    }

    private double ParseExpression()
    {
        double left = ParseTerm();

        while (currentTokenIndex < tokens.Count)
        {
            var token = tokens[currentTokenIndex];
            if (token.Type == TokenType.PLUS)
            {
                currentTokenIndex++;
                left += ParseTerm();
            }
            else if (token.Type == TokenType.MINUS)
            {
                currentTokenIndex++;
                left -= ParseTerm();
            }
            else
            {
                break;
            }
        }

        return left;
    }

    private double ParseTerm()
    {
        double left = ParseFactor();

        while (currentTokenIndex < tokens.Count)
        {
            var token = tokens[currentTokenIndex];
            if (token.Type == TokenType.MULTIPLICATION)
            {
                currentTokenIndex++;
                left *= ParseFactor();
            }
            else if (token.Type == TokenType.SLASH)
            {
                currentTokenIndex++;
                double right = ParseFactor();
                if (right == 0)
                {
                    throw new DivideByZeroException("Division by zero is not allowed.");
                }
                left /= right;
            }
            else
            {
                break;
            }
        }

        return left;
    }

    private double ParseFactor()
    {
        var token = tokens[currentTokenIndex++];
        if (token.Type == TokenType.IntegerLiteral)
        {
            if (double.TryParse(token.Lexeme, out double result))
            {
                return result;
            }
            else
            {
                throw new ArgumentException($"Invalid number: {token.Lexeme}");
            }
        }
        else if (token.Type == TokenType.LPAREN)
        {
            double expressionValue = ParseExpression();
            if (currentTokenIndex < tokens.Count && tokens[currentTokenIndex].Type == TokenType.RPAREN)
            {
                currentTokenIndex++;
                return expressionValue;
            }
            else
            {
                throw new ArgumentException("Missing closing parenthesis.");
            }
        }
        else
        {
            throw new ArgumentException($"Unexpected token: {token.Lexeme}");
        }
    }

    private bool logicOperator(Token logicOperator, double left, double right)
    {
        bool result = false;

        if (logicOperator.Type == TokenType.Equals) result = left == right;
        else if (logicOperator.Type == TokenType.GT) result = left > right;
        else if (logicOperator.Type == TokenType.LT) result = left < right;
        else if (logicOperator.Type == TokenType.GTE) result = left >= right;
        else if (logicOperator.Type == TokenType.LTE) result = left <= right;
        else if (logicOperator.Type == TokenType.NEGATION) result = left !=right;
        else throw new ArgumentException("Logic token detected but not found");

        return result;
    }
}