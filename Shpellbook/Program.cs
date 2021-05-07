using System;
using System.IO;
using System.Threading;

namespace Shpellbook
{
    public class Program
    {
        public static void Run(TextReader input, bool isConsole)
        {
            Console.Write("Shpellbook$ ");
            //bool infinity = true;
            while (isConsole)
            {
                Parser parser = new Parser(input);
                Command command = parser.ParseInput();
                
                
                if (command == null)
                    break;

                if (command.args.Length > 0)
                {
                    int code = Eval.Evaluate(command);
                    Console.WriteLine("Command ended with value {0}", code);
                }
                Console.Write("Shpellbook$ ");
                string userCommand = Console.ReadLine();
                input = new StringReader(userCommand);
            }
        }

        public static void Main(string[] args)
        {
            Run(Console.In, true);
        }
    }
}