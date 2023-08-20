using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                Console.WriteLine($"Found identifier: {currentToken.Lexeme}");
            }
            else if (currentToken.Type == TokenType.IntegerLiteral)
            {
                Console.WriteLine($"Found integer literal: {currentToken.Lexeme}");
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
            else
            {
                Console.WriteLine($"Found illegal token: {currentToken.Lexeme} ");
            }

            currentToken = lexer.NextToken();
        }
    }
}