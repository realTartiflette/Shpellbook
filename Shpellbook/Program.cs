using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Shpellbook
{
    public class Program
    {
        public static void Run(TextReader input, bool isConsole)
        {
            Parser parser = new Parser(input);
            while (true)
            {
                if (isConsole)
                    Console.Write("Shpellbook$ ");
                
                Command command = parser.ParseInput();
                
                if (command == null)
                    break;

                if (command.args.Length > 0 && command.args[0] != "")
                {
                    //Task<int> code = Eval.EvaluateBuiltin(command);
                    int code = Eval.Evaluate(command);
                    Eval.UpdateJobs();
                    if (code == -1)
                        Console.WriteLine("Program is running in background");
                    else
                        Console.WriteLine("Command ended with value {0}", code);
                }
                
            }
        }

        public static void Main(string[] args)
        {
            if (args.Length == 0)
                Run(Console.In, true);
            else
            {
                foreach (var arg in args)
                {
                    if (!File.Exists(arg))
                        Console.Error.WriteLine("The path {0} does not exists or is not a file", arg);
                    else
                    {
                        TextReader action = new StringReader(arg);
                        Run(action, false);
                    }
                }
            }
        }
    }
}