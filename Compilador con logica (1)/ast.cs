using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using static Token;
using static TokenType;

namespace ast{
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

public class IfExpression : Expression
{
    public If IfValue { get; }

    public IfExpression(Token token, If ifExpression) : base(token)
    {
        IfValue = ifExpression;
    }
}

public class Program : ASTNode
{
    public List<Statement> Statements { get; }

    public Program(List<Statement> statements)
    {
        Statements = statements;
    }

    public override string TokenLiteral()
    {
        if (Statements.Count > 0)
        {
            return Statements[0].TokenLiteral();
        }

        return "";
    }
//hello
    public override string ToString()
    {
        var output = new List<string>();
        foreach (var statement in Statements)
        {
            output.Add(statement.ToString());
        }

        return string.Join("", output);
    }
}

public class Identifier : Expression
{
    public string Value { get; }

    public Identifier(Token token, string value) : base(token)
    {
        Value = value;
    }

    public override string ToString()
    {
        return Value;
    }
}

public class LetStatement : Statement
{
    public Identifier Name { get; }
    public Expression Value { get; }

    public LetStatement(Token token, Identifier name, Expression value)
        : base(token)
    {
        Name = name;
        Value = value;
    }

    public override string ToString()
    {
        return $"{TokenLiteral()} {Name} = {Value};";
    }
}

public class ReturnStatement : Statement
{
    public Expression? ReturnValue { get; } // Usa Expression? para permitir nulos

    public ReturnStatement(Token token, Expression? returnValue = null) // Marca como parámetro opcional
        : base(token)
    {
        ReturnValue = returnValue;
    }

    public override string ToString()
    {
        string returnValueStr = ReturnValue != null ? ReturnValue.ToString() : "";
        return $"{TokenLiteral()} {returnValueStr};";
    }
}

public class ExpressionStatement : Statement
{
    public Expression? Expression { get; }

    public ExpressionStatement(Token token, Expression? expression = null)
        : base(token)
    {
        Expression = expression;
    }

    public override string ToString()
    {
        return Expression?.ToString() ?? "";
    }
}

public class Inte : Expression
{
    public int? Value { get; }

    public Inte(Token token, int? value = null)
        : base(token)
    {
        Value = value;
    }

    public override string ToString()
    {
        return Value.HasValue ? Value.Value.ToString() : "";
    }
}

public class Prefix : Expression
{
    public string Operator { get; }
    public Expression? Right { get; }

    public Prefix(Token token, string @operator, Expression? right = null)
        : base(token)
    {
        Operator = @operator;
        Right = right;
    }

    public override string ToString()
    {
        return $"({Operator}{(Right != null ? Right.ToString() : "")})";
    }
}

public class Infix : Expression
{
    public Expression Left { get; }
    public string Operator { get; }
    public Expression? Right { get; }

    public Infix(Token token, Expression left, string @operator, Expression? right = null)
        : base(token)
    {
        Left = left;
        Operator = @operator;
        Right = right;
    }

    public override string ToString()
    {
        return $"({Left} {Operator} {Right?.ToString() ?? ""})";
    }
}

public class Bool : Expression
{
    public bool? Value { get; }

    public Bool(Token token, bool? value = null)
        : base(token)
    {
        Value = value;
    }

    public override string ToString()
    {
        return Value.HasValue ? Value.Value.ToString() : "";
    }
}

public class Block : Statement
{
    public List<Statement> Statements { get; }

    public Block(Token token, List<Statement> statements)
        : base(token)
    {
        Statements = statements;
    }

    public override string ToString()
    {
        var output = new List<string>();
        foreach (var statement in Statements)
        {
            output.Add(statement.ToString());
        }

        return string.Join("", output);
    }
}

public class If : Statement
{
    public Expression Condition { get; }
    public Block Consequence { get; }
    public Block? Alternative { get; }

    public If(Token token, Expression condition, Block consequence, Block? alternative = null)
        : base(token)
    {
        Condition = condition;
        Consequence = consequence;
        Alternative = alternative;
    }

    public override string ToString()
    {
        string alternativeStr = Alternative != null ? $" {Alternative}" : "";
        return $"{TokenLiteral()} {Condition} {Consequence}{alternativeStr}";
    }
}

public class Function : Expression
{
    public List<Identifier> Parameters { get; }
    public Block? Body { get; }

    public Function(Token token, List<Identifier>? parameters = null, Block? body = null)
        : base(token)
    {
        Parameters = parameters ?? new List<Identifier>();
        Body = body;
    }

    public override string ToString()
    {
        string paramList = string.Join(", ", Parameters.Select(p => p.ToString()));
        return $"{TokenLiteral()}({paramList}) {Body}";
    }
}

public class Call : Expression
{
    public Expression Function { get; }
    public List<Expression> Arguments { get; }

    public Call(Token token, Expression function, List<Expression>? arguments = null)
        : base(token)
    {
        Function = function;
        Arguments = arguments ?? new List<Expression>();
    }

    public override string ToString()
    {
        string argList = string.Join(", ", Arguments.Select(a => a.ToString()));
        return $"{Function}({argList})";
    }
}
}







