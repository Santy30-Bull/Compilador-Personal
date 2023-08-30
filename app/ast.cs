using System;
using System.Collections.Generic;

public abstract class ASTNode
{
    public abstract string TokenLiteral();
    public abstract override string ToString();
}

public class Statement : ASTNode
{
    private Token token;

    public Statement(Token token)
    {
        this.token = token;
    }

    public override string TokenLiteral()
    {
        return token.Lexeme;  // Corregido: usar Lexeme en lugar de Literal
    }

    public override string ToString()
    {
        return TokenLiteral();
    }
}

public class Expression : ASTNode
{
    private Token token;

    public Expression(Token token)
    {
        this.token = token;
    }

    public override string TokenLiteral()
    {
        return token.Lexeme;  // Corregido: usar Lexeme en lugar de Literal
    }

    public override string ToString()
    {
        return TokenLiteral();
    }
}

public class Progra : ASTNode
{
    private List<Statement> statements;

    public Progra(List<Statement> statements)
    {
        this.statements = statements;
    }

    public override string TokenLiteral()
    {
        if (statements.Count > 0)
        {
            return statements[0].TokenLiteral();
        }
        return "";
    }

    public override string ToString()
    {
        List<string> outList = new List<string>();
        foreach (Statement statement in statements)
        {
            outList.Add(statement.ToString());
        }
        return string.Join("", outList);
    }
}