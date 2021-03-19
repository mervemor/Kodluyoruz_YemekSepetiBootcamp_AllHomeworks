using System;
using System.Collections.Generic;
using System.Text;

namespace CleanCodePrinciples
{
    public class NumberGenerator
    {
        public void Generator()
        {
            Random random = new Random();
            int number = random.Next(1, 100);
            bool numberRange = number < 50;

            if (numberRange)
                Console.WriteLine($"Randomly generated number less than 50 : {number}");
            else
                Console.WriteLine($"Randomly generated number more than 50 : {number}");
        }
    }
}
