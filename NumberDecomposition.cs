using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PrvyProgram
{
    class Ctecka
    {
        public static int ReadSymbol()
        {
            int previous = '?';
            int symbol = Console.Read();
            while ((symbol < '0') || (symbol > '9'))
            {
                previous = symbol;
                symbol = Console.Read();
            }
            int x = 0;

            while ((symbol >= '0') && (symbol <= '9'))
            {
                x = 10 * x + (symbol - '0');
                symbol = Console.Read();
            }
            if (previous == '-')
                x = -x;
            return x;
        }
    }
    class prime_numbers
    {
        public static List<int> erosthenovo_sito(int x)
        {
            List<int> prime_numbers = new List<int>();
            bool[] is_prime = new bool[x + 1];
            for (int i = 0; i < is_prime.Length; i++)
            {
                is_prime[i] = true;
            }

            for (int i = 2; i < x; i++)
            {
                if (is_prime[i] == true)
                {
                    prime_numbers.Add(i);

                }
                for (int j = (int)Math.Pow(i, 2); j < x; j += i)
                {
                    is_prime[j] = false;
                }
            }
            prime_numbers.Reverse();
            return prime_numbers;
        }
    }
    internal class Program
    {
        public static List<int> decomposition(int x)
        {
            List<int> dividers = new List<int>();
            List<int> primes_1 = prime_numbers.erosthenovo_sito(x);
            int[] primes = primes_1.ToArray();

            while (x != 1)
            {
                for (int i = 0; i < primes.Length; i++)
                {

                    if (x % primes[i] == 0)
                    {
                        dividers.Add(primes[i]);
                        x = x / primes[i];

                    }
                }
            }
            dividers.Reverse();
            return dividers;
        }

        static void Main(string[] args)

        {
            int x = Ctecka.ReadSymbol();
            Console.WriteLine(x+"="+string.Join('*',decomposition(x)));
        }
    }
    
}