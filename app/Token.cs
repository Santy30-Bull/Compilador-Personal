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
    Illegal,
    LT,
    GT,
    LPAREN,
    RPAREN,
    RBRACE,
    LBRACE,
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