using System;
using System.Collections.Generic;
using System.Text;

namespace CleanCodePrinciples
{
    public class LettersGenerator
    {
        public void CharGenerator()
        {
            Random random = new Random();
            int ascii = random.Next(65, 91);
            char letter = Convert.ToChar(ascii);
            string vowels = "AEIOU";
            
            bool IsVowel = vowels.Contains(letter);
            
            if(IsVowel)
                Console.WriteLine($"This letter is vowel {letter}");
            else
                Console.WriteLine($"This letter is not vowel {letter}");
        }
    }
}
