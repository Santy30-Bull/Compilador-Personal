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

    public List<Token> Tokenize()
    {
        List<Token> tokens = new List<Token>();

        while (position < input.Length)
        {
            char currentChar = input[position];

            if (char.IsDigit(currentChar))
            {
                tokens.Add(ScanNumber());
            }
            else if (currentChar == '+')
            {
                tokens.Add(new Token(TokenType.PLUS, currentChar.ToString()));
                position++;
            }
            else if (currentChar == '-')
            {
                tokens.Add(new Token(TokenType.MINUS, currentChar.ToString()));
                position++;
            }
            else if (currentChar == '*')
            {
                tokens.Add(new Token(TokenType.MULTIPLICATION, currentChar.ToString()));
                position++;
            }
            else if (currentChar == '/')
            {
                tokens.Add(new Token(TokenType.SLASH, currentChar.ToString()));
                position++;
            }
            else if (currentChar == '(')
            {
                tokens.Add(new Token(TokenType.LPAREN, currentChar.ToString()));
                position++;
            }
            else if (currentChar == ')')
            {
                tokens.Add(new Token(TokenType.RPAREN, currentChar.ToString()));
                position++;
            }
            else if (CurrentChar == '=')
            {

                tokens.Add(new Token(TokenType.Equals, currentChar.ToString()));
                position++;

            }
            else if (CurrentChar == '!')
            {

                tokens.Add(new Token(TokenType.NEGATION, currentChar.ToString()));
                position++;

            }
            
            else if (CurrentChar == '>')
            {
                if (input[position + 1] == '=')
                {
                    tokens.Add(new Token(TokenType.GTE, currentChar.ToString() + input[position + 1].ToString()));
                    position++;
                    position++;
                }
                else
                {
                    tokens.Add(new Token(TokenType.GT, currentChar.ToString()));
                    position++;
                }
            }
            else if (CurrentChar == '<')
            {
                if (input[position + 1] == '=')
                {
                    tokens.Add(new Token(TokenType.LTE, currentChar.ToString() + input[position + 1].ToString()));
                    position++;
                    position++;
                }
                else
                {
                    tokens.Add(new Token(TokenType.LT, currentChar.ToString()));
                    position++;
                }
            }
            else if (char.IsWhiteSpace(currentChar))
            {
                position++;
            }
            else
            {
                throw new Exception($"Invalid character: {currentChar}");
            }
        }

        return tokens;
    }

    private Token ScanNumber()
    {
        int start = position;
        while (position < input.Length && (char.IsDigit(input[position]) || input[position] == '.'))
        {
            position++;
        }

        string numberText = input.Substring(start, position - start);
        if (double.TryParse(numberText, out double numberValue))
        {
            return new Token(TokenType.IntegerLiteral, numberText);
        }
        else
        {
            throw new Exception($"Invalid number: {numberText}");
        }
    }

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
            string literal = ReadIdentifier(); // Leer el identificador completo
            TokenType tokenType = lookup_token_type(literal); // Comprobar si es una palabra reservada
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
    int start = position;
    
    while (EsLetra(CurrentChar) || char.IsDigit(CurrentChar))
    {
        Advance();
    }
    
    return input.Substring(start, position - start);
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