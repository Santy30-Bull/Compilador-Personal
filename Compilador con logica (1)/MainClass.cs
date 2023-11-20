using System;
using System.Collections.Generic;

class MainClass
{
    static Dictionary<string, Func<double, double, double, double>> funciones = new Dictionary<string, Func<double, double, double, double>>();
    static HashSet<string> constantes = new HashSet<string>();


    static void Main(string[] args)
    {
        Console.WriteLine("Bienvenido al compilador.");

        while (true)
        {
            Console.Write(">> ");
            string entrada = Console.ReadLine();

            if (entrada == "salir")
                break;
            else if (entrada.Contains("="))
            {
                string[] partes = entrada.Split('=');
                if (partes.Length == 2)
                {
                    string nombre = partes[0].Trim();
                    string expresion = partes[1].Trim();
                    funciones[nombre] = (x, y, z) => EvaluarExpresion(expresion, x, y, z);
                    Console.WriteLine($"Función '{nombre}' asignada.");
                }
                else
                {
                    Console.WriteLine("Entrada no válida. Use 'nombre = expresión' para asignar una función.");
                }
            }
            else
            {
                try
                {
                    double resultado = EvaluarExpresionConArgumento(entrada);
                    Console.WriteLine($"Resultado: {resultado}");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error al evaluar la función: {e.Message}");
                }
            }
        }

        Console.WriteLine("Saliendo del programa.");
    }
static double EvaluarExpresion(string expresion, double x, double y, double z)
{
    try
    {
        var tokens = Tokenize(expresion);

        Stack<double> numeros = new Stack<double>();
        Stack<string> operadores = new Stack<string>();

        foreach (var token in tokens)
        {
            if (double.TryParse(token, out double valor))
            {
                numeros.Push(valor);
            }
            else if (EsOperador(token))
            {
                while (operadores.Count > 0 && Prioridad(operadores.Peek()) >= Prioridad(token))
                {
                    RealizarOperacion(numeros, operadores);
                }
                operadores.Push(token);
            }
            else if (token == "(")
            {
                operadores.Push(token);
            }
            else if (token == ")")
            {
                while (operadores.Count > 0 && operadores.Peek() != "(")
                {
                    RealizarOperacion(numeros, operadores);
                }
                if (operadores.Count > 0 && operadores.Peek() == "(")
                {
                    operadores.Pop(); // Retira el paréntesis izquierdo
                }
                else
                {
                    throw new Exception("Expresión no válida. Paréntesis desequilibrados.");
                }
            }
            else if (token == "x")
            {
                numeros.Push(x);
            }
            else if (token == "y")
            {
                numeros.Push(y);
            }
            else if (token == "z")
            {
                numeros.Push(z);
            }
        }

        while (operadores.Count > 0)
        {
            RealizarOperacion(numeros, operadores);
        }

        if (numeros.Count == 1)
        {
            return numeros.Pop();
        }
        else
        {
            throw new Exception("Expresión no válida. No se evaluó completamente.");
        }
    }
    catch (Exception e)
    {
        throw new Exception($"Error al evaluar la expresión: {e.Message}");
    }
}


    static List<string> Tokenize(string expresion)
    {
        var tokens = new List<string>();
        string actual = "";

        foreach (var caracter in expresion)
        {
            if (EsOperador(caracter.ToString()) || caracter == '(' || caracter == ')')
            {
                if (!string.IsNullOrEmpty(actual))
                {
                    tokens.Add(actual);
                    actual = "";
                }
                tokens.Add(caracter.ToString());
            }
            else
            {
                actual += caracter;
            }
        }

        if (!string.IsNullOrEmpty(actual))
        {
            tokens.Add(actual);
        }

        return tokens;
    }

    static bool EsOperador(string token)
    {
        return token == "+" || token == "-" || token == "*" || token == "/" || token == "^";
    }

    static int Prioridad(string operador)
    {
        switch (operador)
        {
            case "+":
            case "-":
                return 1;
            case "*":
            case "/":
                return 2;
            case "^":
                return 3;
            default:
                return 0;
        }
    }

    static void RealizarOperacion(Stack<double> numeros, Stack<string> operadores)
    {
        double b = numeros.Pop();
        double a = numeros.Pop();
        string operador = operadores.Pop();

        switch (operador)
        {
            case "+":
                numeros.Push(a + b);
                break;
            case "-":
                numeros.Push(a - b);
                break;
            case "*":
                numeros.Push(a * b);
                break;
            case "/":
                if (b != 0)
                    numeros.Push(a / b);
                else
                    throw new DivideByZeroException("División por cero.");
                break;
            case "^":
                numeros.Push(Math.Pow(a, b));
                break;
        }
    }
    static double EvaluarExpresionConArgumento(string entrada)
    {
        var partes = entrada.Split('(', ')');
        if (partes.Length != 3)
            throw new ArgumentException("Formato de entrada no válido.");

        string nombre = partes[0].Trim();
        string[] argumentos = partes[1].Split(',');

        double arg1 = 0.0, arg2 = 0.0, arg3 = 0.0;

        if (argumentos.Length != 3)
        {
            if (argumentos.Length == 1)
            {
                arg1 = double.Parse(argumentos[0]);
            }
            else if (argumentos.Length == 2)
            {
                arg1 = double.Parse(argumentos[0]);
                arg2 = double.Parse(argumentos[1]);
            }
        }
        else
        {
            arg1 = double.Parse(argumentos[0]);
            arg2 = double.Parse(argumentos[1]);
            arg3 = double.Parse(argumentos[2]);
        }

        if (funciones.ContainsKey(nombre))
        {
            return funciones[nombre](arg1, arg2, arg3);
        }
        else
        {
            if (constantes.Contains(nombre))
            {
                throw new ArgumentException($"La variable '{nombre}' es una constante y no se puede evaluar como función.");
            }
            else
            {
                throw new ArgumentException($"La función o variable '{nombre}' no está definida.");
            }
        }
}
}
