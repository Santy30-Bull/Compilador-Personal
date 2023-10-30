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
        var tokens = expresion.Split(' ');

        double resultado = 0;
        double operando = 0;
        string operador = "+";

        foreach (var token in tokens)
        {
            if (double.TryParse(token, out double valor))
            {
                operando = valor;

                if (operador == "+")
                    resultado += operando;
                else if (operador == "-")
                    resultado -= operando;
                else if (operador == "*")
                    resultado *= operando;
                else if (operador == "/")
                {
                    if (operando != 0)
                        resultado /= operando;
                    else
                        throw new DivideByZeroException("División por cero.");
                }
            }
            else if (token == "+")
                operador = "+";
            else if (token == "-")
                operador = "-";
            else if (token == "*")
                operador = "*";
            else if (token == "/")
                operador = "/";
            else if (token == "x")
            {
                operando = x;

                if (operador == "+")
                    resultado += operando;
                else if (operador == "-")
                    resultado -= operando;
                else if (operador == "*")
                    resultado *= operando;
                else if (operador == "/")
                {
                    if (operando != 0)
                        resultado /= operando;
                    else
                        throw new DivideByZeroException("División por cero.");
                }
            }
        }
        return resultado;
    }
    catch (Exception e)
    {
        throw new Exception($"Error al evaluar la expresión: {e.Message}");
    }
}

 
static double EvaluarTokens(string[] tokens, double x)
    {
        double resultado = 0;
        string operador = "+";
        foreach (var token in tokens)
        {
            if (double.TryParse(token, out double valor))
            {
                if (operador == "+")
                    resultado += valor;
                else if (operador == "-")
                    resultado -= valor;
            }
            else if (token == "+")
            {
                operador = "+";
            }
            else if (token == "-")
            {
                operador = "-";
            }
            else if (token == "x")
            {
                resultado += x;
            }
        }
        return resultado;
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
