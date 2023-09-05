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
    SEMICOLON,
    PLUS,
    NEGATION,
    LTE,
    GTE,
    ASSING,
    COMMA,
    MINUS,
    EOF,
    TRUE,
    FALSE,
    FUNCTION,
    RETURN,
    IF,
    ELSE,
    LET
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

    public static TokenType lookup_token_type(string literal)
{
    var keywords = new Dictionary<string, TokenType>
    {
        {"verdadero", TokenType.TRUE},
        {"falso", TokenType.FALSE},
        {"procedimiento", TokenType.FUNCTION},
        {"regresa", TokenType.RETURN},
        {"si", TokenType.IF},
        {"si_no", TokenType.ELSE},
        {"variable", TokenType.LET},
    };

    if (keywords.ContainsKey(literal))
    {
        return keywords[literal];
    }

    return TokenType.Identifier;
}
}

