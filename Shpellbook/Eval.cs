﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
            throw new NotImplementedException();
        }
    }
}