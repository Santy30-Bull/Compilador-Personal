using System;
using System.Collections.Generic;

class MainClass
{
    static Dictionary<string, Func<double, double>> funciones = new Dictionary<string, Func<double, double>>();

    static void Main(string[] args)
    {
        Console.WriteLine("Bienvenido al compilador mas sapa perra de este planeta.");

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
                    try
                    {
                        funciones[nombre] = x => EvaluarExpresion(expresion, x);
                        Console.WriteLine($"Función '{nombre}' asignada.");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error al asignar la función: {e.Message}");
                    }
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

    static double EvaluarExpresion(string expresion, double x)
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
                    operadores.Pop(); // Quitamos el paréntesis izquierdo
                }
                else if (token == "x")
                {
                    numeros.Push(x);
                }
            }

            while (operadores.Count > 0)
            {
                RealizarOperacion(numeros, operadores);
            }

            return numeros.Pop();
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
        return token == "+" || token == "-" || token == "*" || token == "/";
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
        }
    }

    static double EvaluarExpresionConArgumento(string entrada)
    {
        var partes = entrada.Split('(', ')');
        if (partes.Length != 3)
            throw new ArgumentException("Formato de entrada no válido.");

        string nombre = partes[0].Trim();
        double argumento = double.Parse(partes[1]);

        if (funciones.ContainsKey(nombre))
        {
            return funciones[nombre](argumento);
        }
        else
        {
            throw new ArgumentException($"La función '{nombre}' no está definida.");
        }
    }
}
