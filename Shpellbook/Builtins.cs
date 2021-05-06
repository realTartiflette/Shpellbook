using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Shpellbook
{
    public class Builtins
    {
        public static int Sleep(string[] args)
        {
            if (args.Length == 1)
            {
                Console.Error.WriteLine("sleep: Not enough args");
                return 1;
            }

            if (!int.TryParse(args[1], out var time))
            {
                Console.Error.WriteLine("sleep: argument should be a positive integer");
                return 1;
            }

            // Convert seconds into milliseconds
            Console.WriteLine($"Sleeping for {time} seconds.");
            Thread.Sleep(time * 1000);
            Console.WriteLine("Sleep finished");
            return 0;
        }

        private static bool IsHidden(string file)
        {
            if (file.Length > 0 && file[0] != '.')
                return false;

            return true;
        }

        private static int LsDir(string dir)
        {
            var files = new List<string>();

            foreach (var file in Directory.GetFileSystemEntries(dir))
            {
                var fileName = Path.GetFileName(file);
                if (!IsHidden(fileName))
                    files.Add(fileName);
            }

            if (files.Count > 0)
                Console.Write(Path.GetFileName(files[0]));
            for (var i = 1; i < files.Count; i++)
                Console.Write(" " + Path.GetFileName(files[i]));

            Console.WriteLine();

            return 0;
        }


        public static int Ls(string[] args)
        {
            if (args.Length != 1)
                for (var i = 1; i < args.Length; i++)
                {
                    var file = args[i];
                    if (Directory.Exists(file))
                    {
                        Console.WriteLine("{0}:", file);
                        LsDir(file);
                    }
                    else if (File.Exists(file))
                    {
                        Console.WriteLine(file);
                    }
                    else
                    {
                        Console.Error.WriteLine("ls: cannot access '"
                                                + file + "': No such file or directory");
                        return 1;
                    }
                }
            else
                LsDir(Directory.GetCurrentDirectory());

            return 0;
        }
    }
}