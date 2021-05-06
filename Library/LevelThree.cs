using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Library
{
    public class LevelThree
    {
        /// <summary>
        ///     Generates all primes numbers up to n included
        /// </summary>
        public static List<int> UsualPrimesGenerator(int n)
        {
            if (n < 0)
                return new List<int>();
            
            var numbers = Enumerable.Range(2, n).ToList();
            numbers.RemoveAll(x =>
            {
                for (var i = 2; i < x; i++)
                    if (x % i == 0 && x != i)
                        return true;

                return false;
            });

            return numbers;
        }


        /// <summary>
        ///     Remove integers divisible by n in the list
        /// </summary>
        /// <param name="n">the base multiple</param>
        /// <param name="primes">the list to remove from</param>
        public static void RemoveNotPrimes(int n, List<int> primes)
        {
            for (int i = 0; i < primes.Count; i++)
            {
                if (primes[i] % n == 0)
                    primes.Remove(primes[i]);
            }
        }

        public static Func<List<int>> RemoveNotPrimesWithStart(int start, int end, List<int> primes)
        {
            primes.RemoveAll(x =>
            {
                for (var i = start; i < end; i++)
                    if (x % i == 0 && x != i)
                        return true;

                return false;
            });

            return primes;
        }
        
        
        public static List<int> MagicPrimesGenerator(int n, int nbTasks)
        {
            if (nbTasks <= 0)
                throw new ArgumentException("nbTasks < 0");
            
            
            int start = 2;
            int end = n;
            Task[] arrTasks = new Task[nbTasks];
            for (int i = 0; i < nbTasks; i++)
            {
                var numbers = Enumerable.Range(start, end).ToList();
                Task<List<int>> task = new Task<List<int>>(RemoveNotPrimesWithStart(start, end, numbers));
                arrTasks[i] = task;
                task.Start();
                start = end + 1;
                end = ;
            }
            
        }
    }
}