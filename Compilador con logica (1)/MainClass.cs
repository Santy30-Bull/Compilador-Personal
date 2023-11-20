using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

class MainClass
{
    static Dictionary<string, Func<double, double, double, double>> funciones = new Dictionary<string, Func<double, double, double, double>>();
    static Dictionary<string, double> variables = new Dictionary<string, double>();

    static void Main(string[] args)
    {
        Console.WriteLine("Bienvenido al compilador.");

        while (true)
        {
            Console.Write(">> ");
            string entrada = Console.ReadLine()?.Trim() ?? "";
            if (entrada == "salir")
            {
                break;
            }
            else if (entrada == "opcion 2")
            {
                while (true)
                {
                    string input = Console.ReadLine() ?? ""; // Si Console.ReadLine() devuelve null, asigna una cadena vacía
                    if (string.IsNullOrWhiteSpace(input))
                        break;
                    else
                    {
                        Lexer lexer = new Lexer(input);
                        List<Token> tokens = lexer.Tokenize(); // Tokenize the input

                        Parser parser = new Parser(tokens);

                        try
                        {
                            double result = parser.Parse(); // Parse the tokens and get the result
                            if (result == 1)
                            {
                                Console.WriteLine("Resultado: " + true);
                            }
                            else if (result == 0)
                            {
                                Console.WriteLine("Resultado: " + false);
                            }
                            else
                            {
                                Console.WriteLine("Resultado: " + result);
                            }

                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Error: " + e.Message);
                        }
                    }
                }
            }

            else if (entrada.Contains("="))
            {
                string[] partes = entrada.Split('=');
                if (partes.Length == 2)
                {
                    string nombre = partes[0].Trim();
                    string expresion = partes[1].Trim();

                    try
                    {
                        double valor = EvaluarExpresion(expresion);
                        if (variables.ContainsKey(nombre))
                        {
                            variables[nombre] = valor;
                            Console.WriteLine($"Variable '{nombre}' ha sido asignada como valor {valor}.");
                        }
                        else
                        {
                            variables.Add(nombre, valor);
                            Console.WriteLine($"Variable '{nombre}' ha sido asignada como valor {valor}.");
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error al evaluar la expresión: {e.Message}");
                    }
                }
            }
            else
            {
                try
                {
                    double valor = EvaluarExpresion(entrada);
                    variables.Add(entrada, valor);
                    Console.WriteLine($"Resultado: {valor}");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error: {e.Message}");
                }
            }
        }

        static bool EsConstante(string expresion)
        {
            return double.TryParse(expresion, out _);
        }

        static double EvaluarExpresion(string expresion, double? x = null, double? y = null, double? z = null)
        {
            if (EsConstante(expresion))
            {
                // If it's a constant, simply return its value
                return double.Parse(expresion);
            }
            else
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
                                operadores.Pop(); // Remove the left parenthesis
                            }
                            else
                            {
                                throw new Exception("Invalid expression. Unbalanced parentheses.");
                            }
                        }
                        else
                        {
                            if (!variables.ContainsKey(token))
                            {
                                Console.Write($"Enter value for variable '{token}': ");
                                double variableValue = double.Parse(Console.ReadLine());
                                variables.Add(token, variableValue);
                            }
                            numeros.Push(variables[token]);
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
                    throw new Exception($"Error evaluating expression: {e.Message}");
                }
            }
        }


        static List<string> Tokenize(string expresion)
        {
            var tokens = new List<string>();
            string actual = "";

            foreach (var caracter in expresion)
            {
                if (caracter == ' ')
                {
                    continue; // Ignorar espacios en blanco
                }
                else if (EsOperador(caracter.ToString()) || caracter == '(' || caracter == ')')
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
    }
}
