using System;
using System.Collections.Generic;

namespace Library
{
    public class Program
    {
        public static void Main(string[] args)
        {
            List<int> list = LevelThree.UsualPrimesGenerator(20);
            foreach (var l in list)
            {
                Console.Write(l + " - ");
            }
            LevelThree.RemoveNotPrimes(5, list);
            Console.WriteLine();
            foreach (var l in list)
            {
                Console.Write(l + " - ");
            }
            
        }
    }
}