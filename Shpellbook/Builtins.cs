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

        public static int Pwd(string[] args)
        {
            if (args.Length > 1)
            {
                Console.Error.WriteLine("pwd: too many arguments");
                return 1;
            }
            Console.WriteLine(Directory.GetCurrentDirectory());
            return 0;
        }

        public static int Cd(string[] args)
        {
            if (args.Length > 2)
            {
                Console.Error.WriteLine("cd: to many arguments");
                return 1;
            }

            string currentDir = Directory.GetCurrentDirectory();
            if (args.Length == 1)
            {
                Directory.SetCurrentDirectory("/home/" + Environment.UserName);
                return 0;
            }
            else
            {
                if (Directory.Exists(args[1]))
                {
                    Directory.SetCurrentDirectory(currentDir + "/" + args[1]);
                    return 0;
                }

                if (args[1] == "..")
                {
                    Directory.SetCurrentDirectory(Directory.GetParent(currentDir).ToString());
                    return 0;
                }

                if (File.Exists(args[1]))
                {
                    Console.Error.WriteLine("cd: {0}: Not a directory", currentDir);
                    return 1;
                }
                Console.Error.WriteLine("cd: {0}: No such file or directory", currentDir);
                return 1;
            }
        }
        
        public static int Echo(string[] args)
        {
            for (int i = 1; i < args.Length; i++)
            {
                Console.Write(args[i] + " ");
            }
            Console.WriteLine();
            return 0;
        }

        public static int Cat(string[] args)
        {
            if (args.Length > 1)
            {
                for (int i = 1; i < args.Length; i++)
                {
                    if (!File.Exists(args[i]))
                    {
                        if (Directory.Exists(args[1]))
                        {
                            Console.Error.WriteLine("cat: {0}: Is a directory", Directory.GetCurrentDirectory());
                            return 1;
                        }
                        Console.Error.WriteLine("cat: {0}: No such file or directory", Directory.GetCurrentDirectory());
                        return 1;
                    }
                    StreamReader streamReader = new StreamReader(args[i]);
                    string line;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        Console.WriteLine(line);
                    }
                }
            }

            if (args.Length == 0)
            {
                Console.Error.WriteLine("cat: {0}: No such file or directory", Directory.GetCurrentDirectory());
                return 1;
            }

            return 0;
        }

        public static int Exit(string[] args)
        {
            if (args.Length > 2)
            {
                Console.Error.WriteLine("exit: too many arguements");
                return 1;
            }

            if (args.Length == 1)
            {
                Environment.Exit(0);
            }
            int n;
            if (Int32.TryParse(args[1], out n))
            {
                if (n >= 0)
                    Environment.Exit(n);
            }
            Console.Error.WriteLine("First argument of exit must be a positive integer");
            return 1;
        }
    }
}