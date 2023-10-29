using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

class MainClass
{
    static void Main(string[] args)
    {
        Console.WriteLine("Bienvenido al analizador léxico");

        while (true)
        {
            Console.Write(">> ");
            string input = Console.ReadLine() ?? ""; // Si Console.ReadLine() devuelve null, asigna una cadena vacía

            if (string.IsNullOrWhiteSpace(input))
                break;

            Lexer lexer = new Lexer(input);
            List<Token> tokens = lexer.Tokenize(); // Tokenize the input

            Parser parser = new Parser(tokens);

            try
            {
                double result = parser.Parse(); // Parse the tokens and get the result
                if(result == 1){
                    Console.WriteLine("Resultado: " + true);
                }
                else if(result == 0){
                    Console.WriteLine("Resultado: " + false);
                }else{
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