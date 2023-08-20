using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            Console.Write(">> ");
            string input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
                break;

            Lexer lexer = new Lexer(input);
            Parser parser = new Parser(lexer);

            try
            {
                parser.Parse();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }
    }
}