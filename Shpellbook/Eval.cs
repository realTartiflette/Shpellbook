using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Shpellbook
{
    public class Eval
    {
        public static Dictionary<string, Func<string[], int>> builtins =
            new Dictionary<string, Func<string[], int>>
            {
                {"ls", Builtins.Ls},
                {"sleep", Builtins.Sleep},
                {"pwd", Builtins.Pwd},
                {"cd", Builtins.Cd},
                {"echo", Builtins.Echo},
                {"cat", Builtins.Cat},
                {"exit", Builtins.Exit},
            };

        public static List<Task<int>> jobs = new List<Task<int>>();

        /// <summary>
        ///     Launch a Process and wait for it in a Task
        /// </summary>
        /// <param name="command">
        ///     The command object to evaluate
        /// </param>
        /// <returns>
        ///     The return code of the process
        /// </returns>
        public static Task<int> EvaluatePath(Command command)
        {
            return Task.Run(() =>
            {
                using var process = new Process();
                var processInfo = new ProcessStartInfo(command.args[0]);

                for (var i = 1; i < command.args.Length; i++)
                    processInfo.ArgumentList.Add(command.args[i]);

                process.StartInfo = processInfo;

                try
                {
                    process.Start();
                    process.WaitForExit();
                }

                // This exception also works on Unix
                // Guess they are doing a good job here :)
                catch (Win32Exception)
                {
                    return 127;
                }

                return process.ExitCode;
            });
        }

        /// <summary>
        ///     Launching the result gave by the parser
        /// </summary>
        /// <param name="command">
        ///     The command object returned by the parser
        /// </param>
        /// <returns>
        ///     Return the integer the execution of the command returned
        /// </returns>
        public static int Evaluate(Command command)
        {
            Task<int> builtinTask = EvaluateBuiltin(command);
            BackgroundEvent backgroundEvent = new BackgroundEvent();
            Task[] tasks = {builtinTask, backgroundEvent.task};
            Task.WaitAny(tasks);
            //builtinTask.RunSynchronously(); 
            //return task.Result;
            
            //backgroundEvent.task.RunSynchronously();

            if (builtinTask.IsCompleted)
                return builtinTask.Result;
            jobs.Add(builtinTask);
            return -1;
        }

        public static Task<int> EvaluateBuiltin(Command command)
        {
            Task<int> res = null;
            foreach (var cmd in builtins)
            {
                if (cmd.Key == command.args[0])
                {
                    res = new Task<int>(() => cmd.Value(command.args));
                    res.Start();
                    break;
                }
            }

            return res;
        }

        public static void UpdateJobs()
        {
            for (int i = 0; i < jobs.Count; i++)
            {
                if (jobs[i].IsCompleted)
                {
                    Console.WriteLine("Job number {0} terminated with code {1}", i, jobs[i].Result);
                    jobs.RemoveAt(i);
                }
            }
        }

        /*public static void Run(TextReader input, bool is_console)
        {
            Parser parser = new Parser(input);
            while (is_console)
            {
                Evaluate()
                UpdateJobs();
            }
        }*/
    }
}