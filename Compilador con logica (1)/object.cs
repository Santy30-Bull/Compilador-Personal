using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


public enum ObjectType
{
    BOOLEAN,
    INTEGER,
    NULL
}

public abstract class Object
{
    public abstract ObjectType Type();
    public abstract string Inspect();
}

public class Integer : Object
{
    public int value;

    public Integer(int value)
    {
        this.value = value;
    }

    public override ObjectType Type()
    {
        return ObjectType.INTEGER;
    }

    public override string Inspect()
    {
        return value.ToString();
    }
}

public class Boolean : Object
{
    private bool value;

    public Boolean(bool value)
    {
        this.value = value;
    }

    public override ObjectType Type()
    {
        return ObjectType.BOOLEAN;
    }

    public override string Inspect()
    {
        return value ? "verdadero" : "falso";
    }
}

public class Null : Object
{
    public override ObjectType Type()
    {
        return ObjectType.NULL;
    }

    public override string Inspect()
    {
        return "nulo";
    }
}
//hello