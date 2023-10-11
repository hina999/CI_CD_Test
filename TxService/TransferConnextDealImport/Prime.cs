/* 27 Oct 2014
// This code released to community under The Code Project Open License (CPOL) 1.02
// The copyright owner and author of this version of the code is Robert Ellis, except as otherwise stated
// Please retain this notice and clearly identify your own edits, amendments and/or contributions
// In line with the CPOL this code is provided "AS IS" and without warranty
// Use entirely at your own risk
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// The original routine for calculation of Primes was sourced here:
// http://stackoverflow.com/questions/13001578/i-need-a-slow-c-sharp-function
// With my thanks to, and acknowledgement of, the original author of this code

namespace PrimeCalcTimerService
{
    internal static class Prime
    {
        // Adjust these ranges if the simulated work is too fast or slow 
        //  in your local environment
        private const int SmallRangeMin = 75000;
        private const int SmallRangeMax = 100000;
        private const int LargeRangeMin = 150000;
        private const int LargeRangeMax = 200000;

        /// <summary>
        /// Calculate the Nth prime number where N is a random number between A and B
        /// </summary>
        /// <param name="N"></param>
        /// <param name="prime"></param>
        private static void GetNthRandomPrime
            (Random r, int A, int B, out int N, out long prime)
        {
            N = r.Next(A, B);

            int count = 0;
            long c = 2;

            while (count < N)
            {
                long b = 2;
                int  p = 1; // to check if found a prime

                while (b * b <= c)
                {
                    if (c % b == 0)
                    {
                        p = 0;
                        break;
                    }
                    b++;
                }
                if (p > 0)
                    count++;
                c++;
            }

            prime = --c;
        }

        internal static void GetNthRandomPrimeSmall
            (Random r, out int N, out long prime)
        {
            GetNthRandomPrime(r, SmallRangeMin, SmallRangeMax, out N, out prime);
        }

        internal static void GetNthRandomPrimeLarge
            (Random r, out int N, out long prime)
        {
            GetNthRandomPrime(r, LargeRangeMin, LargeRangeMax, out N, out prime);
        }
    }
}
