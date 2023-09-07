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
    private char _character;
    private int _readPosition;


 public Lexer(string input)
{
    this.input = input;
    position = 0;
    _character = '\0';      // Inicializar _character
    _readPosition = 0;      // Inicializar _readPosition
    ReadCharacter();        // Llamar al método ReadCharacter para leer el primer carácter
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
        else if (currentChar == '/')
        {
            Advance();
            return new Token(TokenType.SLASH, "/");
        }
        else if (currentChar == '*')
        {
            Advance();
            return new Token(TokenType.MULTIPLICATION, "*");
        }
        else if (EsNumero(_character))
        {
            string literal = ReadNumber();
            return new Token(TokenType.IntegerLiteral, literal);
        }
        
        //aqui se hacen las palabras reservadas
        else if (EsLetra(_character))
        {
            string literal = ReadIdentifier();
            if (literal.Length == 3 && literal[0] == 'n' && literal[1] == 'e' && literal[2] == 'l')
            {
                return new Token(TokenType.FALSE, "nel");
            }
            else if (literal.Length == 3 && literal[0] == 's' && literal[1] == 'z' && literal[2] == 's')
            {
                return new Token(TokenType.TRUE, "szs");
            }
            else if (literal.Length == 2 && literal[0] == 's' && literal[1] == 'i')
            {
                return new Token(TokenType.IF, "si");
            }
            else if (literal.Length == 4 && literal[0] == 's' && literal[1] == 'i' && literal[2] == 'n' && literal[3] == 'o')
            {
                return new Token(TokenType.ELSE, "sino");
            }
            else if (literal.Length == 7 && literal[0] == 'r' && literal[1] == 'e' && literal[2] == 'g' && literal[3] == 'r' && literal[4] == 'e' && literal[5] == 's' && literal[6] == 'a')
            {
                return new Token(TokenType.RETURN, "regresa");
            }
            else if (literal.Length == 13 && literal[0] == 'p' && literal[1] == 'r' && literal[2] == 'o' && literal[3] == 'c' && literal[4] == 'e' && literal[5] == 'd' && literal[6] == 'i' && literal[7] == 'm' && literal[8] == 'i' && literal[9] == 'e' && literal[10] == 'n' && literal[11] == 't' && literal[12] == 'o')
            {
                return new Token(TokenType.FUNCTION, "procedimiento");
            }
            TokenType tokenType = lookup_token_type(literal);
            return new Token(tokenType, literal);
        }
        else
        {
            Advance();
            return new Token(TokenType.Illegal, currentChar.ToString());
        }
        
    }

    private bool EsLetra(char character)
    {
        return (character >= 'a' && character <= 'z') || (character >= 'A' && character <= 'Z');
    }

    private bool EsNumero(char character)
    {
        return char.IsDigit(character);
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

    private void ReadCharacter()
    {
        if (_readPosition >= input.Length)
        {
            _character = '\0'; // Fin de archivo
        }
        else
        {
            _character = input[_readPosition];
        }

        position = _readPosition;
        _readPosition++;
    }

    private string ReadIdentifier()
    {
        int initialPosition = position;

        while (EsLetra(_character))
        {
            ReadCharacter();
        }

        return input.Substring(initialPosition, position - initialPosition);
    }

    private string ReadNumber()
    {
        int initialPosition = position;

        while (EsNumero(_character))
        {
            ReadCharacter();
        }

        return input.Substring(initialPosition, position - initialPosition);
    }

    private Token MakeTwoCharacterToken(TokenType tokenType)
    {
        char prefix = _character;
        ReadCharacter();
        char suffix = _character;
        Token token = new Token(tokenType, prefix.ToString() + suffix.ToString());
        return token;
    }
}