using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using static Token;
using static TokenType;

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
        saltarEspacioBlanco();

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
            if (CurrentChar == '=')
            {
                Advance();
                return new Token(TokenType.ASSING, "==");
            }
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
        else if (currentChar == '<')
        {
            Advance();
            if (CurrentChar == '=')
            {
                Advance();
                return new Token(TokenType.LTE, "<=");
            }
            return new Token(TokenType.LT, "<");
        } 
        else if (currentChar == '>')
        {
            Advance();
            if (CurrentChar == '=')
            {
                Advance();
                return new Token(TokenType.GTE, ">=");
            }
            return new Token(TokenType.GT, ">");
        }
        else if (currentChar == '(')
        {
            Advance();
            return new Token(TokenType.LPAREN, "(");
        }
        else if (currentChar == ')')
        {
            Advance();
            return new Token(TokenType.RPAREN, ")");
        }
        else if (currentChar == '{')
        {
            Advance();
            return new Token(TokenType.LBRACE, "{");
        }
        else if (currentChar == '}')
        {
            Advance();
            return new Token(TokenType.RBRACE, "}");
        }
        else if (currentChar == ';')
        {
            Advance();
            return new Token(TokenType.SEMICOLON, ";");
        }
        else if (currentChar == '+')
        {
            Advance();
            return new Token(TokenType.PLUS, "+");
        }
        else if (currentChar == '!')
        {
            Advance();
            return new Token(TokenType.NEGATION, "!");
        }
        else if (currentChar == ',')
        {
            Advance();
            return new Token(TokenType.COMMA, ",");
        }
        else if (currentChar == '-')
        {
            Advance();
            return new Token(TokenType.MINUS, "-");
        }
        else
        {
            Advance();
            return new Token(TokenType.Illegal, currentChar.ToString());
        }
    }

    private void Advance()
    {
        position++;
    }
    public void saltarEspacioBlanco()
    {
        while (CurrentChar == ' ' || CurrentChar == '\t' || CurrentChar == '\n' || CurrentChar == '\r')
            Advance();
    }
    public string escogerCaracter()
    {
        char currentChar = CurrentChar;
        string caracter = currentChar.ToString();
        return caracter;
    }
    
    public Token juntarCaracteres(TokenType tipo)
    {
        string caracter = escogerCaracter();
        string caracter2 = CurrentChar.ToString();
        string juntos = caracter + caracter2;
        Advance();
        return new Token(tipo, juntos);
    } 
}