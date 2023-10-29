using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;
using static Boolean;
using static Integer;
using static Null;
using static Object;
using ast;
using static ObjectType;
public class Constants
{
    public static readonly bool TRUE = true;
    public static readonly bool FALSE = false;
    public static readonly string NULL = "null";
}

public class Evaluator
{
    public static object EvaluateBangOperatorExpression(object right)
    {
        if (Constants.TRUE.Equals(right))
        {
            return Constants.FALSE;
        }
        else if (Constants.FALSE.Equals(right))
        {
            return Constants.TRUE;
        }
        else if (right == null)
        {
            return Constants.TRUE;
        }
        else
        {
            return Constants.FALSE;
        }
    }
    public static object ToBooleanObject(bool value)
    {
        return value ? Constants.TRUE : Constants.FALSE;
    }
public static object? EvaluateMinusOperatorExpression(object right)
{
    if (!(right is Integer))
    {
        return null;
    }

    Integer integerRight = (Integer)right;

    return new Integer(-integerRight?.value ?? 0); // Usamos 0 como valor predeterminado
}

public static object? EvaluatePrefixExpression(string operador, object right)
{
    if (operador == "!")
    {
        return EvaluateBangOperatorExpression(right);
    }
    else if (operador == "-")
    {
        return EvaluateMinusOperatorExpression(right);
    }
    else
    {
        return Constants.NULL;
    }
}

public static object EvaluateIntegerInfixExpression(string operador, object left, object right)
{
    int leftValue = ((Integer)left).value;
    int rightValue = ((Integer)right).value;

    switch (operador)
    {
        case "+":
            return new Integer(leftValue + rightValue);
        case "-":
            return new Integer(leftValue - rightValue);
        case "*":
            return new Integer(leftValue * rightValue);
        case "/":
            return new Integer(leftValue / rightValue);
        case "<":
            return ToBooleanObject(leftValue < rightValue);
        case "<=":
            return ToBooleanObject(leftValue <= rightValue);
        case ">":
            return ToBooleanObject(leftValue > rightValue);
        case ">=":
            return ToBooleanObject(leftValue >= rightValue);
        case "==":
            return ToBooleanObject(leftValue == rightValue);
        case "!=":
            return ToBooleanObject(leftValue != rightValue);
        default:
            return Constants.NULL;
    }
}

public static object EvaluateInfixExpression(string operador, object left, object right)
{
    if (left is Integer && right is Integer)
    {
        return EvaluateIntegerInfixExpression(operador, left, right);
    }
    else if (operador == "==")
    {
        return ToBooleanObject(ReferenceEquals(left, right));
    }
    else if (operador == "!=")
    {
        return ToBooleanObject(!ReferenceEquals(left, right));
    }
    return Constants.NULL;
}
public static object? Evaluate(ast.ASTNode node)
    {
        Type nodeType = node.GetType();

        if (nodeType == typeof(ast.Program))
        {
            ast.Program programNode = (ast.Program)node;
            return EvaluateStatements(programNode.Statements);
        }
        else if (nodeType == typeof(ast.ExpressionStatement))
        {
            ast.ExpressionStatement expressionStatementNode = (ast.ExpressionStatement)node;
            Contract.Assert(expressionStatementNode.Expression != null);
            return Evaluate(expressionStatementNode.Expression);
        }
        else if (nodeType == typeof(ast.Inte))
        {
            ast.Inte integerNode = (ast.Inte)node;
            Contract.Assert(integerNode.Value != null);
            return new Integer((int)integerNode.Value);
        }
        else if (nodeType == typeof(ast.Bool))
        {
            ast.Bool booleanNode = (ast.Bool)node;
            Contract.Assert(booleanNode.Value != null);
            return ToBooleanObject((bool)booleanNode.Value);
        }
        else if (nodeType == typeof(ast.Prefix))
        {
            ast.Prefix prefixNode = (ast.Prefix)node;
            Contract.Assert(prefixNode.Right != null);
            object? right = Evaluate(prefixNode.Right);
            Contract.Assert(right != null);
            return EvaluatePrefixExpression(prefixNode.Operator, right);
        }
        else if (nodeType == typeof(ast.Infix))
        {
            ast.Infix infixNode = (ast.Infix)node;
            Contract.Assert(infixNode.Left != null && infixNode.Right != null);
            object? left = Evaluate(infixNode.Left);
            object? right = Evaluate(infixNode.Right);
            Contract.Assert(left != null && right != null);
            return EvaluateInfixExpression(infixNode.Operator, left, right);
        }
        else if (nodeType == typeof(ast.Block))
        {
            ast.Block blockNode = (ast.Block)node;
            return EvaluateStatements(blockNode.Statements);
        }
        else if (nodeType == typeof(ast.If))
        {
            ast.If ifNode = (ast.If)node;
            return EvaluateIfExpression(ifNode);
        }
        return null;
    }

public static object? EvaluateStatements(List<ast.Statement> statements)
{
    object? result = null;
    foreach (var statement in statements)
    {
        result = Evaluate(statement);
    }
    return result;
}
public static bool IsTruthy(object obj)
{
    if (object.Equals(obj, Constants.NULL))
    {
        return false;
    }
    else if (object.Equals(obj, Constants.TRUE))
    {
        return true;
    }
    else if (object.Equals(obj, Constants.FALSE))
    {
        return false;
    }
    return true;
}
public static object? EvaluateIfExpression(ast.If ifExpression)
{
    if (ifExpression != null)
    {
        object? condition = Evaluate(ifExpression.Condition);
        if (condition != null && IsTruthy(condition))
        {
            if (ifExpression.Consequence != null)
            {
                return Evaluate(ifExpression.Consequence);
            }
        }
        else if (ifExpression.Alternative != null)
        {
            return Evaluate(ifExpression.Alternative);
        }
    }
    return Constants.NULL;
}
}
