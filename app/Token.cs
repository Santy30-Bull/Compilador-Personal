using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public enum TokenType
{
    Identifier,
    IntegerLiteral,
    Equals,
    And,
    EOF
}

public class Token
{
    public TokenType Type { get; }
    public string Lexeme { get; }

    public Token(TokenType type, string lexeme)
    {
        Type = type;
        Lexeme = lexeme;
    }
}