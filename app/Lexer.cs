using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

public class Lexer
{
    private readonly string input;
    private int position;

    public Lexer(string input)
    {
        this.input = input;
        position = 0;
    }

    private char CurrentChar => position < input.Length ? input[position] : '\0';

    public Token NextToken()
    {
        if (position >= input.Length)
            return new Token(TokenType.EOF, "");

        char currentChar = CurrentChar;

        if (char.IsDigit(currentChar))
        {
            string num = "";

            while (char.IsDigit(currentChar))
            {
                num += currentChar;
                Advance();
                currentChar = CurrentChar;
            }

            return new Token(TokenType.IntegerLiteral, num);
        }
        else if (currentChar == 'Y')
        {
            Advance();
            return new Token(TokenType.And, "Y");
        }
        else if (currentChar == '=')
        {
            Advance();
            return new Token(TokenType.Equals, "=");
        }
        else if (char.IsLetter(currentChar))
        {
            string identifier = "";

            while (char.IsLetter(currentChar))
            {
                identifier += currentChar;
                Advance();
                currentChar = CurrentChar;
            }

            return new Token(TokenType.Identifier, identifier);
        }
        else
        {
            throw new Exception("Unexpected character: " + currentChar);
        }
    }

    private void Advance()
    {
        position++;
    }
}