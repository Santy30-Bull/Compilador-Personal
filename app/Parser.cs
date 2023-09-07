using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Token;
using static TokenType;

public class Parser
{
    private readonly Lexer lexer;
    private Token currentToken;

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
                Console.WriteLine($"found if statement: {currentToken.Lexeme}");
            }
            else if (currentToken.Type==TokenType.ELSE)
            {
                Console.WriteLine($"found else statement: {currentToken.Lexeme}");
            }
            else if (currentToken.Type==TokenType.TRUE)
            {
                Console.WriteLine($"found true statement: {currentToken.Lexeme}");
            }
            else if (currentToken.Type==TokenType.RETURN)
            {
                Console.WriteLine($"found return statement: {currentToken.Lexeme}");
            }
            else if (currentToken.Type==TokenType.FUNCTION)
            {
                Console.WriteLine($"found function statement: {currentToken.Lexeme}");
            }
            else if (currentToken.Type==TokenType.FALSE)
            {
                Console.WriteLine($"found false statement: {currentToken.Lexeme}");
            }
            else if (currentToken.Type==TokenType.SLASH)
            {
                Console.WriteLine($"found slash statement: {currentToken.Lexeme}");
            }
            else if (currentToken.Type==TokenType.MULTIPLICATION)
            {
                Console.WriteLine($"found multiplication statement: {currentToken.Lexeme}");
            }
            else
            {
                Console.WriteLine($"Found illegal token: {currentToken.Lexeme} ");
            }

            currentToken = lexer.NextToken();
        }
    }
}