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
    SLASH,
    IF,
    MULTIPLICATION,
    ELSE,
    LET,
    WHILE,
    CALL,
    WORD,
    STRING
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


    //Esta son las palabras reservadas, podemos agregar mas si queremos 
    public static TokenType lookup_token_type(string literal)
    {
        var keywords = new Dictionary<string, TokenType>
        {
            {"szs", TokenType.TRUE},
            {"nel", TokenType.FALSE},
            {"procedimiento", TokenType.FUNCTION},
            {"regresa", TokenType.RETURN},
            {"si", TokenType.IF},
            {"sino", TokenType.ELSE},
            {"variable", TokenType.LET},
        };

        if (keywords.ContainsKey(literal))
        {
            return keywords[literal];
        }

        return TokenType.Identifier;
    }
}

//otra forma de hacer el metodo de lookup_token_type, sin tener que meterlo dentro de la clase token
/*
public class TokenHelper
{
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
*/

